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

namespace Grab_and_Attributes
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.imageViewer = new NationalInstruments.Vision.WindowsForms.ImageViewer();
            this.startButton = new System.Windows.Forms.Button();
            this.bufNumTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.stopButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.quitButton = new System.Windows.Forms.Button();
            this.tree = new System.Windows.Forms.TreeView();
            this.attributeInfo = new System.Windows.Forms.RichTextBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.emptyPage = new System.Windows.Forms.TabPage();
            this.numericPage = new System.Windows.Forms.TabPage();
            this.rangeLabel = new System.Windows.Forms.Label();
            this.numericControl = new System.Windows.Forms.NumericUpDown();
            this.enumPage = new System.Windows.Forms.TabPage();
            this.enumItemsBox = new System.Windows.Forms.ComboBox();
            this.stringPage = new System.Windows.Forms.TabPage();
            this.stringSetButton = new System.Windows.Forms.Button();
            this.stringControl = new System.Windows.Forms.TextBox();
            this.commandPage = new System.Windows.Forms.TabPage();
            this.commandControl = new System.Windows.Forms.Button();
            this.boolPage = new System.Windows.Forms.TabPage();
            this.boolControl = new System.Windows.Forms.CheckBox();
            this.cameraName = new System.Windows.Forms.ComboBox();
            this.openButton = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.numericPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericControl)).BeginInit();
            this.enumPage.SuspendLayout();
            this.stringPage.SuspendLayout();
            this.commandPage.SuspendLayout();
            this.boolPage.SuspendLayout();
            this.SuspendLayout();
            //
            // imageViewer
            //
            this.imageViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageViewer.Location = new System.Drawing.Point(217, 12);
            this.imageViewer.Name = "imageViewer";
            this.imageViewer.ShowImageInfo = true;
            this.imageViewer.ShowScrollbars = true;
            this.imageViewer.Size = new System.Drawing.Size(397, 344);
            this.imageViewer.TabIndex = 0;
            this.imageViewer.ZoomToFit = true;
            //
            // startButton
            //
            this.startButton.Location = new System.Drawing.Point(3, 434);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(82, 25);
            this.startButton.TabIndex = 2;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            //
            // bufNumTextBox
            //
            this.bufNumTextBox.Location = new System.Drawing.Point(3, 509);
            this.bufNumTextBox.Name = "bufNumTextBox";
            this.bufNumTextBox.ReadOnly = true;
            this.bufNumTextBox.Size = new System.Drawing.Size(110, 20);
            this.bufNumTextBox.TabIndex = 3;
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 360);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Camera Name";
            //
            // stopButton
            //
            this.stopButton.Location = new System.Drawing.Point(3, 465);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(82, 25);
            this.stopButton.TabIndex = 2;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 493);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Buffer Number";
            //
            // quitButton
            //
            this.quitButton.Location = new System.Drawing.Point(3, 535);
            this.quitButton.Name = "quitButton";
            this.quitButton.Size = new System.Drawing.Size(82, 25);
            this.quitButton.TabIndex = 2;
            this.quitButton.Text = "Quit";
            this.quitButton.UseVisualStyleBackColor = true;
            this.quitButton.Click += new System.EventHandler(this.quitButton_Click);
            //
            // tree
            //
            this.tree.Location = new System.Drawing.Point(3, 12);
            this.tree.Name = "tree";
            this.tree.Size = new System.Drawing.Size(212, 344);
            this.tree.TabIndex = 6;
            this.tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tree_AfterSelect);
            //
            // attributeInfo
            //
            this.attributeInfo.Location = new System.Drawing.Point(119, 362);
            this.attributeInfo.Name = "attributeInfo";
            this.attributeInfo.ReadOnly = true;
            this.attributeInfo.Size = new System.Drawing.Size(495, 76);
            this.attributeInfo.TabIndex = 8;
            this.attributeInfo.Text = "";
            //
            // tabControl
            //
            this.tabControl.Controls.Add(this.emptyPage);
            this.tabControl.Controls.Add(this.numericPage);
            this.tabControl.Controls.Add(this.enumPage);
            this.tabControl.Controls.Add(this.stringPage);
            this.tabControl.Controls.Add(this.commandPage);
            this.tabControl.Controls.Add(this.boolPage);
            this.tabControl.Location = new System.Drawing.Point(119, 444);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(495, 116);
            this.tabControl.TabIndex = 9;
            this.tabControl.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.OnTabChanging);
            //
            // emptyPage
            //
            this.emptyPage.Location = new System.Drawing.Point(4, 22);
            this.emptyPage.Name = "emptyPage";
            this.emptyPage.Padding = new System.Windows.Forms.Padding(3);
            this.emptyPage.Size = new System.Drawing.Size(487, 90);
            this.emptyPage.TabIndex = 6;
            this.emptyPage.UseVisualStyleBackColor = true;
            //
            // numericPage
            //
            this.numericPage.Controls.Add(this.rangeLabel);
            this.numericPage.Controls.Add(this.numericControl);
            this.numericPage.Location = new System.Drawing.Point(4, 22);
            this.numericPage.Name = "numericPage";
            this.numericPage.Size = new System.Drawing.Size(487, 90);
            this.numericPage.TabIndex = 1;
            this.numericPage.Text = "Numeric";
            this.numericPage.UseVisualStyleBackColor = true;
            //
            // rangeLabel
            //
            this.rangeLabel.Location = new System.Drawing.Point(179, 11);
            this.rangeLabel.Name = "rangeLabel";
            this.rangeLabel.Size = new System.Drawing.Size(258, 79);
            this.rangeLabel.TabIndex = 1;
            this.rangeLabel.Text = "Range Info";
            //
            // numericControl
            //
            this.numericControl.Location = new System.Drawing.Point(18, 23);
            this.numericControl.Name = "numericControl";
            this.numericControl.Size = new System.Drawing.Size(136, 20);
            this.numericControl.TabIndex = 0;
            this.numericControl.ValueChanged += new System.EventHandler(this.numericControl_ValueChanged);
            //
            // enumPage
            //
            this.enumPage.Controls.Add(this.enumItemsBox);
            this.enumPage.Location = new System.Drawing.Point(4, 22);
            this.enumPage.Name = "enumPage";
            this.enumPage.Size = new System.Drawing.Size(487, 90);
            this.enumPage.TabIndex = 2;
            this.enumPage.Text = "Enumeration";
            this.enumPage.UseVisualStyleBackColor = true;
            //
            // enumItemsBox
            //
            this.enumItemsBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.enumItemsBox.FormattingEnabled = true;
            this.enumItemsBox.Location = new System.Drawing.Point(20, 22);
            this.enumItemsBox.Name = "enumItemsBox";
            this.enumItemsBox.Size = new System.Drawing.Size(209, 21);
            this.enumItemsBox.TabIndex = 0;
            this.enumItemsBox.SelectedIndexChanged += new System.EventHandler(this.enumItemsBox_SelectedIndexChanged);
            //
            // stringPage
            //
            this.stringPage.Controls.Add(this.stringSetButton);
            this.stringPage.Controls.Add(this.stringControl);
            this.stringPage.Location = new System.Drawing.Point(4, 22);
            this.stringPage.Name = "stringPage";
            this.stringPage.Padding = new System.Windows.Forms.Padding(3);
            this.stringPage.Size = new System.Drawing.Size(487, 90);
            this.stringPage.TabIndex = 3;
            this.stringPage.Text = "String";
            this.stringPage.UseVisualStyleBackColor = true;
            //
            // stringSetButton
            //
            this.stringSetButton.Location = new System.Drawing.Point(331, 15);
            this.stringSetButton.Name = "stringSetButton";
            this.stringSetButton.Size = new System.Drawing.Size(75, 23);
            this.stringSetButton.TabIndex = 1;
            this.stringSetButton.Text = "Set";
            this.stringSetButton.UseVisualStyleBackColor = true;
            this.stringSetButton.Click += new System.EventHandler(this.stringSetButton_Click);
            //
            // stringControl
            //
            this.stringControl.Location = new System.Drawing.Point(13, 15);
            this.stringControl.Name = "stringControl";
            this.stringControl.Size = new System.Drawing.Size(311, 20);
            this.stringControl.TabIndex = 0;
            //
            // commandPage
            //
            this.commandPage.Controls.Add(this.commandControl);
            this.commandPage.Location = new System.Drawing.Point(4, 22);
            this.commandPage.Name = "commandPage";
            this.commandPage.Size = new System.Drawing.Size(487, 90);
            this.commandPage.TabIndex = 4;
            this.commandPage.Text = "Command";
            this.commandPage.UseVisualStyleBackColor = true;
            //
            // commandControl
            //
            this.commandControl.Location = new System.Drawing.Point(39, 19);
            this.commandControl.Name = "commandControl";
            this.commandControl.Size = new System.Drawing.Size(75, 23);
            this.commandControl.TabIndex = 0;
            this.commandControl.Text = "Execute";
            this.commandControl.UseVisualStyleBackColor = true;
            this.commandControl.Click += new System.EventHandler(this.commandControl_Click);
            //
            // boolPage
            //
            this.boolPage.Controls.Add(this.boolControl);
            this.boolPage.Location = new System.Drawing.Point(4, 22);
            this.boolPage.Name = "boolPage";
            this.boolPage.Size = new System.Drawing.Size(487, 90);
            this.boolPage.TabIndex = 5;
            this.boolPage.Text = "Boolean";
            this.boolPage.UseVisualStyleBackColor = true;
            //
            // boolControl
            //
            this.boolControl.AutoSize = true;
            this.boolControl.Location = new System.Drawing.Point(37, 20);
            this.boolControl.Name = "boolControl";
            this.boolControl.Size = new System.Drawing.Size(53, 17);
            this.boolControl.TabIndex = 0;
            this.boolControl.Text = "Value";
            this.boolControl.UseVisualStyleBackColor = true;
            this.boolControl.CheckedChanged += new System.EventHandler(this.boolControl_CheckedChanged);
            //
            // cameraName
            //
            this.cameraName.FormattingEnabled = true;
            this.cameraName.Location = new System.Drawing.Point(3, 376);
            this.cameraName.Name = "cameraName";
            this.cameraName.Size = new System.Drawing.Size(110, 21);
            this.cameraName.TabIndex = 10;
            this.cameraName.Text = "cam0";
            //
            // openButton
            //
            this.openButton.Location = new System.Drawing.Point(3, 403);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(82, 25);
            this.openButton.TabIndex = 2;
            this.openButton.Text = "Open";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            //
            // Form1
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 566);
            this.Controls.Add(this.cameraName);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.attributeInfo);
            this.Controls.Add(this.tree);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bufNumTextBox);
            this.Controls.Add(this.quitButton);
            this.Controls.Add(this.openButton);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.imageViewer);
            this.Name = "Form1";
            this.Text = "NI-IMAQdx Grab and Attributes Example";
            this.tabControl.ResumeLayout(false);
            this.numericPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericControl)).EndInit();
            this.enumPage.ResumeLayout(false);
            this.stringPage.ResumeLayout(false);
            this.stringPage.PerformLayout();
            this.commandPage.ResumeLayout(false);
            this.boolPage.ResumeLayout(false);
            this.boolPage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NationalInstruments.Vision.WindowsForms.ImageViewer imageViewer;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.TextBox bufNumTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button quitButton;
        private System.Windows.Forms.TreeView tree;
        private System.Windows.Forms.RichTextBox attributeInfo;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage numericPage;
        private System.Windows.Forms.TabPage enumPage;
        private System.Windows.Forms.ComboBox enumItemsBox;
        private System.Windows.Forms.NumericUpDown numericControl;
        private System.Windows.Forms.TabPage stringPage;
        private System.Windows.Forms.TabPage commandPage;
        private System.Windows.Forms.TextBox stringControl;
        private System.Windows.Forms.Button commandControl;
        private System.Windows.Forms.TabPage boolPage;
        private System.Windows.Forms.CheckBox boolControl;
        private System.Windows.Forms.ComboBox cameraName;
        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.TabPage emptyPage;
        private System.Windows.Forms.Label rangeLabel;
        private System.Windows.Forms.Button stringSetButton;
    }
}

