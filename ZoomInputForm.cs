using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace firstdnet
{
    public partial class ZoomScaleInput : Form
    {
        private double zoomScale;

        public double ZoomScale
        {
            get { return zoomScale; }
            set
            {
                zoomScale = value;
                ZoomNumericUpDown.Value = Convert.ToDecimal(zoomScale);
            }
        }

        private Size videoSize;

        public Size VideoSize
        {
            get { return videoSize; }
            set
            {
                videoSize = value;
                VideoSizeX.Value = videoSize.Width;
                VideoSizeY.Value = videoSize.Height;
            }
        }

        private bool inputZoomScale = true;
        private bool ShowFlag = false;

        public bool InputZoomScale
        {
            get { return inputZoomScale; }
            set
            {
                inputZoomScale = value;
                //if (inputZoomScale == true)
                //{
                //    ZoomScaleRadioButton.Checked = true;
                //}
                //else
                //{
                //    InputVideoSizeRadioButton.Checked = true;
                //}
            }
        }

        public ZoomScaleInput()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (ZoomScaleRadioButton.Checked == true)
            {
                zoomScale = Convert.ToDouble(ZoomNumericUpDown.Value);
                inputZoomScale = true;
            }
            else if (InputVideoSizeRadioButton.Checked == true)
            {
                videoSize = new Size(Convert.ToInt32(VideoSizeX.Value), Convert.ToInt32(VideoSizeY.Value));
                inputZoomScale = false;
            }
            else
            {
                switch (SelectVideoSizeComboBox.SelectedIndex)
                {
                    // 1920x1080(16:9)
                    case 0:
                        videoSize = new Size(1920, 1080);
                        break;
                    // 1440x1080(4:3)
                    case 1:
                        videoSize = new Size(1440, 1080);
                        break;
                    // 1280x720(16:9)
                    case 2:
                        videoSize = new Size(1280, 720);
                        break;
                    // 960x720(4:3)
                    case 3:
                        videoSize = new Size(960, 720);
                        break;
                    // 720x404(16:9)
                    case 4:
                        videoSize = new Size(720, 404);
                        break;
                    // 640x480(4:3)
                    case 5:
                        videoSize = new Size(640, 480);
                        break;
                }
                inputZoomScale = false;
            }
        }

        private void VideoSizeX_ValueChanged(object sender, EventArgs e)
        {
            if (ShowFlag == true && InputVideoSizeRadioButton.Checked == false)
            {
                InputVideoSizeRadioButton.Checked = true;
            }
        }

        private void VideoSizeX_Enter(object sender, EventArgs e)
        {
            VideoSizeX.Select(0, VideoSizeX.Text.Length);
        }

        private void VideoSizeY_Enter(object sender, EventArgs e)
        {
            VideoSizeY.Select(0, VideoSizeY.Text.Length);
        }

        private void ZoomNumericUpDown_Enter(object sender, EventArgs e)
        {
            ZoomNumericUpDown.Select(0, ZoomNumericUpDown.Text.Length);
            //ZoomScaleRadioButton.Checked = true;
        }

        private void ZoomNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (ShowFlag == true && ZoomScaleRadioButton.Checked == false)
            {
                ZoomScaleRadioButton.Checked = true;
            }
        }

        private void SelectVideoSizeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ShowFlag == true)
            {
                SelectVideoSizeRadioButton.Checked = true;
            }
        }

        private void ZoomScaleInput_Shown(object sender, EventArgs e)
        {
            ShowFlag = true;
        }
    }
}