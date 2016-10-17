// Decompiled with JetBrains decompiler
// Type: UnityEditor.AvatarEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityEditor
{
  [CustomEditor(typeof (Avatar))]
  internal class AvatarEditor : Editor
  {
    private const int sMappingTab = 0;
    private const int sMuscleTab = 1;
    private const int sHandleTab = 2;
    private const int sColliderTab = 3;
    private const int sDefaultTab = 0;
    private static AvatarEditor.Styles s_Styles;
    protected int m_TabIndex;
    internal GameObject m_GameObject;
    internal Dictionary<Transform, bool> m_ModelBones;
    private AvatarEditor.EditMode m_EditMode;
    internal bool m_CameFromImportSettings;
    private bool m_SwitchToEditMode;
    internal static bool s_EditImmediatelyOnNextOpen;
    private SceneSetup[] sceneSetup;
    protected bool m_InspectorLocked;
    protected List<AvatarEditor.SceneStateCache> m_SceneStates;
    private AvatarMuscleEditor m_MuscleEditor;
    private AvatarHandleEditor m_HandleEditor;
    private AvatarColliderEditor m_ColliderEditor;
    private AvatarMappingEditor m_MappingEditor;
    private GameObject m_PrefabInstance;

    private static AvatarEditor.Styles styles
    {
      get
      {
        if (AvatarEditor.s_Styles == null)
          AvatarEditor.s_Styles = new AvatarEditor.Styles();
        return AvatarEditor.s_Styles;
      }
    }

    internal Avatar avatar
    {
      get
      {
        return this.target as Avatar;
      }
    }

    protected AvatarSubEditor editor
    {
      get
      {
        switch (this.m_TabIndex)
        {
          case 1:
            return (AvatarSubEditor) this.m_MuscleEditor;
          case 2:
            return (AvatarSubEditor) this.m_HandleEditor;
          case 3:
            return (AvatarSubEditor) this.m_ColliderEditor;
          default:
            return (AvatarSubEditor) this.m_MappingEditor;
        }
      }
      set
      {
        switch (this.m_TabIndex)
        {
          case 1:
            this.m_MuscleEditor = value as AvatarMuscleEditor;
            break;
          case 2:
            this.m_HandleEditor = value as AvatarHandleEditor;
            break;
          case 3:
            this.m_ColliderEditor = value as AvatarColliderEditor;
            break;
          default:
            this.m_MappingEditor = value as AvatarMappingEditor;
            break;
        }
      }
    }

    public GameObject prefab
    {
      get
      {
        return AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GetAssetPath(this.target)) as GameObject;
      }
    }

    internal override SerializedObject GetSerializedObjectInternal()
    {
      if (this.m_SerializedObject == null)
        this.m_SerializedObject = SerializedObject.LoadFromCache(this.GetInstanceID());
      if (this.m_SerializedObject == null)
        this.m_SerializedObject = new SerializedObject((UnityEngine.Object) AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(this.target)));
      return this.m_SerializedObject;
    }

    private void OnEnable()
    {
      EditorApplication.update += new EditorApplication.CallbackFunction(this.Update);
      this.m_SwitchToEditMode = false;
      if (this.m_EditMode == AvatarEditor.EditMode.Editing)
      {
        this.m_ModelBones = AvatarSetupTool.GetModelBones(this.m_GameObject.transform, false, (AvatarSetupTool.BoneWrapper[]) null);
        this.editor.Enable(this);
      }
      else
      {
        if (this.m_EditMode != AvatarEditor.EditMode.NotEditing)
          return;
        this.editor = (AvatarSubEditor) null;
        if (!AvatarEditor.s_EditImmediatelyOnNextOpen)
          return;
        this.m_CameFromImportSettings = true;
        AvatarEditor.s_EditImmediatelyOnNextOpen = false;
      }
    }

    private void OnDisable()
    {
      if (this.m_EditMode == AvatarEditor.EditMode.Editing)
        this.editor.Disable();
      EditorApplication.update -= new EditorApplication.CallbackFunction(this.Update);
      if (this.m_SerializedObject == null)
        return;
      this.m_SerializedObject.Cache(this.GetInstanceID());
      this.m_SerializedObject = (SerializedObject) null;
    }

    private void OnDestroy()
    {
      if (this.m_EditMode != AvatarEditor.EditMode.Editing)
        return;
      this.SwitchToAssetMode();
    }

    private void SelectAsset()
    {
      Selection.activeObject = !this.m_CameFromImportSettings ? this.target : AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GetAssetPath(this.target));
    }

    protected void CreateEditor()
    {
      switch (this.m_TabIndex)
      {
        case 1:
          this.editor = (AvatarSubEditor) ScriptableObject.CreateInstance<AvatarMuscleEditor>();
          break;
        case 2:
          this.editor = (AvatarSubEditor) ScriptableObject.CreateInstance<AvatarHandleEditor>();
          break;
        case 3:
          this.editor = (AvatarSubEditor) ScriptableObject.CreateInstance<AvatarColliderEditor>();
          break;
        default:
          this.editor = (AvatarSubEditor) ScriptableObject.CreateInstance<AvatarMappingEditor>();
          break;
      }
      this.editor.hideFlags = HideFlags.HideAndDontSave;
      this.editor.Enable(this);
    }

    protected void DestroyEditor()
    {
      this.editor.OnDestroy();
      this.editor = (AvatarSubEditor) null;
    }

    public override bool UseDefaultMargins()
    {
      return false;
    }

    public override void OnInspectorGUI()
    {
      GUI.enabled = true;
      EditorGUILayout.BeginVertical(EditorStyles.inspectorFullWidthMargins, new GUILayoutOption[0]);
      if (this.m_EditMode == AvatarEditor.EditMode.Editing)
        this.EditingGUI();
      else if (!this.m_CameFromImportSettings)
        this.EditButtonGUI();
      else if (this.m_EditMode == AvatarEditor.EditMode.NotEditing && Event.current.type == EventType.Repaint)
        this.m_SwitchToEditMode = true;
      EditorGUILayout.EndVertical();
    }

    private void EditButtonGUI()
    {
      if ((UnityEngine.Object) this.avatar == (UnityEngine.Object) null || !this.avatar.isHuman || (UnityEngine.Object) (AssetImporter.GetAtPath(AssetDatabase.GetAssetPath((UnityEngine.Object) this.avatar)) as ModelImporter) == (UnityEngine.Object) null)
        return;
      EditorGUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      if (GUILayout.Button(AvatarEditor.styles.editCharacter, new GUILayoutOption[1]{ GUILayout.Width(120f) }) && EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
      {
        this.SwitchToEditMode();
        GUIUtility.ExitGUI();
      }
      GUILayout.FlexibleSpace();
      EditorGUILayout.EndHorizontal();
    }

    private void EditingGUI()
    {
      GUILayout.BeginHorizontal();
      int tabIndex = this.m_TabIndex;
      bool enabled = GUI.enabled;
      GUI.enabled = ((UnityEngine.Object) this.avatar == (UnityEngine.Object) null ? 1 : (!this.avatar.isHuman ? 1 : 0)) == 0;
      int num = GUILayout.Toolbar(tabIndex, AvatarEditor.styles.tabs);
      GUI.enabled = enabled;
      if (num != this.m_TabIndex)
      {
        this.DestroyEditor();
        this.m_TabIndex = num;
        this.CreateEditor();
      }
      GUILayout.EndHorizontal();
      this.editor.OnInspectorGUI();
    }

    public void OnSceneGUI()
    {
      if (this.m_EditMode != AvatarEditor.EditMode.Editing)
        return;
      this.editor.OnSceneGUI();
    }

    internal void SwitchToEditMode()
    {
      this.m_EditMode = AvatarEditor.EditMode.Starting;
      this.ChangeInspectorLock(true);
      this.sceneSetup = EditorSceneManager.GetSceneManagerSetup();
      EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);
      this.m_GameObject = UnityEngine.Object.Instantiate<GameObject>(this.prefab);
      if (this.serializedObject.FindProperty("m_OptimizeGameObjects").boolValue)
        AnimatorUtility.DeoptimizeTransformHierarchy(this.m_GameObject);
      Animator component = this.m_GameObject.GetComponent<Animator>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && (UnityEngine.Object) component.runtimeAnimatorController == (UnityEngine.Object) null)
      {
        AnimatorController animatorController = new AnimatorController();
        animatorController.hideFlags = HideFlags.DontSave;
        animatorController.AddLayer("preview");
        animatorController.layers[0].stateMachine.hideFlags = HideFlags.DontSave;
        component.runtimeAnimatorController = (RuntimeAnimatorController) animatorController;
      }
      this.m_ModelBones = AvatarSetupTool.GetModelBones(this.m_GameObject.transform, false, AvatarSetupTool.GetHumanBones(this.serializedObject, AvatarSetupTool.GetModelBones(this.m_GameObject.transform, true, (AvatarSetupTool.BoneWrapper[]) null)));
      Selection.activeObject = (UnityEngine.Object) this.m_GameObject;
      foreach (SceneHierarchyWindow sceneHierarchyWindow in Resources.FindObjectsOfTypeAll(typeof (SceneHierarchyWindow)))
        sceneHierarchyWindow.SetExpandedRecursive(this.m_GameObject.GetInstanceID(), true);
      this.CreateEditor();
      this.m_EditMode = AvatarEditor.EditMode.Editing;
      this.m_SceneStates = new List<AvatarEditor.SceneStateCache>();
      foreach (SceneView sceneView in SceneView.sceneViews)
      {
        this.m_SceneStates.Add(new AvatarEditor.SceneStateCache()
        {
          state = new SceneView.SceneViewState(sceneView.m_SceneViewState),
          view = sceneView
        });
        sceneView.m_SceneViewState.showFlares = false;
        sceneView.m_SceneViewState.showMaterialUpdate = false;
        sceneView.m_SceneViewState.showFog = false;
        sceneView.m_SceneViewState.showSkybox = false;
        sceneView.FrameSelected();
      }
    }

    internal void SwitchToAssetMode()
    {
      using (List<AvatarEditor.SceneStateCache>.Enumerator enumerator = this.m_SceneStates.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AvatarEditor.SceneStateCache current = enumerator.Current;
          if (!((UnityEngine.Object) current.view == (UnityEngine.Object) null))
          {
            current.view.m_SceneViewState.showFog = current.state.showFog;
            current.view.m_SceneViewState.showFlares = current.state.showFlares;
            current.view.m_SceneViewState.showMaterialUpdate = current.state.showMaterialUpdate;
            current.view.m_SceneViewState.showSkybox = current.state.showSkybox;
          }
        }
      }
      this.m_EditMode = AvatarEditor.EditMode.Stopping;
      this.DestroyEditor();
      this.ChangeInspectorLock(this.m_InspectorLocked);
      if (!EditorApplication.isUpdating && !Unsupported.IsDestroyScriptableObject((ScriptableObject) this))
      {
        if (SceneManager.GetActiveScene().path.Length <= 0)
        {
          if (this.sceneSetup != null && this.sceneSetup.Length > 0)
          {
            EditorSceneManager.RestoreSceneManagerSetup(this.sceneSetup);
            this.sceneSetup = (SceneSetup[]) null;
          }
          else
            EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);
        }
      }
      else if (Unsupported.IsDestroyScriptableObject((ScriptableObject) this))
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AvatarEditor.\u003CSwitchToAssetMode\u003Ec__AnonStorey9C modeCAnonStorey9C = new AvatarEditor.\u003CSwitchToAssetMode\u003Ec__AnonStorey9C();
        // ISSUE: reference to a compiler-generated field
        modeCAnonStorey9C.\u003C\u003Ef__this = this;
        // ISSUE: reference to a compiler-generated field
        modeCAnonStorey9C.CleanUpSceneOnDestroy = (EditorApplication.CallbackFunction) null;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        modeCAnonStorey9C.CleanUpSceneOnDestroy = new EditorApplication.CallbackFunction(modeCAnonStorey9C.\u003C\u003Em__1C9);
        // ISSUE: reference to a compiler-generated field
        EditorApplication.update += modeCAnonStorey9C.CleanUpSceneOnDestroy;
      }
      this.m_GameObject = (GameObject) null;
      this.m_ModelBones = (Dictionary<Transform, bool>) null;
      this.SelectAsset();
      if (this.m_CameFromImportSettings)
        return;
      this.m_EditMode = AvatarEditor.EditMode.NotEditing;
    }

    private void ChangeInspectorLock(bool locked)
    {
      foreach (InspectorWindow allInspectorWindow in InspectorWindow.GetAllInspectorWindows())
      {
        foreach (UnityEngine.Object activeEditor in allInspectorWindow.GetTracker().activeEditors)
        {
          if (activeEditor == (UnityEngine.Object) this)
          {
            this.m_InspectorLocked = allInspectorWindow.isLocked;
            allInspectorWindow.isLocked = locked;
          }
        }
      }
    }

    public void Update()
    {
      if (this.m_SwitchToEditMode)
      {
        this.m_SwitchToEditMode = false;
        this.SwitchToEditMode();
        this.Repaint();
      }
      if (this.m_EditMode != AvatarEditor.EditMode.Editing)
        return;
      if ((UnityEngine.Object) this.m_GameObject == (UnityEngine.Object) null || this.m_ModelBones == null)
        this.SwitchToAssetMode();
      else if (EditorApplication.isPlaying)
      {
        this.SwitchToAssetMode();
      }
      else
      {
        if (this.m_ModelBones == null)
          return;
        using (Dictionary<Transform, bool>.Enumerator enumerator = this.m_ModelBones.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            if ((UnityEngine.Object) enumerator.Current.Key == (UnityEngine.Object) null)
            {
              this.SwitchToAssetMode();
              break;
            }
          }
        }
      }
    }

    public bool HasFrameBounds()
    {
      if (this.m_ModelBones != null)
      {
        using (Dictionary<Transform, bool>.Enumerator enumerator = this.m_ModelBones.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            if ((UnityEngine.Object) enumerator.Current.Key == (UnityEngine.Object) Selection.activeTransform)
              return true;
          }
        }
      }
      return false;
    }

    public Bounds OnGetFrameBounds()
    {
      Transform activeTransform = Selection.activeTransform;
      Bounds bounds = new Bounds(activeTransform.position, new Vector3(0.0f, 0.0f, 0.0f));
      foreach (Transform transform in activeTransform)
        bounds.Encapsulate(transform.position);
      if ((bool) ((UnityEngine.Object) activeTransform.parent))
        bounds.Encapsulate(activeTransform.parent.position);
      return bounds;
    }

    private class Styles
    {
      public GUIContent[] tabs = new GUIContent[2]{ EditorGUIUtility.TextContent("Mapping"), EditorGUIUtility.TextContent("Muscles & Settings") };
      public GUIContent editCharacter = EditorGUIUtility.TextContent("Configure Avatar");
      public GUIContent reset = EditorGUIUtility.TextContent("Reset");
    }

    private enum EditMode
    {
      NotEditing,
      Starting,
      Editing,
      Stopping,
    }

    [Serializable]
    protected class SceneStateCache
    {
      public SceneView view;
      public SceneView.SceneViewState state;
    }
  }
}
