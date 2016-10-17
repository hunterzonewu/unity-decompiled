// Decompiled with JetBrains decompiler
// Type: UnityEngine.ADBannerView
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  [Obsolete("ADBannerView class is deprecated. Please use iOS.ADBannerView instead (UnityUpgradable) -> UnityEngine.iOS.ADBannerView", true)]
  public sealed class ADBannerView
  {
    public bool loaded
    {
      get
      {
        return false;
      }
    }

    public bool visible
    {
      get
      {
        return false;
      }
      set
      {
      }
    }

    public ADBannerView.Layout layout
    {
      get
      {
        return ADBannerView.Layout.Top;
      }
      set
      {
      }
    }

    public Vector2 position
    {
      get
      {
        return new Vector2();
      }
      set
      {
      }
    }

    public Vector2 size
    {
      get
      {
        return new Vector2();
      }
    }

    public static event ADBannerView.BannerWasClickedDelegate onBannerWasClicked
    {
      add
      {
      }
      remove
      {
      }
    }

    public static event ADBannerView.BannerWasLoadedDelegate onBannerWasLoaded
    {
      add
      {
      }
      remove
      {
      }
    }

    public ADBannerView(ADBannerView.Type type, ADBannerView.Layout layout)
    {
    }

    public static bool IsAvailable(ADBannerView.Type type)
    {
      return false;
    }

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

    public enum Type
    {
      Banner,
      MediumRect,
    }

    public delegate void BannerWasClickedDelegate();

    public delegate void BannerWasLoadedDelegate();
  }
}
