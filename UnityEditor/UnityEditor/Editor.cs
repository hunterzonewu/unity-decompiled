// Decompiled with JetBrains decompiler
// Type: UnityEditor.Editor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Base class to derive custom Editors from. Use this to create your own custom inspectors and editors for your objects.</para>
  /// </summary>
  [RequiredByNativeCode]
  [StructLayout(LayoutKind.Sequential)]
  public class Editor : ScriptableObject, IPreviewable
  {
    internal static bool m_AllowMultiObjectAccess = true;
    private PropertyHandlerCache m_PropertyHandlerCache = new PropertyHandlerCache();
    internal const float kLineHeight = 16f;
    private const float kImageSectionWidth = 44f;
    private UnityEngine.Object[] m_Targets;
    private int m_IsDirty;
    private int m_ReferenceTargetIndex;
    private IPreviewable m_DummyPreview;
    internal SerializedObject m_SerializedObject;
    private OptimizedGUIBlock m_OptimizedBlock;
    internal InspectorMode m_InspectorMode;
    internal bool hideInspector;
    private static Editor.Styles s_Styles;

    internal bool canEditMultipleObjects
    {
      get
      {
        return this.GetType().GetCustomAttributes(typeof (CanEditMultipleObjects), false).Length > 0;
      }
    }

    /// <summary>
    ///   <para>The object being inspected.</para>
    /// </summary>
    public UnityEngine.Object target
    {
      get
      {
        return this.m_Targets[this.referenceTargetIndex];
      }
      set
      {
        throw new InvalidOperationException("You can't set the target on an editor.");
      }
    }

    /// <summary>
    ///   <para>An array of all the object being inspected.</para>
    /// </summary>
    public UnityEngine.Object[] targets
    {
      get
      {
        if (!Editor.m_AllowMultiObjectAccess)
          Debug.LogError((object) "The targets array should not be used inside OnSceneGUI or OnPreviewGUI. Use the single target property instead.");
        return this.m_Targets;
      }
    }

    internal virtual int referenceTargetIndex
    {
      get
      {
        return Mathf.Clamp(this.m_ReferenceTargetIndex, 0, this.m_Targets.Length - 1);
      }
      set
      {
        this.m_ReferenceTargetIndex = (Math.Abs(value * this.m_Targets.Length) + value) % this.m_Targets.Length;
      }
    }

    internal virtual string targetTitle
    {
      get
      {
        if (this.m_Targets.Length == 1 || !Editor.m_AllowMultiObjectAccess)
          return this.target.name;
        return this.m_Targets.Length.ToString() + " " + ObjectNames.NicifyVariableName(ObjectNames.GetClassName(this.target)) + "s";
      }
    }

    /// <summary>
    ///   <para>A SerializedObject representing the object or objects being inspected.</para>
    /// </summary>
    public SerializedObject serializedObject
    {
      get
      {
        if (!Editor.m_AllowMultiObjectAccess)
          Debug.LogError((object) "The serializedObject should not be used inside OnSceneGUI or OnPreviewGUI. Use the target property directly instead.");
        return this.GetSerializedObjectInternal();
      }
    }

    internal bool isInspectorDirty
    {
      get
      {
        return this.m_IsDirty != 0;
      }
      set
      {
        this.m_IsDirty = !value ? 0 : 1;
      }
    }

    internal virtual IPreviewable preview
    {
      get
      {
        if (this.m_DummyPreview == null)
        {
          this.m_DummyPreview = (IPreviewable) new ObjectPreview();
          this.m_DummyPreview.Initialize(this.targets);
        }
        return this.m_DummyPreview;
      }
    }

    internal PropertyHandlerCache propertyHandlerCache
    {
      get
      {
        return this.m_PropertyHandlerCache;
      }
    }

    /// <summary>
    ///   <para>Make a custom editor for targetObject or targetObjects.</para>
    /// </summary>
    /// <param name="objects">All objects must be of same exact type.</param>
    /// <param name="targetObject"></param>
    /// <param name="editorType"></param>
    /// <param name="targetObjects"></param>
    [ExcludeFromDocs]
    public static Editor CreateEditor(UnityEngine.Object targetObject)
    {
      System.Type editorType = (System.Type) null;
      return Editor.CreateEditor(targetObject, editorType);
    }

    /// <summary>
    ///   <para>Make a custom editor for targetObject or targetObjects.</para>
    /// </summary>
    /// <param name="objects">All objects must be of same exact type.</param>
    /// <param name="targetObject"></param>
    /// <param name="editorType"></param>
    /// <param name="targetObjects"></param>
    public static Editor CreateEditor(UnityEngine.Object targetObject, [DefaultValue("null")] System.Type editorType)
    {
      return Editor.CreateEditor(new UnityEngine.Object[1]{ targetObject }, editorType);
    }

    /// <summary>
    ///   <para>Make a custom editor for targetObject or targetObjects.</para>
    /// </summary>
    /// <param name="objects">All objects must be of same exact type.</param>
    /// <param name="targetObject"></param>
    /// <param name="editorType"></param>
    /// <param name="targetObjects"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Editor CreateEditor(UnityEngine.Object[] targetObjects, [DefaultValue("null")] System.Type editorType);

    /// <summary>
    ///   <para>Make a custom editor for targetObject or targetObjects.</para>
    /// </summary>
    /// <param name="objects">All objects must be of same exact type.</param>
    /// <param name="targetObject"></param>
    /// <param name="editorType"></param>
    /// <param name="targetObjects"></param>
    [ExcludeFromDocs]
    public static Editor CreateEditor(UnityEngine.Object[] targetObjects)
    {
      System.Type editorType = (System.Type) null;
      return Editor.CreateEditor(targetObjects, editorType);
    }

    public static void CreateCachedEditor(UnityEngine.Object targetObject, System.Type editorType, ref Editor previousEditor)
    {
      if ((UnityEngine.Object) previousEditor != (UnityEngine.Object) null && previousEditor.m_Targets.Length == 1 && previousEditor.m_Targets[0] == targetObject)
        return;
      if ((UnityEngine.Object) previousEditor != (UnityEngine.Object) null)
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) previousEditor);
      previousEditor = Editor.CreateEditor(targetObject, editorType);
    }

    public static void CreateCachedEditor(UnityEngine.Object[] targetObjects, System.Type editorType, ref Editor previousEditor)
    {
      if ((UnityEngine.Object) previousEditor != (UnityEngine.Object) null && ArrayUtility.ArrayEquals<UnityEngine.Object>(previousEditor.m_Targets, targetObjects))
        return;
      if ((UnityEngine.Object) previousEditor != (UnityEngine.Object) null)
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) previousEditor);
      previousEditor = Editor.CreateEditor(targetObjects, editorType);
    }

    internal virtual SerializedObject GetSerializedObjectInternal()
    {
      if (this.m_SerializedObject == null)
        this.m_SerializedObject = new SerializedObject(this.targets);
      return this.m_SerializedObject;
    }

    private void CleanupPropertyEditor()
    {
      if (this.m_OptimizedBlock != null)
      {
        this.m_OptimizedBlock.Dispose();
        this.m_OptimizedBlock = (OptimizedGUIBlock) null;
      }
      if (this.m_SerializedObject == null)
        return;
      this.m_SerializedObject.Dispose();
      this.m_SerializedObject = (SerializedObject) null;
    }

    private void OnDisableINTERNAL()
    {
      this.CleanupPropertyEditor();
    }

    internal virtual void OnForceReloadInspector()
    {
      if (this.m_SerializedObject == null)
        return;
      this.m_SerializedObject.SetIsDifferentCacheDirty();
    }

    internal bool GetOptimizedGUIBlockImplementation(bool isDirty, bool isVisible, out OptimizedGUIBlock block, out float height)
    {
      if (this.m_OptimizedBlock == null)
        this.m_OptimizedBlock = new OptimizedGUIBlock();
      block = this.m_OptimizedBlock;
      if (!isVisible)
      {
        height = 0.0f;
        return true;
      }
      if (this.m_SerializedObject == null)
        this.m_SerializedObject = new SerializedObject(this.targets);
      else
        this.m_SerializedObject.Update();
      this.m_SerializedObject.inspectorMode = this.m_InspectorMode;
      SerializedProperty iterator = this.m_SerializedObject.GetIterator();
      height = 2f;
      for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
        height = height + (EditorGUI.GetPropertyHeight(iterator, (GUIContent) null, true) + 2f);
      if ((double) height == 2.0)
        height = 0.0f;
      return true;
    }

    internal bool OptimizedInspectorGUIImplementation(Rect contentRect)
    {
      SerializedProperty iterator = this.m_SerializedObject.GetIterator();
      bool enterChildren = true;
      bool enabled = GUI.enabled;
      contentRect.xMin += 14f;
      contentRect.xMax -= 4f;
      contentRect.y += 2f;
      while (iterator.NextVisible(enterChildren))
      {
        contentRect.height = EditorGUI.GetPropertyHeight(iterator, (GUIContent) null, false);
        EditorGUI.indentLevel = iterator.depth;
        using (new EditorGUI.DisabledGroupScope(this.m_InspectorMode == InspectorMode.Normal && "m_Script" == iterator.propertyPath))
          enterChildren = EditorGUI.PropertyField(contentRect, iterator);
        contentRect.y += contentRect.height + 2f;
      }
      GUI.enabled = enabled;
      return this.m_SerializedObject.ApplyModifiedProperties();
    }

    protected internal static void DrawPropertiesExcluding(SerializedObject obj, params string[] propertyToExclude)
    {
      SerializedProperty iterator = obj.GetIterator();
      bool enterChildren = true;
      while (iterator.NextVisible(enterChildren))
      {
        enterChildren = false;
        if (!((IEnumerable<string>) propertyToExclude).Contains<string>(iterator.name))
          EditorGUILayout.PropertyField(iterator, true, new GUILayoutOption[0]);
      }
    }

    /// <summary>
    ///   <para>Draw the built-in inspector.</para>
    /// </summary>
    public bool DrawDefaultInspector()
    {
      return this.DoDrawDefaultInspector();
    }

    /// <summary>
    ///   <para>Implement this function to make a custom inspector.</para>
    /// </summary>
    public virtual void OnInspectorGUI()
    {
      this.DrawDefaultInspector();
    }

    /// <summary>
    ///   <para>Does this edit require to be repainted constantly in its current state?</para>
    /// </summary>
    public virtual bool RequiresConstantRepaint()
    {
      return false;
    }

    internal void InternalSetTargets(UnityEngine.Object[] t)
    {
      this.m_Targets = t;
    }

    internal void InternalSetHidden(bool hidden)
    {
      this.hideInspector = hidden;
    }

    internal virtual bool GetOptimizedGUIBlock(bool isDirty, bool isVisible, out OptimizedGUIBlock block, out float height)
    {
      block = (OptimizedGUIBlock) null;
      height = -1f;
      return false;
    }

    internal virtual bool OnOptimizedInspectorGUI(Rect contentRect)
    {
      Debug.LogError((object) "Not supported");
      return false;
    }

    /// <summary>
    ///   <para>Repaint any inspectors that shows this editor.</para>
    /// </summary>
    public void Repaint()
    {
      InspectorWindow.RepaintAllInspectors();
    }

    /// <summary>
    ///   <para>Override this method in subclasses if you implement OnPreviewGUI.</para>
    /// </summary>
    /// <returns>
    ///   <para>True if this component can be Previewed in its current state.</para>
    /// </returns>
    public virtual bool HasPreviewGUI()
    {
      return this.preview.HasPreviewGUI();
    }

    /// <summary>
    ///   <para>Override this method if you want to change the label of the Preview area.</para>
    /// </summary>
    public virtual GUIContent GetPreviewTitle()
    {
      return this.preview.GetPreviewTitle();
    }

    /// <summary>
    ///   <para>Override this method if you want to render a static preview that shows.</para>
    /// </summary>
    /// <param name="assetPath"></param>
    /// <param name="subAssets"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public virtual Texture2D RenderStaticPreview(string assetPath, UnityEngine.Object[] subAssets, int width, int height)
    {
      return (Texture2D) null;
    }

    /// <summary>
    ///   <para>Implement to create your own custom preview for the preview area of the inspector, primary editor headers and the object selector.</para>
    /// </summary>
    /// <param name="r">Rectangle in which to draw the preview.</param>
    /// <param name="background">Background image.</param>
    public virtual void OnPreviewGUI(Rect r, GUIStyle background)
    {
      this.preview.OnPreviewGUI(r, background);
    }

    /// <summary>
    ///   <para>Implement to create your own interactive custom preview. Interactive custom previews are used in the preview area of the inspector and the object selector.</para>
    /// </summary>
    /// <param name="r">Rectangle in which to draw the preview.</param>
    /// <param name="background">Background image.</param>
    public virtual void OnInteractivePreviewGUI(Rect r, GUIStyle background)
    {
      this.OnPreviewGUI(r, background);
    }

    /// <summary>
    ///   <para>Override this method if you want to show custom controls in the preview header.</para>
    /// </summary>
    public virtual void OnPreviewSettings()
    {
      this.preview.OnPreviewSettings();
    }

    /// <summary>
    ///   <para>Implement this method to show asset information on top of the asset preview.</para>
    /// </summary>
    public virtual string GetInfoString()
    {
      return this.preview.GetInfoString();
    }

    internal virtual void OnAssetStoreInspectorGUI()
    {
    }

    public virtual void ReloadPreviewInstances()
    {
      this.preview.ReloadPreviewInstances();
    }

    internal static bool DoDrawDefaultInspector(SerializedObject obj)
    {
      EditorGUI.BeginChangeCheck();
      obj.Update();
      SerializedProperty iterator = obj.GetIterator();
      for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
      {
        EditorGUI.BeginDisabledGroup("m_Script" == iterator.propertyPath);
        EditorGUILayout.PropertyField(iterator, true, new GUILayoutOption[0]);
        EditorGUI.EndDisabledGroup();
      }
      obj.ApplyModifiedProperties();
      return EditorGUI.EndChangeCheck();
    }

    internal bool DoDrawDefaultInspector()
    {
      return Editor.DoDrawDefaultInspector(this.serializedObject);
    }

    /// <summary>
    ///   <para>Call this function to draw the header of the editor.</para>
    /// </summary>
    public void DrawHeader()
    {
      if (EditorGUIUtility.hierarchyMode)
        this.DrawHeaderFromInsideHierarchy();
      else
        this.OnHeaderGUI();
    }

    protected virtual void OnHeaderGUI()
    {
      Editor.DrawHeaderGUI(this, this.targetTitle);
    }

    internal virtual void OnHeaderControlsGUI()
    {
      GUILayoutUtility.GetRect(10f, 10f, 16f, 16f, EditorStyles.layerMaskField);
      GUILayout.FlexibleSpace();
      bool flag = true;
      if (!(this is AssetImporterInspector))
      {
        if (!AssetDatabase.IsMainAsset(this.targets[0]))
          flag = false;
        AssetImporter atPath = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(this.targets[0]));
        if ((bool) ((UnityEngine.Object) atPath) && atPath.GetType() != typeof (AssetImporter))
          flag = false;
      }
      if (!flag || !GUILayout.Button("Open", EditorStyles.miniButton, new GUILayoutOption[0]))
        return;
      if (this is AssetImporterInspector)
        AssetDatabase.OpenAsset((this as AssetImporterInspector).assetEditor.targets);
      else
        AssetDatabase.OpenAsset(this.targets);
      GUIUtility.ExitGUI();
    }

    internal virtual void OnHeaderIconGUI(Rect iconRect)
    {
      if (Editor.s_Styles == null)
        Editor.s_Styles = new Editor.Styles();
      Texture2D texture2D = (Texture2D) null;
      if (!this.HasPreviewGUI())
      {
        bool flag = AssetPreview.IsLoadingAssetPreview(this.target.GetInstanceID());
        texture2D = AssetPreview.GetAssetPreview(this.target);
        if (!(bool) ((UnityEngine.Object) texture2D))
        {
          if (flag)
            this.Repaint();
          texture2D = AssetPreview.GetMiniThumbnail(this.target);
        }
      }
      if (this.HasPreviewGUI())
      {
        this.OnPreviewGUI(iconRect, Editor.s_Styles.inspectorBigInner);
      }
      else
      {
        if (!(bool) ((UnityEngine.Object) texture2D))
          return;
        GUI.Label(iconRect, (Texture) texture2D, Editor.s_Styles.centerStyle);
      }
    }

    internal virtual void OnHeaderTitleGUI(Rect titleRect, string header)
    {
      titleRect.yMin -= 2f;
      titleRect.yMax += 2f;
      GUI.Label(titleRect, header, EditorStyles.largeLabel);
    }

    internal virtual void DrawHeaderHelpAndSettingsGUI(Rect r)
    {
      UnityEngine.Object target = this.target;
      float num = 18f;
      if (this.IsEnabled())
      {
        Rect position = new Rect(r.xMax - num, r.y + 5f, 14f, 14f);
        if (EditorGUI.ButtonMouseDown(position, EditorGUI.GUIContents.titleSettingsIcon, FocusType.Native, EditorStyles.inspectorTitlebarText))
          EditorUtility.DisplayObjectContextMenu(position, this.targets, 0);
        num += 18f;
      }
      EditorGUI.HelpIconButton(new Rect(r.xMax - num, r.y + 5f, 14f, 14f), target);
    }

    private void DrawHeaderFromInsideHierarchy()
    {
      GUIStyle style = GUILayoutUtility.current.topLevel.style;
      EditorGUILayout.EndVertical();
      this.OnHeaderGUI();
      EditorGUILayout.BeginVertical(style, new GUILayoutOption[0]);
    }

    internal static Rect DrawHeaderGUI(Editor editor, string header)
    {
      return Editor.DrawHeaderGUI(editor, header, 0.0f);
    }

    internal static Rect DrawHeaderGUI(Editor editor, string header, float leftMargin)
    {
      if (Editor.s_Styles == null)
        Editor.s_Styles = new Editor.Styles();
      GUILayout.BeginHorizontal(Editor.s_Styles.inspectorBig, new GUILayoutOption[0]);
      GUILayout.Space(38f);
      GUILayout.BeginVertical();
      GUILayout.Space(19f);
      GUILayout.BeginHorizontal();
      if ((double) leftMargin > 0.0)
        GUILayout.Space(leftMargin);
      if ((bool) ((UnityEngine.Object) editor))
        editor.OnHeaderControlsGUI();
      else
        EditorGUILayout.GetControlRect();
      GUILayout.EndHorizontal();
      GUILayout.EndVertical();
      GUILayout.EndHorizontal();
      Rect lastRect = GUILayoutUtility.GetLastRect();
      Rect r = new Rect(lastRect.x + leftMargin, lastRect.y, lastRect.width - leftMargin, lastRect.height);
      Rect rect1 = new Rect(r.x + 6f, r.y + 6f, 32f, 32f);
      if ((bool) ((UnityEngine.Object) editor))
        editor.OnHeaderIconGUI(rect1);
      else
        GUI.Label(rect1, (Texture) AssetPreview.GetMiniTypeThumbnail(typeof (UnityEngine.Object)), Editor.s_Styles.centerStyle);
      Rect rect2 = new Rect(r.x + 44f, r.y + 6f, (float) ((double) r.width - 44.0 - 38.0 - 4.0), 16f);
      if ((bool) ((UnityEngine.Object) editor))
        editor.OnHeaderTitleGUI(rect2, header);
      else
        GUI.Label(rect2, header, EditorStyles.largeLabel);
      if ((bool) ((UnityEngine.Object) editor))
        editor.DrawHeaderHelpAndSettingsGUI(r);
      Event current = Event.current;
      if ((UnityEngine.Object) editor != (UnityEngine.Object) null && !editor.IsEnabled() && (current.type == EventType.MouseDown && current.button == 1) && r.Contains(current.mousePosition))
      {
        EditorUtility.DisplayObjectContextMenu(new Rect(current.mousePosition.x, current.mousePosition.y, 0.0f, 0.0f), editor.targets, 0);
        current.Use();
      }
      return lastRect;
    }

    /// <summary>
    ///   <para>The first entry point for Preview Drawing.</para>
    /// </summary>
    /// <param name="previewPosition">The available area to draw the preview.</param>
    /// <param name="previewArea"></param>
    public virtual void DrawPreview(Rect previewArea)
    {
      ObjectPreview.DrawPreview((IPreviewable) this, previewArea, this.targets);
    }

    internal bool CanBeExpandedViaAFoldout()
    {
      if (this.m_SerializedObject == null)
        this.m_SerializedObject = new SerializedObject(this.targets);
      else
        this.m_SerializedObject.Update();
      this.m_SerializedObject.inspectorMode = this.m_InspectorMode;
      SerializedProperty iterator = this.m_SerializedObject.GetIterator();
      for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
      {
        if ((double) EditorGUI.GetPropertyHeight(iterator, (GUIContent) null, true) > 0.0)
          return true;
      }
      return false;
    }

    internal static bool IsAppropriateFileOpenForEdit(UnityEngine.Object assetObject)
    {
      string message;
      return Editor.IsAppropriateFileOpenForEdit(assetObject, out message);
    }

    internal static bool IsAppropriateFileOpenForEdit(UnityEngine.Object assetObject, out string message)
    {
      message = string.Empty;
      if (AssetDatabase.IsNativeAsset(assetObject))
      {
        if (!AssetDatabase.IsOpenForEdit(assetObject, out message))
          return false;
      }
      else if (AssetDatabase.IsForeignAsset(assetObject) && !AssetDatabase.IsMetaFileOpenForEdit(assetObject, out message))
        return false;
      return true;
    }

    internal virtual bool IsEnabled()
    {
      foreach (UnityEngine.Object target in this.targets)
      {
        if ((target.hideFlags & HideFlags.NotEditable) != HideFlags.None || EditorUtility.IsPersistent(target) && !Editor.IsAppropriateFileOpenForEdit(target))
          return false;
      }
      return true;
    }

    internal bool IsOpenForEdit()
    {
      string message;
      return this.IsOpenForEdit(out message);
    }

    internal bool IsOpenForEdit(out string message)
    {
      message = string.Empty;
      foreach (UnityEngine.Object target in this.targets)
      {
        if (EditorUtility.IsPersistent(target) && !Editor.IsAppropriateFileOpenForEdit(target))
          return false;
      }
      return true;
    }

    /// <summary>
    ///   <para>Override this method in subclasses to return false if you don't want default margins.</para>
    /// </summary>
    public virtual bool UseDefaultMargins()
    {
      return true;
    }

    public void Initialize(UnityEngine.Object[] targets)
    {
      throw new InvalidOperationException("You shouldn't call Initialize for Editors");
    }

    public bool MoveNextTarget()
    {
      ++this.referenceTargetIndex;
      return this.referenceTargetIndex < this.targets.Length;
    }

    public void ResetTarget()
    {
      this.referenceTargetIndex = 0;
    }

    private class Styles
    {
      public GUIStyle inspectorBig = new GUIStyle(EditorStyles.inspectorBig);
      public GUIStyle inspectorBigInner = new GUIStyle((GUIStyle) "IN BigTitle inner");
      public GUIStyle centerStyle = new GUIStyle();

      public Styles()
      {
        this.centerStyle.alignment = TextAnchor.MiddleCenter;
        --this.inspectorBig.padding.bottom;
      }
    }
  }
}
