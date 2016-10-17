// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.GridLayoutGroup
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>Layout child layout elements in a grid.</para>
  /// </summary>
  [AddComponentMenu("Layout/Grid Layout Group", 152)]
  public class GridLayoutGroup : LayoutGroup
  {
    [SerializeField]
    protected Vector2 m_CellSize = new Vector2(100f, 100f);
    [SerializeField]
    protected Vector2 m_Spacing = Vector2.zero;
    [SerializeField]
    protected int m_ConstraintCount = 2;
    [SerializeField]
    protected GridLayoutGroup.Corner m_StartCorner;
    [SerializeField]
    protected GridLayoutGroup.Axis m_StartAxis;
    [SerializeField]
    protected GridLayoutGroup.Constraint m_Constraint;

    /// <summary>
    ///   <para>Which corner should the first cell be placed in?</para>
    /// </summary>
    public GridLayoutGroup.Corner startCorner
    {
      get
      {
        return this.m_StartCorner;
      }
      set
      {
        this.SetProperty<GridLayoutGroup.Corner>(ref this.m_StartCorner, value);
      }
    }

    /// <summary>
    ///   <para>Which axis should cells be placed along first?</para>
    /// </summary>
    public GridLayoutGroup.Axis startAxis
    {
      get
      {
        return this.m_StartAxis;
      }
      set
      {
        this.SetProperty<GridLayoutGroup.Axis>(ref this.m_StartAxis, value);
      }
    }

    /// <summary>
    ///   <para>The size to use for each cell in the grid.</para>
    /// </summary>
    public Vector2 cellSize
    {
      get
      {
        return this.m_CellSize;
      }
      set
      {
        this.SetProperty<Vector2>(ref this.m_CellSize, value);
      }
    }

    /// <summary>
    ///   <para>The spacing to use between layout elements in the grid.</para>
    /// </summary>
    public Vector2 spacing
    {
      get
      {
        return this.m_Spacing;
      }
      set
      {
        this.SetProperty<Vector2>(ref this.m_Spacing, value);
      }
    }

    /// <summary>
    ///   <para>Which constraint to use for the GridLayoutGroup.</para>
    /// </summary>
    public GridLayoutGroup.Constraint constraint
    {
      get
      {
        return this.m_Constraint;
      }
      set
      {
        this.SetProperty<GridLayoutGroup.Constraint>(ref this.m_Constraint, value);
      }
    }

    /// <summary>
    ///   <para>How many cells there should be along the constrained axis.</para>
    /// </summary>
    public int constraintCount
    {
      get
      {
        return this.m_ConstraintCount;
      }
      set
      {
        this.SetProperty<int>(ref this.m_ConstraintCount, Mathf.Max(1, value));
      }
    }

    protected GridLayoutGroup()
    {
    }

    protected override void OnValidate()
    {
      base.OnValidate();
      this.constraintCount = this.constraintCount;
    }

    /// <summary>
    ///   <para>Called by the layout system.</para>
    /// </summary>
    public override void CalculateLayoutInputHorizontal()
    {
      base.CalculateLayoutInputHorizontal();
      int constraintCount;
      int num;
      if (this.m_Constraint == GridLayoutGroup.Constraint.FixedColumnCount)
        num = constraintCount = this.m_ConstraintCount;
      else if (this.m_Constraint == GridLayoutGroup.Constraint.FixedRowCount)
      {
        num = constraintCount = Mathf.CeilToInt((float) ((double) this.rectChildren.Count / (double) this.m_ConstraintCount - 1.0 / 1000.0));
      }
      else
      {
        num = 1;
        constraintCount = Mathf.CeilToInt(Mathf.Sqrt((float) this.rectChildren.Count));
      }
      this.SetLayoutInputForAxis((float) this.padding.horizontal + (this.cellSize.x + this.spacing.x) * (float) num - this.spacing.x, (float) this.padding.horizontal + (this.cellSize.x + this.spacing.x) * (float) constraintCount - this.spacing.x, -1f, 0);
    }

    /// <summary>
    ///   <para>Called by the layout system.</para>
    /// </summary>
    public override void CalculateLayoutInputVertical()
    {
      float num = (float) this.padding.vertical + (this.cellSize.y + this.spacing.y) * (this.m_Constraint != GridLayoutGroup.Constraint.FixedColumnCount ? (this.m_Constraint != GridLayoutGroup.Constraint.FixedRowCount ? (float) Mathf.CeilToInt((float) this.rectChildren.Count / (float) Mathf.Max(1, Mathf.FloorToInt((float) (((double) this.rectTransform.rect.size.x - (double) this.padding.horizontal + (double) this.spacing.x + 1.0 / 1000.0) / ((double) this.cellSize.x + (double) this.spacing.x))))) : (float) this.m_ConstraintCount) : (float) Mathf.CeilToInt((float) ((double) this.rectChildren.Count / (double) this.m_ConstraintCount - 1.0 / 1000.0))) - this.spacing.y;
      this.SetLayoutInputForAxis(num, num, -1f, 1);
    }

    /// <summary>
    ///   <para>Called by the layout system.</para>
    /// </summary>
    public override void SetLayoutHorizontal()
    {
      this.SetCellsAlongAxis(0);
    }

    /// <summary>
    ///   <para>Called by the layout system.</para>
    /// </summary>
    public override void SetLayoutVertical()
    {
      this.SetCellsAlongAxis(1);
    }

    private void SetCellsAlongAxis(int axis)
    {
      if (axis == 0)
      {
        for (int index = 0; index < this.rectChildren.Count; ++index)
        {
          RectTransform rectChild = this.rectChildren[index];
          this.m_Tracker.Add((Object) this, rectChild, DrivenTransformProperties.Anchors | DrivenTransformProperties.AnchoredPosition | DrivenTransformProperties.SizeDelta);
          rectChild.anchorMin = Vector2.up;
          rectChild.anchorMax = Vector2.up;
          rectChild.sizeDelta = this.cellSize;
        }
      }
      else
      {
        float x = this.rectTransform.rect.size.x;
        float y = this.rectTransform.rect.size.y;
        int num1;
        int num2;
        if (this.m_Constraint == GridLayoutGroup.Constraint.FixedColumnCount)
        {
          num1 = this.m_ConstraintCount;
          num2 = Mathf.CeilToInt((float) ((double) this.rectChildren.Count / (double) num1 - 1.0 / 1000.0));
        }
        else if (this.m_Constraint == GridLayoutGroup.Constraint.FixedRowCount)
        {
          num2 = this.m_ConstraintCount;
          num1 = Mathf.CeilToInt((float) ((double) this.rectChildren.Count / (double) num2 - 1.0 / 1000.0));
        }
        else
        {
          num1 = (double) this.cellSize.x + (double) this.spacing.x > 0.0 ? Mathf.Max(1, Mathf.FloorToInt((float) (((double) x - (double) this.padding.horizontal + (double) this.spacing.x + 1.0 / 1000.0) / ((double) this.cellSize.x + (double) this.spacing.x)))) : int.MaxValue;
          num2 = (double) this.cellSize.y + (double) this.spacing.y > 0.0 ? Mathf.Max(1, Mathf.FloorToInt((float) (((double) y - (double) this.padding.vertical + (double) this.spacing.y + 1.0 / 1000.0) / ((double) this.cellSize.y + (double) this.spacing.y)))) : int.MaxValue;
        }
        int num3 = (int) this.startCorner % 2;
        int num4 = (int) this.startCorner / 2;
        int num5;
        int num6;
        int num7;
        if (this.startAxis == GridLayoutGroup.Axis.Horizontal)
        {
          num5 = num1;
          num6 = Mathf.Clamp(num1, 1, this.rectChildren.Count);
          num7 = Mathf.Clamp(num2, 1, Mathf.CeilToInt((float) this.rectChildren.Count / (float) num5));
        }
        else
        {
          num5 = num2;
          num7 = Mathf.Clamp(num2, 1, this.rectChildren.Count);
          num6 = Mathf.Clamp(num1, 1, Mathf.CeilToInt((float) this.rectChildren.Count / (float) num5));
        }
        Vector2 vector2_1 = new Vector2((float) ((double) num6 * (double) this.cellSize.x + (double) (num6 - 1) * (double) this.spacing.x), (float) ((double) num7 * (double) this.cellSize.y + (double) (num7 - 1) * (double) this.spacing.y));
        Vector2 vector2_2 = new Vector2(this.GetStartOffset(0, vector2_1.x), this.GetStartOffset(1, vector2_1.y));
        for (int index = 0; index < this.rectChildren.Count; ++index)
        {
          int num8;
          int num9;
          if (this.startAxis == GridLayoutGroup.Axis.Horizontal)
          {
            num8 = index % num5;
            num9 = index / num5;
          }
          else
          {
            num8 = index / num5;
            num9 = index % num5;
          }
          if (num3 == 1)
            num8 = num6 - 1 - num8;
          if (num4 == 1)
            num9 = num7 - 1 - num9;
          this.SetChildAlongAxis(this.rectChildren[index], 0, vector2_2.x + (this.cellSize[0] + this.spacing[0]) * (float) num8, this.cellSize[0]);
          this.SetChildAlongAxis(this.rectChildren[index], 1, vector2_2.y + (this.cellSize[1] + this.spacing[1]) * (float) num9, this.cellSize[1]);
        }
      }
    }

    /// <summary>
    ///   <para>One of the four corners in a rectangle.</para>
    /// </summary>
    public enum Corner
    {
      UpperLeft,
      UpperRight,
      LowerLeft,
      LowerRight,
    }

    /// <summary>
    ///   <para>An axis that can be horizontal or vertical.</para>
    /// </summary>
    public enum Axis
    {
      Horizontal,
      Vertical,
    }

    /// <summary>
    ///   <para>A constraint on either the number of columns or rows.</para>
    /// </summary>
    public enum Constraint
    {
      Flexible,
      FixedColumnCount,
      FixedRowCount,
    }
  }
}
