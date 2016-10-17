// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.LocalClient
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System.Collections.Generic;

namespace UnityEngine.Networking
{
  internal sealed class LocalClient : NetworkClient
  {
    private List<LocalClient.InternalMsg> m_InternalMsgs = new List<LocalClient.InternalMsg>();
    private List<LocalClient.InternalMsg> m_InternalMsgs2 = new List<LocalClient.InternalMsg>();
    private NetworkMessage s_InternalMessage = new NetworkMessage();
    private const int k_InitialFreeMessagePoolSize = 64;
    private Stack<LocalClient.InternalMsg> m_FreeMessages;
    private NetworkServer m_LocalServer;
    private bool m_Connected;

    public override void Disconnect()
    {
      ClientScene.HandleClientDisconnect(this.m_Connection);
      if (this.m_Connected)
      {
        this.PostInternalMessage((short) 33);
        this.m_Connected = false;
      }
      this.m_AsyncConnect = NetworkClient.ConnectState.Disconnected;
      this.m_LocalServer.RemoveLocalClient(this.m_Connection);
    }

    internal void InternalConnectLocalServer(bool generateConnectMsg)
    {
      if (this.m_FreeMessages == null)
      {
        this.m_FreeMessages = new Stack<LocalClient.InternalMsg>();
        for (int index = 0; index < 64; ++index)
          this.m_FreeMessages.Push(new LocalClient.InternalMsg());
      }
      this.m_LocalServer = NetworkServer.instance;
      this.m_Connection = (NetworkConnection) new ULocalConnectionToServer(this.m_LocalServer);
      this.SetHandlers(this.m_Connection);
      this.m_Connection.connectionId = this.m_LocalServer.AddLocalClient(this);
      this.m_AsyncConnect = NetworkClient.ConnectState.Connected;
      NetworkClient.SetActive(true);
      this.RegisterSystemHandlers(true);
      if (generateConnectMsg)
        this.PostInternalMessage((short) 32);
      this.m_Connected = true;
    }

    internal override void Update()
    {
      this.ProcessInternalMessages();
    }

    internal void AddLocalPlayer(PlayerController localPlayer)
    {
      if (LogFilter.logDev)
        Debug.Log((object) ("Local client AddLocalPlayer " + localPlayer.gameObject.name + " conn=" + (object) this.m_Connection.connectionId));
      this.m_Connection.isReady = true;
      this.m_Connection.SetPlayerController(localPlayer);
      NetworkIdentity unetView = localPlayer.unetView;
      if ((Object) unetView != (Object) null)
      {
        ClientScene.SetLocalObject(unetView.netId, localPlayer.gameObject);
        unetView.SetConnectionToServer(this.m_Connection);
      }
      ClientScene.InternalAddPlayer(unetView, localPlayer.playerControllerId);
    }

    private void PostInternalMessage(byte[] buffer, int channelId)
    {
      LocalClient.InternalMsg internalMsg = this.m_FreeMessages.Count != 0 ? this.m_FreeMessages.Pop() : new LocalClient.InternalMsg();
      internalMsg.buffer = buffer;
      internalMsg.channelId = channelId;
      this.m_InternalMsgs.Add(internalMsg);
    }

    private void PostInternalMessage(short msgType)
    {
      NetworkWriter networkWriter = new NetworkWriter();
      networkWriter.StartMessage(msgType);
      networkWriter.FinishMessage();
      this.PostInternalMessage(networkWriter.AsArray(), 0);
    }

    private void ProcessInternalMessages()
    {
      if (this.m_InternalMsgs.Count == 0)
        return;
      List<LocalClient.InternalMsg> internalMsgs = this.m_InternalMsgs;
      this.m_InternalMsgs = this.m_InternalMsgs2;
      using (List<LocalClient.InternalMsg>.Enumerator enumerator = internalMsgs.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          LocalClient.InternalMsg current = enumerator.Current;
          if (this.s_InternalMessage.reader == null)
            this.s_InternalMessage.reader = new NetworkReader(current.buffer);
          else
            this.s_InternalMessage.reader.Replace(current.buffer);
          int num = (int) this.s_InternalMessage.reader.ReadInt16();
          this.s_InternalMessage.channelId = current.channelId;
          this.s_InternalMessage.conn = this.connection;
          this.s_InternalMessage.msgType = this.s_InternalMessage.reader.ReadInt16();
          this.m_Connection.InvokeHandler(this.s_InternalMessage);
          this.m_FreeMessages.Push(current);
          this.connection.lastMessageTime = Time.time;
        }
      }
      this.m_InternalMsgs = internalMsgs;
      this.m_InternalMsgs.Clear();
      using (List<LocalClient.InternalMsg>.Enumerator enumerator = this.m_InternalMsgs2.GetEnumerator())
      {
        while (enumerator.MoveNext())
          this.m_InternalMsgs.Add(enumerator.Current);
      }
      this.m_InternalMsgs2.Clear();
    }

    internal void InvokeHandlerOnClient(short msgType, MessageBase msg, int channelId)
    {
      NetworkWriter writer = new NetworkWriter();
      writer.StartMessage(msgType);
      msg.Serialize(writer);
      writer.FinishMessage();
      this.InvokeBytesOnClient(writer.AsArray(), channelId);
    }

    internal void InvokeBytesOnClient(byte[] buffer, int channelId)
    {
      this.PostInternalMessage(buffer, channelId);
    }

    private struct InternalMsg
    {
      internal byte[] buffer;
      internal int channelId;
    }
  }
}
