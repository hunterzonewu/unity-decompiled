// Decompiled with JetBrains decompiler
// Type: UnityEditor.ReflectionProbeEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (ReflectionProbe))]
  internal class ReflectionProbeEditor : Editor
  {
    private static int s_BoxHash = "ReflectionProbeEditorHash".GetHashCode();
    internal static Color kGizmoReflectionProbe = new Color(1f, 0.8980392f, 0.5803922f, 0.5019608f);
    internal static Color kGizmoHandleReflectionProbe = new Color(1f, 0.8980392f, 0.6666667f, 1f);
    private Vector3 m_OldTransformPosition = Vector3.zero;
    private BoxEditor m_BoxEditor = new BoxEditor(true, ReflectionProbeEditor.s_BoxHash);
    private readonly AnimBool m_ShowProbeModeRealtimeOptions = new AnimBool();
    private readonly AnimBool m_ShowProbeModeCustomOptions = new AnimBool();
    private readonly AnimBool m_ShowBoxOptions = new AnimBool();
    private static ReflectionProbeEditor s_LastInteractedEditor;
    private SerializedProperty m_Mode;
    private SerializedProperty m_RefreshMode;
    private SerializedProperty m_TimeSlicingMode;
    private SerializedProperty m_Resolution;
    private SerializedProperty m_ShadowDistance;
    private SerializedProperty m_Importance;
    private SerializedProperty m_BoxSize;
    private SerializedProperty m_BoxOffset;
    private SerializedProperty m_CullingMask;
    private SerializedProperty m_ClearFlags;
    private SerializedProperty m_BackgroundColor;
    private SerializedProperty m_HDR;
    private SerializedProperty m_BoxProjection;
    private SerializedProperty m_IntensityMultiplier;
    private SerializedProperty m_BlendDistance;
    private SerializedProperty m_CustomBakedTexture;
    private SerializedProperty m_RenderDynamicObjects;
    private SerializedProperty m_UseOcclusionCulling;
    private SerializedProperty[] m_NearAndFarProperties;
    private static Mesh s_SphereMesh;
    private static Mesh s_PlaneMesh;
    private Material m_ReflectiveMaterial;
    private float m_MipLevelPreview;
    private TextureInspector m_CubemapEditor;

    private bool sceneViewEditing
    {
      get
      {
        if (this.IsReflectionProbeEditMode(UnityEditorInternal.EditMode.editMode))
          return UnityEditorInternal.EditMode.IsOwner((Editor) this);
        return false;
      }
    }

    private ReflectionProbe reflectionProbeTarget
    {
      get
      {
        return (ReflectionProbe) this.target;
      }
    }

    private ReflectionProbeMode reflectionProbeMode
    {
      get
      {
        return this.reflectionProbeTarget.mode;
      }
    }

    private static Mesh sphereMesh
    {
      get
      {
        return ReflectionProbeEditor.s_SphereMesh ?? (ReflectionProbeEditor.s_SphereMesh = Resources.GetBuiltinResource(typeof (Mesh), "New-Sphere.fbx") as Mesh);
      }
    }

    private static Mesh planeMesh
    {
      get
      {
        return ReflectionProbeEditor.s_PlaneMesh ?? (ReflectionProbeEditor.s_PlaneMesh = Resources.GetBuiltinResource(typeof (Mesh), "New-Plane.fbx") as Mesh);
      }
    }

    private Material reflectiveMaterial
    {
      get
      {
        if ((UnityEngine.Object) this.m_ReflectiveMaterial == (UnityEngine.Object) null)
        {
          this.m_ReflectiveMaterial = (Material) UnityEngine.Object.Instantiate(EditorGUIUtility.Load("Previews/PreviewCubemapMaterial.mat"));
          this.m_ReflectiveMaterial.hideFlags = HideFlags.HideAndDontSave;
        }
        return this.m_ReflectiveMaterial;
      }
    }

    private bool IsReflectionProbeEditMode(UnityEditorInternal.EditMode.SceneViewEditMode editMode)
    {
      if (editMode != UnityEditorInternal.EditMode.SceneViewEditMode.ReflectionProbeBox)
        return editMode == UnityEditorInternal.EditMode.SceneViewEditMode.ReflectionProbeOrigin;
      return true;
    }

    public void OnEnable()
    {
      this.m_Mode = this.serializedObject.FindProperty("m_Mode");
      this.m_RefreshMode = this.serializedObject.FindProperty("m_RefreshMode");
      this.m_TimeSlicingMode = this.serializedObject.FindProperty("m_TimeSlicingMode");
      this.m_Resolution = this.serializedObject.FindProperty("m_Resolution");
      this.m_NearAndFarProperties = new SerializedProperty[2]
      {
        this.serializedObject.FindProperty("m_NearClip"),
        this.serializedObject.FindProperty("m_FarClip")
      };
      this.m_ShadowDistance = this.serializedObject.FindProperty("m_ShadowDistance");
      this.m_Importance = this.serializedObject.FindProperty("m_Importance");
      this.m_BoxSize = this.serializedObject.FindProperty("m_BoxSize");
      this.m_BoxOffset = this.serializedObject.FindProperty("m_BoxOffset");
      this.m_CullingMask = this.serializedObject.FindProperty("m_CullingMask");
      this.m_ClearFlags = this.serializedObject.FindProperty("m_ClearFlags");
      this.m_BackgroundColor = this.serializedObject.FindProperty("m_BackGroundColor");
      this.m_HDR = this.serializedObject.FindProperty("m_HDR");
      this.m_BoxProjection = this.serializedObject.FindProperty("m_BoxProjection");
      this.m_IntensityMultiplier = this.serializedObject.FindProperty("m_IntensityMultiplier");
      this.m_BlendDistance = this.serializedObject.FindProperty("m_BlendDistance");
      this.m_CustomBakedTexture = this.serializedObject.FindProperty("m_CustomBakedTexture");
      this.m_RenderDynamicObjects = this.serializedObject.FindProperty("m_RenderDynamicObjects");
      this.m_UseOcclusionCulling = this.serializedObject.FindProperty("m_UseOcclusionCulling");
      ReflectionProbe target = this.target as ReflectionProbe;
      this.m_ShowProbeModeRealtimeOptions.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowProbeModeCustomOptions.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowBoxOptions.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowProbeModeRealtimeOptions.value = target.mode == ReflectionProbeMode.Realtime;
      this.m_ShowProbeModeCustomOptions.value = target.mode == ReflectionProbeMode.Custom;
      this.m_ShowBoxOptions.value = !this.m_BoxSize.hasMultipleDifferentValues && !this.m_BoxOffset.hasMultipleDifferentValues && target.type == ReflectionProbeType.Cube;
      this.m_BoxEditor.OnEnable();
      this.m_BoxEditor.SetAlwaysDisplayHandles(true);
      this.m_BoxEditor.allowNegativeSize = false;
      this.m_OldTransformPosition = ((Component) this.target).transform.position;
    }

    public void OnDisable()
    {
      this.m_BoxEditor.OnDisable();
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_ReflectiveMaterial);
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_CubemapEditor);
    }

    private bool IsCollidingWithOtherProbes(string targetPath, ReflectionProbe targetProbe, out ReflectionProbe collidingProbe)
    {
      ReflectionProbe[] array = ((IEnumerable<ReflectionProbe>) UnityEngine.Object.FindObjectsOfType<ReflectionProbe>()).ToArray<ReflectionProbe>();
      collidingProbe = (ReflectionProbe) null;
      foreach (ReflectionProbe reflectionProbe in array)
      {
        if (!((UnityEngine.Object) reflectionProbe == (UnityEngine.Object) targetProbe) && !((UnityEngine.Object) reflectionProbe.customBakedTexture == (UnityEngine.Object) null) && AssetDatabase.GetAssetPath((UnityEngine.Object) reflectionProbe.customBakedTexture) == targetPath)
        {
          collidingProbe = reflectionProbe;
          return true;
        }
      }
      return false;
    }

    private void BakeCustomReflectionProbe(ReflectionProbe probe, bool usePreviousAssetPath)
    {
      string str1 = string.Empty;
      if (usePreviousAssetPath)
        str1 = AssetDatabase.GetAssetPath((UnityEngine.Object) probe.customBakedTexture);
      string extension = !probe.hdr ? "png" : "exr";
      if (string.IsNullOrEmpty(str1) || Path.GetExtension(str1) != "." + extension)
      {
        string str2 = FileUtil.GetPathWithoutExtension(SceneManager.GetActiveScene().path);
        if (string.IsNullOrEmpty(str2))
          str2 = "Assets";
        else if (!Directory.Exists(str2))
          Directory.CreateDirectory(str2);
        string path2 = probe.name + (!probe.hdr ? "-reflection" : "-reflectionHDR") + "." + extension;
        str1 = EditorUtility.SaveFilePanelInProject("Save reflection probe's cubemap.", Path.GetFileNameWithoutExtension(AssetDatabase.GenerateUniqueAssetPath(Path.Combine(str2, path2))), extension, string.Empty, str2);
        ReflectionProbe collidingProbe;
        if (string.IsNullOrEmpty(str1) || this.IsCollidingWithOtherProbes(str1, probe, out collidingProbe) && !EditorUtility.DisplayDialog("Cubemap is used by other reflection probe", string.Format("'{0}' path is used by the game object '{1}', do you really want to overwrite it?", (object) str1, (object) collidingProbe.name), "Yes", "No"))
          return;
      }
      EditorUtility.DisplayProgressBar("Reflection Probes", "Baking " + str1, 0.5f);
      if (!Lightmapping.BakeReflectionProbe(probe, str1))
        Debug.LogError((object) ("Failed to bake reflection probe to " + str1));
      EditorUtility.ClearProgressBar();
    }

    private void OnBakeCustomButton(object data)
    {
      int num = (int) data;
      ReflectionProbe target = this.target as ReflectionProbe;
      if (num != 0)
        return;
      this.BakeCustomReflectionProbe(target, false);
    }

    private void OnBakeButton(object data)
    {
      if ((int) data != 0)
        return;
      Lightmapping.BakeAllReflectionProbesSnapshots();
    }

    private void DoBakeButton()
    {
      if (this.reflectionProbeTarget.mode == ReflectionProbeMode.Realtime)
      {
        EditorGUILayout.HelpBox("Baking of this reflection probe should be initiated from the scripting API because the type is 'Realtime'", MessageType.Info);
        if (QualitySettings.realtimeReflectionProbes)
          return;
        EditorGUILayout.HelpBox("Realtime reflection probes are disabled in Quality Settings", MessageType.Warning);
      }
      else if (this.reflectionProbeTarget.mode == ReflectionProbeMode.Baked && Lightmapping.giWorkflowMode != Lightmapping.GIWorkflowMode.OnDemand)
      {
        EditorGUILayout.HelpBox("Baking of this reflection probe is automatic because this probe's type is 'Baked' and the Lighting window is using 'Auto Baking'. The cubemap created is stored in the GI cache.", MessageType.Info);
      }
      else
      {
        GUILayout.BeginHorizontal();
        GUILayout.Space(EditorGUIUtility.labelWidth);
        switch (this.reflectionProbeMode)
        {
          case ReflectionProbeMode.Baked:
            EditorGUI.BeginDisabledGroup(!this.reflectionProbeTarget.enabled);
            if (EditorGUI.ButtonWithDropdownList(ReflectionProbeEditor.Styles.bakeButtonText, ReflectionProbeEditor.Styles.bakeButtonsText, new GenericMenu.MenuFunction2(this.OnBakeButton)))
            {
              Lightmapping.BakeReflectionProbeSnapshot(this.reflectionProbeTarget);
              GUIUtility.ExitGUI();
            }
            EditorGUI.EndDisabledGroup();
            break;
          case ReflectionProbeMode.Custom:
            if (EditorGUI.ButtonWithDropdownList(ReflectionProbeEditor.Styles.bakeButtonText, ReflectionProbeEditor.Styles.bakeCustomButtonsText, new GenericMenu.MenuFunction2(this.OnBakeCustomButton)))
            {
              this.BakeCustomReflectionProbe(this.reflectionProbeTarget, true);
              GUIUtility.ExitGUI();
              break;
            }
            break;
        }
        GUILayout.EndHorizontal();
      }
    }

    private void DoToolbar()
    {
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      GUI.changed = false;
      UnityEditorInternal.EditMode.SceneViewEditMode editMode = UnityEditorInternal.EditMode.editMode;
      EditorGUI.BeginChangeCheck();
      UnityEditorInternal.EditMode.DoInspectorToolbar(ReflectionProbeEditor.Styles.sceneViewEditModes, ReflectionProbeEditor.Styles.toolContents, this.GetBounds(), (Editor) this);
      if (EditorGUI.EndChangeCheck())
        ReflectionProbeEditor.s_LastInteractedEditor = this;
      if (editMode != UnityEditorInternal.EditMode.editMode)
      {
        if (UnityEditorInternal.EditMode.editMode == UnityEditorInternal.EditMode.SceneViewEditMode.ReflectionProbeOrigin)
          this.m_OldTransformPosition = ((Component) this.target).transform.position;
        if ((UnityEngine.Object) Toolbar.get != (UnityEngine.Object) null)
          Toolbar.get.Repaint();
      }
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
      GUILayout.BeginVertical(EditorStyles.helpBox, new GUILayoutOption[0]);
      string text = ReflectionProbeEditor.Styles.baseSceneEditingToolText;
      if (this.sceneViewEditing)
      {
        int index = ArrayUtility.IndexOf<UnityEditorInternal.EditMode.SceneViewEditMode>(ReflectionProbeEditor.Styles.sceneViewEditModes, UnityEditorInternal.EditMode.editMode);
        if (index >= 0)
          text = ReflectionProbeEditor.Styles.toolNames[index].text;
      }
      GUILayout.Label(text, ReflectionProbeEditor.Styles.richTextMiniLabel, new GUILayoutOption[0]);
      GUILayout.EndVertical();
      EditorGUILayout.Space();
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      if (this.targets.Length == 1)
        this.DoToolbar();
      this.m_ShowProbeModeRealtimeOptions.target = this.reflectionProbeMode == ReflectionProbeMode.Realtime;
      this.m_ShowProbeModeCustomOptions.target = this.reflectionProbeMode == ReflectionProbeMode.Custom;
      EditorGUILayout.IntPopup(this.m_Mode, ReflectionProbeEditor.Styles.reflectionProbeMode, ReflectionProbeEditor.Styles.reflectionProbeModeValues, ReflectionProbeEditor.Styles.typeText, new GUILayoutOption[0]);
      if (!this.m_Mode.hasMultipleDifferentValues)
      {
        ++EditorGUI.indentLevel;
        if (EditorGUILayout.BeginFadeGroup(this.m_ShowProbeModeCustomOptions.faded))
        {
          EditorGUILayout.PropertyField(this.m_RenderDynamicObjects, ReflectionProbeEditor.Styles.renderDynamicObjects, new GUILayoutOption[0]);
          EditorGUI.BeginChangeCheck();
          EditorGUI.showMixedValue = this.m_CustomBakedTexture.hasMultipleDifferentValues;
          UnityEngine.Object @object = EditorGUILayout.ObjectField(ReflectionProbeEditor.Styles.customCubemapText, this.m_CustomBakedTexture.objectReferenceValue, typeof (Cubemap), false, new GUILayoutOption[0]);
          EditorGUI.showMixedValue = false;
          if (EditorGUI.EndChangeCheck())
            this.m_CustomBakedTexture.objectReferenceValue = @object;
        }
        EditorGUILayout.EndFadeGroup();
        if (EditorGUILayout.BeginFadeGroup(this.m_ShowProbeModeRealtimeOptions.faded))
        {
          EditorGUILayout.PropertyField(this.m_RefreshMode, ReflectionProbeEditor.Styles.refreshMode, new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_TimeSlicingMode, ReflectionProbeEditor.Styles.timeSlicing, new GUILayoutOption[0]);
          EditorGUILayout.Space();
        }
        EditorGUILayout.EndFadeGroup();
        --EditorGUI.indentLevel;
      }
      EditorGUILayout.Space();
      GUILayout.Label(ReflectionProbeEditor.Styles.runtimeSettingsHeader);
      ++EditorGUI.indentLevel;
      EditorGUILayout.PropertyField(this.m_Importance, ReflectionProbeEditor.Styles.importanceText, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_IntensityMultiplier, ReflectionProbeEditor.Styles.intensityText, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_BoxProjection, ReflectionProbeEditor.Styles.boxProjectionText, new GUILayoutOption[0]);
      EditorGUI.BeginDisabledGroup(!SceneView.IsUsingDeferredRenderingPath() || UnityEngine.Rendering.GraphicsSettings.GetShaderMode(BuiltinShaderType.DeferredReflections) == UnityEngine.Rendering.BuiltinShaderMode.Disabled);
      EditorGUILayout.PropertyField(this.m_BlendDistance, ReflectionProbeEditor.Styles.blendDistanceText, new GUILayoutOption[0]);
      EditorGUI.EndDisabledGroup();
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowBoxOptions.faded))
      {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(this.m_BoxSize, ReflectionProbeEditor.Styles.sizeText, new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(this.m_BoxOffset, ReflectionProbeEditor.Styles.centerText, new GUILayoutOption[0]);
        if (EditorGUI.EndChangeCheck())
        {
          Vector3 vector3Value1 = this.m_BoxOffset.vector3Value;
          Vector3 vector3Value2 = this.m_BoxSize.vector3Value;
          if (this.ValidateAABB(ref vector3Value1, ref vector3Value2))
          {
            this.m_BoxOffset.vector3Value = vector3Value1;
            this.m_BoxSize.vector3Value = vector3Value2;
          }
        }
      }
      EditorGUILayout.EndFadeGroup();
      --EditorGUI.indentLevel;
      EditorGUILayout.Space();
      GUILayout.Label(ReflectionProbeEditor.Styles.captureCubemapHeaderText);
      ++EditorGUI.indentLevel;
      EditorGUILayout.IntPopup(this.m_Resolution, ReflectionProbeEditor.Styles.renderTextureSizes, ReflectionProbeEditor.Styles.renderTextureSizesValues, ReflectionProbeEditor.Styles.resolutionText, new GUILayoutOption[1]
      {
        GUILayout.MinWidth(40f)
      });
      EditorGUILayout.PropertyField(this.m_HDR);
      EditorGUILayout.PropertyField(this.m_ShadowDistance);
      EditorGUILayout.IntPopup(this.m_ClearFlags, ReflectionProbeEditor.Styles.clearFlags, ReflectionProbeEditor.Styles.clearFlagsValues, ReflectionProbeEditor.Styles.clearFlagsText, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_BackgroundColor, ReflectionProbeEditor.Styles.backgroundColorText, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_CullingMask);
      EditorGUILayout.PropertyField(this.m_UseOcclusionCulling);
      EditorGUILayout.PropertiesField(EditorGUI.s_ClipingPlanesLabel, this.m_NearAndFarProperties, EditorGUI.s_NearAndFarLabels, 35f);
      --EditorGUI.indentLevel;
      EditorGUILayout.Space();
      if (this.targets.Length == 1)
      {
        ReflectionProbe target = (ReflectionProbe) this.target;
        if (target.mode == ReflectionProbeMode.Custom && (UnityEngine.Object) target.customBakedTexture != (UnityEngine.Object) null)
        {
          Cubemap customBakedTexture = target.customBakedTexture as Cubemap;
          if ((bool) ((UnityEngine.Object) customBakedTexture) && customBakedTexture.mipmapCount == 1)
            EditorGUILayout.HelpBox("No mipmaps in the cubemap, Smoothness value in Standard shader will be ignored.", MessageType.Warning);
        }
      }
      this.DoBakeButton();
      EditorGUILayout.Space();
      this.serializedObject.ApplyModifiedProperties();
    }

    private Bounds GetBounds()
    {
      if (this.target is ReflectionProbe)
        return ((ReflectionProbe) this.target).bounds;
      return new Bounds();
    }

    private bool ValidPreviewSetup()
    {
      ReflectionProbe target = (ReflectionProbe) this.target;
      if ((UnityEngine.Object) target != (UnityEngine.Object) null)
        return (UnityEngine.Object) target.texture != (UnityEngine.Object) null;
      return false;
    }

    public override bool HasPreviewGUI()
    {
      if (this.targets.Length > 1)
        return false;
      if (this.ValidPreviewSetup())
      {
        Editor cubemapEditor = (Editor) this.m_CubemapEditor;
        Editor.CreateCachedEditor((UnityEngine.Object) ((ReflectionProbe) this.target).texture, (System.Type) null, ref cubemapEditor);
        this.m_CubemapEditor = cubemapEditor as TextureInspector;
      }
      return true;
    }

    public override void OnPreviewSettings()
    {
      if (!this.ValidPreviewSetup())
        return;
      this.m_CubemapEditor.mipLevel = this.m_MipLevelPreview;
      EditorGUI.BeginChangeCheck();
      this.m_CubemapEditor.OnPreviewSettings();
      if (!EditorGUI.EndChangeCheck())
        return;
      EditorApplication.SetSceneRepaintDirty();
      this.m_MipLevelPreview = this.m_CubemapEditor.mipLevel;
    }

    public override void OnPreviewGUI(Rect position, GUIStyle style)
    {
      if (!this.ValidPreviewSetup())
      {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        Color color = GUI.color;
        GUI.color = new Color(1f, 1f, 1f, 0.5f);
        GUILayout.Label("Reflection Probe not baked yet");
        GUI.color = color;
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
      }
      else
      {
        ReflectionProbe target = this.target as ReflectionProbe;
        if ((UnityEngine.Object) target != (UnityEngine.Object) null && (UnityEngine.Object) target.texture != (UnityEngine.Object) null && this.targets.Length == 1)
        {
          Editor cubemapEditor = (Editor) this.m_CubemapEditor;
          Editor.CreateCachedEditor((UnityEngine.Object) target.texture, (System.Type) null, ref cubemapEditor);
          this.m_CubemapEditor = cubemapEditor as TextureInspector;
        }
        if (!((UnityEngine.Object) this.m_CubemapEditor != (UnityEngine.Object) null))
          return;
        this.m_CubemapEditor.SetCubemapIntensity(this.GetProbeIntensity((ReflectionProbe) this.target));
        this.m_CubemapEditor.OnPreviewGUI(position, style);
      }
    }

    private float GetProbeIntensity(ReflectionProbe p)
    {
      if ((UnityEngine.Object) p == (UnityEngine.Object) null || (UnityEngine.Object) p.texture == (UnityEngine.Object) null)
        return 1f;
      float num = p.intensity;
      if (TextureUtil.GetTextureColorSpaceString(p.texture) == "Linear")
        num = Mathf.LinearToGammaSpace(num);
      return num;
    }

    public void OnPreSceneGUI()
    {
      if (Event.current.type != EventType.Repaint)
        return;
      ReflectionProbe target = (ReflectionProbe) this.target;
      if (!(bool) ((UnityEngine.Object) this.reflectiveMaterial))
        return;
      Matrix4x4 matrix = new Matrix4x4();
      Material reflectiveMaterial = this.reflectiveMaterial;
      if (target.type == ReflectionProbeType.Cube)
      {
        float num1 = 0.0f;
        TextureInspector cubemapEditor = this.m_CubemapEditor;
        if ((bool) ((UnityEngine.Object) cubemapEditor))
          num1 = cubemapEditor.GetMipLevelForRendering();
        reflectiveMaterial.mainTexture = target.texture;
        reflectiveMaterial.SetMatrix("_CubemapRotation", Matrix4x4.identity);
        reflectiveMaterial.SetFloat("_Mip", num1);
        reflectiveMaterial.SetFloat("_Alpha", 0.0f);
        reflectiveMaterial.SetFloat("_Intensity", this.GetProbeIntensity(target));
        float num2 = target.transform.lossyScale.magnitude * 0.5f;
        matrix.SetTRS(target.transform.position, Quaternion.identity, new Vector3(num2, num2, num2));
        Graphics.DrawMesh(ReflectionProbeEditor.sphereMesh, matrix, this.reflectiveMaterial, 0, SceneView.currentDrawingSceneView.camera);
      }
      else
      {
        reflectiveMaterial.SetTexture("_MainTex", target.texture);
        reflectiveMaterial.SetFloat("_ReflectionProbeType", 1f);
        reflectiveMaterial.SetFloat("_Intensity", 1f);
        Vector3 vector3 = new Vector3();
        Vector3 s = target.transform.lossyScale * 0.2f;
        s.x *= -1f;
        s.z *= -1f;
        matrix.SetTRS(target.transform.position, target.transform.rotation * Quaternion.AngleAxis(90f, Vector3.right), s);
        Graphics.DrawMesh(ReflectionProbeEditor.planeMesh, matrix, this.reflectiveMaterial, 0, SceneView.currentDrawingSceneView.camera);
      }
    }

    private bool ValidateAABB(ref Vector3 center, ref Vector3 size)
    {
      Vector3 position = ((Component) this.target).transform.position;
      Bounds bounds = new Bounds(center + position, size);
      if (bounds.Contains(position))
        return false;
      bounds.Encapsulate(position);
      center = bounds.center - position;
      size = bounds.size;
      return true;
    }

    [DrawGizmo(GizmoType.Active)]
    private static void RenderBoxGizmo(ReflectionProbe reflectionProbe, GizmoType gizmoType)
    {
      if ((UnityEngine.Object) ReflectionProbeEditor.s_LastInteractedEditor == (UnityEngine.Object) null || !ReflectionProbeEditor.s_LastInteractedEditor.sceneViewEditing || UnityEditorInternal.EditMode.editMode != UnityEditorInternal.EditMode.SceneViewEditMode.ReflectionProbeBox)
        return;
      Color color = Gizmos.color;
      Gizmos.color = ReflectionProbeEditor.kGizmoReflectionProbe;
      Gizmos.DrawCube(reflectionProbe.transform.position + reflectionProbe.center, -1f * reflectionProbe.size);
      Gizmos.color = color;
    }

    public void OnSceneGUI()
    {
      if (!this.sceneViewEditing)
        return;
      switch (UnityEditorInternal.EditMode.editMode)
      {
        case UnityEditorInternal.EditMode.SceneViewEditMode.ReflectionProbeBox:
          this.DoBoxEditing();
          break;
        case UnityEditorInternal.EditMode.SceneViewEditMode.ReflectionProbeOrigin:
          this.DoOriginEditing();
          break;
      }
    }

    private void DoOriginEditing()
    {
      ReflectionProbe target = (ReflectionProbe) this.target;
      Vector3 position = target.transform.position;
      Vector3 size = target.size;
      Vector3 center = target.center + position;
      EditorGUI.BeginChangeCheck();
      Vector3 point = Handles.PositionHandle(position, Quaternion.identity);
      bool flag = EditorGUI.EndChangeCheck();
      if (!flag)
      {
        point = position;
        flag = (double) (this.m_OldTransformPosition - point).magnitude > 9.99999974737875E-06;
        if (flag)
          center = target.center + this.m_OldTransformPosition;
      }
      if (!flag)
        return;
      Undo.RecordObject((UnityEngine.Object) target, "Modified Reflection Probe Origin");
      Bounds bounds = new Bounds(center, size);
      Vector3 vector3_1 = bounds.ClosestPoint(point);
      Vector3 vector3_2 = vector3_1;
      target.transform.position = vector3_2;
      this.m_OldTransformPosition = vector3_2;
      target.center = bounds.center - vector3_1;
      EditorUtility.SetDirty(this.target);
    }

    private void DoBoxEditing()
    {
      ReflectionProbe target = (ReflectionProbe) this.target;
      Vector3 position = target.transform.position;
      Vector3 size = target.size;
      Vector3 center1 = target.center + position;
      if (!this.m_BoxEditor.OnSceneGUI(Matrix4x4.identity, ReflectionProbeEditor.kGizmoReflectionProbe, ReflectionProbeEditor.kGizmoHandleReflectionProbe, true, ref center1, ref size))
        return;
      Undo.RecordObject((UnityEngine.Object) target, "Modified Reflection Probe AABB");
      Vector3 center2 = center1 - position;
      this.ValidateAABB(ref center2, ref size);
      target.size = size;
      target.center = center2;
      EditorUtility.SetDirty(this.target);
    }

    private static class Styles
    {
      public static GUIStyle richTextMiniLabel = new GUIStyle(EditorStyles.miniLabel);
      public static string bakeButtonText = "Bake";
      public static string editBoundsText = "Edit Bounds";
      public static string[] bakeCustomButtonsText = new string[1]{ "Bake as new Cubemap..." };
      public static string[] bakeButtonsText = new string[1]{ "Bake All Reflection Probes" };
      public static GUIContent runtimeSettingsHeader = new GUIContent("Runtime settings", "These settings are used by objects when they render with the cubemap of this probe");
      public static GUIContent backgroundColorText = new GUIContent("Background", "Camera clears the screen to this color before rendering.");
      public static GUIContent clearFlagsText = new GUIContent("Clear Flags");
      public static GUIContent intensityText = new GUIContent("Intensity");
      public static GUIContent resolutionText = new GUIContent("Resolution");
      public static GUIContent captureCubemapHeaderText = new GUIContent("Cubemap capture settings");
      public static GUIContent boxProjectionText = new GUIContent("Box Projection", "Box projection is useful for reflections in enclosed spaces where some parrallax and movement in the reflection is wanted. If not set then cubemap reflection will we treated as coming infinite far away. And within this zone objects with the Standard shader will receive this probe's cubemap.");
      public static GUIContent blendDistanceText = new GUIContent("Blend Distance", "Area around the probe where it is blended with other probes. Only used in deferred probes.");
      public static GUIContent sizeText = new GUIContent("Size");
      public static GUIContent centerText = new GUIContent("Probe Origin");
      public static GUIContent skipFramesText = new GUIContent("Skip frames");
      public static GUIContent customCubemapText = new GUIContent("Cubemap");
      public static GUIContent editorUpdateText = new GUIContent("Editor Update");
      public static GUIContent importanceText = new GUIContent("Importance");
      public static GUIContent renderDynamicObjects = new GUIContent("Dynamic Objects", "If enabled dynamic objects are also rendered into the cubemap");
      public static GUIContent timeSlicing = new GUIContent("Time Slicing", "If enabled this probe will update over several frames, to help reduce the impact on the frame rate");
      public static GUIContent refreshMode = new GUIContent("Refresh Mode", "Controls how this probe refreshes in the Player");
      public static GUIContent typeText = new GUIContent("Type", "'Baked Cubemap' uses the 'Auto Baking' mode from the Lighting window. If it is enabled then baking is automatic otherwise manual bake is needed (use the bake button below). \n'Custom' can be used if a custom cubemap is wanted. \n'Realtime' can be used to dynamically re-render the cubemap during runtime (via scripting).");
      public static GUIContent[] reflectionProbeMode = new GUIContent[3]{ new GUIContent("Baked"), new GUIContent("Custom"), new GUIContent("Realtime") };
      public static int[] reflectionProbeModeValues = new int[3]{ 0, 2, 1 };
      public static int[] renderTextureSizesValues = new int[8]{ 16, 32, 64, 128, 256, 512, 1024, 2048 };
      public static GUIContent[] renderTextureSizes = ((IEnumerable<int>) ReflectionProbeEditor.Styles.renderTextureSizesValues).Select<int, GUIContent>((Func<int, GUIContent>) (n => new GUIContent(n.ToString()))).ToArray<GUIContent>();
      public static GUIContent[] clearFlags = new GUIContent[2]{ new GUIContent("Skybox"), new GUIContent("Solid Color") };
      public static int[] clearFlagsValues = new int[2]{ 1, 2 };
      public static GUIContent[] toolContents = new GUIContent[2]{ EditorGUIUtility.IconContent("EditCollider"), EditorGUIUtility.IconContent("MoveTool", "|Move the selected objects.") };
      public static UnityEditorInternal.EditMode.SceneViewEditMode[] sceneViewEditModes = new UnityEditorInternal.EditMode.SceneViewEditMode[2]{ UnityEditorInternal.EditMode.SceneViewEditMode.ReflectionProbeBox, UnityEditorInternal.EditMode.SceneViewEditMode.ReflectionProbeOrigin };
      public static string baseSceneEditingToolText = "<color=grey>Probe Scene Editing Mode:</color> ";
      public static GUIContent[] toolNames = new GUIContent[2]{ new GUIContent(ReflectionProbeEditor.Styles.baseSceneEditingToolText + "Box Projection Bounds", string.Empty), new GUIContent(ReflectionProbeEditor.Styles.baseSceneEditingToolText + "Probe Origin", string.Empty) };
      public static GUIStyle commandStyle = (GUIStyle) "Command";

      static Styles()
      {
        ReflectionProbeEditor.Styles.richTextMiniLabel.richText = true;
      }
    }
  }
}
