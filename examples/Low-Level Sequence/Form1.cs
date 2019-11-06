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

namespace Low_Level_Sequence
{
    public partial class Form1 : Form
    {
        private ImaqdxSession _session = null;
        private VisionImage[] images;

        public Form1()
        {
            InitializeComponent();

            // Enumerate cameras an populate list
            ImaqdxCameraInformation[] cameraList = ImaqdxSystem.GetCameraInformation(true);

            if (cameraList.Length > 0)
            {
                cameraComboBox.Items.Clear();
                foreach (ImaqdxCameraInformation camInfo in cameraList)
                {
                    cameraComboBox.Items.Add(camInfo.Name);
                }
                cameraComboBox.SelectedIndex = 0;
            }

            // Setup initial UI state
            startButton.Enabled = true;
            quitButton.Enabled = true;
            imageScrollBar.Enabled = false;
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            try
            {
                startButton.Enabled = false;
                int numberOfImages = (int)numImages.Value;

                // Open the camera
                _session = new ImaqdxSession(cameraComboBox.Text);

                // Create images
                images = new VisionImage[numberOfImages];
                for (uint i = 0; i < images.Length; ++i)
                {
                    images[i] = new VisionImage();
                }

                // Configure and start the camera
                _session.Acquisition.Configure(ImaqdxAcquisitionType.SingleShot, numberOfImages);
                _session.Acquisition.Start();

                // Get each image in the sequence
                for (uint i = 0; i < images.Length; ++i)
                {
                    _session.Acquisition.GetImageAt(images[i], i);
                }

                // Stop, Unconfigure, and Close the camera
                _session.Acquisition.Stop();
                _session.Acquisition.Unconfigure();
                _session.Close();
                _session = null;

                // Attach the viewer to the first image
                imageViewer.Attach(images[0]);

                // Setup the scroll bar to select the images
                imageScrollBar.Minimum = 0;
                imageScrollBar.Value = imageScrollBar.Minimum;
                imageScrollBar.Maximum = numberOfImages - 1;
                imageScrollBar.Enabled = true;
            }
            catch (ImaqdxException ex)
            {
                MessageBox.Show(ex.Message, "NI-IMAQdx Error");
            }
            startButton.Enabled = true;
        }

        private void quitButton_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void imageScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.Type == ScrollEventType.EndScroll)
            {
                // View the image selected
                imageViewer.Attach(images[imageScrollBar.Value]);
            }
        }
    }
}