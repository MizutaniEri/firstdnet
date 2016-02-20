using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace firstdnet
{
    public partial class InputSeekTime : Form
    {
        private DateTime seekTime;
        private String endTime = string.Empty;

        public String EndTime
        {
            get { return endTime; }
            set
            {
                label1.Text = value;
            }
        }
        public DateTime SeekTime
        {
            get { return seekTime; }
            set
            {
                seekTime = value;
                dateTimePicker1.Value = value;
            }
        }

        public InputSeekTime()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            seekTime = dateTimePicker1.Value;
        }
    }
}
