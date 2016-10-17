// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.Utility
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking
{
  public sealed class Utility
  {
    private static System.Random s_randomGenerator = new System.Random(Environment.TickCount);
    private static bool s_useRandomSourceID = false;
    private static int s_randomSourceComponent = 0;
    private static AppID s_programAppID = AppID.Invalid;
    private static Dictionary<NetworkID, NetworkAccessToken> s_dictTokens = new Dictionary<NetworkID, NetworkAccessToken>();

    public static bool useRandomSourceID
    {
      get
      {
        return Utility.s_useRandomSourceID;
      }
      set
      {
        Utility.SetUseRandomSourceID(value);
      }
    }

    private Utility()
    {
    }

    public static SourceID GetSourceID()
    {
      return (SourceID) (SystemInfo.deviceUniqueIdentifier + (object) Utility.s_randomSourceComponent).GetHashCode();
    }

    private static void SetUseRandomSourceID(bool useRandomSourceID)
    {
      if (useRandomSourceID && !Utility.s_useRandomSourceID)
        Utility.s_randomSourceComponent = Utility.s_randomGenerator.Next(int.MaxValue);
      else if (!useRandomSourceID && Utility.s_useRandomSourceID)
        Utility.s_randomSourceComponent = 0;
      Utility.s_useRandomSourceID = useRandomSourceID;
    }

    public static void SetAppID(AppID newAppID)
    {
      Utility.s_programAppID = newAppID;
    }

    public static AppID GetAppID()
    {
      return Utility.s_programAppID;
    }

    public static void SetAccessTokenForNetwork(NetworkID netId, NetworkAccessToken accessToken)
    {
      Utility.s_dictTokens.Add(netId, accessToken);
    }

    public static NetworkAccessToken GetAccessTokenForNetwork(NetworkID netId)
    {
      NetworkAccessToken networkAccessToken;
      if (!Utility.s_dictTokens.TryGetValue(netId, out networkAccessToken))
        networkAccessToken = new NetworkAccessToken();
      return networkAccessToken;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void FetchNetworkingId(string projectId);
  }
}
