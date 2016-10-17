// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.ChannelQOS
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>Defines parameters of channels.</para>
  /// </summary>
  [Serializable]
  public class ChannelQOS
  {
    [SerializeField]
    internal QosType m_Type;

    /// <summary>
    ///   <para>Channel quality of service.</para>
    /// </summary>
    public QosType QOS
    {
      get
      {
        return this.m_Type;
      }
    }

    /// <summary>
    ///   <para>UnderlyingModel.MemDoc.MemDocModel.</para>
    /// </summary>
    /// <param name="value">Requested type of quality of service (default Unreliable).</param>
    /// <param name="channel">Copy constructor.</param>
    public ChannelQOS(QosType value)
    {
      this.m_Type = value;
    }

    /// <summary>
    ///   <para>UnderlyingModel.MemDoc.MemDocModel.</para>
    /// </summary>
    /// <param name="value">Requested type of quality of service (default Unreliable).</param>
    /// <param name="channel">Copy constructor.</param>
    public ChannelQOS()
    {
      this.m_Type = QosType.Unreliable;
    }

    /// <summary>
    ///   <para>UnderlyingModel.MemDoc.MemDocModel.</para>
    /// </summary>
    /// <param name="value">Requested type of quality of service (default Unreliable).</param>
    /// <param name="channel">Copy constructor.</param>
    public ChannelQOS(ChannelQOS channel)
    {
      if (channel == null)
        throw new NullReferenceException("channel is not defined");
      this.m_Type = channel.m_Type;
    }
  }
}
