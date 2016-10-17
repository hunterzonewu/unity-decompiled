// Decompiled with JetBrains decompiler
// Type: UnityEditor.NetworkDetailStats
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine.Networking;

namespace UnityEditor
{
  internal class NetworkDetailStats
  {
    internal static Dictionary<short, NetworkDetailStats.NetworkOperationDetails> m_NetworkOperations = new Dictionary<short, NetworkDetailStats.NetworkOperationDetails>();
    private const int kPacketHistoryTicks = 20;
    private const float kPacketTickInterval = 0.5f;
    private static float s_LastTickTime;

    public static void NewProfilerTick(float newTime)
    {
      if ((double) newTime - (double) NetworkDetailStats.s_LastTickTime <= 0.5)
        return;
      NetworkDetailStats.s_LastTickTime = newTime;
      int tickId = (int) NetworkDetailStats.s_LastTickTime % 20;
      using (Dictionary<short, NetworkDetailStats.NetworkOperationDetails>.ValueCollection.Enumerator enumerator = NetworkDetailStats.m_NetworkOperations.Values.GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.NewProfilerTick(tickId);
      }
    }

    public static void SetStat(NetworkDetailStats.NetworkDirection direction, short msgId, string entryName, int amount)
    {
      NetworkDetailStats.NetworkOperationDetails operationDetails;
      if (NetworkDetailStats.m_NetworkOperations.ContainsKey(msgId))
      {
        operationDetails = NetworkDetailStats.m_NetworkOperations[msgId];
      }
      else
      {
        operationDetails = new NetworkDetailStats.NetworkOperationDetails();
        operationDetails.MsgId = msgId;
        NetworkDetailStats.m_NetworkOperations[msgId] = operationDetails;
      }
      operationDetails.SetStat(direction, entryName, amount);
    }

    public static void IncrementStat(NetworkDetailStats.NetworkDirection direction, short msgId, string entryName, int amount)
    {
      NetworkDetailStats.NetworkOperationDetails operationDetails;
      if (NetworkDetailStats.m_NetworkOperations.ContainsKey(msgId))
      {
        operationDetails = NetworkDetailStats.m_NetworkOperations[msgId];
      }
      else
      {
        operationDetails = new NetworkDetailStats.NetworkOperationDetails();
        operationDetails.MsgId = msgId;
        NetworkDetailStats.m_NetworkOperations[msgId] = operationDetails;
      }
      operationDetails.IncrementStat(direction, entryName, amount);
    }

    public static void ResetAll()
    {
      using (Dictionary<short, NetworkDetailStats.NetworkOperationDetails>.ValueCollection.Enumerator enumerator = NetworkDetailStats.m_NetworkOperations.Values.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          NetworkDetailStats.NetworkOperationDetails current = enumerator.Current;
          NetworkTransport.SetPacketStat(0, (int) current.MsgId, 0, 1);
          NetworkTransport.SetPacketStat(1, (int) current.MsgId, 0, 1);
        }
      }
      NetworkDetailStats.m_NetworkOperations.Clear();
    }

    public enum NetworkDirection
    {
      Incoming,
      Outgoing,
    }

    internal class NetworkStatsSequence
    {
      private int[] m_MessagesPerTick = new int[20];
      public int MessageTotal;

      public void Add(int tick, int amount)
      {
        this.m_MessagesPerTick[tick] += amount;
        this.MessageTotal += amount;
      }

      public void NewProfilerTick(int tick)
      {
        this.MessageTotal -= this.m_MessagesPerTick[tick];
        this.m_MessagesPerTick[tick] = 0;
      }

      public int GetFiveTick(int tick)
      {
        int num = 0;
        for (int index = 0; index < 5; ++index)
          num += this.m_MessagesPerTick[(tick - index + 20) % 20];
        return num / 5;
      }

      public int GetTenTick(int tick)
      {
        int num = 0;
        for (int index = 0; index < 10; ++index)
          num += this.m_MessagesPerTick[(tick - index + 20) % 20];
        return num / 10;
      }
    }

