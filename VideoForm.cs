using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DSInterfaces;
using Utility;

namespace firstdnet
{
    public partial class VideoForm : Form
    {
        [FlagsAttribute]
        public enum ExecutionState : uint
        {
            // 関数が失敗した時の戻り値
            Null = 0,
            // スタンバイを抑止
            SystemRequired = 1,
            // 画面OFFを抑止
            DisplayRequired = 2,
            // 効果を永続させる。ほかオプションと併用する。
            Continuous = 0x80000000,
        }

        [DllImport("user32.dll")]
        extern public static int RegisterWindowMessage(string sRwm);
        public static int WM_DSEvent = RegisterWindowMessage(Application.ExecutablePath);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter,
            int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("kernel32.dll")]
        extern static ExecutionState SetThreadExecutionState(ExecutionState esFlags);

        public readonly int SWP_NOSIZE = 0x0001;
        public readonly int SWP_NOMOVE = 0x0002;
        public readonly int SWP_NOZORDER = 0x0004;
        public readonly int SWP_NOREDRAW = 0x0008;
        public readonly int SWP_NOACTIVATE = 0x0010;
        public readonly int SWP_FRAMECHANGED = 0x0020;  /* The frame changed: send WM_NCCALCSIZE */
        public readonly int SWP_SHOWWINDOW = 0x0040;
        public readonly int SWP_HIDEWINDOW = 0x0080;
        public readonly int SWP_NOCOPYBITS = 0x0100;
        public readonly int SWP_NOOWNERZORDER = 0x0200; /* Don't do owner Z ordering */
        public readonly int SWP_NOSENDCHANGING = 0x0400;  /* Don't send WM_WINDOWPOSCHANGING */
        public readonly int HWND_TOP = (0);
        public readonly int HWND_BOTTOM = (1);
        public readonly int HWND_TOPMOST = (-1);
        public readonly int HWND_NOTOPMOST = (-2);
        public readonly int WM_SYSCOMMAND = 0x0112;
        public readonly int SC_CLOSE = 0xf060;

        private FormLib fl = null;
        private String videoFileName = null;
        private int JpegQuality = 100;
        private Point mousePoint { get; set; }
        private int initVolume = 80;
        private ZoomScaleInput ZoomScaleInputForm = null;
        private int MouseX = 0;
        private int MouseY = 0;
        private bool hideMouse = false;
        private bool formMove = false;
        private Point formMovePoint = new Point();
        private bool forceEvr = true;
        private InfobarForm fm2 = null;
        private int fourCorner = 0;
        private int toSide = 0;
        private String videoInfo = "";
        private double nowZoomScale = 1.0;
        private string savaFileNameStore = "";
        private bool muteStatus = false;
        private int nowVolume = 0;
        private ulong ResumePosition;
        private Form workForm;

        public async Task<string> GetNextVideoFile(string nowFile, int befNext)
        {
            var list = await FileList.GetFileListAsync(nowFile);
            return (FileList.GetNextFile(list, nowFile, befNext));
        }

        public VideoForm()
        {
            InitializeComponent();
            //Size = new Size(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width,
            //    System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height);
            ZoomScaleInputForm = new ZoomScaleInput();

            fl = new FormLib(this);
            fl.Complete += async (sender, e) =>
            {
                var nextFile = await GetNextVideoFile(videoFileName, 1);
                if (nextFile == string.Empty)
                {
                    timer1.Stop();
                    fl.PauseGraph();
                    fl.StopGraph();
                    Close();
                }
                else
                {
                    noChangeSizeVideoPlay(nextFile);
                }
            };
            String[] args = System.Environment.GetCommandLineArgs();
            String fileName = null;
            bool screenSize = false;

            // =====> 単純イベント登録(ラムダ式使用のみ) ====>
            // Seek
            seekToolStripMenuItem.Click += (sender, e) => videoSeek();
            // Capture Image
            captureImageToolStripMenuItem.Click += (sender, e) => nowVideoSaveImage();
            // Open Video X1
            openVideox1ToolStripMenuItem.Click += (sender, e) => orginalVideoPlay();
            // Zoom Video
            zoomX05ToolStripMenuItem.Click += (sender, e) => changeClientFormSize(0.5);
            zoomX08ToolStripMenuItem.Click += (sender, e) => changeClientFormSize(0.8);
            zoomX09ToolStripMenuItem.Click += (sender, e) => changeClientFormSize(0.9);
            zoomX10ToolStripMenuItem.Click += (sender, e) => changeClientFormSize(1.0);
            zoomX12ToolStripMenuItem.Click += (sender, e) => changeClientFormSize(1.2);
            zoomX14ToolStripMenuItem.Click += (sender, e) => changeClientFormSize(1.4);
            zoomX18ToolStripMenuItem.Click += (sender, e) => changeClientFormSize(1.8);
            zoomX20ToolStripMenuItem.Click += (sender, e) => changeClientFormSize(2.0);
            zoomX25ToolStripMenuItem.Click += (sender, e) => changeClientFormSize(2.5);
            zoomX30ToolStripMenuItem.Click += (sender, e) => changeClientFormSize(3.0);
            zoomX35ToolStripMenuItem.Click += (sender, e) => changeClientFormSize(3.5);
            zoomX40ToolStripMenuItem.Click += (sender, e) => changeClientFormSize(4.5);
            // ETC...
            viewScaleInputToolStripMenuItem.Click += (sender, e) => zoomScaleChange();
            playPauseToolStripMenuItem.Click += (sender, e) => playOrPause();
            stopToolStripMenuItem.Click += (sender, e) => stopVideo();
            fastForwardToolStripMenuItem.Click += (sender, e) => fl.Position += convSecondToUlong(30.0);
            skip1min30secMenuItem1.Click += (sender, e) => fl.Position += convSecondToUlong(90.0);
            rewindToolStripMenuItem.Click += (sender, e) => fl.Position -= convSecondToUlong(30.0);
            resetToolStripMenuItem.Click += (sender, e) => resetPlay();
            filenameRenameToolStripMenuItem.Click += (sender, e) => videoFileRename();
            panel1.DoubleClick += (sender, e) => changeFullScreen();
            DoubleClick += (sender, e) => changeFullScreen();
            contextMenuStrip1.Opened += (sender, e) => hideCursorShow();
            muteToolStripMenuItem.Click += (sender, e) => MuteSet();

            nextPlayToolStripMenuItem.Click += async (sender, e) =>
            {
                var nextFile = await GetNextVideoFile(videoFileName, 1);
                if (nextFile != string.Empty)
                {
                    noChangeSizeVideoPlay(nextFile);
                }
            };
            beforePlayToolStripMenuItem.Click += async (sender, e) =>
            {
                var nextFile = await GetNextVideoFile(videoFileName, -1);
                if (nextFile != string.Empty)
                {
                    noChangeSizeVideoPlay(nextFile);
                }
            };

            if (args.Length > 1)
            {
                // 引数全調査
                Enumerable.Range(0, args.Length).ForEach((x, i) =>
                {
                    if (args[i] == "-volume")
                    {
                        i++;
                        try
                        {
                            initVolume = Convert.ToInt32(args[i]);
                        }
                        catch
                        {
                            initVolume = 100;
                        }
                    }
                    else if (args[i] == "-screensize")
                    {
                        screenSize = true;
                    }
                    else if (args[i] == "-nonevr")
                    {
                        forceEvr = false;
                    }
                    else
                    {
                        fileName = args[i];
                    }
                });
                if (forceEvr == false)
                {
                    fl.IsEvrPlay = false;
                }
                playVideo(fileName, screenSize);
                setTitleBar();
            }
        }

