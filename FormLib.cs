using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DSInterfaces;
using Microsoft.Win32;
using System.Collections.Generic;


namespace firstdnet
{
    public class FormLib
    {
        const int NormalSpeed = 3;
        static double[] SpeedArray = { 0.25, 0.5, 0.75, 1.0, 1.25, 1.5, 2.0 };

        static readonly Guid MR_VIDEO_RENDER_SERVICE = new Guid(0x1092a86c, 0xab1a, 0x459a, 0xa3, 0x36, 0x83, 0x1f, 0xbc, 0x4d, 0x11, 0xff);
        static readonly Guid IID_IMFVideoDisplayControl = new Guid("a490b1e4-ab84-4d31-a1b2-181e03b1077a");

        private VideoForm form1 = null;
        private IFilterGraph2 graphBuilder = null;
        private IMediaControl mediaControl = null;
        private IMediaEventEx mediaEvent = null;
        private IBaseFilter baseFilter = null;
        private IMFVideoDisplayControl EVRCtrl = null;
        private IVMRWindowlessControl VMRCtrl = null;
        private IDvdGraphBuilder dvdGraphBuilder = null;
        private int currentSpeed = NormalSpeed;
        private IntPtr winHandle;

        public event EventHandler Complete;

        public bool IsEvrPlay { get; set; }

        public FormLib(VideoForm f)
        {
            form1 = f;
            AddHandlers();
            System.OperatingSystem os = System.Environment.OSVersion;
            IsEvrPlay = os.Version.Major > 5;
        }

        public void CloseInterfaces()
        {
            //mediaEvent = null;
            if (mediaControl != null)
                mediaControl.Stop();

            if (baseFilter != null)
            {
                Marshal.ReleaseComObject(baseFilter);
                baseFilter = null;
            }
            EVRCtrl = null;
            VMRCtrl = null;

            if (dvdGraphBuilder != null)
            {
                Marshal.ReleaseComObject(dvdGraphBuilder);
                dvdGraphBuilder = null;
            }

            if (graphBuilder != null)
            {
                Marshal.ReleaseComObject(graphBuilder);
                graphBuilder = null;
            }
            mediaControl = null;

        }

        private void AddHandlers()
        {
            // Add handlers for VMR purpose
            form1.Paint += new PaintEventHandler(Form1_Paint); // for WM_PAINT
            form1.Resize += new EventHandler(Form1_ResizeMove); // for WM_SIZE
            form1.Move += new EventHandler(Form1_ResizeMove); // for WM_MOVE
            SystemEvents.DisplaySettingsChanged += new EventHandler(SystemEvents_DisplaySettingsChanged); // for WM_DISPLAYCHANGE
        }

        private void RemoveHandlers()
        {
            // remove handlers when they are no more needed
            form1.Paint -= new PaintEventHandler(Form1_Paint);
            form1.Resize -= new EventHandler(Form1_ResizeMove);
            form1.Move -= new EventHandler(Form1_ResizeMove);
            SystemEvents.DisplaySettingsChanged -= new EventHandler(SystemEvents_DisplaySettingsChanged);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (EVRCtrl != null)
                {
                    int hr = EVRCtrl.RepaintVideo();
                }
                else if (VMRCtrl != null)
                {
                    IntPtr hdc = e.Graphics.GetHdc();
                    int hr = VMRCtrl.RepaintVideo(winHandle, hdc);
                    e.Graphics.ReleaseHdc(hdc);
                }
                else
                {

                }
            }
            catch { }
        }

        private void Form1_ResizeMove(object sender, EventArgs e)
        {
            try
            {
                Rectangle r = form1.ClientRectangle;
                if (EVRCtrl != null)
                {
                    int hr = EVRCtrl.SetVideoPosition(null, DsRect.FromRectangle(r));
                }
                else if (VMRCtrl != null)
                {
                    int hr = VMRCtrl.SetVideoPosition(null, DsRect.FromRectangle(r));
                }
                else
                {
                }
            }
            catch
            {
            }
        }

        private void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
        {
            if (EVRCtrl != null)
            {
            }
            else if (VMRCtrl != null)
            {
                int hr = VMRCtrl.DisplayModeChanged();
            }
        }

        public bool BuildDvdGraph(string fileName)
        {
            dvdGraphBuilder = (IDvdGraphBuilder)new DvdGraphBuilder();
            currentSpeed = NormalSpeed;
            int hr = 0;
            try
            {
                IGraphBuilder g;
                dvdGraphBuilder.GetFiltergraph(out g);
                graphBuilder = (IFilterGraph2)g;
                mediaControl = (IMediaControl)graphBuilder;
                mediaEvent = (IMediaEventEx)graphBuilder;
                mediaEvent.SetNotifyWindow(winHandle, VideoForm.WM_DSEvent, winHandle);
                ConfigureVMR();

                AMDvdRenderStatus status;
                hr = dvdGraphBuilder.RenderDvdVideoVolume(fileName, AMDvdGraphFlags.HWDecPrefer, out status);
                if (hr < 0)
                {
                    CloseInterfaces();
                    return false;
                }
            }
            catch
            {
                CloseInterfaces();
                return false;
            }
            return true;
        }

