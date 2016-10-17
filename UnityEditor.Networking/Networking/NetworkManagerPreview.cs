// Decompiled with JetBrains decompiler
// Type: UnityEditor.Networking.NetworkManagerPreview
// Assembly: UnityEditor.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6916FF53-78D9-4C64-A56C-55C7AE140DAE
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.Networking.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityEditor.Networking
{
  [CustomPreview(typeof (NetworkManager))]
  internal class NetworkManagerPreview : ObjectPreview
  {
    private const int k_Padding = 4;
    private const int k_ColumnWidth = 120;
    private const int k_RowHeight = 16;
    private NetworkManager m_Manager;
    private GUIContent m_Title;
    protected GUIContent m_ShowServerMessagesLabel;
    protected GUIContent m_ShowClientMessagesLabel;

    public override void Initialize(Object[] targets)
    {
      base.Initialize(targets);
      this.GetNetworkInformation(this.target as NetworkManager);
      this.m_ShowServerMessagesLabel = new GUIContent("Server Message Handlers:", "Registered network message handler functions");
      this.m_ShowClientMessagesLabel = new GUIContent("Client Message Handlers:", "Registered network message handler functions");
    }

    public override GUIContent GetPreviewTitle()
    {
      if (this.m_Title == null)
        this.m_Title = new GUIContent("NetworkManager Message Handlers");
      return this.m_Title;
    }

    public override bool HasPreviewGUI()
    {
      return (Object) this.m_Manager != (Object) null;
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
      if (Event.current.type != EventType.Repaint || (Object) this.m_Manager == (Object) null)
        return;
      int posY1 = (int) ((double) r.yMin + 4.0);
      int posY2 = this.ShowServerMessageHandlers(r, posY1);
      this.ShowClientMessageHandlers(r, posY2);
    }

    private static string FormatHandler(KeyValuePair<short, NetworkMessageDelegate> handler)
    {
      return string.Format("{0}:{1}()", (object) handler.Value.Method.DeclaringType.Name, (object) handler.Value.Method.Name);
    }

    private int ShowServerMessageHandlers(Rect r, int posY)
    {
      if (NetworkServer.handlers.Count == 0)
        return posY;
      GUI.Label(new Rect(r.xMin + 4f, (float) posY, 400f, 16f), this.m_ShowServerMessagesLabel);
      posY += 16;
      using (Dictionary<short, NetworkMessageDelegate>.Enumerator enumerator = NetworkServer.handlers.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<short, NetworkMessageDelegate> current = enumerator.Current;
          GUI.Label(new Rect(r.xMin + 16f, (float) posY, 400f, 16f), MsgType.MsgTypeToString(current.Key));
          GUI.Label(new Rect((float) ((double) r.xMin + 16.0 + 120.0), (float) posY, 400f, 16f), NetworkManagerPreview.FormatHandler(current));
          posY += 16;
        }
      }
      return posY;
    }

    private int ShowClientMessageHandlers(Rect r, int posY)
    {
      if (NetworkClient.allClients.Count == 0)
        return posY;
      NetworkClient allClient = NetworkClient.allClients[0];
      if (allClient == null)
        return posY;
      GUI.Label(new Rect(r.xMin + 4f, (float) posY, 400f, 16f), this.m_ShowClientMessagesLabel);
      posY += 16;
      using (Dictionary<short, NetworkMessageDelegate>.Enumerator enumerator = allClient.handlers.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<short, NetworkMessageDelegate> current = enumerator.Current;
          GUI.Label(new Rect(r.xMin + 16f, (float) posY, 400f, 16f), MsgType.MsgTypeToString(current.Key));
          GUI.Label(new Rect((float) ((double) r.xMin + 16.0 + 120.0), (float) posY, 400f, 16f), NetworkManagerPreview.FormatHandler(current));
          posY += 16;
        }
      }
      return posY;
    }

    private void GetNetworkInformation(NetworkManager man)
    {
      this.m_Manager = man;
    }
  }
}
