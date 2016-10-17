// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.Networking.DownloadHandlerTexture
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine.Experimental.Networking
{
  /// <summary>
  ///   <para>A DownloadHandler subclass specialized for downloading images for use as Texture objects.</para>
  /// </summary>
  [StructLayout(LayoutKind.Sequential)]
  public sealed class DownloadHandlerTexture : DownloadHandler
  {
    /// <summary>
    ///   <para>Returns the downloaded Texture, or null. (Read Only)</para>
    /// </summary>
    public Texture2D texture
    {
      get
      {
        return this.InternalGetTexture();
      }
    }

    /// <summary>
    ///   <para>Default constructor.</para>
    /// </summary>
    public DownloadHandlerTexture()
    {
      this.InternalCreateTexture(true);
    }

    /// <summary>
    ///   <para>Constructor, allows TextureImporter.isReadable property to be set.</para>
    /// </summary>
    /// <param name="readable">Value to set for TextureImporter.isReadable.</param>
    public DownloadHandlerTexture(bool readable)
    {
      this.InternalCreateTexture(readable);
    }

    /// <summary>
    ///   <para>Called by DownloadHandler.data. Returns a copy of the downloaded image data as raw bytes.</para>
    /// </summary>
    /// <returns>
    ///   <para>A copy of the downloaded data.</para>
    /// </returns>
    protected override byte[] GetData()
    {
      return this.InternalGetData();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private Texture2D InternalGetTexture();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private byte[] InternalGetData();

    /// <summary>
    ///   <para>Returns the downloaded Texture, or null.</para>
    /// </summary>
    /// <param name="www">A finished UnityWebRequest object with DownloadHandlerTexture attached.</param>
    /// <returns>
    ///   <para>The same as DownloadHandlerTexture.texture</para>
    /// </returns>
    public static Texture2D GetContent(UnityWebRequest www)
    {
      return DownloadHandler.GetCheckedDownloader<DownloadHandlerTexture>(www).texture;
    }
  }
}
