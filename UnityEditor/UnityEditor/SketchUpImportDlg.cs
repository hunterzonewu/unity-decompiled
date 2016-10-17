// Decompiled with JetBrains decompiler
// Type: UnityEditor.SketchUpImportDlg
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal class SketchUpImportDlg : EditorWindow
  {
    private readonly Vector2 m_WindowMinSize = new Vector2(350f, 350f);
    private const float kHeaderHeight = 25f;
    private const float kBottomHeight = 30f;
    private TreeView m_TreeView;
    private SketchUpTreeViewGUI m_ImportGUI;
    private SketchUpDataSource m_DataSource;
    private int[] m_Selection;
    private WeakReference m_ModelEditor;
    private TreeViewState m_TreeViewState;

    private bool isModal { get; set; }

    public void Init(SketchUpNodeInfo[] nodes, SketchUpImporterModelEditor suModelEditor)
    {
      this.titleContent = SketchUpImportDlg.Styles.styles.windowTitle;
      this.minSize = this.m_WindowMinSize;
      this.position = new Rect(this.position.x, this.position.y, this.minSize.x, this.minSize.y);
      this.m_TreeViewState = new TreeViewState();
      this.m_TreeView = new TreeView((EditorWindow) this, this.m_TreeViewState);
      this.m_ImportGUI = new SketchUpTreeViewGUI(this.m_TreeView);
      this.m_DataSource = new SketchUpDataSource(this.m_TreeView, nodes);
      this.m_TreeView.Init(this.position, (ITreeViewDataSource) this.m_DataSource, (ITreeViewGUI) this.m_ImportGUI, (ITreeViewDragging) null);
      this.m_TreeView.selectionChangedCallback += new System.Action<int[]>(this.OnTreeSelectionChanged);
      this.m_ModelEditor = new WeakReference((object) suModelEditor);
      this.isModal = false;
    }

    internal static void Launch(SketchUpNodeInfo[] nodes, SketchUpImporterModelEditor suModelEditor)
    {
      SketchUpImportDlg windowDontShow = EditorWindow.GetWindowDontShow<SketchUpImportDlg>();
      windowDontShow.Init(nodes, suModelEditor);
      windowDontShow.ShowAuxWindow();
    }

    internal static int[] LaunchAsModal(SketchUpNodeInfo[] nodes)
    {
      SketchUpImportDlg windowDontShow = EditorWindow.GetWindowDontShow<SketchUpImportDlg>();
      windowDontShow.Init(nodes, (SketchUpImporterModelEditor) null);
      windowDontShow.isModal = true;
      windowDontShow.ShowModal();
      return windowDontShow.m_DataSource.FetchEnableNodes();
    }

    private void HandleKeyboardEvents()
    {
      Event current = Event.current;
      if (current.type != EventType.KeyDown || current.keyCode != KeyCode.Space && current.keyCode != KeyCode.Return && current.keyCode != KeyCode.KeypadEnter || (this.m_Selection == null || this.m_Selection.Length <= 0))
        return;
      SketchUpNode node = this.m_TreeView.FindNode(this.m_Selection[0]) as SketchUpNode;
      if (node == null || node == this.m_DataSource.root)
        return;
      node.Enabled = !node.Enabled;
      current.Use();
      this.Repaint();
    }

    public void OnTreeSelectionChanged(int[] selection)
    {
      this.m_Selection = selection;
    }

    private void OnGUI()
    {
      Rect rect = new Rect(0.0f, 0.0f, this.position.width, this.position.height);
      int controlId = GUIUtility.GetControlID(FocusType.Keyboard, rect);
      Rect position = new Rect(0.0f, 0.0f, this.position.width, 25f);
      GUI.Label(position, string.Empty, SketchUpImportDlg.Styles.styles.headerStyle);
      GUI.Label(new Rect(10f, 2f, this.position.width, 25f), SketchUpImportDlg.Styles.styles.nodesLabel);
      Rect screenRect = new Rect(rect.x, rect.yMax - 30f, rect.width, 30f);
      GUILayout.BeginArea(screenRect);
      GUILayout.Box(string.Empty, SketchUpImportDlg.Styles.styles.boxBackground, GUILayout.ExpandWidth(true), GUILayout.Height(1f));
      GUILayout.Space(2f);
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      bool flag = false;
      if (this.isModal)
      {
        if (GUILayout.Button(SketchUpImportDlg.Styles.styles.okButton))
          flag = true;
      }
      else if (GUILayout.Button(SketchUpImportDlg.Styles.styles.cancelButton))
        flag = true;
      else if (GUILayout.Button(SketchUpImportDlg.Styles.styles.okButton))
      {
        flag = true;
        if (this.m_ModelEditor.IsAlive)
          (this.m_ModelEditor.Target as SketchUpImporterModelEditor).SetSelectedNodes(this.m_DataSource.FetchEnableNodes());
      }
      GUILayout.Space(10f);
      GUILayout.EndHorizontal();
      GUILayout.EndArea();
      rect.y = 18f;
      rect.height -= (float) ((double) position.height + (double) screenRect.height - 7.0);
      this.m_TreeView.OnEvent();
      this.m_TreeView.OnGUI(rect, controlId);
      this.HandleKeyboardEvents();
      if (!flag)
        return;
      this.Close();
    }

    private void OnLostFocus()
    {
      if (this.isModal)
        return;
      this.Close();
    }

    internal class Styles
    {
      public readonly GUIStyle boxBackground = (GUIStyle) "OL Box";
      public readonly GUIContent okButton = EditorGUIUtility.TextContent("OK");
      public readonly GUIContent cancelButton = EditorGUIUtility.TextContent("Cancel");
      public readonly GUIContent nodesLabel = EditorGUIUtility.TextContent("Select the SketchUp nodes to import|Nodes in the file hierarchy");
      public readonly GUIContent windowTitle = EditorGUIUtility.TextContent("SketchUp Node Selection Dialog|SketchUp Node Selection Dialog");
      public readonly float buttonWidth;
      public readonly GUIStyle headerStyle;
      public readonly GUIStyle toggleStyle;
      private static SketchUpImportDlg.Styles s_Styles;

      public static SketchUpImportDlg.Styles styles
      {
        get
        {
          return SketchUpImportDlg.Styles.s_Styles ?? (SketchUpImportDlg.Styles.s_Styles = new SketchUpImportDlg.Styles());
        }
      }

      public Styles()
      {
        this.buttonWidth = 32f;
        this.headerStyle = new GUIStyle(EditorStyles.toolbarButton);
        this.headerStyle.padding.left = 4;
        this.headerStyle.alignment = TextAnchor.MiddleLeft;
        this.toggleStyle = new GUIStyle(EditorStyles.toggle);
        this.toggleStyle.padding.left = 8;
        this.toggleStyle.alignment = TextAnchor.MiddleCenter;
      }
    }
  }
}
