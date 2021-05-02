using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace firstdnet
{
    public partial class videoRename : Form
    {
        String OriginalVideoName;

        public String OrgVideoName
        {
            set { OriginalVideoName = value; }
        }

        String RenameVideoName;

        public String NewVideoName
        {
            get { return RenameVideoName; }
        }

        public videoRename()
        {
            InitializeComponent();
        }

        private void videoRename_Shown(object sender, EventArgs e)
        {
            oldName.Text = OriginalVideoName;
            newName.Text = OriginalVideoName;
            newName.SelectionStart = 0;
            newName.SelectionLength = OriginalVideoName.LastIndexOf(".");
        }

        private void OKbutton_Click(object sender, EventArgs e)
        {
            RenameVideoName = newName.Text;
        }
    }
}
