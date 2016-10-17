// Decompiled with JetBrains decompiler
// Type: UnityEditor.SpriteUtilityWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class SpriteUtilityWindow : EditorWindow
  {
    protected float m_Zoom = -1f;
    protected Vector2 m_ScrollPosition = new Vector2();
    protected const float k_BorderMargin = 10f;
    protected const float k_ScrollbarMargin = 16f;
    protected const float k_InspectorWindowMargin = 8f;
    protected const float k_InspectorWidth = 330f;
    protected const float k_InspectorHeight = 148f;
    protected const float k_MinZoomPercentage = 0.9f;
    protected const float k_MaxZoom = 50f;
    protected const float k_WheelZoomSpeed = 0.03f;
    protected const float k_MouseZoomSpeed = 0.005f;
    protected static SpriteUtilityWindow.Styles s_Styles;
    protected Texture2D m_Texture;
    protected Texture2D m_TextureAlphaOverride;
    protected Rect m_TextureViewRect;
    protected Rect m_TextureRect;
    protected bool m_ShowAlpha;
    protected float m_MipLevel;

    protected Rect maxScrollRect
    {
      get
      {
        float num1 = (float) this.m_Texture.width * 0.5f * this.m_Zoom;
        float num2 = (float) this.m_Texture.height * 0.5f * this.m_Zoom;
        return new Rect(-num1, -num2, this.m_TextureViewRect.width + num1 * 2f, this.m_TextureViewRect.height + num2 * 2f);
      }
    }

    protected Rect maxRect
    {
      get
      {
        float num1 = this.m_TextureViewRect.width * 0.5f / this.GetMinZoom();
        float num2 = this.m_TextureViewRect.height * 0.5f / this.GetMinZoom();
        return new Rect(-num1, -num2, (float) this.m_Texture.width + num1 * 2f, (float) this.m_Texture.height + num2 * 2f);
      }
    }

    protected void InitStyles()
    {
      if (SpriteUtilityWindow.s_Styles != null)
        return;
      SpriteUtilityWindow.s_Styles = new SpriteUtilityWindow.Styles();
    }

    protected float GetMinZoom()
    {
      if ((UnityEngine.Object) this.m_Texture == (UnityEngine.Object) null)
        return 1f;
      return Mathf.Min((float) ((double) this.m_TextureViewRect.width / (double) this.m_Texture.width), (float) ((double) this.m_TextureViewRect.height / (double) this.m_Texture.height), 50f) * 0.9f;
    }

    protected void HandleZoom()
    {
      bool flag = Event.current.alt && Event.current.button == 1;
      if (flag)
        EditorGUIUtility.AddCursorRect(this.m_TextureViewRect, MouseCursor.Zoom);
      if ((Event.current.type == EventType.MouseUp || Event.current.type == EventType.MouseDown) && flag || (Event.current.type == EventType.KeyUp || Event.current.type == EventType.KeyDown) && Event.current.keyCode == KeyCode.LeftAlt)
        this.Repaint();
      if (Event.current.type != EventType.ScrollWheel && (Event.current.type != EventType.MouseDrag || !Event.current.alt || Event.current.button != 1))
        return;
      float num1 = (float) (1.0 - (double) Event.current.delta.y * (Event.current.type != EventType.ScrollWheel ? -0.00499999988824129 : 0.0299999993294477));
      float num2 = this.m_Zoom * num1;
      float num3 = Mathf.Clamp(num2, this.GetMinZoom(), 50f);
      if ((double) num3 == (double) this.m_Zoom)
        return;
      this.m_Zoom = num3;
      if ((double) num2 != (double) num3)
        num1 /= num2 / num3;
      this.m_ScrollPosition *= num1;
      float num4 = (float) ((double) Event.current.mousePosition.x / (double) this.m_TextureViewRect.width - 0.5);
      float num5 = (float) ((double) Event.current.mousePosition.y / (double) this.m_TextureViewRect.height - 0.5);
      float num6 = num4 * (num1 - 1f);
      float num7 = num5 * (num1 - 1f);
      Rect maxScrollRect = this.maxScrollRect;
      this.m_ScrollPosition.x += num6 * (maxScrollRect.width / 2f);
      this.m_ScrollPosition.y += num7 * (maxScrollRect.height / 2f);
      Event.current.Use();
    }

    protected void HandlePanning()
    {
      bool flag = !Event.current.alt && Event.current.button > 0 || Event.current.alt && Event.current.button <= 0;
      if (flag && GUIUtility.hotControl == 0)
      {
        EditorGUIUtility.AddCursorRect(this.m_TextureViewRect, MouseCursor.Pan);
        if (Event.current.type == EventType.MouseDrag)
        {
          this.m_ScrollPosition -= Event.current.delta;
          Event.current.Use();
        }
      }
      if ((Event.current.type != EventType.MouseUp && Event.current.type != EventType.MouseDown || !flag) && (Event.current.type != EventType.KeyUp && Event.current.type != EventType.KeyDown || Event.current.keyCode != KeyCode.LeftAlt))
        return;
      this.Repaint();
    }

    protected void DrawTexturespaceBackground()
    {
      float num1 = Mathf.Max(this.maxRect.width, this.maxRect.height);
      Vector2 vector2 = new Vector2(this.maxRect.xMin, this.maxRect.yMin);
      float num2 = num1 * 0.5f;
      float a = !EditorGUIUtility.isProSkin ? 0.08f : 0.15f;
      float num3 = 8f;
      SpriteEditorUtility.BeginLines(new Color(0.0f, 0.0f, 0.0f, a));
      float num4 = 0.0f;
      while ((double) num4 <= (double) num1)
      {
        SpriteEditorUtility.DrawLine((Vector3) (new Vector2(-num2 + num4, num2 + num4) + vector2), (Vector3) (new Vector2(num2 + num4, -num2 + num4) + vector2));
        num4 += num3;
      }
      SpriteEditorUtility.EndLines();
    }

    private float Log2(float x)
    {
      return (float) (Math.Log((double) x) / Math.Log(2.0));
    }

    protected void DrawTexture()
    {
      int num1 = Mathf.Max(this.m_Texture.width, 1);
      float num2 = Mathf.Min(this.m_MipLevel, (float) (TextureUtil.CountMipmaps((Texture) this.m_Texture) - 1));
      float mipMapBias = this.m_Texture.mipMapBias;
      TextureUtil.SetMipMapBiasNoDirty((Texture) this.m_Texture, num2 - this.Log2((float) num1 / this.m_TextureRect.width));
      UnityEngine.FilterMode filterMode = this.m_Texture.filterMode;
      TextureUtil.SetFilterModeNoDirty((Texture) this.m_Texture, UnityEngine.FilterMode.Point);
      if (this.m_ShowAlpha)
      {
        if ((UnityEngine.Object) this.m_TextureAlphaOverride != (UnityEngine.Object) null)
          EditorGUI.DrawTextureTransparent(this.m_TextureRect, (Texture) this.m_TextureAlphaOverride);
        else
          EditorGUI.DrawTextureAlpha(this.m_TextureRect, (Texture) this.m_Texture);
      }
      else
        EditorGUI.DrawTextureTransparent(this.m_TextureRect, (Texture) this.m_Texture);
      TextureUtil.SetMipMapBiasNoDirty((Texture) this.m_Texture, mipMapBias);
      TextureUtil.SetFilterModeNoDirty((Texture) this.m_Texture, filterMode);
    }

    protected void DrawScreenspaceBackground()
    {
      if (Event.current.type != EventType.Repaint)
        return;
      SpriteUtilityWindow.s_Styles.preBackground.Draw(this.m_TextureViewRect, false, false, false, false);
    }

    protected void HandleScrollbars()
    {
      this.m_ScrollPosition.x = GUI.HorizontalScrollbar(new Rect(this.m_TextureViewRect.xMin, this.m_TextureViewRect.yMax, this.m_TextureViewRect.width, 16f), this.m_ScrollPosition.x, this.m_TextureViewRect.width, this.maxScrollRect.xMin, this.maxScrollRect.xMax);
      this.m_ScrollPosition.y = GUI.VerticalScrollbar(new Rect(this.m_TextureViewRect.xMax, this.m_TextureViewRect.yMin, 16f, this.m_TextureViewRect.height), this.m_ScrollPosition.y, this.m_TextureViewRect.height, this.maxScrollRect.yMin, this.maxScrollRect.yMax);
    }

    protected void SetupHandlesMatrix()
    {
      Handles.matrix = Matrix4x4.TRS(new Vector3(this.m_TextureRect.x, this.m_TextureRect.yMax, 0.0f), Quaternion.identity, new Vector3(this.m_Zoom, -this.m_Zoom, 1f));
    }

    protected void DoAlphaZoomToolbarGUI()
    {
      this.m_ShowAlpha = GUILayout.Toggle(this.m_ShowAlpha, !this.m_ShowAlpha ? SpriteUtilityWindow.s_Styles.RGBIcon : SpriteUtilityWindow.s_Styles.alphaIcon, (GUIStyle) "toolbarButton", new GUILayoutOption[0]);
      this.m_Zoom = GUILayout.HorizontalSlider(this.m_Zoom, this.GetMinZoom(), 50f, SpriteUtilityWindow.s_Styles.preSlider, SpriteUtilityWindow.s_Styles.preSliderThumb, GUILayout.MaxWidth(64f));
      int a = 1;
      if ((UnityEngine.Object) this.m_Texture != (UnityEngine.Object) null)
        a = Mathf.Max(a, TextureUtil.CountMipmaps((Texture) this.m_Texture));
      EditorGUI.BeginDisabledGroup(a == 1);
      GUILayout.Box(SpriteUtilityWindow.s_Styles.smallMip, SpriteUtilityWindow.s_Styles.preLabel, new GUILayoutOption[0]);
      this.m_MipLevel = Mathf.Round(GUILayout.HorizontalSlider(this.m_MipLevel, (float) (a - 1), 0.0f, SpriteUtilityWindow.s_Styles.preSlider, SpriteUtilityWindow.s_Styles.preSliderThumb, GUILayout.MaxWidth(64f)));
      GUILayout.Box(SpriteUtilityWindow.s_Styles.largeMip, SpriteUtilityWindow.s_Styles.preLabel, new GUILayoutOption[0]);
      EditorGUI.EndDisabledGroup();
    }

    protected void DoTextureGUI()
    {
      if ((UnityEngine.Object) this.m_Texture == (UnityEngine.Object) null)
        return;
      if ((double) this.m_Zoom < 0.0)
        this.m_Zoom = this.GetMinZoom();
      this.m_TextureRect = new Rect((float) ((double) this.m_TextureViewRect.width / 2.0 - (double) this.m_Texture.width * (double) this.m_Zoom / 2.0), (float) ((double) this.m_TextureViewRect.height / 2.0 - (double) this.m_Texture.height * (double) this.m_Zoom / 2.0), (float) this.m_Texture.width * this.m_Zoom, (float) this.m_Texture.height * this.m_Zoom);
      this.HandleScrollbars();
      this.SetupHandlesMatrix();
      this.HandleZoom();
      this.HandlePanning();
      this.DrawScreenspaceBackground();
      GUIClip.Push(this.m_TextureViewRect, -this.m_ScrollPosition, Vector2.zero, false);
      if (Event.current.type == EventType.Repaint)
      {
        this.DrawTexturespaceBackground();
        this.DrawTexture();
        this.DrawGizmos();
      }
      this.DoTextureGUIExtras();
      GUIClip.Pop();
    }

    protected virtual void DoTextureGUIExtras()
    {
    }

    protected virtual void DrawGizmos()
    {
    }

    protected void SetNewTexture(Texture2D texture)
    {
      if (!((UnityEngine.Object) texture != (UnityEngine.Object) this.m_Texture))
        return;
      this.m_Texture = texture;
      this.m_Zoom = -1f;
      this.m_TextureAlphaOverride = (Texture2D) null;
    }

    protected void SetAlphaTextureOverride(Texture2D alphaTexture)
    {
      if (!((UnityEngine.Object) alphaTexture != (UnityEngine.Object) this.m_TextureAlphaOverride))
        return;
      this.m_TextureAlphaOverride = alphaTexture;
      this.m_Zoom = -1f;
    }

    internal override void OnResized()
    {
      if (!((UnityEngine.Object) this.m_Texture != (UnityEngine.Object) null) || Event.current == null)
        return;
      this.HandleZoom();
    }

    protected class Styles
    {
      public static readonly GUIContent[] spriteAlignmentOptions = new GUIContent[10]{ EditorGUIUtility.TextContent("Center"), EditorGUIUtility.TextContent("Top Left"), EditorGUIUtility.TextContent("Top"), EditorGUIUtility.TextContent("Top Right"), EditorGUIUtility.TextContent("Left"), EditorGUIUtility.TextContent("Right"), EditorGUIUtility.TextContent("Bottom Left"), EditorGUIUtility.TextContent("Bottom"), EditorGUIUtility.TextContent("Bottom Right"), EditorGUIUtility.TextContent("Custom") };
      public static GUIContent s_PivotLabel = EditorGUIUtility.TextContent("Pivot");
      public static GUIContent s_NoSelectionWarning = EditorGUIUtility.TextContent("No texture or sprite selected");
      public readonly GUIStyle dragdot = (GUIStyle) "U2D.dragDot";
      public readonly GUIStyle dragdotDimmed = (GUIStyle) "U2D.dragDotDimmed";
      public readonly GUIStyle dragdotactive = (GUIStyle) "U2D.dragDotActive";
      public readonly GUIStyle createRect = (GUIStyle) "U2D.createRect";
      public readonly GUIStyle preToolbar = (GUIStyle) "preToolbar";
      public readonly GUIStyle preButton = (GUIStyle) "preButton";
      public readonly GUIStyle preLabel = (GUIStyle) "preLabel";
      public readonly GUIStyle preSlider = (GUIStyle) "preSlider";
      public readonly GUIStyle preSliderThumb = (GUIStyle) "preSliderThumb";
      public readonly GUIStyle preBackground = (GUIStyle) "preBackground";
      public readonly GUIStyle pivotdotactive = (GUIStyle) "U2D.pivotDotActive";
      public readonly GUIStyle pivotdot = (GUIStyle) "U2D.pivotDot";
      public readonly GUIStyle dragBorderdot = new GUIStyle();
      public readonly GUIStyle dragBorderDotActive = new GUIStyle();
      public readonly GUIStyle toolbar;
      public readonly GUIContent alphaIcon;
      public readonly GUIContent RGBIcon;
      public readonly GUIStyle notice;
      public readonly GUIContent smallMip;
      public readonly GUIContent largeMip;

      public Styles()
      {
        this.toolbar = new GUIStyle(EditorStyles.inspectorBig);
        this.toolbar.margin.top = 0;
        this.toolbar.margin.bottom = 0;
        this.alphaIcon = EditorGUIUtility.IconContent("PreTextureAlpha");
        this.RGBIcon = EditorGUIUtility.IconContent("PreTextureRGB");
        this.preToolbar.border.top = 0;
        this.createRect.border = new RectOffset(3, 3, 3, 3);
        this.notice = new GUIStyle(GUI.skin.label);
        this.notice.alignment = TextAnchor.MiddleCenter;
        this.notice.normal.textColor = Color.yellow;
        this.dragBorderdot.fixedHeight = 5f;
        this.dragBorderdot.fixedWidth = 5f;
        this.dragBorderdot.normal.background = EditorGUIUtility.whiteTexture;
        this.dragBorderDotActive.fixedHeight = this.dragBorderdot.fixedHeight;
        this.dragBorderDotActive.fixedWidth = this.dragBorderdot.fixedWidth;
        this.dragBorderDotActive.normal.background = EditorGUIUtility.whiteTexture;
        this.smallMip = EditorGUIUtility.IconContent("PreTextureMipMapLow");
        this.largeMip = EditorGUIUtility.IconContent("PreTextureMipMapHigh");
      }
    }
  }
}