        /// <summary>
        /// ウィンドウメッセージ処理のオーバーライド
        /// </summary>
        /// <param name="msg"></param>
        protected override void WndProc(ref Message msg)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_SCREENSAVE = 0xF140;
            const int SC_MONITORPOWER = 0xF170;
            if (msg.Msg == WM_DSEvent)
            {
                fl.HandleEvent();
                msg.Result = IntPtr.Zero;
            }
            // Closeボタン（右上の赤背景に白い×ボタン）または、左ボックスのシステムメニューから閉じるを選択
            if (msg.Msg == WM_SYSCOMMAND && (int)msg.WParam == SC_CLOSE)
            {
                stopVideo();
                Close();
            }
            if (msg.Msg == WM_SYSCOMMAND && msg.WParam.ToInt32() == SC_SCREENSAVE)
                return;
            if (msg.Msg == WM_SYSCOMMAND && msg.WParam.ToInt32() == SC_MONITORPOWER)
                return;
            base.WndProc(ref msg);
        }

        /// <summary>
        /// ウィンドウの画面中央配置
        /// </summary>
        private void setCenterPosition()
        {
            int x = Screen.PrimaryScreen.WorkingArea.Width;
            int y = Screen.PrimaryScreen.WorkingArea.Height;
            x = (x - this.Size.Width) / 2;
            y = (y - this.Size.Height) / 2;
            this.Location = new Point(x, y);
        }

        /// <summary>
        /// 再生開始
        /// </summary>
        /// <param name="videoFile"></param>
        /// <param name="forceScreenSize"></param>
        private void playVideo(String videoFile, bool forceScreenSize = false)
        {
            bool isAudio, isVideo;
            long audioLength;
            fl.GetMediaContents(videoFile, out isVideo, out isAudio, out audioLength);
            //fl.BuildGraph(panel1.Handle, videoFile, isVideo);
            panel1.Visible = false;
            fl.BuildGraph(Handle, videoFile, isVideo);
            fl.RunGraph();
            // タイマーイベント登録
            timer1.Tick += (sender, e) =>
            {
                // 動画再生中にスリープさせないためのシステムへの通知を行う
                if (WindowState != FormWindowState.Minimized &&
                    fl.State == FilterState.Running)
                {
                    SetThreadExecutionState(ExecutionState.DisplayRequired);
                }
                setTitleBar();
            };
            // タイマー起動
            timer1.Start();
            if (hideMouse)
            {
                TraceDebug.WriteLine("play video -> cursor show");
                System.Windows.Forms.Cursor.Show();
                hideMouse = false;
            }

            videoFileName = videoFile;

            fl.Volume = initVolume;
            setVideoSize2ClientSize(forceScreenSize);
            setCenterPosition();
        }

        /// <summary>
        /// ビデオサイズから、画面からはみ出さないクライアントサイズの設定
        /// </summary>
        private void setVideoSize2ClientSize(bool force)
        {
            // ビデオサイズ取得
            int vWidth, vHeight;
            fl.GetVideoSize(out vWidth, out vHeight);

            // スクリーンプライマリワークサイズ取得
            int workWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int workHeight = Screen.PrimaryScreen.WorkingArea.Height;

            double aspectRatio = Convert.ToDouble(vHeight) / Convert.ToDouble(vWidth);
            // ウィンドウの横サイズとクライアントの横のサイズ差から、フレーム横のサイズ計を取得
            int frameSize = 0;//Size.Width - ClientSize.Width;

            // デフォルトはビデオサイズ
            int newWidth = vWidth;
            int newHeight = vHeight;

            // ビデオの横幅が画面からはみ出る
            if ((vWidth > (workWidth - frameSize)) || force)
            {
                newWidth = (workWidth - frameSize);
                newHeight = Convert.ToInt32(Convert.ToDouble((workWidth - frameSize)) * aspectRatio);
            }
            var newSize = new Size(newWidth, newHeight);
            this.Size = newSize;
            this.ClientSize = newSize;
            TraceDebug.WriteLine("Client Size=(" + ClientSize.Width + "," + ClientSize.Height + ")");
        }

        /// <summary>
        /// 再生または一時停止
        /// </summary>
        private void playOrPause()
        {
            if (!fl.Active) return;
            if (fl.State == FilterState.Running)
            {
                MuteSet(true);
                fl.PauseGraph();
            }
            else
            {
                MuteSet(false);
                fl.RunGraph();
            }
        }

        private void volumeUpDown(int volPlus)
        {
            if (!fl.Active) return;
            fl.Volume += volPlus;
        }

        public void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            int x, y;
            int newX, newY;
            fl.GetVideoSize(out x, out y);
            newX = (x / 2) - (contextMenuStrip1.Width / 2);
            newY = (y / 2) - (contextMenuStrip1.Height / 2);
            // CTRL+スペース＝右クリックメニューの表示
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control &&
                e.KeyCode == Keys.Space)
            {
                contextMenuStrip1.Show(this, new Point(newX, newY));
            }
            else if (e.KeyCode == Keys.Space)
            {
                playOrPause();
            }
            // U Key -> Volume up
            else if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift &&
                e.KeyCode == Keys.U)
            {
                volumeUpDown(+1);
                initVolume = fl.Volume;
            }
            else if (e.KeyCode == Keys.U)
            {
                volumeUpDown(+5);
                initVolume = fl.Volume;
            }
            // D Key -> Volume down
            else if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift &&
                e.KeyCode == Keys.D)
            {
                volumeUpDown(-1);
                initVolume = fl.Volume;
            }
            // D Key -> Volume down
            else if (e.KeyCode == Keys.D)
            {
                volumeUpDown(-5);
                initVolume = fl.Volume;
            }
            // P Key -> Video capture
            else if (e.KeyCode == Keys.P)
            {
                nowVideoSaveImage();
            }
            // F key -> Fast forward
            else if (e.KeyCode == Keys.F)
            {
                fl.Position += convSecondToUlong(5.0);
            }
            // W Key -> Rewind
            else if (e.KeyCode == Keys.W)
            {
                fl.Position -= convSecondToUlong(5.0);
            }
            else if (e.KeyCode == Keys.I)
            {
                setInfoWindow();
            }
            // J Key -> Most rewind
            else if (e.KeyCode == Keys.J)
            {
                fl.Position -= convSecondToUlong(15.0);
            }
            // K key -> Most fast forward
            else if (e.KeyCode == Keys.K)
            {
                fl.Position += convSecondToUlong(15.0);
            }
            // R Key -> reset
            else if (e.KeyCode == Keys.R)
            {
                resetPlay();
            }
            else if (e.KeyCode == Keys.O)
            {
                openVideoToolStripMenuItem_Click(null, null);
            }
            else if (e.KeyCode == Keys.Z)
            {
                zoomScaleChange();
            }
            // . key -> コマ送り
            else if (e.KeyCode == Keys.OemPeriod)
            {
                fl.Position += convSecondToUlong(0.03);
            }
            // , Key -> コマ戻し
            else if (e.KeyCode == Keys.Oemcomma)
            {
                fl.Position -= convSecondToUlong(0.03);
            }
            // Alt+Enter -> フルスクリーン切り替え
            // Ctrl+Enter -> フルスクリーン切り替え
            else if (((Control.ModifierKeys & Keys.Alt) == Keys.Alt &&
                e.KeyCode == Keys.Enter) ||
                     ((Control.ModifierKeys & Keys.Control) == Keys.Control &&
                e.KeyCode == Keys.Enter))
            {
                changeFullScreen();
            }
            else if (e.KeyCode == Keys.E)
            {
                videoSeek();
            }
            else if (e.KeyCode == Keys.S)
            {
                stopVideo();
            }
            else if (e.KeyCode == Keys.F2)
            {
                videoFileRename();
            }
            //else if (e.Shift == true && e.KeyCode == Keys.F9)
            //{
            //    //Top = 0;
            //    Left = 0;
            //}
            //else if (e.Control == true && e.KeyCode == Keys.F9)
            //{
            //    Top = 0;
            //    //Left = 0;
            //}
            else if (e.KeyCode == Keys.F8)
            {
                move2Side();
            }
            else if (e.KeyCode == Keys.F9)
            {
                //Top = 0;
                //Left = 0;
                move4Corner();
            }
            // Enter -> 再生
            else if ((Control.ModifierKeys & Keys.Alt) != Keys.Alt &&
                e.KeyCode == Keys.Enter)
            {
                //if (!fl.Active) return;
            if (fl.State == FilterState.Paused)
            {
                playOrPause();
            }
                else
                {
                    MuteOffSet();
                    fl.RunGraph();
                }
            }
            else if (e.KeyCode == Keys.End)
            {
                setCenterPosition();
            }
            // Alt+F4 -> 終了
            else if ((Control.ModifierKeys & Keys.Alt) == Keys.Alt &&
                e.KeyCode == Keys.F4)
            {
                stopVideo();
                Close();
            }
            // + -> スピードアップ 
            else if (e.KeyCode == Keys.Oemplus)
            {
                fl.SpeedUp();
            }
            // + -> スピードダウン
            else if (e.KeyCode == Keys.OemMinus)
            {
                fl.SpeedDown();
            }
        }

        private void move2Side()
        {
            int left = 0, top = 0;

            // 右上、右下移動用座標取得
            int workLeftCenter = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2;
            int workTopCenter = (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2;
            int workLeft = Screen.PrimaryScreen.WorkingArea.Width - this.Width;
            int workTop = Screen.PrimaryScreen.WorkingArea.Height - this.Height;

            // 次へ
            switch (toSide)
            {
                case 1:
                    toSide = 2;
                    break;
                case 2:
                    toSide = 3;
                    break;
                case 3:
                    toSide = 4;
                    break;
                case 4:
                    toSide = 1;
                    break;
                default:
                    toSide = 1;
                    break;
            }

            // 座標セット
            switch (toSide)
            {
                // 中央上
                case 1:
                    top = 0;
                    left = workLeftCenter;
                    break;
                // 左中央
                case 2:
                    top = workTopCenter;
                    left = workLeft;
                    break;
                // 中央下
                case 3:
                    top = workTop;
                    left = workLeftCenter;
                    break;
                // 右中央
                case 4:
                    top = workTopCenter;
                    left = 0;
                    break;
            }

            // ウィンドウの移動
            this.Top = top;
            this.Left = left;
        }

        private void move4Corner()
        {
            int left = 0, top = 0;

            // 右上、右下移動用座標取得
            int workLeft = Screen.PrimaryScreen.WorkingArea.Width - this.Width;
            int workTop = Screen.PrimaryScreen.WorkingArea.Height - this.Height;

            // 次へ
            switch (fourCorner)
            {
                case 1:
                    fourCorner = 2;
                    break;
                case 2:
                    fourCorner = 3;
                    break;
                case 3:
                    fourCorner = 4;
                    break;
                case 4:
                    fourCorner = 1;
                    break;
                default:
                    fourCorner = 1;
                    break;
            }

            // 座標セット
            switch (fourCorner)
            {
                // 左上
                case 1:
                    top = 0;
                    left = 0;
                    break;
                // 右上
                case 2:
                    top = 0;
                    left = workLeft;
                    break;
                // 右下
                case 3:
                    top = workTop;
                    left = workLeft;
                    break;
                // 左下
                case 4:
                    top = workTop;
                    left = 0;
                    break;
            }

            // ウィンドウの移動
            this.Top = top;
            this.Left = left;
        }

        private void setInfoWindow()
        {
            if (fm2 == null)
            {
                fm2 = new InfobarForm(this);
                fm2.Show();
                infoWindowMove();
                fm2.ClientSize = new Size(ClientSize.Width, fm2.ClientSize.Height);
                fm2.drawText = videoInfo;
                fm2.TopMost = true;
            }
            else
            {
                fm2.Close();
                fm2.Dispose();
                fm2 = null;
            }
        }

        private void nowVideoSaveImage()
        {
            String fileName = null;
            fileName = videoFileName;
            saveFileDialog1.FileName = "";
            SaveImageFiles(fileName);
        }

        /// <summary>
        /// JPEG/PSP THM File Making
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SaveImageFiles(String fileName)
        {
            bool pauseFlg = false;

            // No Video Open
            if (!fl.Active) return;

            // No Playing
            if (fl.State == FilterState.Stopped)
            {
                return;
            }

            hideCursorShow();

            // No Pause -> Pause
            if (fl.State != FilterState.Paused)
            {
                fl.PauseGraph();
            }
            else
            {
                pauseFlg = true;
            }

            if (string.IsNullOrWhiteSpace(saveFileDialog1.InitialDirectory))
            {
                saveFileDialog1.InitialDirectory = Path.GetDirectoryName(fileName);
            }
            if (string.IsNullOrWhiteSpace(savaFileNameStore))
            {
                String thmName = Path.GetFileName(Path.ChangeExtension(fileName, ".jpg"));
                saveFileDialog1.FilterIndex = 4;
                saveFileDialog1.FileName = Path.ChangeExtension(thmName, ".png");
            }
            else
            {
                string ext = Path.GetExtension(savaFileNameStore).ToLower();
                if (ext == ".jpg" || ext == ".jpeg" || ext == "*.jpe")
                {
                    saveFileDialog1.FilterIndex = 1;
                }
                if (ext == ".png")
                {
                    saveFileDialog1.FilterIndex = 4;
                }
                if (ext == ".thm")
                {
                    saveFileDialog1.FilterIndex = 2;
                }
                if (ext == ".bmp")
                {
                    saveFileDialog1.FilterIndex = 5;
                }
                saveFileDialog1.FileName = Path.GetFileName(savaFileNameStore);
            }

            saveFileDialog1.Filter = "JPEG Image File|*.jpg;*.jpeg;*.jpe|" +
                                     "PSP THM File|*.thm|" +
                                     "JPEG THM File|*.jpg;*.thm|" +
                                     "PNG File|*.png|" +
                                     "Windows Bitmap File|*.bmp";

            // Save OK
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                String saveFileName = saveFileDialog1.FileName;
                savaFileNameStore = saveFileName;

                Stream getBMPStream = null; ;
                Image snapImage = null;
                // Bitmapストリームをビデオから取得
                try
                {
                    fl.GetBitmap(out getBMPStream);
                    snapImage = Image.FromStream(getBMPStream);
                }
                catch { }
                // BitmapストリームをImage形式に変換
                String thmFileName = System.IO.Path.ChangeExtension(fileName, ".thm");

                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        // JPEGファイル保存の実行
                        JpegSaveImage(snapImage, ref saveFileName, JpegQuality);
                        break;
                    case 2:
                        // THMファイル保存の実行
                        exifImageSave(snapImage, thmFileName);
                        break;
                    case 3:
                        // THMファイル保存の実行
                        exifImageSave(snapImage, thmFileName);
                        // JPEG&THMファイル保存の実行
                        JpegSaveImage(snapImage, ref saveFileName, JpegQuality);
                        break;
                    case 4:
                        // PNGファイル保存の実行
                        pngSaveImage(snapImage, ref saveFileName, JpegQuality);
                        break;
                    case 5:
                        // BMPファイル保存の実行
                        bmpSaveImage(snapImage, ref saveFileName, JpegQuality);
                        break;
                }

                // イメージの解放
                snapImage.Dispose();

                // 次回のための設定
                saveFileDialog1.InitialDirectory = System.IO.Path.GetDirectoryName(saveFileDialog1.FileName);
                saveFileDialog1.FileName = System.IO.Path.GetFileName(saveFileDialog1.FileName);
            }

            mouseTimer.Enabled = true;

            // Play -> Pause -> Play
            if (pauseFlg == false)
            {
                fl.RunGraph();
            }
        }

        public void JpegSaveImage(Image img, ref string fileName, int quality)
        {
            pictureSaveImage(img, ref fileName, "image/jpeg", quality);
        }

        public void pngSaveImage(Image img, ref string fileName, int quality)
        {
            pictureSaveImage(img, ref fileName, "image/png", quality);
        }

        public void bmpSaveImage(Image img, ref string fileName, int quality)
        {
            pictureSaveImage(img, ref fileName, "image/bmp", quality);
        }

        /// <summary>
        /// 指定されたファイルを品質を指定してJPEGで保存する
        /// </summary>
        /// <param name="fileName">画像ファイル名</param>
        /// <param name="quality">品質</param>
        public void pictureSaveImage(Image img, ref string fileName, string mineType, int quality)
        {
            //EncoderParameterオブジェクトを1つ格納できる
            //EncoderParametersクラスの新しいインスタンスを初期化
            //ここでは品質のみ指定するため1つだけ用意する
            var eps = new EncoderParameters(1);
            //品質を指定
            var ep = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            //EncoderParametersにセットする
            eps.Param[0] = ep;

            //イメージエンコーダに関する情報を取得する
            var ici = mineType.GetEncoderInfo();

            //新しいファイルの拡張子を取得する
            string ext = ici.FilenameExtension.Split(';')[0];
            ext = System.IO.Path.GetExtension(ext).ToLower();

            //保存するファイル名を決定（拡張子を変える）
            string saveName = System.IO.Path.ChangeExtension(fileName, ext);
            try
            {
                //保存する
                img.Save(saveName, ici, eps);
            }
            catch { }
        }
        /// <summary>
        /// イメージのExif情報付きJpegファイル保存(PSPビデオ用サムネイル
        /// 画像用ファイルのため160x120固定)
        /// </summary>
        /// <param name="path">保存するファイル名</param>
        /// <param name="bmp">保存するイメージ</param>
        public void exifImageSave(Image bmp, string path)
        {
            string date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"); //"2009:01:01 00:00:00";

            // Exif情報を0から作成できないため(コンストラクタがない)、
            // リソースにあるExif情報付きJPEGファイルを開いて取得したExif情報を使用する
            Image exif = new Bitmap(this.GetType(), "exif.jpg");

            PropertyItem pi = exif.GetPropertyItem(0x9003);
            pi.Value = Encoding.ASCII.GetBytes(date + '\0');
            pi.Len = pi.Value.Length;

            // アスペクト比保持のための処理
            int imageX = bmp.Width;
            int imageY = bmp.Height;
            int thmX = 0;
            int thmY = 0;
            int newX = 0;
            int newY = 0;

            const double thmImageWidth = 160.0;
            const double thmImageHeight = 120.0;
            double dwImageWidth = Convert.ToDouble(imageX);
            double dwImageHeight = Convert.ToDouble(imageY);

            // 大きい方を固定値にする
            if (imageX > imageY)
            {
                thmX = 160;
                thmY = Convert.ToInt32(thmImageWidth / dwImageWidth * dwImageHeight);
                newX = 0;
                newY = Convert.ToInt32((thmImageHeight - Convert.ToDouble(thmY)) / 2.0);
            }
            else
            {
                thmX = Convert.ToInt32(thmImageHeight / dwImageHeight * dwImageWidth);
                thmY = 120;
                newX = Convert.ToInt32((thmImageWidth - Convert.ToDouble(thmX)) / 2.0);
                newY = 0;
            }

            // サムネイル画像作成
            Image img =
                bmp.GetThumbnailImage(thmX, thmY, null, IntPtr.Zero);
            // THM用固定サイズの画像作成
            Bitmap bmp2 = new Bitmap(160, 120);
            Graphics grp = Graphics.FromImage(bmp2);
            // 160x120の画像に、160 x nn or mm x 120のサムネイル画像を上書きコピー
            grp.DrawImage(img, newX, newY, thmX, thmY);
            img = bmp2;
            // Exif情報を付加してJPEGファイルとして保存
            img.SetPropertyItem(pi);
            img.Save(path, ImageFormat.Jpeg);
            img.Dispose();
        }

        /// <summary>
        /// タイトルバー文字列の作成設定
        /// </summary>
        private void setTitleBar()
        {
            if (!fl.Active)
            {
                Text = Path.GetFileNameWithoutExtension(Path.GetFileName(Application.ExecutablePath));
                return;
            }
            // ビデオサイズの取得
            int ww = 0;
            int wh = 0;

            try
            {
                fl.GetVideoSize(out ww, out wh);

                // 再生位置の時間文字列への変換
                int ii = (int)(fl.Position / 10000000);
                int h = ii / 3600;
                int m = (ii % 3600) / 60;
                int s = (ii % 3600) % 60;
                string now = String.Format("{0:D2}:{1:D2}:{2:D2}", h, m, s);

                // ビデオ長の時間文字列への変換
                ii = (int)(fl.StopPosition / 10000000);
                h = ii / 3600;
                m = (ii % 3600) / 60;
                s = (ii % 3600) % 60;
                string total = String.Format("{0:D2}:{1:D2}:{2:D2}", h, m, s);

                // ビデオ名 ビデオサイズ 再生時間 ビデオ長
                string ws = Path.GetFileName(videoFileName) + " (" + ww + " x " + wh + ") ";
                //ws += "→ (" + ClientSize.Width + " x " + ClientSize.Height + ") ";
                ws += now.ToString() + "/" + total.ToString();
                if (muteStatus)
                {
                    ws += "Volume:Mute";
                }
                else
                {
                    ws += " Volume:" + fl.Volume;
                }
                double rate;
                fl.GetRate(out rate);
                ws += " Speed:x" + rate;
                // 実行名を追加
                //Text = Path.GetFileNameWithoutExtension(Path.GetFileName(Application.ExecutablePath)) + " - " + ws;
                Text = ws;
                videoInfo = ws;
                if (fm2 != null)
                {
                    fm2.drawText = videoInfo;
                }
            }
            catch
            {
                Text = Path.GetFileNameWithoutExtension(Path.GetFileName(Application.ExecutablePath));
            }
        }

        /// <summary>
        /// 秒数のuLONG変換
        /// </summary>
        /// <param name="Sec"></param>
        /// <returns></returns>
        private ulong convSecondToUlong(double Sec)
        {
            return (Convert.ToUInt64(Sec * 10000000.0));
        }

        /// <summary>
        /// フルスクリーンチェンジ
        /// </summary>
        private void changeFullScreen()
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            }
            else
            {
                // この2つの順番によって、タスクバーが表示されるか隠れるかが変わるので重要！
                // non->max=タスクバー非表示／max->non=タスクバー表示
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Maximized;
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fl.StopGraph();
            fl.CloseInterfaces();
            Close();
        }

        private void fullScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            changeFullScreen();
            if (WindowState == FormWindowState.Maximized)
            {
                fullScreenToolStripMenuItem.Checked = true;
            }
            else
            {
                fullScreenToolStripMenuItem.Checked = false;
            }
        }

        /// <summary>
        /// 映像シーク
        /// </summary>
        private void videoSeek()
        {
            bool pauseFlag = false;
            timer1.Stop();
            if (!fl.Active) return;

            hideCursorShow();

            // No Pause -> Pause
            if (fl.State != FilterState.Paused)
            {
                fl.PauseGraph();
                pauseFlag = true;
            }

            var Form5 = new InputSeekTime();
            int ii = (int)(fl.Position / 10000000);
            int h = ii / 3600;
            int m = (ii % 3600) / 60;
            int s = (ii % 3600) % 60;
            Form5.SeekTime = new DateTime(2000, 1, 1, h, m, s);

            // ビデオ長の時間文字列への変換
            ii = (int)(fl.StopPosition / 10000000);
            h = ii / 3600;
            m = (ii % 3600) / 60;
            s = (ii % 3600) % 60;
            string total = String.Format("{0:D2}:{1:D2}:{2:D2}", h, m, s);
            Form5.EndTime = total;

            if (Form5.ShowDialog() == DialogResult.OK)
            {
                DateTime seekTime = Form5.SeekTime;
                fl.Position = Convert.ToUInt64(seekTime.Hour * 3600 + seekTime.Minute * 60 + seekTime.Second) * 10000000;
            }
            timer1.Start();
            if (pauseFlag == true)
            {
                fl.RunGraph();
            }
        }

        /// <summary>
        /// 映像停止
        /// </summary>
        private void stopVideo()
        {
            if (!fl.Active) return;
            fl.StopGraph();
            fl.Position = 0;
        }

        private void openVideoFile()
        {
            int currVol = fl.Volume;
            hideCursorShow();

            bool pauseFlg = false;
            if (fl.State == FilterState.Running)
            {
                fl.PauseGraph();
                pauseFlg = true;
            }

            openFileDialog1.FileName = Path.GetFileName(videoFileName);
            openFileDialog1.InitialDirectory = Path.GetDirectoryName(videoFileName);
            String extStr = System.IO.Path.GetExtension(videoFileName);
            String nowOpenFilter = null;
            if (!string.IsNullOrWhiteSpace(videoFileName))
            {
                nowOpenFilter = "Open File Extension|*" + extStr + "|";
            }
            openFileDialog1.Filter = nowOpenFilter +
                                     "Movie Files|*.mp4;*.mkv;*.mov;*.flv;*.mpg;*.avi;*.wmv|" +
                                     "Audio files|*.mp3;*.ac3;*.wav;*.wma|" +
                                     "All Files|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (fl.State != FilterState.Stopped)
                {
                    stopVideo();
                    fl.CloseInterfaces();
                }
                FileInfo fi = new FileInfo(openFileDialog1.FileName);

                playVideo(fi.FullName);
                if (currVol != -1)
                {
                    fl.Volume = currVol;
                }
            }
            else if (pauseFlg == true)
            {
                fl.RunGraph();
            }
        }

        /// <summary>
        /// サイズ変更なしでビデオ再生
        /// </summary>
        /// <param name="fileName"></param>
        private void noChangeSizeVideoPlay(string fileName)
        {
            bool sizeSave = false;
            Size clSize = new Size();
            if (fl.Active)
            {
                clSize = ClientSize;
                sizeSave = true;
            }
            stopVideo();
            playVideo(fileName, false);
            if (sizeSave)
            {
                ClientSize = clSize;
            }
            setCenterPosition();
            TraceDebug.WriteLine("Client Size=(" + ClientSize.Width + "," + ClientSize.Height + ")");
        }

        private void openVideoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool sizeSave = false;
            Size clSize = new Size();
            if (fl.Active)
            {
                clSize = ClientSize;
                sizeSave = true;
            }
            orginalVideoPlay();
            if (sizeSave)
            {
                ClientSize = clSize;
            }
            setCenterPosition();
            TraceDebug.WriteLine("Client Size=(" + ClientSize.Width + "," + ClientSize.Height + ")");
        }

        private void orginalVideoPlay()
        {
            Point loc = Location;
            formMoveTimer.Enabled = false;
            mouseTimer.Enabled = false;
            openVideoFile();
            mouseTimer.Enabled = true;
            formMoveTimer.Enabled = true;
            Location = loc;
            formMovePoint = loc;
            MuteOffSet();
        }

        private void changeClientFormSize(int newSizeX, int newSizeY)
        {
            if (WindowState != FormWindowState.Normal)
            {
                WindowState = FormWindowState.Normal;
            }
            ClientSize = new Size(newSizeX, newSizeY);
            setCenterPosition();
            TraceDebug.WriteLine("Client Size=(" + ClientSize.Width + "," + ClientSize.Height + ")");
            //setCenterPosition();
        }

        private void changeClientFormSize(double zoomScale)
        {
            int newSizeX = 0;
            int newSizeY = 0;
            double work;
            //動画のサイズの取得
            int width = 0;
            int height = 0;

            fl.GetVideoSize(out width, out height);

            Size videoSize = new Size(width, height);
            work = videoSize.Width;
            newSizeX = Convert.ToInt32(work * zoomScale);
            work = videoSize.Height;
            newSizeY = Convert.ToInt32(work * zoomScale);
            changeClientFormSize(newSizeX, newSizeY);
            nowZoomScale = zoomScale;
            if (FormBorderStyle == FormBorderStyle.None)
            {
                FormBorderStyle = FormBorderStyle.FixedSingle;
            }
        }

        public void zoomScaleChange()
        {
            bool pauseFlag = false;
            if (!fl.Active) return;

            if (fl.State == FilterState.Running)
            {
                fl.PauseGraph();
                pauseFlag = true;
            }

            hideCursorShow();

            ZoomScaleInputForm.ZoomScale = nowZoomScale;
            ZoomScaleInputForm.VideoSize = new Size(ClientSize.Width, ClientSize.Height);

            if (ZoomScaleInputForm.ShowDialog() == DialogResult.OK)
            {
                if (ZoomScaleInputForm.InputZoomScale)
                {
                    nowZoomScale = ZoomScaleInputForm.ZoomScale;
                    changeClientFormSize(nowZoomScale);
                }
                else
                {
                    changeClientFormSize(ZoomScaleInputForm.VideoSize.Width, ZoomScaleInputForm.VideoSize.Height);
                    setCenterPosition();
                }
            }

            if (pauseFlag == true)
            {
                fl.RunGraph();
            }
            mouseTimer.Enabled = true;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            playOrPause();
            hideCursorShow();
            var abox = new AboutBox();
            abox.ShowDialog();
            mouseTimer.Enabled = true;
            playOrPause();
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            var fileNames = e.Data.GetData(DataFormats.FileDrop, false) as string[];
            bool pauseFlg = false;
            int currVol = fl.Volume;
            Size clSize = ClientSize;
            Point loc = Location;
            if (fl.State == FilterState.Running)
            {
                fl.PauseGraph();
                pauseFlg = true;
            }
            if (fl.State != FilterState.Stopped)
            {
                stopVideo();
                fl.CloseInterfaces();
            }
            FileInfo fi = null;
            if (System.IO.File.Exists(fileNames[0]))
            {
                fi = new FileInfo(fileNames[0]);

                playVideo(fi.FullName);
                if (currVol != -1)
                {
                    fl.Volume = currVol;
                }
            }
            ClientSize = clSize;
            TraceDebug.WriteLine("Client Size=(" + ClientSize.Width + "," + ClientSize.Height + ")");
            Location = loc;

            MuteOffSet();
            if (pauseFlg == true)
            {
                fl.RunGraph();
            }
        }

        private void resetPlay()
        {
            bool pauseFlg = false;
            Size clSize = ClientSize;
            Point loc = Location;

            if (fl.State == FilterState.Running)
            {
                fl.PauseGraph();
                pauseFlg = true;
            }

            hideCursorShow();

            ulong videoPosition = fl.Position;
            //int currVol = fl.Volume;

            if (fl.State != FilterState.Stopped)
            {
                stopVideo();
                fl.CloseInterfaces();
            }

            playVideo(videoFileName);
            //if (currVol != -1)
            //{
            //    fl.Volume = currVol;
            fl.Volume = 80;
            //}
            ClientSize = clSize;
            TraceDebug.WriteLine("Client Size=(" + ClientSize.Width + "," + ClientSize.Height + ")");
            Location = loc;
            if (pauseFlg == true)
            {
                fl.RunGraph();
            }
            fl.Position = videoPosition;
            mouseTimer.Enabled = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
            if (fl.Active)
            {
                CloseFile();
                e.Cancel = true;
            }
        }
        private void CloseFile()
        {
            fl.StopGraph();
            fl.CloseInterfaces();
            Invalidate();
        }

        private void videoFileRename()
        {
            bool pauseFlg = false;

            // No Video Open
            if (!fl.Active) return;

            // No Playing
            if (fl.State == FilterState.Stopped)
            {
                return;
            }

            hideCursorShow();

            // No Pause -> Pause
            if (fl.State == FilterState.Running)
            {
                fl.PauseGraph();
                pauseFlg = true;
            }

            var renVideo = new videoRename();
            renVideo.OrgVideoName = Path.GetFileName(videoFileName);
            if (renVideo.ShowDialog() == DialogResult.OK)
            {
                if (MessageBox.Show(renVideo.NewVideoName,
                    "本当にこの名前に変更しますか",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1) == DialogResult.OK)
                {
                    try
                    {
                        timer1.Stop();
                        ulong currPos = fl.Position;
                        int currVol = fl.Volume;
                        Size clSize = ClientSize;
                        Point loc = Location;
                        stopVideo();
                        fl.CloseInterfaces();
                        FileInfo fi = new FileInfo(videoFileName.FileRename(renVideo.NewVideoName));
                        playVideo(fi.FullName);
                        videoFileName = fi.FullName;
                        changeClientFormSize(nowZoomScale);
                        fl.Position = currPos;
                        if (currVol != -1)
                        {
                            fl.Volume = currVol;
                        }
                        ClientSize = clSize;
                        TraceDebug.WriteLine("Client Size=(" + ClientSize.Width + "," + ClientSize.Height + ")");
                        Location = loc;
                        timer1.Start();
                    }
                    catch
                    {
                        MessageBox.Show("ファイルの変更ができませんでした。");
                    }
                }
            }

            // Play -> Pause -> Play
            if (pauseFlg == true && fl.State != FilterState.Running)
            {
                fl.RunGraph();
            }
            mouseTimer.Enabled = true;
        }

        public void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Location.X == MouseX) && (e.Location.Y == MouseY))
            {
                mouseTimer.Enabled = true;
            }
            else
            {
                MouseX = e.Location.X;
                MouseY = e.Location.Y;
                if (hideMouse)
                {
                    System.Windows.Forms.Cursor.Show();
                    TraceDebug.WriteLine("mouse move -> cursor show");
                    hideMouse = false;
                }
                mouseTimer.Enabled = false;
                mouseTimer.Enabled = true;
            }

            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                this.Left += e.X - mousePoint.X;
                this.Top += e.Y - mousePoint.Y;
                //または、つぎのようにする
                //this.Location = new Point(
                //    this.Location.X + e.X - mousePoint.X,
                //    this.Location.Y + e.Y - mousePoint.Y);
            }
        }

        public void Form1_MouseLeave(object sender, EventArgs e)
        {
            mouseTimer.Enabled = false;
            TraceDebug.WriteLine("mouse timer -> enaabled false");
        }

        public void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                //位置を記憶する
                mousePoint = new Point(e.X, e.Y);
            }
            if ((e.Button & MouseButtons.Right) == MouseButtons.Right)
            {
                // カーソル非表示対応のため、自動表示から表示コードで表示する方式に変更。
                // フォームの左上隅を基準としたマウスのX座標とY座標 (ピクセル単位) が
                // e.Locationに格納されるので、画面座標に変換してコンテキストメニューを表示する。
                Point screenLocation = PointToScreen(e.Location);
                contextMenuStrip1.Show(screenLocation);
            }
        }

        private void mouseTimer_Tick(object sender, EventArgs e)
        {
            if (!hideMouse)
            {
                TraceDebug.WriteLine("mouse timer -> cursor hide");
                System.Windows.Forms.Cursor.Hide();
                hideMouse = true;
            }
        }

        private void hideCursorShow()
        {
            if (hideMouse)
            {
                TraceDebug.WriteLine("hideCursorShow -> cursor show");
                System.Windows.Forms.Cursor.Show();
                hideMouse = false;
                mouseTimer.Enabled = false;
            }

        }

        private void contextMenuStrip1_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            // クローズ理由がアイテム選択以外の時、タイマー動作
            if (e.CloseReason != ToolStripDropDownCloseReason.ItemClicked)
            {
                mouseTimer.Enabled = true;
            }
        }

        private void Form1_Move(object sender, EventArgs e)
        {
            if (formMove)
            {
                formMove = false;
            }
            TraceDebug.WriteLine("Client Size=(" + ClientSize.Width + "," + ClientSize.Height + ")");
            infoWindowMove();
            TraceDebug.WriteLine("Move location=" + Location.X.ToString() + "," + Location.Y.ToString());
            TraceDebug.WriteLine("Client Size=(" + ClientSize.Width + "," + ClientSize.Height + ")");
        }

        private void formMoveTimer_Tick(object sender, EventArgs e)
        {
            formMove = true;
            formMoveTimer.Enabled = false;
            Location = formMovePoint;
            TraceDebug.WriteLine("formTimer location=" + Location.X.ToString() + "," + Location.Y.ToString());
        }

        private void infoWindowMove()
        {
            if (fm2 == null)
            {
                return;
            }
            Point loc = new Point(0, 0);
            //loc.Y = 0;
            Point lcPoint = PointToScreen(loc);
            fm2.Location = new Point(lcPoint.X, lcPoint.Y);
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            infoWindowMove();

            TraceDebug.WriteLine("Sender=" + sender + " Window Size=(" + Width + "," + Height +
                ") Client Size=(" + ClientSize.Width + "," + ClientSize.Height + ")");
        }

        private void volumeUPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            volumeUpDown(5);
            initVolume = fl.Volume;
        }

        private void volumeDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            volumeUpDown(-5);
            initVolume = fl.Volume;
        }

        private void enumFiltersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IBaseFilter[] filters;
            fl.EnumFilters(out filters);
            filters.ForEach((filter) => TraceDebug.WriteLine("Fiter=" + filter.ToString()));
        }

        private void openDVDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //現在のコンピュータの論理ドライブを取得
            var drives = System.IO.DriveInfo.GetDrives();
            var dvdInfoList = new List<DriveInfo>();
            drives.ForEach((drive) =>
            {
                if (drive.DriveType == DriveType.CDRom)
                {
                    dvdInfoList.Add(drive);
                }
            });
            var DvdInput = new DVDSelectForm();
            DvdInput.SetDvdDrive(dvdInfoList);
            if (DvdInput.ShowDialog() == DialogResult.OK)
            {
                object data;
                var SelectType = DvdInput.GetSelectDrive(out data);
                string DvdPath = null;
                switch (SelectType)
                {
                    case 0:
                        DvdPath = null;
                        break;
                    case 1:
                        DvdPath = (data as DriveInfo).Name;
                        break;
                    case 2:
                        DvdPath = data as string;
                        break;
                }
                if (!fl.BuildDvdGraph(DvdPath))
                {
                    MessageBox.Show("再生できませんでした!");
                }
            }
        }

        private void noFrameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (noFrameToolStripMenuItem.Checked)
            {
                FormBorderStyle = FormBorderStyle.None;
                Location = new Point(0, 0);
            }
            else
            {
                FormBorderStyle = FormBorderStyle.Sizable;
            }
        }

        /// <summary>
        /// ミュート設定
        /// </summary>
        /// <param name="mute"></param>
        public void MuteSet(bool mute)
        {
            if (mute)
            {
                nowVolume = fl.Volume;
                volumeUpDown(-fl.Volume);
                muteStatus = true;
            }
            else
            {
                fl.Volume = nowVolume;
                muteStatus = false;
            }
        }
        /// <summary>
        /// ミュートオフ
        /// </summary>
        public void MuteOffSet()
        {
            if (muteStatus)
            {
                fl.Volume = nowVolume;
                muteStatus = false;
            }
            else
            {
            }
        }
        /// <summary>
        /// ミュート設定トグル
        /// </summary>
        public void MuteSet()
        {
            if (!muteStatus)
            {
                nowVolume = fl.Volume;
                volumeUpDown(-fl.Volume);
                muteStatus = true;
            }
            else
            {
                fl.Volume = nowVolume;
                muteStatus = false;
            }
        }

        private void stopResumePlayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 停止
            if (stopResumePlayToolStripMenuItem.Checked)
            {
                pauseOnScreen();
                ResumePosition = fl.Position;
                fl.StopGraph();
            }
            // レジューム再生
            else
            {
                fl.Position = ResumePosition;
                fl.RunGraph();
            }
        }

        public void pauseOnScreen()
        {
            fl.PauseGraph();
            Stream getBMPStream = null; ;
            Image snapImage = null;
            // Bitmapストリームをビデオから取得
            try
            {
                fl.GetBitmap(out getBMPStream);
                snapImage = Image.FromStream(getBMPStream);
            }
            catch { }
            Thread.Sleep(1000);
            workForm = new Form();
            workForm.FormBorderStyle = FormBorderStyle.None;
            workForm.TopLevel = false;
            Controls.Add(workForm);
            workForm.Location = new Point(0, 0);
            workForm.ClientSize = ClientSize;
            workForm.BackgroundImage = snapImage;   
        }
    }
}