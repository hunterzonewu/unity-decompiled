// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.Networking.DownloadHandlerBuffer
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine.Experimental.Networking
{
  /// <summary>
  ///   <para>A general-purpose DownloadHandler implementation which stores received data in a native byte buffer.</para>
  /// </summary>
  [StructLayout(LayoutKind.Sequential)]
  public sealed class DownloadHandlerBuffer : DownloadHandler
  {
    /// <summary>
    ///   <para>Default constructor.</para>
    /// </summary>
    public DownloadHandlerBuffer()
    {
      this.InternalCreateString();
    }

    /// <summary>
    ///   <para>Returns a copy of the contents of the native-memory data buffer as a byte array.</para>
    /// </summary>
    /// <returns>
    ///   <para>A copy of the data which has been downloaded.</para>
    /// </returns>
    protected override byte[] GetData()
    {
      return this.InternalGetData();
    }

    /// <summary>
    ///   <para>Returns a copy of the native-memory buffer interpreted as a UTF8 string.</para>
    /// </summary>
    /// <returns>
    ///   <para>A string representing the data in the native-memory buffer.</para>
    /// </returns>
    protected override string GetText()
    {
      return this.InternalGetText();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private byte[] InternalGetData();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private string InternalGetText();

    /// <summary>
    ///   <para>Returns a copy of the native-memory buffer interpreted as a UTF8 string.</para>
    /// </summary>
    /// <param name="www">A finished UnityWebRequest object with DownloadHandlerBuffer attached.</param>
    /// <returns>
    ///   <para>The same as DownloadHandlerBuffer.text</para>
    /// </returns>
    public static string GetContent(UnityWebRequest www)
    {
      return DownloadHandler.GetCheckedDownloader<DownloadHandlerBuffer>(www).text;
    }
  }
}
