// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.MsgType
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>Container class for networking system built-in message types.</para>
  /// </summary>
  public class MsgType
  {
    internal static string[] msgLabels = new string[48]{ "none", "ObjectDestroy", "Rpc", "ObjectSpawn", "Owner", "Command", "LocalPlayerTransform", "SyncEvent", "UpdateVars", "SyncList", "ObjectSpawnScene", "NetworkInfo", "SpawnFinished", "ObjectHide", "CRC", "LocalClientAuthority", "LocalChildTransform", "PeerClientAuthority", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "Connect", "Disconnect", "Error", "Ready", "NotReady", "AddPlayer", "RemovePlayer", "Scene", "Animation", "AnimationParams", "AnimationTrigger", "LobbyReadyToBegin", "LobbySceneLoaded", "LobbyAddPlayerFailed", "LobbyReturnToLobby", "ReconnectPlayer" };
    /// <summary>
    ///   <para>Internal networking system message for destroying objects.</para>
    /// </summary>
    public const short ObjectDestroy = 1;
    /// <summary>
    ///   <para>Internal networking system message for sending a ClientRPC from server to client.</para>
    /// </summary>
    public const short Rpc = 2;
    /// <summary>
    ///   <para>Internal networking system message for spawning objects.</para>
    /// </summary>
    public const short ObjectSpawn = 3;
    /// <summary>
    ///   <para>Internal networking system message for telling clients they own a player object.</para>
    /// </summary>
    public const short Owner = 4;
    /// <summary>
    ///   <para>Internal networking system message for sending a command from client to server.</para>
    /// </summary>
    public const short Command = 5;
    /// <summary>
    ///   <para>Internal networking system message for sending tranforms from client to server.</para>
    /// </summary>
    public const short LocalPlayerTransform = 6;
    /// <summary>
    ///   <para>Internal networking system message for sending a SyncEvent from server to client.</para>
    /// </summary>
    public const short SyncEvent = 7;
    /// <summary>
    ///   <para>Internal networking system message for updating SyncVars on a client from a server.</para>
    /// </summary>
    public const short UpdateVars = 8;
    /// <summary>
    ///   <para>Internal networking system message for sending a USyncList generic list.</para>
    /// </summary>
    public const short SyncList = 9;
    /// <summary>
    ///   <para>Internal networking system message for spawning scene objects.</para>
    /// </summary>
    public const short ObjectSpawnScene = 10;
    /// <summary>
    ///   <para>Internal networking system message for sending information about network peers to clients.</para>
    /// </summary>
    public const short NetworkInfo = 11;
    /// <summary>
    ///   <para>Internal networking system messages used to tell when the initial contents of a scene is being spawned.</para>
    /// </summary>
    public const short SpawnFinished = 12;
    /// <summary>
    ///   <para>Internal networking system message for hiding objects.</para>
    /// </summary>
    public const short ObjectHide = 13;
    /// <summary>
    ///   <para>Internal networking system message for HLAPI CRC checking.</para>
    /// </summary>
    public const short CRC = 14;
    /// <summary>
    ///   <para>Internal networking system message for setting authority to a client for an object.</para>
    /// </summary>
    public const short LocalClientAuthority = 15;
    /// <summary>
    ///   <para>Internal networking system message for sending tranforms for client object from client to server.</para>
    /// </summary>
    public const short LocalChildTransform = 16;
    /// <summary>
    ///   <para>Internal networking system message for sending information about changes in authority for non-player objects to clients.</para>
    /// </summary>
    public const short PeerClientAuthority = 17;
    internal const short UserMessage = 0;
    internal const short HLAPIMsg = 28;
    internal const short LLAPIMsg = 29;
    internal const short HLAPIResend = 30;
    internal const short HLAPIPending = 31;
    /// <summary>
    ///   <para>The highest value of internal networking system message ids. User messages must be above this value. User code cannot replace these handlers.</para>
    /// </summary>
    public const short InternalHighest = 31;
    /// <summary>
    ///   <para>Internal networking system message for communicating a connection has occurred.</para>
    /// </summary>
    public const short Connect = 32;
    /// <summary>
    ///   <para>Internal networking system message for communicating a disconnect has occurred,.</para>
    /// </summary>
    public const short Disconnect = 33;
    /// <summary>
    ///   <para>Internal networking system message for communicating an error.</para>
    /// </summary>
    public const short Error = 34;
    /// <summary>
    ///   <para>Internal networking system message for clients to tell server they are ready.</para>
    /// </summary>
    public const short Ready = 35;
    /// <summary>
    ///   <para>Internal networking system message for server to tell clients they are no longer ready.</para>
    /// </summary>
    public const short NotReady = 36;
    /// <summary>
    ///   <para>Internal networking system message for adding player objects to client instances.</para>
    /// </summary>
    public const short AddPlayer = 37;
    /// <summary>
    ///   <para>Internal networking system message for removing a player object which was spawned for a client.</para>
    /// </summary>
    public const short RemovePlayer = 38;
    /// <summary>
    ///   <para>Internal networking system message that tells clients which scene to load when they connect to a server.</para>
    /// </summary>
    public const short Scene = 39;
    /// <summary>
    ///   <para>Internal networking system message for sending synchronizing animation state.</para>
    /// </summary>
    public const short Animation = 40;
    /// <summary>
    ///   <para>Internal networking system message for sending synchronizing animation parameter state.</para>
    /// </summary>
    public const short AnimationParameters = 41;
    /// <summary>
    ///   <para>Internal networking system message for sending animation triggers.</para>
    /// </summary>
    public const short AnimationTrigger = 42;
    /// <summary>
    ///   <para>Internal networking system message for communicating a player is ready in the lobby.</para>
    /// </summary>
    public const short LobbyReadyToBegin = 43;
    /// <summary>
    ///   <para>Internal networking system message for communicating a lobby player has loaded the game scene.</para>
    /// </summary>
    public const short LobbySceneLoaded = 44;
    /// <summary>
    ///   <para>Internal networking system message for communicating failing to add lobby player.</para>
    /// </summary>
    public const short LobbyAddPlayerFailed = 45;
    /// <summary>
    ///   <para>Internal networking system messages used to return the game to the lobby scene.</para>
    /// </summary>
    public const short LobbyReturnToLobby = 46;
    /// <summary>
    ///   <para>Internal networking system message used when a client connects to the new host of a game.</para>
    /// </summary>
    public const short ReconnectPlayer = 47;
    /// <summary>
    ///   <para>The highest value of built-in networking system message ids. User messages must be above this value.</para>
    /// </summary>
    public const short Highest = 47;

    /// <summary>
    ///   <para>Returns the name of internal message types by their id.</para>
    /// </summary>
    /// <param name="value">A internal message id value.</param>
    /// <returns>
    ///   <para>The name of the internal message.</para>
    /// </returns>
    public static string MsgTypeToString(short value)
    {
      if ((int) value < 0 || (int) value > 47)
        return string.Empty;
      string str = MsgType.msgLabels[(int) value];
      if (string.IsNullOrEmpty(str))
        str = "[" + (object) value + "]";
      return str;
    }
  }
}
