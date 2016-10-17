// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.SyncEventAttribute
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>This is an attribute that can be put on events in NetworkBehaviour classes to allow them to be invoked on client when the event is called on the sserver.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Event)]
  public class SyncEventAttribute : Attribute
  {
    /// <summary>
    ///   <para>The UNET QoS channel that this event should be sent on.</para>
    /// </summary>
    public int channel;
  }
}
