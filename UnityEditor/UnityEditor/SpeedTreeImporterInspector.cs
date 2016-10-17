// Decompiled with JetBrains decompiler
// Type: UnityEditor.SpeedTreeImporterInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.AnimatedValues;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (SpeedTreeImporter))]
  internal class SpeedTreeImporterInspector : AssetImporterInspector
  {
    private int m_SelectedLODSlider = -1;
    private readonly AnimBool m_ShowSmoothLODOptions = new AnimBool();
    private readonly AnimBool m_ShowCrossFadeWidthOptions = new AnimBool();
    private readonly int m_LODSliderId = "LODSliderIDHash".GetHashCode();
    private const float kFeetToMetersRatio = 0.3048f;
    private SerializedProperty m_LODSettings;
    private SerializedProperty m_EnableSmoothLOD;
    private SerializedProperty m_AnimateCrossFading;
    private SerializedProperty m_BillboardTransitionCrossFadeWidth;
    private SerializedProperty m_FadeOutWidth;
    private SerializedProperty m_MainColor;
    private SerializedProperty m_SpecColor;
    private SerializedProperty m_HueVariation;
    private SerializedProperty m_Shininess;
    private SerializedProperty m_AlphaTestRef;
    private SerializedProperty m_ScaleFactor;
    private int m_SelectedLODRange;

    private SpeedTreeImporter[] importers
    {
      get
      {
        return this.targets.Cast<SpeedTreeImporter>().ToArray<SpeedTreeImporter>();
      }
    }

    private bool upgradeMaterials
    {
      get
      {
        return ((IEnumerable<SpeedTreeImporter>) this.importers).Any<SpeedTreeImporter>((Func<SpeedTreeImporter, bool>) (i => i.materialsShouldBeRegenerated));
      }
    }

    private void OnEnable()
    {
      this.m_LODSettings = this.serializedObject.FindProperty("m_LODSettings");
      this.m_EnableSmoothLOD = this.serializedObject.FindProperty("m_EnableSmoothLODTransition");
      this.m_AnimateCrossFading = this.serializedObject.FindProperty("m_AnimateCrossFading");
      this.m_BillboardTransitionCrossFadeWidth = this.serializedObject.FindProperty("m_BillboardTransitionCrossFadeWidth");
      this.m_FadeOutWidth = this.serializedObject.FindProperty("m_FadeOutWidth");
      this.m_MainColor = this.serializedObject.FindProperty("m_MainColor");
      this.m_SpecColor = this.serializedObject.FindProperty("m_SpecColor");
      this.m_HueVariation = this.serializedObject.FindProperty("m_HueVariation");
      this.m_Shininess = this.serializedObject.FindProperty("m_Shininess");
      this.m_AlphaTestRef = this.serializedObject.FindProperty("m_AlphaTestRef");
      this.m_ScaleFactor = this.serializedObject.FindProperty("m_ScaleFactor");
      this.m_ShowSmoothLODOptions.value = this.m_EnableSmoothLOD.hasMultipleDifferentValues || this.m_EnableSmoothLOD.boolValue;
      this.m_ShowSmoothLODOptions.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowCrossFadeWidthOptions.value = this.m_AnimateCrossFading.hasMultipleDifferentValues || !this.m_AnimateCrossFading.boolValue;
      this.m_ShowCrossFadeWidthOptions.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
    }

    public override void OnDisable()
    {
      base.OnDisable();
      this.m_ShowSmoothLODOptions.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowCrossFadeWidthOptions.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
    }

    protected override bool ApplyRevertGUIButtons()
    {
      EditorGUI.BeginDisabledGroup(!this.HasModified());
      this.RevertButton();
      bool flag1 = this.ApplyButton("Apply Prefab");
      EditorGUI.EndDisabledGroup();
      bool upgradeMaterials = this.upgradeMaterials;
      if (GUILayout.Button(this.HasModified() || upgradeMaterials ? SpeedTreeImporterInspector.Styles.ApplyAndGenerate : SpeedTreeImporterInspector.Styles.Regenerate))
      {
        bool flag2 = this.HasModified();
        if (flag2)
          this.Apply();
        if (upgradeMaterials)
        {
          foreach (SpeedTreeImporter importer in this.importers)
            importer.SetMaterialVersionToCurrent();
        }
        this.GenerateMaterials();
        if (flag2 || upgradeMaterials)
        {
          this.ApplyAndImport();
          flag1 = true;
        }
      }
      return flag1;
    }

    private void GenerateMaterials()
    {
      string[] array1 = ((IEnumerable<SpeedTreeImporter>) this.importers).Select<SpeedTreeImporter, string>((Func<SpeedTreeImporter, string>) (im => im.materialFolderPath)).ToArray<string>();
      string[] array2 = ((IEnumerable<string>) AssetDatabase.FindAssets("t:Material", array1)).Select<string, string>((Func<string, string>) (guid => AssetDatabase.GUIDToAssetPath(guid))).ToArray<string>();
      bool flag = true;
      if (array2.Length > 0)
        flag = Provider.PromptAndCheckoutIfNeeded(array2, string.Format("Materials will be checked out in:\n{0}", (object) string.Join("\n", array1)));
      if (!flag)
        return;
      foreach (SpeedTreeImporter importer in this.importers)
        importer.GenerateMaterials();
    }

    internal List<LODGroupGUI.LODInfo> GetLODInfoArray(Rect area)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SpeedTreeImporterInspector.\u003CGetLODInfoArray\u003Ec__AnonStorey7F arrayCAnonStorey7F = new SpeedTreeImporterInspector.\u003CGetLODInfoArray\u003Ec__AnonStorey7F();
      // ISSUE: reference to a compiler-generated field
      arrayCAnonStorey7F.\u003C\u003Ef__this = this;
      // ISSUE: reference to a compiler-generated field
      arrayCAnonStorey7F.lodCount = this.m_LODSettings.arraySize;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      return LODGroupGUI.CreateLODInfos(arrayCAnonStorey7F.lodCount, area, new Func<int, string>(arrayCAnonStorey7F.\u003C\u003Em__12E), new Func<int, float>(arrayCAnonStorey7F.\u003C\u003Em__12F));
    }

    public bool HasSameLODConfig()
    {
      if (this.serializedObject.FindProperty("m_HasBillboard").hasMultipleDifferentValues || this.m_LODSettings.FindPropertyRelative("Array.size").hasMultipleDifferentValues)
        return false;
      for (int index = 0; index < this.m_LODSettings.arraySize; ++index)
      {
        if (this.m_LODSettings.GetArrayElementAtIndex(index).FindPropertyRelative("height").hasMultipleDifferentValues)
          return false;
      }
      return true;
    }

    public bool CanUnifyLODConfig()
    {
      if (!this.serializedObject.FindProperty("m_HasBillboard").hasMultipleDifferentValues)
        return !this.m_LODSettings.FindPropertyRelative("Array.size").hasMultipleDifferentValues;
      return false;
    }

    public override void OnInspectorGUI()
    {
      this.ShowMeshGUI();
      this.ShowMaterialGUI();
      this.ShowLODGUI();
      EditorGUILayout.Space();
      if (this.upgradeMaterials)
        EditorGUILayout.HelpBox(string.Format("SpeedTree materials need to be upgraded. Please back them up (if modified manually) then hit the \"{0}\" button below.", (object) SpeedTreeImporterInspector.Styles.ApplyAndGenerate.text), MessageType.Warning);
      this.ApplyRevertGUI();
    }

    private void ShowMeshGUI()
    {
      GUILayout.Label(SpeedTreeImporterInspector.Styles.MeshesHeader, EditorStyles.boldLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_ScaleFactor, SpeedTreeImporterInspector.Styles.ScaleFactor, new GUILayoutOption[0]);
      if (this.m_ScaleFactor.hasMultipleDifferentValues || !Mathf.Approximately(this.m_ScaleFactor.floatValue, 0.3048f))
        return;
      EditorGUILayout.HelpBox(SpeedTreeImporterInspector.Styles.ScaleFactorHelp.text, MessageType.Info);
    }

    private void ShowMaterialGUI()
    {
      GUILayout.Label(SpeedTreeImporterInspector.Styles.MaterialsHeader, EditorStyles.boldLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_MainColor, SpeedTreeImporterInspector.Styles.MainColor, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_SpecColor, SpeedTreeImporterInspector.Styles.SpecColor, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_HueVariation, SpeedTreeImporterInspector.Styles.HueVariation, new GUILayoutOption[0]);
      EditorGUILayout.Slider(this.m_Shininess, 0.01f, 1f, SpeedTreeImporterInspector.Styles.Shininess, new GUILayoutOption[0]);
      EditorGUILayout.Slider(this.m_AlphaTestRef, 0.0f, 1f, SpeedTreeImporterInspector.Styles.AlphaTestRef, new GUILayoutOption[0]);
    }

    private void ShowLODGUI()
    {
      this.m_ShowSmoothLODOptions.target = this.m_EnableSmoothLOD.hasMultipleDifferentValues || this.m_EnableSmoothLOD.boolValue;
      this.m_ShowCrossFadeWidthOptions.target = this.m_AnimateCrossFading.hasMultipleDifferentValues || !this.m_AnimateCrossFading.boolValue;
      GUILayout.Label(SpeedTreeImporterInspector.Styles.LODHeader, EditorStyles.boldLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_EnableSmoothLOD, SpeedTreeImporterInspector.Styles.SmoothLOD, new GUILayoutOption[0]);
      ++EditorGUI.indentLevel;
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowSmoothLODOptions.faded))
      {
        EditorGUILayout.PropertyField(this.m_AnimateCrossFading, SpeedTreeImporterInspector.Styles.AnimateCrossFading, new GUILayoutOption[0]);
        if (EditorGUILayout.BeginFadeGroup(this.m_ShowCrossFadeWidthOptions.faded))
        {
          EditorGUILayout.Slider(this.m_BillboardTransitionCrossFadeWidth, 0.0f, 1f, SpeedTreeImporterInspector.Styles.CrossFadeWidth, new GUILayoutOption[0]);
          EditorGUILayout.Slider(this.m_FadeOutWidth, 0.0f, 1f, SpeedTreeImporterInspector.Styles.FadeOutWidth, new GUILayoutOption[0]);
        }
        EditorGUILayout.EndFadeGroup();
      }
      EditorGUILayout.EndFadeGroup();
      --EditorGUI.indentLevel;
      EditorGUILayout.Space();
      if (this.HasSameLODConfig())
      {
        EditorGUILayout.Space();
        Rect rect = GUILayoutUtility.GetRect(0.0f, 30f, new GUILayoutOption[1]{ GUILayout.ExpandWidth(true) });
        List<LODGroupGUI.LODInfo> lodInfoArray = this.GetLODInfoArray(rect);
        this.DrawLODLevelSlider(rect, lodInfoArray);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        if (this.m_SelectedLODRange != -1 && lodInfoArray.Count > 0)
        {
          EditorGUILayout.LabelField(lodInfoArray[this.m_SelectedLODRange].LODName + " Options", EditorStyles.boldLabel, new GUILayoutOption[0]);
          bool flag = this.m_SelectedLODRange == lodInfoArray.Count - 1 && this.importers[0].hasBillboard;
          EditorGUILayout.PropertyField(this.m_LODSettings.GetArrayElementAtIndex(this.m_SelectedLODRange).FindPropertyRelative("castShadows"), SpeedTreeImporterInspector.Styles.CastShadows, new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_LODSettings.GetArrayElementAtIndex(this.m_SelectedLODRange).FindPropertyRelative("receiveShadows"), SpeedTreeImporterInspector.Styles.ReceiveShadows, new GUILayoutOption[0]);
          SerializedProperty propertyRelative = this.m_LODSettings.GetArrayElementAtIndex(this.m_SelectedLODRange).FindPropertyRelative("useLightProbes");
          EditorGUILayout.PropertyField(propertyRelative, SpeedTreeImporterInspector.Styles.UseLightProbes, new GUILayoutOption[0]);
          if (!propertyRelative.hasMultipleDifferentValues && propertyRelative.boolValue && flag)
            EditorGUILayout.HelpBox("Enabling Light Probe for billboards breaks batched rendering and may cause performance problem.", MessageType.Warning);
          EditorGUILayout.PropertyField(this.m_LODSettings.GetArrayElementAtIndex(this.m_SelectedLODRange).FindPropertyRelative("enableBump"), SpeedTreeImporterInspector.Styles.EnableBump, new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_LODSettings.GetArrayElementAtIndex(this.m_SelectedLODRange).FindPropertyRelative("enableHue"), SpeedTreeImporterInspector.Styles.EnableHue, new GUILayoutOption[0]);
          int num = ((IEnumerable<SpeedTreeImporter>) this.importers).Min<SpeedTreeImporter>((Func<SpeedTreeImporter, int>) (im => im.bestWindQuality));
          if (num > 0)
          {
            if (flag)
              num = num < 1 ? 0 : 1;
            EditorGUILayout.Popup(this.m_LODSettings.GetArrayElementAtIndex(this.m_SelectedLODRange).FindPropertyRelative("windQuality"), ((IEnumerable<string>) SpeedTreeImporter.windQualityNames).Take<string>(num + 1).Select<string, GUIContent>((Func<string, GUIContent>) (s => new GUIContent(s))).ToArray<GUIContent>(), SpeedTreeImporterInspector.Styles.WindQuality, new GUILayoutOption[0]);
          }
        }
      }
      else
      {
        if (this.CanUnifyLODConfig())
        {
          EditorGUILayout.BeginHorizontal();
          GUILayout.FlexibleSpace();
          Rect rect = GUILayoutUtility.GetRect(SpeedTreeImporterInspector.Styles.ResetLOD, EditorStyles.miniButton);
          if (GUI.Button(rect, SpeedTreeImporterInspector.Styles.ResetLOD, EditorStyles.miniButton))
          {
            GenericMenu genericMenu = new GenericMenu();
            foreach (SpeedTreeImporter speedTreeImporter in this.targets.Cast<SpeedTreeImporter>())
            {
              string text = string.Format("{0}: {1}", (object) Path.GetFileNameWithoutExtension(speedTreeImporter.assetPath), (object) string.Join(" | ", ((IEnumerable<float>) speedTreeImporter.LODHeights).Select<float, string>((Func<float, string>) (height => string.Format("{0:0}%", (object) (height * 100f)))).ToArray<string>()));
              genericMenu.AddItem(new GUIContent(text), false, new GenericMenu.MenuFunction2(this.OnResetLODMenuClick), (object) speedTreeImporter);
            }
            genericMenu.DropDown(rect);
          }
          EditorGUILayout.EndHorizontal();
        }
        Rect rect1 = GUILayoutUtility.GetRect(0.0f, 30f, new GUILayoutOption[1]{ GUILayout.ExpandWidth(true) });
        if (Event.current.type == EventType.Repaint)
          LODGroupGUI.DrawMixedValueLODSlider(rect1);
      }
      EditorGUILayout.Space();
    }

    private void DrawLODLevelSlider(Rect sliderPosition, List<LODGroupGUI.LODInfo> lods)
    {
      int controlId = GUIUtility.GetControlID(this.m_LODSliderId, FocusType.Passive);
      Event current1 = Event.current;
      switch (current1.GetTypeForControl(controlId))
      {
        case EventType.MouseDown:
          Rect rect = sliderPosition;
          rect.x -= 5f;
          rect.width += 10f;
          if (!rect.Contains(current1.mousePosition))
            break;
          current1.Use();
          GUIUtility.hotControl = controlId;
          IOrderedEnumerable<LODGroupGUI.LODInfo> orderedEnumerable1 = lods.Where<LODGroupGUI.LODInfo>((Func<LODGroupGUI.LODInfo, bool>) (lod => (double) lod.ScreenPercent > 0.5)).OrderByDescending<LODGroupGUI.LODInfo, int>((Func<LODGroupGUI.LODInfo, int>) (x => x.LODLevel));
          IOrderedEnumerable<LODGroupGUI.LODInfo> orderedEnumerable2 = lods.Where<LODGroupGUI.LODInfo>((Func<LODGroupGUI.LODInfo, bool>) (lod => (double) lod.ScreenPercent <= 0.5)).OrderBy<LODGroupGUI.LODInfo, int>((Func<LODGroupGUI.LODInfo, int>) (x => x.LODLevel));
          List<LODGroupGUI.LODInfo> lodInfoList = new List<LODGroupGUI.LODInfo>();
          lodInfoList.AddRange((IEnumerable<LODGroupGUI.LODInfo>) orderedEnumerable1);
          lodInfoList.AddRange((IEnumerable<LODGroupGUI.LODInfo>) orderedEnumerable2);
          using (List<LODGroupGUI.LODInfo>.Enumerator enumerator = lodInfoList.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              LODGroupGUI.LODInfo current2 = enumerator.Current;
              if (current2.m_ButtonPosition.Contains(current1.mousePosition))
              {
                this.m_SelectedLODSlider = current2.LODLevel;
                this.m_SelectedLODRange = current2.LODLevel;
                break;
              }
              if (current2.m_RangePosition.Contains(current1.mousePosition))
              {
                this.m_SelectedLODSlider = -1;
                this.m_SelectedLODRange = current2.LODLevel;
                break;
              }
            }
            break;
          }
        case EventType.MouseUp:
          if (GUIUtility.hotControl != controlId)
            break;
          GUIUtility.hotControl = 0;
          current1.Use();
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl != controlId || this.m_SelectedLODSlider < 0 || lods[this.m_SelectedLODSlider] == null)
            break;
          current1.Use();
          LODGroupGUI.SetSelectedLODLevelPercentage(Mathf.Clamp01((float) (1.0 - ((double) current1.mousePosition.x - (double) sliderPosition.x) / (double) sliderPosition.width)) - 1f / 1000f, this.m_SelectedLODSlider, lods);
          this.m_LODSettings.GetArrayElementAtIndex(this.m_SelectedLODSlider).FindPropertyRelative("height").floatValue = lods[this.m_SelectedLODSlider].RawScreenPercent;
          break;
        case EventType.Repaint:
          LODGroupGUI.DrawLODSlider(sliderPosition, (IList<LODGroupGUI.LODInfo>) lods, this.m_SelectedLODRange);
          break;
      }
    }

    private void OnResetLODMenuClick(object userData)
    {
      float[] lodHeights = (userData as SpeedTreeImporter).LODHeights;
      for (int index = 0; index < lodHeights.Length; ++index)
        this.m_LODSettings.GetArrayElementAtIndex(index).FindPropertyRelative("height").floatValue = lodHeights[index];
    }

    private class Styles
    {
      public static GUIContent LODHeader = EditorGUIUtility.TextContent("LODs");
      public static GUIContent ResetLOD = EditorGUIUtility.TextContent("Reset LOD to...|Unify the LOD settings for all selected assets.");
      public static GUIContent SmoothLOD = EditorGUIUtility.TextContent("Smooth LOD|Toggles smooth LOD transitions.");
      public static GUIContent AnimateCrossFading = EditorGUIUtility.TextContent("Animate Cross-fading|Cross-fading is animated instead of being calculated by distance.");
      public static GUIContent CrossFadeWidth = EditorGUIUtility.TextContent("Crossfade Width|Proportion of the last 3D mesh LOD region width which is used for cross-fading to billboard tree.");
      public static GUIContent FadeOutWidth = EditorGUIUtility.TextContent("Fade Out Width|Proportion of the billboard LOD region width which is used for fading out the billboard.");
      public static GUIContent MeshesHeader = EditorGUIUtility.TextContent("Meshes");
      public static GUIContent ScaleFactor = EditorGUIUtility.TextContent("Scale Factor|How much to scale the tree model compared to what is in the .spm file.");
      public static GUIContent ScaleFactorHelp = EditorGUIUtility.TextContent("The default value of Scale Factor is 0.3048, the conversion ratio from feet to meters, as these are the most conventional measurements used in SpeedTree and Unity, respectively.");
      public static GUIContent MaterialsHeader = EditorGUIUtility.TextContent("Materials");
      public static GUIContent MainColor = EditorGUIUtility.TextContent("Main Color|The color modulating the diffuse lighting component.");
      public static GUIContent SpecColor = EditorGUIUtility.TextContent("Specular Color|The color modulating the specular lighting component.");
      public static GUIContent HueVariation = EditorGUIUtility.TextContent("Hue Color|Apply to LODs that have Hue Variation effect enabled.");
      public static GUIContent Shininess = EditorGUIUtility.TextContent("Shininess|The shininess value.");
      public static GUIContent AlphaTestRef = EditorGUIUtility.TextContent("Alpha Cutoff|The alpha-test reference value.");
      public static GUIContent CastShadows = EditorGUIUtility.TextContent("Cast Shadows|The tree casts shadow.");
      public static GUIContent ReceiveShadows = EditorGUIUtility.TextContent("Receive Shadows|The tree receives shadow.");
      public static GUIContent UseLightProbes = EditorGUIUtility.TextContent("Use Light Probes|The tree uses light probe for lighting.");
      public static GUIContent UseReflectionProbes = EditorGUIUtility.TextContent("Use Reflection Probes|The tree uses reflection probe for rendering.");
      public static GUIContent EnableBump = EditorGUIUtility.TextContent("Normal Map|Enable normal mapping (aka Bump mapping).");
      public static GUIContent EnableHue = EditorGUIUtility.TextContent("Enable Hue Variation|Enable Hue variation color (color is adjusted between Main Color and Hue Color).");
      public static GUIContent WindQuality = EditorGUIUtility.TextContent("Wind Quality|Controls the wind quality.");
      public static GUIContent ApplyAndGenerate = EditorGUIUtility.TextContent("Apply & Generate Materials|Apply current importer settings and generate materials with new settings.");
      public static GUIContent Regenerate = EditorGUIUtility.TextContent("Regenerate Materials|Regenerate materials from the current importer settings.");
    }
  }
}
