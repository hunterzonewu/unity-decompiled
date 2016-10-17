// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkSettingsAttribute
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>This attribute is used to configure the network settings of scripts that are derived from the NetworkBehaviour base class.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Class)]
  public class NetworkSettingsAttribute : Attribute
  {
    /// <summary>
    ///   <para>The sendInterval control how frequently updates are sent for this script.</para>
    /// </summary>
    public float sendInterval = 0.1f;
    /// <summary>
    ///   <para>The QoS channel to use for updates for this script.</para>
    /// </summary>
    public int channel;
  }
}
