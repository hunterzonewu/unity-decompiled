// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.HorizontalLayoutGroup
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>Layout child layout elements side by side.</para>
  /// </summary>
  [AddComponentMenu("Layout/Horizontal Layout Group", 150)]
  public class HorizontalLayoutGroup : HorizontalOrVerticalLayoutGroup
  {
    protected HorizontalLayoutGroup()
    {
    }

    /// <summary>
    ///   <para>Called by the layout system.</para>
    /// </summary>
    public override void CalculateLayoutInputHorizontal()
    {
      base.CalculateLayoutInputHorizontal();
      this.CalcAlongAxis(0, false);
    }

    /// <summary>
    ///   <para>Called by the layout system.</para>
    /// </summary>
    public override void CalculateLayoutInputVertical()
    {
      this.CalcAlongAxis(1, false);
    }

    /// <summary>
    ///   <para>Called by the layout system.</para>
    /// </summary>
    public override void SetLayoutHorizontal()
    {
      this.SetChildrenAlongAxis(0, false);
    }

    /// <summary>
    ///   <para>Called by the layout system.</para>
    /// </summary>
    public override void SetLayoutVertical()
    {
      this.SetChildrenAlongAxis(1, false);
    }
  }
}
