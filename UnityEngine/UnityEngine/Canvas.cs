// Decompiled with JetBrains decompiler
// Type: UnityEngine.Canvas
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Element that can be used for screen rendering.</para>
  /// </summary>
  public sealed class Canvas : Behaviour
  {
    /// <summary>
    ///   <para>Is the Canvas in World or Overlay mode?</para>
    /// </summary>
    public RenderMode renderMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is this the root Canvas?</para>
    /// </summary>
    public bool isRootCanvas { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Camera used for sizing the Canvas when in Screen Space - Camera. Also used as the Camera that events will be sent through for a World Space [[Canvas].</para>
    /// </summary>
    public Camera worldCamera { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Get the render rect for the Canvas.</para>
    /// </summary>
    public Rect pixelRect
    {
      get
      {
        Rect rect;
        this.INTERNAL_get_pixelRect(out rect);
        return rect;
      }
    }

    /// <summary>
    ///   <para>Used to scale the entire canvas, while still making it fit the screen. Only applies with renderMode is Screen Space.</para>
    /// </summary>
    public float scaleFactor { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The number of pixels per unit that is considered the default.</para>
    /// </summary>
    public float referencePixelsPerUnit { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Allows for nested canvases to override pixelPerfect settings inherited from parent canvases.</para>
    /// </summary>
    public bool overridePixelPerfect { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Force elements in the canvas to be aligned with pixels. Only applies with renderMode is Screen Space.</para>
    /// </summary>
    public bool pixelPerfect { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>How far away from the camera is the Canvas generated.</para>
    /// </summary>
    public float planeDistance { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The render order in which the canvas is being emitted to the scene.</para>
    /// </summary>
    public int renderOrder { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Override the sorting of canvas.</para>
    /// </summary>
    public bool overrideSorting { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Canvas' order within a sorting layer.</para>
    /// </summary>
    public int sortingOrder { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>For Overlay mode, display index on which the UI canvas will appear.</para>
    /// </summary>
    public int targetDisplay { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The normalized grid size that the canvas will split the renderable area into.</para>
    /// </summary>
    public int sortingGridNormalizedSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Unique ID of the Canvas' sorting layer.</para>
    /// </summary>
    public int sortingLayerID { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Cached calculated value based upon SortingLayerID.</para>
    /// </summary>
    public int cachedSortingLayerValue { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Name of the Canvas' sorting layer.</para>
    /// </summary>
    public string sortingLayerName { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Returns the Canvas closest to root, by checking through each parent and returning the last canvas found. If no other canvas is found then the canvas will return itself.</para>
    /// </summary>
    public Canvas rootCanvas { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static event Canvas.WillRenderCanvases willRenderCanvases;

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_pixelRect(out Rect value);

    /// <summary>
    ///   <para>Returns the default material that can be used for rendering normal elements on the Canvas.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Material GetDefaultCanvasMaterial();

    /// <summary>
    ///   <para>Returns the default material that can be used for rendering text elements on the Canvas.</para>
    /// </summary>
    [Obsolete("Shared default material now used for text and general UI elements, call Canvas.GetDefaultCanvasMaterial()")]
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Material GetDefaultCanvasTextMaterial();

    [RequiredByNativeCode]
    private static void SendWillRenderCanvases()
    {
      if (Canvas.willRenderCanvases == null)
        return;
      Canvas.willRenderCanvases();
    }

    /// <summary>
    ///   <para>Force all canvases to update their content.</para>
    /// </summary>
    public static void ForceUpdateCanvases()
    {
      Canvas.SendWillRenderCanvases();
    }

    public delegate void WillRenderCanvases();
  }
}
