// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkMessageHandlers
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System.Collections.Generic;

namespace UnityEngine.Networking
{
  internal class NetworkMessageHandlers
  {
    private Dictionary<short, NetworkMessageDelegate> m_MsgHandlers = new Dictionary<short, NetworkMessageDelegate>();

    internal void RegisterHandlerSafe(short msgType, NetworkMessageDelegate handler)
    {
      if (handler == null)
      {
        if (!LogFilter.logError)
          return;
        Debug.LogError((object) ("RegisterHandlerSafe id:" + (object) msgType + " handler is null"));
      }
      else
      {
        if (LogFilter.logDebug)
          Debug.Log((object) ("RegisterHandlerSafe id:" + (object) msgType + " handler:" + handler.Method.Name));
        if (this.m_MsgHandlers.ContainsKey(msgType))
          return;
        this.m_MsgHandlers.Add(msgType, handler);
      }
    }

    public void RegisterHandler(short msgType, NetworkMessageDelegate handler)
    {
      if (handler == null)
      {
        if (!LogFilter.logError)
          return;
        Debug.LogError((object) ("RegisterHandler id:" + (object) msgType + " handler is null"));
      }
      else if ((int) msgType <= 31)
      {
        if (!LogFilter.logError)
          return;
        Debug.LogError((object) ("RegisterHandler: Cannot replace system message handler " + (object) msgType));
      }
      else
      {
        if (this.m_MsgHandlers.ContainsKey(msgType))
        {
          if (LogFilter.logDebug)
            Debug.Log((object) ("RegisterHandler replacing " + (object) msgType));
          this.m_MsgHandlers.Remove(msgType);
        }
        if (LogFilter.logDebug)
          Debug.Log((object) ("RegisterHandler id:" + (object) msgType + " handler:" + handler.Method.Name));
        this.m_MsgHandlers.Add(msgType, handler);
      }
    }

    public void UnregisterHandler(short msgType)
    {
      this.m_MsgHandlers.Remove(msgType);
    }

    internal NetworkMessageDelegate GetHandler(short msgType)
    {
      if (this.m_MsgHandlers.ContainsKey(msgType))
        return this.m_MsgHandlers[msgType];
      return (NetworkMessageDelegate) null;
    }

    internal Dictionary<short, NetworkMessageDelegate> GetHandlers()
    {
      return this.m_MsgHandlers;
    }

    internal void ClearMessageHandlers()
    {
      this.m_MsgHandlers.Clear();
    }
  }
}
