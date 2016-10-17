// Decompiled with JetBrains decompiler
// Type: UnityEditor.AsyncHTTPClient
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.Internal;

namespace UnityEditor
{
  internal sealed class AsyncHTTPClient
  {
    private IntPtr m_Handle;
    public AsyncHTTPClient.StatusCallback statusCallback;
    public AsyncHTTPClient.DoneCallback doneCallback;
    private string m_ToUrl;
    private string m_FromData;
    private string m_Method;
    public Dictionary<string, string> header;

    public string url
    {
      get
      {
        return this.m_ToUrl;
      }
    }

    public string text
    {
      get
      {
        UTF8Encoding utF8Encoding = new UTF8Encoding();
        byte[] bytes = this.bytes;
        if (bytes == null)
          return (string) null;
        return utF8Encoding.GetString(bytes);
      }
    }

    public byte[] bytes
    {
      get
      {
        return AsyncHTTPClient.GetBytesByHandle(this.m_Handle);
      }
    }

    public Texture2D texture
    {
      get
      {
        return AsyncHTTPClient.GetTextureByHandle(this.m_Handle);
      }
    }

    public AsyncHTTPClient.State state { get; private set; }

    public int responseCode { get; private set; }

    public string tag { get; set; }

    public string postData
    {
      set
      {
        this.m_FromData = value;
        if (this.m_Method == string.Empty)
          this.m_Method = "POST";
        if (this.header.ContainsKey("Content-Type"))
          return;
        this.header["Content-Type"] = "application/x-www-form-urlencoded";
      }
    }

    public Dictionary<string, string> postDictionary
    {
      set
      {
        this.postData = string.Join("&", value.Select<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (kv => this.EscapeLong(kv.Key) + "=" + this.EscapeLong(kv.Value))).ToArray<string>());
      }
    }

    public AsyncHTTPClient(string _toUrl)
    {
      this.m_ToUrl = _toUrl;
      this.m_FromData = (string) null;
      this.m_Method = string.Empty;
      this.state = AsyncHTTPClient.State.INIT;
      this.header = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      this.m_Handle = (IntPtr) 0;
      this.tag = string.Empty;
      this.statusCallback = (AsyncHTTPClient.StatusCallback) null;
    }

    public AsyncHTTPClient(string _toUrl, string _method)
    {
      this.m_ToUrl = _toUrl;
      this.m_FromData = (string) null;
      this.m_Method = _method;
      this.state = AsyncHTTPClient.State.INIT;
      this.header = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      this.m_Handle = (IntPtr) 0;
      this.tag = string.Empty;
      this.statusCallback = (AsyncHTTPClient.StatusCallback) null;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern IntPtr SubmitClientRequest(string tag, string url, string[] headers, string method, string data, AsyncHTTPClient.RequestDoneCallback doneDelegate, [DefaultValue("null")] AsyncHTTPClient.RequestProgressCallback progressDelegate);

    [ExcludeFromDocs]
    private static IntPtr SubmitClientRequest(string tag, string url, string[] headers, string method, string data, AsyncHTTPClient.RequestDoneCallback doneDelegate)
    {
      AsyncHTTPClient.RequestProgressCallback progressDelegate = (AsyncHTTPClient.RequestProgressCallback) null;
      return AsyncHTTPClient.SubmitClientRequest(tag, url, headers, method, data, doneDelegate, progressDelegate);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern byte[] GetBytesByHandle(IntPtr handle);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Texture2D GetTextureByHandle(IntPtr handle);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void AbortByTag(string tag);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void AbortByHandle(IntPtr handle);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void CurlRequestCheck();

    public void Abort()
    {
      this.state = AsyncHTTPClient.State.ABORTED;
      AsyncHTTPClient.AbortByHandle(this.m_Handle);
    }

    public bool IsAborted()
    {
      return this.state == AsyncHTTPClient.State.ABORTED;
    }

    public bool IsDone()
    {
      return AsyncHTTPClient.IsDone(this.state);
    }

    public static bool IsDone(AsyncHTTPClient.State state)
    {
      switch (state)
      {
        case AsyncHTTPClient.State.DONE_OK:
        case AsyncHTTPClient.State.DONE_FAILED:
        case AsyncHTTPClient.State.ABORTED:
        case AsyncHTTPClient.State.TIMEOUT:
          return true;
        default:
          return false;
      }
    }

    public bool IsSuccess()
    {
      return this.state == AsyncHTTPClient.State.DONE_OK;
    }

    public static bool IsSuccess(AsyncHTTPClient.State state)
    {
      return state == AsyncHTTPClient.State.DONE_OK;
    }

    public void Begin()
    {
      if (this.IsAborted())
      {
        this.state = AsyncHTTPClient.State.ABORTED;
      }
      else
      {
        if (this.m_Method == string.Empty)
          this.m_Method = "GET";
        this.m_Handle = AsyncHTTPClient.SubmitClientRequest(this.tag, this.m_ToUrl, this.header.Select<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (kv => string.Format("{0}: {1}", (object) kv.Key, (object) kv.Value))).ToArray<string>(), this.m_Method, this.m_FromData, new AsyncHTTPClient.RequestDoneCallback(this.Done), new AsyncHTTPClient.RequestProgressCallback(this.Progress));
      }
    }

    private void Done(AsyncHTTPClient.State status, int i_ResponseCode)
    {
      this.state = status;
      this.responseCode = i_ResponseCode;
      if (this.doneCallback != null)
        this.doneCallback(this);
      this.m_Handle = (IntPtr) 0;
    }

    private void Progress(AsyncHTTPClient.State status, int bytesDone, int bytesTotal)
    {
      this.state = status;
      if (this.statusCallback == null)
        return;
      this.statusCallback(status, bytesDone, bytesTotal);
    }

    private string EscapeLong(string v)
    {
      StringBuilder stringBuilder = new StringBuilder();
      int startIndex = 0;
      while (startIndex < v.Length)
      {
        stringBuilder.Append(Uri.EscapeDataString(v.Substring(startIndex, v.Length - startIndex <= 32766 ? v.Length - startIndex : 32766)));
        startIndex += 32766;
      }
      return stringBuilder.ToString();
    }

    internal enum State
    {
      INIT,
      CONNECTING,
      CONNECTED,
      UPLOADING,
      DOWNLOADING,
      CONFIRMING,
      DONE_OK,
      DONE_FAILED,
      ABORTED,
      TIMEOUT,
    }

    private delegate void RequestProgressCallback(AsyncHTTPClient.State status, int downloaded, int totalSize);

    private delegate void RequestDoneCallback(AsyncHTTPClient.State status, int httpStatus);

    public delegate void DoneCallback(AsyncHTTPClient client);

    public delegate void StatusCallback(AsyncHTTPClient.State status, int bytesDone, int bytesTotal);
  }
}
