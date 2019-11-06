//////////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2019, National Instruments Corp.

// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:

// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NationalInstruments.Vision;
using NationalInstruments.Vision.Acquisition.Imaqdx;

namespace Grab
{
    public partial class Form1 : Form
    {
        private ImaqdxSession _session = null;
        private System.ComponentModel.BackgroundWorker acquisitionWorker;

        public Form1()
        {
            InitializeComponent();

            // Set up initial UI state
            startButton.Enabled = true;
            stopButton.Enabled = false;

            // Enumerate available cameras
            ImaqdxCameraInformation[] cameraList = ImaqdxSystem.GetCameraInformation(true);
            foreach (ImaqdxCameraInformation camInfo in cameraList)
            {
                cameraComboBox.Items.Add(camInfo.Name);
            }
            cameraComboBox.SelectedIndex = cameraList.Length > 0 ? 0 : -1;

            // Set up image viewer
            VisionImage image = new VisionImage();
            imageViewer.Attach(image);

            // Set up background acquisition worker
            acquisitionWorker = new System.ComponentModel.BackgroundWorker();
            acquisitionWorker.DoWork += new DoWorkEventHandler(acquisitionWorker_DoWork);
            acquisitionWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(acquisitionWorker_RunWorkerCompleted);
            acquisitionWorker.ProgressChanged += new ProgressChangedEventHandler(acquisitionWorker_ProgressChanged);
            acquisitionWorker.WorkerReportsProgress = true;
            acquisitionWorker.WorkerSupportsCancellation = true;
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Open a new session and configure a grab
                _session = new ImaqdxSession(cameraComboBox.Text);
                _session.ConfigureGrab();

                // Update UI state
                startButton.Enabled = false;
                stopButton.Enabled = true;

                // Start up background worker to acquire images
                acquisitionWorker.RunWorkerAsync();
            }
            catch (ImaqdxException ex)
            {
                MessageBox.Show(ex.Message, "NI-IMAQdx Error");
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            try
            {
                acquisitionWorker.CancelAsync();
                if (_session != null)
                {
                    _session.Close();
                    _session = null;
                }
                bufNumTextBox.Text = "";
            }
            catch (ImaqdxException ex)
            {
                MessageBox.Show(ex.Message, "NI-IMAQdx Error");
            }
        }

        void acquisitionWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            // Keep acquiring until we error or cancel
            while (!worker.CancellationPending)
            {
                try
                {
                    // Grab the next image
                    uint bufNum;
                    _session.Grab(imageViewer.Image, true, out bufNum);

                    // Report our buffer number acquired
                    worker.ReportProgress(0, bufNum);
                }
                catch (ImaqdxException exception)
                {
                    // If we have been stopped, ignore the exception since
                    // it is likely just an error indicating the acquisition has
                    // been stopped
                    if (!worker.CancellationPending)
                    {
                        e.Result = exception;
                    }
                    return;
                }
            }
        }

        void acquisitionWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Our acquired buffer number is in the user state to update
            // the display
            uint bufferNumber = (uint)e.UserState;
            bufNumTextBox.Text = bufferNumber.ToString();
        }

        void acquisitionWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result is ImaqdxException)
            {
                MessageBox.Show(((ImaqdxException)e.Result).ToString(), "NI-IMAQdx Error");
            }
            try
            {
                // Close the camera in case it hasn't already been closed
                if (_session != null)
                {
                    _session.Close();
                    _session = null;
                }
            }
            catch (ImaqdxException error)
            {
                MessageBox.Show(error.ToString(), "NI-IMAQdx Error");
            }

            // Update UI controls
            bufNumTextBox.Text = "";
            startButton.Enabled = true;
            stopButton.Enabled = false;
        }

        private void quitButton_Click(object sender, EventArgs e)
        {
            stopButton.PerformClick();
            this.Close();
            Application.Exit();
        }
    }
}