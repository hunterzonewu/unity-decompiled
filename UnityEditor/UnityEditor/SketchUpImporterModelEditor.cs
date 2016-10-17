// Decompiled with JetBrains decompiler
// Type: UnityEditor.SketchUpImporterModelEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class SketchUpImporterModelEditor : ModelImporterModelEditor
  {
    private const float kInchToMeter = 0.0254f;
    private SerializedProperty m_GenerateBackFace;
    private SerializedProperty m_MergeCoplanarFaces;
    private SerializedProperty m_FileUnit;
    private SerializedProperty m_GlobalScale;
    private SerializedProperty m_Latitude;
    private SerializedProperty m_Longitude;
    private SerializedProperty m_NorthCorrection;
    private SerializedProperty m_SelectedNodes;
    private SketchUpImporter m_Target;
    private float lengthToUnit;

    internal override void OnEnable()
    {
      this.m_GenerateBackFace = this.serializedObject.FindProperty("m_GenerateBackFace");
      this.m_MergeCoplanarFaces = this.serializedObject.FindProperty("m_MergeCoplanarFaces");
      this.m_FileUnit = this.serializedObject.FindProperty("m_FileUnit");
      this.m_GlobalScale = this.serializedObject.FindProperty("m_GlobalScale");
      this.m_Latitude = this.serializedObject.FindProperty("m_Latitude");
      this.m_Longitude = this.serializedObject.FindProperty("m_Longitude");
      this.m_NorthCorrection = this.serializedObject.FindProperty("m_NorthCorrection");
      this.m_SelectedNodes = this.serializedObject.FindProperty("m_SelectedNodes");
      this.m_Target = this.target as SketchUpImporter;
      base.OnEnable();
    }

    private static float CovertUnitToGlobalScale(SketchUpImporterModelEditor.EFileUnit measurement, float unit)
    {
      switch (measurement)
      {
        case SketchUpImporterModelEditor.EFileUnit.Meters:
          return 0.0254f * unit;
        case SketchUpImporterModelEditor.EFileUnit.Centimeters:
          return 0.000254f * unit;
        case SketchUpImporterModelEditor.EFileUnit.Millimeters:
          return unit * 2.54E-05f;
        case SketchUpImporterModelEditor.EFileUnit.Feet:
          return unit * 0.00774192f;
        case SketchUpImporterModelEditor.EFileUnit.Inches:
          return unit;
        default:
          Debug.LogError((object) "File Unit value is invalid");
          return 1f;
      }
    }

    private static float ConvertGlobalScaleToUnit(SketchUpImporterModelEditor.EFileUnit measurement, float globalScale)
    {
      switch (measurement)
      {
        case SketchUpImporterModelEditor.EFileUnit.Meters:
          return globalScale / 0.0254f;
        case SketchUpImporterModelEditor.EFileUnit.Centimeters:
          return globalScale / 0.000254f;
        case SketchUpImporterModelEditor.EFileUnit.Millimeters:
          return globalScale / 2.54E-05f;
        case SketchUpImporterModelEditor.EFileUnit.Feet:
          return globalScale / 0.00774192f;
        case SketchUpImporterModelEditor.EFileUnit.Inches:
          return globalScale;
        default:
          Debug.LogError((object) "File Unit value is invalid");
          return 1f;
      }
    }

    public override void OnInspectorGUI()
    {
      GUILayout.Label(SketchUpImporterModelEditor.Styles.sketchUpLabel, EditorStyles.boldLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_GenerateBackFace, SketchUpImporterModelEditor.Styles.generateBackFaceLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_MergeCoplanarFaces, SketchUpImporterModelEditor.Styles.mergeCoplanarFaces, new GUILayoutOption[0]);
      EditorGUILayout.BeginHorizontal();
      GUILayout.Label(SketchUpImporterModelEditor.Styles.fileUnitLabel, new GUILayoutOption[1]
      {
        GUILayout.MinWidth(EditorGUIUtility.labelWidth)
      });
      GUILayout.Label("1");
      EditorGUILayout.Popup(this.m_FileUnit, SketchUpImporterModelEditor.Styles.measurementOptions, GUIContent.Temp(string.Empty), new GUILayoutOption[1]
      {
        GUILayout.MaxWidth(100f)
      });
      this.lengthToUnit = SketchUpImporterModelEditor.ConvertGlobalScaleToUnit((SketchUpImporterModelEditor.EFileUnit) this.m_FileUnit.intValue, this.m_GlobalScale.floatValue);
      GUILayout.Label("=");
      this.lengthToUnit = EditorGUILayout.FloatField(this.lengthToUnit);
      this.m_GlobalScale.floatValue = SketchUpImporterModelEditor.CovertUnitToGlobalScale((SketchUpImporterModelEditor.EFileUnit) this.m_FileUnit.intValue, this.lengthToUnit);
      EditorGUILayout.EndHorizontal();
      EditorGUI.BeginDisabledGroup(true);
      double num1 = (double) EditorGUILayout.FloatField(SketchUpImporterModelEditor.Styles.longitudeLabel, this.m_Longitude.floatValue, new GUILayoutOption[0]);
      double num2 = (double) EditorGUILayout.FloatField(SketchUpImporterModelEditor.Styles.latitudeLabel, this.m_Latitude.floatValue, new GUILayoutOption[0]);
      double num3 = (double) EditorGUILayout.FloatField(SketchUpImporterModelEditor.Styles.northCorrectionLabel, this.m_NorthCorrection.floatValue, new GUILayoutOption[0]);
      EditorGUI.EndDisabledGroup();
      EditorGUILayout.BeginHorizontal();
      if (GUILayout.Button(SketchUpImporterModelEditor.Styles.selectNodeButton))
      {
        SketchUpImportDlg.Launch(this.m_Target.GetNodes(), this);
        GUIUtility.ExitGUI();
      }
      GUILayout.FlexibleSpace();
      EditorGUILayout.EndHorizontal();
      base.OnInspectorGUI();
    }

    public void SetSelectedNodes(int[] selectedNodes)
    {
      if (selectedNodes == null)
        return;
      this.m_SelectedNodes.ClearArray();
      for (int index = 0; index < selectedNodes.Length; ++index)
      {
        this.m_SelectedNodes.InsertArrayElementAtIndex(index);
        this.m_SelectedNodes.GetArrayElementAtIndex(index).intValue = selectedNodes[index];
      }
    }

    private enum EFileUnit
    {
      Meters,
      Centimeters,
      Millimeters,
      Feet,
      Inches,
    }

    private class Styles
    {
      public static readonly GUIContent sketchUpLabel = EditorGUIUtility.TextContent("SketchUp|SketchUp import settings");
      public static readonly GUIContent generateBackFaceLabel = EditorGUIUtility.TextContent("Generate Back Face|Enable/disable generation of back facing polygons");
      public static readonly GUIContent mergeCoplanarFaces = EditorGUIUtility.TextContent("Merge Coplanar Faces|Enable/disable merging of coplanar faces when generating meshes");
      public static readonly GUIContent selectNodeButton = EditorGUIUtility.TextContent("Select Nodes...|Brings up the node selection dialog box");
      public static readonly GUIContent fileUnitLabel = EditorGUIUtility.TextContent("Unit conversion|Length measurement to unit conversion. The value in Scale Factor is calculated based on the value here");
      public static readonly GUIContent longitudeLabel = EditorGUIUtility.TextContent("Longitude|Longitude Geo-location");
      public static readonly GUIContent latitudeLabel = EditorGUIUtility.TextContent("Latitude|Latitude Geo-location");
      public static readonly GUIContent northCorrectionLabel = EditorGUIUtility.TextContent("North Correction|The angle which will rotate the north direction to the z-axis for a the model");
      public static readonly GUIContent[] measurementOptions = new GUIContent[5]
      {
        EditorGUIUtility.TextContent("Meters"),
        EditorGUIUtility.TextContent("Centimeters"),
        EditorGUIUtility.TextContent("Millimeters"),
        EditorGUIUtility.TextContent("Feet"),
        EditorGUIUtility.TextContent("Inches")
      };
    }
  }
}
