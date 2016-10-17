// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.IClippable
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

namespace UnityEngine.UI
{
  public interface IClippable
  {
    /// <summary>
    ///   <para>RectTransform of the clippable.</para>
    /// </summary>
    RectTransform rectTransform { get; }

    /// <summary>
    ///   <para>Called when the state of a parent IClippable changes.</para>
    /// </summary>
    void RecalculateClipping();

    /// <summary>
    ///   <para>Clip and cull the IClippable given the clipRect.</para>
    /// </summary>
    /// <param name="clipRect">Rectangle to clip against.</param>
    /// <param name="validRect">Is the Rect valid. If not then the rect has 0 size.</param>
    void Cull(Rect clipRect, bool validRect);

    /// <summary>
    ///   <para>Set the clip rect for the IClippable.</para>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="validRect"></param>
    void SetClipRect(Rect value, bool validRect);
  }
}
