// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.ConnectionArray
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System.Collections.Generic;

namespace UnityEngine.Networking
{
  internal class ConnectionArray
  {
    private List<NetworkConnection> m_LocalConnections;
    private List<NetworkConnection> m_Connections;

    internal List<NetworkConnection> localConnections
    {
      get
      {
        return this.m_LocalConnections;
      }
    }

    internal List<NetworkConnection> connections
    {
      get
      {
        return this.m_Connections;
      }
    }

    public int Count
    {
      get
      {
        return this.m_Connections.Count;
      }
    }

    public int LocalIndex
    {
      get
      {
        return -this.m_LocalConnections.Count;
      }
    }

    public ConnectionArray()
    {
      this.m_Connections = new List<NetworkConnection>();
      this.m_LocalConnections = new List<NetworkConnection>();
    }

    public int Add(int connId, NetworkConnection conn)
    {
      if (connId < 0)
      {
        if (LogFilter.logWarn)
          Debug.LogWarning((object) ("ConnectionArray Add bad id " + (object) connId));
        return -1;
      }
      if (connId < this.m_Connections.Count && this.m_Connections[connId] != null)
      {
        if (LogFilter.logWarn)
          Debug.LogWarning((object) ("ConnectionArray Add dupe at " + (object) connId));
        return -1;
      }
      while (connId > this.m_Connections.Count - 1)
        this.m_Connections.Add((NetworkConnection) null);
      this.m_Connections[connId] = conn;
      return connId;
    }

    public NetworkConnection Get(int connId)
    {
      if (connId < 0)
        return this.m_LocalConnections[Mathf.Abs(connId) - 1];
      if (connId >= 0 && connId <= this.m_Connections.Count)
        return this.m_Connections[connId];
      if (LogFilter.logWarn)
        Debug.LogWarning((object) ("ConnectionArray Get invalid index " + (object) connId));
      return (NetworkConnection) null;
    }

    public NetworkConnection GetUnsafe(int connId)
    {
      if (connId < 0 || connId > this.m_Connections.Count)
        return (NetworkConnection) null;
      return this.m_Connections[connId];
    }

    public void Remove(int connId)
    {
      if (connId < 0)
        this.m_LocalConnections[Mathf.Abs(connId) - 1] = (NetworkConnection) null;
      else if (connId < 0 || connId > this.m_Connections.Count)
      {
        if (!LogFilter.logWarn)
          return;
        Debug.LogWarning((object) ("ConnectionArray Remove invalid index " + (object) connId));
      }
      else
        this.m_Connections[connId] = (NetworkConnection) null;
    }

    public int AddLocal(NetworkConnection conn)
    {
      this.m_LocalConnections.Add(conn);
      int num = -this.m_LocalConnections.Count;
      conn.connectionId = num;
      return num;
    }

    public bool ContainsPlayer(GameObject player, out NetworkConnection conn)
    {
      conn = (NetworkConnection) null;
      if ((Object) player == (Object) null)
        return false;
      for (int localIndex = this.LocalIndex; localIndex < this.m_Connections.Count; ++localIndex)
      {
        conn = this.Get(localIndex);
        if (conn != null)
        {
          for (int index = 0; index < conn.playerControllers.Count; ++index)
          {
            if (conn.playerControllers[index].IsValid && (Object) conn.playerControllers[index].gameObject == (Object) player)
              return true;
          }
        }
      }
      return false;
    }
  }
}
