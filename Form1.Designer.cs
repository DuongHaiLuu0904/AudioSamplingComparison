namespace AudioSamplingComparison
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpSamplingMethod = new System.Windows.Forms.GroupBox();
            this.txtMethodDescription = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbSamplingMethod = new System.Windows.Forms.ComboBox();
            this.grpSamplingParams = new System.Windows.Forms.GroupBox();
            this.numSamplingInterval = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numChannels = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numBitsPerSample = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numSampleRate = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.grpActions = new System.Windows.Forms.GroupBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnCapture = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.grpResults = new System.Windows.Forms.GroupBox();
            this.lvResults = new System.Windows.Forms.ListView();
            this.grpSamplingMethod.SuspendLayout();
            this.grpSamplingParams.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSamplingInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChannels)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBitsPerSample)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSampleRate)).BeginInit();
            this.grpActions.SuspendLayout();
            this.grpResults.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSamplingMethod
            // 
            this.grpSamplingMethod.Controls.Add(this.txtMethodDescription);
            this.grpSamplingMethod.Controls.Add(this.label5);
            this.grpSamplingMethod.Controls.Add(this.cmbSamplingMethod);
            this.grpSamplingMethod.Location = new System.Drawing.Point(12, 12);
            this.grpSamplingMethod.Name = "grpSamplingMethod";
            this.grpSamplingMethod.Size = new System.Drawing.Size(776, 123);
            this.grpSamplingMethod.TabIndex = 0;
            this.grpSamplingMethod.TabStop = false;
            this.grpSamplingMethod.Text = "Audio Sampling Method";
            // 
            // txtMethodDescription
            // 
            this.txtMethodDescription.Location = new System.Drawing.Point(17, 55);
            this.txtMethodDescription.Multiline = true;
            this.txtMethodDescription.Name = "txtMethodDescription";
            this.txtMethodDescription.ReadOnly = true;
            this.txtMethodDescription.Size = new System.Drawing.Size(743, 53);
            this.txtMethodDescription.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(131, 15);
            this.label5.TabIndex = 1;
            this.label5.Text = "Select Sampling Method:";
            // 
            // cmbSamplingMethod
            // 
            this.cmbSamplingMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSamplingMethod.FormattingEnabled = true;
            this.cmbSamplingMethod.Location = new System.Drawing.Point(154, 26);
            this.cmbSamplingMethod.Name = "cmbSamplingMethod";
            this.cmbSamplingMethod.Size = new System.Drawing.Size(166, 23);
            this.cmbSamplingMethod.TabIndex = 0;
            // 
            // grpSamplingParams
            // 
            this.grpSamplingParams.Controls.Add(this.numSamplingInterval);
            this.grpSamplingParams.Controls.Add(this.label4);
            this.grpSamplingParams.Controls.Add(this.numChannels);
            this.grpSamplingParams.Controls.Add(this.label3);
            this.grpSamplingParams.Controls.Add(this.numBitsPerSample);
            this.grpSamplingParams.Controls.Add(this.label2);
            this.grpSamplingParams.Controls.Add(this.numSampleRate);
            this.grpSamplingParams.Controls.Add(this.label1);
            this.grpSamplingParams.Location = new System.Drawing.Point(12, 141);
            this.grpSamplingParams.Name = "grpSamplingParams";
            this.grpSamplingParams.Size = new System.Drawing.Size(370, 119);
            this.grpSamplingParams.TabIndex = 1;
            this.grpSamplingParams.TabStop = false;
            this.grpSamplingParams.Text = "Sampling Parameters";
            // 
            // numSamplingInterval
            // 
            this.numSamplingInterval.Location = new System.Drawing.Point(154, 81);
            this.numSamplingInterval.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numSamplingInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSamplingInterval.Name = "numSamplingInterval";
            this.numSamplingInterval.Size = new System.Drawing.Size(89, 23);
            this.numSamplingInterval.TabIndex = 7;
            this.numSamplingInterval.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(131, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "Sampling Interval (ms):";
            // 
            // numChannels
            // 
            this.numChannels.Location = new System.Drawing.Point(154, 52);
            this.numChannels.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numChannels.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numChannels.Name = "numChannels";
            this.numChannels.Size = new System.Drawing.Size(89, 23);
            this.numChannels.TabIndex = 5;
            this.numChannels.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Channels:";
            // 
            // numBitsPerSample
            // 
            this.numBitsPerSample.Location = new System.Drawing.Point(263, 23);
            this.numBitsPerSample.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.numBitsPerSample.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numBitsPerSample.Name = "numBitsPerSample";
            this.numBitsPerSample.Size = new System.Drawing.Size(89, 23);
            this.numBitsPerSample.TabIndex = 3;
            this.numBitsPerSample.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(172, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Bits/Sample:";
            // 
            // numSampleRate
            // 
            this.numSampleRate.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numSampleRate.Location = new System.Drawing.Point(89, 23);
            this.numSampleRate.Maximum = new decimal(new int[] {
            192000,
            0,
            0,
            0});
            this.numSampleRate.Minimum = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            this.numSampleRate.Name = "numSampleRate";
            this.numSampleRate.Size = new System.Drawing.Size(77, 23);
            this.numSampleRate.TabIndex = 1;
            this.numSampleRate.Value = new decimal(new int[] {
            44100,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sample Rate:";
            // 
            // grpActions
            // 
            this.grpActions.Controls.Add(this.btnStop);
            this.grpActions.Controls.Add(this.btnPlay);
            this.grpActions.Controls.Add(this.btnCapture);
            this.grpActions.Controls.Add(this.progressBar);
            this.grpActions.Location = new System.Drawing.Point(388, 141);
            this.grpActions.Name = "grpActions";
            this.grpActions.Size = new System.Drawing.Size(400, 119);
            this.grpActions.TabIndex = 2;
            this.grpActions.TabStop = false;
            this.grpActions.Text = "Actions";
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(155, 34);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(111, 34);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            // 
            // btnPlay
            // 
            this.btnPlay.Enabled = false;
            this.btnPlay.Location = new System.Drawing.Point(272, 34);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(111, 34);
            this.btnPlay.TabIndex = 2;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            // 
            // btnCapture
            // 
            this.btnCapture.Location = new System.Drawing.Point(38, 34);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(111, 34);
            this.btnCapture.TabIndex = 1;
            this.btnCapture.Text = "Capture (1 min)";
            this.btnCapture.UseVisualStyleBackColor = true;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(38, 74);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(345, 23);
            this.progressBar.TabIndex = 0;
            // 
            // grpResults
            // 
            this.grpResults.Controls.Add(this.lvResults);
            this.grpResults.Location = new System.Drawing.Point(12, 266);
            this.grpResults.Name = "grpResults";
            this.grpResults.Size = new System.Drawing.Size(776, 231);
            this.grpResults.TabIndex = 3;
            this.grpResults.TabStop = false;
            this.grpResults.Text = "Results Comparison";
            // 
            // lvResults
            // 
            this.lvResults.HideSelection = false;
            this.lvResults.Location = new System.Drawing.Point(17, 22);
            this.lvResults.Name = "lvResults";
            this.lvResults.Size = new System.Drawing.Size(743, 193);
            this.lvResults.TabIndex = 0;
            this.lvResults.UseCompatibleStateImageBehavior = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 509);
            this.Controls.Add(this.grpResults);
            this.Controls.Add(this.grpActions);
            this.Controls.Add(this.grpSamplingParams);
            this.Controls.Add(this.grpSamplingMethod);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Audio Sampling Methods Comparison";
            this.grpSamplingMethod.ResumeLayout(false);
            this.grpSamplingMethod.PerformLayout();
            this.grpSamplingParams.ResumeLayout(false);
            this.grpSamplingParams.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSamplingInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChannels)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBitsPerSample)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSampleRate)).EndInit();
            this.grpActions.ResumeLayout(false);
            this.grpResults.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSamplingMethod;
        private System.Windows.Forms.GroupBox grpSamplingParams;
        private System.Windows.Forms.GroupBox grpActions;
        private System.Windows.Forms.GroupBox grpResults;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numSampleRate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numBitsPerSample;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numChannels;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numSamplingInterval;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbSamplingMethod;
        private System.Windows.Forms.TextBox txtMethodDescription;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btnCapture;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.ListView lvResults;
    }
}
