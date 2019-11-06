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

namespace Low_Level_Snap
{
    public partial class Form1 : Form
    {
        private ImaqdxSession _session = null;
        private VisionImage image;

        public Form1()
        {
            InitializeComponent();

            startButton.Enabled = true;

            // Enumerate cameras and populate control
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

            // Create image and attach to viewer
            image = new VisionImage();
            imageViewer.Attach(image);
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Open camera
                _session = new ImaqdxSession(cameraComboBox.Text);

                // Configure and start a single-shot acquisition with 1 image
                _session.Acquisition.Configure(ImaqdxAcquisitionType.SingleShot, 1);
                _session.Acquisition.Start();

                // Get our image
                _session.Acquisition.GetImageAt(image, 0);

                //Stop, unconfigure, and close camera
                _session.Acquisition.Stop();
                _session.Acquisition.Unconfigure();
                _session.Close();
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
    }
}