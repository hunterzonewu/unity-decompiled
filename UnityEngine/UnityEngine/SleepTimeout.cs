// Decompiled with JetBrains decompiler
// Type: UnityEngine.SleepTimeout
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

namespace UnityEngine
{
  /// <summary>
  ///   <para>Constants for special values of Screen.sleepTimeout.</para>
  /// </summary>
  public sealed class SleepTimeout
  {
    /// <summary>
    ///   <para>Prevent screen dimming.</para>
    /// </summary>
    public const int NeverSleep = -1;
    /// <summary>
    ///   <para>Set the sleep timeout to whatever the user has specified in the system settings.</para>
    /// </summary>
    public const int SystemSetting = -2;
  }
}
