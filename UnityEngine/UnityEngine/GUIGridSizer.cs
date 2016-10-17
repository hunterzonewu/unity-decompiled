// Decompiled with JetBrains decompiler
// Type: UnityEngine.GUIGridSizer
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

namespace UnityEngine
{
  internal sealed class GUIGridSizer : GUILayoutEntry
  {
    private readonly float m_MinButtonWidth = -1f;
    private readonly float m_MaxButtonWidth = -1f;
    private readonly float m_MinButtonHeight = -1f;
    private readonly float m_MaxButtonHeight = -1f;
    private readonly int m_Count;
    private readonly int m_XCount;

    private int rows
    {
      get
      {
        int num = this.m_Count / this.m_XCount;
        if (this.m_Count % this.m_XCount != 0)
          ++num;
        return num;
      }
    }

    private GUIGridSizer(GUIContent[] contents, int xCount, GUIStyle buttonStyle, GUILayoutOption[] options)
      : base(0.0f, 0.0f, 0.0f, 0.0f, GUIStyle.none)
    {
      this.m_Count = contents.Length;
      this.m_XCount = xCount;
      this.ApplyStyleSettings(buttonStyle);
      this.ApplyOptions(options);
      if (xCount == 0 || contents.Length == 0)
        return;
      float num1 = (float) (Mathf.Max(buttonStyle.margin.left, buttonStyle.margin.right) * (this.m_XCount - 1));
      float num2 = (float) (Mathf.Max(buttonStyle.margin.top, buttonStyle.margin.bottom) * (this.rows - 1));
      if ((double) buttonStyle.fixedWidth != 0.0)
        this.m_MinButtonWidth = this.m_MaxButtonWidth = buttonStyle.fixedWidth;
      if ((double) buttonStyle.fixedHeight != 0.0)
        this.m_MinButtonHeight = this.m_MaxButtonHeight = buttonStyle.fixedHeight;
      if ((double) this.m_MinButtonWidth == -1.0)
      {
        if ((double) this.minWidth != 0.0)
          this.m_MinButtonWidth = (this.minWidth - num1) / (float) this.m_XCount;
        if ((double) this.maxWidth != 0.0)
          this.m_MaxButtonWidth = (this.maxWidth - num1) / (float) this.m_XCount;
      }
      if ((double) this.m_MinButtonHeight == -1.0)
      {
        if ((double) this.minHeight != 0.0)
          this.m_MinButtonHeight = (this.minHeight - num2) / (float) this.rows;
        if ((double) this.maxHeight != 0.0)
          this.m_MaxButtonHeight = (this.maxHeight - num2) / (float) this.rows;
      }
      if ((double) this.m_MinButtonHeight == -1.0 || (double) this.m_MaxButtonHeight == -1.0 || ((double) this.m_MinButtonWidth == -1.0 || (double) this.m_MaxButtonWidth == -1.0))
      {
        float a1 = 0.0f;
        float a2 = 0.0f;
        foreach (GUIContent content in contents)
        {
          Vector2 vector2 = buttonStyle.CalcSize(content);
          a2 = Mathf.Max(a2, vector2.x);
          a1 = Mathf.Max(a1, vector2.y);
        }
        if ((double) this.m_MinButtonWidth == -1.0)
          this.m_MinButtonWidth = (double) this.m_MaxButtonWidth == -1.0 ? a2 : Mathf.Min(a2, this.m_MaxButtonWidth);
        if ((double) this.m_MaxButtonWidth == -1.0)
          this.m_MaxButtonWidth = (double) this.m_MinButtonWidth == -1.0 ? a2 : Mathf.Max(a2, this.m_MinButtonWidth);
        if ((double) this.m_MinButtonHeight == -1.0)
          this.m_MinButtonHeight = (double) this.m_MaxButtonHeight == -1.0 ? a1 : Mathf.Min(a1, this.m_MaxButtonHeight);
        if ((double) this.m_MaxButtonHeight == -1.0)
        {
          if ((double) this.m_MinButtonHeight != -1.0)
            this.maxHeight = Mathf.Max(this.maxHeight, this.m_MinButtonHeight);
          this.m_MaxButtonHeight = this.maxHeight;
        }
      }
      this.minWidth = this.m_MinButtonWidth * (float) this.m_XCount + num1;
      this.maxWidth = this.m_MaxButtonWidth * (float) this.m_XCount + num1;
      this.minHeight = this.m_MinButtonHeight * (float) this.rows + num2;
      this.maxHeight = this.m_MaxButtonHeight * (float) this.rows + num2;
    }

    public static Rect GetRect(GUIContent[] contents, int xCount, GUIStyle style, GUILayoutOption[] options)
    {
      Rect rect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
      switch (Event.current.type)
      {
        case EventType.Layout:
          GUILayoutUtility.current.topLevel.Add((GUILayoutEntry) new GUIGridSizer(contents, xCount, style, options));
          break;
        case EventType.Used:
          return GUILayoutEntry.kDummyRect;
        default:
          rect = GUILayoutUtility.current.topLevel.GetNext().rect;
          break;
      }
      return rect;
    }
  }
}
