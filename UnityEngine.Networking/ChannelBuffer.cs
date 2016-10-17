// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.ChannelBuffer
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System;
using System.Collections.Generic;
using UnityEditor;

namespace UnityEngine.Networking
{
  internal class ChannelBuffer : IDisposable
  {
    private static NetworkWriter s_SendWriter = new NetworkWriter();
    public float maxDelay = 0.01f;
    private float m_LastBufferedMessageCountTimer = Time.realtimeSinceStartup;
    private const int k_MaxFreePacketCount = 512;
    private const int k_MaxPendingPacketCount = 16;
    private const int k_PacketHeaderReserveSize = 100;
    private NetworkConnection m_Connection;
    private ChannelPacket m_CurrentPacket;
    private float m_LastFlushTime;
    private byte m_ChannelId;
    private int m_MaxPacketSize;
    private bool m_IsReliable;
    private bool m_IsBroken;
    private int m_MaxPendingPacketCount;
    private Queue<ChannelPacket> m_PendingPackets;
    private static List<ChannelPacket> s_FreePackets;
    internal static int pendingPacketCount;
    private bool m_Disposed;

    public int numMsgsOut { get; private set; }

    public int numBufferedMsgsOut { get; private set; }

    public int numBytesOut { get; private set; }

    public int numMsgsIn { get; private set; }

    public int numBytesIn { get; private set; }

    public int numBufferedPerSecond { get; private set; }

    public int lastBufferedPerSecond { get; private set; }

    public ChannelBuffer(NetworkConnection conn, int bufferSize, byte cid, bool isReliable)
    {
      this.m_Connection = conn;
      this.m_MaxPacketSize = bufferSize - 100;
      this.m_CurrentPacket = new ChannelPacket(this.m_MaxPacketSize, isReliable);
      this.m_ChannelId = cid;
      this.m_MaxPendingPacketCount = 16;
      this.m_IsReliable = isReliable;
      if (!isReliable)
        return;
      this.m_PendingPackets = new Queue<ChannelPacket>();
      if (ChannelBuffer.s_FreePackets != null)
        return;
      ChannelBuffer.s_FreePackets = new List<ChannelPacket>();
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!this.m_Disposed && disposing && this.m_PendingPackets != null)
      {
        while (this.m_PendingPackets.Count > 0)
        {
          --ChannelBuffer.pendingPacketCount;
          ChannelPacket channelPacket = this.m_PendingPackets.Dequeue();
          if (ChannelBuffer.s_FreePackets.Count < 512)
            ChannelBuffer.s_FreePackets.Add(channelPacket);
        }
        this.m_PendingPackets.Clear();
      }
      this.m_Disposed = true;
    }

    public bool SetOption(ChannelOption option, int value)
    {
      if (option != ChannelOption.MaxPendingBuffers)
        return false;
      if (!this.m_IsReliable)
      {
        if (LogFilter.logError)
          Debug.LogError((object) ("Cannot set MaxPendingBuffers on unreliable channel " + (object) this.m_ChannelId));
        return false;
      }
      if (value < 0 || value >= 512)
      {
        if (LogFilter.logError)
          Debug.LogError((object) ("Invalid MaxPendingBuffers for channel " + (object) this.m_ChannelId + ". Must be greater than zero and less than " + (object) 512));
        return false;
      }
      this.m_MaxPendingPacketCount = value;
      return true;
    }

    public void CheckInternalBuffer()
    {
      if ((double) Time.realtimeSinceStartup - (double) this.m_LastFlushTime > (double) this.maxDelay && !this.m_CurrentPacket.IsEmpty())
      {
        this.SendInternalBuffer();
        this.m_LastFlushTime = Time.realtimeSinceStartup;
      }
      if ((double) Time.realtimeSinceStartup - (double) this.m_LastBufferedMessageCountTimer <= 1.0)
        return;
      this.lastBufferedPerSecond = this.numBufferedPerSecond;
      this.numBufferedPerSecond = 0;
      this.m_LastBufferedMessageCountTimer = Time.realtimeSinceStartup;
    }

    public bool SendWriter(NetworkWriter writer)
    {
      return this.SendBytes(writer.AsArraySegment().Array, writer.AsArraySegment().Count);
    }

    public bool Send(short msgType, MessageBase msg)
    {
      ChannelBuffer.s_SendWriter.StartMessage(msgType);
      msg.Serialize(ChannelBuffer.s_SendWriter);
      ChannelBuffer.s_SendWriter.FinishMessage();
      ++this.numMsgsOut;
      return this.SendWriter(ChannelBuffer.s_SendWriter);
    }

