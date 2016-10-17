// Decompiled with JetBrains decompiler
// Type: UnityEngine.ADInterstitialAd
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  [Obsolete("ADInterstitialAd class is deprecated. Please use iOS.ADInterstitialAd instead (UnityUpgradable) -> UnityEngine.iOS.ADInterstitialAd", true)]
  public sealed class ADInterstitialAd
  {
    public static bool isAvailable
    {
      get
      {
        return false;
      }
    }

    public bool loaded
    {
      get
      {
        return false;
      }
    }

    public static event ADInterstitialAd.InterstitialWasLoadedDelegate onInterstitialWasLoaded
    {
      add
      {
      }
      remove
      {
      }
    }

    public ADInterstitialAd(bool autoReload)
    {
    }

    public ADInterstitialAd()
    {
    }

    ~ADInterstitialAd()
    {
    }

    public void Show()
    {
    }

    public void ReloadAd()
    {
    }

    public delegate void InterstitialWasLoadedDelegate();
  }
}
