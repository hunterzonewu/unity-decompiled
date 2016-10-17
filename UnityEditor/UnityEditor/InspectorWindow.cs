// Decompiled with JetBrains decompiler
// Type: UnityEditor.InspectorWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor.VersionControl;
using UnityEditorInternal;
using UnityEditorInternal.VersionControl;
using UnityEngine;

namespace UnityEditor
{
  [EditorWindowTitle(title = "Inspector", useTypeNameAsIconName = true)]
  internal class InspectorWindow : EditorWindow, IHasCustomMenu
  {
    private static readonly List<InspectorWindow> m_AllInspectors = new List<InspectorWindow>();
    [SerializeField]
    private PreviewResizer m_PreviewResizer = new PreviewResizer();
    private LabelGUI m_LabelGUI = new LabelGUI();
    private AssetBundleNameGUI m_AssetBundleNameGUI = new AssetBundleNameGUI();
    private bool m_InvalidateGUIBlockCache = true;
    private const float kBottomToolbarHeight = 17f;
    internal const int kInspectorPaddingLeft = 14;
    internal const int kInspectorPaddingRight = 4;
    private const long delayRepaintWhilePlayingAnimation = 150;
    public Vector2 m_ScrollPosition;
    public InspectorMode m_InspectorMode;
    private static bool s_AllOptimizedGUIBlocksNeedsRebuild;
    private long s_LastUpdateWhilePlayingAnimation;
    private bool m_ResetKeyboardControl;
    protected ActiveEditorTracker m_Tracker;
    private Editor m_LastInteractedEditor;
    private bool m_IsOpenForEdit;
    private static InspectorWindow.Styles s_Styles;
    [SerializeField]
    private PreviewWindow m_PreviewWindow;
    private TypeSelectionList m_TypeSelectionList;
    private double m_lastRenderedTime;
    private List<IPreviewable> m_Previews;
    private IPreviewable m_SelectedPreview;
    public static InspectorWindow s_CurrentInspectorWindow;

    internal static InspectorWindow.Styles styles
    {
      get
      {
        return InspectorWindow.s_Styles ?? (InspectorWindow.s_Styles = new InspectorWindow.Styles());
      }
    }

    public bool isLocked
    {
      get
      {
        this.CreateTracker();
        return this.m_Tracker.isLocked;
      }
      set
      {
        this.CreateTracker();
        this.m_Tracker.isLocked = value;
      }
    }

    private void Awake()
    {
      if (InspectorWindow.m_AllInspectors.Contains(this))
        return;
      InspectorWindow.m_AllInspectors.Add(this);
    }

    private void OnDestroy()
    {
      if ((UnityEngine.Object) this.m_PreviewWindow != (UnityEngine.Object) null)
        this.m_PreviewWindow.Close();
      if (this.m_Tracker == null || this.m_Tracker.Equals((object) ActiveEditorTracker.sharedTracker))
        return;
      this.m_Tracker.Destroy();
    }

    protected virtual void OnEnable()
    {
      this.RefreshTitle();
      this.minSize = new Vector2(275f, 50f);
      if (!InspectorWindow.m_AllInspectors.Contains(this))
        InspectorWindow.m_AllInspectors.Add(this);
      this.m_PreviewResizer.Init("InspectorPreview");
      this.m_LabelGUI.OnEnable();
    }

    protected virtual void OnDisable()
    {
      InspectorWindow.m_AllInspectors.Remove(this);
    }

    private void OnLostFocus()
    {
      EditorGUI.EndEditingActiveTextField();
      this.m_LabelGUI.OnLostFocus();
    }

    internal static void RepaintAllInspectors()
    {
      using (List<InspectorWindow>.Enumerator enumerator = InspectorWindow.m_AllInspectors.GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.Repaint();
      }
    }

    internal static List<InspectorWindow> GetInspectors()
    {
      return InspectorWindow.m_AllInspectors;
    }

    private void OnSelectionChange()
    {
      this.m_Previews = (List<IPreviewable>) null;
      this.m_SelectedPreview = (IPreviewable) null;
      this.m_TypeSelectionList = (TypeSelectionList) null;
      this.m_Parent.ClearKeyboardControl();
      ScriptAttributeUtility.ClearGlobalCache();
      this.Repaint();
    }

    public static InspectorWindow[] GetAllInspectorWindows()
    {
      return InspectorWindow.m_AllInspectors.ToArray();
    }

    private void OnInspectorUpdate()
    {
      if (this.m_Tracker != null)
      {
        this.m_Tracker.VerifyModifiedMonoBehaviours();
        if (!this.m_Tracker.isDirty || !this.ReadyToRepaint())
          return;
      }
      this.Repaint();
    }

    public virtual void AddItemsToMenu(GenericMenu menu)
    {
      menu.AddItem(new GUIContent("Normal"), this.m_InspectorMode == InspectorMode.Normal, new GenericMenu.MenuFunction(this.SetNormal));
      menu.AddItem(new GUIContent("Debug"), this.m_InspectorMode == InspectorMode.Debug, new GenericMenu.MenuFunction(this.SetDebug));
      if (Unsupported.IsDeveloperBuild())
        menu.AddItem(new GUIContent("Debug-Internal"), this.m_InspectorMode == InspectorMode.DebugInternal, new GenericMenu.MenuFunction(this.SetDebugInternal));
      menu.AddSeparator(string.Empty);
      menu.AddItem(new GUIContent("Lock"), this.m_Tracker != null && this.isLocked, new GenericMenu.MenuFunction(this.FlipLocked));
    }

    private void RefreshTitle()
    {
      string icon = "UnityEditor.InspectorWindow";
      if (this.m_InspectorMode == InspectorMode.Normal)
        this.titleContent = EditorGUIUtility.TextContentWithIcon("Inspector", icon);
      else
        this.titleContent = EditorGUIUtility.TextContentWithIcon("Debug", icon);
    }

    private void SetMode(InspectorMode mode)
    {
      this.m_InspectorMode = mode;
      this.RefreshTitle();
      this.CreateTracker();
      this.m_Tracker.inspectorMode = mode;
      this.m_ResetKeyboardControl = true;
    }

