// Decompiled with JetBrains decompiler
// Type: UnityEditor.PragmaFixingWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor.Scripting;
using UnityEngine;

namespace UnityEditor
{
  internal class PragmaFixingWindow : EditorWindow
  {
    private ListViewState m_LV = new ListViewState();
    private static PragmaFixingWindow.Styles s_Styles;
    private string[] m_Paths;

    public PragmaFixingWindow()
    {
      this.titleContent = new GUIContent("Unity - #pragma fixing");
    }

    public static void ShowWindow(string[] paths)
    {
      PragmaFixingWindow window = EditorWindow.GetWindow<PragmaFixingWindow>(true);
      window.SetPaths(paths);
      window.ShowModal();
    }

    public void SetPaths(string[] paths)
    {
      this.m_Paths = paths;
      this.m_LV.totalRows = paths.Length;
    }

    private void OnGUI()
    {
      if (PragmaFixingWindow.s_Styles == null)
      {
        PragmaFixingWindow.s_Styles = new PragmaFixingWindow.Styles();
        this.minSize = new Vector2(450f, 300f);
        this.position = new Rect(this.position.x, this.position.y, this.minSize.x, this.minSize.y);
      }
      GUILayout.Space(10f);
      GUILayout.Label("#pragma implicit and #pragma downcast need to be added to following files\nfor backwards compatibility");
      GUILayout.Space(10f);
      GUILayout.BeginHorizontal();
      GUILayout.Space(10f);
      foreach (ListViewElement listViewElement in ListViewGUILayout.ListView(this.m_LV, PragmaFixingWindow.s_Styles.box))
      {
        if (listViewElement.row == this.m_LV.row && Event.current.type == EventType.Repaint)
          PragmaFixingWindow.s_Styles.selected.Draw(listViewElement.position, false, false, false, false);
        GUILayout.Label(this.m_Paths[listViewElement.row]);
      }
      GUILayout.Space(10f);
      GUILayout.EndHorizontal();
      GUILayout.Space(10f);
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      if (GUILayout.Button("Fix now", PragmaFixingWindow.s_Styles.button, new GUILayoutOption[0]))
      {
        this.Close();
        PragmaFixing30.FixFiles(this.m_Paths);
        GUIUtility.ExitGUI();
      }
      if (GUILayout.Button("Ignore", PragmaFixingWindow.s_Styles.button, new GUILayoutOption[0]))
      {
        this.Close();
        GUIUtility.ExitGUI();
      }
      if (GUILayout.Button("Quit", PragmaFixingWindow.s_Styles.button, new GUILayoutOption[0]))
      {
        EditorApplication.Exit(0);
        GUIUtility.ExitGUI();
      }
      GUILayout.Space(10f);
      GUILayout.EndHorizontal();
      GUILayout.Space(10f);
    }

    private class Styles
    {
      public GUIStyle selected = (GUIStyle) "ServerUpdateChangesetOn";
      public GUIStyle box = (GUIStyle) "OL Box";
      public GUIStyle button = (GUIStyle) "LargeButton";
    }
  }
}
