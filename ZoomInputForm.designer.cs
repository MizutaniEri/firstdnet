namespace firstdnet
{
    partial class ZoomScaleInput
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.OkButton = new System.Windows.Forms.Button();
            this.CancelButton1 = new System.Windows.Forms.Button();
            this.ZoomNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.VideoSizeX = new System.Windows.Forms.NumericUpDown();
            this.VideoSizeY = new System.Windows.Forms.NumericUpDown();
            this.InputVideoSizeRadioButton = new System.Windows.Forms.RadioButton();
            this.ZoomScaleRadioButton = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.SelectVideoSizeComboBox = new System.Windows.Forms.ComboBox();
            this.SelectVideoSizeRadioButton = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.ZoomNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VideoSizeX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VideoSizeY)).BeginInit();
            this.SuspendLayout();
            // 
            // OkButton
            // 
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButton.Location = new System.Drawing.Point(43, 195);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 5;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // CancelButton1
            // 
            this.CancelButton1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton1.Location = new System.Drawing.Point(162, 195);
            this.CancelButton1.Name = "CancelButton1";
            this.CancelButton1.Size = new System.Drawing.Size(75, 23);
            this.CancelButton1.TabIndex = 6;
            this.CancelButton1.Text = "Cancel";
            this.CancelButton1.UseVisualStyleBackColor = true;
            // 
            // ZoomNumericUpDown
            // 
            this.ZoomNumericUpDown.DecimalPlaces = 2;
            this.ZoomNumericUpDown.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ZoomNumericUpDown.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.ZoomNumericUpDown.Location = new System.Drawing.Point(50, 54);
            this.ZoomNumericUpDown.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.ZoomNumericUpDown.Name = "ZoomNumericUpDown";
            this.ZoomNumericUpDown.Size = new System.Drawing.Size(78, 19);
            this.ZoomNumericUpDown.TabIndex = 1;
            this.ZoomNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ZoomNumericUpDown.ValueChanged += new System.EventHandler(this.ZoomNumericUpDown_ValueChanged);
            this.ZoomNumericUpDown.Enter += new System.EventHandler(this.ZoomNumericUpDown_Enter);
            // 
            // VideoSizeX
            // 
            this.VideoSizeX.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.VideoSizeX.Location = new System.Drawing.Point(53, 104);
            this.VideoSizeX.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.VideoSizeX.Name = "VideoSizeX";
            this.VideoSizeX.Size = new System.Drawing.Size(78, 19);
            this.VideoSizeX.TabIndex = 3;
            this.VideoSizeX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.VideoSizeX.ValueChanged += new System.EventHandler(this.VideoSizeX_ValueChanged);
            this.VideoSizeX.Enter += new System.EventHandler(this.VideoSizeX_Enter);
            // 
            // VideoSizeY
            // 
            this.VideoSizeY.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.VideoSizeY.Location = new System.Drawing.Point(155, 104);
            this.VideoSizeY.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.VideoSizeY.Name = "VideoSizeY";
            this.VideoSizeY.Size = new System.Drawing.Size(78, 19);
            this.VideoSizeY.TabIndex = 4;
            this.VideoSizeY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.VideoSizeY.ValueChanged += new System.EventHandler(this.VideoSizeX_ValueChanged);
            this.VideoSizeY.Enter += new System.EventHandler(this.VideoSizeY_Enter);
            // 
            // InputVideoSizeRadioButton
            // 
            this.InputVideoSizeRadioButton.AutoSize = true;
            this.InputVideoSizeRadioButton.Location = new System.Drawing.Point(36, 82);
            this.InputVideoSizeRadioButton.Name = "InputVideoSizeRadioButton";
            this.InputVideoSizeRadioButton.Size = new System.Drawing.Size(106, 16);
            this.InputVideoSizeRadioButton.TabIndex = 2;
            this.InputVideoSizeRadioButton.Text = "Input Video Size";
            this.InputVideoSizeRadioButton.UseVisualStyleBackColor = true;
            // 
            // ZoomScaleRadioButton
            // 
            this.ZoomScaleRadioButton.AutoSize = true;
            this.ZoomScaleRadioButton.Checked = true;
            this.ZoomScaleRadioButton.Location = new System.Drawing.Point(36, 32);
            this.ZoomScaleRadioButton.Name = "ZoomScaleRadioButton";
            this.ZoomScaleRadioButton.Size = new System.Drawing.Size(82, 16);
            this.ZoomScaleRadioButton.TabIndex = 0;
            this.ZoomScaleRadioButton.TabStop = true;
            this.ZoomScaleRadioButton.Text = "Zoom scale";
            this.ZoomScaleRadioButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(137, 108);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(11, 12);
            this.label1.TabIndex = 11;
            this.label1.Text = "x";
            // 
            // SelectVideoSizeComboBox
            // 
            this.SelectVideoSizeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelectVideoSizeComboBox.FormattingEnabled = true;
            this.SelectVideoSizeComboBox.Items.AddRange(new object[] {
            "1920x1080(16:9)",
            "1440x1080(4:3)",
            "1280x720(16:9)",
            "960x720(4:3)",
            "720x404(16:9)",
            "640x480(4:3)"});
            this.SelectVideoSizeComboBox.Location = new System.Drawing.Point(53, 151);
            this.SelectVideoSizeComboBox.Name = "SelectVideoSizeComboBox";
            this.SelectVideoSizeComboBox.Size = new System.Drawing.Size(180, 20);
            this.SelectVideoSizeComboBox.TabIndex = 12;
            this.SelectVideoSizeComboBox.SelectedIndexChanged += new System.EventHandler(this.SelectVideoSizeComboBox_SelectedIndexChanged);
            this.SelectVideoSizeComboBox.SelectedIndex = 0;
            //this.SelectVideoSizeComboBox.SelectedIndex = 0;
            // 
            // SelectVideoSizeRadioButton
            // 
            this.SelectVideoSizeRadioButton.AutoSize = true;
            this.SelectVideoSizeRadioButton.Location = new System.Drawing.Point(36, 129);
            this.SelectVideoSizeRadioButton.Name = "SelectVideoSizeRadioButton";
            this.SelectVideoSizeRadioButton.Size = new System.Drawing.Size(113, 16);
            this.SelectVideoSizeRadioButton.TabIndex = 13;
            this.SelectVideoSizeRadioButton.Text = "Select Video Size";
            this.SelectVideoSizeRadioButton.UseVisualStyleBackColor = true;
            // 
            // ZoomScaleInput
            // 
            this.AcceptButton = this.OkButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelButton1;
            this.ClientSize = new System.Drawing.Size(264, 230);
            this.Controls.Add(this.SelectVideoSizeRadioButton);
            this.Controls.Add(this.SelectVideoSizeComboBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ZoomScaleRadioButton);
            this.Controls.Add(this.InputVideoSizeRadioButton);
            this.Controls.Add(this.VideoSizeY);
            this.Controls.Add(this.VideoSizeX);
            this.Controls.Add(this.ZoomNumericUpDown);
            this.Controls.Add(this.CancelButton1);
            this.Controls.Add(this.OkButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "ZoomScaleInput";
            this.Text = "Zoom Scale";
            this.Shown += new System.EventHandler(this.ZoomScaleInput_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.ZoomNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VideoSizeX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VideoSizeY)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CancelButton1;
        private System.Windows.Forms.NumericUpDown ZoomNumericUpDown;
        private System.Windows.Forms.NumericUpDown VideoSizeX;
        private System.Windows.Forms.NumericUpDown VideoSizeY;
        private System.Windows.Forms.RadioButton InputVideoSizeRadioButton;
        private System.Windows.Forms.RadioButton ZoomScaleRadioButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox SelectVideoSizeComboBox;
        private System.Windows.Forms.RadioButton SelectVideoSizeRadioButton;
    }
}