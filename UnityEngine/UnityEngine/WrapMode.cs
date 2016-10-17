// Decompiled with JetBrains decompiler
// Type: UnityEngine.WrapMode
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

namespace UnityEngine
{
  /// <summary>
  ///   <para>Determines how time is treated outside of the keyframed range of an AnimationClip or AnimationCurve.</para>
  /// </summary>
  public enum WrapMode
  {
    Default = 0,
    Clamp = 1,
    Once = 1,
    Loop = 2,
    PingPong = 4,
    ClampForever = 8,
  }
}
