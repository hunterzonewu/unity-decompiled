// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.Networking.UploadHandlerRaw
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine.Experimental.Networking
{
  /// <summary>
  ///   <para>A general-purpose UploadHandler subclass, using a native-code memory buffer.</para>
  /// </summary>
  [StructLayout(LayoutKind.Sequential)]
  public sealed class UploadHandlerRaw : UploadHandler
  {
    /// <summary>
    ///   <para>General constructor. Contents of the input argument are copied into a native buffer.</para>
    /// </summary>
    /// <param name="data">Raw data to transmit to the remote server.</param>
    public UploadHandlerRaw(byte[] data)
    {
      this.InternalCreateRaw(data);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private string InternalGetContentType();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void InternalSetContentType(string newContentType);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private byte[] InternalGetData();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private float InternalGetProgress();

    internal override string GetContentType()
    {
      return this.InternalGetContentType();
    }

    internal override void SetContentType(string newContentType)
    {
      this.InternalSetContentType(newContentType);
    }

    internal override byte[] GetData()
    {
      return this.InternalGetData();
    }

    internal override float GetProgress()
    {
      return this.InternalGetProgress();
    }
  }
}
