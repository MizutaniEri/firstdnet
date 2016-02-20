namespace firstdnet
{
    partial class DVDSelectForm
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.DvdDriveSelectRadio = new System.Windows.Forms.RadioButton();
            this.DefaultRadio = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.DrivePathRadio = new System.Windows.Forms.RadioButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(139, 69);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(143, 20);
            this.comboBox1.TabIndex = 0;
            // 
            // DvdDriveSelectRadio
            // 
            this.DvdDriveSelectRadio.AutoSize = true;
            this.DvdDriveSelectRadio.Location = new System.Drawing.Point(19, 69);
            this.DvdDriveSelectRadio.Name = "DvdDriveSelectRadio";
            this.DvdDriveSelectRadio.Size = new System.Drawing.Size(113, 16);
            this.DvdDriveSelectRadio.TabIndex = 1;
            this.DvdDriveSelectRadio.Text = "DVD Drive select";
            this.DvdDriveSelectRadio.UseVisualStyleBackColor = true;
            // 
            // DefaultRadio
            // 
            this.DefaultRadio.AutoSize = true;
            this.DefaultRadio.Checked = true;
            this.DefaultRadio.Location = new System.Drawing.Point(19, 30);
            this.DefaultRadio.Name = "DefaultRadio";
            this.DefaultRadio.Size = new System.Drawing.Size(124, 16);
            this.DefaultRadio.TabIndex = 2;
            this.DefaultRadio.TabStop = true;
            this.DefaultRadio.Text = "Default drive select";
            this.DefaultRadio.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(19, 136);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(186, 136);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // DrivePathRadio
            // 
            this.DrivePathRadio.AutoSize = true;
            this.DrivePathRadio.Location = new System.Drawing.Point(19, 105);
            this.DrivePathRadio.Name = "DrivePathRadio";
            this.DrivePathRadio.Size = new System.Drawing.Size(76, 16);
            this.DrivePathRadio.TabIndex = 5;
            this.DrivePathRadio.TabStop = true;
            this.DrivePathRadio.Text = "Drive path";
            this.DrivePathRadio.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(101, 104);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(154, 19);
            this.textBox1.TabIndex = 6;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(261, 104);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(20, 16);
            this.button3.TabIndex = 7;
            this.button3.Text = "...";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // DVDSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(294, 171);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.DrivePathRadio);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.DefaultRadio);
            this.Controls.Add(this.DvdDriveSelectRadio);
            this.Controls.Add(this.comboBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "DVDSelectForm";
            this.Text = "DVDSelectForm";
            this.Load += new System.EventHandler(this.DVDSelectForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.RadioButton DvdDriveSelectRadio;
        private System.Windows.Forms.RadioButton DefaultRadio;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RadioButton DrivePathRadio;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button3;
    }
}