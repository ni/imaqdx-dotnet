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

namespace Grab_and_Attributes
{
    public partial class Form1 : Form
    {
        private ImaqdxSession _session;
        private bool updatingControls = false;
        private System.ComponentModel.BackgroundWorker acquisitionWorker;

        public Form1()
        {
            InitializeComponent();

            // Set up initial button states
            startButton.Enabled = true;
            stopButton.Enabled = false;
            startButton.Enabled = false;

            // Set up image viewer
            VisionImage image = new VisionImage();
            imageViewer.Attach(image);

            // Enumerate cameras and update controls
            ImaqdxCameraInformation[] cameras = ImaqdxSystem.GetCameraInformation(true);
            foreach (ImaqdxCameraInformation camera in cameras)
            {
                cameraName.Items.Add(camera.Name);
            }
            cameraName.SelectedIndex = cameraName.Items.Count > 0 ? 0 : -1;

            // Set up background acquisition worker
            acquisitionWorker = new System.ComponentModel.BackgroundWorker();
            acquisitionWorker.DoWork += new DoWorkEventHandler(acquisitionWorker_DoWork);
            acquisitionWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(acquisitionWorker_RunWorkerCompleted);
            acquisitionWorker.ProgressChanged += new ProgressChangedEventHandler(acquisitionWorker_ProgressChanged);
            acquisitionWorker.WorkerReportsProgress = true;
            acquisitionWorker.WorkerSupportsCancellation = true;
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Stop any on-going acquisition
                stopButton.PerformClick();

                // Close the camera if one has already been opened
                if (_session != null)
                {
                    _session.Close();
                    _session = null;
                }

                // Create the session
                _session = new ImaqdxSession(cameraName.Text);

                // Update the tree of attributes
                PopulateTree();

                // Update the UI buttons
                startButton.Enabled = true;
            }
            catch (ImaqdxException error)
            {
                MessageBox.Show(error.ToString(), "NI-IMAQdx Error");
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Configure and start the acquisition
                const int numberOfBuffers = 10;
                _session.Acquisition.Configure(ImaqdxAcquisitionType.Continuous, numberOfBuffers);
                _session.Acquisition.Start();

                // Update the currently shown attribute in case it changes
                // value or access mode when acquiring
                UpdateAttribute();

                // Update the UI buttons
                startButton.Enabled = false;
                stopButton.Enabled = true;

                // Start up our background worker to acquire images
                acquisitionWorker.RunWorkerAsync();
            }
            catch (ImaqdxException error)
            {
                MessageBox.Show(error.ToString(), "NI-IMAQdx Error");
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Notify the acquisition thread we are stopping
                acquisitionWorker.CancelAsync();

                // Stop the acquisition. This will cause the acquisition
                // thread to get an "acquisition stopped" error if it is
                // blocked waiting for an image
                _session.Acquisition.Stop();
            }
            catch (ImaqdxException error)
            {
                MessageBox.Show(error.ToString(), "NI-IMAQdx Error");
            }
        }

        private void quitButton_Click(object sender, EventArgs e)
        {
            // Stop any acquisition
            stopButton.PerformClick();

            // Close the camera
            if (_session != null)
            {
                try
                {
                    _session.Close();
                    _session = null;
                }
                catch (ImaqdxException)
                {
                    // We might as well ignore errors here since we are quitting
                }
            }
            this.Close();
            Application.Exit();
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
            if(e.Result is ImaqdxException)
            {
                MessageBox.Show(((ImaqdxException)e.Result).ToString(), "NI-IMAQdx Error");
            }

            if (_session != null)
            {
                try
                {
                    // Unconfigure in case it already hasn't been unconfigured
                    _session.Acquisition.Unconfigure();

                    // Update the attribute displayed in case it changes
                    // when we start/stop acquiring
                    UpdateAttribute();
                }
                catch (ImaqdxException)
                {
                    // Ignore any errors unconfiguring since we already displayed
                    // one error to the user possibly
                }
            }
            // Update UI controls
            bufNumTextBox.Text = "";
            startButton.Enabled = true;
            stopButton.Enabled = false;
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

        private void PopulateTree()
        {
            // This function populates the tree containing all the attributes. The
            // attributes have fully-qualified names which contain each level of sub-category
            // seperated by a double colon ("::"). Attributes may be indexed by either the fullly
            // qualified name or by any sub-portion from the last portion and previous.
            //    e.g. "AcquisitionAttributes::Timeout" can be accessed directly or by the name
            //        "Timeout"
            // The code below seperates out the names and creates the tree structure as needed
            // to add each attribute
            tree.Nodes.Clear();
            foreach (ImaqdxAttribute attribute in _session.Attributes)
            {
                // We'll use the DisplayName for our tree. This name is like the normal name
                // but features cannot be directly referred to in code by this name. This allows
                // them to be much nicer names with spaces and formatting
                string name = attribute.DisplayName;
                string[] seperator = {"::"};
                string[] nameParts = name.Split(seperator, StringSplitOptions.None);
                // Find the root node this belongs under or create it if it isn't present
                TreeNode[] matches = tree.Nodes.Find(nameParts[0], true);
                TreeNode currentNode = matches.Length == 0 ? tree.Nodes.Add(nameParts[0], nameParts[0]) : matches[0];
                // Now go through each of the next parts of the name and add nodes as needed
                for (int i = 1; i < nameParts.Length; ++i)
                {
                    matches = currentNode.Nodes.Find(nameParts[i], true);
                    if (matches.Length == 0)
                    {
                        // This node isn't found, so add it
                        currentNode = currentNode.Nodes.Add(nameParts[i], nameParts[i]);
                        // If we are at the last portion of the name, we are done with
                        // the categories and are displaying the attribute itself. We'll
                        // assign the attribute into the node's tag so we can access it
                        // when the user clicks on the node
                        if (i == nameParts.Length - 1)
                        {
                            currentNode.Tag = attribute;
                        }
                    }
                    else
                    {
                        // The node was present, so traverse down it
                        currentNode = matches[0];
                    }
                }
            }
        }

        private void tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // The user clicked on an attribute in the tree, so update what
            // we are displaying
            UpdateAttribute();
        }