    internal class NetworkOperationEntryDetails
    {
      public NetworkDetailStats.NetworkStatsSequence m_IncomingSequence = new NetworkDetailStats.NetworkStatsSequence();
      public NetworkDetailStats.NetworkStatsSequence m_OutgoingSequence = new NetworkDetailStats.NetworkStatsSequence();
      public string m_EntryName;
      public int m_IncomingTotal;
      public int m_OutgoingTotal;

      public void NewProfilerTick(int tickId)
      {
        this.m_IncomingSequence.NewProfilerTick(tickId);
        this.m_OutgoingSequence.NewProfilerTick(tickId);
      }

      public void Clear()
      {
        this.m_IncomingTotal = 0;
        this.m_OutgoingTotal = 0;
      }

      public void AddStat(NetworkDetailStats.NetworkDirection direction, int amount)
      {
        int tick = (int) NetworkDetailStats.s_LastTickTime % 20;
        switch (direction)
        {
          case NetworkDetailStats.NetworkDirection.Incoming:
            this.m_IncomingTotal += amount;
            this.m_IncomingSequence.Add(tick, amount);
            break;
          case NetworkDetailStats.NetworkDirection.Outgoing:
            this.m_OutgoingTotal += amount;
            this.m_OutgoingSequence.Add(tick, amount);
            break;
        }
      }
    }

    internal class NetworkOperationDetails
    {
      public Dictionary<string, NetworkDetailStats.NetworkOperationEntryDetails> m_Entries = new Dictionary<string, NetworkDetailStats.NetworkOperationEntryDetails>();
      public short MsgId;
      public float totalIn;
      public float totalOut;

      public void NewProfilerTick(int tickId)
      {
        using (Dictionary<string, NetworkDetailStats.NetworkOperationEntryDetails>.ValueCollection.Enumerator enumerator = this.m_Entries.Values.GetEnumerator())
        {
          while (enumerator.MoveNext())
            enumerator.Current.NewProfilerTick(tickId);
        }
        NetworkTransport.SetPacketStat(0, (int) this.MsgId, (int) this.totalIn, 1);
        NetworkTransport.SetPacketStat(1, (int) this.MsgId, (int) this.totalOut, 1);
        this.totalIn = 0.0f;
        this.totalOut = 0.0f;
      }

      public void Clear()
      {
        using (Dictionary<string, NetworkDetailStats.NetworkOperationEntryDetails>.ValueCollection.Enumerator enumerator = this.m_Entries.Values.GetEnumerator())
        {
          while (enumerator.MoveNext())
            enumerator.Current.Clear();
        }
        this.totalIn = 0.0f;
        this.totalOut = 0.0f;
      }

      public void SetStat(NetworkDetailStats.NetworkDirection direction, string entryName, int amount)
      {
        NetworkDetailStats.NetworkOperationEntryDetails operationEntryDetails;
        if (this.m_Entries.ContainsKey(entryName))
        {
          operationEntryDetails = this.m_Entries[entryName];
        }
        else
        {
          operationEntryDetails = new NetworkDetailStats.NetworkOperationEntryDetails();
          operationEntryDetails.m_EntryName = entryName;
          this.m_Entries[entryName] = operationEntryDetails;
        }
        operationEntryDetails.AddStat(direction, amount);
        switch (direction)
        {
          case NetworkDetailStats.NetworkDirection.Incoming:
            this.totalIn = (float) amount;
            break;
          case NetworkDetailStats.NetworkDirection.Outgoing:
            this.totalOut = (float) amount;
            break;
        }
      }

      public void IncrementStat(NetworkDetailStats.NetworkDirection direction, string entryName, int amount)
      {
        NetworkDetailStats.NetworkOperationEntryDetails operationEntryDetails;
        if (this.m_Entries.ContainsKey(entryName))
        {
          operationEntryDetails = this.m_Entries[entryName];
        }
        else
        {
          operationEntryDetails = new NetworkDetailStats.NetworkOperationEntryDetails();
          operationEntryDetails.m_EntryName = entryName;
          this.m_Entries[entryName] = operationEntryDetails;
        }
        operationEntryDetails.AddStat(direction, amount);
        switch (direction)
        {
          case NetworkDetailStats.NetworkDirection.Incoming:
            this.totalIn += (float) amount;
            break;
          case NetworkDetailStats.NetworkDirection.Outgoing:
            this.totalOut += (float) amount;
            break;
        }
      }
    }
  }
}
