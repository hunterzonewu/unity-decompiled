// Decompiled with JetBrains decompiler
// Type: UnityEngine.iPhoneUtils
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  public class iPhoneUtils
  {
    [Obsolete("isApplicationGenuine property is deprecated. Please use Application.genuine instead (UnityUpgradable) -> Application.genuine")]
    public static bool isApplicationGenuine
    {
      get
      {
        return false;
      }
    }

    [Obsolete("isApplicationGenuineAvailable property is deprecated. Please use Application.genuineCheckAvailable instead (UnityUpgradable) -> Application.genuineCheckAvailable")]
    public static bool isApplicationGenuineAvailable
    {
      get
      {
        return false;
      }
    }

    [Obsolete("PlayMovie method is deprecated. Please use Handheld.PlayFullScreenMovie instead (UnityUpgradable) -> [mscorlib] System.Boolean Handheld.PlayFullScreenMovie(*)")]
    public static void PlayMovie(string path, Color bgColor, iPhoneMovieControlMode controlMode, iPhoneMovieScalingMode scalingMode)
    {
    }

    [Obsolete("PlayMovie method is deprecated. Please use Handheld.PlayFullScreenMovie instead (UnityUpgradable) -> [mscorlib] System.Boolean Handheld.PlayFullScreenMovie(*)")]
    public static void PlayMovie(string path, Color bgColor, iPhoneMovieControlMode controlMode)
    {
    }

    [Obsolete("PlayMovie method is deprecated. Please use Handheld.PlayFullScreenMovie instead (UnityUpgradable) -> [mscorlib] System.Boolean Handheld.PlayFullScreenMovie(*)")]
    public static void PlayMovie(string path, Color bgColor)
    {
    }

    [Obsolete("PlayMovieURL method is deprecated. Please use Handheld.PlayFullScreenMovie instead (UnityUpgradable) -> [mscorlib] System.Boolean Handheld.PlayFullScreenMovie(*)")]
    public static void PlayMovieURL(string url, Color bgColor, iPhoneMovieControlMode controlMode, iPhoneMovieScalingMode scalingMode)
    {
    }

    [Obsolete("PlayMovieURL method is deprecated. Please use Handheld.PlayFullScreenMovie instead (UnityUpgradable) -> [mscorlib] System.Boolean Handheld.PlayFullScreenMovie(*)")]
    public static void PlayMovieURL(string url, Color bgColor, iPhoneMovieControlMode controlMode)
    {
    }

    [Obsolete("PlayMovieURL method is deprecated. Please use Handheld.PlayFullScreenMovie instead (UnityUpgradable) -> [mscorlib] System.Boolean Handheld.PlayFullScreenMovie(*)")]
    public static void PlayMovieURL(string url, Color bgColor)
    {
    }

    [Obsolete("Vibrate method is deprecated. Please use Handheld.Vibrate instead (UnityUpgradable) -> Handheld.Vibrate()")]
    public static void Vibrate()
    {
    }
  }
}