        private void UpdateAttribute()
        {
            // Use this variable to ensure that when we modify the UI controls
            // we ignore the update events so that we don't think the user is
            // setting them themselves
            updatingControls = true;
            try
            {
                if (tree.SelectedNode != null && tree.SelectedNode.Tag is ImaqdxAttribute)
                {
                    // The user clicked on an actual attribute so we will update the
                    // controls for the one they clicked on
                    UpdateItem((ImaqdxAttribute)tree.SelectedNode.Tag);
                }
                else
                {
                    // The user did not click on an attribute so we'll clear out whatever
                    // we were displaying
                    UpdateItem(null);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "NI-IMAQdx Error");
            }
            updatingControls = false;
        }

        private void UpdateItem(ImaqdxAttribute attribute)
        {
            attributeInfo.Clear();

            // If we are not displaying any attribute, switch to the empty tab
            // and return
            if (attribute == null)
            {
                tabControl.SelectedIndex = tabControl.TabPages.IndexOf(emptyPage);
                return;
            }
            // Show some useful generic information about the attribute
            attributeInfo.Text += "Name: " + attribute.Name + "\r\n";
            attributeInfo.Text += "Display Name: " + attribute.DisplayName + "\r\n";
            attributeInfo.Text += "Description: " + attribute.Description + "\r\n";
            attributeInfo.Text += "ToolTip: " + attribute.Tooltip + "\r\n";
            attributeInfo.Text += "Units: " + attribute.Units + "\r\n";
            attributeInfo.Text += "Type: " + attribute.Type.ToString() + "\r\n";
            attributeInfo.Text += "Access: " + (attribute.Readable ? "R" : "") + (attribute.Writable ? "W" : "") + "\r\n";
            attributeInfo.Text += "Visibility: " + attribute.Visibility.ToString() + "\r\n";

            // Now we need to go through each of the basic types that need different
            // controls populated. We use different tabs to contain the different
            // controls to easily hide the ones not in use. While the base ImaqdxAttribute
            // class will let you access the attribute in a basic fashion, to get all the
            // functionality we need to cast it to a more specific type
            if (attribute is ImaqdxEnumAttribute)
            {
                // This is an enumeration so switch to the Enum page
                ImaqdxEnumAttribute enumAttribute = (ImaqdxEnumAttribute)attribute;
                tabControl.SelectedIndex = tabControl.TabPages.IndexOf(enumPage);

                // Update the combobox with all the available enumerations
                enumItemsBox.Items.Clear();
                enumItemsBox.Items.AddRange(enumAttribute.GetSupportedValues());

                // If the attribute is readable, set the currently active value
                if (enumAttribute.Readable)
                {
                    enumItemsBox.SelectedItem = enumAttribute.Value;
                }

                // If the attribute is writable, enable the control
                enumItemsBox.Enabled = enumAttribute.Writable;
            }
            else if (attribute is ImaqdxNumericAttribute)
            {
                // This is a numeric attribute so switch to the Numeric page
                // Note that there are several different numeric types but we can treat
                // them more or less the same. The numeric attribute type uses the "decimal"
                // type which can contain most reasonable values of all the different types
                // (UInt32, Int64, Double)
                ImaqdxNumericAttribute numericAttribute = (ImaqdxNumericAttribute)attribute;
                tabControl.SelectedIndex = tabControl.TabPages.IndexOf(numericPage);

                rangeLabel.Text = "Min: " + numericAttribute.Minimum.ToString() + "\r\n";
                rangeLabel.Text += "Max: " + numericAttribute.Maximum.ToString() + "\r\n";
                rangeLabel.Text += "Increment: " + numericAttribute.Increment.ToString();

                numericControl.Minimum = (decimal)numericAttribute.Minimum;
                numericControl.Maximum = (decimal)numericAttribute.Maximum;
                numericControl.Increment = (decimal)numericAttribute.Increment;

                // We'll only show decimal places if it is a double attribute. All the rest
                // are integer types
                numericControl.DecimalPlaces = numericAttribute is ImaqdxDoubleAttribute ? 5 : 0;

                // If the attribute is readable, set the currently active value
                if (numericAttribute.Readable)
                {
                    numericControl.Value = (decimal)numericAttribute.Value;
                }

                // If the attribute is writable, enable the control
                numericControl.Enabled = numericAttribute.Writable;
            }
            else if (attribute is ImaqdxStringAttribute)
            {
                // This is a string attribute so switch to the String page
                ImaqdxStringAttribute stringAttribute = (ImaqdxStringAttribute)attribute;
                tabControl.SelectedIndex = tabControl.TabPages.IndexOf(stringPage);

                stringControl.Clear();

                // If the attribute is readable, set the currently active value
                if (stringAttribute.Readable)
                {
                    stringControl.Text = stringAttribute.Value;
                }

                // If the attribute is writable, enable the control
                stringControl.Enabled = stringSetButton.Enabled = stringAttribute.Writable;
            }
            else if (attribute is ImaqdxBoolAttribute)
            {
                // This is a bool attribute so switch to the Bool page
                ImaqdxBoolAttribute boolAttribute = (ImaqdxBoolAttribute)attribute;
                tabControl.SelectedIndex = tabControl.TabPages.IndexOf(boolPage);

                // If the attribute is readable, set the currently active value
                if (boolAttribute.Readable)
                {
                    boolControl.Checked = boolAttribute.Value;
                }

                // If the attribute is writable, enable the control
                boolControl.Enabled = boolAttribute.Writable;
            }
            else if (attribute is ImaqdxCommandAttribute)
            {
                // This is a command attribute so switch to the Command page
                ImaqdxCommandAttribute commandAttribute = (ImaqdxCommandAttribute)attribute;
                tabControl.SelectedIndex = tabControl.TabPages.IndexOf(commandPage);

                // If the attribute is writable, enable the control
                commandControl.Enabled = commandAttribute.Writable;
            }
        }

