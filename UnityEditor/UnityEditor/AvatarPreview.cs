// Decompiled with JetBrains decompiler
// Type: UnityEditor.AvatarPreview
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class AvatarPreview
  {
    public int fps = 60;
    private int m_PreviewHint = "Preview".GetHashCode();
    private int m_PreviewSceneHint = "PreviewSene".GetHashCode();
    private bool m_ShowIKOnFeetButton = true;
    private int m_ModelSelectorId = GUIUtility.GetPermanentControlID();
    private Vector2 m_PreviewDir = new Vector2(120f, -20f);
    private float m_AvatarScale = 1f;
    private float m_ZoomFactor = 1f;
    private Vector3 m_PivotPositionOffset = Vector3.zero;
    private float m_LastNormalizedTime = -1000f;
    private float m_LastStartTime = -1000f;
    private float m_LastStopTime = -1000f;
    private bool m_NextTargetIsForward = true;
    private const string kIkPref = "AvatarpreviewShowIK";
    private const string kReferencePref = "AvatarpreviewShowReference";
    private const string kSpeedPref = "AvatarpreviewSpeed";
    private const float kTimeControlRectHeight = 21f;
    private const string s_PreviewStr = "Preview";
    private const string s_PreviewSceneStr = "PreviewSene";
    private const float kFloorFadeDuration = 0.2f;
    private const float kFloorScale = 5f;
    private const float kFloorScaleSmall = 0.2f;
    private const float kFloorTextureScale = 4f;
    private const float kFloorAlpha = 0.5f;
    private const float kFloorShadowAlpha = 0.3f;
    private AvatarPreview.OnAvatarChange m_OnAvatarChangeFunc;
    public TimeControl timeControl;
    private Material m_FloorMaterial;
    private Material m_FloorMaterialSmall;
    private Material m_ShadowMaskMaterial;
    private Material m_ShadowPlaneMaterial;
    private PreviewRenderUtility m_PreviewUtility;
    private GameObject m_PreviewInstance;
    private GameObject m_ReferenceInstance;
    private GameObject m_DirectionInstance;
    private GameObject m_PivotInstance;
    private GameObject m_RootInstance;
    private float m_BoundingVolumeScale;
    private Motion m_SourcePreviewMotion;
    private Animator m_SourceScenePreviewAnimator;
    private Texture2D m_FloorTexture;
    private Mesh m_FloorPlane;
    private bool m_ShowReference;
    private bool m_IKOnFeet;
    private bool m_IsValid;
    private float m_PrevFloorHeight;
    private float m_NextFloorHeight;
    private static AvatarPreview.Styles s_Styles;
    protected AvatarPreview.ViewTool m_ViewTool;

    public AvatarPreview.OnAvatarChange OnAvatarChangeFunc
    {
      set
      {
        this.m_OnAvatarChangeFunc = value;
      }
    }

    public bool IKOnFeet
    {
      get
      {
        return this.m_IKOnFeet;
      }
    }

    public bool ShowIKOnFeetButton
    {
      get
      {
        return this.m_ShowIKOnFeetButton;
      }
      set
      {
        this.m_ShowIKOnFeetButton = value;
      }
    }

    public Animator Animator
    {
      get
      {
        if ((Object) this.m_PreviewInstance != (Object) null)
          return this.m_PreviewInstance.GetComponent(typeof (Animator)) as Animator;
        return (Animator) null;
      }
    }

    public GameObject PreviewObject
    {
      get
      {
        return this.m_PreviewInstance;
      }
    }

    public ModelImporterAnimationType animationClipType
    {
      get
      {
        return AvatarPreview.GetAnimationType(this.m_SourcePreviewMotion);
      }
    }

    public Vector3 bodyPosition
    {
      get
      {
        if ((bool) ((Object) this.Animator) && this.Animator.isHuman)
          return this.Animator.bodyPosition;
        return GameObjectInspector.GetRenderableCenterRecurse(this.m_PreviewInstance, 2, 8);
      }
    }

    protected AvatarPreview.ViewTool viewTool
    {
      get
      {
        Event current = Event.current;
        if (this.m_ViewTool == AvatarPreview.ViewTool.None)
        {
          bool flag1 = current.control && Application.platform == RuntimePlatform.OSXEditor;
          bool actionKey = EditorGUI.actionKey;
          bool flag2 = !actionKey && !flag1 && !current.alt;
          if (current.button <= 0 && flag2 || current.button <= 0 && actionKey || current.button == 2)
            this.m_ViewTool = AvatarPreview.ViewTool.Pan;
          else if (current.button <= 0 && flag1 || current.button == 1 && current.alt)
            this.m_ViewTool = AvatarPreview.ViewTool.Zoom;
          else if (current.button <= 0 && current.alt || current.button == 1)
            this.m_ViewTool = AvatarPreview.ViewTool.Orbit;
        }
        return this.m_ViewTool;
      }
    }

    protected MouseCursor currentCursor
    {
      get
      {
        switch (this.m_ViewTool)
        {
          case AvatarPreview.ViewTool.Pan:
            return MouseCursor.Pan;
          case AvatarPreview.ViewTool.Zoom:
            return MouseCursor.Zoom;
          case AvatarPreview.ViewTool.Orbit:
            return MouseCursor.Orbit;
          default:
            return MouseCursor.Arrow;
        }
      }
    }

    public AvatarPreview(Animator previewObjectInScene, Motion objectOnSameAsset)
    {
      this.InitInstance(previewObjectInScene, objectOnSameAsset);
    }

    private void SetPreviewCharacterEnabled(bool enabled, bool showReference)
    {
      if ((Object) this.m_PreviewInstance != (Object) null)
        GameObjectInspector.SetEnabledRecursive(this.m_PreviewInstance, enabled);
      GameObjectInspector.SetEnabledRecursive(this.m_ReferenceInstance, showReference && enabled);
      GameObjectInspector.SetEnabledRecursive(this.m_DirectionInstance, showReference && enabled);
      GameObjectInspector.SetEnabledRecursive(this.m_PivotInstance, showReference && enabled);
      GameObjectInspector.SetEnabledRecursive(this.m_RootInstance, showReference && enabled);
    }

    private static AnimationClip GetFirstAnimationClipFromMotion(Motion motion)
    {
      AnimationClip animationClip = motion as AnimationClip;
      if ((bool) ((Object) animationClip))
        return animationClip;
      UnityEditor.Animations.BlendTree blendTree = motion as UnityEditor.Animations.BlendTree;
      if ((bool) ((Object) blendTree))
      {
        AnimationClip[] animationClipsFlattened = blendTree.GetAnimationClipsFlattened();
        if (animationClipsFlattened.Length > 0)
          return animationClipsFlattened[0];
      }
      return (AnimationClip) null;
    }

    public static ModelImporterAnimationType GetAnimationType(GameObject go)
    {
      Animator component = go.GetComponent<Animator>();
      if ((bool) ((Object) component))
      {
        Avatar avatar = !((Object) component != (Object) null) ? (Avatar) null : component.avatar;
        return (bool) ((Object) avatar) && avatar.isHuman ? ModelImporterAnimationType.Human : ModelImporterAnimationType.Generic;
      }
      return (Object) go.GetComponent<Animation>() != (Object) null ? ModelImporterAnimationType.Legacy : ModelImporterAnimationType.None;
    }

    public static ModelImporterAnimationType GetAnimationType(Motion motion)
    {
      AnimationClip animationClipFromMotion = AvatarPreview.GetFirstAnimationClipFromMotion(motion);
      if (!(bool) ((Object) animationClipFromMotion))
        return ModelImporterAnimationType.None;
      if (animationClipFromMotion.legacy)
        return ModelImporterAnimationType.Legacy;
      return animationClipFromMotion.humanMotion ? ModelImporterAnimationType.Human : ModelImporterAnimationType.Generic;
    }

    public static bool IsValidPreviewGameObject(GameObject target, ModelImporterAnimationType requiredClipType)
    {
      if ((Object) target != (Object) null && !target.activeSelf)
        Debug.LogWarning((object) "Can't preview inactive object, using fallback object");
      if ((Object) target != (Object) null && target.activeSelf && GameObjectInspector.HasRenderablePartsRecurse(target))
        return (requiredClipType == ModelImporterAnimationType.None ? 0 : (AvatarPreview.GetAnimationType(target) != requiredClipType ? 1 : 0)) == 0;
      return false;
    }

    public static GameObject FindBestFittingRenderableGameObjectFromModelAsset(Object asset, ModelImporterAnimationType animationType)
    {
      if (asset == (Object) null)
        return (GameObject) null;
      ModelImporter atPath = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(asset)) as ModelImporter;
      if ((Object) atPath == (Object) null)
        return (GameObject) null;
      GameObject target = AssetDatabase.LoadMainAssetAtPath(atPath.CalculateBestFittingPreviewGameObject()) as GameObject;
      if (AvatarPreview.IsValidPreviewGameObject(target, animationType))
        return target;
      return (GameObject) null;
    }

    private static GameObject CalculatePreviewGameObject(Animator selectedAnimator, Motion motion, ModelImporterAnimationType animationType)
    {
      AnimationClip animationClipFromMotion = AvatarPreview.GetFirstAnimationClipFromMotion(motion);
      GameObject preview = AvatarPreviewSelection.GetPreview(animationType);
      if (AvatarPreview.IsValidPreviewGameObject(preview, ModelImporterAnimationType.None))
        return preview;
      if ((Object) selectedAnimator != (Object) null && AvatarPreview.IsValidPreviewGameObject(selectedAnimator.gameObject, animationType))
        return selectedAnimator.gameObject;
      GameObject objectFromModelAsset = AvatarPreview.FindBestFittingRenderableGameObjectFromModelAsset((Object) animationClipFromMotion, animationType);
      if ((Object) objectFromModelAsset != (Object) null)
        return objectFromModelAsset;
      if (animationType == ModelImporterAnimationType.Human)
        return AvatarPreview.GetHumanoidFallback();
      if (animationType == ModelImporterAnimationType.Generic)
        return AvatarPreview.GetGenericAnimationFallback();
      return (GameObject) null;
    }

    private static GameObject GetGenericAnimationFallback()
    {
      return (GameObject) EditorGUIUtility.Load("Avatar/DefaultGeneric.fbx");
    }

    private static GameObject GetHumanoidFallback()
    {
      return (GameObject) EditorGUIUtility.Load("Avatar/DefaultAvatar.fbx");
    }

    public void ResetPreviewInstance()
    {
      Object.DestroyImmediate((Object) this.m_PreviewInstance);
      this.SetupBounds(AvatarPreview.CalculatePreviewGameObject(this.m_SourceScenePreviewAnimator, this.m_SourcePreviewMotion, this.animationClipType));
    }

    private void SetupBounds(GameObject go)
    {
      this.m_IsValid = (Object) go != (Object) null && (Object) go != (Object) AvatarPreview.GetGenericAnimationFallback();
      if (!((Object) go != (Object) null))
        return;
      this.m_PreviewInstance = EditorUtility.InstantiateForAnimatorPreview((Object) go);
      Bounds bounds = new Bounds(this.m_PreviewInstance.transform.position, Vector3.zero);
      GameObjectInspector.GetRenderableBoundsRecurse(ref bounds, this.m_PreviewInstance);
      this.m_BoundingVolumeScale = Mathf.Max(bounds.size.x, Mathf.Max(bounds.size.y, bounds.size.z));
      if ((bool) ((Object) this.Animator) && this.Animator.isHuman)
        this.m_AvatarScale = this.m_ZoomFactor = this.Animator.humanScale;
      else
        this.m_AvatarScale = this.m_ZoomFactor = this.m_BoundingVolumeScale / 2f;
    }

    private void InitInstance(Animator scenePreviewObject, Motion motion)
    {
      this.m_SourcePreviewMotion = motion;
      this.m_SourceScenePreviewAnimator = scenePreviewObject;
      if ((Object) this.m_PreviewInstance == (Object) null)
        this.SetupBounds(AvatarPreview.CalculatePreviewGameObject(scenePreviewObject, motion, this.animationClipType));
      if (this.timeControl == null)
        this.timeControl = new TimeControl();
      if ((Object) this.m_ReferenceInstance == (Object) null)
      {
        this.m_ReferenceInstance = (GameObject) Object.Instantiate(EditorGUIUtility.Load("Avatar/dial_flat.prefab"), Vector3.zero, Quaternion.identity);
        EditorUtility.InitInstantiatedPreviewRecursive(this.m_ReferenceInstance);
      }
      if ((Object) this.m_DirectionInstance == (Object) null)
      {
        this.m_DirectionInstance = (GameObject) Object.Instantiate(EditorGUIUtility.Load("Avatar/arrow.fbx"), Vector3.zero, Quaternion.identity);
        EditorUtility.InitInstantiatedPreviewRecursive(this.m_DirectionInstance);
      }
      if ((Object) this.m_PivotInstance == (Object) null)
      {
        this.m_PivotInstance = (GameObject) Object.Instantiate(EditorGUIUtility.Load("Avatar/root.fbx"), Vector3.zero, Quaternion.identity);
        EditorUtility.InitInstantiatedPreviewRecursive(this.m_PivotInstance);
      }
      if ((Object) this.m_RootInstance == (Object) null)
      {
        this.m_RootInstance = (GameObject) Object.Instantiate(EditorGUIUtility.Load("Avatar/root.fbx"), Vector3.zero, Quaternion.identity);
        EditorUtility.InitInstantiatedPreviewRecursive(this.m_RootInstance);
      }
      this.m_IKOnFeet = EditorPrefs.GetBool("AvatarpreviewShowIK", false);
      this.m_ShowReference = EditorPrefs.GetBool("AvatarpreviewShowReference", true);
      this.timeControl.playbackSpeed = EditorPrefs.GetFloat("AvatarpreviewSpeed", 1f);
      this.SetPreviewCharacterEnabled(false, false);
    }

    private void Init()
    {
      if (this.m_PreviewUtility == null)
      {
        this.m_PreviewUtility = new PreviewRenderUtility(true);
        this.m_PreviewUtility.m_CameraFieldOfView = 30f;
        this.m_PreviewUtility.m_Camera.cullingMask = 1 << Camera.PreviewCullingLayer;
      }
      if (AvatarPreview.s_Styles == null)
        AvatarPreview.s_Styles = new AvatarPreview.Styles();
      if ((Object) this.m_FloorPlane == (Object) null)
        this.m_FloorPlane = Resources.GetBuiltinResource(typeof (Mesh), "New-Plane.fbx") as Mesh;
      if ((Object) this.m_FloorTexture == (Object) null)
        this.m_FloorTexture = (Texture2D) EditorGUIUtility.Load("Avatar/Textures/AvatarFloor.png");
      if ((Object) this.m_FloorMaterial == (Object) null)
      {
        this.m_FloorMaterial = new Material(EditorGUIUtility.LoadRequired("Previews/PreviewPlaneWithShadow.shader") as Shader);
        this.m_FloorMaterial.mainTexture = (Texture) this.m_FloorTexture;
        this.m_FloorMaterial.mainTextureScale = Vector2.one * 5f * 4f;
        this.m_FloorMaterial.SetVector("_Alphas", new Vector4(0.5f, 0.3f, 0.0f, 0.0f));
        this.m_FloorMaterial.hideFlags = HideFlags.HideAndDontSave;
        this.m_FloorMaterialSmall = new Material(this.m_FloorMaterial);
        this.m_FloorMaterialSmall.mainTextureScale = Vector2.one * 0.2f * 4f;
        this.m_FloorMaterialSmall.hideFlags = HideFlags.HideAndDontSave;
      }
      if ((Object) this.m_ShadowMaskMaterial == (Object) null)
      {
        this.m_ShadowMaskMaterial = new Material(EditorGUIUtility.LoadRequired("Previews/PreviewShadowMask.shader") as Shader);
        this.m_ShadowMaskMaterial.hideFlags = HideFlags.HideAndDontSave;
      }
      if (!((Object) this.m_ShadowPlaneMaterial == (Object) null))
        return;
      this.m_ShadowPlaneMaterial = new Material(EditorGUIUtility.LoadRequired("Previews/PreviewShadowPlaneClip.shader") as Shader);
      this.m_ShadowPlaneMaterial.hideFlags = HideFlags.HideAndDontSave;
    }

    public void OnDestroy()
    {
      if (this.m_PreviewUtility != null)
      {
        this.m_PreviewUtility.Cleanup();
        this.m_PreviewUtility = (PreviewRenderUtility) null;
      }
      Object.DestroyImmediate((Object) this.m_PreviewInstance);
      Object.DestroyImmediate((Object) this.m_FloorMaterial);
      Object.DestroyImmediate((Object) this.m_FloorMaterialSmall);
      Object.DestroyImmediate((Object) this.m_ShadowMaskMaterial);
      Object.DestroyImmediate((Object) this.m_ShadowPlaneMaterial);
      Object.DestroyImmediate((Object) this.m_ReferenceInstance);
      Object.DestroyImmediate((Object) this.m_RootInstance);
      Object.DestroyImmediate((Object) this.m_PivotInstance);
      Object.DestroyImmediate((Object) this.m_DirectionInstance);
      if (this.timeControl == null)
        return;
      this.timeControl.OnDisable();
    }

    public void DoSelectionChange()
    {
      this.m_OnAvatarChangeFunc();
    }

    private float PreviewSlider(float val, float snapThreshold)
    {
      val = GUILayout.HorizontalSlider(val, 0.1f, 2f, AvatarPreview.s_Styles.preSlider, AvatarPreview.s_Styles.preSliderThumb, GUILayout.MaxWidth(64f));
      if ((double) val > 0.25 - (double) snapThreshold && (double) val < 0.25 + (double) snapThreshold)
        val = 0.25f;
      else if ((double) val > 0.5 - (double) snapThreshold && (double) val < 0.5 + (double) snapThreshold)
        val = 0.5f;
      else if ((double) val > 0.75 - (double) snapThreshold && (double) val < 0.75 + (double) snapThreshold)
        val = 0.75f;
      else if ((double) val > 1.0 - (double) snapThreshold && (double) val < 1.0 + (double) snapThreshold)
        val = 1f;
      else if ((double) val > 1.25 - (double) snapThreshold && (double) val < 1.25 + (double) snapThreshold)
        val = 1.25f;
      else if ((double) val > 1.5 - (double) snapThreshold && (double) val < 1.5 + (double) snapThreshold)
        val = 1.5f;
      else if ((double) val > 1.75 - (double) snapThreshold && (double) val < 1.75 + (double) snapThreshold)
        val = 1.75f;
      return val;
    }

    public void DoPreviewSettings()
    {
      this.Init();
      if (this.m_ShowIKOnFeetButton)
      {
        EditorGUI.BeginChangeCheck();
        this.m_IKOnFeet = GUILayout.Toggle(this.m_IKOnFeet, AvatarPreview.s_Styles.ik, AvatarPreview.s_Styles.preButton, new GUILayoutOption[0]);
        if (EditorGUI.EndChangeCheck())
          EditorPrefs.SetBool("AvatarpreviewShowIK", this.m_IKOnFeet);
      }
      EditorGUI.BeginChangeCheck();
      this.m_ShowReference = GUILayout.Toggle(this.m_ShowReference, AvatarPreview.s_Styles.pivot, AvatarPreview.s_Styles.preButton, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
        EditorPrefs.SetBool("AvatarpreviewShowReference", this.m_ShowReference);
      GUILayout.Box(AvatarPreview.s_Styles.speedScale, AvatarPreview.s_Styles.preLabel, new GUILayoutOption[0]);
      EditorGUI.BeginChangeCheck();
      this.timeControl.playbackSpeed = this.PreviewSlider(this.timeControl.playbackSpeed, 0.03f);
      if (EditorGUI.EndChangeCheck())
        EditorPrefs.SetFloat("AvatarpreviewSpeed", this.timeControl.playbackSpeed);
      GUILayout.Label(this.timeControl.playbackSpeed.ToString("f2"), AvatarPreview.s_Styles.preLabel, new GUILayoutOption[0]);
    }

    private RenderTexture RenderPreviewShadowmap(Light light, float scale, Vector3 center, Vector3 floorPos, out Matrix4x4 outShadowMatrix)
    {
      Camera camera = this.m_PreviewUtility.m_Camera;
      camera.orthographic = true;
      camera.orthographicSize = scale * 2f;
      camera.nearClipPlane = 1f * scale;
      camera.farClipPlane = 25f * scale;
      camera.transform.rotation = light.transform.rotation;
      camera.transform.position = center - light.transform.forward * (scale * 5.5f);
      CameraClearFlags clearFlags = camera.clearFlags;
      camera.clearFlags = CameraClearFlags.Color;
      Color backgroundColor = camera.backgroundColor;
      camera.backgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
      RenderTexture targetTexture = camera.targetTexture;
      RenderTexture temporary = RenderTexture.GetTemporary(256, 256, 16);
      temporary.isPowerOfTwo = true;
      temporary.wrapMode = TextureWrapMode.Clamp;
      temporary.filterMode = UnityEngine.FilterMode.Bilinear;
      camera.targetTexture = temporary;
      this.SetPreviewCharacterEnabled(true, false);
      this.m_PreviewUtility.m_Camera.Render();
      RenderTexture.active = temporary;
      GL.PushMatrix();
      GL.LoadOrtho();
      this.m_ShadowMaskMaterial.SetPass(0);
      GL.Begin(7);
      GL.Vertex3(0.0f, 0.0f, -99f);
      GL.Vertex3(1f, 0.0f, -99f);
      GL.Vertex3(1f, 1f, -99f);
      GL.Vertex3(0.0f, 1f, -99f);
      GL.End();
      GL.LoadProjectionMatrix(camera.projectionMatrix);
      GL.LoadIdentity();
      GL.MultMatrix(camera.worldToCameraMatrix);
      this.m_ShadowPlaneMaterial.SetPass(0);
      GL.Begin(7);
      float num = 5f * scale;
      GL.Vertex(floorPos + new Vector3(-num, 0.0f, -num));
      GL.Vertex(floorPos + new Vector3(num, 0.0f, -num));
      GL.Vertex(floorPos + new Vector3(num, 0.0f, num));
      GL.Vertex(floorPos + new Vector3(-num, 0.0f, num));
      GL.End();
      GL.PopMatrix();
      Matrix4x4 matrix4x4 = Matrix4x4.TRS(new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, new Vector3(0.5f, 0.5f, 0.5f));
      outShadowMatrix = matrix4x4 * camera.projectionMatrix * camera.worldToCameraMatrix;
      camera.orthographic = false;
      camera.clearFlags = clearFlags;
      camera.backgroundColor = backgroundColor;
      camera.targetTexture = targetTexture;
      return temporary;
    }

    public void DoRenderPreview(Rect previewRect, GUIStyle background)
    {
      this.m_PreviewUtility.BeginPreview(previewRect, background);
      Vector3 bodyPosition = this.bodyPosition;
      Quaternion quaternion1;
      Vector3 rootPos;
      Quaternion bodyRot;
      Vector3 pivotPos;
      if ((bool) ((Object) this.Animator) && this.Animator.isHuman)
      {
        quaternion1 = this.Animator.rootRotation;
        rootPos = this.Animator.rootPosition;
        bodyRot = this.Animator.bodyRotation;
        pivotPos = this.Animator.pivotPosition;
      }
      else if ((bool) ((Object) this.Animator) && this.Animator.hasRootMotion)
      {
        quaternion1 = this.Animator.rootRotation;
        rootPos = this.Animator.rootPosition;
        bodyRot = Quaternion.identity;
        pivotPos = Vector3.zero;
      }
      else
      {
        quaternion1 = Quaternion.identity;
        rootPos = Vector3.zero;
        bodyRot = Quaternion.identity;
        pivotPos = Vector3.zero;
      }
      bool oldFog = this.SetupPreviewLightingAndFx();
      Vector3 forward = bodyRot * Vector3.forward;
      forward[1] = 0.0f;
      Quaternion directionRot = Quaternion.LookRotation(forward);
      Vector3 directionPos = rootPos;
      this.PositionPreviewObjects(quaternion1, pivotPos, bodyRot, bodyPosition, directionRot, quaternion1, rootPos, directionPos, this.m_AvatarScale);
      bool flag1 = (double) Mathf.Abs(this.m_NextFloorHeight - this.m_PrevFloorHeight) > (double) this.m_ZoomFactor * 0.00999999977648258;
      float num1;
      float num2;
      if (flag1)
      {
        float num3 = (double) this.m_NextFloorHeight >= (double) this.m_PrevFloorHeight ? 0.8f : 0.2f;
        num1 = (double) this.timeControl.normalizedTime >= (double) num3 ? this.m_NextFloorHeight : this.m_PrevFloorHeight;
        num2 = Mathf.Clamp01(Mathf.Abs(this.timeControl.normalizedTime - num3) / 0.2f);
      }
      else
      {
        num1 = this.m_PrevFloorHeight;
        num2 = 1f;
      }
      Quaternion identity = Quaternion.identity;
      Vector3 vector3 = new Vector3(0.0f, 0.0f, 0.0f);
      Vector3 position = this.m_ReferenceInstance.transform.position;
      position.y = num1;
      Matrix4x4 outShadowMatrix;
      RenderTexture temp = this.RenderPreviewShadowmap(this.m_PreviewUtility.m_Light[0], this.m_BoundingVolumeScale / 2f, bodyPosition, position, out outShadowMatrix);
      this.m_PreviewUtility.m_Camera.nearClipPlane = 0.5f * this.m_ZoomFactor;
      this.m_PreviewUtility.m_Camera.farClipPlane = 100f * this.m_AvatarScale;
      Quaternion quaternion2 = Quaternion.Euler(-this.m_PreviewDir.y, -this.m_PreviewDir.x, 0.0f);
      this.m_PreviewUtility.m_Camera.transform.position = quaternion2 * Vector3.forward * -5.5f * this.m_ZoomFactor + bodyPosition + this.m_PivotPositionOffset;
      this.m_PreviewUtility.m_Camera.transform.rotation = quaternion2;
      position.y = num1;
      Material floorMaterial = this.m_FloorMaterial;
      Matrix4x4 matrix = Matrix4x4.TRS(position, identity, Vector3.one * 5f * this.m_AvatarScale);
      floorMaterial.mainTextureOffset = -new Vector2(position.x, position.z) * 5f * 0.08f * (1f / this.m_AvatarScale);
      floorMaterial.SetTexture("_ShadowTexture", (Texture) temp);
      floorMaterial.SetMatrix("_ShadowTextureMatrix", outShadowMatrix);
      floorMaterial.SetVector("_Alphas", new Vector4(0.5f * num2, 0.3f * num2, 0.0f, 0.0f));
      Graphics.DrawMesh(this.m_FloorPlane, matrix, floorMaterial, Camera.PreviewCullingLayer, this.m_PreviewUtility.m_Camera, 0);
      if (flag1)
      {
        bool flag2 = (double) this.m_NextFloorHeight > (double) this.m_PrevFloorHeight;
        float b = !flag2 ? this.m_PrevFloorHeight : this.m_NextFloorHeight;
        float a = !flag2 ? this.m_NextFloorHeight : this.m_PrevFloorHeight;
        float num3 = ((double) b != (double) num1 ? 1f : 1f - num2) * Mathf.InverseLerp(a, b, rootPos.y);
        position.y = b;
        Material floorMaterialSmall = this.m_FloorMaterialSmall;
        floorMaterialSmall.mainTextureOffset = -new Vector2(position.x, position.z) * 0.2f * 0.08f;
        floorMaterialSmall.SetTexture("_ShadowTexture", (Texture) temp);
        floorMaterialSmall.SetMatrix("_ShadowTextureMatrix", outShadowMatrix);
        floorMaterialSmall.SetVector("_Alphas", new Vector4(0.5f * num3, 0.0f, 0.0f, 0.0f));
        Graphics.DrawMesh(this.m_FloorPlane, Matrix4x4.TRS(position, identity, Vector3.one * 0.2f * this.m_AvatarScale), floorMaterialSmall, Camera.PreviewCullingLayer, this.m_PreviewUtility.m_Camera, 0);
      }
      this.SetPreviewCharacterEnabled(true, this.m_ShowReference);
      this.m_PreviewUtility.m_Camera.Render();
      this.SetPreviewCharacterEnabled(false, false);
      AvatarPreview.TeardownPreviewLightingAndFx(oldFog);
      RenderTexture.ReleaseTemporary(temp);
    }

    private bool SetupPreviewLightingAndFx()
    {
      this.m_PreviewUtility.m_Light[0].intensity = 1.4f;
      this.m_PreviewUtility.m_Light[0].transform.rotation = Quaternion.Euler(40f, 40f, 0.0f);
      this.m_PreviewUtility.m_Light[1].intensity = 1.4f;
      InternalEditorUtility.SetCustomLighting(this.m_PreviewUtility.m_Light, new Color(0.1f, 0.1f, 0.1f, 0.0f));
      bool fog = RenderSettings.fog;
      Unsupported.SetRenderSettingsUseFogNoDirty(false);
      return fog;
    }

    private static void TeardownPreviewLightingAndFx(bool oldFog)
    {
      Unsupported.SetRenderSettingsUseFogNoDirty(oldFog);
      InternalEditorUtility.RemoveCustomLighting();
    }

    private void PositionPreviewObjects(Quaternion pivotRot, Vector3 pivotPos, Quaternion bodyRot, Vector3 bodyPos, Quaternion directionRot, Quaternion rootRot, Vector3 rootPos, Vector3 directionPos, float scale)
    {
      this.m_ReferenceInstance.transform.position = rootPos;
      this.m_ReferenceInstance.transform.rotation = rootRot;
      this.m_ReferenceInstance.transform.localScale = Vector3.one * scale * 1.25f;
      this.m_DirectionInstance.transform.position = directionPos;
      this.m_DirectionInstance.transform.rotation = directionRot;
      this.m_DirectionInstance.transform.localScale = Vector3.one * scale * 2f;
      this.m_PivotInstance.transform.position = pivotPos;
      this.m_PivotInstance.transform.rotation = pivotRot;
      this.m_PivotInstance.transform.localScale = Vector3.one * scale * 0.1f;
      this.m_RootInstance.transform.position = bodyPos;
      this.m_RootInstance.transform.rotation = bodyRot;
      this.m_RootInstance.transform.localScale = Vector3.one * scale * 0.25f;
      if (!(bool) ((Object) this.Animator))
        return;
      float normalizedTime = this.timeControl.normalizedTime;
      float num1 = this.timeControl.deltaTime / (this.timeControl.stopTime - this.timeControl.startTime);
      if ((double) normalizedTime - (double) num1 < 0.0 || (double) normalizedTime - (double) num1 >= 1.0)
        this.m_PrevFloorHeight = this.m_NextFloorHeight;
      if ((double) this.m_LastNormalizedTime != -1000.0 && (double) this.timeControl.startTime == (double) this.m_LastStartTime && (double) this.timeControl.stopTime == (double) this.m_LastStopTime)
      {
        float num2 = normalizedTime - num1 - this.m_LastNormalizedTime;
        float num3;
        if ((double) num2 > 0.5)
          num3 = num2 - 1f;
        else if ((double) num2 < -0.5)
          num3 = num2 + 1f;
      }
      this.m_LastNormalizedTime = normalizedTime;
      this.m_LastStartTime = this.timeControl.startTime;
      this.m_LastStopTime = this.timeControl.stopTime;
      if (this.m_NextTargetIsForward)
        this.m_NextFloorHeight = this.Animator.targetPosition.y;
      else
        this.m_PrevFloorHeight = this.Animator.targetPosition.y;
      this.m_NextTargetIsForward = !this.m_NextTargetIsForward;
      this.Animator.SetTarget(AvatarTarget.Root, !this.m_NextTargetIsForward ? 0.0f : 1f);
    }

    public void AvatarTimeControlGUI(Rect rect)
    {
      Rect rect1 = rect;
      rect1.height = 21f;
      this.timeControl.DoTimeControl(rect1);
      rect.y = rect.yMax - 20f;
      float num = this.timeControl.currentTime - this.timeControl.startTime;
      EditorGUI.DropShadowLabel(new Rect(rect.x, rect.y, rect.width, 20f), string.Format("{0,2}:{1:00} ({2:000.0%}) Frame {3}", (object) (int) num, (object) this.Repeat(Mathf.FloorToInt(num * (float) this.fps), this.fps), (object) this.timeControl.normalizedTime, (object) Mathf.FloorToInt(this.timeControl.currentTime * (float) this.fps)));
    }

    protected void HandleMouseDown(Event evt, int id, Rect previewRect)
    {
      if (this.viewTool == AvatarPreview.ViewTool.None || !previewRect.Contains(evt.mousePosition))
        return;
      EditorGUIUtility.SetWantsMouseJumping(1);
      evt.Use();
      GUIUtility.hotControl = id;
    }

    protected void HandleMouseUp(Event evt, int id)
    {
      if (GUIUtility.hotControl != id)
        return;
      this.m_ViewTool = AvatarPreview.ViewTool.None;
      GUIUtility.hotControl = 0;
      EditorGUIUtility.SetWantsMouseJumping(0);
      evt.Use();
    }

    protected void HandleMouseDrag(Event evt, int id, Rect previewRect)
    {
      if ((Object) this.m_PreviewInstance == (Object) null || GUIUtility.hotControl != id)
        return;
      switch (this.m_ViewTool)
      {
        case AvatarPreview.ViewTool.Pan:
          this.DoAvatarPreviewPan(evt);
          break;
        case AvatarPreview.ViewTool.Zoom:
          this.DoAvatarPreviewZoom(evt, (float) (-(double) HandleUtility.niceMouseDeltaZoom * (!evt.shift ? 0.5 : 2.0)));
          break;
        case AvatarPreview.ViewTool.Orbit:
          this.DoAvatarPreviewOrbit(evt, previewRect);
          break;
        default:
          Debug.Log((object) "Enum value not handled");
          break;
      }
    }

    protected void HandleViewTool(Event evt, EventType eventType, int id, Rect previewRect)
    {
      switch (eventType)
      {
        case EventType.MouseDown:
          this.HandleMouseDown(evt, id, previewRect);
          break;
        case EventType.MouseUp:
          this.HandleMouseUp(evt, id);
          break;
        case EventType.MouseDrag:
          this.HandleMouseDrag(evt, id, previewRect);
          break;
        case EventType.ScrollWheel:
          this.DoAvatarPreviewZoom(evt, HandleUtility.niceMouseDeltaZoom * (!evt.shift ? 0.5f : 2f));
          break;
      }
    }

    public void DoAvatarPreviewDrag(EventType type)
    {
      if (type == EventType.DragUpdated)
      {
        DragAndDrop.visualMode = DragAndDropVisualMode.Link;
      }
      else
      {
        if (type != EventType.DragPerform)
          return;
        DragAndDrop.visualMode = DragAndDropVisualMode.Link;
        GameObject objectReference = DragAndDrop.objectReferences[0] as GameObject;
        if (!(bool) ((Object) objectReference))
          return;
        DragAndDrop.AcceptDrag();
        this.SetPreview(objectReference);
      }
    }

    public void DoAvatarPreviewOrbit(Event evt, Rect previewRect)
    {
      this.m_PreviewDir -= evt.delta * (!evt.shift ? 1f : 3f) / Mathf.Min(previewRect.width, previewRect.height) * 140f;
      this.m_PreviewDir.y = Mathf.Clamp(this.m_PreviewDir.y, -90f, 90f);
      evt.Use();
    }

    public void DoAvatarPreviewPan(Event evt)
    {
      Camera camera = this.m_PreviewUtility.m_Camera;
      Vector3 position = camera.WorldToScreenPoint(this.bodyPosition + this.m_PivotPositionOffset) + new Vector3(-evt.delta.x, evt.delta.y, 0.0f) * Mathf.Lerp(0.25f, 2f, this.m_ZoomFactor * 0.5f);
      this.m_PivotPositionOffset += camera.ScreenToWorldPoint(position) - this.bodyPosition + this.m_PivotPositionOffset;
      evt.Use();
    }

    public void DoAvatarPreviewFrame(Event evt, EventType type, Rect previewRect)
    {
      if (type == EventType.KeyDown && evt.keyCode == KeyCode.F)
      {
        this.m_PivotPositionOffset = Vector3.zero;
        this.m_ZoomFactor = this.m_AvatarScale;
        evt.Use();
      }
      if (type != EventType.KeyDown || Event.current.keyCode != KeyCode.G)
        return;
      this.m_PivotPositionOffset = this.GetCurrentMouseWorldPosition(evt, previewRect) - this.bodyPosition;
      evt.Use();
    }

    protected Vector3 GetCurrentMouseWorldPosition(Event evt, Rect previewRect)
    {
      Camera camera = this.m_PreviewUtility.m_Camera;
      float scaleFactor = this.m_PreviewUtility.GetScaleFactor(previewRect.width, previewRect.height);
      return camera.ScreenToWorldPoint(new Vector3((evt.mousePosition.x - previewRect.x) * scaleFactor, (previewRect.height - (evt.mousePosition.y - previewRect.y)) * scaleFactor, 0.0f) { z = Vector3.Distance(this.bodyPosition, camera.transform.position) });
    }

    public void DoAvatarPreviewZoom(Event evt, float delta)
    {
      this.m_ZoomFactor += this.m_ZoomFactor * (float) (-(double) delta * 0.0500000007450581);
      this.m_ZoomFactor = Mathf.Max(this.m_ZoomFactor, this.m_AvatarScale / 10f);
      evt.Use();
    }

    public void DoAvatarPreview(Rect rect, GUIStyle background)
    {
      this.Init();
      Rect position1 = new Rect(rect.xMax - 16f, rect.yMax - 16f, 16f, 16f);
      if (EditorGUI.ButtonMouseDown(position1, GUIContent.none, FocusType.Passive, GUIStyle.none))
      {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Auto"), false, new GenericMenu.MenuFunction2(this.SetPreviewAvatarOption), (object) AvatarPreview.PreviewPopupOptions.Auto);
        genericMenu.AddItem(new GUIContent("Unity Model"), false, new GenericMenu.MenuFunction2(this.SetPreviewAvatarOption), (object) AvatarPreview.PreviewPopupOptions.DefaultModel);
        genericMenu.AddItem(new GUIContent("Other..."), false, new GenericMenu.MenuFunction2(this.SetPreviewAvatarOption), (object) AvatarPreview.PreviewPopupOptions.Other);
        genericMenu.ShowAsContext();
      }
      Rect rect1 = rect;
      rect1.yMin += 21f;
      rect1.height = Mathf.Max(rect1.height, 64f);
      int controlId1 = GUIUtility.GetControlID(this.m_PreviewHint, FocusType.Native, rect1);
      Event current = Event.current;
      if (current.GetTypeForControl(controlId1) == EventType.Repaint && this.m_IsValid)
      {
        this.DoRenderPreview(rect1, background);
        this.m_PreviewUtility.EndAndDrawPreview(rect1);
      }
      this.AvatarTimeControlGUI(rect);
      GUI.DrawTexture(position1, AvatarPreview.s_Styles.avatarIcon.image);
      int controlId2 = GUIUtility.GetControlID(this.m_PreviewSceneHint, FocusType.Native);
      EventType typeForControl = current.GetTypeForControl(controlId2);
      this.DoAvatarPreviewDrag(typeForControl);
      this.HandleViewTool(current, typeForControl, controlId2, rect1);
      this.DoAvatarPreviewFrame(current, typeForControl, rect1);
      if (!this.m_IsValid)
      {
        Rect position2 = rect1;
        position2.yMax -= (float) ((double) position2.height / 2.0 - 16.0);
        EditorGUI.DropShadowLabel(position2, "No model is available for preview.\nPlease drag a model into this Preview Area.");
      }
      if (current.type == EventType.ExecuteCommand && current.commandName == "ObjectSelectorUpdated" && ObjectSelector.get.objectSelectorID == this.m_ModelSelectorId)
      {
        this.SetPreview(ObjectSelector.GetCurrentObject() as GameObject);
        current.Use();
      }
      if (current.type != EventType.Repaint)
        return;
      EditorGUIUtility.AddCursorRect(rect1, this.currentCursor);
    }

    private void SetPreviewAvatarOption(object obj)
    {
      switch ((AvatarPreview.PreviewPopupOptions) obj)
      {
        case AvatarPreview.PreviewPopupOptions.Auto:
          this.SetPreview((GameObject) null);
          break;
        case AvatarPreview.PreviewPopupOptions.DefaultModel:
          this.SetPreview(AvatarPreview.GetHumanoidFallback());
          break;
        case AvatarPreview.PreviewPopupOptions.Other:
          ObjectSelector.get.Show((Object) null, typeof (GameObject), (SerializedProperty) null, false);
          ObjectSelector.get.objectSelectorID = this.m_ModelSelectorId;
          break;
      }
    }

    private void SetPreview(GameObject gameObject)
    {
      AvatarPreviewSelection.SetPreview(this.animationClipType, gameObject);
      Object.DestroyImmediate((Object) this.m_PreviewInstance);
      this.InitInstance(this.m_SourceScenePreviewAnimator, this.m_SourcePreviewMotion);
      if (this.m_OnAvatarChangeFunc == null)
        return;
      this.m_OnAvatarChangeFunc();
    }

    private int Repeat(int t, int length)
    {
      return (t % length + length) % length;
    }

    private class Styles
    {
      public GUIContent speedScale = EditorGUIUtility.IconContent("SpeedScale", "Changes animation preview speed");
      public GUIContent pivot = EditorGUIUtility.IconContent("AvatarPivot", "Displays avatar's pivot and mass center");
      public GUIContent ik = new GUIContent("IK", "Activates feet IK preview");
      public GUIContent avatarIcon = EditorGUIUtility.IconContent("Avatar Icon", "Changes the model to use for previewing.");
      public GUIStyle preButton = (GUIStyle) "preButton";
      public GUIStyle preSlider = (GUIStyle) "preSlider";
      public GUIStyle preSliderThumb = (GUIStyle) "preSliderThumb";
      public GUIStyle preLabel = (GUIStyle) "preLabel";
    }

    private enum PreviewPopupOptions
    {
      Auto,
      DefaultModel,
      Other,
    }

    protected enum ViewTool
    {
      None,
      Pan,
      Zoom,
      Orbit,
    }

    public delegate void OnAvatarChange();
  }
}
