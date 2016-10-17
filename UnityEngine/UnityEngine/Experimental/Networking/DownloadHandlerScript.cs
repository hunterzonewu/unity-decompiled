// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.Networking.DownloadHandlerScript
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine.Experimental.Networking
{
  /// <summary>
  ///   <para>An abstract base class for user-created scripting-driven DownloadHandler implementations.</para>
  /// </summary>
  [StructLayout(LayoutKind.Sequential)]
  public class DownloadHandlerScript : DownloadHandler
  {
    /// <summary>
    ///   <para>Create a DownloadHandlerScript which allocates new buffers when passing data to callbacks.</para>
    /// </summary>
    public DownloadHandlerScript()
    {
      this.InternalCreateScript();
    }

    /// <summary>
    ///   <para>Create a DownloadHandlerScript which reuses a preallocated buffer to pass data to callbacks.</para>
    /// </summary>
    /// <param name="preallocatedBuffer">A byte buffer into which data will be copied, for use by DownloadHandler.ReceiveData.</param>
    public DownloadHandlerScript(byte[] preallocatedBuffer)
    {
      if (preallocatedBuffer == null || preallocatedBuffer.Length < 1)
        throw new ArgumentException("Cannot create a preallocated-buffer DownloadHandlerScript backed by a null or zero-length array");
      this.InternalCreateScript();
      this.InternalSetPreallocatedBuffer(preallocatedBuffer);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void InternalSetPreallocatedBuffer(byte[] buffer);
  }
}
