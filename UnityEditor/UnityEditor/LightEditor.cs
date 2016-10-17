// Decompiled with JetBrains decompiler
// Type: UnityEditor.LightEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (Light))]
  internal class LightEditor : Editor
  {
    internal static Color kGizmoLight = new Color(0.9960784f, 0.9921569f, 0.5333334f, 0.5019608f);
    internal static Color kGizmoDisabledLight = new Color(0.5294118f, 0.454902f, 0.1960784f, 0.5019608f);
    private AnimBool m_ShowSpotOptions = new AnimBool();
    private AnimBool m_ShowPointOptions = new AnimBool();
    private AnimBool m_ShowDirOptions = new AnimBool();
    private AnimBool m_ShowAreaOptions = new AnimBool();
    private AnimBool m_ShowRuntimeOptions = new AnimBool();
    private AnimBool m_ShowShadowOptions = new AnimBool();
    private AnimBool m_ShowIndirectWarning = new AnimBool();
    private AnimBool m_ShowBakingWarning = new AnimBool();
    private AnimBool m_BakedShadowAngleOptions = new AnimBool();
    private AnimBool m_BakedShadowRadiusOptions = new AnimBool();
    private SerializedProperty m_Type;
    private SerializedProperty m_Range;
    private SerializedProperty m_SpotAngle;
    private SerializedProperty m_CookieSize;
    private SerializedProperty m_Color;
    private SerializedProperty m_Intensity;
    private SerializedProperty m_BounceIntensity;
    private SerializedProperty m_Cookie;
    private SerializedProperty m_ShadowsType;
    private SerializedProperty m_ShadowsStrength;
    private SerializedProperty m_ShadowsResolution;
    private SerializedProperty m_ShadowsBias;
    private SerializedProperty m_ShadowsNormalBias;
    private SerializedProperty m_ShadowsNearPlane;
    private SerializedProperty m_Halo;
    private SerializedProperty m_Flare;
    private SerializedProperty m_RenderMode;
    private SerializedProperty m_CullingMask;
    private SerializedProperty m_Lightmapping;
    private SerializedProperty m_AreaSizeX;
    private SerializedProperty m_AreaSizeY;
    private SerializedProperty m_BakedShadowRadius;
    private SerializedProperty m_BakedShadowAngle;
    private static LightEditor.Styles s_Styles;

    private bool typeIsSame
    {
      get
      {
        return !this.m_Type.hasMultipleDifferentValues;
      }
    }

    private bool shadowTypeIsSame
    {
      get
      {
        return !this.m_ShadowsType.hasMultipleDifferentValues;
      }
    }

    private Light light
    {
      get
      {
        return this.target as Light;
      }
    }

    private bool isBakedOrMixed
    {
      get
      {
        return this.m_Lightmapping.intValue != 4;
      }
    }

    private bool spotOptionsValue
    {
      get
      {
        if (this.typeIsSame)
          return this.light.type == LightType.Spot;
        return false;
      }
    }

    private bool pointOptionsValue
    {
      get
      {
        if (this.typeIsSame)
          return this.light.type == LightType.Point;
        return false;
      }
    }

    private bool dirOptionsValue
    {
      get
      {
        if (this.typeIsSame)
          return this.light.type == LightType.Directional;
        return false;
      }
    }

    private bool areaOptionsValue
    {
      get
      {
        if (this.typeIsSame)
          return this.light.type == LightType.Area;
        return false;
      }
    }

    private bool runtimeOptionsValue
    {
      get
      {
        if (this.typeIsSame && this.light.type != LightType.Area)
          return this.m_Lightmapping.intValue != 2;
        return false;
      }
    }

    private bool bakedShadowRadius
    {
      get
      {
        if (this.typeIsSame && (this.light.type == LightType.Point || this.light.type == LightType.Spot))
          return this.isBakedOrMixed;
        return false;
      }
    }

    private bool bakedShadowAngle
    {
      get
      {
        if (this.typeIsSame && this.light.type == LightType.Directional)
          return this.isBakedOrMixed;
        return false;
      }
    }

    private bool shadowOptionsValue
    {
      get
      {
        if (this.shadowTypeIsSame)
          return this.light.shadows != LightShadows.None;
        return false;
      }
    }

    private bool areaWarningValue
    {
      get
      {
        if (this.typeIsSame)
          return this.light.type == LightType.Area;
        return false;
      }
    }

    private bool bounceWarningValue
    {
      get
      {
        if (this.typeIsSame && (this.light.type == LightType.Point || this.light.type == LightType.Spot) && this.m_Lightmapping.intValue == 4)
          return (double) this.m_BounceIntensity.floatValue > 0.0;
        return false;
      }
    }

    private bool bakingWarningValue
    {
      get
      {
        if (!Lightmapping.bakedLightmapsEnabled)
          return this.isBakedOrMixed;
        return false;
      }
    }

    private void SetOptions(AnimBool animBool, bool initialize, bool targetValue)
    {
      if (initialize)
      {
        animBool.value = targetValue;
        animBool.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      }
      else
        animBool.target = targetValue;
    }

    private void UpdateShowOptions(bool initialize)
    {
      this.SetOptions(this.m_ShowSpotOptions, initialize, this.spotOptionsValue);
      this.SetOptions(this.m_ShowPointOptions, initialize, this.pointOptionsValue);
      this.SetOptions(this.m_ShowDirOptions, initialize, this.dirOptionsValue);
      this.SetOptions(this.m_ShowAreaOptions, initialize, this.areaOptionsValue);
      this.SetOptions(this.m_ShowShadowOptions, initialize, this.shadowOptionsValue);
      this.SetOptions(this.m_ShowIndirectWarning, initialize, this.bounceWarningValue);
      this.SetOptions(this.m_ShowBakingWarning, initialize, this.bakingWarningValue);
      this.SetOptions(this.m_ShowRuntimeOptions, initialize, this.runtimeOptionsValue);
      this.SetOptions(this.m_BakedShadowAngleOptions, initialize, this.bakedShadowAngle);
      this.SetOptions(this.m_BakedShadowRadiusOptions, initialize, this.bakedShadowRadius);
    }

    private void OnEnable()
    {
      this.m_Type = this.serializedObject.FindProperty("m_Type");
      this.m_Range = this.serializedObject.FindProperty("m_Range");
      this.m_SpotAngle = this.serializedObject.FindProperty("m_SpotAngle");
      this.m_CookieSize = this.serializedObject.FindProperty("m_CookieSize");
      this.m_Color = this.serializedObject.FindProperty("m_Color");
      this.m_Intensity = this.serializedObject.FindProperty("m_Intensity");
      this.m_BounceIntensity = this.serializedObject.FindProperty("m_BounceIntensity");
      this.m_Cookie = this.serializedObject.FindProperty("m_Cookie");
      this.m_ShadowsType = this.serializedObject.FindProperty("m_Shadows.m_Type");
      this.m_ShadowsStrength = this.serializedObject.FindProperty("m_Shadows.m_Strength");
      this.m_ShadowsResolution = this.serializedObject.FindProperty("m_Shadows.m_Resolution");
      this.m_ShadowsBias = this.serializedObject.FindProperty("m_Shadows.m_Bias");
      this.m_ShadowsNormalBias = this.serializedObject.FindProperty("m_Shadows.m_NormalBias");
      this.m_ShadowsNearPlane = this.serializedObject.FindProperty("m_Shadows.m_NearPlane");
      this.m_Halo = this.serializedObject.FindProperty("m_DrawHalo");
      this.m_Flare = this.serializedObject.FindProperty("m_Flare");
      this.m_RenderMode = this.serializedObject.FindProperty("m_RenderMode");
      this.m_CullingMask = this.serializedObject.FindProperty("m_CullingMask");
      this.m_Lightmapping = this.serializedObject.FindProperty("m_Lightmapping");
      this.m_AreaSizeX = this.serializedObject.FindProperty("m_AreaSize.x");
      this.m_AreaSizeY = this.serializedObject.FindProperty("m_AreaSize.y");
      this.m_BakedShadowRadius = this.serializedObject.FindProperty("m_ShadowRadius");
      this.m_BakedShadowAngle = this.serializedObject.FindProperty("m_ShadowAngle");
      this.UpdateShowOptions(true);
    }

    public override void OnInspectorGUI()
    {
      if (LightEditor.s_Styles == null)
        LightEditor.s_Styles = new LightEditor.Styles();
      this.serializedObject.Update();
      this.UpdateShowOptions(false);
      EditorGUILayout.PropertyField(this.m_Type);
      if (EditorGUILayout.BeginFadeGroup(1f - this.m_ShowAreaOptions.faded))
      {
        EditorGUILayout.IntPopup(this.m_Lightmapping, LightEditor.s_Styles.LightmappingModes, LightEditor.s_Styles.LightmappingModeValues, LightEditor.s_Styles.LightmappingModeLabel, new GUILayoutOption[0]);
        if (EditorGUILayout.BeginFadeGroup(this.m_ShowBakingWarning.faded))
          EditorGUILayout.HelpBox(EditorGUIUtility.TextContent("Enable Baked GI from Lighting window to use Baked or Mixed.").text, MessageType.Warning, false);
        EditorGUILayout.EndFadeGroup();
      }
      EditorGUILayout.EndFadeGroup();
      EditorGUILayout.Space();
      if (EditorGUILayout.BeginFadeGroup(!this.m_ShowDirOptions.isAnimating || !this.m_ShowAreaOptions.isAnimating || !this.m_ShowDirOptions.target && !this.m_ShowAreaOptions.target ? 1f - Mathf.Max(this.m_ShowDirOptions.faded, this.m_ShowAreaOptions.faded) : 0.0f))
        EditorGUILayout.PropertyField(this.m_Range);
      EditorGUILayout.EndFadeGroup();
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowSpotOptions.faded))
        EditorGUILayout.Slider(this.m_SpotAngle, 1f, 179f);
      EditorGUILayout.EndFadeGroup();
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowAreaOptions.faded))
      {
        EditorGUILayout.PropertyField(this.m_AreaSizeX, EditorGUIUtility.TextContent("Width"), new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(this.m_AreaSizeY, EditorGUIUtility.TextContent("Height"), new GUILayoutOption[0]);
      }
      EditorGUILayout.EndFadeGroup();
      EditorGUILayout.PropertyField(this.m_Color);
      EditorGUILayout.Slider(this.m_Intensity, 0.0f, 8f);
      EditorGUILayout.Slider(this.m_BounceIntensity, 0.0f, 8f, LightEditor.s_Styles.LightBounceIntensity, new GUILayoutOption[0]);
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowIndirectWarning.faded))
        EditorGUILayout.HelpBox(EditorGUIUtility.TextContent("Currently realtime indirect bounce light shadowing for spot and point lights is not supported.").text, MessageType.Warning, false);
      EditorGUILayout.EndFadeGroup();
      this.ShadowsGUI();
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowRuntimeOptions.faded))
        EditorGUILayout.PropertyField(this.m_Cookie);
      EditorGUILayout.EndFadeGroup();
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowRuntimeOptions.faded * this.m_ShowDirOptions.faded))
        EditorGUILayout.PropertyField(this.m_CookieSize);
      EditorGUILayout.EndFadeGroup();
      EditorGUILayout.PropertyField(this.m_Halo);
      EditorGUILayout.PropertyField(this.m_Flare);
      EditorGUILayout.PropertyField(this.m_RenderMode);
      EditorGUILayout.PropertyField(this.m_CullingMask);
      EditorGUILayout.Space();
      if ((Object) SceneView.currentDrawingSceneView != (Object) null && !SceneView.currentDrawingSceneView.m_SceneLighting)
        EditorGUILayout.HelpBox(EditorGUIUtility.TextContent("One of your scene views has lighting disabled, please keep this in mind when editing lighting.").text, MessageType.Warning, false);
      this.serializedObject.ApplyModifiedProperties();
    }

    private void ShadowsGUI()
    {
      float num1 = 1f - this.m_ShowAreaOptions.faded;
      if (EditorGUILayout.BeginFadeGroup(num1))
      {
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(this.m_ShadowsType, LightEditor.s_Styles.ShadowType, new GUILayoutOption[0]);
      }
      EditorGUILayout.EndFadeGroup();
      ++EditorGUI.indentLevel;
      float num2 = num1 * this.m_ShowShadowOptions.faded;
      if (EditorGUILayout.BeginFadeGroup(num2 * this.m_ShowRuntimeOptions.faded))
      {
        EditorGUILayout.Slider(this.m_ShadowsStrength, 0.0f, 1f);
        EditorGUILayout.PropertyField(this.m_ShadowsResolution);
        EditorGUILayout.Slider(this.m_ShadowsBias, 0.0f, 2f);
        EditorGUILayout.Slider(this.m_ShadowsNormalBias, 0.0f, 3f);
        EditorGUILayout.Slider(this.m_ShadowsNearPlane, Mathf.Min(0.01f * this.m_Range.floatValue, 0.1f), 10f, LightEditor.s_Styles.ShadowNearPlane, new GUILayoutOption[0]);
      }
      EditorGUILayout.EndFadeGroup();
      if (EditorGUILayout.BeginFadeGroup(num2 * this.m_BakedShadowRadiusOptions.faded))
      {
        EditorGUI.BeginDisabledGroup(this.m_ShadowsType.intValue != 2);
        EditorGUILayout.PropertyField(this.m_BakedShadowRadius, LightEditor.s_Styles.BakedShadowRadius, new GUILayoutOption[0]);
        EditorGUI.EndDisabledGroup();
      }
      EditorGUILayout.EndFadeGroup();
      if (EditorGUILayout.BeginFadeGroup(num2 * this.m_BakedShadowAngleOptions.faded))
      {
        EditorGUI.BeginDisabledGroup(this.m_ShadowsType.intValue != 2);
        EditorGUILayout.Slider(this.m_BakedShadowAngle, 0.0f, 90f, LightEditor.s_Styles.BakedShadowAngle, new GUILayoutOption[0]);
        EditorGUI.EndDisabledGroup();
      }
      EditorGUILayout.EndFadeGroup();
      --EditorGUI.indentLevel;
      EditorGUILayout.Space();
    }

    private void OnSceneGUI()
    {
      Light target = (Light) this.target;
      Color color1 = Handles.color;
      Handles.color = !target.enabled ? LightEditor.kGizmoDisabledLight : LightEditor.kGizmoLight;
      float range = target.range;
      switch (target.type)
      {
        case LightType.Spot:
          Color color2 = Handles.color;
          color2.a = Mathf.Clamp01(color1.a * 2f);
          Handles.color = color2;
          Vector2 angleAndRange = new Vector2(target.spotAngle, target.range);
          angleAndRange = Handles.ConeHandle(target.transform.rotation, target.transform.position, angleAndRange, 1f, 1f, true);
          if (GUI.changed)
          {
            Undo.RecordObject((Object) target, "Adjust Spot Light");
            target.spotAngle = angleAndRange.x;
            target.range = Mathf.Max(angleAndRange.y, 0.01f);
            break;
          }
          break;
        case LightType.Point:
          float num = Handles.RadiusHandle(Quaternion.identity, target.transform.position, range, true);
          if (GUI.changed)
          {
            Undo.RecordObject((Object) target, "Adjust Point Light");
            target.range = num;
            break;
          }
          break;
        case LightType.Area:
          EditorGUI.BeginChangeCheck();
          Vector2 vector2 = Handles.DoRectHandles(target.transform.rotation, target.transform.position, target.areaSize);
          if (EditorGUI.EndChangeCheck())
          {
            Undo.RecordObject((Object) target, "Adjust Area Light");
            target.areaSize = vector2;
            break;
          }
          break;
      }
      Handles.color = color1;
    }

    private class Styles
    {
      public readonly GUIContent LightBounceIntensity = EditorGUIUtility.TextContent("Bounce Intensity|Indirect light intensity multiplier.");
      public readonly GUIContent ShadowType = EditorGUIUtility.TextContent("Shadow Type|Shadow cast options");
      public readonly GUIContent BakedShadowRadius = EditorGUIUtility.TextContent("Baked Shadow Radius");
      public readonly GUIContent BakedShadowAngle = EditorGUIUtility.TextContent("Baked Shadow Angle");
      public readonly GUIContent ShadowNearPlane = EditorGUIUtility.TextContent("Shadow Near Plane|Shadow near plane, clamped to 0.1 units or 1% of light range, whichever is lower.");
      public readonly GUIContent LightmappingModeLabel = EditorGUIUtility.TextContent("Baking");
      public readonly GUIContent[] LightmappingModes = new GUIContent[3]{ EditorGUIUtility.TextContent("Realtime"), EditorGUIUtility.TextContent("Baked"), EditorGUIUtility.TextContent("Mixed") };
      public readonly int[] LightmappingModeValues = new int[3]{ 4, 2, 1 };
    }
  }
}
