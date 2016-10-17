// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.Networking.UploadHandler
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine.Experimental.Networking
{
  /// <summary>
  ///   <para>Helper object for UnityWebRequests. Manages the buffering and transmission of body data during HTTP requests.</para>
  /// </summary>
  [StructLayout(LayoutKind.Sequential)]
  public class UploadHandler : IDisposable
  {
    [NonSerialized]
    internal IntPtr m_Ptr;

    /// <summary>
    ///   <para>The raw data which will be transmitted to the remote server as body data. (Read Only)</para>
    /// </summary>
    public byte[] data
    {
      get
      {
        return this.GetData();
      }
    }

    /// <summary>
    ///   <para>Determines the default Content-Type header which will be transmitted with the outbound HTTP request.</para>
    /// </summary>
    public string contentType
    {
      get
      {
        return this.GetContentType();
      }
      set
      {
        this.SetContentType(value);
      }
    }

    /// <summary>
    ///   <para>Returns the proportion of data uploaded to the remote server compared to the total amount of data to upload. (Read Only)</para>
    /// </summary>
    public float progress
    {
      get
      {
        return this.GetProgress();
      }
    }

    internal UploadHandler()
    {
    }

    ~UploadHandler()
    {
      this.InternalDestroy();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void InternalCreateRaw(byte[] data);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void InternalDestroy();

    /// <summary>
    ///   <para>Signals that this [UploadHandler] is no longer being used, and should clean up any resources it is using.</para>
    /// </summary>
    public void Dispose()
    {
      this.InternalDestroy();
      GC.SuppressFinalize((object) this);
    }

    internal virtual byte[] GetData()
    {
      return (byte[]) null;
    }

    internal virtual string GetContentType()
    {
      return "text/plain";
    }

    internal virtual void SetContentType(string newContentType)
    {
    }

    internal virtual float GetProgress()
    {
      return 0.5f;
    }
  }
}
