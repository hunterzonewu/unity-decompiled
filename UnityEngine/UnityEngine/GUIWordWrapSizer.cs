// Decompiled with JetBrains decompiler
// Type: UnityEngine.GUIWordWrapSizer
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

namespace UnityEngine
{
  internal sealed class GUIWordWrapSizer : GUILayoutEntry
  {
    private readonly GUIContent m_Content;
    private readonly float m_ForcedMinHeight;
    private readonly float m_ForcedMaxHeight;

    public GUIWordWrapSizer(GUIStyle style, GUIContent content, GUILayoutOption[] options)
      : base(0.0f, 0.0f, 0.0f, 0.0f, style)
    {
      this.m_Content = new GUIContent(content);
      this.ApplyOptions(options);
      this.m_ForcedMinHeight = this.minHeight;
      this.m_ForcedMaxHeight = this.maxHeight;
    }

    public override void CalcWidth()
    {
      if ((double) this.minWidth != 0.0 && (double) this.maxWidth != 0.0)
        return;
      float minWidth;
      float maxWidth;
      this.style.CalcMinMaxWidth(this.m_Content, out minWidth, out maxWidth);
      if ((double) this.minWidth == 0.0)
        this.minWidth = minWidth;
      if ((double) this.maxWidth != 0.0)
        return;
      this.maxWidth = maxWidth;
    }

    public override void CalcHeight()
    {
      if ((double) this.m_ForcedMinHeight != 0.0 && (double) this.m_ForcedMaxHeight != 0.0)
        return;
      float num = this.style.CalcHeight(this.m_Content, this.rect.width);
      if ((double) this.m_ForcedMinHeight == 0.0)
        this.minHeight = num;
      else
        this.minHeight = this.m_ForcedMinHeight;
      if ((double) this.m_ForcedMaxHeight == 0.0)
        this.maxHeight = num;
      else
        this.maxHeight = this.m_ForcedMaxHeight;
    }
  }
}
