// パクリ元の明記
// DirectShowLib Version 2.0
// http://directshownet.sourceforge.net
// Licensed under Lesser General Public License.

using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace DSInterfaces
{
    //[ComImport, Guid(0xFCC152B7,0xF372,0x11D0,0x8E,0x00,0x00,0xC0,0x4F,0xD7,0xC0,0x8B)]
    [ComImport, Guid("FCC152B7-F372-11d0-8E00-00C04FD7C08B")]
    public class DvdGraphBuilder
    {
    }

    [ComImport, Guid("B87BEB7B-8D29-423f-AE4D-6582C10175AC")]
    public class VideoMixingRenderer
    {
    }

    [ComImport, Guid("e436ebb3-524f-11ce-9f53-0020af0ba770")]
    public class FilterGraph
    {
    }

    [ComImport, Guid("FA10746C-9B63-4b6c-BC49-FC300EA5F256")]
    public class EnhancedVideoRenderer
    {
    }

    [ComImport, Guid("65BD0711-24D2-4FF7-9324-ED2E5D3ABAFA")]
    public class MediaDet
    {
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
Guid("FCC152B6-F372-11d0-8E00-00C04FD7C08B"),
InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDvdGraphBuilder
    {
        [PreserveSig]
        int GetFiltergraph([Out] out IGraphBuilder ppGB);

        [PreserveSig]
        int GetDvdInterface(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid,
            [Out, MarshalAs(UnmanagedType.Interface)] out object ppvIF
            );

        [PreserveSig]
        int RenderDvdVideoVolume(
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpcwszPathName,
            [In] AMDvdGraphFlags dwFlags,
            [Out] out AMDvdRenderStatus pStatus
            );
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
Guid("56a8689a-0ad4-11ce-b03a-0020af0ba770"),
InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMediaSample
    {
        [PreserveSig]
        int GetPointer([Out] out IntPtr ppBuffer); // BYTE **

        [PreserveSig]
        int GetSize();

        [PreserveSig]
        int GetTime(
            [Out] out long pTimeStart,
            [Out] out long pTimeEnd
            );

        [PreserveSig]
        int SetTime(
            [In, MarshalAs(UnmanagedType.LPStruct)] DsLong pTimeStart,
            [In, MarshalAs(UnmanagedType.LPStruct)] DsLong pTimeEnd
            );

        [PreserveSig]
        int IsSyncPoint();

        [PreserveSig]
        int SetSyncPoint([In, MarshalAs(UnmanagedType.Bool)] bool bIsSyncPoint);

        [PreserveSig]
        int IsPreroll();

        [PreserveSig]
        int SetPreroll([In, MarshalAs(UnmanagedType.Bool)] bool bIsPreroll);

        [PreserveSig]
        int GetActualDataLength();

        [PreserveSig]
        int SetActualDataLength([In] int len);

        /// <summary>
        /// Returned object must be released with DsUtils.FreeAMMediaType()
        /// </summary>
        [PreserveSig]
        int GetMediaType([Out, MarshalAs(UnmanagedType.LPStruct)] out AMMediaType ppMediaType);

        [PreserveSig]
        int SetMediaType([In, MarshalAs(UnmanagedType.LPStruct)] AMMediaType pMediaType);

        [PreserveSig]
        int IsDiscontinuity();

        [PreserveSig]
        int SetDiscontinuity([In, MarshalAs(UnmanagedType.Bool)] bool bDiscontinuity);

        [PreserveSig]
        int GetMediaTime(
            [Out] out long pTimeStart,
            [Out] out long pTimeEnd
            );

        [PreserveSig]
        int SetMediaTime(
            [In, MarshalAs(UnmanagedType.LPStruct)] DsLong pTimeStart,
            [In, MarshalAs(UnmanagedType.LPStruct)] DsLong pTimeEnd
            );
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
Guid("6B652FFF-11FE-4fce-92AD-0266B5D7C78F"),
InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ISampleGrabber
    {
        [PreserveSig]
        int SetOneShot(
            [In, MarshalAs(UnmanagedType.Bool)] bool OneShot);

        [PreserveSig]
        int SetMediaType(
            [In, MarshalAs(UnmanagedType.LPStruct)] AMMediaType pmt);

        [PreserveSig]
        int GetConnectedMediaType(
            [Out, MarshalAs(UnmanagedType.LPStruct)] AMMediaType pmt);

        [PreserveSig]
        int SetBufferSamples(
            [In, MarshalAs(UnmanagedType.Bool)] bool BufferThem);

        [PreserveSig]
        int GetCurrentBuffer(ref int pBufferSize, IntPtr pBuffer);

        [PreserveSig]
        int GetCurrentSample(out IMediaSample ppSample);

        [PreserveSig]
        int SetCallback(ISampleGrabberCB pCallback, int WhichMethodToCallback);
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
Guid("0579154A-2B53-4994-B0D0-E773148EFF85"),
InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ISampleGrabberCB
    {
        /// <summary>
        /// When called, callee must release pSample
        /// </summary>
        [PreserveSig]
        int SampleCB(double SampleTime, IMediaSample pSample);

        [PreserveSig]
        int BufferCB(double SampleTime, IntPtr pBuffer, int BufferLen);
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
Guid("65BD0710-24D2-4FF7-9324-ED2E5D3ABAFA")]
    public interface IMediaDet
    {
        [PreserveSig]
        int get_Filter(
            [MarshalAs(UnmanagedType.IUnknown)] out object pVal
            );

        [PreserveSig]
        int put_Filter(
            [MarshalAs(UnmanagedType.IUnknown)] object newVal
            );

        [PreserveSig]
        int get_OutputStreams(
            out int pVal
            );

        [PreserveSig]
        int get_CurrentStream(
            out int pVal
            );

        [PreserveSig]
        int put_CurrentStream(
            int newVal
            );

        [PreserveSig]
        int get_StreamType(
            out Guid pVal
            );

        [PreserveSig]
        int get_StreamTypeB(
            [MarshalAs(UnmanagedType.BStr)] out string pVal
            );

        [PreserveSig]
        int get_StreamLength(
            out double pVal
            );

        [PreserveSig]
        int get_Filename(
            [MarshalAs(UnmanagedType.BStr)] out string pVal
            );

        [PreserveSig]
        int put_Filename(
            [MarshalAs(UnmanagedType.BStr)] string newVal
            );

        [PreserveSig]
        int GetBitmapBits(
            double StreamTime,
            out int pBufferSize,
            [In] IntPtr pBuffer,
            int Width,
            int Height
            );

        [PreserveSig]
        int WriteBitmapBits(
            double StreamTime,
            int Width,
            int Height,
            [In, MarshalAs(UnmanagedType.BStr)] string Filename);

        [PreserveSig]
        int get_StreamMediaType(
            [Out, MarshalAs(UnmanagedType.LPStruct)] AMMediaType pVal);

        [PreserveSig]
        int GetSampleGrabber(
            out ISampleGrabber ppVal);

        [PreserveSig]
        int get_FrameRate(
            out double pVal);

        [PreserveSig]
        int EnterBitmapGrabMode(
            double SeekTime);
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
Guid("B196B28B-BAB4-101A-B69C-00AA00341D07"),
InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ISpecifyPropertyPages
    {
        [PreserveSig]
        int GetPages(out DsCAUUID pPages);
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
Guid("56a868b3-0ad4-11ce-b03a-0020af0ba770"),
InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IBasicAudio
    {
        [PreserveSig]
        int put_Volume([In] int lVolume);

        [PreserveSig]
        int get_Volume([Out] out int plVolume);

        [PreserveSig]
        int put_Balance([In] int lBalance);

        [PreserveSig]
        int get_Balance([Out] out int plBalance);
    }


    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
Guid("36b73880-c2c8-11cf-8b46-00805f6cef60"),
InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMediaSeeking
    {
        [PreserveSig]
        int GetCapabilities([Out] out AMSeekingSeekingCapabilities pCapabilities);

        [PreserveSig]
        int CheckCapabilities([In, Out] ref AMSeekingSeekingCapabilities pCapabilities);

        [PreserveSig]
        int IsFormatSupported([In, MarshalAs(UnmanagedType.LPStruct)] Guid pFormat);

        [PreserveSig]
        int QueryPreferredFormat([Out] out Guid pFormat);

        [PreserveSig]
        int GetTimeFormat([Out] out Guid pFormat);

        [PreserveSig]
        int IsUsingTimeFormat([In, MarshalAs(UnmanagedType.LPStruct)] Guid pFormat);

        [PreserveSig]
        int SetTimeFormat([In, MarshalAs(UnmanagedType.LPStruct)] Guid pFormat);

        [PreserveSig]
        int GetDuration([Out] out long pDuration);

        [PreserveSig]
        int GetStopPosition([Out] out long pStop);

        [PreserveSig]
        int GetCurrentPosition([Out] out long pCurrent);

        [PreserveSig]
        int ConvertTimeFormat(
            [Out] out long pTarget,
            [In, MarshalAs(UnmanagedType.LPStruct)] DsGuid pTargetFormat,
            [In] long Source,
            [In, MarshalAs(UnmanagedType.LPStruct)] DsGuid pSourceFormat
            );

        [PreserveSig]
        int SetPositions(
            [In, Out, MarshalAs(UnmanagedType.LPStruct)] DsLong pCurrent,
            [In] AMSeekingSeekingFlags dwCurrentFlags,
            [In, Out, MarshalAs(UnmanagedType.LPStruct)] DsLong pStop,
            [In] AMSeekingSeekingFlags dwStopFlags
            );

        [PreserveSig]
        int GetPositions(
            [Out] out long pCurrent,
            [Out] out long pStop
            );

        [PreserveSig]
        int GetAvailable(
            [Out] out long pEarliest,
            [Out] out long pLatest
            );

        [PreserveSig]
        int SetRate([In] double dRate);

        [PreserveSig]
        int GetRate([Out] out double pdRate);

        [PreserveSig]
        int GetPreroll([Out] out long pllPreroll);
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
Guid("7a4fb5af-479f-4074-bb40-ce6722e43c82"),
InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IVMRImageCompositor
    {
        [PreserveSig]
        int InitCompositionTarget(
            [In] IntPtr pD3DDevice,
            [In] IntPtr pddsRenderTarget
            );

        [PreserveSig]
        int TermCompositionTarget(
            [In] IntPtr pD3DDevice,
            [In] IntPtr pddsRenderTarget
            );

        [PreserveSig]
        int SetStreamMediaType(
            [In] int dwStrmID,
            [In] AMMediaType pmt,
            [In, MarshalAs(UnmanagedType.Bool)] bool fTexture
            );

        [PreserveSig]
        int CompositeImage(
            [In] IntPtr pD3DDevice,
            [In] IntPtr pddsRenderTarget,
            [In] AMMediaType pmtRenderTarget,
            [In] long rtStart,
            [In] long rtEnd,
            [In] int dwClrBkGnd,
            [In, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Struct, SizeParamIndex = 7)] VMRVideoStreamInfo[] pVideoStreamInfo,
            [In] int cStreams
            );
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
Guid("9e5530c5-7034-48b4-bb46-0b8a6efc8e36"),
InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IVMRFilterConfig
    {
        [PreserveSig]
        int SetImageCompositor([In] IVMRImageCompositor lpVMRImgCompositor);

        [PreserveSig]
        int SetNumberOfStreams([In] int dwMaxStreams);

        [PreserveSig]
        int GetNumberOfStreams([Out] out int pdwMaxStreams);

        [PreserveSig]
        int SetRenderingPrefs([In] VMRRenderPrefs dwRenderFlags);

        [PreserveSig]
        int GetRenderingPrefs([Out] out VMRRenderPrefs pdwRenderFlags);

        [PreserveSig]
        int SetRenderingMode([In] VMRMode Mode);

        [PreserveSig]
        int GetRenderingMode([Out] out VMRMode Mode);
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
Guid("0eb1088c-4dcd-46f0-878f-39dae86a51b7"),
InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IVMRWindowlessControl
    {
        [PreserveSig]
        int GetNativeVideoSize(
            [Out] out int lpWidth,
            [Out] out int lpHeight,
            [Out] out int lpARWidth,
            [Out] out int lpARHeight
            );

        [PreserveSig]
        int GetMinIdealVideoSize(
            [Out] out int lpWidth,
            [Out] out int lpHeight
            );

        [PreserveSig]
        int GetMaxIdealVideoSize(
            [Out] out int lpWidth,
            [Out] out int lpHeight
            );

        [PreserveSig]
        int SetVideoPosition(
            [In] DsRect lpSRCRect,
            [In] DsRect lpDSTRect
            );

        [PreserveSig]
        int GetVideoPosition(
            [Out] DsRect lpSRCRect,
            [Out] DsRect lpDSTRect
            );

        [PreserveSig]
        int GetAspectRatioMode([Out] out VMRAspectRatioMode lpAspectRatioMode);

        [PreserveSig]
        int SetAspectRatioMode([In] VMRAspectRatioMode AspectRatioMode);

        [PreserveSig]
        int SetVideoClippingWindow([In] IntPtr hwnd); // HWND

        [PreserveSig]
        int RepaintVideo(
            [In] IntPtr hwnd, // HWND
            [In] IntPtr hdc // HDC
            );

        [PreserveSig]
        int DisplayModeChanged();

        /// <summary>
        /// the caller is responsible for free the returned memory by calling CoTaskMemFree.
        /// </summary>
        [PreserveSig]
        int GetCurrentImage([Out] out IntPtr lpDib); // BYTE**

        [PreserveSig]
        int SetBorderColor([In] int Clr);

        [PreserveSig]
        int GetBorderColor([Out] out int lpClr);

        [PreserveSig]
        int SetColorKey([In] int Clr);

        [PreserveSig]
        int GetColorKey([Out] out int lpClr);
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
Guid("56a868b6-0ad4-11ce-b03a-0020af0ba770"),
InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMediaEvent
    {
        [PreserveSig]
        int GetEventHandle([Out] out IntPtr hEvent); // HEVENT

        [PreserveSig]
        int GetEvent(
            [Out] out EventCode lEventCode,
            [Out] out IntPtr lParam1,
            [Out] out IntPtr lParam2,
            [In] int msTimeout
            );

        [PreserveSig]
        int WaitForCompletion(
            [In] int msTimeout,
            [Out] out EventCode pEvCode
            );

        [PreserveSig]
        int CancelDefaultHandling([In] EventCode lEvCode);

        [PreserveSig]
        int RestoreDefaultHandling([In] EventCode lEvCode);

        [PreserveSig]
        int FreeEventParams(
            [In] EventCode lEvCode,
            [In] IntPtr lParam1,
            [In] IntPtr lParam2
            );
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
Guid("56a868c0-0ad4-11ce-b03a-0020af0ba770"),
InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMediaEventEx : IMediaEvent
    {
        #region IMediaEvent Methods

        [PreserveSig]
        new int GetEventHandle([Out] out IntPtr hEvent); // HEVENT

        [PreserveSig]
        new int GetEvent(
            [Out] out EventCode lEventCode,
            [Out] out IntPtr lParam1,
            [Out] out IntPtr lParam2,
            [In] int msTimeout
            );

        [PreserveSig]
        new int WaitForCompletion(
            [In] int msTimeout,
            [Out] out EventCode pEvCode
            );

        [PreserveSig]
        new int CancelDefaultHandling([In] EventCode lEvCode);

        [PreserveSig]
        new int RestoreDefaultHandling([In] EventCode lEvCode);

        [PreserveSig]
        new int FreeEventParams(
            [In] EventCode lEvCode,
            [In] IntPtr lParam1,
            [In] IntPtr lParam2
            );

        #endregion

        [PreserveSig]
        int SetNotifyWindow(
            [In] IntPtr hwnd, // HWND *
            [In] int lMsg,
            [In] IntPtr lInstanceData // PVOID
            );

        [PreserveSig]
        int SetNotifyFlags([In] NotifyFlags lNoNotifyFlags);

        [PreserveSig]
        int GetNotifyFlags([Out] out NotifyFlags lplNoNotifyFlags);
    }


    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
Guid("56a868b1-0ad4-11ce-b03a-0020af0ba770"),
InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMediaControl
    {
        [PreserveSig]
        int Run();

        [PreserveSig]
        int Pause();

        [PreserveSig]
        int Stop();

        [PreserveSig]
        int GetState([In] int msTimeout, [Out] out FilterState pfs);

        [PreserveSig]
        int RenderFile([In, MarshalAs(UnmanagedType.BStr)] string strFilename);

        [PreserveSig,
        Obsolete("Automation interface, for pre-.NET VB.  Use IGraphBuilder::AddSourceFilter instead", false)]
        int AddSourceFilter(
            [In, MarshalAs(UnmanagedType.BStr)] string strFilename,
            [Out, MarshalAs(UnmanagedType.IDispatch)] out object ppUnk
            );

        [PreserveSig,
        Obsolete("Automation interface, for pre-.NET VB.  Use IFilterGraph::EnumFilters instead", false)]
        int get_FilterCollection([Out, MarshalAs(UnmanagedType.IDispatch)] out object ppUnk);

        [PreserveSig,
        Obsolete("Automation interface, for pre-.NET VB.  Use IFilterMapper2::EnumMatchingFilters instead", false)]
        int get_RegFilterCollection([Out, MarshalAs(UnmanagedType.IDispatch)] out object ppUnk);

        [PreserveSig]
        int StopWhenReady();
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
Guid("56a86893-0ad4-11ce-b03a-0020af0ba770"),
InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumFilters
    {
        [PreserveSig]
        int Next(
            [In] int cFilters,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] IBaseFilter[] ppFilter,
            [In] IntPtr pcFetched
            );

        [PreserveSig]
        int Skip([In] int cFilters);

        [PreserveSig]
        int Reset();

        [PreserveSig]
        int Clone([Out] out IEnumFilters ppEnum);
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
Guid("89c31040-846b-11ce-97d3-00aa0055595a"),
InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumMediaTypes
    {
        [PreserveSig]
        int Next(
            [In] int cMediaTypes,
            [In, Out, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(EMTMarshaler), SizeParamIndex = 0)] AMMediaType[] ppMediaTypes,
            [In] IntPtr pcFetched
            );

        [PreserveSig]
        int Skip([In] int cMediaTypes);

        [PreserveSig]
        int Reset();

        [PreserveSig]
        int Clone([Out] out IEnumMediaTypes ppEnum);
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
Guid("56a86891-0ad4-11ce-b03a-0020af0ba770"),
InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPin
    {
        [PreserveSig]
        int Connect(
            [In] IPin pReceivePin,
            [In, MarshalAs(UnmanagedType.LPStruct)] AMMediaType pmt
            );

        [PreserveSig]
        int ReceiveConnection(
            [In] IPin pReceivePin,
            [In, MarshalAs(UnmanagedType.LPStruct)] AMMediaType pmt
            );

        [PreserveSig]
        int Disconnect();

        [PreserveSig]
        int ConnectedTo(
            [Out] out IPin ppPin);

        /// <summary>
        /// Release returned parameter with DsUtils.FreeAMMediaType
        /// </summary>
        [PreserveSig]
        int ConnectionMediaType(
            [Out, MarshalAs(UnmanagedType.LPStruct)] AMMediaType pmt);

        /// <summary>
        /// Release returned parameter with DsUtils.FreePinInfo
        /// </summary>
        [PreserveSig]
        int QueryPinInfo([Out] out PinInfo pInfo);

        [PreserveSig]
        int QueryDirection(out PinDirection pPinDir);

        [PreserveSig]
        int QueryId([Out, MarshalAs(UnmanagedType.LPWStr)] out string Id);

        [PreserveSig]
        int QueryAccept([In, MarshalAs(UnmanagedType.LPStruct)] AMMediaType pmt);

        [PreserveSig]
        int EnumMediaTypes([Out] out IEnumMediaTypes ppEnum);

        [PreserveSig]
        int QueryInternalConnections(
            [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] IPin[] ppPins,
            [In, Out] ref int nPin
            );

        [PreserveSig]
        int EndOfStream();

        [PreserveSig]
        int BeginFlush();

        [PreserveSig]
        int EndFlush();

        [PreserveSig]
        int NewSegment(
            [In] long tStart,
            [In] long tStop,
            [In] double dRate
            );
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
Guid("56a86892-0ad4-11ce-b03a-0020af0ba770"),
InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumPins
    {
        [PreserveSig]
        int Next(
            [In] int cPins,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] IPin[] ppPins,
            [In] IntPtr pcFetched
            );

        [PreserveSig]
        int Skip([In] int cPins);

        [PreserveSig]
        int Reset();

        [PreserveSig]
        int Clone([Out] out IEnumPins ppEnum);
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
Guid("56a86897-0ad4-11ce-b03a-0020af0ba770"),
InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IReferenceClock
    {
        [PreserveSig]
        int GetTime([Out] out long pTime);

        [PreserveSig]
        int AdviseTime(
            [In] long baseTime,
            [In] long streamTime,
            [In] IntPtr hEvent, // System.Threading.WaitHandle?
            [Out] out int pdwAdviseCookie
            );

        [PreserveSig]
        int AdvisePeriodic(
            [In] long startTime,
            [In] long periodTime,
            [In] IntPtr hSemaphore, // System.Threading.WaitHandle?
            [Out] out int pdwAdviseCookie
            );

        [PreserveSig]
        int Unadvise([In] int dwAdviseCookie);
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
Guid("0000010c-0000-0000-C000-000000000046"),
InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPersist
    {
        [PreserveSig]
        int GetClassID([Out] out Guid pClassID);
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
    Guid("b61178d1-a2d9-11cf-9e53-00aa00a216a1"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IKsPin
    {
        /// <summary>
        /// The caller must free the returned structures, using the CoTaskMemFree function
        /// </summary>
        [PreserveSig]
        int KsQueryMediums(
            out IntPtr ip);
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
Guid("56a86899-0ad4-11ce-b03a-0020af0ba770"),
InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMediaFilter : IPersist
    {
        #region IPersist Methods

        [PreserveSig]
        new int GetClassID(
            [Out] out Guid pClassID);

        #endregion

        [PreserveSig]
        int Stop();

        [PreserveSig]
        int Pause();

        [PreserveSig]
        int Run([In] long tStart);

        [PreserveSig]
        int GetState(
            [In] int dwMilliSecsTimeout,
            [Out] out FilterState filtState
            );

        [PreserveSig]
        int SetSyncSource([In] IReferenceClock pClock);

        [PreserveSig]
        int GetSyncSource([Out] out IReferenceClock pClock);
    }


    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
Guid("56a86895-0ad4-11ce-b03a-0020af0ba770"),
InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IBaseFilter : IMediaFilter
    {
        #region IPersist Methods

        [PreserveSig]
        new int GetClassID(
            [Out] out Guid pClassID);

        #endregion

        #region IMediaFilter Methods

        [PreserveSig]
        new int Stop();

        [PreserveSig]
        new int Pause();

        [PreserveSig]
        new int Run(long tStart);

        [PreserveSig]
        new int GetState([In] int dwMilliSecsTimeout, [Out] out FilterState filtState);

        [PreserveSig]
        new int SetSyncSource([In] IReferenceClock pClock);

        [PreserveSig]
        new int GetSyncSource([Out] out IReferenceClock pClock);

        #endregion

        [PreserveSig]
        int EnumPins([Out] out IEnumPins ppEnum);

        [PreserveSig]
        int FindPin(
            [In, MarshalAs(UnmanagedType.LPWStr)] string Id,
            [Out] out IPin ppPin
            );

        [PreserveSig]
        int QueryFilterInfo([Out] out FilterInfo pInfo);

        [PreserveSig]
        int JoinFilterGraph(
            [In] IFilterGraph pGraph,
            [In, MarshalAs(UnmanagedType.LPWStr)] string pName
            );

        [PreserveSig]
        int QueryVendorInfo([Out, MarshalAs(UnmanagedType.LPWStr)] out string pVendorInfo);
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
Guid("56a8689f-0ad4-11ce-b03a-0020af0ba770"),
InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IFilterGraph
    {
        [PreserveSig]
        int AddFilter(
            [In] IBaseFilter pFilter,
            [In, MarshalAs(UnmanagedType.LPWStr)] string pName
            );

        [PreserveSig]
        int RemoveFilter([In] IBaseFilter pFilter);

        [PreserveSig]
        int EnumFilters([Out] out IEnumFilters ppEnum);

        [PreserveSig]
        int FindFilterByName(
            [In, MarshalAs(UnmanagedType.LPWStr)] string pName,
            [Out] out IBaseFilter ppFilter
            );

        [PreserveSig]
        int ConnectDirect(
            [In] IPin ppinOut,
            [In] IPin ppinIn,
            [In, MarshalAs(UnmanagedType.LPStruct)] AMMediaType pmt
            );

        [PreserveSig]
        [Obsolete("This method is obsolete; use the IFilterGraph2.ReconnectEx method instead.")]
        int Reconnect([In] IPin ppin);

        [PreserveSig]
        int Disconnect([In] IPin ppin);

        [PreserveSig]
        int SetDefaultSyncSource();
    }


    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
Guid("56a868a9-0ad4-11ce-b03a-0020af0ba770"),
InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IGraphBuilder : IFilterGraph
    {
        #region IFilterGraph Methods

        [PreserveSig]
        new int AddFilter(
            [In] IBaseFilter pFilter,
            [In, MarshalAs(UnmanagedType.LPWStr)] string pName
            );

        [PreserveSig]
        new int RemoveFilter([In] IBaseFilter pFilter);

        [PreserveSig]
        new int EnumFilters([Out] out IEnumFilters ppEnum);

        [PreserveSig]
        new int FindFilterByName(
            [In, MarshalAs(UnmanagedType.LPWStr)] string pName,
            [Out] out IBaseFilter ppFilter
            );

        [PreserveSig]
        new int ConnectDirect(
            [In] IPin ppinOut,
            [In] IPin ppinIn,
            [In, MarshalAs(UnmanagedType.LPStruct)]
            AMMediaType pmt
            );

        [PreserveSig]
        new int Reconnect([In] IPin ppin);

        [PreserveSig]
        new int Disconnect([In] IPin ppin);

        [PreserveSig]
        new int SetDefaultSyncSource();

        #endregion

        [PreserveSig]
        int Connect(
            [In] IPin ppinOut,
            [In] IPin ppinIn
            );

        [PreserveSig]
        int Render([In] IPin ppinOut);

        [PreserveSig]
        int RenderFile(
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpcwstrFile,
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpcwstrPlayList
            );

        [PreserveSig]
        int AddSourceFilter(
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpcwstrFileName,
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpcwstrFilterName,
            [Out] out IBaseFilter ppFilter
            );

        [PreserveSig]
        int SetLogFile(IntPtr hFile); // DWORD_PTR

        [PreserveSig]
        int Abort();

        [PreserveSig]
        int ShouldOperationContinue();
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
Guid("36b73882-c2c8-11cf-8b46-00805f6cef60"),
InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IFilterGraph2 : IGraphBuilder
    {
        #region IFilterGraph Methods

        [PreserveSig]
        new int AddFilter(
            [In] IBaseFilter pFilter,
            [In, MarshalAs(UnmanagedType.LPWStr)] string pName
            );

        [PreserveSig]
        new int RemoveFilter([In] IBaseFilter pFilter);

        [PreserveSig]
        new int EnumFilters([Out] out IEnumFilters ppEnum);

        [PreserveSig]
        new int FindFilterByName(
            [In, MarshalAs(UnmanagedType.LPWStr)] string pName,
            [Out] out IBaseFilter ppFilter
            );

        [PreserveSig]
        new int ConnectDirect(
            [In] IPin ppinOut,
            [In] IPin ppinIn,
            [In, MarshalAs(UnmanagedType.LPStruct)]
            AMMediaType pmt
            );

        [PreserveSig]
        new int Reconnect([In] IPin ppin);

        [PreserveSig]
        new int Disconnect([In] IPin ppin);

        [PreserveSig]
        new int SetDefaultSyncSource();

        #endregion

        #region IGraphBuilder Method

        [PreserveSig]
        new int Connect(
            [In] IPin ppinOut,
            [In] IPin ppinIn
            );

        [PreserveSig]
        new int Render([In] IPin ppinOut);

        [PreserveSig]
        new int RenderFile(
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpcwstrFile,
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpcwstrPlayList
            );

        [PreserveSig]
        new int AddSourceFilter(
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpcwstrFileName,
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpcwstrFilterName,
            [Out] out IBaseFilter ppFilter
            );

        [PreserveSig]
        new int SetLogFile(IntPtr hFile); // DWORD_PTR

        [PreserveSig]
        new int Abort();

        [PreserveSig]
        new int ShouldOperationContinue();

        #endregion

        [PreserveSig]
        int AddSourceFilterForMoniker(
#if USING_NET11
            [In] UCOMIMoniker pMoniker,
            [In] UCOMIBindCtx pCtx,
#else
[In] IMoniker pMoniker,
[In] IBindCtx pCtx,
#endif
 [In, MarshalAs(UnmanagedType.LPWStr)] string lpcwstrFilterName,
 [Out] out IBaseFilter ppFilter
 );

        [PreserveSig]
        int ReconnectEx(
            [In] IPin ppin,
            [In] AMMediaType pmt
            );

        [PreserveSig]
        int RenderEx(
            [In] IPin pPinOut,
            [In] AMRenderExFlags dwFlags,
            [In] IntPtr pvContext // DWORD *
            );
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
    Guid("83E91E85-82C1-4ea7-801D-85DC50B75086"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEVRFilterConfig
    {
        [PreserveSig]
        int SetNumberOfStreams([In] int dwMaxStreams);

        [PreserveSig]
        int GetNumberOfStreams([Out] out int pdwMaxStreams);
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("FA993888-4383-415A-A930-DD472A8CF6F7")]
    public interface IMFGetService
    {
        void GetService(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid guidService,
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid,
            [MarshalAs(UnmanagedType.Interface)] out object ppvObject
            );
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
    Guid("a490b1e4-ab84-4d31-a1b2-181e03b1077a"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMFVideoDisplayControl
    {
        int GetNativeVideoSize(
            [Out] out Size pszVideo,
            [Out] out Size pszARVideo
            );

        int GetIdealVideoSize(
            [Out] out Size pszMin,
            [Out] out Size pszMax
            );

        int SetVideoPosition(
            [In] DsRect pnrcSource,
            [In] DsRect prcDest
            );

        int GetVideoPosition(
            [Out] DsRect pnrcSource,
            [Out] DsRect prcDest
            );

        int SetAspectRatioMode([In] int AspectRatioMode);

        int GetAspectRatioMode([Out] out int lpAspectRatioMode);

        int SetVideoWindow([In] IntPtr hwnd); // HWND

        int GetVideoWindow([Out] out IntPtr hwnd); // HWND

        int RepaintVideo();

        int GetCurrentImage(
            [In] IntPtr pBih,
            [Out] out IntPtr pDib,
            [Out] out uint pcbDib,
            [In, Out] ref ulong pTimeStamp

            );

        int SetBorderColor([In] int Clr);

        int GetBorderColor([Out] out int lpClr);

        int SetRenderingPrefs([In] int dwRenderFlags);

        int GetRenderingPrefs([Out] out int pdwRenderFlags);

        int SetFullscreen([In, MarshalAs(UnmanagedType.Bool)] bool fFullscreen);

        int GetFullscreen([Out, MarshalAs(UnmanagedType.Bool)] out bool pfFullscreen);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BITMAPFILEHEADER
    {
        public ushort bfType;
        public uint bfSize;
        public ushort bfReserved1;
        public ushort bfReserved2;
        public uint bfOffBits;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BITMAPINFOHEADER
    {
        public uint biSize;
        public int biWidth;
        public int biHeight;
        public ushort biPlanes;
        public ushort biBitCount;
        public uint biCompression;
        public uint biSizeImage;
        public int biXPelsPerMeter;
        public int biYPelsPerMeter;
        public uint biClrUsed;
        public uint biClrImportant;
        public const int BI_RGB = 0;
    };

    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct RGBQUAD
    {
        [FieldOffset(0)]
        public byte rgbBlue;

        [FieldOffset(1)]
        public byte rgbGreen;

        [FieldOffset(2)]
        public byte rgbRed;

        [FieldOffset(3)]
        public byte rgbReserved;
    }

    public enum FilterState
    {
        Stopped,
        Paused,
        Running
    }

    [StructLayout(LayoutKind.Sequential)]
    public class AMMediaType
    {
        public Guid majorType;
        public Guid subType;
        [MarshalAs(UnmanagedType.Bool)]
        public bool fixedSizeSamples;
        [MarshalAs(UnmanagedType.Bool)]
        public bool temporalCompression;
        public int sampleSize;
        public Guid formatType;
        public IntPtr unkPtr; // IUnknown Pointer
        public int formatSize;
        public IntPtr formatPtr; // Pointer to a buff determined by formatType
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct PinInfo
    {
        [MarshalAs(UnmanagedType.Interface)]
        public IBaseFilter filter;
        public PinDirection dir;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string name;
    }

    public enum PinDirection
    {
        Input,
        Output
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct FilterInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string achName;
        [MarshalAs(UnmanagedType.Interface)]
        public IFilterGraph pGraph;
    }

    [Flags]
    public enum AMRenderExFlags
    {
        None = 0,
        RenderToExistingRenderers = 1
    }

    [StructLayout(LayoutKind.Sequential)]
    public class DsRect
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        /// <summary>
        /// Empty contructor. Initialize all fields to 0
        /// </summary>
        public DsRect()
        {
            this.left = 0;
            this.top = 0;
            this.right = 0;
            this.bottom = 0;
        }

        /// <summary>
        /// A parametred constructor. Initialize fields with given values.
        /// </summary>
        /// <param name="left">the left value</param>
        /// <param name="top">the top value</param>
        /// <param name="right">the right value</param>
        /// <param name="bottom">the bottom value</param>
        public DsRect(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        /// <summary>
        /// A parametred constructor. Initialize fields with a given <see cref="System.Drawing.Rectangle"/>.
        /// </summary>
        /// <param name="rectangle">A <see cref="System.Drawing.Rectangle"/></param>
        /// <remarks>
        /// Warning, DsRect define a rectangle by defining two of his corners and <see cref="System.Drawing.Rectangle"/> define a rectangle with his upper/left corner, his width and his height.
        /// </remarks>
        public DsRect(Rectangle rectangle)
        {
            this.left = rectangle.Left;
            this.top = rectangle.Top;
            this.right = rectangle.Right;
            this.bottom = rectangle.Bottom;
        }

        /// <summary>
        /// Provide de string representation of this DsRect instance
        /// </summary>
        /// <returns>A string formated like this : [left, top - right, bottom]</returns>
        public override string ToString()
        {
            return string.Format("[{0}, {1} - {2}, {3}]", this.left, this.top, this.right, this.bottom);
        }

        public override int GetHashCode()
        {
            return this.left.GetHashCode() |
                this.top.GetHashCode() |
                this.right.GetHashCode() |
                this.bottom.GetHashCode();
        }

        /// <summary>
        /// Define implicit cast between DirectShowLib.DsRect and System.Drawing.Rectangle for languages supporting this feature.
        /// VB.Net doesn't support implicit cast. <see cref="DirectShowLib.DsRect.ToRectangle"/> for similar functionality.
        /// <code>
        ///   // Define a new Rectangle instance
        ///   Rectangle r = new Rectangle(0, 0, 100, 100);
        ///   // Do implicit cast between Rectangle and DsRect
        ///   DsRect dsR = r;
        ///
        ///   Console.WriteLine(dsR.ToString());
        /// </code>
        /// </summary>
        /// <param name="r">a DsRect to be cast</param>
        /// <returns>A casted System.Drawing.Rectangle</returns>
        public static implicit operator Rectangle(DsRect r)
        {
            return r.ToRectangle();
        }

        /// <summary>
        /// Define implicit cast between System.Drawing.Rectangle and DirectShowLib.DsRect for languages supporting this feature.
        /// VB.Net doesn't support implicit cast. <see cref="DirectShowLib.DsRect.FromRectangle"/> for similar functionality.
        /// <code>
        ///   // Define a new DsRect instance
        ///   DsRect dsR = new DsRect(0, 0, 100, 100);
        ///   // Do implicit cast between DsRect and Rectangle
        ///   Rectangle r = dsR;
        ///
        ///   Console.WriteLine(r.ToString());
        /// </code>
        /// </summary>
        /// <param name="r">A System.Drawing.Rectangle to be cast</param>
        /// <returns>A casted DsRect</returns>
        public static implicit operator DsRect(Rectangle r)
        {
            return new DsRect(r);
        }

        /// <summary>
        /// Get the System.Drawing.Rectangle equivalent to this DirectShowLib.DsRect instance.
        /// </summary>
        /// <returns>A System.Drawing.Rectangle</returns>
        public Rectangle ToRectangle()
        {
            return new Rectangle(this.left, this.top, (this.right - this.left), (this.bottom - this.top));
        }

        /// <summary>
        /// Get a new DirectShowLib.DsRect instance for a given <see cref="System.Drawing.Rectangle"/>
        /// </summary>
        /// <param name="r">The <see cref="System.Drawing.Rectangle"/> used to initialize this new DirectShowLib.DsGuid</param>
        /// <returns>A new instance of DirectShowLib.DsGuid</returns>
        public static DsRect FromRectangle(Rectangle r)
        {
            return new DsRect(r);
        }
    }
    public enum EventCode
    {
        // EvCod.h
        Complete = 0x01, // EC_COMPLETE
        UserAbort = 0x02, // EC_USERABORT
        ErrorAbort = 0x03, // EC_ERRORABORT
        Time = 0x04, // EC_TIME
        Repaint = 0x05, // EC_REPAINT
        StErrStopped = 0x06, // EC_STREAM_ERROR_STOPPED
        StErrStPlaying = 0x07, // EC_STREAM_ERROR_STILLPLAYING
        ErrorStPlaying = 0x08, // EC_ERROR_STILLPLAYING
        PaletteChanged = 0x09, // EC_PALETTE_CHANGED
        VideoSizeChanged = 0x0a, // EC_VIDEO_SIZE_CHANGED
        QualityChange = 0x0b, // EC_QUALITY_CHANGE
        ShuttingDown = 0x0c, // EC_SHUTTING_DOWN
        ClockChanged = 0x0d, // EC_CLOCK_CHANGED
        Paused = 0x0e, // EC_PAUSED
        OpeningFile = 0x10, // EC_OPENING_FILE
        BufferingData = 0x11, // EC_BUFFERING_DATA
        FullScreenLost = 0x12, // EC_FULLSCREEN_LOST
        Activate = 0x13, // EC_ACTIVATE
        NeedRestart = 0x14, // EC_NEED_RESTART
        WindowDestroyed = 0x15, // EC_WINDOW_DESTROYED
        DisplayChanged = 0x16, // EC_DISPLAY_CHANGED
        Starvation = 0x17, // EC_STARVATION
        OleEvent = 0x18, // EC_OLE_EVENT
        NotifyWindow = 0x19, // EC_NOTIFY_WINDOW
        StreamControlStopped = 0x1A, // EC_STREAM_CONTROL_STOPPED
        StreamControlStarted = 0x1B, // EC_STREAM_CONTROL_STARTED
        EndOfSegment = 0x1C, // EC_END_OF_SEGMENT
        SegmentStarted = 0x1D, // EC_SEGMENT_STARTED
        LengthChanged = 0x1E, // EC_LENGTH_CHANGED
        DeviceLost = 0x1f, // EC_DEVICE_LOST
        StepComplete = 0x24, // EC_STEP_COMPLETE
        TimeCodeAvailable = 0x30, // EC_TIMECODE_AVAILABLE
        ExtDeviceModeChange = 0x31, // EC_EXTDEVICE_MODE_CHANGE
        StateChange = 0x32, // EC_STATE_CHANGE
        GraphChanged = 0x50, // EC_GRAPH_CHANGED
        ClockUnset = 0x51, // EC_CLOCK_UNSET
        VMRRenderDeviceSet = 0x53, // EC_VMR_RENDERDEVICE_SET
        VMRSurfaceFlipped = 0x54, // EC_VMR_SURFACE_FLIPPED
        VMRReconnectionFailed = 0x55, // EC_VMR_RECONNECTION_FAILED
        PreprocessComplete = 0x56, // EC_PREPROCESS_COMPLETE
        CodecApiEvent = 0x57, // EC_CODECAPI_EVENT

        // DVDevCod.h
        DvdDomainChange = 0x101, // EC_DVD_DOMAIN_CHANGE
        DvdTitleChange = 0x102, // EC_DVD_TITLE_CHANGE
        DvdChapterStart = 0x103, // EC_DVD_CHAPTER_START
        DvdAudioStreamChange = 0x104, // EC_DVD_AUDIO_STREAM_CHANGE
        DvdSubPicictureStreamChange = 0x105, // EC_DVD_SUBPICTURE_STREAM_CHANGE
        DvdAngleChange = 0x106, // EC_DVD_ANGLE_CHANGE
        DvdButtonChange = 0x107, // EC_DVD_BUTTON_CHANGE
        DvdValidUopsChange = 0x108, // EC_DVD_VALID_UOPS_CHANGE
        DvdStillOn = 0x109, // EC_DVD_STILL_ON
        DvdStillOff = 0x10a, // EC_DVD_STILL_OFF
        DvdCurrentTime = 0x10b, // EC_DVD_CURRENT_TIME
        DvdError = 0x10c, // EC_DVD_ERROR
        DvdWarning = 0x10d, // EC_DVD_WARNING
        DvdChapterAutoStop = 0x10e, // EC_DVD_CHAPTER_AUTOSTOP
        DvdNoFpPgc = 0x10f, // EC_DVD_NO_FP_PGC
        DvdPlaybackRateChange = 0x110, // EC_DVD_PLAYBACK_RATE_CHANGE
        DvdParentalLevelChange = 0x111, // EC_DVD_PARENTAL_LEVEL_CHANGE
        DvdPlaybackStopped = 0x112, // EC_DVD_PLAYBACK_STOPPED
        DvdAnglesAvailable = 0x113, // EC_DVD_ANGLES_AVAILABLE
        DvdPlayPeriodAutoStop = 0x114, // EC_DVD_PLAYPERIOD_AUTOSTOP
        DvdButtonAutoActivated = 0x115, // EC_DVD_BUTTON_AUTO_ACTIVATED
        DvdCmdStart = 0x116, // EC_DVD_CMD_START
        DvdCmdEnd = 0x117, // EC_DVD_CMD_END
        DvdDiscEjected = 0x118, // EC_DVD_DISC_EJECTED
        DvdDiscInserted = 0x119, // EC_DVD_DISC_INSERTED
        DvdCurrentHmsfTime = 0x11a, // EC_DVD_CURRENT_HMSF_TIME
        DvdKaraokeMode = 0x11b, // EC_DVD_KARAOKE_MODE

        // AudEvCod.h
        SNDDEVInError = 0x200, // EC_SNDDEV_IN_ERROR
        SNDDEVOutError = 0x201, // EC_SNDDEV_OUT_ERROR

        WMTIndexEvent = 0x0251, // EC_WMT_INDEX_EVENT
        WMTEvent = 0x0252, // EC_WMT_EVENT

        Built = 0x300, // EC_BUILT
        Unbuilt = 0x301, // EC_UNBUILT

        // Sbe.h
        StreamBufferTimeHole = 0x0326, // STREAMBUFFER_EC_TIMEHOLE
        StreamBufferStaleDataRead = 0x0327, // STREAMBUFFER_EC_STALE_DATA_READ
        StreamBufferStaleFileDeleted = 0x0328, // STREAMBUFFER_EC_STALE_FILE_DELETED
        StreamBufferContentBecomingStale = 0x0329, // STREAMBUFFER_EC_CONTENT_BECOMING_STALE
        StreamBufferWriteFailure = 0x032a, // STREAMBUFFER_EC_WRITE_FAILURE
        StreamBufferReadFailure = 0x032b, // STREAMBUFFER_EC_READ_FAILURE
        StreamBufferRateChanged = 0x032c, // STREAMBUFFER_EC_RATE_CHANGED
    }
    [Flags]
    public enum NotifyFlags
    {
        None,
        NoNotify
    }
    public enum VMRAspectRatioMode
    {
        None,
        LetterBox
    }

    abstract internal class DsMarshaler : ICustomMarshaler
    {
        #region Data Members
        // The cookie isn't currently being used.
        protected string m_cookie;

        // The managed object passed in to MarshalManagedToNative, and modified in MarshalNativeToManaged
        protected object m_obj;
        #endregion

        // The constructor.  This is called from GetInstance (below)
        public DsMarshaler(string cookie)
        {
            // If we get a cookie, save it.
            m_cookie = cookie;
        }

        // Called just before invoking the COM method.  The returned IntPtr is what goes on the stack
        // for the COM call.  The input arg is the parameter that was passed to the method.
        virtual public IntPtr MarshalManagedToNative(object managedObj)
        {
            // Save off the passed-in value.  Safe since we just checked the type.
            m_obj = managedObj;

            // Create an appropriately sized buffer, blank it, and send it to the marshaler to
            // make the COM call with.
            int iSize = GetNativeDataSize() + 3;
            IntPtr p = Marshal.AllocCoTaskMem(iSize);

            Enumerable.Range(0, iSize).ForEach((x) =>
            {
                Marshal.WriteInt32(p, x * 4, 0);
            });

            return p;
        }

        // Called just after invoking the COM method.  The IntPtr is the same one that just got returned
        // from MarshalManagedToNative.  The return value is unused.
        virtual public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            return m_obj;
        }

        // Release the (now unused) buffer
        virtual public void CleanUpNativeData(IntPtr pNativeData)
        {
            if (pNativeData != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(pNativeData);
            }
        }

        // Release the (now unused) managed object
        virtual public void CleanUpManagedData(object managedObj)
        {
            m_obj = null;
        }

        // This routine is (apparently) never called by the marshaler.  However it can be useful.
        abstract public int GetNativeDataSize();

        // GetInstance is called by the marshaler in preparation to doing custom marshaling.  The (optional)
        // cookie is the value specified in MarshalCookie="asdf", or "" is none is specified.

        // It is commented out in this abstract class, but MUST be implemented in derived classes
        //public static ICustomMarshaler GetInstance(string cookie)
    }

    // c# does not correctly marshal arrays of pointers.
    //


    internal class EMTMarshaler : DsMarshaler
    {
        public EMTMarshaler(string cookie)
            : base(cookie)
        {
        }

        // Called just after invoking the COM method.  The IntPtr is the same one that just got returned
        // from MarshalManagedToNative.  The return value is unused.
        override public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            AMMediaType[] emt = m_obj as AMMediaType[];

            emt.ForEach((amm, x) =>
            {
                // Copy in the value, and advance the pointer
                IntPtr p = Marshal.ReadIntPtr(pNativeData, x * IntPtr.Size);
                if (p != IntPtr.Zero)
                {
                    emt[x] = (AMMediaType)Marshal.PtrToStructure(p, typeof(AMMediaType));
                }
                else
                {
                    emt[x] = null;
                }
            });

            return null;
        }

        // The number of bytes to marshal out
        override public int GetNativeDataSize()
        {
            // Get the array size
            int i = ((Array)m_obj).Length;

            // Multiply that times the size of a pointer
            int j = i * IntPtr.Size;

            return j;
        }

        // This method is called by interop to create the custom marshaler.  The (optional)
        // cookie is the value specified in MarshalCookie="asdf", or "" is none is specified.
        public static ICustomMarshaler GetInstance(string cookie)
        {
            return new EMTMarshaler(cookie);
        }
    }
    sealed public class DsError
    {
        private DsError()
        {
            // Prevent people from trying to instantiate this class
        }

        [DllImport("quartz.dll", CharSet = CharSet.Auto)]
        public static extern int AMGetErrorText(int hr, StringBuilder buf, int max);

        /// <summary>
        /// If hr has a "failed" status code (E_*), throw an exception.  Note that status
        /// messages (S_*) are not considered failure codes.  If DirectShow error text
        /// is available, it is used to build the exception, otherwise a generic com error
        /// is thrown.
        /// </summary>
        /// <param name="hr">The HRESULT to check</param>
        public static void ThrowExceptionForHR(int hr)
        {
            // If a severe error has occurred
            if (hr < 0)
            {
                string s = GetErrorText(hr);

                // If a string is returned, build a com error from it
                if (s != null)
                {
                    throw new COMException(s, hr);
                }
                else
                {
                    // No string, just use standard com error
                    Marshal.ThrowExceptionForHR(hr);
                }
            }
        }

        /// <summary>
        /// Returns a string describing a DS error.  Works for both error codes
        /// (values < 0) and Status codes (values >= 0)
        /// </summary>
        /// <param name="hr">HRESULT for which to get description</param>
        /// <returns>The string, or null if no error text can be found</returns>
        public static string GetErrorText(int hr)
        {
            const int MAX_ERROR_TEXT_LEN = 160;

            // Make a buffer to hold the string
            StringBuilder buf = new StringBuilder(MAX_ERROR_TEXT_LEN, MAX_ERROR_TEXT_LEN);

            // If a string is returned, build a com error from it
            if (AMGetErrorText(hr, buf, MAX_ERROR_TEXT_LEN) > 0)
            {
                return buf.ToString();
            }

            return null;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VMRVideoStreamInfo
    {
        public IntPtr pddsVideoSurface;
        public int dwWidth;
        public int dwHeight;
        public int dwStrmID;
        public float fAlpha;
        public DDColorKey ddClrKey;
        public NormalizedRect rNormal;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct DDColorKey
    {
        public int dw1;
        public int dw2;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct NormalizedRect
    {
        public float left;
        public float top;
        public float right;
        public float bottom;

        public NormalizedRect(float l, float t, float r, float b)
        {
            this.left = l;
            this.top = t;
            this.right = r;
            this.bottom = b;
        }

        public NormalizedRect(RectangleF r)
        {
            this.left = r.Left;
            this.top = r.Top;
            this.right = r.Right;
            this.bottom = r.Bottom;
        }

        public override string ToString()
        {
            return string.Format("[{0}, {1} - {2}, {3}]", this.left, this.top, this.right, this.bottom);
        }

        public override int GetHashCode()
        {
            return this.left.GetHashCode() |
                this.top.GetHashCode() |
                this.right.GetHashCode() |
                this.bottom.GetHashCode();
        }

        public static implicit operator RectangleF(NormalizedRect r)
        {
            return r.ToRectangleF();
        }

        public static implicit operator NormalizedRect(Rectangle r)
        {
            return new NormalizedRect(r);
        }

        public static bool operator ==(NormalizedRect r1, NormalizedRect r2)
        {
            return ((r1.left == r2.left) && (r1.top == r2.top) && (r1.right == r2.right) && (r1.bottom == r2.bottom));
        }

        public static bool operator !=(NormalizedRect r1, NormalizedRect r2)
        {
            return ((r1.left != r2.left) || (r1.top != r2.top) || (r1.right != r2.right) || (r1.bottom != r2.bottom));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is NormalizedRect))
                return false;

            NormalizedRect other = (NormalizedRect)obj;
            return (this == other);
        }


        public RectangleF ToRectangleF()
        {
            return new RectangleF(this.left, this.top, (this.right - this.left), (this.bottom - this.top));
        }

        public static NormalizedRect FromRectangle(RectangleF r)
        {
            return new NormalizedRect(r);
        }
    }

    [Flags]
    public enum VMRRenderPrefs
    {
        RestrictToInitialMonitor = 0x00000000,
        ForceOffscreen = 0x00000001,
        ForceOverlays = 0x00000002,
        AllowOverlays = 0x00000000,
        AllowOffscreen = 0x00000000,
        DoNotRenderColorKeyAndBorder = 0x00000008,
        Reserved = 0x00000010,
        PreferAGPMemWhenMixing = 0x00000020,

        Mask = 0x0000003f,
    }
    [Flags]
    public enum VMRMode
    {
        None = 0,
        Windowed = 0x00000001,
        Windowless = 0x00000002,
        Renderless = 0x00000004,
    }
    [Flags]
    public enum AMSeekingSeekingCapabilities
    {
        None = 0,
        CanSeekAbsolute = 0x001,
        CanSeekForwards = 0x002,
        CanSeekBackwards = 0x004,
        CanGetCurrentPos = 0x008,
        CanGetStopPos = 0x010,
        CanGetDuration = 0x020,
        CanPlayBackwards = 0x040,
        CanDoSegments = 0x080,
        Source = 0x100
    }
    [StructLayout(LayoutKind.Explicit)]
    public class DsGuid
    {
        [FieldOffset(0)]
        private Guid guid;

        public static readonly DsGuid Empty = Guid.Empty;

        /// <summary>
        /// Empty constructor. 
        /// Initialize it with System.Guid.Empty
        /// </summary>
        public DsGuid()
        {
            this.guid = Guid.Empty;
        }

        /// <summary>
        /// Constructor.
        /// Initialize this instance with a given System.Guid string representation.
        /// </summary>
        /// <param name="g">A valid System.Guid as string</param>
        public DsGuid(string g)
        {
            this.guid = new Guid(g);
        }

        /// <summary>
        /// Constructor.
        /// Initialize this instance with a given System.Guid.
        /// </summary>
        /// <param name="g">A System.Guid value type</param>
        public DsGuid(Guid g)
        {
            this.guid = g;
        }

        /// <summary>
        /// Get a string representation of this DirectShowLib.DsGuid Instance.
        /// </summary>
        /// <returns>A string representing this instance</returns>
        public override string ToString()
        {
            return this.guid.ToString();
        }

        /// <summary>
        /// Get a string representation of this DirectShowLib.DsGuid Instance with a specific format.
        /// </summary>
        /// <param name="format"><see cref="System.Guid.ToString"/> for a description of the format parameter.</param>
        /// <returns>A string representing this instance according to the format parameter</returns>
        public string ToString(string format)
        {
            return this.guid.ToString(format);
        }

        public override int GetHashCode()
        {
            return this.guid.GetHashCode();
        }

        /// <summary>
        /// Define implicit cast between DirectShowLib.DsGuid and System.Guid for languages supporting this feature.
        /// VB.Net doesn't support implicit cast. <see cref="DirectShowLib.DsGuid.ToGuid"/> for similar functionality.
        /// <code>
        ///   // Define a new DsGuid instance
        ///   DsGuid dsG = new DsGuid("{33D57EBF-7C9D-435e-A15E-D300B52FBD91}");
        ///   // Do implicit cast between DsGuid and Guid
        ///   Guid g = dsG;
        ///
        ///   Console.WriteLine(g.ToString());
        /// </code>
        /// </summary>
        /// <param name="g">DirectShowLib.DsGuid to be cast</param>
        /// <returns>A casted System.Guid</returns>
        public static implicit operator Guid(DsGuid g)
        {
            return g.guid;
        }

        /// <summary>
        /// Define implicit cast between System.Guid and DirectShowLib.DsGuid for languages supporting this feature.
        /// VB.Net doesn't support implicit cast. <see cref="DirectShowLib.DsGuid.FromGuid"/> for similar functionality.
        /// <code>
        ///   // Define a new Guid instance
        ///   Guid g = new Guid("{B9364217-366E-45f8-AA2D-B0ED9E7D932D}");
        ///   // Do implicit cast between Guid and DsGuid
        ///   DsGuid dsG = g;
        ///
        ///   Console.WriteLine(dsG.ToString());
        /// </code>
        /// </summary>
        /// <param name="g">System.Guid to be cast</param>
        /// <returns>A casted DirectShowLib.DsGuid</returns>
        public static implicit operator DsGuid(Guid g)
        {
            return new DsGuid(g);
        }

        /// <summary>
        /// Get the System.Guid equivalent to this DirectShowLib.DsGuid instance.
        /// </summary>
        /// <returns>A System.Guid</returns>
        public Guid ToGuid()
        {
            return this.guid;
        }

        /// <summary>
        /// Get a new DirectShowLib.DsGuid instance for a given System.Guid
        /// </summary>
        /// <param name="g">The System.Guid to wrap into a DirectShowLib.DsGuid</param>
        /// <returns>A new instance of DirectShowLib.DsGuid</returns>
        public static DsGuid FromGuid(Guid g)
        {
            return new DsGuid(g);
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    public class DsLong
    {
        private long Value;

        /// <summary>
        /// Constructor
        /// Initialize a new instance of DirectShowLib.DsLong with the Value parameter
        /// </summary>
        /// <param name="Value">Value to assign to this new instance</param>
        public DsLong(long Value)
        {
            this.Value = Value;
        }

        /// <summary>
        /// Get a string representation of this DirectShowLib.DsLong Instance.
        /// </summary>
        /// <returns>A string representing this instance</returns>
        public override string ToString()
        {
            return this.Value.ToString();
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        /// <summary>
        /// Define implicit cast between DirectShowLib.DsLong and System.Int64 for languages supporting this feature.
        /// VB.Net doesn't support implicit cast. <see cref="DirectShowLib.DsLong.ToInt64"/> for similar functionality.
        /// <code>
        ///   // Define a new DsLong instance
        ///   DsLong dsL = new DsLong(9876543210);
        ///   // Do implicit cast between DsLong and Int64
        ///   long l = dsL;
        ///
        ///   Console.WriteLine(l.ToString());
        /// </code>
        /// </summary>
        /// <param name="g">DirectShowLib.DsLong to be cast</param>
        /// <returns>A casted System.Int64</returns>
        public static implicit operator long(DsLong l)
        {
            return l.Value;
        }

        /// <summary>
        /// Define implicit cast between System.Int64 and DirectShowLib.DsLong for languages supporting this feature.
        /// VB.Net doesn't support implicit cast. <see cref="DirectShowLib.DsGuid.FromInt64"/> for similar functionality.
        /// <code>
        ///   // Define a new Int64 instance
        ///   long l = 9876543210;
        ///   // Do implicit cast between Int64 and DsLong
        ///   DsLong dsl = l;
        ///
        ///   Console.WriteLine(dsl.ToString());
        /// </code>
        /// </summary>
        /// <param name="g">System.Int64 to be cast</param>
        /// <returns>A casted DirectShowLib.DsLong</returns>
        public static implicit operator DsLong(long l)
        {
            return new DsLong(l);
        }

        /// <summary>
        /// Get the System.Int64 equivalent to this DirectShowLib.DsLong instance.
        /// </summary>
        /// <returns>A System.Int64</returns>
        public long ToInt64()
        {
            return this.Value;
        }

        /// <summary>
        /// Get a new DirectShowLib.DsLong instance for a given System.Int64
        /// </summary>
        /// <param name="g">The System.Int64 to wrap into a DirectShowLib.DsLong</param>
        /// <returns>A new instance of DirectShowLib.DsLong</returns>
        public static DsLong FromInt64(long l)
        {
            return new DsLong(l);
        }
    }

    [Flags]
    public enum AMSeekingSeekingFlags
    {
        NoPositioning = 0x00,
        AbsolutePositioning = 0x01,
        RelativePositioning = 0x02,
        IncrementalPositioning = 0x03,
        PositioningBitsMask = 0x03,
        SeekToKeyFrame = 0x04,
        ReturnTime = 0x08,
        Segment = 0x10,
        NoFlush = 0x20
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DsCAUUID
    {
        public int cElems;
        public IntPtr pElems;

        /// <summary>
        /// Perform a manual marshaling of pElems to retrieve an array of System.Guid.
        /// Assume this structure has been already filled by the ISpecifyPropertyPages.GetPages() method.
        /// </summary>
        /// <returns>A managed representation of pElems (cElems items)</returns>
        public Guid[] ToGuidArray()
        {
            Guid[] retval = new Guid[this.cElems];
            var pel = pElems;
            retval.ForEach((ret, i) =>
            {
                IntPtr ptr = new IntPtr(pel.ToInt64() + (Marshal.SizeOf(typeof(Guid)) * i));
                ret = (Guid)Marshal.PtrToStructure(ptr, typeof(Guid));
            });

            return retval;
        }
    }

    sealed public class MediaType
    {
        private MediaType()
        {
            // Prevent people from trying to instantiate this class
        }

        public static readonly Guid Null = Guid.Empty;

        /// <summary> MEDIATYPE_Video 'vids' </summary>
        public static readonly Guid Video = new Guid(0x73646976, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIATYPE_Interleaved 'iavs' </summary>
        public static readonly Guid Interleaved = new Guid(0x73766169, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIATYPE_Audio 'auds' </summary>
        public static readonly Guid Audio = new Guid(0x73647561, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIATYPE_Text 'txts' </summary>
        public static readonly Guid Text = new Guid(0x73747874, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIATYPE_Stream </summary>
        public static readonly Guid Stream = new Guid(0xe436eb83, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

        /// <summary> MEDIATYPE_VBI </summary>
        public static readonly Guid VBI = new Guid(0xf72a76e1, 0xeb0a, 0x11d0, 0xac, 0xe4, 0x00, 0x00, 0xc0, 0xcc, 0x16, 0xba);

        /// <summary> MEDIATYPE_Midi </summary>
        public static readonly Guid Midi = new Guid(0x7364696D, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIATYPE_File </summary>
        public static readonly Guid File = new Guid(0x656c6966, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIATYPE_ScriptCommand </summary>
        public static readonly Guid ScriptCommand = new Guid(0x73636d64, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIATYPE_AUXLine21Data </summary>
        public static readonly Guid AuxLine21Data = new Guid(0x670aea80, 0x3a82, 0x11d0, 0xb7, 0x9b, 0x00, 0xaa, 0x00, 0x37, 0x67, 0xa7);

        /// <summary> MEDIATYPE_Timecode </summary>
        public static readonly Guid Timecode = new Guid(0x0482dee3, 0x7817, 0x11cf, 0x8a, 0x03, 0x00, 0xaa, 0x00, 0x6e, 0xcb, 0x65);

        /// <summary> MEDIATYPE_LMRT </summary>
        public static readonly Guid LMRT = new Guid(0x74726c6d, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIATYPE_URL_STREAM </summary>
        public static readonly Guid URLStream = new Guid(0x736c7275, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIATYPE_AnalogVideo </summary>
        public static readonly Guid AnalogVideo = new Guid(0x0482dde1, 0x7817, 0x11cf, 0x8a, 0x03, 0x00, 0xaa, 0x00, 0x6e, 0xcb, 0x65);

        /// <summary> MEDIATYPE_AnalogAudio </summary>
        public static readonly Guid AnalogAudio = new Guid(0x0482dee1, 0x7817, 0x11cf, 0x8a, 0x03, 0x00, 0xaa, 0x00, 0x6e, 0xcb, 0x65);

        /// <summary> MEDIATYPE_MPEG2_SECTIONS </summary>
        public static readonly Guid Mpeg2Sections = new Guid(0x455f176c, 0x4b06, 0x47ce, 0x9a, 0xef, 0x8c, 0xae, 0xf7, 0x3d, 0xf7, 0xb5);

        /// <summary> MEDIATYPE_DTVCCData </summary>
        public static readonly Guid DTVCCData = new Guid(0xfb77e152, 0x53b2, 0x499c, 0xb4, 0x6b, 0x50, 0x9f, 0xc3, 0x3e, 0xdf, 0xd7);

        /// <summary> MEDIATYPE_MSTVCaption </summary>
        public static readonly Guid MSTVCaption = new Guid(0xB88B8A89, 0xB049, 0x4C80, 0xAD, 0xCF, 0x58, 0x98, 0x98, 0x5E, 0x22, 0xC1);

    }

    sealed public class MediaSubType
    {
        private MediaSubType()
        {
            // Prevent people from trying to instantiate this class
        }

        public static readonly Guid Null = Guid.Empty;

        /// <summary> MEDIASUBTYPE_CLPL </summary>
        public static readonly Guid CLPL = new Guid(0x4C504C43, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_YUYV </summary>
        public static readonly Guid YUYV = new Guid(0x56595559, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_IYUV </summary>
        public static readonly Guid IYUV = new Guid(0x56555949, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_YVU9 </summary>
        public static readonly Guid YVU9 = new Guid(0x39555659, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_Y411 </summary>
        public static readonly Guid Y411 = new Guid(0x31313459, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_Y41P </summary>
        public static readonly Guid Y41P = new Guid(0x50313459, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_YUY2 </summary>
        public static readonly Guid YUY2 = new Guid(0x32595559, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_YVYU </summary>
        public static readonly Guid YVYU = new Guid(0x55595659, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_UYVY </summary>
        public static readonly Guid UYVY = new Guid(0x59565955, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_Y211 </summary>
        public static readonly Guid Y211 = new Guid(0x31313259, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_CLJR </summary>
        public static readonly Guid CLJR = new Guid(0x524a4c43, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_IF09 </summary>
        public static readonly Guid IF09 = new Guid(0x39304649, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_CPLA </summary>
        public static readonly Guid CPLA = new Guid(0x414c5043, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_MJPG </summary>
        public static readonly Guid MJPG = new Guid(0x47504A4D, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_TVMJ </summary>
        public static readonly Guid TVMJ = new Guid(0x4A4D5654, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_WAKE </summary>
        public static readonly Guid WAKE = new Guid(0x454B4157, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_CFCC </summary>
        public static readonly Guid CFCC = new Guid(0x43434643, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_IJPG </summary>
        public static readonly Guid IJPG = new Guid(0x47504A49, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_Plum </summary>
        public static readonly Guid PLUM = new Guid(0x6D756C50, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_DVCS </summary>
        public static readonly Guid DVCS = new Guid(0x53435644, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_DVSD </summary>
        public static readonly Guid DVSD = new Guid(0x44535644, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_MDVF </summary>
        public static readonly Guid MDVF = new Guid(0x4656444D, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_RGB1 </summary>
        public static readonly Guid RGB1 = new Guid(0xe436eb78, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

        /// <summary> MEDIASUBTYPE_RGB4 </summary>
        public static readonly Guid RGB4 = new Guid(0xe436eb79, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

        /// <summary> MEDIASUBTYPE_RGB8 </summary>
        public static readonly Guid RGB8 = new Guid(0xe436eb7a, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

        /// <summary> MEDIASUBTYPE_RGB565 </summary>
        public static readonly Guid RGB565 = new Guid(0xe436eb7b, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

        /// <summary> MEDIASUBTYPE_RGB555 </summary>
        public static readonly Guid RGB555 = new Guid(0xe436eb7c, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

        /// <summary> MEDIASUBTYPE_RGB24 </summary>
        public static readonly Guid RGB24 = new Guid(0xe436eb7d, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

        /// <summary> MEDIASUBTYPE_RGB32 </summary>
        public static readonly Guid RGB32 = new Guid(0xe436eb7e, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

        /// <summary> MEDIASUBTYPE_ARGB1555 </summary>
        public static readonly Guid ARGB1555 = new Guid(0x297c55af, 0xe209, 0x4cb3, 0xb7, 0x57, 0xc7, 0x6d, 0x6b, 0x9c, 0x88, 0xa8);

        /// <summary> MEDIASUBTYPE_ARGB4444 </summary>
        public static readonly Guid ARGB4444 = new Guid(0x6e6415e6, 0x5c24, 0x425f, 0x93, 0xcd, 0x80, 0x10, 0x2b, 0x3d, 0x1c, 0xca);

        /// <summary> MEDIASUBTYPE_ARGB32 </summary>
        public static readonly Guid ARGB32 = new Guid(0x773c9ac0, 0x3274, 0x11d0, 0xb7, 0x24, 0x00, 0xaa, 0x00, 0x6c, 0x1a, 0x01);

        /// <summary> MEDIASUBTYPE_A2R10G10B10 </summary>
        public static readonly Guid A2R10G10B10 = new Guid(0x2f8bb76d, 0xb644, 0x4550, 0xac, 0xf3, 0xd3, 0x0c, 0xaa, 0x65, 0xd5, 0xc5);

        /// <summary> MEDIASUBTYPE_A2B10G10R10 </summary>
        public static readonly Guid A2B10G10R10 = new Guid(0x576f7893, 0xbdf6, 0x48c4, 0x87, 0x5f, 0xae, 0x7b, 0x81, 0x83, 0x45, 0x67);

        /// <summary> MEDIASUBTYPE_AYUV </summary>
        public static readonly Guid AYUV = new Guid(0x56555941, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_AI44 </summary>
        public static readonly Guid AI44 = new Guid(0x34344941, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_IA44 </summary>
        public static readonly Guid IA44 = new Guid(0x34344149, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_RGB32_D3D_DX7_RT </summary>
        public static readonly Guid RGB32_D3D_DX7_RT = new Guid(0x32335237, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_RGB16_D3D_DX7_RT </summary>
        public static readonly Guid RGB16_D3D_DX7_RT = new Guid(0x36315237, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_ARGB32_D3D_DX7_RT </summary>
        public static readonly Guid ARGB32_D3D_DX7_RT = new Guid(0x38384137, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_ARGB4444_D3D_DX7_RT </summary>
        public static readonly Guid ARGB4444_D3D_DX7_RT = new Guid(0x34344137, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_ARGB1555_D3D_DX7_RT </summary>
        public static readonly Guid ARGB1555_D3D_DX7_RT = new Guid(0x35314137, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_RGB32_D3D_DX9_RT </summary>
        public static readonly Guid RGB32_D3D_DX9_RT = new Guid(0x32335239, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_RGB16_D3D_DX9_RT </summary>
        public static readonly Guid RGB16_D3D_DX9_RT = new Guid(0x36315239, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_ARGB32_D3D_DX9_RT </summary>
        public static readonly Guid ARGB32_D3D_DX9_RT = new Guid(0x38384139, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_ARGB4444_D3D_DX9_RT </summary>
        public static readonly Guid ARGB4444_D3D_DX9_RT = new Guid(0x34344139, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_ARGB1555_D3D_DX9_RT </summary>
        public static readonly Guid ARGB1555_D3D_DX9_RT = new Guid(0x35314139, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_YV12 </summary>
        public static readonly Guid YV12 = new Guid(0x32315659, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_NV12 </summary>
        public static readonly Guid NV12 = new Guid(0x3231564E, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_IMC1 </summary>
        public static readonly Guid IMC1 = new Guid(0x31434D49, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_IMC2 </summary>
        public static readonly Guid IMC2 = new Guid(0x32434D49, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_IMC3 </summary>
        public static readonly Guid IMC3 = new Guid(0x33434D49, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_IMC4 </summary>
        public static readonly Guid IMC4 = new Guid(0x34434D49, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_S340 </summary>
        public static readonly Guid S340 = new Guid(0x30343353, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_S342 </summary>
        public static readonly Guid S342 = new Guid(0x32343353, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_Overlay </summary>
        public static readonly Guid Overlay = new Guid(0xe436eb7f, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

        /// <summary> MEDIASUBTYPE_MPEG1Packet </summary>
        public static readonly Guid MPEG1Packet = new Guid(0xe436eb80, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

        /// <summary> MEDIASUBTYPE_MPEG1Payload </summary>
        public static readonly Guid MPEG1Payload = new Guid(0xe436eb81, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

        /// <summary> MEDIASUBTYPE_MPEG1AudioPayload </summary>
        public static readonly Guid MPEG1AudioPayload = new Guid(0x00000050, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71);

        /// <summary> MEDIATYPE_MPEG1SystemStream </summary>
        public static readonly Guid MPEG1SystemStream = new Guid(0xe436eb82, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

        /// <summary> MEDIASUBTYPE_MPEG1System </summary>
        public static readonly Guid MPEG1System = new Guid(0xe436eb84, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

        /// <summary> MEDIASUBTYPE_MPEG1VideoCD </summary>
        public static readonly Guid MPEG1VideoCD = new Guid(0xe436eb85, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

        /// <summary> MEDIASUBTYPE_MPEG1Video </summary>
        public static readonly Guid MPEG1Video = new Guid(0xe436eb86, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

        /// <summary> MEDIASUBTYPE_MPEG1Audio </summary>
        public static readonly Guid MPEG1Audio = new Guid(0xe436eb87, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

        /// <summary> MEDIASUBTYPE_Avi </summary>
        public static readonly Guid Avi = new Guid(0xe436eb88, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

        /// <summary> MEDIASUBTYPE_Asf </summary>
        public static readonly Guid Asf = new Guid(0x3db80f90, 0x9412, 0x11d1, 0xad, 0xed, 0x00, 0x00, 0xf8, 0x75, 0x4b, 0x99);

        /// <summary> MEDIASUBTYPE_QTMovie </summary>
        public static readonly Guid QTMovie = new Guid(0xe436eb89, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

        /// <summary> MEDIASUBTYPE_QTRpza </summary>
        public static readonly Guid QTRpza = new Guid(0x617a7072, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_QTSmc </summary>
        public static readonly Guid QTSmc = new Guid(0x20636d73, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_QTRle </summary>
        public static readonly Guid QTRle = new Guid(0x20656c72, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_QTJpeg </summary>
        public static readonly Guid QTJpeg = new Guid(0x6765706a, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_PCMAudio_Obsolete </summary>
        public static readonly Guid PCMAudio_Obsolete = new Guid(0xe436eb8a, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

        /// <summary> MEDIASUBTYPE_PCM </summary>
        public static readonly Guid PCM = new Guid(0x00000001, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71);

        /// <summary> MEDIASUBTYPE_WAVE </summary>
        public static readonly Guid WAVE = new Guid(0xe436eb8b, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

        /// <summary> MEDIASUBTYPE_AU </summary>
        public static readonly Guid AU = new Guid(0xe436eb8c, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

        /// <summary> MEDIASUBTYPE_AIFF </summary>
        public static readonly Guid AIFF = new Guid(0xe436eb8d, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

        /// <summary> MEDIASUBTYPE_dvhd </summary>
        public static readonly Guid dvhd = new Guid(0x64687664, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_dvsl </summary>
        public static readonly Guid dvsl = new Guid(0x6c737664, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_dv25 </summary>
        public static readonly Guid dv25 = new Guid(0x35327664, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_dv50 </summary>
        public static readonly Guid dv50 = new Guid(0x30357664, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_dvh1 </summary>
        public static readonly Guid dvh1 = new Guid(0x31687664, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_Line21_BytePair </summary>
        public static readonly Guid Line21_BytePair = new Guid(0x6e8d4a22, 0x310c, 0x11d0, 0xb7, 0x9a, 0x00, 0xaa, 0x00, 0x37, 0x67, 0xa7);

        /// <summary> MEDIASUBTYPE_Line21_GOPPacket </summary>
        public static readonly Guid Line21_GOPPacket = new Guid(0x6e8d4a23, 0x310c, 0x11d0, 0xb7, 0x9a, 0x00, 0xaa, 0x00, 0x37, 0x67, 0xa7);

        /// <summary> MEDIASUBTYPE_Line21_VBIRawData </summary>
        public static readonly Guid Line21_VBIRawData = new Guid(0x6e8d4a24, 0x310c, 0x11d0, 0xb7, 0x9a, 0x00, 0xaa, 0x00, 0x37, 0x67, 0xa7);

        /// <summary> MEDIASUBTYPE_TELETEXT </summary>
        public static readonly Guid TELETEXT = new Guid(0xf72a76e3, 0xeb0a, 0x11d0, 0xac, 0xe4, 0x00, 0x00, 0xc0, 0xcc, 0x16, 0xba);

        /// <summary> MEDIASUBTYPE_WSS </summary>
        public static readonly Guid WSS = new Guid(0x2791D576, 0x8E7A, 0x466F, 0x9E, 0x90, 0x5D, 0x3F, 0x30, 0x83, 0x73, 0x8B);

        /// <summary> MEDIASUBTYPE_VPS </summary>
        public static readonly Guid VPS = new Guid(0xa1b3f620, 0x9792, 0x4d8d, 0x81, 0xa4, 0x86, 0xaf, 0x25, 0x77, 0x20, 0x90);

        /// <summary> MEDIASUBTYPE_DRM_Audio </summary>
        public static readonly Guid DRM_Audio = new Guid(0x00000009, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_IEEE_FLOAT </summary>
        public static readonly Guid IEEE_FLOAT = new Guid(0x00000003, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_DOLBY_AC3_SPDIF </summary>
        public static readonly Guid DOLBY_AC3_SPDIF = new Guid(0x00000092, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_RAW_SPORT </summary>
        public static readonly Guid RAW_SPORT = new Guid(0x00000240, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_SPDIF_TAG_241h </summary>
        public static readonly Guid SPDIF_TAG_241h = new Guid(0x00000241, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_DssVideo </summary>
        public static readonly Guid DssVideo = new Guid(0xa0af4f81, 0xe163, 0x11d0, 0xba, 0xd9, 0x00, 0x60, 0x97, 0x44, 0x11, 0x1a);

        /// <summary> MEDIASUBTYPE_DssAudio </summary>
        public static readonly Guid DssAudio = new Guid(0xa0af4f82, 0xe163, 0x11d0, 0xba, 0xd9, 0x00, 0x60, 0x97, 0x44, 0x11, 0x1a);

        /// <summary> MEDIASUBTYPE_VPVideo </summary>
        public static readonly Guid VPVideo = new Guid(0x5a9b6a40, 0x1a22, 0x11d1, 0xba, 0xd9, 0x00, 0x60, 0x97, 0x44, 0x11, 0x1a);

        /// <summary> MEDIASUBTYPE_VPVBI </summary>
        public static readonly Guid VPVBI = new Guid(0x5a9b6a41, 0x1a22, 0x11d1, 0xba, 0xd9, 0x00, 0x60, 0x97, 0x44, 0x11, 0x1a);

        /// <summary> MEDIASUBTYPE_AnalogVideo_NTSC_M </summary>
        public static readonly Guid AnalogVideo_NTSC_M = new Guid(0x0482dde2, 0x7817, 0x11cf, 0x8a, 0x03, 0x00, 0xaa, 0x00, 0x6e, 0xcb, 0x65);

        /// <summary> MEDIASUBTYPE_AnalogVideo_PAL_B </summary>
        public static readonly Guid AnalogVideo_PAL_B = new Guid(0x0482dde5, 0x7817, 0x11cf, 0x8a, 0x03, 0x00, 0xaa, 0x00, 0x6e, 0xcb, 0x65);

        /// <summary> MEDIASUBTYPE_AnalogVideo_PAL_D </summary>
        public static readonly Guid AnalogVideo_PAL_D = new Guid(0x0482dde6, 0x7817, 0x11cf, 0x8a, 0x03, 0x00, 0xaa, 0x00, 0x6e, 0xcb, 0x65);

        /// <summary> MEDIASUBTYPE_AnalogVideo_PAL_G </summary>
        public static readonly Guid AnalogVideo_PAL_G = new Guid(0x0482dde7, 0x7817, 0x11cf, 0x8a, 0x03, 0x00, 0xaa, 0x00, 0x6e, 0xcb, 0x65);

        /// <summary> MEDIASUBTYPE_AnalogVideo_PAL_H </summary>
        public static readonly Guid AnalogVideo_PAL_H = new Guid(0x0482dde8, 0x7817, 0x11cf, 0x8a, 0x03, 0x00, 0xaa, 0x00, 0x6e, 0xcb, 0x65);

        /// <summary> MEDIASUBTYPE_AnalogVideo_PAL_I </summary>
        public static readonly Guid AnalogVideo_PAL_I = new Guid(0x0482dde9, 0x7817, 0x11cf, 0x8a, 0x03, 0x00, 0xaa, 0x00, 0x6e, 0xcb, 0x65);

        /// <summary> MEDIASUBTYPE_AnalogVideo_PAL_M </summary>
        public static readonly Guid AnalogVideo_PAL_M = new Guid(0x0482ddea, 0x7817, 0x11cf, 0x8a, 0x03, 0x00, 0xaa, 0x00, 0x6e, 0xcb, 0x65);

        /// <summary> MEDIASUBTYPE_AnalogVideo_PAL_N </summary>
        public static readonly Guid AnalogVideo_PAL_N = new Guid(0x0482ddeb, 0x7817, 0x11cf, 0x8a, 0x03, 0x00, 0xaa, 0x00, 0x6e, 0xcb, 0x65);

        /// <summary> MEDIASUBTYPE_AnalogVideo_PAL_N_COMBO </summary>
        public static readonly Guid AnalogVideo_PAL_N_COMBO = new Guid(0x0482ddec, 0x7817, 0x11cf, 0x8a, 0x03, 0x00, 0xaa, 0x00, 0x6e, 0xcb, 0x65);

        /// <summary> MEDIASUBTYPE_AnalogVideo_SECAM_B </summary>
        public static readonly Guid AnalogVideo_SECAM_B = new Guid(0x0482ddf0, 0x7817, 0x11cf, 0x8a, 0x03, 0x00, 0xaa, 0x00, 0x6e, 0xcb, 0x65);

        /// <summary> MEDIASUBTYPE_AnalogVideo_SECAM_D </summary>
        public static readonly Guid AnalogVideo_SECAM_D = new Guid(0x0482ddf1, 0x7817, 0x11cf, 0x8a, 0x03, 0x00, 0xaa, 0x00, 0x6e, 0xcb, 0x65);

        /// <summary> MEDIASUBTYPE_AnalogVideo_SECAM_G </summary>
        public static readonly Guid AnalogVideo_SECAM_G = new Guid(0x0482ddf2, 0x7817, 0x11cf, 0x8a, 0x03, 0x00, 0xaa, 0x00, 0x6e, 0xcb, 0x65);

        /// <summary> MEDIASUBTYPE_AnalogVideo_SECAM_H </summary>
        public static readonly Guid AnalogVideo_SECAM_H = new Guid(0x0482ddf3, 0x7817, 0x11cf, 0x8a, 0x03, 0x00, 0xaa, 0x00, 0x6e, 0xcb, 0x65);

        /// <summary> MEDIASUBTYPE_AnalogVideo_SECAM_K </summary>
        public static readonly Guid AnalogVideo_SECAM_K = new Guid(0x0482ddf4, 0x7817, 0x11cf, 0x8a, 0x03, 0x00, 0xaa, 0x00, 0x6e, 0xcb, 0x65);

        /// <summary> MEDIASUBTYPE_AnalogVideo_SECAM_K1 </summary>
        public static readonly Guid AnalogVideo_SECAM_K1 = new Guid(0x0482ddf5, 0x7817, 0x11cf, 0x8a, 0x03, 0x00, 0xaa, 0x00, 0x6e, 0xcb, 0x65);

        /// <summary> MEDIASUBTYPE_AnalogVideo_SECAM_L </summary>
        public static readonly Guid AnalogVideo_SECAM_L = new Guid(0x0482ddf6, 0x7817, 0x11cf, 0x8a, 0x03, 0x00, 0xaa, 0x00, 0x6e, 0xcb, 0x65);

        /// <summary> not in uuids.h </summary>
        public static readonly Guid I420 = new Guid(0x30323449, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> WMMEDIASUBTYPE_VIDEOIMAGE </summary>
        public static readonly Guid VideoImage = new Guid(0x1d4a45f2, 0xe5f6, 0x4b44, 0x83, 0x88, 0xf0, 0xae, 0x5c, 0x0e, 0x0c, 0x37);

        /// <summary> WMMEDIASUBTYPE_MPEG2_VIDEO </summary>
        public static readonly Guid Mpeg2Video = new Guid(0xe06d8026, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x5f, 0x6c, 0xbb, 0xea);

        /// <summary> WMMEDIASUBTYPE_WebStream </summary>
        public static readonly Guid WebStream = new Guid(0x776257d4, 0xc627, 0x41cb, 0x8f, 0x81, 0x7a, 0xc7, 0xff, 0x1c, 0x40, 0xcc);

        /// <summary> MEDIASUBTYPE_MPEG2_AUDIO </summary>
        public static readonly Guid Mpeg2Audio = new Guid(0xe06d802b, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x5f, 0x6c, 0xbb, 0xea);

        /// <summary> MEDIASUBTYPE_DOLBY_AC3 </summary>
        public static readonly Guid DolbyAC3 = new Guid(0xe06d802c, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x5f, 0x6c, 0xbb, 0xea);

        /// <summary> MEDIASUBTYPE_DVB_SI </summary>
        public static readonly Guid DvbSI = new Guid(0xe9dd31a3, 0x221d, 0x4adb, 0x85, 0x32, 0x9a, 0xf3, 0x09, 0xc1, 0xa4, 0x08);

        /// <summary> MEDIASUBTYPE_ATSC_SI </summary>
        public static readonly Guid AtscSI = new Guid(0xb3c7397c, 0xd303, 0x414d, 0xb3, 0x3c, 0x4e, 0xd2, 0xc9, 0xd2, 0x97, 0x33);

        /// <summary> MEDIASUBTYPE_MPEG2DATA </summary>
        public static readonly Guid Mpeg2Data = new Guid(0xc892e55b, 0x252d, 0x42b5, 0xa3, 0x16, 0xd9, 0x97, 0xe7, 0xa5, 0xd9, 0x95);

        /// <summary> MEDIASUBTYPE_MPEG2_PROGRAM </summary>
        public static readonly Guid Mpeg2Program = new Guid(0xe06d8022, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x5f, 0x6c, 0xbb, 0xea);

        /// <summary> MEDIASUBTYPE_MPEG2_TRANSPORT </summary>
        public static readonly Guid Mpeg2Transport = new Guid(0xe06d8023, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x5f, 0x6c, 0xbb, 0xea);

        /// <summary> MEDIASUBTYPE_MPEG2_TRANSPORT_STRIDE </summary>
        public static readonly Guid Mpeg2TransportStride = new Guid(0x138aa9a4, 0x1ee2, 0x4c5b, 0x98, 0x8e, 0x19, 0xab, 0xfd, 0xbc, 0x8a, 0x11);

        /// <summary> MEDIASUBTYPE_None </summary>
        public static readonly Guid None = new Guid(0xe436eb8e, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

        /// <summary> MEDIASUBTYPE_H264 </summary>
        public static readonly Guid H264 = new Guid(0x34363248, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_NV24 </summary>
        public static readonly Guid NV24 = new Guid(0x3432564E, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary> MEDIASUBTYPE_708_608Data </summary>
        public static readonly Guid Data708_608 = new Guid(0xaf414bc, 0x4ed2, 0x445e, 0x98, 0x39, 0x8f, 0x9, 0x55, 0x68, 0xab, 0x3c);

        /// <summary> MEDIASUBTYPE_DtvCcData </summary>
        public static readonly Guid DtvCcData = new Guid(0xF52ADDAA, 0x36F0, 0x43F5, 0x95, 0xEA, 0x6D, 0x86, 0x64, 0x84, 0x26, 0x2A);
        
        //追加
          // MS Mepg4 DMO
        public static readonly Guid MS_Mpeg4 = new Guid("{3234504D-0000-0010-8000-00AA00389B71}");
  // DIVX
        public static readonly Guid Divx = new Guid("{58564944-0000-0010-8000-00AA00389B71}");
        public static readonly Guid Divx5 = new Guid("{30355844-0000-0010-8000-00aa00389b71}");
        // VoxWare MetaSound
        public static readonly Guid VoxWare = new Guid("{00000075-0000-0010-8000-00AA00389B71}");

        public static readonly Guid Mpeg_Layer3 = new Guid("{00000055-0000-0010-8000-00AA00389B71}");
        public static readonly Guid LPCM = new Guid("{E06D8032-DB46-11CF-B4D1-00805F6CBBEA}");
        public static readonly Guid Vorbis = new Guid("{8D2FD10B-5841-4A6B-8905-588FEC1ADED9}");
        //30355844-0000-0010-8000-00aa00389b71
    }

    sealed public class FormatType
    {
        private FormatType()
        {
            // Prevent people from trying to instantiate this class
        }

        public static readonly Guid Null = Guid.Empty;

        /// <summary> FORMAT_None </summary>
        public static readonly Guid None = new Guid(0x0F6417D6, 0xc318, 0x11d0, 0xa4, 0x3f, 0x00, 0xa0, 0xc9, 0x22, 0x31, 0x96);

        /// <summary> FORMAT_VideoInfo </summary>
        public static readonly Guid VideoInfo = new Guid(0x05589f80, 0xc356, 0x11ce, 0xbf, 0x01, 0x00, 0xaa, 0x00, 0x55, 0x59, 0x5a);

        /// <summary> FORMAT_VideoInfo2 </summary>
        public static readonly Guid VideoInfo2 = new Guid(0xf72a76A0, 0xeb0a, 0x11d0, 0xac, 0xe4, 0x00, 0x00, 0xc0, 0xcc, 0x16, 0xba);

        /// <summary> FORMAT_WaveFormatEx </summary>
        public static readonly Guid WaveEx = new Guid(0x05589f81, 0xc356, 0x11ce, 0xbf, 0x01, 0x00, 0xaa, 0x00, 0x55, 0x59, 0x5a);

        /// <summary> FORMAT_MPEGVideo </summary>
        public static readonly Guid MpegVideo = new Guid(0x05589f82, 0xc356, 0x11ce, 0xbf, 0x01, 0x00, 0xaa, 0x00, 0x55, 0x59, 0x5a);

        /// <summary> FORMAT_MPEGStreams </summary>
        public static readonly Guid MpegStreams = new Guid(0x05589f83, 0xc356, 0x11ce, 0xbf, 0x01, 0x00, 0xaa, 0x00, 0x55, 0x59, 0x5a);

        /// <summary> FORMAT_DvInfo </summary>
        public static readonly Guid DvInfo = new Guid(0x05589f84, 0xc356, 0x11ce, 0xbf, 0x01, 0x00, 0xaa, 0x00, 0x55, 0x59, 0x5a);

        /// <summary> FORMAT_AnalogVideo </summary>
        public static readonly Guid AnalogVideo = new Guid(0x0482dde0, 0x7817, 0x11cf, 0x8a, 0x03, 0x00, 0xaa, 0x00, 0x6e, 0xcb, 0x65);

        /// <summary> FORMAT_MPEG2Video </summary>
        public static readonly Guid Mpeg2Video = new Guid(0xe06d80e3, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x5f, 0x6c, 0xbb, 0xea);

        /// <summary> FORMAT_DolbyAC3 </summary>
        public static readonly Guid DolbyAC3 = new Guid(0xe06d80e4, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x5f, 0x6c, 0xbb, 0xea);

        /// <summary> FORMAT_MPEG2Audio </summary>
        public static readonly Guid Mpeg2Audio = new Guid(0xe06d80e5, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x5f, 0x6c, 0xbb, 0xea);

        /// <summary> FORMAT_525WSS </summary>
        public static readonly Guid WSS525 = new Guid(0xc7ecf04d, 0x4582, 0x4869, 0x9a, 0xbb, 0xbf, 0xb5, 0x23, 0xb6, 0x2e, 0xdf);
//追加
        public static readonly Guid DVD_LPCMAudio = new Guid(0xE06D80E6, 0xDB46, 0x11CF, 0xB4, 0xD1, 0x00, 0x80, 0x005F, 0x6C, 0xBB, 0xEA);

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VideoInfoHeader
    {
        public Rectangle SrcRect;
        public Rectangle TargetRect;
        public int BitRate;
        public int BitErrorRate;
        public long AvgTimePerFrame;
        public BITMAPINFOHEADER BmiHeader;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VideoInfoHeader2
    {
        public Rectangle SrcRect;
        public Rectangle TargetRect;
        public int BitRate;
        public int BitErrorRate;
        public long AvgTimePerFrame;
        public AMInterlace InterlaceFlags;
        public AMCopyProtect CopyProtectFlags;
        public int PictAspectRatioX;
        public int PictAspectRatioY;
        public AMControl ControlFlags;
        public int Reserved2;
        public BITMAPINFOHEADER BmiHeader;
    }

    [Flags]
    public enum AMInterlace
    {
        None = 0,
        IsInterlaced = 0x00000001,
        OneFieldPerSample = 0x00000002,
        Field1First = 0x00000004,
        Unused = 0x00000008,
        FieldPatternMask = 0x00000030,
        FieldPatField1Only = 0x00000000,
        FieldPatField2Only = 0x00000010,
        FieldPatBothRegular = 0x00000020,
        FieldPatBothIrregular = 0x00000030,
        DisplayModeMask = 0x000000c0,
        DisplayModeBobOnly = 0x00000000,
        DisplayModeWeaveOnly = 0x00000040,
        DisplayModeBobOrWeave = 0x00000080,
    }

    public enum AMCopyProtect
    {
        None = 0,
        RestrictDuplication = 0x00000001
    }

    [Flags]
    public enum AMControl
    {
        None = 0,
        Used = 0x00000001,
        PadTo4x3 = 0x00000002,
        PadTo16x9 = 0x00000004,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WaveFormatEx
    {
        public short wFormatTag;
        public short nChannels;
        public int nSamplesPerSec;
        public int nAvgBytesPerSec;
        public short nBlockAlign;
        public short wBitsPerSample;
        public short cbSize;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MPEG1VideoInfo
    {
        public VideoInfoHeader hdr;
        public int dwStartTimeCode;
        public int cbSequenceHeader;
        public byte bSequenceHeader;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MPEG2VideoInfo
    {
        public VideoInfoHeader2 hdr;
        public int dwStartTimeCode;
        public int cbSequenceHeader;
        public int dwProfile;
        public int dwLevel;
        public int dwFlags;
        public byte bSequenceHeader;
    }

    [Flags]
    public enum AMDvdGraphFlags
    {
        None = 0,
        HWDecPrefer = 0x01,
        HWDecOnly = 0x02,
        SWDecPrefer = 0x04,
        SWDecOnly = 0x08,
        NoVPE = 0x100,
        DoNotClear = 0x200,
        VMR9Only = 0x800,
        EvrOnly = 0x1000,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct AMDvdRenderStatus
    {
        public int hrVPEStatus;
        [MarshalAs(UnmanagedType.Bool)]
        public bool bDvdVolInvalid;
        [MarshalAs(UnmanagedType.Bool)]
        public bool bDvdVolUnknown;
        [MarshalAs(UnmanagedType.Bool)]
        public bool bNoLine21In;
        [MarshalAs(UnmanagedType.Bool)]
        public bool bNoLine21Out;
        public int iNumStreams;
        public int iNumStreamsFailed;
        public AMDvdStreamFlags dwFailedStreamsFlag;
    }

    [Flags]
    public enum AMDvdStreamFlags
    {
        None = 0x00,
        Video = 0x01,
        Audio = 0x02,
        SubPic = 0x04
    }

}
