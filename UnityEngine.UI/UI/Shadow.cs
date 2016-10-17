// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.Shadow
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using System.Collections.Generic;

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>Adds an outline to a graphic using IVertexModifier.</para>
  /// </summary>
  [AddComponentMenu("UI/Effects/Shadow", 14)]
  public class Shadow : BaseMeshEffect
  {
    [SerializeField]
    private Color m_EffectColor = new Color(0.0f, 0.0f, 0.0f, 0.5f);
    [SerializeField]
    private Vector2 m_EffectDistance = new Vector2(1f, -1f);
    [SerializeField]
    private bool m_UseGraphicAlpha = true;
    private const float kMaxEffectDistance = 600f;

    /// <summary>
    ///   <para>Color for the effect.</para>
    /// </summary>
    public Color effectColor
    {
      get
      {
        return this.m_EffectColor;
      }
      set
      {
        this.m_EffectColor = value;
        if (!((Object) this.graphic != (Object) null))
          return;
        this.graphic.SetVerticesDirty();
      }
    }

    /// <summary>
    ///   <para>How far is the shadow from the graphic.</para>
    /// </summary>
    public Vector2 effectDistance
    {
      get
      {
        return this.m_EffectDistance;
      }
      set
      {
        if ((double) value.x > 600.0)
          value.x = 600f;
        if ((double) value.x < -600.0)
          value.x = -600f;
        if ((double) value.y > 600.0)
          value.y = 600f;
        if ((double) value.y < -600.0)
          value.y = -600f;
        if (this.m_EffectDistance == value)
          return;
        this.m_EffectDistance = value;
        if (!((Object) this.graphic != (Object) null))
          return;
        this.graphic.SetVerticesDirty();
      }
    }

    /// <summary>
    ///   <para>Should the shadow inherit the alpha from the graphic?</para>
    /// </summary>
    public bool useGraphicAlpha
    {
      get
      {
        return this.m_UseGraphicAlpha;
      }
      set
      {
        this.m_UseGraphicAlpha = value;
        if (!((Object) this.graphic != (Object) null))
          return;
        this.graphic.SetVerticesDirty();
      }
    }

    protected Shadow()
    {
    }

    protected override void OnValidate()
    {
      this.effectDistance = this.m_EffectDistance;
      base.OnValidate();
    }

    protected void ApplyShadowZeroAlloc(List<UIVertex> verts, Color32 color, int start, int end, float x, float y)
    {
      int num = verts.Count + end - start;
      if (verts.Capacity < num)
        verts.Capacity = num;
      for (int index = start; index < end; ++index)
      {
        UIVertex vert = verts[index];
        verts.Add(vert);
        Vector3 position = vert.position;
        position.x += x;
        position.y += y;
        vert.position = position;
        Color32 color32 = color;
        if (this.m_UseGraphicAlpha)
          color32.a = (byte) ((int) color32.a * (int) verts[index].color.a / (int) byte.MaxValue);
        vert.color = color32;
        verts[index] = vert;
      }
    }

    protected void ApplyShadow(List<UIVertex> verts, Color32 color, int start, int end, float x, float y)
    {
      this.ApplyShadowZeroAlloc(verts, color, start, end, x, y);
    }

    public override void ModifyMesh(VertexHelper vh)
    {
      if (!this.IsActive())
        return;
      List<UIVertex> uiVertexList = ListPool<UIVertex>.Get();
      vh.GetUIVertexStream(uiVertexList);
      this.ApplyShadow(uiVertexList, (Color32) this.effectColor, 0, uiVertexList.Count, this.effectDistance.x, this.effectDistance.y);
      vh.Clear();
      vh.AddUIVertexTriangleStream(uiVertexList);
      ListPool<UIVertex>.Release(uiVertexList);
    }
  }
}
