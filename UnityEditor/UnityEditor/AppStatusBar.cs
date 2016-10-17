// Decompiled with JetBrains decompiler
// Type: UnityEditor.AppStatusBar
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  internal class AppStatusBar : GUIView
  {
    private string m_LastMiniMemoryOverview = string.Empty;
    private static AppStatusBar s_AppStatusBar;
    private static GUIContent[] s_StatusWheel;
    private static GUIStyle background;
    private static GUIStyle resize;

    private void OnEnable()
    {
      AppStatusBar.s_AppStatusBar = this;
      AppStatusBar.s_StatusWheel = new GUIContent[12];
      for (int index = 0; index < 12; ++index)
        AppStatusBar.s_StatusWheel[index] = EditorGUIUtility.IconContent("WaitSpin" + index.ToString("00"));
    }

    [RequiredByNativeCode]
    public static void StatusChanged()
    {
      if (!(bool) ((Object) AppStatusBar.s_AppStatusBar))
        return;
      AppStatusBar.s_AppStatusBar.Repaint();
    }

    private void OnInspectorUpdate()
    {
      string miniMemoryOverview = ProfilerDriver.miniMemoryOverview;
      if (!Unsupported.IsDeveloperBuild() || !(this.m_LastMiniMemoryOverview != miniMemoryOverview))
        return;
      this.m_LastMiniMemoryOverview = miniMemoryOverview;
      this.Repaint();
    }

    private void OnGUI()
    {
      ConsoleWindow.LoadIcons();
      if (AppStatusBar.background == null)
        AppStatusBar.background = (GUIStyle) "AppToolbar";
      if (EditorApplication.isPlayingOrWillChangePlaymode)
        GUI.color = (Color) HostView.kPlayModeDarken;
      if (Event.current.type == EventType.Repaint)
        AppStatusBar.background.Draw(new Rect(0.0f, 0.0f, this.position.width, this.position.height), false, false, false, false);
      bool isCompiling = EditorApplication.isCompiling;
      GUILayout.Space(2f);
      GUILayout.BeginHorizontal();
      GUILayout.Space(2f);
      string statusText = LogEntries.GetStatusText();
      if (statusText != null)
      {
        int statusMask = LogEntries.GetStatusMask();
        GUIStyle styleForErrorMode = ConsoleWindow.GetStatusStyleForErrorMode(statusMask);
        GUILayout.Label((Texture) ConsoleWindow.GetIconForErrorMode(statusMask, false), styleForErrorMode, new GUILayoutOption[0]);
        GUILayout.Space(2f);
        GUILayout.BeginVertical();
        GUILayout.Space(2f);
        if (isCompiling)
          GUILayout.Label(statusText, styleForErrorMode, new GUILayoutOption[1]
          {
            GUILayout.MaxWidth(GUIView.current.position.width - 52f)
          });
        else
          GUILayout.Label(statusText, styleForErrorMode, new GUILayoutOption[0]);
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        if (Event.current.type == EventType.MouseDown)
        {
          Event.current.Use();
          LogEntries.ClickStatusBar(Event.current.clickCount);
          GUIUtility.ExitGUI();
        }
      }
      GUILayout.EndHorizontal();
      if (Event.current.type == EventType.Repaint)
      {
        float x = this.position.width - 24f;
        if (AsyncProgressBar.isShowing)
        {
          x -= 188f;
          EditorGUI.ProgressBar(new Rect(x, 0.0f, 185f, 19f), AsyncProgressBar.progress, AsyncProgressBar.progressInfo);
        }
        if (isCompiling)
        {
          int index = (int) Mathf.Repeat(Time.realtimeSinceStartup * 10f, 11.99f);
          GUI.Label(new Rect(this.position.width - 24f, 0.0f, (float) AppStatusBar.s_StatusWheel[index].image.width, (float) AppStatusBar.s_StatusWheel[index].image.height), AppStatusBar.s_StatusWheel[index], GUIStyle.none);
        }
        if (Unsupported.IsBleedingEdgeBuild())
        {
          Color color = GUI.color;
          GUI.color = Color.yellow;
          GUI.Label(new Rect(x - 310f, 0.0f, 310f, 19f), "THIS IS AN UNTESTED BLEEDINGEDGE UNITY BUILD");
          GUI.color = color;
        }
        else if (Unsupported.IsDeveloperBuild())
        {
          GUI.Label(new Rect(x - 200f, 0.0f, 200f, 19f), this.m_LastMiniMemoryOverview, EditorStyles.progressBarText);
          EditorGUIUtility.CleanCache(this.m_LastMiniMemoryOverview);
        }
      }
      this.DoWindowDecorationEnd();
      EditorGUI.ShowRepaints();
    }
  }
}
