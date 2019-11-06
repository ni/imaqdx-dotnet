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

namespace Sequence
{
    public partial class Form1 : Form
    {
        private ImaqdxSession _session = null;
        private VisionImage[] images;

        public Form1()
        {
            InitializeComponent();

            // Initialize UI controls
            startButton.Enabled = true;
            quitButton.Enabled = true;
            imageScrollBar.Enabled = false;

            // Enumerate cameras and update control
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
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            try
            {
                imageScrollBar.Enabled = false;
                int numberOfImages = (int)numImages.Value;

                // Open camera
                _session = new ImaqdxSession(cameraComboBox.Text);

                // Create array of images
                images = new VisionImage[numberOfImages];
                for (int i = 0; i < images.Length; ++i)
                {
                    images[i] = new VisionImage();
                }

                // Acquire the sequence of images
                _session.Sequence(images, numberOfImages);

                // Close the camera
                _session.Close();

                // Update UI controls
                imageViewer.Attach(images[0]);
                imageScrollBar.Minimum = 0;
                imageScrollBar.Value = imageScrollBar.Minimum;
                imageScrollBar.Maximum = numberOfImages - 1;
                imageScrollBar.Enabled = true;
            }
            catch (ImaqdxException ex)
            {
                MessageBox.Show(ex.Message, "NI-IMAQdx Error");
            }
        }

        private void quitButton_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void imageScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            if(e.Type == ScrollEventType.EndScroll)
            {
                imageViewer.Attach(images[imageScrollBar.Value]);
            }
        }
    }
}