        public bool BuildGraph(IntPtr windowHandle, string fileName, bool isVideo)
        {
            currentSpeed = NormalSpeed;
            int hr = 0;
            try
            {
                graphBuilder = (IFilterGraph2)new FilterGraph();
                mediaControl = (IMediaControl)graphBuilder;
                mediaEvent = (IMediaEventEx)graphBuilder;
                winHandle = windowHandle;
                mediaEvent.SetNotifyWindow(winHandle, VideoForm.WM_DSEvent, winHandle);

                if (isVideo)
                {
                    if (IsEvrPlay)
                    {
                        ConfigureEVR();
                    }
                    else
                    {
                        ConfigureVMR();
                    }
                }
                hr = graphBuilder.RenderFile(fileName, null);
                DsError.ThrowExceptionForHR(hr);

            }
            catch (Exception e)
            {
                MessageBox.Show(fileName + "\n" + e.Message);
                return false;
            }
            return true;
        }


        private void ConfigureVMR()
        {
            baseFilter = (IBaseFilter)new VideoMixingRenderer();
            IVMRFilterConfig filterConfig = (IVMRFilterConfig)baseFilter;
            filterConfig.SetNumberOfStreams(1);
            filterConfig.SetRenderingMode(VMRMode.Windowless);
            VMRCtrl = (IVMRWindowlessControl)baseFilter;
            VMRCtrl.SetVideoClippingWindow(winHandle);
            VMRCtrl.SetAspectRatioMode(VMRAspectRatioMode.LetterBox);
            Form1_ResizeMove(null, null);
            graphBuilder.AddFilter(baseFilter, "Video Mixing Renderer");
        }

        private void ConfigureEVR()
        {
            baseFilter = (IBaseFilter)new EnhancedVideoRenderer();
            IEVRFilterConfig filterConfig = (IEVRFilterConfig)baseFilter;
            filterConfig.SetNumberOfStreams(1);
            object o;
            ((IMFGetService)baseFilter).GetService(MR_VIDEO_RENDER_SERVICE, IID_IMFVideoDisplayControl, out o);
            EVRCtrl = (IMFVideoDisplayControl)o;
            EVRCtrl.SetVideoWindow(winHandle);
            EVRCtrl.SetAspectRatioMode(1);
            Form1_ResizeMove(null, null);
            graphBuilder.AddFilter(baseFilter, "Enhanced Video Renderer");
        }

        public void RunGraph()
        {
            if (mediaControl != null)
            {
                int hr = mediaControl.Run();
                DsError.ThrowExceptionForHR(hr);
            }
        }

        public void PauseGraph()
        {
            if (mediaControl != null)
            {
                int hr = mediaControl.Pause();
                DsError.ThrowExceptionForHR(hr);
            }
        }

        public void StopGraph()
        {
            if (mediaControl != null)
            {
                int hr = mediaControl.Stop();
                DsError.ThrowExceptionForHR(hr);
            }
        }

        public bool Active { get { return graphBuilder != null; } }

