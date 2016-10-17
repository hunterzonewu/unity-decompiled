// Decompiled with JetBrains decompiler
// Type: UnityEngine.RenderTargetSetup
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using UnityEngine.Rendering;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Fully describes setup of RenderTarget.</para>
  /// </summary>
  public struct RenderTargetSetup
  {
    /// <summary>
    ///   <para>Color Buffers to set.</para>
    /// </summary>
    public RenderBuffer[] color;
    /// <summary>
    ///   <para>Depth Buffer to set.</para>
    /// </summary>
    public RenderBuffer depth;
    /// <summary>
    ///   <para>Mip Level to render to.</para>
    /// </summary>
    public int mipLevel;
    /// <summary>
    ///   <para>Cubemap face to render to.</para>
    /// </summary>
    public CubemapFace cubemapFace;
    /// <summary>
    ///   <para>Load Actions for Color Buffers. It will override any actions set on RenderBuffers themselves.</para>
    /// </summary>
    public RenderBufferLoadAction[] colorLoad;
    /// <summary>
    ///   <para>Store Actions for Color Buffers. It will override any actions set on RenderBuffers themselves.</para>
    /// </summary>
    public RenderBufferStoreAction[] colorStore;
    /// <summary>
    ///   <para>Load Action for Depth Buffer. It will override any actions set on RenderBuffer itself.</para>
    /// </summary>
    public RenderBufferLoadAction depthLoad;
    /// <summary>
    ///   <para>Store Actions for Depth Buffer. It will override any actions set on RenderBuffer itself.</para>
    /// </summary>
    public RenderBufferStoreAction depthStore;

    public RenderTargetSetup(RenderBuffer[] color, RenderBuffer depth, int mip, CubemapFace face, RenderBufferLoadAction[] colorLoad, RenderBufferStoreAction[] colorStore, RenderBufferLoadAction depthLoad, RenderBufferStoreAction depthStore)
    {
      this.color = color;
      this.depth = depth;
      this.mipLevel = mip;
      this.cubemapFace = face;
      this.colorLoad = colorLoad;
      this.colorStore = colorStore;
      this.depthLoad = depthLoad;
      this.depthStore = depthStore;
    }

    /// <summary>
    ///   <para>Constructs RenderTargetSetup.</para>
    /// </summary>
    /// <param name="color">Color Buffer(s) to set.</param>
    /// <param name="depth">Depth Buffer to set.</param>
    /// <param name="mipLevel">Mip Level to render to.</param>
    /// <param name="face">Cubemap face to render to.</param>
    /// <param name="mip"></param>
    public RenderTargetSetup(RenderBuffer color, RenderBuffer depth)
    {
      this = new RenderTargetSetup(new RenderBuffer[1]{ color }, depth);
    }

    /// <summary>
    ///   <para>Constructs RenderTargetSetup.</para>
    /// </summary>
    /// <param name="color">Color Buffer(s) to set.</param>
    /// <param name="depth">Depth Buffer to set.</param>
    /// <param name="mipLevel">Mip Level to render to.</param>
    /// <param name="face">Cubemap face to render to.</param>
    /// <param name="mip"></param>
    public RenderTargetSetup(RenderBuffer color, RenderBuffer depth, int mipLevel)
    {
      this = new RenderTargetSetup(new RenderBuffer[1]{ color }, depth, mipLevel);
    }

    /// <summary>
    ///   <para>Constructs RenderTargetSetup.</para>
    /// </summary>
    /// <param name="color">Color Buffer(s) to set.</param>
    /// <param name="depth">Depth Buffer to set.</param>
    /// <param name="mipLevel">Mip Level to render to.</param>
    /// <param name="face">Cubemap face to render to.</param>
    /// <param name="mip"></param>
    public RenderTargetSetup(RenderBuffer color, RenderBuffer depth, int mipLevel, CubemapFace face)
    {
      this = new RenderTargetSetup(new RenderBuffer[1]{ color }, depth, mipLevel, face);
    }

    /// <summary>
    ///   <para>Constructs RenderTargetSetup.</para>
    /// </summary>
    /// <param name="color">Color Buffer(s) to set.</param>
    /// <param name="depth">Depth Buffer to set.</param>
    /// <param name="mipLevel">Mip Level to render to.</param>
    /// <param name="face">Cubemap face to render to.</param>
    /// <param name="mip"></param>
    public RenderTargetSetup(RenderBuffer[] color, RenderBuffer depth)
    {
      this = new RenderTargetSetup(color, depth, 0, CubemapFace.Unknown);
    }

    /// <summary>
    ///   <para>Constructs RenderTargetSetup.</para>
    /// </summary>
    /// <param name="color">Color Buffer(s) to set.</param>
    /// <param name="depth">Depth Buffer to set.</param>
    /// <param name="mipLevel">Mip Level to render to.</param>
    /// <param name="face">Cubemap face to render to.</param>
    /// <param name="mip"></param>
    public RenderTargetSetup(RenderBuffer[] color, RenderBuffer depth, int mipLevel)
    {
      this = new RenderTargetSetup(color, depth, mipLevel, CubemapFace.Unknown);
    }

    /// <summary>
    ///   <para>Constructs RenderTargetSetup.</para>
    /// </summary>
    /// <param name="color">Color Buffer(s) to set.</param>
    /// <param name="depth">Depth Buffer to set.</param>
    /// <param name="mipLevel">Mip Level to render to.</param>
    /// <param name="face">Cubemap face to render to.</param>
    /// <param name="mip"></param>
    public RenderTargetSetup(RenderBuffer[] color, RenderBuffer depth, int mip, CubemapFace face)
    {
      this = new RenderTargetSetup(color, depth, mip, face, RenderTargetSetup.LoadActions(color), RenderTargetSetup.StoreActions(color), depth.loadAction, depth.storeAction);
    }

    internal static RenderBufferLoadAction[] LoadActions(RenderBuffer[] buf)
    {
      RenderBufferLoadAction[] bufferLoadActionArray = new RenderBufferLoadAction[buf.Length];
      for (int index = 0; index < buf.Length; ++index)
      {
        bufferLoadActionArray[index] = buf[index].loadAction;
        buf[index].loadAction = RenderBufferLoadAction.Load;
      }
      return bufferLoadActionArray;
    }

    internal static RenderBufferStoreAction[] StoreActions(RenderBuffer[] buf)
    {
      RenderBufferStoreAction[] bufferStoreActionArray = new RenderBufferStoreAction[buf.Length];
      for (int index = 0; index < buf.Length; ++index)
      {
        bufferStoreActionArray[index] = buf[index].storeAction;
        buf[index].storeAction = RenderBufferStoreAction.Store;
      }
      return bufferStoreActionArray;
    }
  }
}
