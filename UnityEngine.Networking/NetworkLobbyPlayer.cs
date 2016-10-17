// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkLobbyPlayer
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using UnityEngine.Networking.NetworkSystem;
using UnityEngine.SceneManagement;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>This component works in conjunction with the NetworkLobbyManager to make up the multiplayer lobby system.</para>
  /// </summary>
  [DisallowMultipleComponent]
  [AddComponentMenu("Network/NetworkLobbyPlayer")]
  public class NetworkLobbyPlayer : NetworkBehaviour
  {
    /// <summary>
    ///   <para>This flag controls whether the default UI is shown for the lobby player.</para>
    /// </summary>
    [SerializeField]
    public bool ShowLobbyGUI = true;
    private byte m_Slot;
    private bool m_ReadyToBegin;

    /// <summary>
    ///   <para>The slot within the lobby that this player inhabits.</para>
    /// </summary>
    public byte slot
    {
      get
      {
        return this.m_Slot;
      }
      set
      {
        this.m_Slot = value;
      }
    }

    /// <summary>
    ///   <para>This is a flag that control whether this player is ready for the game to begin.</para>
    /// </summary>
    public bool readyToBegin
    {
      get
      {
        return this.m_ReadyToBegin;
      }
      set
      {
        this.m_ReadyToBegin = value;
      }
    }

    private void Start()
    {
      Object.DontDestroyOnLoad((Object) this.gameObject);
    }

    public override void OnStartClient()
    {
      NetworkLobbyManager singleton = NetworkManager.singleton as NetworkLobbyManager;
      if ((bool) ((Object) singleton))
      {
        singleton.lobbySlots[(int) this.m_Slot] = this;
        this.m_ReadyToBegin = false;
        this.OnClientEnterLobby();
      }
      else
        Debug.LogError((object) "LobbyPlayer could not find a NetworkLobbyManager. The LobbyPlayer requires a NetworkLobbyManager object to function. Make sure that there is one in the scene.");
    }

    /// <summary>
    ///   <para>This is used on clients to tell the server that this player is ready for the game to begin.</para>
    /// </summary>
    public void SendReadyToBeginMessage()
    {
      if (LogFilter.logDebug)
        Debug.Log((object) "NetworkLobbyPlayer SendReadyToBeginMessage");
      NetworkLobbyManager singleton = NetworkManager.singleton as NetworkLobbyManager;
      if (!(bool) ((Object) singleton))
        return;
      singleton.client.Send((short) 43, (MessageBase) new LobbyReadyToBeginMessage()
      {
        slotId = (byte) this.playerControllerId,
        readyState = true
      });
    }

    /// <summary>
    ///   <para>This is used on clients to tell the server that this player is not ready for the game to begin.</para>
    /// </summary>
    public void SendNotReadyToBeginMessage()
    {
      if (LogFilter.logDebug)
        Debug.Log((object) "NetworkLobbyPlayer SendReadyToBeginMessage");
      NetworkLobbyManager singleton = NetworkManager.singleton as NetworkLobbyManager;
      if (!(bool) ((Object) singleton))
        return;
      singleton.client.Send((short) 43, (MessageBase) new LobbyReadyToBeginMessage()
      {
        slotId = (byte) this.playerControllerId,
        readyState = false
      });
    }

    /// <summary>
    ///   <para>This is used on clients to tell the server that the client has switched from the lobby to the GameScene and is ready to play.</para>
    /// </summary>
    public void SendSceneLoadedMessage()
    {
      if (LogFilter.logDebug)
        Debug.Log((object) "NetworkLobbyPlayer SendSceneLoadedMessage");
      NetworkLobbyManager singleton = NetworkManager.singleton as NetworkLobbyManager;
      if (!(bool) ((Object) singleton))
        return;
      IntegerMessage integerMessage = new IntegerMessage((int) this.playerControllerId);
      singleton.client.Send((short) 44, (MessageBase) integerMessage);
    }

    private void OnLevelWasLoaded()
    {
      NetworkLobbyManager singleton = NetworkManager.singleton as NetworkLobbyManager;
      if ((bool) ((Object) singleton) && SceneManager.GetSceneAt(0).name == singleton.lobbyScene || !this.isLocalPlayer)
        return;
      this.SendSceneLoadedMessage();
    }

    /// <summary>
    ///   <para>This removes this player from the lobby.</para>
    /// </summary>
    public void RemovePlayer()
    {
      if (!this.isLocalPlayer || this.m_ReadyToBegin)
        return;
      if (LogFilter.logDebug)
        Debug.Log((object) "NetworkLobbyPlayer RemovePlayer");
      ClientScene.RemovePlayer(this.GetComponent<NetworkIdentity>().playerControllerId);
    }

    /// <summary>
    ///   <para>This is a hook that is invoked on all player objects when entering the lobby.</para>
    /// </summary>
    public virtual void OnClientEnterLobby()
    {
    }

    /// <summary>
    ///   <para>This is a hook that is invoked on all player objects when exiting the lobby.</para>
    /// </summary>
    public virtual void OnClientExitLobby()
    {
    }

    public virtual void OnClientReady(bool readyState)
    {
    }

    public override bool OnSerialize(NetworkWriter writer, bool initialState)
    {
      writer.WritePackedUInt32(1U);
      writer.Write(this.m_Slot);
      writer.Write(this.m_ReadyToBegin);
      return true;
    }

    public override void OnDeserialize(NetworkReader reader, bool initialState)
    {
      if ((int) reader.ReadPackedUInt32() == 0)
        return;
      this.m_Slot = reader.ReadByte();
      this.m_ReadyToBegin = reader.ReadBoolean();
    }

    private void OnGUI()
    {
      if (!this.ShowLobbyGUI)
        return;
      NetworkLobbyManager singleton = NetworkManager.singleton as NetworkLobbyManager;
      if ((bool) ((Object) singleton) && (!singleton.showLobbyGUI || SceneManager.GetSceneAt(0).name != singleton.lobbyScene))
        return;
      Rect position = new Rect((float) (100 + (int) this.m_Slot * 100), 200f, 90f, 20f);
      if (this.isLocalPlayer)
      {
        string text = !this.m_ReadyToBegin ? "(Not Ready)" : "(Ready)";
        GUI.Label(position, text);
        if (this.m_ReadyToBegin)
        {
          position.y += 25f;
          if (!GUI.Button(position, "STOP"))
            return;
          this.SendNotReadyToBeginMessage();
        }
        else
        {
          position.y += 25f;
          if (GUI.Button(position, "START"))
            this.SendReadyToBeginMessage();
          position.y += 25f;
          if (!GUI.Button(position, "Remove"))
            return;
          ClientScene.RemovePlayer(this.GetComponent<NetworkIdentity>().playerControllerId);
        }
      }
      else
      {
        GUI.Label(position, "Player [" + (object) this.netId + "]");
        position.y += 25f;
        GUI.Label(position, "Ready [" + (object) this.m_ReadyToBegin + "]");
      }
    }
  }
}
