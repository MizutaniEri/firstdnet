using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace firstdnet
{
    public partial class DVDSelectForm : Form
    {
        private List<DriveInfo> DvdInfoList;
        public void SetDvdDrive(List<DriveInfo> dvdInfoList)
        {
            DvdInfoList = dvdInfoList;
            comboBox1.Items.Clear();
            dvdInfoList.ForEach(drive =>
            {
                string Vol = string.Empty;
                try
                {
                    Vol = drive.VolumeLabel;
                }
                catch (IOException)
                {
                    Vol = "(Not Ready)";
                }
                comboBox1.Items.Add(drive.Name + " " + Vol);
            });
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
        }

        public int GetSelectDrive(out object info)
        {
            if (DvdDriveSelectRadio.Checked)
            {
                info = DvdInfoList[comboBox1.SelectedIndex];
                return 1;
            }
            else if (DrivePathRadio.Checked)
            {
                info = null;
                return 0;
            }
            info = textBox1.Text;
            return 2;
        }

        public DVDSelectForm()
        {
            InitializeComponent();
        }

        private void DVDSelectForm_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndexChanged += (snd, ex) =>
            {
                DvdDriveSelectRadio.Checked = true;
            };
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var openDialog = new OpenFileDialog();
            openDialog.CheckFileExists = false;
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = Path.GetDirectoryName(openDialog.FileName);
                DrivePathRadio.Checked = true;
            }
        }
    }
}
