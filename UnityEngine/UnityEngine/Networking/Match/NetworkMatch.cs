// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.Match.NetworkMatch
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking.Match
{
  /// <summary>
  ///   <para>A component for communicating with the UNET Matchmaking service.</para>
  /// </summary>
  public class NetworkMatch : MonoBehaviour
  {
    private Uri m_BaseUri = new Uri("https://mm.unet.unity3d.com");
    private const string kMultiplayerNetworkingIdKey = "CloudNetworkingId";

    /// <summary>
    ///   <para>The base URI of the UNET MatchMaker that this NetworkMatch will communicate with.</para>
    /// </summary>
    public Uri baseUri
    {
      get
      {
        return this.m_BaseUri;
      }
      set
      {
        this.m_BaseUri = value;
      }
    }

    /// <summary>
    ///   <para>Set this before calling any UNET functions. Must match AppID for this program from the Cloud API.</para>
    /// </summary>
    /// <param name="programAppID">AppID that corresponds to the Cloud API AppID for this app.</param>
    public void SetProgramAppID(AppID programAppID)
    {
      Utility.SetAppID(programAppID);
    }

    public Coroutine CreateMatch(string matchName, uint matchSize, bool matchAdvertise, string matchPassword, NetworkMatch.ResponseDelegate<CreateMatchResponse> callback)
    {
      return this.CreateMatch(new CreateMatchRequest() { name = matchName, size = matchSize, advertise = matchAdvertise, password = matchPassword }, callback);
    }

    public Coroutine CreateMatch(CreateMatchRequest req, NetworkMatch.ResponseDelegate<CreateMatchResponse> callback)
    {
      Uri uri = new Uri(this.baseUri, "/json/reply/CreateMatchRequest");
      UnityEngine.Debug.Log((object) ("MatchMakingClient Create :" + (object) uri));
      WWWForm form = new WWWForm();
      form.AddField("projectId", Application.cloudProjectId);
      form.AddField("sourceId", Utility.GetSourceID().ToString());
      form.AddField("appId", Utility.GetAppID().ToString());
      form.AddField("accessTokenString", 0);
      form.AddField("domain", 0);
      form.AddField("name", req.name);
      form.AddField("size", req.size.ToString());
      form.AddField("advertise", req.advertise.ToString());
      form.AddField("password", req.password);
      form.headers["Accept"] = "application/json";
      return this.StartCoroutine(this.ProcessMatchResponse<CreateMatchResponse>(new WWW(uri.ToString(), form), callback));
    }

    public Coroutine JoinMatch(NetworkID netId, string matchPassword, NetworkMatch.ResponseDelegate<JoinMatchResponse> callback)
    {
      return this.JoinMatch(new JoinMatchRequest() { networkId = netId, password = matchPassword }, callback);
    }

    public Coroutine JoinMatch(JoinMatchRequest req, NetworkMatch.ResponseDelegate<JoinMatchResponse> callback)
    {
      Uri uri = new Uri(this.baseUri, "/json/reply/JoinMatchRequest");
      UnityEngine.Debug.Log((object) ("MatchMakingClient Join :" + (object) uri));
      WWWForm form = new WWWForm();
      form.AddField("projectId", Application.cloudProjectId);
      form.AddField("sourceId", Utility.GetSourceID().ToString());
      form.AddField("appId", Utility.GetAppID().ToString());
      form.AddField("accessTokenString", 0);
      form.AddField("domain", 0);
      form.AddField("networkId", req.networkId.ToString());
      form.AddField("password", req.password);
      form.headers["Accept"] = "application/json";
      return this.StartCoroutine(this.ProcessMatchResponse<JoinMatchResponse>(new WWW(uri.ToString(), form), callback));
    }

    public Coroutine DestroyMatch(NetworkID netId, NetworkMatch.ResponseDelegate<BasicResponse> callback)
    {
      return this.DestroyMatch(new DestroyMatchRequest() { networkId = netId }, callback);
    }

    public Coroutine DestroyMatch(DestroyMatchRequest req, NetworkMatch.ResponseDelegate<BasicResponse> callback)
    {
      Uri uri = new Uri(this.baseUri, "/json/reply/DestroyMatchRequest");
      UnityEngine.Debug.Log((object) ("MatchMakingClient Destroy :" + uri.ToString()));
      WWWForm form = new WWWForm();
      form.AddField("projectId", Application.cloudProjectId);
      form.AddField("sourceId", Utility.GetSourceID().ToString());
      form.AddField("appId", Utility.GetAppID().ToString());
      form.AddField("accessTokenString", Utility.GetAccessTokenForNetwork(req.networkId).GetByteString());
      form.AddField("domain", 0);
      form.AddField("networkId", req.networkId.ToString());
      form.headers["Accept"] = "application/json";
      return this.StartCoroutine(this.ProcessMatchResponse<BasicResponse>(new WWW(uri.ToString(), form), callback));
    }

    public Coroutine DropConnection(NetworkID netId, NodeID dropNodeId, NetworkMatch.ResponseDelegate<BasicResponse> callback)
    {
      return this.DropConnection(new DropConnectionRequest() { networkId = netId, nodeId = dropNodeId }, callback);
    }

    public Coroutine DropConnection(DropConnectionRequest req, NetworkMatch.ResponseDelegate<BasicResponse> callback)
    {
      Uri uri = new Uri(this.baseUri, "/json/reply/DropConnectionRequest");
      UnityEngine.Debug.Log((object) ("MatchMakingClient DropConnection :" + (object) uri));
      WWWForm form = new WWWForm();
      form.AddField("projectId", Application.cloudProjectId);
      form.AddField("sourceId", Utility.GetSourceID().ToString());
      form.AddField("appId", Utility.GetAppID().ToString());
      form.AddField("accessTokenString", Utility.GetAccessTokenForNetwork(req.networkId).GetByteString());
      form.AddField("domain", 0);
      form.AddField("networkId", req.networkId.ToString());
      form.AddField("nodeId", req.nodeId.ToString());
      form.headers["Accept"] = "application/json";
      return this.StartCoroutine(this.ProcessMatchResponse<BasicResponse>(new WWW(uri.ToString(), form), callback));
    }

    public Coroutine ListMatches(int startPageNumber, int resultPageSize, string matchNameFilter, NetworkMatch.ResponseDelegate<ListMatchResponse> callback)
    {
      return this.ListMatches(new ListMatchRequest() { pageNum = startPageNumber, pageSize = resultPageSize, nameFilter = matchNameFilter }, callback);
    }

    public Coroutine ListMatches(ListMatchRequest req, NetworkMatch.ResponseDelegate<ListMatchResponse> callback)
    {
      Uri uri = new Uri(this.baseUri, "/json/reply/ListMatchRequest");
      UnityEngine.Debug.Log((object) ("MatchMakingClient ListMatches :" + (object) uri));
      WWWForm form = new WWWForm();
      form.AddField("projectId", Application.cloudProjectId);
      form.AddField("sourceId", Utility.GetSourceID().ToString());
      form.AddField("appId", Utility.GetAppID().ToString());
      form.AddField("includePasswordMatches", req.includePasswordMatches.ToString());
      form.AddField("accessTokenString", 0);
      form.AddField("domain", 0);
      form.AddField("pageSize", req.pageSize);
      form.AddField("pageNum", req.pageNum);
      form.AddField("nameFilter", req.nameFilter);
      form.headers["Accept"] = "application/json";
      return this.StartCoroutine(this.ProcessMatchResponse<ListMatchResponse>(new WWW(uri.ToString(), form), callback));
    }

    [DebuggerHidden]
    private IEnumerator ProcessMatchResponse<JSONRESPONSE>(WWW client, NetworkMatch.ResponseDelegate<JSONRESPONSE> callback) where JSONRESPONSE : Response, new()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new NetworkMatch.\u003CProcessMatchResponse\u003Ec__Iterator0<JSONRESPONSE>() { client = client, callback = callback, \u003C\u0024\u003Eclient = client, \u003C\u0024\u003Ecallback = callback };
    }

    public delegate void ResponseDelegate<T>(T response);
  }
}
