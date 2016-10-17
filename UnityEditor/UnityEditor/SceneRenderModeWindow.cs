// Decompiled with JetBrains decompiler
// Type: UnityEditor.SceneRenderModeWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class SceneRenderModeWindow : PopupWindowContent
  {
    private static readonly int sRenderModeCount = SceneRenderModeWindow.Styles.sRenderModeOptions.Length;
    private static readonly int sMenuRowCount = SceneRenderModeWindow.sRenderModeCount + 4;
    private readonly float m_WindowHeight = (float) ((double) SceneRenderModeWindow.sMenuRowCount * 16.0 + 9.0 + 22.0);
    private const float m_WindowWidth = 205f;
    private const int kMenuHeaderCount = 4;
    private const float kSeparatorHeight = 3f;
    private const float kFrameWidth = 1f;
    private const float kHeaderHorizontalPadding = 5f;
    private const float kHeaderVerticalPadding = 1f;
    private const float kShowLightmapResolutionHeight = 22f;
    private const float kTogglePadding = 7f;
    private SerializedProperty m_EnableRealtimeGI;
    private SerializedProperty m_EnableBakedGI;
    private readonly SceneView m_SceneView;

    public SceneRenderModeWindow(SceneView sceneView)
    {
      this.m_SceneView = sceneView;
    }

    public override Vector2 GetWindowSize()
    {
      return new Vector2(205f, this.m_WindowHeight);
    }

    public override void OnOpen()
    {
      SerializedObject serializedObject = new SerializedObject(LightmapEditorSettings.GetLightmapSettings());
      this.m_EnableRealtimeGI = serializedObject.FindProperty("m_GISettings.m_EnableRealtimeLightmaps");
      this.m_EnableBakedGI = serializedObject.FindProperty("m_GISettings.m_EnableBakedLightmaps");
    }

    public override void OnGUI(Rect rect)
    {
      if ((Object) this.m_SceneView == (Object) null || this.m_SceneView.m_SceneViewState == null || Event.current.type == EventType.Layout)
        return;
      this.Draw(this.editorWindow, rect.width);
      if (Event.current.type == EventType.MouseMove)
        Event.current.Use();
      if (Event.current.type != EventType.KeyDown || Event.current.keyCode != KeyCode.Escape)
        return;
      this.editorWindow.Close();
      GUIUtility.ExitGUI();
    }

    private void DrawSeparator(ref Rect rect)
    {
      Rect position = rect;
      position.x += 5f;
      position.y += 3f;
      position.width -= 10f;
      position.height = 3f;
      GUI.Label(position, GUIContent.none, SceneRenderModeWindow.Styles.sSeparator);
      rect.y += 3f;
    }

    private void DrawHeader(ref Rect rect, GUIContent label)
    {
      Rect position = rect;
      ++position.y;
      position.x += 5f;
      position.width = EditorStyles.miniLabel.CalcSize(label).x;
      position.height = EditorStyles.miniLabel.CalcSize(label).y;
      GUI.Label(position, label, EditorStyles.miniLabel);
      rect.y += 16f;
    }

    private void Draw(EditorWindow caller, float listElementWidth)
    {
      Rect rect = new Rect(0.0f, 0.0f, listElementWidth, 16f);
      this.DrawHeader(ref rect, SceneRenderModeWindow.Styles.sShadedHeader);
      for (int index = 0; index < SceneRenderModeWindow.sRenderModeCount; ++index)
      {
        DrawCameraMode drawCameraMode = (DrawCameraMode) index;
        switch (drawCameraMode)
        {
          case DrawCameraMode.ShadowCascades:
            this.DrawSeparator(ref rect);
            this.DrawHeader(ref rect, SceneRenderModeWindow.Styles.sMiscellaneous);
            break;
          case DrawCameraMode.DeferredDiffuse:
            this.DrawSeparator(ref rect);
            this.DrawHeader(ref rect, SceneRenderModeWindow.Styles.sDeferredHeader);
            break;
          case DrawCameraMode.Charting:
            this.DrawSeparator(ref rect);
            this.DrawHeader(ref rect, SceneRenderModeWindow.Styles.sGlobalIlluminationHeader);
            break;
        }
        EditorGUI.BeginDisabledGroup(this.IsModeDisabled(drawCameraMode));
        this.DoOneMode(caller, ref rect, drawCameraMode);
        EditorGUI.EndDisabledGroup();
      }
      bool disabled = this.m_SceneView.renderMode < DrawCameraMode.Charting || this.IsModeDisabled(this.m_SceneView.renderMode);
      this.DoResolutionToggle(rect, disabled);
    }

    private bool IsModeDisabled(DrawCameraMode mode)
    {
      if (!this.m_EnableBakedGI.boolValue && mode == DrawCameraMode.Baked)
        return true;
      if (!this.m_EnableRealtimeGI.boolValue && !this.m_EnableBakedGI.boolValue)
        return mode >= DrawCameraMode.Charting;
      return false;
    }

    private void DoResolutionToggle(Rect rect, bool disabled)
    {
      GUI.Label(new Rect(1f, rect.y, 203f, 22f), string.Empty, EditorStyles.inspectorBig);
      rect.y += 3f;
      rect.x += 7f;
      EditorGUI.BeginDisabledGroup(disabled);
      EditorGUI.BeginChangeCheck();
      bool flag = GUI.Toggle(rect, LightmapVisualization.showResolution, SceneRenderModeWindow.Styles.sResolutionToggle);
      if (EditorGUI.EndChangeCheck())
      {
        LightmapVisualization.showResolution = flag;
        SceneView.RepaintAll();
      }
      EditorGUI.EndDisabledGroup();
    }

    private void DoOneMode(EditorWindow caller, ref Rect rect, DrawCameraMode drawCameraMode)
    {
      EditorGUI.BeginDisabledGroup(!this.m_SceneView.CheckDrawModeForRenderingPath(drawCameraMode));
      EditorGUI.BeginChangeCheck();
      GUI.Toggle(rect, this.m_SceneView.renderMode == drawCameraMode, SceneRenderModeWindow.GetGUIContent(drawCameraMode), SceneRenderModeWindow.Styles.sMenuItem);
      if (EditorGUI.EndChangeCheck())
      {
        this.m_SceneView.renderMode = drawCameraMode;
        this.m_SceneView.Repaint();
        GUIUtility.ExitGUI();
      }
      rect.y += 16f;
      EditorGUI.EndDisabledGroup();
    }

    public static GUIContent GetGUIContent(DrawCameraMode drawCameraMode)
    {
      return SceneRenderModeWindow.Styles.sRenderModeOptions[(int) drawCameraMode];
    }

    private class Styles
    {
      public static readonly GUIStyle sMenuItem = (GUIStyle) "MenuItem";
      public static readonly GUIStyle sSeparator = (GUIStyle) "sv_iconselector_sep";
      public static readonly GUIContent sShadedHeader = EditorGUIUtility.TextContent("Shading Mode");
      public static readonly GUIContent sMiscellaneous = EditorGUIUtility.TextContent("Miscellaneous");
      public static readonly GUIContent sDeferredHeader = EditorGUIUtility.TextContent("Deferred");
      public static readonly GUIContent sGlobalIlluminationHeader = EditorGUIUtility.TextContent("Global Illumination");
      public static readonly GUIContent sResolutionToggle = EditorGUIUtility.TextContent("Show Lightmap Resolution");
      public static readonly GUIContent[] sRenderModeOptions = new GUIContent[21]
      {
        EditorGUIUtility.TextContent("Shaded"),
        EditorGUIUtility.TextContent("Wireframe"),
        EditorGUIUtility.TextContent("Shaded Wireframe"),
        EditorGUIUtility.TextContent("Shadow Cascades"),
        EditorGUIUtility.TextContent("Render Paths"),
        EditorGUIUtility.TextContent("Alpha Channel"),
        EditorGUIUtility.TextContent("Overdraw"),
        EditorGUIUtility.TextContent("Mipmaps"),
        EditorGUIUtility.TextContent("Albedo"),
        EditorGUIUtility.TextContent("Specular"),
        EditorGUIUtility.TextContent("Smoothness"),
        EditorGUIUtility.TextContent("Normal"),
        EditorGUIUtility.TextContent("UV Charts"),
        EditorGUIUtility.TextContent("Systems"),
        EditorGUIUtility.TextContent("Albedo"),
        EditorGUIUtility.TextContent("Emissive"),
        EditorGUIUtility.TextContent("Irradiance"),
        EditorGUIUtility.TextContent("Directionality"),
        EditorGUIUtility.TextContent("Baked"),
        EditorGUIUtility.TextContent("Clustering"),
        EditorGUIUtility.TextContent("Lit Clustering")
      };
    }
  }
}