        private void enumItemsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // The user is setting an Enum attribute
            try
            {
                if (!updatingControls && tree.SelectedNode != null && tree.SelectedNode.Tag is ImaqdxEnumAttribute)
                {
                    ImaqdxEnumAttribute attribute = (ImaqdxEnumAttribute)tree.SelectedNode.Tag;
                    if (enumItemsBox.SelectedItem != null)
                    {
                        attribute.Value = (ImaqdxEnumAttributeItem)enumItemsBox.SelectedItem;
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString(), "NI-IMAQdx Error");
            }
        }

        private void numericControl_ValueChanged(object sender, EventArgs e)
        {
            // The user is setting a Numeric attribute
            try
            {
                if (!updatingControls && tree.SelectedNode != null && tree.SelectedNode.Tag is ImaqdxNumericAttribute)
                {
                    ImaqdxNumericAttribute attribute = (ImaqdxNumericAttribute)tree.SelectedNode.Tag;
                    attribute.Value = numericControl.Value;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString(), "NI-IMAQdx Error");
            }
        }

        private void commandControl_Click(object sender, EventArgs e)
        {
            // The user is setting a Command attribute
            try
            {
                if (!updatingControls && tree.SelectedNode != null && tree.SelectedNode.Tag is ImaqdxCommandAttribute)
                {
                    ImaqdxCommandAttribute attribute = (ImaqdxCommandAttribute)tree.SelectedNode.Tag;
                    attribute.Execute();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString(), "NI-IMAQdx Error");
            }
        }

        private void boolControl_CheckedChanged(object sender, EventArgs e)
        {
            // The user is setting a Bool attribute
            try
            {
                if (!updatingControls && tree.SelectedNode != null && tree.SelectedNode.Tag is ImaqdxBoolAttribute)
                {
                    ImaqdxBoolAttribute attribute = (ImaqdxBoolAttribute)tree.SelectedNode.Tag;
                    attribute.Value = boolControl.Checked;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString(), "NI-IMAQdx Error");
            }
        }

        private void stringSetButton_Click(object sender, EventArgs e)
        {
            // The user is setting a String attribute
            try
            {
                if (tree.SelectedNode != null && tree.SelectedNode.Tag is ImaqdxAttribute)
                {
                    ImaqdxStringAttribute attribute = (ImaqdxStringAttribute)tree.SelectedNode.Tag;
                    attribute.Value = stringControl.Text;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString(), "NI-IMAQdx Error");
            }
        }

        private void OnTabChanging(object sender, TabControlCancelEventArgs e)
        {
            // Don't let anything change which tab is displayed unless we
            // are in the middle of updating the tab internally
            e.Cancel = updatingControls ? false : true;
        }
    }
}