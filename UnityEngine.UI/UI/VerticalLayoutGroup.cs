// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.VerticalLayoutGroup
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>Layout child layout elements below each other.</para>
  /// </summary>
  [AddComponentMenu("Layout/Vertical Layout Group", 151)]
  public class VerticalLayoutGroup : HorizontalOrVerticalLayoutGroup
  {
    protected VerticalLayoutGroup()
    {
    }

    /// <summary>
    ///   <para>Called by the layout system.</para>
    /// </summary>
    public override void CalculateLayoutInputHorizontal()
    {
      base.CalculateLayoutInputHorizontal();
      this.CalcAlongAxis(0, true);
    }

    /// <summary>
    ///   <para>Called by the layout system.</para>
    /// </summary>
    public override void CalculateLayoutInputVertical()
    {
      this.CalcAlongAxis(1, true);
    }

    /// <summary>
    ///   <para>Called by the layout system.</para>
    /// </summary>
    public override void SetLayoutHorizontal()
    {
      this.SetChildrenAlongAxis(0, true);
    }

    /// <summary>
    ///   <para>Called by the layout system.</para>
    /// </summary>
    public override void SetLayoutVertical()
    {
      this.SetChildrenAlongAxis(1, true);
    }
  }
}
