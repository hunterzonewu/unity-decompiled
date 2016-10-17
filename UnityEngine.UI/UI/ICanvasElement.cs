// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.ICanvasElement
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

namespace UnityEngine.UI
{
  public interface ICanvasElement
  {
    /// <summary>
    ///   <para>Get the transform associated with the ICanvasElement.</para>
    /// </summary>
    Transform transform { get; }

    /// <summary>
    ///   <para>Rebuild the element for the given stage.</para>
    /// </summary>
    /// <param name="executing">Stage being rebuild.</param>
    void Rebuild(CanvasUpdate executing);

    /// <summary>
    ///   <para>Callback sent when this ICanvasElement has completed layout.</para>
    /// </summary>
    void LayoutComplete();

    /// <summary>
    ///   <para>Callback sent when this ICanvasElement has completed Graphic rebuild.</para>
    /// </summary>
    void GraphicUpdateComplete();

    /// <summary>
    ///         <para>Return true if the element is considered destroyed.
    /// Used if the native representation has been destroyed.</para>
    ///       </summary>
    bool IsDestroyed();
  }
}
