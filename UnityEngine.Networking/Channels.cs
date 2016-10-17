// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.Channels
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>Class containing constants for default network channels.</para>
  /// </summary>
  public class Channels
  {
    /// <summary>
    ///   <para>The id of the default reliable channel used by the UNet HLAPI, This channel is used for state updates and spawning.</para>
    /// </summary>
    public const int DefaultReliable = 0;
    /// <summary>
    ///   <para>The id of the default unreliable channel used for the UNet HLAPI. This channel is used for movement updates.</para>
    /// </summary>
    public const int DefaultUnreliable = 1;
  }
}
