// Decompiled with JetBrains decompiler
// Type: UnityEngine.Advertisements.UnityAdsInternal
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Advertisements
{
  [RequiredByNativeCode]
  internal sealed class UnityAdsInternal
  {
    public static event UnityAdsDelegate onCampaignsAvailable;

    public static event UnityAdsDelegate onCampaignsFetchFailed;

    public static event UnityAdsDelegate onShow;

    public static event UnityAdsDelegate onHide;

    public static event UnityAdsDelegate<string, bool> onVideoCompleted;

    public static event UnityAdsDelegate onVideoStarted;

    public static void RemoveAllEventHandlers()
    {
      UnityAdsInternal.onCampaignsAvailable = (UnityAdsDelegate) null;
      UnityAdsInternal.onCampaignsFetchFailed = (UnityAdsDelegate) null;
      UnityAdsInternal.onShow = (UnityAdsDelegate) null;
      UnityAdsInternal.onHide = (UnityAdsDelegate) null;
      UnityAdsInternal.onVideoCompleted = (UnityAdsDelegate<string, bool>) null;
      UnityAdsInternal.onVideoStarted = (UnityAdsDelegate) null;
    }

    public static void CallUnityAdsCampaignsAvailable()
    {
      UnityAdsDelegate campaignsAvailable = UnityAdsInternal.onCampaignsAvailable;
      if (campaignsAvailable == null)
        return;
      campaignsAvailable();
    }

    public static void CallUnityAdsCampaignsFetchFailed()
    {
      UnityAdsDelegate campaignsFetchFailed = UnityAdsInternal.onCampaignsFetchFailed;
      if (campaignsFetchFailed == null)
        return;
      campaignsFetchFailed();
    }

    public static void CallUnityAdsShow()
    {
      UnityAdsDelegate onShow = UnityAdsInternal.onShow;
      if (onShow == null)
        return;
      onShow();
    }

    public static void CallUnityAdsHide()
    {
      UnityAdsDelegate onHide = UnityAdsInternal.onHide;
      if (onHide == null)
        return;
      onHide();
    }

    public static void CallUnityAdsVideoCompleted(string rewardItemKey, bool skipped)
    {
      UnityAdsDelegate<string, bool> onVideoCompleted = UnityAdsInternal.onVideoCompleted;
      if (onVideoCompleted == null)
        return;
      onVideoCompleted(rewardItemKey, skipped);
    }

    public static void CallUnityAdsVideoStarted()
    {
      UnityAdsDelegate onVideoStarted = UnityAdsInternal.onVideoStarted;
      if (onVideoStarted == null)
        return;
      onVideoStarted();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void RegisterNative();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Init(string gameId, bool testModeEnabled, bool debugModeEnabled, string unityVersion);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool Show(string zoneId, string rewardItemKey, string options);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool CanShowAds(string zoneId);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetLogLevel(int logLevel);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetCampaignDataURL(string url);
  }
}
