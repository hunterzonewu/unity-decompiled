// Decompiled with JetBrains decompiler
// Type: UnityEngine.Analytics.UnityAnalyticsManager
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine.Analytics
{
  internal class UnityAnalyticsManager
  {
    public static extern string unityAdsId { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern bool unityAdsTrackingEnabled { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern string deviceUniqueIdentifier { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }
  }
}
