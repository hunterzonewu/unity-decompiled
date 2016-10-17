// Decompiled with JetBrains decompiler
// Type: UnityEditor.InstructionOverlayWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal class InstructionOverlayWindow : EditorWindow
  {
    private static InstructionOverlayWindow.Styles s_Styles;
    private GUIView m_InspectedGUIView;
    private Rect m_InstructionRect;
    private GUIStyle m_InstructionStyle;
    private RenderTexture m_RenderTexture;
    [NonSerialized]
    private bool m_RenderTextureNeedsRefresh;

    private InstructionOverlayWindow.Styles styles
    {
      get
      {
        if (InstructionOverlayWindow.s_Styles == null)
          InstructionOverlayWindow.s_Styles = new InstructionOverlayWindow.Styles();
        return InstructionOverlayWindow.s_Styles;
      }
    }

    private void Start()
    {
      this.minSize = Vector2.zero;
      this.m_Parent.window.m_DontSaveToLayout = true;
    }

    public void SetTransparent(float d)
    {
      this.m_Parent.window.SetAlpha(d);
      this.m_Parent.window.SetInvisible();
    }

    public void Show(GUIView view, Rect instructionRect, GUIStyle style)
    {
      this.minSize = Vector2.zero;
      this.m_InstructionStyle = style;
      this.m_InspectedGUIView = view;
      this.m_InstructionRect = instructionRect;
      Rect rect = new Rect(instructionRect);
      rect.x += this.m_InspectedGUIView.screenPosition.x;
      rect.y += this.m_InspectedGUIView.screenPosition.y;
      this.position = rect;
      this.m_RenderTextureNeedsRefresh = true;
      this.ShowWithMode(ShowMode.NoShadow);
      this.m_Parent.window.m_DontSaveToLayout = true;
      this.Repaint();
    }

    private void DoRefreshRenderTexture()
    {
      if ((UnityEngine.Object) this.m_RenderTexture == (UnityEngine.Object) null)
      {
        this.m_RenderTexture = new RenderTexture(Mathf.CeilToInt(this.m_InstructionRect.width), Mathf.CeilToInt(this.m_InstructionRect.height), 24);
        this.m_RenderTexture.Create();
      }
      else if ((double) this.m_RenderTexture.width != (double) this.m_InstructionRect.width || (double) this.m_RenderTexture.height != (double) this.m_InstructionRect.height)
      {
        this.m_RenderTexture.Release();
        this.m_RenderTexture.width = Mathf.CeilToInt(this.m_InstructionRect.width);
        this.m_RenderTexture.height = Mathf.CeilToInt(this.m_InstructionRect.height);
        this.m_RenderTexture.Create();
      }
      this.m_RenderTextureNeedsRefresh = false;
      this.Repaint();
    }

    private void Update()
    {
      if (!this.m_RenderTextureNeedsRefresh)
        return;
      this.DoRefreshRenderTexture();
    }

    private void OnFocus()
    {
      EditorWindow.GetWindow<GUIViewDebuggerWindow>();
    }

    private void OnGUI()
    {
      Color color1 = new Color(0.76f, 0.87f, 0.71f);
      Color color2 = new Color(0.62f, 0.77f, 0.9f);
      Rect rect = new Rect(0.0f, 0.0f, this.m_InstructionRect.width, this.m_InstructionRect.height);
      GUI.backgroundColor = color1;
      GUI.Box(rect, GUIContent.none, this.styles.solidColor);
      Rect position = this.m_InstructionStyle.padding.Remove(rect);
      GUI.backgroundColor = color2;
      GUI.Box(position, GUIContent.none, this.styles.solidColor);
    }

    private class Styles
    {
      public GUIStyle solidColor;

      public Styles()
      {
        this.solidColor = new GUIStyle();
        this.solidColor.normal.background = EditorGUIUtility.whiteTexture;
      }
    }
  }
}