        unsafe public bool GetBitmap(out Stream stream)
        {
            BITMAPINFOHEADER* pBIH;
            BITMAPINFOHEADER BIH;
            BITMAPFILEHEADER BFH;
            IntPtr P1;
            int hr;
            uint PaletteSize, bufferSize, dibSize;
            ulong i64 = 0;
            stream = null;

            if (VMRCtrl != null)
            {
                form1.Hide();
                hr = VMRCtrl.GetCurrentImage(out P1);
                if (hr != 0) return false;
                pBIH = (BITMAPINFOHEADER*)P1;
                BFH.bfType = 0x4d42;
                BFH.bfSize = (uint)sizeof(BITMAPFILEHEADER) + pBIH->biSize + pBIH->biSizeImage + pBIH->biClrUsed * (uint)sizeof(RGBQUAD);
                BFH.bfReserved1 = 0;
                BFH.bfReserved2 = 0;
                if ((pBIH->biClrUsed == 0) && (pBIH->biBitCount <= 8))
                {
                    PaletteSize = ((uint)1 << pBIH->biBitCount) * (uint)sizeof(RGBQUAD);

                }
                else
                {
                    PaletteSize = pBIH->biClrUsed * (uint)sizeof(RGBQUAD);
                }
                BFH.bfOffBits = (uint)sizeof(BITMAPFILEHEADER) + pBIH->biSize + PaletteSize;
                bufferSize = (uint)sizeof(BITMAPFILEHEADER) + BFH.bfSize;
                stream = new MemoryStream();
                for (uint i = 0; i < (uint)sizeof(BITMAPFILEHEADER); i++)
                {
                    stream.WriteByte(*((byte*)((IntPtr)((uint)(&BFH) + i))));
                }
                for (uint i = 0; i < BFH.bfSize; i++)
                {
                    stream.WriteByte(*((byte*)((IntPtr)((uint)P1 + i))));
                }
                stream.Position = 0;
                Marshal.FreeCoTaskMem(P1);
                form1.Show();
                return true;
            }
            if (EVRCtrl != null)
            {
                form1.Hide();
                int x, y;
                GetVideoSize(out x, out y);
                Rectangle r = new Rectangle(0, 0, x, y);
                EVRCtrl.SetVideoPosition(null, DsRect.FromRectangle(r));
                BIH = new BITMAPINFOHEADER();
                BIH.biSize = (uint)sizeof(BITMAPINFOHEADER);
                pBIH = &BIH;
                hr = EVRCtrl.GetCurrentImage(((IntPtr)pBIH), out P1, out dibSize, ref i64);
                if (hr != 0)
                {
                    Form1_ResizeMove(null, null);
                    return false;
                }
                BFH.bfType = 0x4d42;
                BFH.bfSize = (uint)sizeof(BITMAPFILEHEADER) + BIH.biSize + BIH.biSizeImage + BIH.biClrUsed * (uint)sizeof(RGBQUAD);
                BFH.bfReserved1 = 0;
                BFH.bfReserved2 = 0;
                if ((BIH.biClrUsed == 0) && (BIH.biBitCount <= 8))
                {
                    PaletteSize = ((uint)1 << BIH.biBitCount) * (uint)sizeof(RGBQUAD);

                }
                else
                {
                    PaletteSize = BIH.biClrUsed * (uint)sizeof(RGBQUAD);
                }
                BFH.bfOffBits = (uint)sizeof(BITMAPFILEHEADER) + BIH.biSize + PaletteSize;
                bufferSize = (uint)sizeof(BITMAPFILEHEADER) + BFH.bfSize;
                stream = new MemoryStream();
#if UNSAFE
                for (uint i = 0; i < (uint)sizeof(BITMAPFILEHEADER); i++)
                {
                    stream.WriteByte(*((byte*)((IntPtr)((uint)(&BFH) + i))));
                }
                for (uint i = 0; i < (uint)sizeof(BITMAPINFOHEADER); i++)
                {
                    stream.WriteByte(*((byte*)((IntPtr)((uint)(&BIH) + i))));
                }
                for (uint i = 0; i < dibSize; i++)
                {
                    stream.WriteByte(*((byte*)((IntPtr)((uint)P1 + i))));
                }
#else
                byte[] fileHeader = new byte[Marshal.SizeOf(typeof(BITMAPFILEHEADER))];
                byte[] infoHeader = new byte[Marshal.SizeOf(typeof(BITMAPINFOHEADER))];
                byte[] bmpImg = new byte[dibSize];
                Marshal.Copy(fileHeader, 0, (IntPtr)((uint)(&BFH)), fileHeader.Length);
                stream.Write(fileHeader, 0, fileHeader.Length);
                Marshal.Copy(infoHeader, 0, (IntPtr)((uint)(&BFH)), infoHeader.Length);
                stream.Write(infoHeader, 0, infoHeader.Length);
                Marshal.Copy(bmpImg, 0, (IntPtr)((uint)P1), (int)dibSize);
                stream.Write(bmpImg, 0, (int)dibSize);
#endif
                stream.Position = 0;
                Marshal.FreeCoTaskMem(P1);
                Form1_ResizeMove(null, null);
                form1.Show();
                return true;
            }
            return false;
        }

        public ulong Duration
        {
            get
            {
                if (graphBuilder == null) return 0;
                long result;
                ((IMediaSeeking)graphBuilder).GetDuration(out result);
                return (ulong)result;
            }
        }

        public ulong Position
        {
            get
            {
                if (graphBuilder == null) return 0;
                long result;
                ((IMediaSeeking)graphBuilder).GetCurrentPosition(out result);
                return (ulong)result;
            }
            set
            {
                if (graphBuilder == null) return;
                ((IMediaSeeking)graphBuilder).SetPositions(DsLong.FromInt64((long)value), AMSeekingSeekingFlags.AbsolutePositioning,
                    DsLong.FromInt64((long)StopPosition), AMSeekingSeekingFlags.NoPositioning);
            }
        }

        public ulong StopPosition
        {
            get
            {
                if (graphBuilder == null) return 0;
                long result;
                ((IMediaSeeking)graphBuilder).GetStopPosition(out result);
                return (ulong)result;
            }
        }

        public FilterState State
        {
            get
            {
                if (mediaControl == null) return 0;
                FilterState result;
                mediaControl.GetState(0, out result);
                return result;
            }
        }

        public void GetVideoSize(out int videoWidth, out int videoHeight)
        {
            if (VMRCtrl != null)
            {
                int i1, i2;
                VMRCtrl.GetNativeVideoSize(out videoWidth, out videoHeight, out i1, out  i2);
                return;
            }
            if (EVRCtrl != null)
            {
                try
                {
                    Size s1, s2;
                    EVRCtrl.GetNativeVideoSize(out s1, out s2);
                    videoWidth = s1.Width;
                    videoHeight = s1.Height;
                    if (videoWidth == 1440 && (videoHeight == 1080 || videoHeight == 1088))
                    {
                        videoWidth = 1920;
                    }
                    if (videoWidth == 2880 && (videoHeight == 2160))
                    {
                        videoWidth = 1920;
                    }
                    return;
                }
                catch { }//オーディオファイルでエラーが出る
            }
            videoWidth = 800;
            videoHeight = 400;

        }

        public void HandleEvent()
        {
            if (mediaEvent == null) return;
            EventCode e = 0;
            IntPtr P1 = IntPtr.Zero, P2 = IntPtr.Zero;
            int hr = -1; 
            try
            {
                hr = mediaEvent.GetEvent(out e, out P1, out P2, 0);
            }
            catch
            {
                Application.ExitThread();
            }
            while (hr == 0)
            {
                switch (e)
                {
                    case EventCode.Complete:
                        Complete(this, EventArgs.Empty);
                        break;
                }
                try
                {
                    mediaEvent.FreeEventParams(e, P1, P2);
                    hr = mediaEvent.GetEvent(out e, out P1, out P2, 0);
                }
                catch
                {
                    break;
                }
            }
        }

