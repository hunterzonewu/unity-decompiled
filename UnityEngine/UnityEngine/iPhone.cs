// Decompiled with JetBrains decompiler
// Type: UnityEngine.iPhone
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  [Obsolete("iPhone class is deprecated. Please use iOS.Device instead (UnityUpgradable) -> UnityEngine.iOS.Device", true)]
  public sealed class iPhone
  {
    public static iPhoneGeneration generation
    {
      get
      {
        return iPhoneGeneration.Unknown;
      }
    }

    public static string vendorIdentifier
    {
      get
      {
        return (string) null;
      }
    }

    public static string advertisingIdentifier
    {
      get
      {
        return (string) null;
      }
    }

    public static bool advertisingTrackingEnabled
    {
      get
      {
        return false;
      }
    }

    public static void SetNoBackupFlag(string path)
    {
    }

    public static void ResetNoBackupFlag(string path)
    {
    }
  }
}