    private void SetDebug()
    {
      this.SetMode(InspectorMode.Debug);
    }

    private void SetNormal()
    {
      this.SetMode(InspectorMode.Normal);
    }

    private void SetDebugInternal()
    {
      this.SetMode(InspectorMode.DebugInternal);
    }

    private void FlipLocked()
    {
      this.isLocked = !this.isLocked;
    }

    private static void DoInspectorDragAndDrop(Rect rect, UnityEngine.Object[] targets)
    {
      if (!InspectorWindow.Dragging(rect))
        return;
      DragAndDrop.visualMode = InternalEditorUtility.InspectorWindowDrag(targets, Event.current.type == EventType.DragPerform);
      if (Event.current.type != EventType.DragPerform)
        return;
      DragAndDrop.AcceptDrag();
    }

    private static bool Dragging(Rect rect)
    {
      if (Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragPerform)
        return rect.Contains(Event.current.mousePosition);
      return false;
    }

    public ActiveEditorTracker GetTracker()
    {
      this.CreateTracker();
      return this.m_Tracker;
    }

    protected virtual void CreateTracker()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      InspectorWindow.\u003CCreateTracker\u003Ec__AnonStorey91 trackerCAnonStorey91 = new InspectorWindow.\u003CCreateTracker\u003Ec__AnonStorey91();
      if (this.m_Tracker != null)
      {
        this.m_Tracker.inspectorMode = this.m_InspectorMode;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        trackerCAnonStorey91.sharedTracker = ActiveEditorTracker.sharedTracker;
        // ISSUE: reference to a compiler-generated method
        this.m_Tracker = !InspectorWindow.m_AllInspectors.Any<InspectorWindow>(new Func<InspectorWindow, bool>(trackerCAnonStorey91.\u003C\u003Em__160)) ? ActiveEditorTracker.sharedTracker : new ActiveEditorTracker();
        this.m_Tracker.inspectorMode = this.m_InspectorMode;
        this.m_Tracker.RebuildIfNecessary();
      }
    }

    protected virtual void CreatePreviewables()
    {
      if (this.m_Previews != null)
        return;
      this.m_Previews = new List<IPreviewable>();
      if (this.m_Tracker.activeEditors.Length == 0)
        return;
      foreach (Editor activeEditor in this.m_Tracker.activeEditors)
      {
        foreach (IPreviewable previewable in this.GetPreviewsForType(activeEditor))
          this.m_Previews.Add(previewable);
      }
    }

    private IEnumerable<IPreviewable> GetPreviewsForType(Editor editor)
    {
      List<IPreviewable> previewableList = new List<IPreviewable>();
      foreach (Assembly loadedAssembly in EditorAssemblies.loadedAssemblies)
      {
        foreach (System.Type type in AssemblyHelper.GetTypesFromAssembly(loadedAssembly))
        {
          if (typeof (IPreviewable).IsAssignableFrom(type) && !typeof (Editor).IsAssignableFrom(type))
          {
            foreach (CustomPreviewAttribute customAttribute in type.GetCustomAttributes(typeof (CustomPreviewAttribute), false))
            {
              if (customAttribute.m_Type == editor.target.GetType())
              {
                IPreviewable instance = Activator.CreateInstance(type) as IPreviewable;
                instance.Initialize(editor.targets);
                previewableList.Add(instance);
              }
            }
          }
        }
      }
      return (IEnumerable<IPreviewable>) previewableList;
    }

    protected virtual void ShowButton(Rect r)
    {
      bool flag = GUI.Toggle(r, this.isLocked, GUIContent.none, InspectorWindow.styles.lockButton);
      if (flag == this.isLocked)
        return;
      this.isLocked = flag;
      this.m_Tracker.RebuildIfNecessary();
    }

    protected virtual void OnGUI()
    {
      Profiler.BeginSample("InspectorWindow.OnGUI");
      this.CreateTracker();
      this.CreatePreviewables();
      InspectorWindow.FlushAllOptimizedGUIBlocksIfNeeded();
      this.ResetKeyboardControl();
      this.m_ScrollPosition = EditorGUILayout.BeginVerticalScrollView(this.m_ScrollPosition);
      if (Event.current.type == EventType.Repaint)
        this.m_Tracker.ClearDirty();
      InspectorWindow.s_CurrentInspectorWindow = this;
      Editor[] activeEditors = this.m_Tracker.activeEditors;
      this.AssignAssetEditor(activeEditors);
      Profiler.BeginSample("InspectorWindow.DrawEditors()");
      this.DrawEditors(activeEditors);
      Profiler.EndSample();
      if (this.m_Tracker.hasComponentsWhichCannotBeMultiEdited)
      {
        if (activeEditors.Length == 0 && !this.m_Tracker.isLocked && Selection.objects.Length > 0)
        {
          this.DrawSelectionPickerList();
        }
        else
        {
          Rect rect = GUILayoutUtility.GetRect(10f, 4f, EditorStyles.inspectorTitlebar);
          if (Event.current.type == EventType.Repaint)
            this.DrawSplitLine(rect.y);
          GUILayout.Label("Components that are only on some of the selected objects cannot be multi-edited.", EditorStyles.helpBox, new GUILayoutOption[0]);
          GUILayout.Space(4f);
        }
      }
      InspectorWindow.s_CurrentInspectorWindow = (InspectorWindow) null;
      EditorGUI.indentLevel = 0;
      this.AddComponentButton(this.m_Tracker.activeEditors);
      GUI.enabled = true;
      this.CheckDragAndDrop(this.m_Tracker.activeEditors);
      this.MoveFocusOnKeyPress();
      GUILayout.EndScrollView();
      Profiler.BeginSample("InspectorWindow.DrawPreviewAndLabels");
      this.DrawPreviewAndLabels();
      Profiler.EndSample();
      if (this.m_Tracker.activeEditors.Length > 0)
        this.DrawVCSShortInfo();
      Profiler.EndSample();
    }

    public virtual Editor GetLastInteractedEditor()
    {
      return this.m_LastInteractedEditor;
    }