        private bool SetRate(double rate)
        {
            if (graphBuilder == null) return false;
            return ((IMediaSeeking)graphBuilder).SetRate(rate) == 0;
        }

        public bool GetRate(out double rate)
        {
            rate = 0;
            if (graphBuilder == null) return false;
            return ((IMediaSeeking)graphBuilder).GetRate(out rate) == 0;
        }

        public int Volume
        {
            //set
            //{
            //    if (graphBuilder == null) return;
            //    IBasicAudio ba = ((IBasicAudio)graphBuilder);
            //    int i = System.Math.Min(100, System.Math.Max(0, value));
            //    if (i == 0)
            //    {
            //        ba.put_Volume(-10000);
            //    }
            //    else
            //    {
            //        ba.put_Volume(-((100 - i) * (100 - i) / 2));
            //    }
            //}

            get
            {
                if (graphBuilder == null) return -1;
                IBasicAudio ba = ((IBasicAudio)graphBuilder);
                try
                {
                    int vol;
                    ba.get_Volume(out vol);

                    int volget = Math.Abs(100 - Math.Abs(vol) / 100);
                    return volget;
                }
                catch
                {
                    return -1;
                }
            }
            set
            {
                if (graphBuilder == null) return;
                IBasicAudio ba = ((IBasicAudio)graphBuilder);
                int volumeSet = (10000 - value * 100) * -1;
                ba.put_Volume( volumeSet );
            }
        }  
      
        public void SpeedUp()
        {
            if (currentSpeed == SpeedArray.Length - 1) return;
            if (SetRate(SpeedArray[currentSpeed + 1])) currentSpeed++;
        }

        public void SpeedDown()
        {
            if (currentSpeed == 0) return;
            if (SetRate(SpeedArray[currentSpeed - 1])) currentSpeed--;
        }

        public bool EnumFilters(out IBaseFilter[] filters)
        {
            var al = new List<IBaseFilter>();
            filters = new IBaseFilter[0];
            if (graphBuilder == null) return false;
            IEnumFilters f;
            if (graphBuilder.EnumFilters(out f) == 0)
            {
                IBaseFilter[] filter = new IBaseFilter[1];
                while (f.Next(1, filter, (IntPtr)0) == 0)
                {
                    al.Add(filter[0]);
                }
                Marshal.ReleaseComObject(f);
                f = null;
                filters = (IBaseFilter[])al.ToArray();
                return true;
            }

            return false;
        }

        public void GetMediaContents(string fileName, out bool video, out bool audio, out long audioLength)
        {
            video = false;
            audio = false;
            audioLength = 0;
            if (fileName == "") return;
            IMediaDet mediaDet = null;
            AMMediaType mt = new AMMediaType();
            IFilterGraph2 fg = (IFilterGraph2)new FilterGraph();
            try
            {
                string ext = Path.GetExtension(fileName);
                switch (ext.ToLower())
                {
                    case ".asx":
                    case ".wax":
                    case ".wvx":
                    //case ".m3u":
                        return;
                }
                try
                {
                    RegistryKey regkey = Registry.ClassesRoot.OpenSubKey(ext);
                    switch (((string)regkey.GetValue("PerceivedType", "")).ToLower())
                    {
                        case "audio":
                            audio = true;
                            break;
                        default://推定ビデオ
                        //case "video":
                            video = true;
                            break;
                        //default: return;
                    }
                    regkey.Close();
                }
                catch { }

                mediaDet = (IMediaDet)new MediaDet();
                mediaDet.put_Filename(fileName);
                int streams;
                mediaDet.get_OutputStreams(out streams);
                for (int i = 0; i < streams; i++)
                {
                    mediaDet.put_CurrentStream(i);
                    mediaDet.get_StreamMediaType(mt);
                    video = video | (mt.majorType == MediaType.AnalogVideo) | (mt.majorType == MediaType.Video);
                    audio = audio | (mt.majorType == MediaType.AnalogAudio) | (mt.majorType == MediaType.Audio) | (mt.majorType == MediaType.Midi);
                }
                //if ((!video) && ((uint)mediaDet.EnterBitmapGrabMode(0.0) != 0x80040200)) video = true;
                if (audio&&(!video)&&(fg.RenderFile(fileName, null) == 0))
                {
                    ((IMediaSeeking)fg).GetStopPosition(out audioLength);
                }
            }
            catch { }
            finally
            {
                if (mediaDet != null) Marshal.ReleaseComObject(mediaDet);
                if (fg != null) Marshal.ReleaseComObject(fg);
            }
        }

        public string GetMediaInfoString(string fileName)
        {
            string result = "";
            if (fileName == "") return result;
            result += fileName + "\n";
            //if (graphBuilder == null) return result;
            IMediaDet mediaDet = null;
            AMMediaType mt = new AMMediaType();
            try
            {
                mediaDet = (IMediaDet)new MediaDet();
                mediaDet.put_Filename(fileName);
                int streams;
                mediaDet.get_OutputStreams(out streams);
                for (int i = 0; i < streams; i++)
                {
                    mediaDet.put_CurrentStream(i);
                    mediaDet.get_StreamMediaType(mt);
                    result += GetMediaTypeString(mt) + "\n";
                }
            }
            catch { }
            finally
            {
                if (mediaDet != null) Marshal.ReleaseComObject(mediaDet);
            }
            return result;
        }

