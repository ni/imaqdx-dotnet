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

namespace Low_Level_Grab
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
            this.cameraComboBox = new System.Windows.Forms.ComboBox();
            this.acquisitionModeComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numberOfBuffersControl = new System.Windows.Forms.NumericUpDown();
            this.bufferWaitMode = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfBuffersControl)).BeginInit();
            this.SuspendLayout();
            //
            // imageViewer
            //
            this.imageViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageViewer.Location = new System.Drawing.Point(3, 12);
            this.imageViewer.Name = "imageViewer";
            this.imageViewer.Size = new System.Drawing.Size(508, 380);
            this.imageViewer.TabIndex = 0;
            this.imageViewer.ZoomToFit = true;
            //
            // startButton
            //
            this.startButton.Location = new System.Drawing.Point(161, 461);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(88, 25);
            this.startButton.TabIndex = 2;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            //
            // bufNumTextBox
            //
            this.bufNumTextBox.Location = new System.Drawing.Point(3, 461);
            this.bufNumTextBox.Name = "bufNumTextBox";
            this.bufNumTextBox.ReadOnly = true;
            this.bufNumTextBox.Size = new System.Drawing.Size(107, 20);
            this.bufNumTextBox.TabIndex = 3;
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 395);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Camera Name";
            //
            // stopButton
            //
            this.stopButton.Location = new System.Drawing.Point(266, 461);
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
            this.label2.Location = new System.Drawing.Point(0, 445);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Buffer Number";
            //
            // quitButton
            //
            this.quitButton.Location = new System.Drawing.Point(423, 461);
            this.quitButton.Name = "quitButton";
            this.quitButton.Size = new System.Drawing.Size(82, 25);
            this.quitButton.TabIndex = 2;
            this.quitButton.Text = "Quit";
            this.quitButton.UseVisualStyleBackColor = true;
            this.quitButton.Click += new System.EventHandler(this.quitButton_Click);
            //
            // cameraComboBox
            //
            this.cameraComboBox.FormattingEnabled = true;
            this.cameraComboBox.Location = new System.Drawing.Point(3, 410);
            this.cameraComboBox.Name = "cameraComboBox";
            this.cameraComboBox.Size = new System.Drawing.Size(115, 21);
            this.cameraComboBox.TabIndex = 5;
            this.cameraComboBox.Text = "cam0";
            //
            // acquisitionModeComboBox
            //
            this.acquisitionModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.acquisitionModeComboBox.Location = new System.Drawing.Point(128, 410);
            this.acquisitionModeComboBox.Name = "acquisitionModeComboBox";
            this.acquisitionModeComboBox.Size = new System.Drawing.Size(121, 21);
            this.acquisitionModeComboBox.TabIndex = 5;
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(128, 395);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Acquisition Mode";
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(257, 395);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Number of Buffers";
            //
            // numberOfBuffersControl
            //
            this.numberOfBuffersControl.Location = new System.Drawing.Point(260, 412);
            this.numberOfBuffersControl.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numberOfBuffersControl.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numberOfBuffersControl.Name = "numberOfBuffersControl";
            this.numberOfBuffersControl.Size = new System.Drawing.Size(120, 20);
            this.numberOfBuffersControl.TabIndex = 9;
            this.numberOfBuffersControl.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            //
            // bufferWaitMode
            //
            this.bufferWaitMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bufferWaitMode.FormattingEnabled = true;
            this.bufferWaitMode.Location = new System.Drawing.Point(390, 411);
            this.bufferWaitMode.Name = "bufferWaitMode";
            this.bufferWaitMode.Size = new System.Drawing.Size(121, 21);
            this.bufferWaitMode.TabIndex = 10;
            //
            // label5
            //
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(390, 395);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Buffer Wait Mode";
            //
            // Form1
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 490);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.bufferWaitMode);
            this.Controls.Add(this.numberOfBuffersControl);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.acquisitionModeComboBox);
            this.Controls.Add(this.cameraComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bufNumTextBox);
            this.Controls.Add(this.quitButton);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.imageViewer);
            this.Name = "Form1";
            this.Text = "NI-IMAQdx Low-Level Grab Example";
            ((System.ComponentModel.ISupportInitialize)(this.numberOfBuffersControl)).EndInit();
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
        private System.Windows.Forms.ComboBox cameraComboBox;
        private System.Windows.Forms.ComboBox acquisitionModeComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numberOfBuffersControl;
        private System.Windows.Forms.ComboBox bufferWaitMode;
        private System.Windows.Forms.Label label5;
    }
}

