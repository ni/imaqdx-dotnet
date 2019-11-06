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

namespace Low_Level_Grab
{
    public partial class Form1 : Form
    {
        private ImaqdxSession _session = null;
        private System.ComponentModel.BackgroundWorker acquisitionWorker;
        private string selectedBufferWaitMode;
        private ImaqdxAcquisitionType selectedAcquisitionType;
        private int numBuffersUsed;

        public Form1()
        {
            InitializeComponent();

            // Set up initial UI state
            startButton.Enabled = true;
            stopButton.Enabled = false;
            numberOfBuffersControl.Enabled = true;
            acquisitionModeComboBox.Enabled = true;
            bufferWaitMode.Enabled = true;
            cameraComboBox.Enabled = true;
            acquisitionModeComboBox.DataSource = Enum.GetValues(typeof(ImaqdxAcquisitionType));
            acquisitionModeComboBox.SelectedItem = ImaqdxAcquisitionType.Continuous;
            bufferWaitMode.Items.AddRange( new string[]  {"Next", "Last", "BufferNumber"} );
            bufferWaitMode.SelectedItem = "BufferNumber";

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
                // Read UI controls
                string cameraName = cameraComboBox.Text;
                selectedAcquisitionType = (ImaqdxAcquisitionType)acquisitionModeComboBox.SelectedItem;
                numBuffersUsed = (int)numberOfBuffersControl.Value;
                selectedBufferWaitMode = bufferWaitMode.Text;

                // Open a new session and configure a grab
                _session = new ImaqdxSession(cameraName);
                _session.Acquisition.Configure(selectedAcquisitionType, numBuffersUsed);
                _session.Acquisition.Start();

                // Update UI state
                startButton.Enabled = false;
                stopButton.Enabled = true;
                numberOfBuffersControl.Enabled = false;
                acquisitionModeComboBox.Enabled = false;
                bufferWaitMode.Enabled = false;
                cameraComboBox.Enabled = false;

                // Start up background worker to acquire images
                acquisitionWorker.RunWorkerAsync();
            }
            catch (ImaqdxException ex)
            {
                _session.Close();
                _session = null;
                MessageBox.Show(ex.Message, "NI-IMAQdx Error");
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Notify the acquisition worker that we are stopping
                acquisitionWorker.CancelAsync();

                // Close the session
                if (_session != null)
                {
                    _session.Close();
                    _session = null;
                }
            }
            catch (ImaqdxException ex)
            {
                MessageBox.Show(ex.Message, "NI-IMAQdx Error");
            }
        }

        void acquisitionWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;

            // Keep track of our next buffer number to acquire
            uint nextBufferNumber = 0;

            // Keep acquiring until we error or cancel
            while (!worker.CancellationPending)
            {
                try
                {
                    // Holds the buffer number we acquired
                    uint bufNum = 0;

                    switch (selectedBufferWaitMode)
                    {
                        case "BufferNumber":
                            //     Ask for a specific buffer by its index
                            // In this example we always ask for an incrementing buffer number.
                            // If this buffer is not available, the driver's behavior is controlled
                            // by the "OverwriteMode" attribute which allows it to either generate
                            // an error or give some other buffer in its place
                            _session.Acquisition.GetImageAt(imageViewer.Image, nextBufferNumber++, out bufNum);
                            break;
                        case "Next":
                            //      Ask for the next image to be transferred.
                            // This is useful if only the absolute latest image should be processed.
                            // This will skip any images that have already arrived since the last
                            // GetImage call
                            _session.Acquisition.GetNextImage(imageViewer.Image, out bufNum);
                            break;
                        case "Last":
                            // This will always return the latest image that has already been acquired,
                            // rather than waiting. This will return the same image multiple times
                            // if no new images have been acquired.
                            _session.Acquisition.GetLastImage(imageViewer.Image, out bufNum);
                            break;
                    }

                    // Report our buffer number acquired
                    worker.ReportProgress(0, bufNum);

                    // If we're doing a single-shot acquisition, check if we're on the
                    // last image and finish up
                    if (selectedAcquisitionType == ImaqdxAcquisitionType.SingleShot)
                    {
                        if (bufNum == numBuffersUsed - 1)
                        {
                            return;
                        }
                    }
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
            // Show the any error result from the Grab operation
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
            startButton.Enabled = true;
            stopButton.Enabled = false;
            numberOfBuffersControl.Enabled = true;
            acquisitionModeComboBox.Enabled = true;
            bufferWaitMode.Enabled = true;
            cameraComboBox.Enabled = true;
        }

        private void quitButton_Click(object sender, EventArgs e)
        {
            stopButton.PerformClick();
            this.Close();
            Application.Exit();
        }
    }
}