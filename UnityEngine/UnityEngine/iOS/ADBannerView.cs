// Decompiled with JetBrains decompiler
// Type: UnityEngine.iOS.ADBannerView
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.iOS
{
  /// <summary>
  ///   <para>ADBannerView is a wrapper around the ADBannerView class found in the Apple iAd framework and is only available on iOS.</para>
  /// </summary>
  [RequiredByNativeCode]
  public sealed class ADBannerView
  {
    private ADBannerView.Layout _layout;
    private IntPtr _bannerView;
    private static bool _AlwaysFalseDummy;

    /// <summary>
    ///   <para>Checks if banner contents are loaded.</para>
    /// </summary>
    public bool loaded
    {
      get
      {
        return ADBannerView.Native_BannerAdLoaded(this._bannerView);
      }
    }

    /// <summary>
    ///   <para>Banner visibility. Initially banner is not visible.</para>
    /// </summary>
    public bool visible
    {
      get
      {
        return ADBannerView.Native_BannerAdVisible(this._bannerView);
      }
      set
      {
        ADBannerView.Native_ShowBanner(this._bannerView, value);
      }
    }

    /// <summary>
    ///   <para>Banner layout.</para>
    /// </summary>
    public ADBannerView.Layout layout
    {
      get
      {
        return this._layout;
      }
      set
      {
        this._layout = value;
        ADBannerView.Native_LayoutBanner(this._bannerView, (int) this._layout);
      }
    }

    /// <summary>
    ///   <para>The position of the banner view.</para>
    /// </summary>
    public Vector2 position
    {
      get
      {
        Vector2 pos;
        ADBannerView.Native_BannerPosition(this._bannerView, out pos);
        return this.OSToScreenCoords(pos);
      }
      set
      {
        ADBannerView.Native_MoveBanner(this._bannerView, new Vector2(value.x / (float) Screen.width, value.y / (float) Screen.height));
      }
    }

    /// <summary>
    ///   <para>The size of the banner view.</para>
    /// </summary>
    public Vector2 size
    {
      get
      {
        Vector2 pos;
        ADBannerView.Native_BannerSize(this._bannerView, out pos);
        return this.OSToScreenCoords(pos);
      }
    }

    public static event ADBannerView.BannerWasClickedDelegate onBannerWasClicked;

    public static event ADBannerView.BannerWasLoadedDelegate onBannerWasLoaded;

    public static event ADBannerView.BannerFailedToLoadDelegate onBannerFailedToLoad;

    public ADBannerView(ADBannerView.Type type, ADBannerView.Layout layout)
    {
      if (ADBannerView._AlwaysFalseDummy)
      {
        ADBannerView.FireBannerWasClicked();
        ADBannerView.FireBannerWasLoaded();
        ADBannerView.FireBannerFailedToLoad();
      }
      this._bannerView = ADBannerView.Native_CreateBanner((int) type, (int) layout);
    }

    ~ADBannerView()
    {
      ADBannerView.Native_DestroyBanner(this._bannerView);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern IntPtr Native_CreateBanner(int type, int layout);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Native_ShowBanner(IntPtr view, bool show);

    private static void Native_MoveBanner(IntPtr view, Vector2 pos)
    {
      ADBannerView.INTERNAL_CALL_Native_MoveBanner(view, ref pos);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Native_MoveBanner(IntPtr view, ref Vector2 pos);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Native_LayoutBanner(IntPtr view, int layout);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool Native_BannerTypeAvailable(int type);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Native_BannerPosition(IntPtr view, out Vector2 pos);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Native_BannerSize(IntPtr view, out Vector2 pos);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool Native_BannerAdLoaded(IntPtr view);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool Native_BannerAdVisible(IntPtr view);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Native_DestroyBanner(IntPtr view);

    public static bool IsAvailable(ADBannerView.Type type)
    {
      return ADBannerView.Native_BannerTypeAvailable((int) type);
    }

    private Vector2 OSToScreenCoords(Vector2 v)
    {
      return new Vector2(v.x * (float) Screen.width, v.y * (float) Screen.height);
    }

    [RequiredByNativeCode]
    private static void FireBannerWasClicked()
    {
      if (ADBannerView.onBannerWasClicked == null)
        return;
      ADBannerView.onBannerWasClicked();
    }

    [RequiredByNativeCode]
    private static void FireBannerWasLoaded()
    {
      if (ADBannerView.onBannerWasLoaded == null)
        return;
      ADBannerView.onBannerWasLoaded();
    }

    [RequiredByNativeCode]
    private static void FireBannerFailedToLoad()
    {
      if (ADBannerView.onBannerFailedToLoad == null)
        return;
      ADBannerView.onBannerFailedToLoad();
    }

    /// <summary>
    ///   <para>Specifies how banner should be layed out on screen.</para>
    /// </summary>
    public enum Layout
    {
      Manual = -1,
      Top = 0,
      TopLeft = 0,
      Bottom = 1,
      BottomLeft = 1,
      CenterLeft = 2,
      TopRight = 4,
      BottomRight = 5,
      CenterRight = 6,
      TopCenter = 8,
      BottomCenter = 9,
      Center = 10,
    }

    /// <summary>
    ///   <para>The type of the banner view.</para>
    /// </summary>
    public enum Type
    {
      Banner,
      MediumRect,
    }

    /// <summary>
    ///   <para>Will be fired when banner was clicked.</para>
    /// </summary>
    public delegate void BannerWasClickedDelegate();

    /// <summary>
    ///   <para>Will be fired when banner loaded new ad.</para>
    /// </summary>
    public delegate void BannerWasLoadedDelegate();

    /// <summary>
    ///   <para>Will be fired when banner ad failed to load.</para>
    /// </summary>
    public delegate void BannerFailedToLoadDelegate();
  }
}
