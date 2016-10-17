// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.HorizontalOrVerticalLayoutGroup
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>Abstract base class for HorizontalLayoutGroup and VerticalLayoutGroup.</para>
  /// </summary>
  public abstract class HorizontalOrVerticalLayoutGroup : LayoutGroup
  {
    [SerializeField]
    protected bool m_ChildForceExpandWidth = true;
    [SerializeField]
    protected bool m_ChildForceExpandHeight = true;
    [SerializeField]
    protected float m_Spacing;

    /// <summary>
    ///   <para>The spacing to use between layout elements in the layout group.</para>
    /// </summary>
    public float spacing
    {
      get
      {
        return this.m_Spacing;
      }
      set
      {
        this.SetProperty<float>(ref this.m_Spacing, value);
      }
    }

    /// <summary>
    ///   <para>Whether to force the children to expand to fill additional available horizontal space.</para>
    /// </summary>
    public bool childForceExpandWidth
    {
      get
      {
        return this.m_ChildForceExpandWidth;
      }
      set
      {
        this.SetProperty<bool>(ref this.m_ChildForceExpandWidth, value);
      }
    }

    /// <summary>
    ///   <para>Whether to force the children to expand to fill additional available vertical space.</para>
    /// </summary>
    public bool childForceExpandHeight
    {
      get
      {
        return this.m_ChildForceExpandHeight;
      }
      set
      {
        this.SetProperty<bool>(ref this.m_ChildForceExpandHeight, value);
      }
    }

    /// <summary>
    ///   <para>Calculate the layout element properties for this layout element along the given axis.</para>
    /// </summary>
    /// <param name="axis">The axis to calculate for. 0 is horizontal and 1 is vertical.</param>
    /// <param name="isVertical">Is this group a vertical group?</param>
    protected void CalcAlongAxis(int axis, bool isVertical)
    {
      float num1 = axis != 0 ? (float) this.padding.vertical : (float) this.padding.horizontal;
      float num2 = num1;
      float b = num1;
      float num3 = 0.0f;
      bool flag = isVertical ^ axis == 1;
      for (int index = 0; index < this.rectChildren.Count; ++index)
      {
        RectTransform rectChild = this.rectChildren[index];
        float minSize = LayoutUtility.GetMinSize(rectChild, axis);
        float preferredSize = LayoutUtility.GetPreferredSize(rectChild, axis);
        float a = LayoutUtility.GetFlexibleSize(rectChild, axis);
        if ((axis != 0 ? (this.childForceExpandHeight ? 1 : 0) : (this.childForceExpandWidth ? 1 : 0)) != 0)
          a = Mathf.Max(a, 1f);
        if (flag)
        {
          num2 = Mathf.Max(minSize + num1, num2);
          b = Mathf.Max(preferredSize + num1, b);
          num3 = Mathf.Max(a, num3);
        }
        else
        {
          num2 += minSize + this.spacing;
          b += preferredSize + this.spacing;
          num3 += a;
        }
      }
      if (!flag && this.rectChildren.Count > 0)
      {
        num2 -= this.spacing;
        b -= this.spacing;
      }
      float totalPreferred = Mathf.Max(num2, b);
      this.SetLayoutInputForAxis(num2, totalPreferred, num3, axis);
    }

    /// <summary>
    ///   <para>Set the positions and sizes of the child layout elements for the given axis.</para>
    /// </summary>
    /// <param name="axis">The axis to handle. 0 is horizontal and 1 is vertical.</param>
    /// <param name="isVertical">Is this group a vertical group?</param>
    protected void SetChildrenAlongAxis(int axis, bool isVertical)
    {
      float num1 = this.rectTransform.rect.size[axis];
      if (isVertical ^ axis == 1)
      {
        float num2 = num1 - (axis != 0 ? (float) this.padding.vertical : (float) this.padding.horizontal);
        for (int index = 0; index < this.rectChildren.Count; ++index)
        {
          RectTransform rectChild = this.rectChildren[index];
          float minSize = LayoutUtility.GetMinSize(rectChild, axis);
          float preferredSize = LayoutUtility.GetPreferredSize(rectChild, axis);
          float a = LayoutUtility.GetFlexibleSize(rectChild, axis);
          if ((axis != 0 ? (this.childForceExpandHeight ? 1 : 0) : (this.childForceExpandWidth ? 1 : 0)) != 0)
            a = Mathf.Max(a, 1f);
          float num3 = Mathf.Clamp(num2, minSize, (double) a <= 0.0 ? preferredSize : num1);
          float startOffset = this.GetStartOffset(axis, num3);
          this.SetChildAlongAxis(rectChild, axis, startOffset, num3);
        }
      }
      else
      {
        float pos = axis != 0 ? (float) this.padding.top : (float) this.padding.left;
        if ((double) this.GetTotalFlexibleSize(axis) == 0.0 && (double) this.GetTotalPreferredSize(axis) < (double) num1)
          pos = this.GetStartOffset(axis, this.GetTotalPreferredSize(axis) - (axis != 0 ? (float) this.padding.vertical : (float) this.padding.horizontal));
        float t = 0.0f;
        if ((double) this.GetTotalMinSize(axis) != (double) this.GetTotalPreferredSize(axis))
          t = Mathf.Clamp01((float) (((double) num1 - (double) this.GetTotalMinSize(axis)) / ((double) this.GetTotalPreferredSize(axis) - (double) this.GetTotalMinSize(axis))));
        float num2 = 0.0f;
        if ((double) num1 > (double) this.GetTotalPreferredSize(axis) && (double) this.GetTotalFlexibleSize(axis) > 0.0)
          num2 = (num1 - this.GetTotalPreferredSize(axis)) / this.GetTotalFlexibleSize(axis);
        for (int index = 0; index < this.rectChildren.Count; ++index)
        {
          RectTransform rectChild = this.rectChildren[index];
          float minSize = LayoutUtility.GetMinSize(rectChild, axis);
          float preferredSize = LayoutUtility.GetPreferredSize(rectChild, axis);
          float a = LayoutUtility.GetFlexibleSize(rectChild, axis);
          if ((axis != 0 ? (this.childForceExpandHeight ? 1 : 0) : (this.childForceExpandWidth ? 1 : 0)) != 0)
            a = Mathf.Max(a, 1f);
          float size = Mathf.Lerp(minSize, preferredSize, t) + a * num2;
          this.SetChildAlongAxis(rectChild, axis, pos, size);
          pos += size + this.spacing;
        }
      }
    }
  }
}
