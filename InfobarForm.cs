using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace firstdnet
{
    public partial class InfobarForm : Form
    {
        private VideoForm mainForm;
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_CLOSE = 0xf060;

        public InfobarForm(VideoForm fm1)
        {
            InitializeComponent();
            mainForm = fm1;
        }

        public String drawText
        {
            set
            {
                label1.Text = value;
                Height = 16;
            }
        }

        protected override void WndProc(ref Message m)
        {
            // Closeボタン（右上の赤背景に白い×ボタン）または、左ボックスのシステムメニューから閉じるを選択
            if (m.Msg == WM_SYSCOMMAND && (int)m.WParam == SC_CLOSE)
            {
                Close();
                mainForm.Close();
            }
            base.WndProc(ref m);
        }

        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            mainForm.Form1_KeyDown(sender, e);
        }

        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            mainForm.Form1_MouseDown(sender, e);
        }

        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {
            mainForm.Form1_MouseMove(sender, e);
        }

        private void Form2_MouseLeave(object sender, EventArgs e)
        {
            mainForm.Form1_MouseLeave(sender, e);
        }
    }
}
