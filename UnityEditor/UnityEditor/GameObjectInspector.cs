// Decompiled with JetBrains decompiler
// Type: UnityEditor.GameObjectInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEditor.VersionControl;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (GameObject))]
  [CanEditMultipleObjects]
  internal class GameObjectInspector : Editor
  {
    private bool m_AllOfSamePrefabType = true;
    private const float kTop = 4f;
    private const float kTop2 = 24f;
    private const float kTop3 = 44f;
    private const float kIconSize = 24f;
    private const float kLeft = 52f;
    private const float kToggleSize = 14f;
    private SerializedProperty m_Name;
    private SerializedProperty m_IsActive;
    private SerializedProperty m_Layer;
    private SerializedProperty m_Tag;
    private SerializedProperty m_StaticEditorFlags;
    private SerializedProperty m_Icon;
    private static GameObjectInspector.Styles s_styles;
    private Vector2 previewDir;
    private PreviewRenderUtility m_PreviewUtility;
    private List<GameObject> m_PreviewInstances;
    private bool m_HasInstance;
    public static GameObject dragObject;

    private GameObjectInspector()
    {
      if (EditorSettings.defaultBehaviorMode == EditorBehaviorMode.Mode2D)
        this.previewDir = new Vector2(0.0f, 0.0f);
      else
        this.previewDir = new Vector2(120f, -20f);
    }

    public void OnEnable()
    {
      this.m_Name = this.serializedObject.FindProperty("m_Name");
      this.m_IsActive = this.serializedObject.FindProperty("m_IsActive");
      this.m_Layer = this.serializedObject.FindProperty("m_Layer");
      this.m_Tag = this.serializedObject.FindProperty("m_TagString");
      this.m_StaticEditorFlags = this.serializedObject.FindProperty("m_StaticEditorFlags");
      this.m_Icon = this.serializedObject.FindProperty("m_Icon");
      this.CalculatePrefabStatus();
    }

    private void CalculatePrefabStatus()
    {
      this.m_HasInstance = false;
      this.m_AllOfSamePrefabType = true;
      PrefabType prefabType1 = PrefabUtility.GetPrefabType((UnityEngine.Object) (this.targets[0] as GameObject));
      foreach (UnityEngine.Object target in this.targets)
      {
        PrefabType prefabType2 = PrefabUtility.GetPrefabType(target);
        if (prefabType2 != prefabType1)
          this.m_AllOfSamePrefabType = false;
        if (prefabType2 != PrefabType.None && prefabType2 != PrefabType.Prefab && prefabType2 != PrefabType.ModelPrefab)
          this.m_HasInstance = true;
      }
    }

    private void OnDisable()
    {
    }

    private static bool ShowMixedStaticEditorFlags(StaticEditorFlags mask)
    {
      uint num1 = 0;
      uint num2 = 0;
      foreach (object obj in Enum.GetValues(typeof (StaticEditorFlags)))
      {
        ++num2;
        if ((mask & (StaticEditorFlags) obj) > (StaticEditorFlags) 0)
          ++num1;
      }
      if (num1 > 0U)
        return (int) num1 != (int) num2;
      return false;
    }

    protected override void OnHeaderGUI()
    {
      this.DrawInspector(GUILayoutUtility.GetRect(0.0f, !this.m_HasInstance ? 40f : 60f));
    }

    public override void OnInspectorGUI()
    {
    }

    internal bool DrawInspector(Rect contentRect)
    {
      if (GameObjectInspector.s_styles == null)
        GameObjectInspector.s_styles = new GameObjectInspector.Styles();
      this.serializedObject.Update();
      GameObject target1 = this.target as GameObject;
      EditorGUIUtility.labelWidth = 52f;
      bool enabled1 = GUI.enabled;
      GUI.enabled = true;
      GUI.Label(new Rect(contentRect.x, contentRect.y, contentRect.width, contentRect.height + 3f), GUIContent.none, EditorStyles.inspectorBig);
      GUI.enabled = enabled1;
      float width1 = contentRect.width;
      float y = contentRect.y;
      GUIContent guiContent = (GUIContent) null;
      PrefabType prefabType = PrefabType.None;
      if (this.m_AllOfSamePrefabType)
      {
        prefabType = PrefabUtility.GetPrefabType((UnityEngine.Object) target1);
        switch (prefabType)
        {
          case PrefabType.None:
            guiContent = GameObjectInspector.s_styles.goIcon;
            break;
          case PrefabType.Prefab:
          case PrefabType.PrefabInstance:
          case PrefabType.DisconnectedPrefabInstance:
            guiContent = GameObjectInspector.s_styles.prefabIcon;
            break;
          case PrefabType.ModelPrefab:
          case PrefabType.ModelPrefabInstance:
          case PrefabType.DisconnectedModelPrefabInstance:
            guiContent = GameObjectInspector.s_styles.modelIcon;
            break;
          case PrefabType.MissingPrefabInstance:
            guiContent = GameObjectInspector.s_styles.prefabIcon;
            break;
        }
      }
      else
        guiContent = GameObjectInspector.s_styles.typelessIcon;
      EditorGUI.ObjectIconDropDown(new Rect(3f, 4f + y, 24f, 24f), this.targets, true, guiContent.image as Texture2D, this.m_Icon);
      EditorGUI.BeginDisabledGroup(prefabType == PrefabType.ModelPrefab);
      EditorGUI.PropertyField(new Rect(34f, 4f + y, 14f, 14f), this.m_IsActive, GUIContent.none);
      float num1 = GameObjectInspector.s_styles.staticFieldToggleWidth + 15f;
      float width2 = (float) ((double) width1 - 52.0 - (double) num1 - 5.0);
      EditorGUI.DelayedTextField(new Rect(52f, (float) (4.0 + (double) y + 1.0), width2, 16f), this.m_Name, GUIContent.none);
      Rect totalPosition = new Rect(width1 - num1, 4f + y, GameObjectInspector.s_styles.staticFieldToggleWidth, 16f);
      EditorGUI.BeginProperty(totalPosition, GUIContent.none, this.m_StaticEditorFlags);
      EditorGUI.BeginChangeCheck();
      Rect position1 = totalPosition;
      EditorGUI.showMixedValue |= GameObjectInspector.ShowMixedStaticEditorFlags((StaticEditorFlags) this.m_StaticEditorFlags.intValue);
      Event current = Event.current;
      EventType type = current.type;
      bool flag = current.type == EventType.MouseDown && current.button != 0;
      if (flag)
        current.type = EventType.Ignore;
      bool flagValue = EditorGUI.ToggleLeft(position1, "Static", target1.isStatic);
      if (flag)
        current.type = type;
      EditorGUI.showMixedValue = false;
      if (EditorGUI.EndChangeCheck())
      {
        SceneModeUtility.SetStaticFlags(this.targets, -1, flagValue);
        this.serializedObject.SetIsDifferentCacheDirty();
      }
      EditorGUI.EndProperty();
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = this.m_StaticEditorFlags.hasMultipleDifferentValues;
      int changedFlags;
      bool changedToValue;
      EditorGUI.EnumMaskField(new Rect(totalPosition.x + GameObjectInspector.s_styles.staticFieldToggleWidth, totalPosition.y, 10f, 14f), (Enum) GameObjectUtility.GetStaticEditorFlags(target1), GameObjectInspector.s_styles.staticDropdown, out changedFlags, out changedToValue);
      EditorGUI.showMixedValue = false;
      if (EditorGUI.EndChangeCheck())
      {
        SceneModeUtility.SetStaticFlags(this.targets, changedFlags, changedToValue);
        this.serializedObject.SetIsDifferentCacheDirty();
      }
      float num2 = 4f;
      float num3 = 4f;
      EditorGUIUtility.fieldWidth = (float) (((double) width1 - (double) num2 - 52.0 - (double) GameObjectInspector.s_styles.layerFieldWidth - (double) num3) / 2.0);
      string tag;
      try
      {
        tag = target1.tag;
      }
      catch (Exception ex)
      {
        tag = "Undefined";
      }
      EditorGUIUtility.labelWidth = GameObjectInspector.s_styles.tagFieldWidth;
      Rect rect = new Rect(52f - EditorGUIUtility.labelWidth, 24f + y, EditorGUIUtility.labelWidth + EditorGUIUtility.fieldWidth, 16f);
      EditorGUI.BeginProperty(rect, GUIContent.none, this.m_Tag);
      EditorGUI.BeginChangeCheck();
      string str = EditorGUI.TagField(rect, EditorGUIUtility.TempContent("Tag"), tag);
      if (EditorGUI.EndChangeCheck())
      {
        this.m_Tag.stringValue = str;
        Undo.RecordObjects(this.targets, "Change Tag of " + this.targetTitle);
        foreach (UnityEngine.Object target2 in this.targets)
          (target2 as GameObject).tag = str;
      }
      EditorGUI.EndProperty();
      EditorGUIUtility.labelWidth = GameObjectInspector.s_styles.layerFieldWidth;
      rect = new Rect(52f + EditorGUIUtility.fieldWidth + num2, 24f + y, EditorGUIUtility.labelWidth + EditorGUIUtility.fieldWidth, 16f);
      EditorGUI.BeginProperty(rect, GUIContent.none, this.m_Layer);
      EditorGUI.BeginChangeCheck();
      int layer = EditorGUI.LayerField(rect, EditorGUIUtility.TempContent("Layer"), target1.layer);
      if (EditorGUI.EndChangeCheck())
      {
        GameObjectUtility.ShouldIncludeChildren shouldIncludeChildren = GameObjectUtility.DisplayUpdateChildrenDialogIfNeeded(this.targets.OfType<GameObject>(), "Change Layer", "Do you want to set layer to " + InternalEditorUtility.GetLayerName(layer) + " for all child objects as well?");
        if (shouldIncludeChildren != GameObjectUtility.ShouldIncludeChildren.Cancel)
        {
          this.m_Layer.intValue = layer;
          this.SetLayer(layer, shouldIncludeChildren == GameObjectUtility.ShouldIncludeChildren.IncludeChildren);
        }
      }
      EditorGUI.EndProperty();
      if (this.m_HasInstance && !EditorApplication.isPlayingOrWillChangePlaymode)
      {
        float width3 = (float) (((double) width1 - 52.0 - 5.0) / 3.0);
        Rect position2 = new Rect((float) (52.0 + (double) width3 * 0.0), 44f + y, width3, 15f);
        Rect position3 = new Rect((float) (52.0 + (double) width3 * 1.0), 44f + y, width3, 15f);
        Rect position4 = new Rect((float) (52.0 + (double) width3 * 2.0), 44f + y, width3, 15f);
        Rect position5 = new Rect(52f, 44f + y, width3 * 3f, 15f);
        GUIContent content = this.targets.Length <= 1 ? GameObjectInspector.s_styles.goTypeLabel[(int) prefabType] : GameObjectInspector.s_styles.goTypeLabelMultiple;
        if (content != null)
        {
          float x = GUI.skin.label.CalcSize(content).x;
          if (prefabType == PrefabType.DisconnectedModelPrefabInstance || prefabType == PrefabType.MissingPrefabInstance || prefabType == PrefabType.DisconnectedPrefabInstance)
          {
            GUI.contentColor = GUI.skin.GetStyle("CN StatusWarn").normal.textColor;
            if (prefabType == PrefabType.MissingPrefabInstance)
              GUI.Label(new Rect(52f, 44f + y, (float) ((double) width1 - 52.0 - 5.0), 18f), content, EditorStyles.whiteLabel);
            else
              GUI.Label(new Rect((float) (52.0 - (double) x - 5.0), 44f + y, (float) ((double) width1 - 52.0 - 5.0), 18f), content, EditorStyles.whiteLabel);
            GUI.contentColor = Color.white;
          }
          else
            GUI.Label(new Rect((float) (52.0 - (double) x - 5.0), 44f + y, x, 18f), content);
        }
        if (this.targets.Length > 1)
        {
          GUI.Label(position5, "Instance Management Disabled", GameObjectInspector.s_styles.instanceManagementInfo);
        }
        else
        {
          if (prefabType != PrefabType.MissingPrefabInstance && GUI.Button(position2, "Select", (GUIStyle) "MiniButtonLeft"))
          {
            Selection.activeObject = PrefabUtility.GetPrefabParent(this.target);
            EditorGUIUtility.PingObject(Selection.activeObject);
          }
          if ((prefabType == PrefabType.DisconnectedModelPrefabInstance || prefabType == PrefabType.DisconnectedPrefabInstance) && GUI.Button(position3, "Revert", (GUIStyle) "MiniButtonMid"))
          {
            Undo.RegisterFullObjectHierarchyUndo((UnityEngine.Object) target1, "Revert to prefab");
            PrefabUtility.ReconnectToLastPrefab(target1);
            PrefabUtility.RevertPrefabInstance(target1);
            this.CalculatePrefabStatus();
            Undo.RegisterCreatedObjectUndo((UnityEngine.Object) target1, "Reconnect prefab");
            GUIUtility.ExitGUI();
          }
          bool enabled2 = GUI.enabled;
          GUI.enabled = GUI.enabled && !AnimationMode.InAnimationMode();
          if ((prefabType == PrefabType.ModelPrefabInstance || prefabType == PrefabType.PrefabInstance) && GUI.Button(position3, "Revert", (GUIStyle) "MiniButtonMid"))
          {
            Undo.RegisterFullObjectHierarchyUndo((UnityEngine.Object) target1, "Revert Prefab Instance");
            PrefabUtility.RevertPrefabInstance(target1);
            this.CalculatePrefabStatus();
            Undo.RegisterCreatedObjectUndo((UnityEngine.Object) target1, "Revert prefab");
            GUIUtility.ExitGUI();
          }
          if (prefabType == PrefabType.PrefabInstance || prefabType == PrefabType.DisconnectedPrefabInstance)
          {
            GameObject prefabInstanceRoot = PrefabUtility.FindValidUploadPrefabInstanceRoot(target1);
            GUI.enabled = (UnityEngine.Object) prefabInstanceRoot != (UnityEngine.Object) null && !AnimationMode.InAnimationMode();
            if (GUI.Button(position4, "Apply", (GUIStyle) "MiniButtonRight"))
            {
              UnityEngine.Object prefabParent = PrefabUtility.GetPrefabParent((UnityEngine.Object) prefabInstanceRoot);
              if (Provider.PromptAndCheckoutIfNeeded(new string[1]{ AssetDatabase.GetAssetPath(prefabParent) }, "The version control requires you to check out the prefab before applying changes."))
              {
                PrefabUtility.ReplacePrefab(prefabInstanceRoot, prefabParent, ReplacePrefabOptions.ConnectToPrefab);
                this.CalculatePrefabStatus();
                EditorSceneManager.MarkSceneDirty(prefabInstanceRoot.scene);
                GUIUtility.ExitGUI();
              }
            }
          }
          GUI.enabled = enabled2;
          if ((prefabType == PrefabType.DisconnectedModelPrefabInstance || prefabType == PrefabType.ModelPrefabInstance) && GUI.Button(position4, "Open", (GUIStyle) "MiniButtonRight"))
          {
            AssetDatabase.OpenAsset(PrefabUtility.GetPrefabParent(this.target));
            GUIUtility.ExitGUI();
          }
        }
      }
      EditorGUI.EndDisabledGroup();
      this.serializedObject.ApplyModifiedProperties();
      return true;
    }

    private UnityEngine.Object[] GetObjects(bool includeChildren)
    {
      return (UnityEngine.Object[]) SceneModeUtility.GetObjects(this.targets, includeChildren);
    }

    private void SetLayer(int layer, bool includeChildren)
    {
      UnityEngine.Object[] objects = this.GetObjects(includeChildren);
      Undo.RecordObjects(objects, "Change Layer of " + this.targetTitle);
      foreach (GameObject gameObject in objects)
        gameObject.layer = layer;
    }

    public static void SetEnabledRecursive(GameObject go, bool enabled)
    {
      foreach (Renderer componentsInChild in go.GetComponentsInChildren<Renderer>())
        componentsInChild.enabled = enabled;
    }

    public override void ReloadPreviewInstances()
    {
      this.CreatePreviewInstances();
    }

    private void CreatePreviewInstances()
    {
      this.DestroyPreviewInstances();
      if (this.m_PreviewInstances == null)
        this.m_PreviewInstances = new List<GameObject>(this.targets.Length);
      for (int index = 0; index < this.targets.Length; ++index)
      {
        GameObject go = EditorUtility.InstantiateForAnimatorPreview(this.targets[index]);
        GameObjectInspector.SetEnabledRecursive(go, false);
        this.m_PreviewInstances.Add(go);
      }
    }

    private void DestroyPreviewInstances()
    {
      if (this.m_PreviewInstances == null || this.m_PreviewInstances.Count == 0)
        return;
      using (List<GameObject>.Enumerator enumerator = this.m_PreviewInstances.GetEnumerator())
      {
        while (enumerator.MoveNext())
          UnityEngine.Object.DestroyImmediate((UnityEngine.Object) enumerator.Current);
      }
      this.m_PreviewInstances.Clear();
    }

    private void InitPreview()
    {
      if (this.m_PreviewUtility != null)
        return;
      this.m_PreviewUtility = new PreviewRenderUtility(true);
      this.m_PreviewUtility.m_CameraFieldOfView = 30f;
      this.m_PreviewUtility.m_Camera.cullingMask = 1 << Camera.PreviewCullingLayer;
      this.CreatePreviewInstances();
    }

    public void OnDestroy()
    {
      this.DestroyPreviewInstances();
      if (this.m_PreviewUtility == null)
        return;
      this.m_PreviewUtility.Cleanup();
      this.m_PreviewUtility = (PreviewRenderUtility) null;
    }

    public static bool HasRenderablePartsRecurse(GameObject go)
    {
      MeshRenderer component1 = go.GetComponent(typeof (MeshRenderer)) as MeshRenderer;
      MeshFilter component2 = go.GetComponent(typeof (MeshFilter)) as MeshFilter;
      if ((bool) ((UnityEngine.Object) component1) && (bool) ((UnityEngine.Object) component2) && (bool) ((UnityEngine.Object) component2.sharedMesh))
        return true;
      SkinnedMeshRenderer component3 = go.GetComponent(typeof (SkinnedMeshRenderer)) as SkinnedMeshRenderer;
      if ((bool) ((UnityEngine.Object) component3) && (bool) ((UnityEngine.Object) component3.sharedMesh))
        return true;
      SpriteRenderer component4 = go.GetComponent(typeof (SpriteRenderer)) as SpriteRenderer;
      if ((bool) ((UnityEngine.Object) component4) && (bool) ((UnityEngine.Object) component4.sprite))
        return true;
      foreach (Component component5 in go.transform)
      {
        if (GameObjectInspector.HasRenderablePartsRecurse(component5.gameObject))
          return true;
      }
      return false;
    }

    public static void GetRenderableBoundsRecurse(ref Bounds bounds, GameObject go)
    {
      MeshRenderer component1 = go.GetComponent(typeof (MeshRenderer)) as MeshRenderer;
      MeshFilter component2 = go.GetComponent(typeof (MeshFilter)) as MeshFilter;
      if ((bool) ((UnityEngine.Object) component1) && (bool) ((UnityEngine.Object) component2) && (bool) ((UnityEngine.Object) component2.sharedMesh))
      {
        if (bounds.extents == Vector3.zero)
          bounds = component1.bounds;
        else
          bounds.Encapsulate(component1.bounds);
      }
      SkinnedMeshRenderer component3 = go.GetComponent(typeof (SkinnedMeshRenderer)) as SkinnedMeshRenderer;
      if ((bool) ((UnityEngine.Object) component3) && (bool) ((UnityEngine.Object) component3.sharedMesh))
      {
        if (bounds.extents == Vector3.zero)
          bounds = component3.bounds;
        else
          bounds.Encapsulate(component3.bounds);
      }
      SpriteRenderer component4 = go.GetComponent(typeof (SpriteRenderer)) as SpriteRenderer;
      if ((bool) ((UnityEngine.Object) component4) && (bool) ((UnityEngine.Object) component4.sprite))
      {
        if (bounds.extents == Vector3.zero)
          bounds = component4.bounds;
        else
          bounds.Encapsulate(component4.bounds);
      }
      foreach (Transform transform in go.transform)
        GameObjectInspector.GetRenderableBoundsRecurse(ref bounds, transform.gameObject);
    }

    private static float GetRenderableCenterRecurse(ref Vector3 center, GameObject go, int depth, int minDepth, int maxDepth)
    {
      if (depth > maxDepth)
        return 0.0f;
      float num = 0.0f;
      if (depth > minDepth)
      {
        MeshRenderer component1 = go.GetComponent(typeof (MeshRenderer)) as MeshRenderer;
        MeshFilter component2 = go.GetComponent(typeof (MeshFilter)) as MeshFilter;
        SkinnedMeshRenderer component3 = go.GetComponent(typeof (SkinnedMeshRenderer)) as SkinnedMeshRenderer;
        SpriteRenderer component4 = go.GetComponent(typeof (SpriteRenderer)) as SpriteRenderer;
        if ((UnityEngine.Object) component1 == (UnityEngine.Object) null && (UnityEngine.Object) component2 == (UnityEngine.Object) null && ((UnityEngine.Object) component3 == (UnityEngine.Object) null && (UnityEngine.Object) component4 == (UnityEngine.Object) null))
        {
          num = 1f;
          center = center + go.transform.position;
        }
        else if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && (UnityEngine.Object) component2 != (UnityEngine.Object) null)
        {
          if ((double) Vector3.Distance(component1.bounds.center, go.transform.position) < 0.00999999977648258)
          {
            num = 1f;
            center = center + go.transform.position;
          }
        }
        else if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
        {
          if ((double) Vector3.Distance(component3.bounds.center, go.transform.position) < 0.00999999977648258)
          {
            num = 1f;
            center = center + go.transform.position;
          }
        }
        else if ((UnityEngine.Object) component4 != (UnityEngine.Object) null && (double) Vector3.Distance(component4.bounds.center, go.transform.position) < 0.00999999977648258)
        {
          num = 1f;
          center = center + go.transform.position;
        }
      }
      ++depth;
      foreach (Transform transform in go.transform)
        num += GameObjectInspector.GetRenderableCenterRecurse(ref center, transform.gameObject, depth, minDepth, maxDepth);
      return num;
    }

    public static Vector3 GetRenderableCenterRecurse(GameObject go, int minDepth, int maxDepth)
    {
      Vector3 zero = Vector3.zero;
      float renderableCenterRecurse = GameObjectInspector.GetRenderableCenterRecurse(ref zero, go, 0, minDepth, maxDepth);
      return (double) renderableCenterRecurse <= 0.0 ? go.transform.position : zero / renderableCenterRecurse;
    }

    public override bool HasPreviewGUI()
    {
      if (!EditorUtility.IsPersistent(this.target))
        return false;
      return this.HasStaticPreview();
    }

    private bool HasStaticPreview()
    {
      if (this.targets.Length > 1)
        return true;
      if (this.target == (UnityEngine.Object) null)
        return false;
      GameObject target = this.target as GameObject;
      if ((bool) ((UnityEngine.Object) (target.GetComponent(typeof (Camera)) as Camera)))
        return true;
      return GameObjectInspector.HasRenderablePartsRecurse(target);
    }

    public override void OnPreviewSettings()
    {
      if (!ShaderUtil.hardwareSupportsRectRenderTexture)
        return;
      GUI.enabled = true;
      this.InitPreview();
    }

    private void DoRenderPreview()
    {
      GameObject previewInstance = this.m_PreviewInstances[this.referenceTargetIndex];
      Bounds bounds = new Bounds(previewInstance.transform.position, Vector3.zero);
      GameObjectInspector.GetRenderableBoundsRecurse(ref bounds, previewInstance);
      float num1 = Mathf.Max(bounds.extents.magnitude, 0.0001f);
      float num2 = num1 * 3.8f;
      Quaternion quaternion = Quaternion.Euler(-this.previewDir.y, -this.previewDir.x, 0.0f);
      this.m_PreviewUtility.m_Camera.transform.position = bounds.center - quaternion * Vector3.forward * num2;
      this.m_PreviewUtility.m_Camera.transform.rotation = quaternion;
      this.m_PreviewUtility.m_Camera.nearClipPlane = num2 - num1 * 1.1f;
      this.m_PreviewUtility.m_Camera.farClipPlane = num2 + num1 * 1.1f;
      this.m_PreviewUtility.m_Light[0].intensity = 0.7f;
      this.m_PreviewUtility.m_Light[0].transform.rotation = quaternion * Quaternion.Euler(40f, 40f, 0.0f);
      this.m_PreviewUtility.m_Light[1].intensity = 0.7f;
      this.m_PreviewUtility.m_Light[1].transform.rotation = quaternion * Quaternion.Euler(340f, 218f, 177f);
      InternalEditorUtility.SetCustomLighting(this.m_PreviewUtility.m_Light, new Color(0.1f, 0.1f, 0.1f, 0.0f));
      bool fog = RenderSettings.fog;
      Unsupported.SetRenderSettingsUseFogNoDirty(false);
      GameObjectInspector.SetEnabledRecursive(previewInstance, true);
      this.m_PreviewUtility.m_Camera.Render();
      GameObjectInspector.SetEnabledRecursive(previewInstance, false);
      Unsupported.SetRenderSettingsUseFogNoDirty(fog);
      InternalEditorUtility.RemoveCustomLighting();
    }

    public override Texture2D RenderStaticPreview(string assetPath, UnityEngine.Object[] subAssets, int width, int height)
    {
      if (!this.HasStaticPreview() || !ShaderUtil.hardwareSupportsRectRenderTexture)
        return (Texture2D) null;
      this.InitPreview();
      this.m_PreviewUtility.BeginStaticPreview(new Rect(0.0f, 0.0f, (float) width, (float) height));
      this.DoRenderPreview();
      return this.m_PreviewUtility.EndStaticPreview();
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
      if (!ShaderUtil.hardwareSupportsRectRenderTexture)
      {
        if (Event.current.type != EventType.Repaint)
          return;
        EditorGUI.DropShadowLabel(new Rect(r.x, r.y, r.width, 40f), "Preview requires\nrender texture support");
      }
      else
      {
        this.InitPreview();
        this.previewDir = PreviewGUI.Drag2D(this.previewDir, r);
        if (Event.current.type != EventType.Repaint)
          return;
        this.m_PreviewUtility.BeginPreview(r, background);
        this.DoRenderPreview();
        this.m_PreviewUtility.EndAndDrawPreview(r);
      }
    }

    public void OnSceneDrag(SceneView sceneView)
    {
      GameObject target = this.target as GameObject;
      switch (PrefabUtility.GetPrefabType((UnityEngine.Object) target))
      {
        case PrefabType.Prefab:
        case PrefabType.ModelPrefab:
          Event current = Event.current;
          switch (current.type)
          {
            case EventType.DragUpdated:
              if ((UnityEngine.Object) GameObjectInspector.dragObject == (UnityEngine.Object) null)
              {
                GameObjectInspector.dragObject = (GameObject) PrefabUtility.InstantiatePrefab((UnityEngine.Object) PrefabUtility.FindPrefabRoot(target));
                HandleUtility.ignoreRaySnapObjects = GameObjectInspector.dragObject.GetComponentsInChildren<Transform>();
                GameObjectInspector.dragObject.hideFlags = HideFlags.HideInHierarchy;
                GameObjectInspector.dragObject.name = target.name;
              }
              DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
              object obj = HandleUtility.RaySnap(HandleUtility.GUIPointToWorldRay(current.mousePosition));
              if (obj != null)
              {
                RaycastHit raycastHit = (RaycastHit) obj;
                float num1 = 0.0f;
                if (Tools.pivotMode == PivotMode.Center)
                {
                  float num2 = HandleUtility.CalcRayPlaceOffset(HandleUtility.ignoreRaySnapObjects, raycastHit.normal);
                  if ((double) num2 != double.PositiveInfinity)
                    num1 = Vector3.Dot(GameObjectInspector.dragObject.transform.position, raycastHit.normal) - num2;
                }
                GameObjectInspector.dragObject.transform.position = Matrix4x4.identity.MultiplyPoint(raycastHit.point + raycastHit.normal * num1);
              }
              else
                GameObjectInspector.dragObject.transform.position = HandleUtility.GUIPointToWorldRay(current.mousePosition).GetPoint(10f);
              if (sceneView.in2DMode)
              {
                Vector3 position = GameObjectInspector.dragObject.transform.position;
                position.z = PrefabUtility.FindPrefabRoot(target).transform.position.z;
                GameObjectInspector.dragObject.transform.position = position;
              }
              current.Use();
              return;
            case EventType.DragPerform:
              string uniqueNameForSibling = GameObjectUtility.GetUniqueNameForSibling((Transform) null, GameObjectInspector.dragObject.name);
              GameObjectInspector.dragObject.hideFlags = HideFlags.None;
              Undo.RegisterCreatedObjectUndo((UnityEngine.Object) GameObjectInspector.dragObject, "Place " + GameObjectInspector.dragObject.name);
              EditorUtility.SetDirty((UnityEngine.Object) GameObjectInspector.dragObject);
              DragAndDrop.AcceptDrag();
              Selection.activeObject = (UnityEngine.Object) GameObjectInspector.dragObject;
              HandleUtility.ignoreRaySnapObjects = (Transform[]) null;
              EditorWindow.mouseOverWindow.Focus();
              GameObjectInspector.dragObject.name = uniqueNameForSibling;
              GameObjectInspector.dragObject = (GameObject) null;
              current.Use();
              return;
            case EventType.DragExited:
              if (!(bool) ((UnityEngine.Object) GameObjectInspector.dragObject))
                return;
              UnityEngine.Object.DestroyImmediate((UnityEngine.Object) GameObjectInspector.dragObject, false);
              HandleUtility.ignoreRaySnapObjects = (Transform[]) null;
              GameObjectInspector.dragObject = (GameObject) null;
              current.Use();
              return;
            default:
              return;
          }
      }
    }

    private class Styles
    {
      public GUIContent goIcon = EditorGUIUtility.IconContent("GameObject Icon");
      public GUIContent typelessIcon = EditorGUIUtility.IconContent("Prefab Icon");
      public GUIContent prefabIcon = EditorGUIUtility.IconContent("PrefabNormal Icon");
      public GUIContent modelIcon = EditorGUIUtility.IconContent("PrefabModel Icon");
      public GUIContent dataTemplateIcon = EditorGUIUtility.IconContent("PrefabNormal Icon");
      public GUIContent dropDownIcon = EditorGUIUtility.IconContent("Icon Dropdown");
      public float staticFieldToggleWidth = EditorStyles.toggle.CalcSize(EditorGUIUtility.TempContent("Static")).x + 6f;
      public float tagFieldWidth = EditorStyles.boldLabel.CalcSize(EditorGUIUtility.TempContent("Tag")).x;
      public float layerFieldWidth = EditorStyles.boldLabel.CalcSize(EditorGUIUtility.TempContent("Layer")).x;
      public float navLayerFieldWidth = EditorStyles.boldLabel.CalcSize(EditorGUIUtility.TempContent("Nav Layer")).x;
      public GUIStyle staticDropdown = (GUIStyle) "StaticDropdown";
      public GUIStyle instanceManagementInfo = new GUIStyle(EditorStyles.helpBox);
      public GUIContent goTypeLabelMultiple = new GUIContent("Multiple");
      public GUIContent[] goTypeLabel = new GUIContent[8]{ null, EditorGUIUtility.TextContent("Prefab"), EditorGUIUtility.TextContent("Model"), EditorGUIUtility.TextContent("Prefab"), EditorGUIUtility.TextContent("Model"), EditorGUIUtility.TextContent("Missing|The source Prefab or Model has been deleted."), EditorGUIUtility.TextContent("Prefab|You have broken the prefab connection. Changes to the prefab will not be applied to this object before you Apply or Revert."), EditorGUIUtility.TextContent("Model|You have broken the prefab connection. Changes to the model will not be applied to this object before you Revert.") };

      public Styles()
      {
        GUIStyle guiStyle = (GUIStyle) "MiniButtonMid";
        this.instanceManagementInfo.padding = guiStyle.padding;
        this.instanceManagementInfo.alignment = guiStyle.alignment;
      }
    }
  }
}