        unsafe private string GetMediaTypeString(AMMediaType mt)
        {
            string result = "";
            try
            {
                if (mt.majorType == MediaType.AnalogAudio) result += "AnalogAudio";
                else if (mt.majorType == MediaType.AnalogVideo) result += "AnalogVideo";
                else if (mt.majorType == MediaType.Audio) result += "Audio";
                else if (mt.majorType == MediaType.AuxLine21Data) result += "AuxLine21Data";
                else if (mt.majorType == MediaType.DTVCCData) result += "DTVCCData";
                else if (mt.majorType == MediaType.File) result += "File";
                else if (mt.majorType == MediaType.Interleaved) result += "Interleaved";
                else if (mt.majorType == MediaType.LMRT) result += "LMRT";
                else if (mt.majorType == MediaType.Midi) result += "Midi";
                else if (mt.majorType == MediaType.Mpeg2Sections) result += "Mpeg2Sections";
                else if (mt.majorType == MediaType.MSTVCaption) result += "MSTVCaption";
                else if (mt.majorType == MediaType.ScriptCommand) result += "ScriptCommand";
                else if (mt.majorType == MediaType.Stream) result += "Stream";
                else if (mt.majorType == MediaType.Text) result += "Text";
                else if (mt.majorType == MediaType.Timecode) result += "Timecode";
                else if (mt.majorType == MediaType.URLStream) result += "URLStream";
                else if (mt.majorType == MediaType.VBI) result += "VBI";
                else if (mt.majorType == MediaType.Video) result += "Video";
                else result += mt.majorType.ToString();//"UnKnown";
                result += ": ";
                if (mt.subType == MediaSubType.A2B10G10R10) result += "A2B10G10R10";
                else if (mt.subType == MediaSubType.A2R10G10B10) result += "A2R10G10B10";
                else if (mt.subType == MediaSubType.AI44) result += "AI44";
                else if (mt.subType == MediaSubType.AIFF) result += "AIFF";
                else if (mt.subType == MediaSubType.AnalogVideo_NTSC_M) result += "AnalogVideo_NTSC_M";
                else if (mt.subType == MediaSubType.AnalogVideo_PAL_B) result += "AnalogVideo_PAL_B";
                else if (mt.subType == MediaSubType.AnalogVideo_PAL_D) result += "AnalogVideo_PAL_D";
                else if (mt.subType == MediaSubType.AnalogVideo_PAL_G) result += "AnalogVideo_PAL_G";
                else if (mt.subType == MediaSubType.AnalogVideo_PAL_H) result += "AnalogVideo_PAL_H";
                else if (mt.subType == MediaSubType.AnalogVideo_PAL_I) result += "AnalogVideo_PAL_I";
                else if (mt.subType == MediaSubType.AnalogVideo_PAL_M) result += "AnalogVideo_PAL_M";
                else if (mt.subType == MediaSubType.AnalogVideo_PAL_N) result += "AnalogVideo_PAL_N";
                else if (mt.subType == MediaSubType.AnalogVideo_PAL_N_COMBO) result += "AnalogVideo_PAL_N_COMBO";
                else if (mt.subType == MediaSubType.AnalogVideo_SECAM_B) result += "AnalogVideo_SECAM_B";
                else if (mt.subType == MediaSubType.AnalogVideo_SECAM_D) result += "AnalogVideo_SECAM_D";
                else if (mt.subType == MediaSubType.AnalogVideo_SECAM_G) result += "AnalogVideo_SECAM_G";
                else if (mt.subType == MediaSubType.AnalogVideo_SECAM_H) result += "AnalogVideo_SECAM_H";
                else if (mt.subType == MediaSubType.AnalogVideo_SECAM_K) result += "AnalogVideo_SECAM_K";
                else if (mt.subType == MediaSubType.AnalogVideo_SECAM_K1) result += "AnalogVideo_SECAM_K1";
                else if (mt.subType == MediaSubType.AnalogVideo_SECAM_L) result += "AnalogVideo_SECAM_L";
                else if (mt.subType == MediaSubType.ARGB1555) result += "ARGB1555";
                else if (mt.subType == MediaSubType.ARGB1555_D3D_DX7_RT) result += "ARGB1555_D3D_DX7_RT";
                else if (mt.subType == MediaSubType.ARGB1555_D3D_DX9_RT) result += "ARGB1555_D3D_DX9_RT";
                else if (mt.subType == MediaSubType.ARGB32) result += "ARGB32";
                else if (mt.subType == MediaSubType.ARGB32_D3D_DX7_RT) result += "ARGB32_D3D_DX7_RT";
                else if (mt.subType == MediaSubType.ARGB32_D3D_DX9_RT) result += "ARGB32_D3D_DX9_RT";
                else if (mt.subType == MediaSubType.ARGB4444) result += "ARGB4444";
                else if (mt.subType == MediaSubType.ARGB4444_D3D_DX7_RT) result += "ARGB4444_D3D_DX7_RT";
                else if (mt.subType == MediaSubType.ARGB4444_D3D_DX9_RT) result += "ARGB4444_D3D_DX9_RT";
                else if (mt.subType == MediaSubType.Asf) result += "Asf";
                else if (mt.subType == MediaSubType.AtscSI) result += "AtscSI";
                else if (mt.subType == MediaSubType.AU) result += "AU";
                else if (mt.subType == MediaSubType.Avi) result += "Avi";
                else if (mt.subType == MediaSubType.AYUV) result += "AYUV";
                else if (mt.subType == MediaSubType.CFCC) result += "CFCC";
                else if (mt.subType == MediaSubType.CLJR) result += "CLJR";
                else if (mt.subType == MediaSubType.CLPL) result += "CLPL";
                else if (mt.subType == MediaSubType.CPLA) result += "CPLA";
                else if (mt.subType == MediaSubType.Data708_608) result += "Data708_608";
                else if (mt.subType == MediaSubType.DOLBY_AC3_SPDIF) result += "DOLBY_AC3_SPDIF";
                else if (mt.subType == MediaSubType.DolbyAC3) result += "DolbyAC3";
                else if (mt.subType == MediaSubType.DRM_Audio) result += "DRM_Audio";
                else if (mt.subType == MediaSubType.DssAudio) result += "DssAudio";
                else if (mt.subType == MediaSubType.DssVideo) result += "DssVideo";
                else if (mt.subType == MediaSubType.DtvCcData) result += "DtvCcData";
                else if (mt.subType == MediaSubType.dv25) result += "dv25";
                else if (mt.subType == MediaSubType.dv50) result += "dv50";
                else if (mt.subType == MediaSubType.DvbSI) result += "DvbSI";
                else if (mt.subType == MediaSubType.DVCS) result += "DVCS";
                else if (mt.subType == MediaSubType.dvh1) result += "dvh1";
                else if (mt.subType == MediaSubType.dvhd) result += "dvhd";
                else if (mt.subType == MediaSubType.DVSD) result += "DVSD";
                else if (mt.subType == MediaSubType.dvsl) result += "dvsl";
                else if (mt.subType == MediaSubType.H264) result += "H264";
                else if (mt.subType == MediaSubType.I420) result += "I420";
                else if (mt.subType == MediaSubType.IA44) result += "IA44";
                else if (mt.subType == MediaSubType.IEEE_FLOAT) result += "IEEE_FLOAT";
                else if (mt.subType == MediaSubType.IF09) result += "IF09";
                else if (mt.subType == MediaSubType.IJPG) result += "IJPG";
                else if (mt.subType == MediaSubType.IMC1) result += "IMC1";
                else if (mt.subType == MediaSubType.IMC2) result += "IMC2";
                else if (mt.subType == MediaSubType.IMC3) result += "IMC3";
                else if (mt.subType == MediaSubType.IMC4) result += "IMC4";
                else if (mt.subType == MediaSubType.IYUV) result += "IYUV";
                else if (mt.subType == MediaSubType.Line21_BytePair) result += "Line21_BytePair";
                else if (mt.subType == MediaSubType.Line21_GOPPacket) result += "Line21_GOPPacket";
                else if (mt.subType == MediaSubType.Line21_VBIRawData) result += "Line21_VBIRawData";
                else if (mt.subType == MediaSubType.MDVF) result += "MDVF";
                else if (mt.subType == MediaSubType.MJPG) result += "MJPG";
                else if (mt.subType == MediaSubType.MPEG1Audio) result += "MPEG1Audio";
                else if (mt.subType == MediaSubType.MPEG1AudioPayload) result += "MPEG1AudioPayload";
                else if (mt.subType == MediaSubType.MPEG1Packet) result += "MPEG1Packet";
                else if (mt.subType == MediaSubType.MPEG1Payload) result += "MPEG1Payload";
                else if (mt.subType == MediaSubType.MPEG1System) result += "MPEG1System";
                else if (mt.subType == MediaSubType.MPEG1SystemStream) result += "MPEG1SystemStream";
                else if (mt.subType == MediaSubType.MPEG1Video) result += "MPEG1Video";
                else if (mt.subType == MediaSubType.MPEG1VideoCD) result += "MPEG1VideoCD";
                else if (mt.subType == MediaSubType.Mpeg2Audio) result += "Mpeg2Audio";
                else if (mt.subType == MediaSubType.Mpeg2Data) result += "Mpeg2Data";
                else if (mt.subType == MediaSubType.Mpeg2Program) result += "Mpeg2Program";
                else if (mt.subType == MediaSubType.Mpeg2Transport) result += "Mpeg2Transport";
                else if (mt.subType == MediaSubType.Mpeg2TransportStride) result += "Mpeg2TransportStride";
                else if (mt.subType == MediaSubType.Mpeg2Video) result += "Mpeg2Video";
                else if (mt.subType == MediaSubType.None) result += "None";
                else if (mt.subType == MediaSubType.NV12) result += "NV12";
                else if (mt.subType == MediaSubType.NV24) result += "NV24";
                else if (mt.subType == MediaSubType.Overlay) result += "Overlay";
                else if (mt.subType == MediaSubType.PCM) result += "PCM";
                else if (mt.subType == MediaSubType.PCMAudio_Obsolete) result += "PCMAudio_Obsolete";
                else if (mt.subType == MediaSubType.PLUM) result += "PLUM";
                else if (mt.subType == MediaSubType.QTJpeg) result += "QTJpeg";
                else if (mt.subType == MediaSubType.QTMovie) result += "QTMovie";
                else if (mt.subType == MediaSubType.QTRle) result += "QTRle";
                else if (mt.subType == MediaSubType.QTRpza) result += "QTRpza";
                else if (mt.subType == MediaSubType.QTSmc) result += "QTSmc";
                else if (mt.subType == MediaSubType.RAW_SPORT) result += "RAW_SPORT";
                else if (mt.subType == MediaSubType.RGB1) result += "RGB1";
                else if (mt.subType == MediaSubType.RGB16_D3D_DX7_RT) result += "RGB16_D3D_DX7_RT";
                else if (mt.subType == MediaSubType.RGB16_D3D_DX9_RT) result += "RGB16_D3D_DX9_RT";
                else if (mt.subType == MediaSubType.RGB24) result += "RGB24";
                else if (mt.subType == MediaSubType.RGB32) result += "RGB32";
                else if (mt.subType == MediaSubType.RGB32_D3D_DX7_RT) result += "RGB32_D3D_DX7_RT";
                else if (mt.subType == MediaSubType.RGB32_D3D_DX9_RT) result += "RGB32_D3D_DX9_RT";
                else if (mt.subType == MediaSubType.RGB4) result += "RGB4";
                else if (mt.subType == MediaSubType.RGB555) result += "RGB555";
                else if (mt.subType == MediaSubType.RGB565) result += "RGB565";
                else if (mt.subType == MediaSubType.RGB8) result += "RGB8";
                else if (mt.subType == MediaSubType.S340) result += "S340";
                else if (mt.subType == MediaSubType.S342) result += "S342";
                else if (mt.subType == MediaSubType.SPDIF_TAG_241h) result += "SPDIF_TAG_241h";
                else if (mt.subType == MediaSubType.TELETEXT) result += "TELETEXT";
                else if (mt.subType == MediaSubType.TVMJ) result += "TVMJ";
                else if (mt.subType == MediaSubType.UYVY) result += "UYVY";
                else if (mt.subType == MediaSubType.VideoImage) result += "VideoImage";
                else if (mt.subType == MediaSubType.VPS) result += "VPS";
                else if (mt.subType == MediaSubType.VPVBI) result += "VPVBI";
                else if (mt.subType == MediaSubType.VPVideo) result += "VPVideo";
                else if (mt.subType == MediaSubType.WAKE) result += "WAKE";
                else if (mt.subType == MediaSubType.WAVE) result += "WAVE";
                else if (mt.subType == MediaSubType.WebStream) result += "WebStream";
                else if (mt.subType == MediaSubType.WSS) result += "WSS";
                else if (mt.subType == MediaSubType.Y211) result += "Y211";
                else if (mt.subType == MediaSubType.Y411) result += "Y411";
                else if (mt.subType == MediaSubType.Y41P) result += "Y41P";
                else if (mt.subType == MediaSubType.YUY2) result += "YUY2";
                else if (mt.subType == MediaSubType.YUYV) result += "YUYV";
                else if (mt.subType == MediaSubType.YV12) result += "YV12";
                else if (mt.subType == MediaSubType.YVU9) result += "YVU9";
                else if (mt.subType == MediaSubType.YVYU) result += "YVYU";
                else if (mt.subType == MediaSubType.MS_Mpeg4) result += "MS_Mpeg4";
                else if (mt.subType == MediaSubType.Divx) result += "Divx";
                else if (mt.subType == MediaSubType.Divx5) result += "Divx5";
                else if (mt.subType == MediaSubType.VoxWare) result += "VoxWare";
                else if (mt.subType == MediaSubType.Mpeg_Layer3) result += "Mpeg_Layer3";
                else if (mt.subType == MediaSubType.LPCM) result += "LPCM";
                else if (mt.subType == MediaSubType.Vorbis) result += "Vorbis";
                else result += mt.subType.ToString();//"UnKnown";
                result += ": ";
                if (mt.formatType == FormatType.VideoInfo)
                {
                    BITMAPINFOHEADER BIH = ((VideoInfoHeader*)mt.formatPtr)->BmiHeader;
                    result += string.Format("{0}x{1}, {2}bits ", BIH.biWidth, BIH.biHeight, BIH.biBitCount);
                }
                else if (mt.formatType == FormatType.VideoInfo2)
                {
                    BITMAPINFOHEADER BIH = ((VideoInfoHeader2*)mt.formatPtr)->BmiHeader;
                    result += string.Format("{0}x{1}, {2}bits ", BIH.biWidth, BIH.biHeight, BIH.biBitCount);
                }
                else if (mt.formatType == FormatType.WaveEx)
                {
                    WaveFormatEx* pWFE = (WaveFormatEx*)mt.formatPtr;
                    switch (pWFE->wFormatTag)
                    {
                        case 0x0001: result += "PCM"; break; // common 
                        case 0x0002: result += "ADPCM"; break;
                        case 0x0003: result += "IEEE_FLOAT"; break;
                        case 0x0005: result += "IBM_CVSD"; break;
                        case 0x0006: result += "ALAW"; break;
                        case 0x0007: result += "MULAW"; break;
                        case 0x0010: result += "OKI_ADPCM"; break;
                        case 0x0011: result += "DVI_ADPCM"; break;
                        case 0x0012: result += "MEDIASPACE_ADPCM"; break;
                        case 0x0013: result += "SIERRA_ADPCM"; break;
                        case 0x0014: result += "G723_ADPCM"; break;
                        case 0x0015: result += "DIGISTD"; break;
                        case 0x0016: result += "DIGIFIX"; break;
                        case 0x0017: result += "DIALOGIC_OKI_ADPCM"; break;
                        case 0x0018: result += "MEDIAVISION_ADPCM"; break;
                        case 0x0020: result += "YAMAHA_ADPCM"; break;
                        case 0x0021: result += "SONARC"; break;
                        case 0x0022: result += "DSPGROUP_TRUESPEECH"; break;
                        case 0x0023: result += "ECHOSC1"; break;
                        case 0x0024: result += "AUDIOFILE_AF36"; break;
                        case 0x0025: result += "APTX"; break;
                        case 0x0026: result += "AUDIOFILE_AF10"; break;
                        case 0x0030: result += "DOLBY_AC2"; break;
                        case 0x0031: result += "GSM610"; break;
                        case 0x0032: result += "MSNAUDIO"; break;
                        case 0x0033: result += "ANTEX_ADPCME"; break;
                        case 0x0034: result += "CONTROL_RES_VQLPC"; break;
                        case 0x0035: result += "DIGIREAL"; break;
                        case 0x0036: result += "DIGIADPCM"; break;
                        case 0x0037: result += "CONTROL_RES_CR10"; break;
                        case 0x0038: result += "NMS_VBXADPCM"; break;
                        case 0x0039: result += "CS_IMAADPCM"; break;
                        case 0x003A: result += "ECHOSC3"; break;
                        case 0x003B: result += "ROCKWELL_ADPCM"; break;
                        case 0x003C: result += "ROCKWELL_DIGITALK"; break;
                        case 0x003D: result += "XEBEC"; break;
                        case 0x0040: result += "G721_ADPCM"; break;
                        case 0x0041: result += "G728_CELP"; break;
                        case 0x0050: result += "MPEG"; break;
                        case 0x0055: result += "MPEGLAYER3"; break;
                        case 0x0060: result += "CIRRUS"; break;
                        case 0x0061: result += "ESPCM"; break;
                        case 0x0062: result += "VOXWARE"; break;
                        case 0x0063: result += "CANOPUS_ATRAC"; break;
                        case 0x0064: result += "G726_ADPCM"; break;
                        case 0x0065: result += "G722_ADPCM"; break;
                        case 0x0066: result += "DSAT"; break;
                        case 0x0067: result += "DSAT_DISPLAY"; break;
                        case 0x0075: result += "VOXWARE"; break;// aditionnal  ??? 
                        case 0x0080: result += "SOFTSOUND"; break;
                        case 0x0100: result += "RHETOREX_ADPCM"; break;
                        case 0x0200: result += "CREATIVE_ADPCM"; break;
                        case 0x0202: result += "CREATIVE_FASTSPEECH8"; break;
                        case 0x0203: result += "CREATIVE_FASTSPEECH10"; break;
                        case 0x0220: result += "QUARTERDECK"; break;
                        case 0x0300: result += "FM_TOWNS_SND"; break;
                        case 0x0400: result += "BTV_DIGITAL"; break;
                        case 0x1000: result += "OLIGSM"; break;
                        case 0x1001: result += "OLIADPCM"; break;
                        case 0x1002: result += "OLICELP"; break;
                        case 0x1003: result += "OLISBC"; break;
                        case 0x1004: result += "OLIOPR"; break;
                        case 0x1100: result += "LH_CODEC"; break;
                        case 0x1400: result += "NORRIS"; break;
                        default: result += pWFE->wFormatTag.ToString("X"); break;
                    }
                    result += string.Format(",{0}Hz,{1}Ch ", pWFE->nSamplesPerSec, pWFE->nChannels);
                }
                else if (mt.formatType == FormatType.MpegVideo)
                {
                    BITMAPINFOHEADER BIH = ((MPEG1VideoInfo*)mt.formatPtr)->hdr.BmiHeader;
                    result += string.Format("{0}x{1}, {2}bits ", BIH.biWidth, BIH.biHeight, BIH.biBitCount);
                }
                else if (mt.formatType == FormatType.Mpeg2Video)
                {
                    BITMAPINFOHEADER BIH = ((MPEG2VideoInfo*)mt.formatPtr)->hdr.BmiHeader;
                    result += string.Format("{0}x{1}, {2}bits ", BIH.biWidth, BIH.biHeight, BIH.biBitCount);
                }
                else if (mt.formatType == FormatType.DvInfo) result += "DVInfo";
                else if (mt.formatType == FormatType.MpegStreams) result += "MPEGStreams";
                else if (mt.formatType == FormatType.DolbyAC3) result += "DolbyAC3";
                else if (mt.formatType == FormatType.Mpeg2Audio) result += "MPEG2Audio";
                else if (mt.formatType == FormatType.DVD_LPCMAudio) result += "DVD_LPCMAudio";
                else result += "Unknown";
            }
            catch { }
            finally { }

            return result;
        }

        public void setFullScreenMode(bool flag)
        {
            if (IsEvrPlay)
            {
                EVRCtrl.SetFullscreen(flag);
            }
        }
    }
}
