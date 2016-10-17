// Decompiled with JetBrains decompiler
// Type: UnityEditor.GUIViewDebuggerWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class GUIViewDebuggerWindow : EditorWindow
  {
    private bool m_ShowOverlay = true;
    [NonSerialized]
    private readonly ListViewState m_ListViewState = new ListViewState();
    private Vector2 m_InstructionDetailsScrollPos = new Vector2();
    private Vector2 m_StacktraceScrollPos = new Vector2();
    private readonly SplitterState m_InstructionListDetailSplitter = new SplitterState(new float[2]{ 30f, 70f }, new int[2]{ 32, 32 }, (int[]) null);
    private readonly SplitterState m_InstructionDetailStacktraceSplitter = new SplitterState(new float[2]{ 80f, 20f }, new int[2]{ 100, 100 }, (int[]) null);
    private GUIView m_Inspected;
    [NonSerialized]
    private GUIViewDebuggerWindow.GUIInstruction m_Instruction;
    [NonSerialized]
    private int m_LastSelectedRow;
    [NonSerialized]
    private Vector2 m_PointToInspect;
    [NonSerialized]
    private bool m_QueuedPointInspection;
    [NonSerialized]
    private GUIViewDebuggerWindow.CachedInstructionInfo m_CachedinstructionInfo;
    private static GUIViewDebuggerWindow.Styles s_Styles;
    private InstructionOverlayWindow m_InstructionOverlayWindow;
    private static GUIViewDebuggerWindow s_ActiveInspector;

    private static void Init()
    {
      if ((UnityEngine.Object) GUIViewDebuggerWindow.s_ActiveInspector == (UnityEngine.Object) null)
        GUIViewDebuggerWindow.s_ActiveInspector = (GUIViewDebuggerWindow) EditorWindow.GetWindow(typeof (GUIViewDebuggerWindow));
      GUIViewDebuggerWindow.s_ActiveInspector.Show();
    }

    private static void InspectPoint(Vector2 point)
    {
      Debug.Log((object) ("Inspecting " + (object) point));
      GUIViewDebuggerWindow.s_ActiveInspector.InspectPointAt(point);
    }

    private void OnEnable()
    {
      this.titleContent = new GUIContent("GUI Inspector");
    }

    private void OnGUI()
    {
      this.InitializeStylesIfNeeded();
      this.DoToolbar();
      this.ShowDrawInstructions();
    }

    private void InitializeStylesIfNeeded()
    {
      if (GUIViewDebuggerWindow.s_Styles != null)
        return;
      GUIViewDebuggerWindow.s_Styles = new GUIViewDebuggerWindow.Styles();
    }

    private static void OnInspectedViewChanged()
    {
      if ((UnityEngine.Object) GUIViewDebuggerWindow.s_ActiveInspector == (UnityEngine.Object) null)
        return;
      GUIViewDebuggerWindow.s_ActiveInspector.Repaint();
    }

    private void DoToolbar()
    {
      GUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
      this.DoWindowPopup();
      this.DoInstructionOverlayToggle();
      GUILayout.EndHorizontal();
    }

    private void DoWindowPopup()
    {
      string t = "<Please Select>";
      if ((UnityEngine.Object) this.m_Inspected != (UnityEngine.Object) null)
        t = GUIViewDebuggerWindow.GetViewName(this.m_Inspected);
      GUILayout.Label("Inspected Window: ", new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(false)
      });
      Rect rect = GUILayoutUtility.GetRect(GUIContent.Temp(t), EditorStyles.toolbarDropDown, new GUILayoutOption[1]{ GUILayout.ExpandWidth(true) });
      if (!GUI.Button(rect, GUIContent.Temp(t), EditorStyles.toolbarDropDown))
        return;
      List<GUIView> views = new List<GUIView>();
      GUIViewDebuggerHelper.GetViews(views);
      List<GUIContent> guiContentList = new List<GUIContent>(views.Count + 1);
      guiContentList.Add(new GUIContent("None"));
      int selected = 0;
      List<GUIView> guiViewList = new List<GUIView>(views.Count + 1);
      for (int index = 0; index < views.Count; ++index)
      {
        GUIView view = views[index];
        if (this.CanInspectView(view))
        {
          GUIContent guiContent = new GUIContent(guiContentList.Count.ToString() + ". " + GUIViewDebuggerWindow.GetViewName(view));
          guiContentList.Add(guiContent);
          guiViewList.Add(view);
          if ((UnityEngine.Object) view == (UnityEngine.Object) this.m_Inspected)
            selected = guiViewList.Count;
        }
      }
      EditorUtility.DisplayCustomMenu(rect, guiContentList.ToArray(), selected, new EditorUtility.SelectMenuItemFunction(this.OnWindowSelected), (object) guiViewList);
    }

    private void DoInstructionOverlayToggle()
    {
      EditorGUI.BeginChangeCheck();
      this.m_ShowOverlay = GUILayout.Toggle(this.m_ShowOverlay, GUIContent.Temp("Show overlay"), EditorStyles.toolbarButton, new GUILayoutOption[0]);
      if (!EditorGUI.EndChangeCheck())
        return;
      this.OnShowOverlayChanged();
    }

    private void OnShowOverlayChanged()
    {
      if (!this.m_ShowOverlay)
      {
        if (!((UnityEngine.Object) this.m_InstructionOverlayWindow != (UnityEngine.Object) null))
          return;
        this.m_InstructionOverlayWindow.Close();
      }
      else
      {
        if (!((UnityEngine.Object) this.m_Inspected != (UnityEngine.Object) null) || this.m_Instruction == null)
          return;
        this.HighlightInstruction(this.m_Inspected, this.m_Instruction.rect, this.m_Instruction.usedGUIStyle);
      }
    }

    private bool CanInspectView(GUIView view)
    {
      EditorWindow editorWindow = GUIViewDebuggerWindow.GetEditorWindow(view);
      return (UnityEngine.Object) editorWindow == (UnityEngine.Object) null || !((UnityEngine.Object) editorWindow == (UnityEngine.Object) this) && !((UnityEngine.Object) editorWindow == (UnityEngine.Object) this.m_InstructionOverlayWindow);
    }

    private void OnWindowSelected(object userdata, string[] options, int selected)
    {
      --selected;
      GUIView guiView = selected < 0 ? (GUIView) null : ((List<GUIView>) userdata)[selected];
      if ((UnityEngine.Object) this.m_Inspected != (UnityEngine.Object) guiView)
      {
        if ((UnityEngine.Object) this.m_InstructionOverlayWindow != (UnityEngine.Object) null)
          this.m_InstructionOverlayWindow.Close();
        this.m_Inspected = guiView;
        if ((UnityEngine.Object) this.m_Inspected != (UnityEngine.Object) null)
        {
          GUIViewDebuggerHelper.DebugWindow(this.m_Inspected);
          this.m_Inspected.Repaint();
        }
        this.m_ListViewState.row = -1;
        this.m_ListViewState.selectionChanged = true;
        this.m_Instruction = (GUIViewDebuggerWindow.GUIInstruction) null;
      }
      this.Repaint();
    }

    private static EditorWindow GetEditorWindow(GUIView view)
    {
      HostView hostView = view as HostView;
      if ((UnityEngine.Object) hostView != (UnityEngine.Object) null)
        return hostView.actualView;
      return (EditorWindow) null;
    }

    private static string GetViewName(GUIView view)
    {
      EditorWindow editorWindow = GUIViewDebuggerWindow.GetEditorWindow(view);
      if ((UnityEngine.Object) editorWindow != (UnityEngine.Object) null)
        return editorWindow.titleContent.text;
      return view.GetType().Name;
    }

    private void ShowDrawInstructions()
    {
      if ((UnityEngine.Object) this.m_Inspected == (UnityEngine.Object) null)
        return;
      this.m_ListViewState.totalRows = GUIViewDebuggerHelper.GetInstructionCount();
      if (this.m_QueuedPointInspection)
      {
        this.m_ListViewState.row = this.FindInstructionUnderPoint(this.m_PointToInspect);
        this.m_ListViewState.selectionChanged = true;
        this.m_QueuedPointInspection = false;
        this.m_Instruction.Reset();
      }
      SplitterGUILayout.BeginHorizontalSplit(this.m_InstructionListDetailSplitter);
      this.DrawInstructionList();
      EditorGUILayout.BeginVertical();
      if (this.m_ListViewState.selectionChanged)
        this.OnSelectedInstructionChanged();
      this.DrawSelectedInstructionDetails();
      EditorGUILayout.EndVertical();
      SplitterGUILayout.EndHorizontalSplit();
    }

    private void OnSelectedInstructionChanged()
    {
      if (this.m_ListViewState.row >= 0)
      {
        if (this.m_Instruction == null)
          this.m_Instruction = new GUIViewDebuggerWindow.GUIInstruction();
        if (this.m_CachedinstructionInfo == null)
          this.m_CachedinstructionInfo = new GUIViewDebuggerWindow.CachedInstructionInfo();
        this.m_Instruction.rect = GUIViewDebuggerHelper.GetRectFromInstruction(this.m_ListViewState.row);
        this.m_Instruction.usedGUIStyle = GUIViewDebuggerHelper.GetStyleFromInstruction(this.m_ListViewState.row);
        this.m_Instruction.usedGUIContent = GUIViewDebuggerHelper.GetContentFromInstruction(this.m_ListViewState.row);
        this.m_Instruction.stackframes = GUIViewDebuggerHelper.GetManagedStackTrace(this.m_ListViewState.row);
        this.m_CachedinstructionInfo.styleContainer.inspectedStyle = this.m_Instruction.usedGUIStyle;
        this.m_CachedinstructionInfo.styleContainerSerializedObject = (SerializedObject) null;
        this.m_CachedinstructionInfo.styleSerializedProperty = (SerializedProperty) null;
        this.GetSelectedStyleProperty(out this.m_CachedinstructionInfo.styleContainerSerializedObject, out this.m_CachedinstructionInfo.styleSerializedProperty);
        this.HighlightInstruction(this.m_Inspected, this.m_Instruction.rect, this.m_Instruction.usedGUIStyle);
      }
      else
      {
        this.m_Instruction = (GUIViewDebuggerWindow.GUIInstruction) null;
        this.m_CachedinstructionInfo = (GUIViewDebuggerWindow.CachedInstructionInfo) null;
        if (!((UnityEngine.Object) this.m_InstructionOverlayWindow != (UnityEngine.Object) null))
          return;
        this.m_InstructionOverlayWindow.Close();
      }
    }

    private void DrawInstructionList()
    {
      Event current = Event.current;
      EditorGUILayout.BeginVertical(GUIViewDebuggerWindow.s_Styles.listBackgroundStyle, new GUILayoutOption[0]);
      GUILayout.Label("Instructions");
      int controlId = GUIUtility.GetControlID(FocusType.Keyboard);
      foreach (ListViewElement el in ListViewGUI.ListView(this.m_ListViewState, GUIViewDebuggerWindow.s_Styles.listBackgroundStyle, new GUILayoutOption[0]))
      {
        if (current.type == EventType.MouseDown && current.button == 0 && (el.position.Contains(current.mousePosition) && current.clickCount == 2))
          this.ShowInstructionInExternalEditor(el.row);
        if (current.type == EventType.Repaint)
        {
          GUIContent content = GUIContent.Temp(this.GetInstructionName(el));
          GUIViewDebuggerWindow.s_Styles.listItemBackground.Draw(el.position, false, false, this.m_ListViewState.row == el.row, false);
          GUIViewDebuggerWindow.s_Styles.listItem.Draw(el.position, content, controlId, this.m_ListViewState.row == el.row);
        }
      }
      EditorGUILayout.EndVertical();
    }

    private void ShowInstructionInExternalEditor(int row)
    {
      StackFrame[] managedStackTrace = GUIViewDebuggerHelper.GetManagedStackTrace(row);
      int interestingFrameIndex = this.GetInterestingFrameIndex(managedStackTrace);
      StackFrame stackFrame = managedStackTrace[interestingFrameIndex];
      InternalEditorUtility.OpenFileAtLineExternal(stackFrame.sourceFile, (int) stackFrame.lineNumber);
    }

    private string GetInstructionName(ListViewElement el)
    {
      int row = el.row;
      string instructionListName = this.GetInstructionListName(GUIViewDebuggerHelper.GetManagedStackTrace(row));
      return string.Format("{0}. {1}", (object) row, (object) instructionListName);
    }

    private string GetInstructionListName(StackFrame[] stacktrace)
    {
      int interestingFrameIndex = this.GetInterestingFrameIndex(stacktrace);
      if (interestingFrameIndex > 0)
        --interestingFrameIndex;
      return stacktrace[interestingFrameIndex].methodName;
    }

    private int GetInterestingFrameIndex(StackFrame[] stacktrace)
    {
      string dataPath = Application.dataPath;
      int num = -1;
      for (int index = 0; index < stacktrace.Length; ++index)
      {
        StackFrame stackFrame = stacktrace[index];
        if (!string.IsNullOrEmpty(stackFrame.sourceFile) && !stackFrame.signature.StartsWith("UnityEngine.GUI") && !stackFrame.signature.StartsWith("UnityEditor.EditorGUI"))
        {
          if (num == -1)
            num = index;
          if (stackFrame.sourceFile.StartsWith(dataPath))
            return index;
        }
      }
      if (num != -1)
        return num;
      return stacktrace.Length - 1;
    }

    private void DrawSelectedInstructionDetails()
    {
      if (this.m_Instruction == null)
      {
        EditorGUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Select a Instruction on the left to see details", GUIViewDebuggerWindow.s_Styles.centeredText, new GUILayoutOption[0]);
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndVertical();
      }
      else
      {
        SplitterGUILayout.BeginVerticalSplit(this.m_InstructionDetailStacktraceSplitter);
        this.m_InstructionDetailsScrollPos = EditorGUILayout.BeginScrollView(this.m_InstructionDetailsScrollPos, GUIViewDebuggerWindow.s_Styles.boxStyle, new GUILayoutOption[0]);
        EditorGUI.BeginDisabledGroup(true);
        this.DrawInspectedRect();
        EditorGUI.EndDisabledGroup();
        this.DrawInspectedStyle();
        EditorGUI.BeginDisabledGroup(true);
        this.DrawInspectedGUIContent();
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndScrollView();
        this.DrawInspectedStacktrace();
        SplitterGUILayout.EndVerticalSplit();
      }
    }

    private void DrawInspectedRect()
    {
      EditorGUILayout.RectField(GUIContent.Temp("Rect"), this.m_Instruction.rect, new GUILayoutOption[0]);
    }

    private void DrawInspectedGUIContent()
    {
      GUILayout.Label(GUIContent.Temp("GUIContent"));
      ++EditorGUI.indentLevel;
      EditorGUILayout.TextField(this.m_Instruction.usedGUIContent.text);
      EditorGUILayout.ObjectField((UnityEngine.Object) this.m_Instruction.usedGUIContent.image, typeof (Texture2D), false, new GUILayoutOption[0]);
      --EditorGUI.indentLevel;
    }

    private void DrawInspectedStyle()
    {
      EditorGUI.BeginChangeCheck();
      EditorGUILayout.PropertyField(this.m_CachedinstructionInfo.styleSerializedProperty, GUIContent.Temp("Style"), true, new GUILayoutOption[0]);
      if (!EditorGUI.EndChangeCheck())
        return;
      this.m_CachedinstructionInfo.styleContainerSerializedObject.ApplyModifiedPropertiesWithoutUndo();
      this.m_Inspected.Repaint();
    }

    private void GetSelectedStyleProperty(out SerializedObject serializedObject, out SerializedProperty styleProperty)
    {
      GUISkin guiSkin = (GUISkin) null;
      GUISkin current = GUISkin.current;
      GUIStyle style = current.FindStyle(this.m_Instruction.usedGUIStyle.name);
      if (style != null && style == this.m_Instruction.usedGUIStyle)
        guiSkin = current;
      styleProperty = (SerializedProperty) null;
      if ((UnityEngine.Object) guiSkin != (UnityEngine.Object) null)
      {
        serializedObject = new SerializedObject((UnityEngine.Object) guiSkin);
        SerializedProperty iterator = serializedObject.GetIterator();
        bool enterChildren = true;
        while (iterator.NextVisible(enterChildren))
        {
          if (iterator.type == "GUIStyle")
          {
            enterChildren = false;
            if (iterator.FindPropertyRelative("m_Name").stringValue == this.m_Instruction.usedGUIStyle.name)
            {
              styleProperty = iterator;
              return;
            }
          }
          else
            enterChildren = true;
        }
        Debug.Log((object) string.Format("Showing editable Style from GUISkin: {0}, IsPersistant: {1}", (object) guiSkin.name, (object) EditorUtility.IsPersistent((UnityEngine.Object) guiSkin)));
      }
      serializedObject = new SerializedObject((UnityEngine.Object) this.m_CachedinstructionInfo.styleContainer);
      styleProperty = serializedObject.FindProperty("inspectedStyle");
    }

    private void DrawInspectedStacktrace()
    {
      this.m_StacktraceScrollPos = EditorGUILayout.BeginScrollView(this.m_StacktraceScrollPos, GUIViewDebuggerWindow.s_Styles.stacktraceBackground, GUILayout.ExpandHeight(false));
      if (this.m_Instruction.stackframes != null)
      {
        foreach (StackFrame stackframe in this.m_Instruction.stackframes)
        {
          if (!string.IsNullOrEmpty(stackframe.sourceFile))
            GUILayout.Label(string.Format("{0} [{1}:{2}]", (object) stackframe.signature, (object) stackframe.sourceFile, (object) stackframe.lineNumber), GUIViewDebuggerWindow.s_Styles.stackframeStyle, new GUILayoutOption[0]);
        }
      }
      EditorGUILayout.EndScrollView();
    }

    private void HighlightInstruction(GUIView view, Rect instructionRect, GUIStyle style)
    {
      if (this.m_ListViewState.row < 0 || !this.m_ShowOverlay)
        return;
      if ((UnityEngine.Object) this.m_InstructionOverlayWindow == (UnityEngine.Object) null)
        this.m_InstructionOverlayWindow = ScriptableObject.CreateInstance<InstructionOverlayWindow>();
      this.m_InstructionOverlayWindow.Show(view, instructionRect, style);
      this.Focus();
    }

    private void InspectPointAt(Vector2 point)
    {
      this.m_PointToInspect = point;
      this.m_QueuedPointInspection = true;
      this.m_Inspected.Repaint();
      this.Repaint();
    }

    private int FindInstructionUnderPoint(Vector2 point)
    {
      int instructionCount = GUIViewDebuggerHelper.GetInstructionCount();
      for (int instructionIndex = 0; instructionIndex < instructionCount; ++instructionIndex)
      {
        if (GUIViewDebuggerHelper.GetRectFromInstruction(instructionIndex).Contains(point))
          return instructionIndex;
      }
      return -1;
    }

    private void OnDisable()
    {
      if ((UnityEngine.Object) this.m_Inspected != (UnityEngine.Object) null)
        GUIViewDebuggerHelper.DebugWindow(this.m_Inspected);
      if (!((UnityEngine.Object) this.m_InstructionOverlayWindow != (UnityEngine.Object) null))
        return;
      this.m_InstructionOverlayWindow.Close();
    }

    private class Styles
    {
      public readonly GUIStyle listItem = new GUIStyle((GUIStyle) "PR Label");
      public readonly GUIStyle listItemBackground = new GUIStyle((GUIStyle) "CN EntryBackOdd");
      public readonly GUIStyle listBackgroundStyle = new GUIStyle((GUIStyle) "CN Box");
      public readonly GUIStyle boxStyle = new GUIStyle((GUIStyle) "CN Box");
      public readonly GUIStyle stackframeStyle = new GUIStyle(EditorStyles.label);
      public readonly GUIStyle stacktraceBackground = new GUIStyle((GUIStyle) "CN Box");
      public readonly GUIStyle centeredText = new GUIStyle((GUIStyle) "PR Label");

      public Styles()
      {
        this.stackframeStyle.margin = new RectOffset(0, 0, 0, 0);
        this.stackframeStyle.padding = new RectOffset(0, 0, 0, 0);
        this.stacktraceBackground.padding = new RectOffset(5, 5, 5, 5);
        this.centeredText.alignment = TextAnchor.MiddleCenter;
        this.centeredText.stretchHeight = true;
        this.centeredText.stretchWidth = true;
      }
    }

    private class GUIInstruction
    {
      public GUIStyle usedGUIStyle = GUIStyle.none;
      public GUIContent usedGUIContent = GUIContent.none;
      public Rect rect;
      public StackFrame[] stackframes;

      public void Reset()
      {
        this.rect = new Rect();
        this.usedGUIStyle = GUIStyle.none;
        this.usedGUIContent = GUIContent.none;
      }
    }

    [Serializable]
    private class CachedInstructionInfo
    {
      public SerializedObject styleContainerSerializedObject;
      public SerializedProperty styleSerializedProperty;
      public readonly GUIStyleHolder styleContainer;

      public CachedInstructionInfo()
      {
        this.styleContainer = ScriptableObject.CreateInstance<GUIStyleHolder>();
      }
    }
  }
}
