// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.ChannelPacket
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System;
using UnityEditor;

namespace UnityEngine.Networking
{
  internal struct ChannelPacket
  {
    private int m_Position;
    private byte[] m_Buffer;
    private bool m_IsReliable;

    public ChannelPacket(int packetSize, bool isReliable)
    {
      this.m_Position = 0;
      this.m_Buffer = new byte[packetSize];
      this.m_IsReliable = isReliable;
    }

    public void Reset()
    {
      this.m_Position = 0;
    }

    public bool IsEmpty()
    {
      return this.m_Position == 0;
    }

    public void Write(byte[] bytes, int numBytes)
    {
      Array.Copy((Array) bytes, 0, (Array) this.m_Buffer, this.m_Position, numBytes);
      this.m_Position += numBytes;
    }

    public bool HasSpace(int numBytes)
    {
      return this.m_Position + numBytes <= this.m_Buffer.Length;
    }

    public bool SendToTransport(NetworkConnection conn, int channelId)
    {
      bool flag = true;
      byte error;
      if (!conn.TransportSend(this.m_Buffer, (int) (ushort) this.m_Position, channelId, out error) && (!this.m_IsReliable || (int) error != 4))
      {
        if (LogFilter.logError)
          Debug.LogError((object) ("Failed to send internal buffer channel:" + (object) channelId + " bytesToSend:" + (object) this.m_Position));
        flag = false;
      }
      if ((int) error != 0)
      {
        if (this.m_IsReliable && (int) error == 4)
        {
          NetworkDetailStats.IncrementStat(NetworkDetailStats.NetworkDirection.Outgoing, (short) 30, "msg", 1);
          return false;
        }
        if (LogFilter.logError)
          Debug.LogError((object) ("Send Error: " + (object) error + " channel:" + (object) channelId + " bytesToSend:" + (object) this.m_Position));
        flag = false;
      }
      this.m_Position = 0;
      return flag;
    }
  }
}