    internal bool SendBytes(byte[] bytes, int bytesToSend)
    {
      NetworkDetailStats.IncrementStat(NetworkDetailStats.NetworkDirection.Outgoing, (short) 28, "msg", 1);
      if (bytesToSend <= 0)
      {
        if (LogFilter.logError)
          Debug.LogError((object) "ChannelBuffer:SendBytes cannot send zero bytes");
        return false;
      }
      if (bytesToSend > this.m_MaxPacketSize)
      {
        if (LogFilter.logError)
          Debug.LogError((object) ("Failed to send big message of " + (object) bytesToSend + " bytes. The maximum is " + (object) this.m_MaxPacketSize + " bytes on this channel."));
        return false;
      }
      if (!this.m_CurrentPacket.HasSpace(bytesToSend))
      {
        if (this.m_IsReliable)
        {
          if (this.m_PendingPackets.Count == 0)
          {
            if (!this.m_CurrentPacket.SendToTransport(this.m_Connection, (int) this.m_ChannelId))
              this.QueuePacket();
            this.m_CurrentPacket.Write(bytes, bytesToSend);
            return true;
          }
          if (this.m_PendingPackets.Count >= this.m_MaxPendingPacketCount)
          {
            if (!this.m_IsBroken && LogFilter.logError)
              Debug.LogError((object) ("ChannelBuffer buffer limit of " + (object) this.m_PendingPackets.Count + " packets reached."));
            this.m_IsBroken = true;
            return false;
          }
          this.QueuePacket();
          this.m_CurrentPacket.Write(bytes, bytesToSend);
          return true;
        }
        if (!this.m_CurrentPacket.SendToTransport(this.m_Connection, (int) this.m_ChannelId))
        {
          if (LogFilter.logError)
            Debug.Log((object) ("ChannelBuffer SendBytes no space on unreliable channel " + (object) this.m_ChannelId));
          return false;
        }
        this.m_CurrentPacket.Write(bytes, bytesToSend);
        return true;
      }
      this.m_CurrentPacket.Write(bytes, bytesToSend);
      if ((double) this.maxDelay == 0.0)
        return this.SendInternalBuffer();
      return true;
    }

    private void QueuePacket()
    {
      ++ChannelBuffer.pendingPacketCount;
      this.m_PendingPackets.Enqueue(this.m_CurrentPacket);
      this.m_CurrentPacket = this.AllocPacket();
    }

    private ChannelPacket AllocPacket()
    {
      NetworkDetailStats.SetStat(NetworkDetailStats.NetworkDirection.Outgoing, (short) 31, "msg", ChannelBuffer.pendingPacketCount);
      if (ChannelBuffer.s_FreePackets.Count == 0)
        return new ChannelPacket(this.m_MaxPacketSize, this.m_IsReliable);
      ChannelPacket freePacket = ChannelBuffer.s_FreePackets[ChannelBuffer.s_FreePackets.Count - 1];
      ChannelBuffer.s_FreePackets.RemoveAt(ChannelBuffer.s_FreePackets.Count - 1);
      freePacket.Reset();
      return freePacket;
    }

    private static void FreePacket(ChannelPacket packet)
    {
      NetworkDetailStats.SetStat(NetworkDetailStats.NetworkDirection.Outgoing, (short) 31, "msg", ChannelBuffer.pendingPacketCount);
      if (ChannelBuffer.s_FreePackets.Count >= 512)
        return;
      ChannelBuffer.s_FreePackets.Add(packet);
    }

    public bool SendInternalBuffer()
    {
      NetworkDetailStats.IncrementStat(NetworkDetailStats.NetworkDirection.Outgoing, (short) 29, "msg", 1);
      if (!this.m_IsReliable || this.m_PendingPackets.Count <= 0)
        return this.m_CurrentPacket.SendToTransport(this.m_Connection, (int) this.m_ChannelId);
      while (this.m_PendingPackets.Count > 0)
      {
        ChannelPacket packet = this.m_PendingPackets.Dequeue();
        if (!packet.SendToTransport(this.m_Connection, (int) this.m_ChannelId))
        {
          this.m_PendingPackets.Enqueue(packet);
          break;
        }
        --ChannelBuffer.pendingPacketCount;
        ChannelBuffer.FreePacket(packet);
        if (this.m_IsBroken && this.m_PendingPackets.Count < this.m_MaxPendingPacketCount / 2)
        {
          if (LogFilter.logWarn)
            Debug.LogWarning((object) "ChannelBuffer recovered from overflow but data was lost.");
          this.m_IsBroken = false;
        }
      }
      return true;
    }
  }
}