    public IPreviewable GetEditorThatControlsPreview(IPreviewable[] editors)
    {
      if (editors.Length == 0)
        return (IPreviewable) null;
      if (this.m_SelectedPreview != null)
        return this.m_SelectedPreview;
      IPreviewable interactedEditor = (IPreviewable) this.GetLastInteractedEditor();
      System.Type type = interactedEditor == null ? (System.Type) null : interactedEditor.GetType();
      IPreviewable previewable1 = (IPreviewable) null;
      IPreviewable previewable2 = (IPreviewable) null;
      foreach (IPreviewable editor in editors)
      {
        if (editor != null && !(editor.target == (UnityEngine.Object) null) && (!EditorUtility.IsPersistent(editor.target) || !(AssetDatabase.GetAssetPath(editor.target) != AssetDatabase.GetAssetPath(editors[0].target))) && ((!(editors[0] is AssetImporterInspector) || editor is AssetImporterInspector) && editor.HasPreviewGUI()))
        {
          if (editor == interactedEditor)
            return editor;
          if (previewable2 == null && editor.GetType() == type)
            previewable2 = editor;
          if (previewable1 == null)
            previewable1 = editor;
        }
      }
      return previewable2 ?? previewable1 ?? (IPreviewable) null;
    }

    public IPreviewable[] GetEditorsWithPreviews(Editor[] editors)
    {
      IList<IPreviewable> source = (IList<IPreviewable>) new List<IPreviewable>();
      int editorIndex = -1;
      foreach (Editor editor in editors)
      {
        ++editorIndex;
        if (!(editor.target == (UnityEngine.Object) null) && (!EditorUtility.IsPersistent(editor.target) || !(AssetDatabase.GetAssetPath(editor.target) != AssetDatabase.GetAssetPath(editors[0].target))) && ((EditorUtility.IsPersistent(editors[0].target) || !EditorUtility.IsPersistent(editor.target)) && !this.ShouldCullEditor(editors, editorIndex)) && ((!(editors[0] is AssetImporterInspector) || editor is AssetImporterInspector) && editor.HasPreviewGUI()))
          source.Add((IPreviewable) editor);
      }
      using (List<IPreviewable>.Enumerator enumerator = this.m_Previews.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          IPreviewable current = enumerator.Current;
          if (current.HasPreviewGUI())
            source.Add(current);
        }
      }
      return source.ToArray<IPreviewable>();
    }

    public UnityEngine.Object GetInspectedObject()
    {
      if (this.m_Tracker == null)
        return (UnityEngine.Object) null;
      Editor importInspectorEditor = this.GetFirstNonImportInspectorEditor(this.m_Tracker.activeEditors);
      if ((UnityEngine.Object) importInspectorEditor == (UnityEngine.Object) null)
        return (UnityEngine.Object) null;
      return importInspectorEditor.target;
    }

    private Editor GetFirstNonImportInspectorEditor(Editor[] editors)
    {
      foreach (Editor editor in editors)
      {
        if (!(editor.target is AssetImporter))
          return editor;
      }
      return (Editor) null;
    }

    private void MoveFocusOnKeyPress()
    {
      KeyCode keyCode = Event.current.keyCode;
      if (Event.current.type != EventType.KeyDown || keyCode != KeyCode.DownArrow && keyCode != KeyCode.UpArrow && keyCode != KeyCode.Tab)
        return;
      if (keyCode != KeyCode.Tab)
        EditorGUIUtility.MoveFocusAndScroll(keyCode == KeyCode.DownArrow);
      else
        EditorGUIUtility.ScrollForTabbing(!Event.current.shift);
      Event.current.Use();
    }

    private void ResetKeyboardControl()
    {
      if (!this.m_ResetKeyboardControl)
        return;
      GUIUtility.keyboardControl = 0;
      this.m_ResetKeyboardControl = false;
    }

    private void CheckDragAndDrop(Editor[] editors)
    {
      Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, new GUILayoutOption[1]{ GUILayout.ExpandHeight(true) });
      if (!rect.Contains(Event.current.mousePosition))
        return;
      Editor importInspectorEditor = this.GetFirstNonImportInspectorEditor(editors);
      if ((UnityEngine.Object) importInspectorEditor != (UnityEngine.Object) null)
        InspectorWindow.DoInspectorDragAndDrop(rect, importInspectorEditor.targets);
      if (Event.current.type != EventType.MouseDown)
        return;
      GUIUtility.keyboardControl = 0;
      Event.current.Use();
    }

    private UnityEngine.Object[] GetInspectedAssets()
    {
      if (this.m_Tracker != null)
      {
        Editor importInspectorEditor = this.GetFirstNonImportInspectorEditor(this.m_Tracker.activeEditors);
        if ((UnityEngine.Object) importInspectorEditor != (UnityEngine.Object) null && (UnityEngine.Object) importInspectorEditor != (UnityEngine.Object) null && importInspectorEditor.targets.Length == 1)
        {
          string assetPath = AssetDatabase.GetAssetPath(importInspectorEditor.target);
          if (assetPath.ToLower().StartsWith("assets") && !Directory.Exists(assetPath))
            return importInspectorEditor.targets;
        }
      }
      return Selection.GetFiltered(typeof (UnityEngine.Object), SelectionMode.Assets);
    }

    private void DrawPreviewAndLabels()
    {
      if ((bool) ((UnityEngine.Object) this.m_PreviewWindow) && Event.current.type == EventType.Repaint)
        this.m_PreviewWindow.Repaint();
      IPreviewable[] editorsWithPreviews = this.GetEditorsWithPreviews(this.m_Tracker.activeEditors);
      IPreviewable thatControlsPreview = this.GetEditorThatControlsPreview(editorsWithPreviews);
      bool flag1 = thatControlsPreview != null && thatControlsPreview.HasPreviewGUI() && (UnityEngine.Object) this.m_PreviewWindow == (UnityEngine.Object) null;
      UnityEngine.Object[] inspectedAssets = this.GetInspectedAssets();
      bool flag2 = inspectedAssets.Length > 0;
      bool flag3 = ((IEnumerable<UnityEngine.Object>) inspectedAssets).Any<UnityEngine.Object>((Func<UnityEngine.Object, bool>) (a =>
      {
        if (!(a is MonoScript))
          return AssetDatabase.IsMainAsset(a);
        return false;
      }));
      if (!flag1 && !flag2)
        return;
      Event current = Event.current;
      Rect position1 = EditorGUILayout.BeginHorizontal(GUIContent.none, InspectorWindow.styles.preToolbar, GUILayout.Height(17f));
      Rect position2 = new Rect();
      GUILayout.FlexibleSpace();
      Rect lastRect = GUILayoutUtility.GetLastRect();
      GUIContent content = !flag1 ? InspectorWindow.styles.labelTitle : thatControlsPreview.GetPreviewTitle() ?? InspectorWindow.styles.preTitle;
      position2.x = lastRect.x + 3f;
      position2.y = (float) ((double) lastRect.y + (17.0 - (double) InspectorWindow.s_Styles.dragHandle.fixedHeight) / 2.0 + 1.0);
      position2.width = lastRect.width - 6f;
      position2.height = InspectorWindow.s_Styles.dragHandle.fixedHeight;
      if (editorsWithPreviews.Length > 1)
      {
        Vector2 vector2 = InspectorWindow.styles.preDropDown.CalcSize(content);
        Rect position3 = new Rect(lastRect.x, lastRect.y, vector2.x, vector2.y);
        lastRect.xMin += vector2.x;
        position2.xMin += vector2.x;
        GUIContent[] options = new GUIContent[editorsWithPreviews.Length];
        int selected = -1;
        for (int index = 0; index < editorsWithPreviews.Length; ++index)
        {
          IPreviewable previewable = editorsWithPreviews[index];
          GUIContent guiContent = previewable.GetPreviewTitle() ?? InspectorWindow.styles.preTitle;
          string text;
          if (guiContent == InspectorWindow.styles.preTitle)
          {
            string str = ObjectNames.GetTypeName(previewable.target);
            if (previewable.target is MonoBehaviour)
              str = MonoScript.FromMonoBehaviour(previewable.target as MonoBehaviour).GetClass().Name;
            text = guiContent.text + " - " + str;
          }
          else
            text = guiContent.text;
          options[index] = new GUIContent(text);
          if (editorsWithPreviews[index] == thatControlsPreview)
            selected = index;
        }
        if (GUI.Button(position3, content, InspectorWindow.styles.preDropDown))
          EditorUtility.DisplayCustomMenu(position3, options, selected, new EditorUtility.SelectMenuItemFunction(this.OnPreviewSelected), (object) editorsWithPreviews);
      }
      else
      {
        float width = Mathf.Min((float) ((double) position2.xMax - (double) lastRect.xMin - 3.0 - 20.0), InspectorWindow.styles.preToolbar2.CalcSize(content).x);
        Rect position3 = new Rect(lastRect.x, lastRect.y, width, lastRect.height);
        position2.xMin = position3.xMax + 3f;
        GUI.Label(position3, content, InspectorWindow.styles.preToolbar2);
      }
      if (flag1 && Event.current.type == EventType.Repaint)
        InspectorWindow.s_Styles.dragHandle.Draw(position2, GUIContent.none, false, false, false, false);
      if (flag1 && this.m_PreviewResizer.GetExpandedBeforeDragging())
        thatControlsPreview.OnPreviewSettings();
      EditorGUILayout.EndHorizontal();
      if (current.type == EventType.MouseUp && current.button == 1 && (position1.Contains(current.mousePosition) && (UnityEngine.Object) this.m_PreviewWindow == (UnityEngine.Object) null))
        this.DetachPreview();
      float height;
      if (flag1)
      {
        Rect position3 = this.position;
        if (EditorSettings.externalVersionControl != ExternalVersionControl.Disabled && EditorSettings.externalVersionControl != ExternalVersionControl.AutoDetect && EditorSettings.externalVersionControl != ExternalVersionControl.Generic)
          position3.height -= 17f;
        height = this.m_PreviewResizer.ResizeHandle(position3, 100f, 100f, 17f, lastRect);
      }
      else
      {
        if (GUI.Button(position1, GUIContent.none, GUIStyle.none))
          this.m_PreviewResizer.ToggleExpanded();
        height = 0.0f;
      }
      if (!this.m_PreviewResizer.GetExpanded())
        return;
      GUILayout.BeginVertical(InspectorWindow.styles.preBackground, GUILayout.Height(height));
      if (flag1)
        thatControlsPreview.DrawPreview(GUILayoutUtility.GetRect(0.0f, 10240f, 64f, 10240f));
      if (flag2)
      {
        EditorGUI.BeginDisabledGroup(((IEnumerable<UnityEngine.Object>) inspectedAssets).Any<UnityEngine.Object>((Func<UnityEngine.Object, bool>) (a =>
        {
          if (EditorUtility.IsPersistent(a))
            return !Editor.IsAppropriateFileOpenForEdit(a);
          return false;
        })));
        this.m_LabelGUI.OnLabelGUI(inspectedAssets);
        EditorGUI.EndDisabledGroup();
      }
      if (flag3)
        this.m_AssetBundleNameGUI.OnAssetBundleNameGUI((IEnumerable<UnityEngine.Object>) inspectedAssets);
      GUILayout.EndVertical();
    }

    protected UnityEngine.Object[] GetTargetsForPreview(IPreviewable previewEditor)
    {
      Editor editor = (Editor) null;
      foreach (Editor activeEditor in this.m_Tracker.activeEditors)
      {
        if (activeEditor.target.GetType() == previewEditor.target.GetType())
        {
          editor = activeEditor;
          break;
        }
      }
      return editor.targets;
    }

    private void OnPreviewSelected(object userData, string[] options, int selected)
    {
      this.m_SelectedPreview = (userData as IPreviewable[])[selected];
    }

    private void DetachPreview()
    {
      Event.current.Use();
      this.m_PreviewWindow = ScriptableObject.CreateInstance(typeof (PreviewWindow)) as PreviewWindow;
      this.m_PreviewWindow.SetParentInspector(this);
      this.m_PreviewWindow.Show();
      this.Repaint();
      GUIUtility.ExitGUI();
    }

    protected virtual void DrawVCSSticky(float offset)
    {
      string message = string.Empty;
      if (EditorPrefs.GetBool("vcssticky") || Editor.IsAppropriateFileOpenForEdit(this.GetFirstNonImportInspectorEditor(this.m_Tracker.activeEditors).target, out message))
        return;
      Rect position1 = new Rect(10f, this.position.height - 94f, this.position.width - 20f, 80f);
      position1.y -= offset;
      if (Event.current.type != EventType.Repaint)
        return;
      InspectorWindow.styles.stickyNote.Draw(position1, false, false, false, false);
      Rect position2 = new Rect(position1.x, (float) ((double) position1.y + (double) position1.height / 2.0 - 32.0), 64f, 64f);
      if (EditorSettings.externalVersionControl == "Perforce")
        InspectorWindow.styles.stickyNotePerforce.Draw(position2, false, false, false, false);
      GUI.Label(new Rect(position1.x + position2.width, position1.y, position1.width - position2.width, position1.height), new GUIContent("<b>Under Version Control</b>\nCheck out this asset in order to make changes."), InspectorWindow.styles.stickyNoteLabel);
      InspectorWindow.styles.stickyNoteArrow.Draw(new Rect(position1.x + position1.width / 2f, position1.y + 80f, 19f, 14f), false, false, false, false);
    }

    private void DrawVCSShortInfo()
    {
      if (EditorSettings.externalVersionControl == ExternalVersionControl.AssetServer)
      {
        EditorGUILayout.BeginHorizontal(GUIContent.none, InspectorWindow.styles.preToolbar, GUILayout.Height(17f));
        UnityEngine.Object target = this.GetFirstNonImportInspectorEditor(this.m_Tracker.activeEditors).target;
        int controlId = GUIUtility.GetControlID(FocusType.Passive);
        GUILayout.FlexibleSpace();
        Rect lastRect = GUILayoutUtility.GetLastRect();
        EditorGUILayout.EndHorizontal();
        AssetInspector.Get().OnAssetStatusGUI(lastRect, controlId, target, InspectorWindow.styles.preToolbar2);
      }
      if (!Provider.isActive || !(EditorSettings.externalVersionControl != ExternalVersionControl.Disabled) || (!(EditorSettings.externalVersionControl != ExternalVersionControl.AutoDetect) || !(EditorSettings.externalVersionControl != ExternalVersionControl.Generic)))
        return;
      Editor importInspectorEditor = this.GetFirstNonImportInspectorEditor(this.m_Tracker.activeEditors);
      string assetPath = AssetDatabase.GetAssetPath(importInspectorEditor.target);
      Asset assetByPath1 = Provider.GetAssetByPath(assetPath);
      if (assetByPath1 == null || !assetByPath1.path.StartsWith("Assets") && !assetByPath1.path.StartsWith("ProjectSettings"))
        return;
      Asset assetByPath2 = Provider.GetAssetByPath(assetPath.Trim('/') + ".meta");
      string currentState1 = assetByPath1.StateToString();
      string currentState2 = assetByPath2 != null ? assetByPath2.StateToString() : string.Empty;
      bool flag1 = assetByPath2 != null && (assetByPath2.state & ~Asset.States.MetaFile) != assetByPath1.state;
      bool flag2 = currentState1 != string.Empty;
      float height = !flag1 || !flag2 ? 17f : 34f;
      GUILayout.Label(GUIContent.none, InspectorWindow.styles.preToolbar, new GUILayoutOption[1]
      {
        GUILayout.Height(height)
      });
      Rect lastRect1 = GUILayoutUtility.GetLastRect();
      bool flag3 = Event.current.type == EventType.Layout || Event.current.type == EventType.Repaint;
      if (flag2 && flag3)
      {
        Texture2D cachedIcon = AssetDatabase.GetCachedIcon(assetPath) as Texture2D;
        if (flag1)
        {
          Rect rect = lastRect1;
          rect.height = 17f;
          this.DrawVCSShortInfoAsset(assetByPath1, this.BuildTooltip(assetByPath1, (Asset) null), rect, cachedIcon, currentState1);
          Texture2D iconForFile = InternalEditorUtility.GetIconForFile(assetByPath2.path);
          rect.y += 17f;
          this.DrawVCSShortInfoAsset(assetByPath2, this.BuildTooltip((Asset) null, assetByPath2), rect, iconForFile, currentState2);
        }
        else
          this.DrawVCSShortInfoAsset(assetByPath1, this.BuildTooltip(assetByPath1, assetByPath2), lastRect1, cachedIcon, currentState1);
      }
      else if (currentState2 != string.Empty && flag3)
      {
        Texture2D iconForFile = InternalEditorUtility.GetIconForFile(assetByPath2.path);
        this.DrawVCSShortInfoAsset(assetByPath2, this.BuildTooltip(assetByPath1, assetByPath2), lastRect1, iconForFile, currentState2);
      }
      string message = string.Empty;
      if (Editor.IsAppropriateFileOpenForEdit(importInspectorEditor.target, out message))
        return;
      float width = 66f;
      if (GUI.Button(new Rect(lastRect1.x + lastRect1.width - width, lastRect1.y, width, lastRect1.height), "Check out", InspectorWindow.styles.lockedHeaderButton))
      {
        EditorPrefs.SetBool("vcssticky", true);
        Provider.Checkout(importInspectorEditor.targets, CheckoutMode.Both).Wait();
        this.Repaint();
      }
      this.DrawVCSSticky(lastRect1.height / 2f);
    }

    protected string BuildTooltip(Asset asset, Asset metaAsset)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (asset != null)
      {
        stringBuilder.AppendLine("Asset:");
        stringBuilder.AppendLine(asset.AllStateToString());
      }
      if (metaAsset != null)
      {
        stringBuilder.AppendLine("Meta file:");
        stringBuilder.AppendLine(metaAsset.AllStateToString());
      }
      return stringBuilder.ToString();
    }

    protected void DrawVCSShortInfoAsset(Asset asset, string tooltip, Rect rect, Texture2D icon, string currentState)
    {
      Rect itemRect = new Rect(rect.x, rect.y, 28f, 16f);
      Rect position1 = itemRect;
      position1.x += 6f;
      position1.width = 16f;
      if ((UnityEngine.Object) icon != (UnityEngine.Object) null)
        GUI.DrawTexture(position1, (Texture) icon);
      Overlay.DrawOverlay(asset, itemRect);
      Rect position2 = new Rect(rect.x + 26f, rect.y, rect.width - 31f, rect.height);
      GUIContent label = GUIContent.Temp(currentState);
      label.tooltip = tooltip;
      EditorGUI.LabelField(position2, label, InspectorWindow.styles.preToolbar2);
    }

    protected void AssignAssetEditor(Editor[] editors)
    {
      if (editors.Length <= 1 || !(editors[0] is AssetImporterInspector))
        return;
      (editors[0] as AssetImporterInspector).assetEditor = editors[1];
    }

    private void DrawEditors(Editor[] editors)
    {
      if (editors.Length == 0)
        return;
      UnityEngine.Object inspectedObject = this.GetInspectedObject();
      string message = string.Empty;
      DockArea parent = this.m_Parent as DockArea;
      if ((UnityEngine.Object) parent != (UnityEngine.Object) null)
        parent.tabStyle = (GUIStyle) "dragtabbright";
      GUILayout.Space(0.0f);
      if (inspectedObject is Material)
      {
        for (int index = 0; index <= 1 && index < editors.Length; ++index)
        {
          MaterialEditor editor = editors[index] as MaterialEditor;
          if ((UnityEngine.Object) editor != (UnityEngine.Object) null)
          {
            editor.forceVisible = true;
            break;
          }
        }
      }
      bool rebuildOptimizedGUIBlock = false;
      if (Event.current.type == EventType.Repaint)
      {
        if (inspectedObject != (UnityEngine.Object) null && this.m_IsOpenForEdit != Editor.IsAppropriateFileOpenForEdit(inspectedObject, out message))
        {
          this.m_IsOpenForEdit = !this.m_IsOpenForEdit;
          rebuildOptimizedGUIBlock = true;
        }
        if (this.m_InvalidateGUIBlockCache)
        {
          rebuildOptimizedGUIBlock = true;
          this.m_InvalidateGUIBlockCache = false;
        }
      }
      else if (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "EyeDropperUpdate")
        rebuildOptimizedGUIBlock = true;
      Editor.m_AllowMultiObjectAccess = true;
      bool showImportedObjectBarNext = false;
      Rect importedObjectBarRect = new Rect();
      for (int editorIndex = 0; editorIndex < editors.Length; ++editorIndex)
      {
        if (this.ShouldCullEditor(editors, editorIndex))
        {
          if (Event.current.type == EventType.Repaint)
            editors[editorIndex].isInspectorDirty = false;
        }
        else
        {
          bool textFieldInput = GUIUtility.textFieldInput;
          this.DrawEditor(editors[editorIndex], editorIndex, rebuildOptimizedGUIBlock, ref showImportedObjectBarNext, ref importedObjectBarRect);
          if (Event.current.type == EventType.Repaint && !textFieldInput && GUIUtility.textFieldInput)
            InspectorWindow.FlushOptimizedGUIBlock(editors[editorIndex]);
        }
      }
      EditorGUIUtility.ResetGUIState();
      if ((double) importedObjectBarRect.height <= 0.0)
        return;
      GUI.BeginGroup(importedObjectBarRect);
      GUI.Label(new Rect(0.0f, 0.0f, importedObjectBarRect.width, importedObjectBarRect.height), "Imported Object", (GUIStyle) "OL Title");
      GUI.EndGroup();
    }

    internal override void OnResized()
    {
      this.m_InvalidateGUIBlockCache = true;
    }

    private void DrawEditor(Editor editor, int editorIndex, bool rebuildOptimizedGUIBlock, ref bool showImportedObjectBarNext, ref Rect importedObjectBarRect)
    {
      if ((UnityEngine.Object) editor == (UnityEngine.Object) null)
        return;
      UnityEngine.Object target = editor.target;
      GUIUtility.GetControlID(target.GetInstanceID(), FocusType.Passive);
      EditorGUIUtility.ResetGUIState();
      GUILayoutGroup topLevel = GUILayoutUtility.current.topLevel;
      int visible = this.m_Tracker.GetVisible(editorIndex);
      bool flag1;
      if (visible == -1)
      {
        flag1 = InternalEditorUtility.GetIsInspectorExpanded(target);
        this.m_Tracker.SetVisible(editorIndex, !flag1 ? 0 : 1);
      }
      else
        flag1 = visible == 1;
      rebuildOptimizedGUIBlock |= editor.isInspectorDirty;
      if (Event.current.type == EventType.Repaint)
        editor.isInspectorDirty = false;
      ScriptAttributeUtility.propertyHandlerCache = editor.propertyHandlerCache;
      bool flag2 = AssetDatabase.IsMainAsset(target) || AssetDatabase.IsSubAsset(target) || editorIndex == 0 || target is Material;
      if (flag2)
      {
        string message = string.Empty;
        bool flag3 = editor.IsOpenForEdit(out message);
        if (showImportedObjectBarNext)
        {
          showImportedObjectBarNext = false;
          GUILayout.Space(15f);
          importedObjectBarRect = GUILayoutUtility.GetRect(16f, 16f);
          importedObjectBarRect.height = 17f;
        }
        flag1 = true;
        EditorGUI.BeginDisabledGroup(!flag3);
        editor.DrawHeader();
        EditorGUI.EndDisabledGroup();
      }
      if (editor.target is AssetImporter)
        showImportedObjectBarNext = true;
      bool flag4 = false;
      if (editor is GenericInspector && CustomEditorAttributes.FindCustomEditorType(target, false) != null && this.m_InspectorMode != InspectorMode.DebugInternal)
      {
        if (this.m_InspectorMode == InspectorMode.Normal)
          flag4 = true;
        else if (target is AssetImporter)
          flag4 = true;
      }
      if (!flag2)
      {
        EditorGUI.BeginDisabledGroup(!editor.IsEnabled());
        bool isExpanded = EditorGUILayout.InspectorTitlebar(flag1, editor.targets, editor.CanBeExpandedViaAFoldout());
        if (flag1 != isExpanded)
        {
          this.m_Tracker.SetVisible(editorIndex, !isExpanded ? 0 : 1);
          InternalEditorUtility.SetIsInspectorExpanded(target, isExpanded);
          if (isExpanded)
            this.m_LastInteractedEditor = editor;
          else if ((UnityEngine.Object) this.m_LastInteractedEditor == (UnityEngine.Object) editor)
            this.m_LastInteractedEditor = (Editor) null;
        }
        EditorGUI.EndDisabledGroup();
      }
      if (flag4 && flag1)
      {
        GUILayout.Label("Multi-object editing not supported.", EditorStyles.helpBox, new GUILayoutOption[0]);
      }
      else
      {
        EditorGUIUtility.ResetGUIState();
        EditorGUI.BeginDisabledGroup(!editor.IsEnabled());
        GenericInspector genericInspector = editor as GenericInspector;
        if ((bool) ((UnityEngine.Object) genericInspector))
          genericInspector.m_InspectorMode = this.m_InspectorMode;
        EditorGUIUtility.hierarchyMode = true;
        EditorGUIUtility.wideMode = (double) this.position.width > 330.0;
        ScriptAttributeUtility.propertyHandlerCache = editor.propertyHandlerCache;
        Rect rect = new Rect();
        OptimizedGUIBlock block;
        float height;
        if (editor.GetOptimizedGUIBlock(rebuildOptimizedGUIBlock, flag1, out block, out height))
        {
          rect = GUILayoutUtility.GetRect(0.0f, !flag1 ? 0.0f : height);
          this.HandleLastInteractedEditor(rect, editor);
          if (Event.current.type == EventType.Layout)
            return;
          if (block.Begin(rebuildOptimizedGUIBlock, rect) && flag1)
          {
            GUI.changed = false;
            editor.OnOptimizedInspectorGUI(rect);
          }
          block.End();
        }
        else
        {
          if (flag1)
          {
            rect = EditorGUILayout.BeginVertical(!editor.UseDefaultMargins() ? GUIStyle.none : EditorStyles.inspectorDefaultMargins, new GUILayoutOption[0]);
            this.HandleLastInteractedEditor(rect, editor);
            GUI.changed = false;
            try
            {
              editor.OnInspectorGUI();
            }
            catch (Exception ex)
            {
              if (ex is ExitGUIException)
                throw;
              else
                Debug.LogException(ex);
            }
            EditorGUILayout.EndVertical();
          }
          if (Event.current.type == EventType.Used)
            return;
        }
        EditorGUI.EndDisabledGroup();
        if (GUILayoutUtility.current.topLevel != topLevel)
        {
          if (!GUILayoutUtility.current.layoutGroups.Contains((object) topLevel))
          {
            Debug.LogError((object) "Expected top level layout group missing! Too many GUILayout.EndScrollView/EndVertical/EndHorizontal?");
            GUIUtility.ExitGUI();
          }
          else
          {
            Debug.LogWarning((object) "Unexpected top level layout group! Missing GUILayout.EndScrollView/EndVertical/EndHorizontal?");
            while (GUILayoutUtility.current.topLevel != topLevel)
              GUILayoutUtility.EndLayoutGroup();
          }
        }
        this.HandleComponentScreenshot(rect, editor);
      }
    }

    private void HandleComponentScreenshot(Rect contentRect, Editor editor)
    {
      if (!ScreenShots.s_TakeComponentScreenshot)
        return;
      contentRect.yMin -= 16f;
      if (!contentRect.Contains(Event.current.mousePosition))
        return;
      Rect contentRect1 = GUIClip.Unclip(contentRect);
      contentRect1.position = contentRect1.position + this.m_Parent.screenPosition.position;
      ScreenShots.ScreenShotComponent(contentRect1, editor.target);
    }

    private bool ShouldCullEditor(Editor[] editors, int editorIndex)
    {
      if (editors[editorIndex].hideInspector)
        return true;
      UnityEngine.Object target = editors[editorIndex].target;
      if (target is SubstanceImporter || target is ParticleSystemRenderer || target.GetType() == typeof (AssetImporter))
        return true;
      if (this.m_InspectorMode == InspectorMode.Normal && editorIndex != 0)
      {
        AssetImporterInspector editor = editors[0] as AssetImporterInspector;
        if ((UnityEngine.Object) editor != (UnityEngine.Object) null && !editor.showImportedObject)
          return true;
      }
      return false;
    }

    private void DrawSelectionPickerList()
    {
      if (this.m_TypeSelectionList == null)
        this.m_TypeSelectionList = new TypeSelectionList(Selection.objects);
      DockArea parent = this.m_Parent as DockArea;
      if ((UnityEngine.Object) parent != (UnityEngine.Object) null)
        parent.tabStyle = (GUIStyle) "dragtabbright";
      GUILayout.Space(0.0f);
      Editor.DrawHeaderGUI((Editor) null, Selection.objects.Length.ToString() + " Objects");
      GUILayout.Label("Narrow the Selection:", EditorStyles.label, new GUILayoutOption[0]);
      GUILayout.Space(4f);
      Vector2 iconSize = EditorGUIUtility.GetIconSize();
      EditorGUIUtility.SetIconSize(new Vector2(16f, 16f));
      using (List<TypeSelection>.Enumerator enumerator = this.m_TypeSelectionList.typeSelections.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          TypeSelection current = enumerator.Current;
          Rect rect = GUILayoutUtility.GetRect(16f, 16f, new GUILayoutOption[1]{ GUILayout.ExpandWidth(true) });
          if (GUI.Button(rect, current.label, InspectorWindow.styles.typeSelection))
          {
            Selection.objects = current.objects;
            Event.current.Use();
          }
          if (GUIUtility.hotControl == 0)
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
          GUILayout.Space(4f);
        }
      }
      EditorGUIUtility.SetIconSize(iconSize);
    }

    private void HandleLastInteractedEditor(Rect componentRect, Editor editor)
    {
      if (!((UnityEngine.Object) editor != (UnityEngine.Object) this.m_LastInteractedEditor) || Event.current.type != EventType.MouseDown || !componentRect.Contains(Event.current.mousePosition))
        return;
      this.m_LastInteractedEditor = editor;
      this.Repaint();
    }

    private void AddComponentButton(Editor[] editors)
    {
      Editor importInspectorEditor = this.GetFirstNonImportInspectorEditor(editors);
      if (!((UnityEngine.Object) importInspectorEditor != (UnityEngine.Object) null) || !(importInspectorEditor.target != (UnityEngine.Object) null) || (!(importInspectorEditor.target is GameObject) || !importInspectorEditor.IsEnabled()))
        return;
      EditorGUILayout.BeginHorizontal();
      GUIContent addComponentLabel = InspectorWindow.s_Styles.addComponentLabel;
      Rect rect = GUILayoutUtility.GetRect(addComponentLabel, InspectorWindow.styles.addComponentButtonStyle, (GUILayoutOption[]) null);
      rect.y += 10f;
      rect.x += (float) (((double) rect.width - 230.0) / 2.0);
      rect.width = 230f;
      if (Event.current.type == EventType.Repaint)
        this.DrawSplitLine(rect.y - 11f);
      Event current = Event.current;
      bool flag = false;
      if (current.type == EventType.ExecuteCommand && current.commandName == "OpenAddComponentDropdown")
      {
        flag = true;
        current.Use();
      }
      if ((EditorGUI.ButtonMouseDown(rect, addComponentLabel, FocusType.Passive, InspectorWindow.styles.addComponentButtonStyle) || flag) && AddComponentWindow.Show(rect, ((IEnumerable<UnityEngine.Object>) importInspectorEditor.targets).Select<UnityEngine.Object, GameObject>((Func<UnityEngine.Object, GameObject>) (o => (GameObject) o)).ToArray<GameObject>()))
        GUIUtility.ExitGUI();
      EditorGUILayout.EndHorizontal();
    }

    private bool ReadyToRepaint()
    {
      if (AnimationMode.InAnimationPlaybackMode())
      {
        long num = DateTime.Now.Ticks / 10000L;
        if (num - this.s_LastUpdateWhilePlayingAnimation < 150L)
          return false;
        this.s_LastUpdateWhilePlayingAnimation = num;
      }
      return true;
    }

    private void DrawSplitLine(float y)
    {
      GUI.DrawTextureWithTexCoords(new Rect(0.0f, y, this.m_Pos.width + 1f, 1f), (Texture) EditorStyles.inspectorTitlebar.normal.background, new Rect(0.0f, 1f, 1f, (float) (1.0 - 1.0 / (double) EditorStyles.inspectorTitlebar.normal.background.height)));
    }

    internal static void ShowWindow()
    {
      EditorWindow.GetWindow(typeof (InspectorWindow));
    }

    private static void FlushOptimizedGUI()
    {
      InspectorWindow.s_AllOptimizedGUIBlocksNeedsRebuild = true;
    }

    private static void FlushAllOptimizedGUIBlocksIfNeeded()
    {
      if (!InspectorWindow.s_AllOptimizedGUIBlocksNeedsRebuild)
        return;
      InspectorWindow.s_AllOptimizedGUIBlocksNeedsRebuild = false;
      using (List<InspectorWindow>.Enumerator enumerator = InspectorWindow.m_AllInspectors.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          InspectorWindow current = enumerator.Current;
          if (current.m_Tracker != null)
          {
            foreach (Editor activeEditor in current.m_Tracker.activeEditors)
              InspectorWindow.FlushOptimizedGUIBlock(activeEditor);
          }
        }
      }
    }

    private static void FlushOptimizedGUIBlock(Editor editor)
    {
      OptimizedGUIBlock block;
      float height;
      if ((UnityEngine.Object) editor == (UnityEngine.Object) null || !editor.GetOptimizedGUIBlock(false, false, out block, out height))
        return;
      block.valid = false;
    }

    private void Update()
    {
      if (this.m_Tracker == null)
        return;
      Editor[] activeEditors = this.m_Tracker.activeEditors;
      if (activeEditors == null)
        return;
      bool flag = false;
      foreach (Editor editor in activeEditors)
      {
        if (editor.RequiresConstantRepaint() && !editor.hideInspector)
          flag = true;
      }
      if (!flag || this.m_lastRenderedTime + 0.0329999998211861 >= EditorApplication.timeSinceStartup)
        return;
      this.m_lastRenderedTime = EditorApplication.timeSinceStartup;
      this.Repaint();
    }

    internal class Styles
    {
      public readonly GUIStyle preToolbar = (GUIStyle) "preToolbar";
      public readonly GUIStyle preToolbar2 = (GUIStyle) "preToolbar2";
      public readonly GUIStyle preDropDown = (GUIStyle) "preDropDown";
      public readonly GUIStyle dragHandle = (GUIStyle) "RL DragHandle";
      public readonly GUIStyle lockButton = (GUIStyle) "IN LockButton";
      public readonly GUIContent preTitle = EditorGUIUtility.TextContent("Preview");
      public readonly GUIContent labelTitle = EditorGUIUtility.TextContent("Asset Labels");
      public readonly GUIContent addComponentLabel = EditorGUIUtility.TextContent("Add Component");
      public GUIStyle preBackground = (GUIStyle) "preBackground";
      public GUIStyle addComponentArea = EditorStyles.inspectorTitlebar;
      public GUIStyle addComponentButtonStyle = (GUIStyle) "LargeButton";
      public GUIStyle previewMiniLabel = new GUIStyle(EditorStyles.whiteMiniLabel);
      public GUIStyle typeSelection = new GUIStyle((GUIStyle) "PR Label");
      public GUIStyle lockedHeaderButton = (GUIStyle) "preButton";
      public GUIStyle stickyNote = new GUIStyle((GUIStyle) "VCS_StickyNote");
      public GUIStyle stickyNoteArrow = new GUIStyle((GUIStyle) "VCS_StickyNoteArrow");
      public GUIStyle stickyNotePerforce = new GUIStyle((GUIStyle) "VCS_StickyNoteP4");
      public GUIStyle stickyNoteLabel = new GUIStyle((GUIStyle) "VCS_StickyNoteLabel");

      public Styles()
      {
        this.typeSelection.padding.left = 12;
      }
    }
  }
}
