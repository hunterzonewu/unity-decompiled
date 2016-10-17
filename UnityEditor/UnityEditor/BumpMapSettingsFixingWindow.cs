// Decompiled with JetBrains decompiler
// Type: UnityEditor.BumpMapSettingsFixingWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class BumpMapSettingsFixingWindow : EditorWindow
  {
    private ListViewState m_LV = new ListViewState();
    private static BumpMapSettingsFixingWindow.Styles s_Styles;
    private string[] m_Paths;

    public BumpMapSettingsFixingWindow()
    {
      this.titleContent = new GUIContent("NormalMap settings");
    }

    public static void ShowWindow(string[] paths)
    {
      BumpMapSettingsFixingWindow window = EditorWindow.GetWindow<BumpMapSettingsFixingWindow>(true);
      window.SetPaths(paths);
      window.ShowUtility();
    }

    public void SetPaths(string[] paths)
    {
      this.m_Paths = paths;
      this.m_LV.totalRows = paths.Length;
    }

    private void OnGUI()
    {
      if (BumpMapSettingsFixingWindow.s_Styles == null)
      {
        BumpMapSettingsFixingWindow.s_Styles = new BumpMapSettingsFixingWindow.Styles();
        this.minSize = new Vector2(400f, 300f);
        this.position = new Rect(this.position.x, this.position.y, this.minSize.x, this.minSize.y);
      }
      GUILayout.Space(5f);
      GUILayout.Label(BumpMapSettingsFixingWindow.s_Styles.overviewText);
      GUILayout.Space(10f);
      GUILayout.BeginHorizontal();
      GUILayout.Space(10f);
      foreach (ListViewElement listViewElement in ListViewGUILayout.ListView(this.m_LV, BumpMapSettingsFixingWindow.s_Styles.box))
      {
        if (listViewElement.row == this.m_LV.row && Event.current.type == EventType.Repaint)
          BumpMapSettingsFixingWindow.s_Styles.selected.Draw(listViewElement.position, false, false, false, false);
        GUILayout.Label(this.m_Paths[listViewElement.row]);
      }
      GUILayout.Space(10f);
      GUILayout.EndHorizontal();
      GUILayout.Space(10f);
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      if (GUILayout.Button("Fix now", BumpMapSettingsFixingWindow.s_Styles.button, new GUILayoutOption[0]))
      {
        InternalEditorUtility.BumpMapSettingsFixingWindowReportResult(1);
        this.Close();
      }
      if (GUILayout.Button("Ignore", BumpMapSettingsFixingWindow.s_Styles.button, new GUILayoutOption[0]))
      {
        InternalEditorUtility.BumpMapSettingsFixingWindowReportResult(0);
        this.Close();
      }
      GUILayout.Space(10f);
      GUILayout.EndHorizontal();
      GUILayout.Space(10f);
    }

    private void OnDestroy()
    {
      InternalEditorUtility.BumpMapSettingsFixingWindowReportResult(0);
    }

    private class Styles
    {
      public GUIStyle selected = (GUIStyle) "ServerUpdateChangesetOn";
      public GUIStyle box = (GUIStyle) "OL Box";
      public GUIStyle button = (GUIStyle) "LargeButton";
      public GUIContent overviewText = EditorGUIUtility.TextContent("A Material is using the texture as a normal map.\nThe texture must be marked as a normal map in the import settings.");
    }
  }
}
