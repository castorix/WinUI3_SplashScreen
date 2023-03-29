using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
using GlobalStructures;

namespace MFPlay
{
    internal class MFPlayTools
    {
        [DllImport("Mfplay.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern HRESULT MFPCreateMediaPlayer(string pwszURL, bool fStartPlayback, MFP_CREATION_OPTIONS creationOptions,
            IMFPMediaPlayerCallback pCallback, IntPtr hWnd,out IMFPMediaPlayer ppMediaPlayer );

        public enum MFP_CREATION_OPTIONS
        {
            MFP_OPTION_NONE = 0,
            MFP_OPTION_FREE_THREADED_CALLBACK = 0x1,
            MFP_OPTION_NO_MMCSS = 0x2,
            MFP_OPTION_NO_REMOTE_DESKTOP_OPTIMIZATION = 0x4
        };
    }

    [ComImport]
    [Guid("A714590A-58AF-430a-85BF-44F5EC838D85")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMFPMediaPlayer
    {
        HRESULT Play();
        HRESULT Pause();
        HRESULT Stop();
        HRESULT FrameStep();
        HRESULT SetPosition(ref Guid guidPositionType, PROPVARIANT pvPositionValue);
        HRESULT GetPosition(ref Guid guidPositionType, out PROPVARIANT pvPositionValue);
        HRESULT GetDuration(ref Guid guidPositionType, out PROPVARIANT pvDurationValue);
        HRESULT SetRate(float flRate);
        HRESULT GetRate(out float pflRate);
        HRESULT GetSupportedRates(bool fForwardDirection, out float pflSlowestRate, out float pflFastestRate);
        HRESULT GetState(out MFP_MEDIAPLAYER_STATE peState);
        HRESULT CreateMediaItemFromURL(string pwszURL, bool fSync, IntPtr dwUserData, out IMFPMediaItem ppMediaItem);
        HRESULT CreateMediaItemFromObject(IntPtr pIntPtrObj, bool fSync, IntPtr dwUserData, out IMFPMediaItem ppMediaItem);
        HRESULT SetMediaItem(IMFPMediaItem pIMFPMediaItem);
        HRESULT ClearMediaItem();
        HRESULT GetMediaItem(out IMFPMediaItem ppIMFPMediaItem);
        HRESULT GetVolume(out float pflVolume);
        HRESULT SetVolume(float flVolume);
        HRESULT GetBalance(out float pflBalance);
        HRESULT SetBalance(float flBalance);
        HRESULT GetMute(out bool pfMute);
        HRESULT SetMute(bool fMute);
        [PreserveSig]
        HRESULT GetNativeVideoSize(out SIZE pszVideo, out SIZE pszARVideo);
        [PreserveSig]
        HRESULT GetIdealVideoSize(out SIZE pszMin, out SIZE pszMax);
        HRESULT SetVideoSourceRect(MFVideoNormalizedRect pnrcSource);
        HRESULT GetVideoSourceRect(out MFVideoNormalizedRect pnrcSource);
        HRESULT SetAspectRatioMode(uint dwAspectRatioMode);
        HRESULT GetAspectRatioMode(out uint pdwAspectRatioMode);
        HRESULT GetVideoWindow(out IntPtr pIntPtrVideo);
        HRESULT UpdateVideo();
        HRESULT SetBorderColor(uint Clr);
        HRESULT GetBorderColor(out uint pClr);
        HRESULT InsertEffect(IntPtr pEffect, bool fOptional);
        HRESULT RemoveEffect(IntPtr pEffect);
        HRESULT RemoveAllEffects();
        HRESULT Shutdown();
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MFVideoNormalizedRect
    {
        public float left;
        public float top;
        public float right;
        public float bottom;
        public MFVideoNormalizedRect(float Left, float Top, float Right, float Bottom)
        {
            left = Left;
            top = Top;
            right = Right;
            bottom = Bottom;
        }
    }

    public enum MFP_MEDIAPLAYER_STATE
    {
        MFP_MEDIAPLAYER_STATE_EMPTY = 0,
        MFP_MEDIAPLAYER_STATE_STOPPED = 0x1,
        MFP_MEDIAPLAYER_STATE_PLAYING = 0x2,
        MFP_MEDIAPLAYER_STATE_PAUSED = 0x3,
        MFP_MEDIAPLAYER_STATE_SHUTDOWN = 0x4
    }

    [ComImport]
    [Guid("90EB3E6B-ECBF-45cc-B1DA-C6FE3EA70D57")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMFPMediaItem
    {
        HRESULT GetMediaPlayer(out IMFPMediaPlayer ppMediaPlayer);
        HRESULT GetURL(out string ppwszURL);
        HRESULT GetObject(out IntPtr ppIUnknown);
        HRESULT GetUserData(out IntPtr pdwUserData);
        HRESULT SetUserData(IntPtr dwUserData);
        HRESULT GetStartStopPosition(out Guid pGuidStartPositionType, out PROPVARIANT pvStartValue, out Guid pGuidStopPositionType, out PROPVARIANT pvStopValue);
        HRESULT SetStartStopPosition(ref Guid pGuidStartPositionType, PROPVARIANT pvStartValue, ref Guid pGuidStopPositionType, PROPVARIANT pvStopValue);
        HRESULT HasVideo(out bool pfHasVideo, out bool pfSelected);
        HRESULT HasAudio(out bool fHasAudio, out bool pfSelected);
        HRESULT IsProtected(out bool pfProtected);
        HRESULT GetDuration(ref Guid GuidPositionType, out PROPVARIANT pvDurationValue);
        HRESULT GetNumberOfStreams(out uint pdwStreamCount);
        HRESULT GetStreamSelection(uint dwStreamIndex, out bool pfEnabled);
        HRESULT SetStreamSelection(uint dwStreamIndex, bool fEnabled);
        HRESULT GetStreamAttribute(uint dwStreamIndex, ref Guid GuidMFAttribute, out PROPVARIANT pvValue);
        HRESULT GetPresentationAttribute(ref Guid GuidMFAttribute, out PROPVARIANT pvValue);
        HRESULT GetCharacteristics(out MFP_MEDIAITEM_CHARACTERISTICS pCharacteristics);
        HRESULT SetStreamSink(uint dwStreamIndex, IntPtr pMediaSink);
        HRESULT GetMetadata(out IPropertyStore ppMetadataStore);
    }

    public enum MFP_MEDIAITEM_CHARACTERISTICS
    {
        MFP_MEDIAITEM_IS_LIVE = 0x1,
        MFP_MEDIAITEM_CAN_SEEK = 0x2,
        MFP_MEDIAITEM_CAN_PAUSE = 0x4,
        MFP_MEDIAITEM_HAS_SLOW_SEEK = 0x8
    }

    [ComImport, Guid("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPropertyStore
    {
        HRESULT GetCount([Out] out uint propertyCount);
        HRESULT GetAt([In] uint propertyIndex, [Out, MarshalAs(UnmanagedType.Struct)] out PROPERTYKEY key);
        HRESULT GetValue([In, MarshalAs(UnmanagedType.Struct)] ref PROPERTYKEY key, [Out, MarshalAs(UnmanagedType.Struct)] out PROPVARIANT pv);
        HRESULT SetValue([In, MarshalAs(UnmanagedType.Struct)] ref PROPERTYKEY key, [In, MarshalAs(UnmanagedType.Struct)] ref PROPVARIANT pv);
        HRESULT Commit();
    }

    [ComImport, Guid("766C8FFB-5FDB-4fea-A28D-B912996F51BD"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMFPMediaPlayerCallback
    {
        [PreserveSig]
        void OnMediaPlayerEvent(MFP_EVENT_HEADER pEventHeader); 
    };    

    [StructLayout(LayoutKind.Sequential)]
    public class MFP_EVENT_HEADER
    {
        public MFP_EVENT_TYPE eEventType;
        public HRESULT hrEvent;
        public IMFPMediaPlayer pMediaPlayer;
        public MFP_MEDIAPLAYER_STATE eState;
        public IPropertyStore pPropertyStore;
    }

    public enum MFP_EVENT_TYPE
    {
        MFP_EVENT_TYPE_PLAY = 0,
        MFP_EVENT_TYPE_PAUSE = 1,
        MFP_EVENT_TYPE_STOP = 2,
        MFP_EVENT_TYPE_POSITION_SET = 3,
        MFP_EVENT_TYPE_RATE_SET = 4,
        MFP_EVENT_TYPE_MEDIAITEM_CREATED = 5,
        MFP_EVENT_TYPE_MEDIAITEM_SET = 6,
        MFP_EVENT_TYPE_FRAME_STEP = 7,
        MFP_EVENT_TYPE_MEDIAITEM_CLEARED = 8,
        MFP_EVENT_TYPE_MF = 9,
        MFP_EVENT_TYPE_ERROR = 10,
        MFP_EVENT_TYPE_PLAYBACK_ENDED = 11,
        MFP_EVENT_TYPE_ACQUIRE_USER_CREDENTIAL = 12
    }
}
