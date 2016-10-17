// Decompiled with JetBrains decompiler
// Type: UnityEditor.SkinnedMeshRendererEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (SkinnedMeshRenderer))]
  [CanEditMultipleObjects]
  internal class SkinnedMeshRendererEditor : RendererEditorBase
  {
    private static int s_BoxHash = "SkinnedMeshRendererEditor".GetHashCode();
    private BoxEditor m_BoxEditor = new BoxEditor(false, SkinnedMeshRendererEditor.s_BoxHash);
    private SerializedProperty m_CastShadows;
    private SerializedProperty m_ReceiveShadows;
    private SerializedProperty m_Materials;
    private SerializedProperty m_AABB;
    private SerializedProperty m_DirtyAABB;
    private SerializedProperty m_BlendShapeWeights;
    private string[] m_ExcludedProperties;

    public override void OnEnable()
    {
      base.OnEnable();
      this.m_CastShadows = this.serializedObject.FindProperty("m_CastShadows");
      this.m_ReceiveShadows = this.serializedObject.FindProperty("m_ReceiveShadows");
      this.m_Materials = this.serializedObject.FindProperty("m_Materials");
      this.m_BlendShapeWeights = this.serializedObject.FindProperty("m_BlendShapeWeights");
      this.m_AABB = this.serializedObject.FindProperty("m_AABB");
      this.m_DirtyAABB = this.serializedObject.FindProperty("m_DirtyAABB");
      this.m_BoxEditor.OnEnable();
      this.m_BoxEditor.SetAlwaysDisplayHandles(true);
      this.InitializeProbeFields();
      List<string> stringList = new List<string>();
      stringList.AddRange((IEnumerable<string>) new string[5]
      {
        "m_CastShadows",
        "m_ReceiveShadows",
        "m_Materials",
        "m_BlendShapeWeights",
        "m_AABB"
      });
      stringList.AddRange((IEnumerable<string>) RendererEditorBase.Probes.GetFieldsStringArray());
      this.m_ExcludedProperties = stringList.ToArray();
    }

    public void OnDisable()
    {
      this.m_BoxEditor.OnDisable();
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      this.OnBlendShapeUI();
      EditorGUILayout.PropertyField(this.m_CastShadows);
      EditorGUILayout.PropertyField(this.m_ReceiveShadows);
      EditorGUILayout.PropertyField(this.m_Materials, true, new GUILayoutOption[0]);
      this.RenderProbeFields();
      Editor.DrawPropertiesExcluding(this.serializedObject, this.m_ExcludedProperties);
      EditorGUI.BeginChangeCheck();
      EditorGUILayout.PropertyField(this.m_AABB, new GUIContent("Bounds"), new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
        this.m_DirtyAABB.boolValue = false;
      this.serializedObject.ApplyModifiedProperties();
    }

    public void OnBlendShapeUI()
    {
      SkinnedMeshRenderer target = (SkinnedMeshRenderer) this.target;
      int num1 = !((Object) target.sharedMesh == (Object) null) ? target.sharedMesh.blendShapeCount : 0;
      if (num1 == 0)
        return;
      GUIContent label = new GUIContent();
      label.text = "BlendShapes";
      EditorGUILayout.PropertyField(this.m_BlendShapeWeights, label, false, new GUILayoutOption[0]);
      if (!this.m_BlendShapeWeights.isExpanded)
        return;
      ++EditorGUI.indentLevel;
      Mesh sharedMesh = target.sharedMesh;
      int num2 = this.m_BlendShapeWeights.arraySize;
      for (int index = 0; index < num1; ++index)
      {
        label.text = sharedMesh.GetBlendShapeName(index);
        if (index < num2)
        {
          EditorGUILayout.PropertyField(this.m_BlendShapeWeights.GetArrayElementAtIndex(index), label, new GUILayoutOption[0]);
        }
        else
        {
          EditorGUI.BeginChangeCheck();
          float num3 = EditorGUILayout.FloatField(label, 0.0f, new GUILayoutOption[0]);
          if (EditorGUI.EndChangeCheck())
          {
            this.m_BlendShapeWeights.arraySize = num1;
            num2 = num1;
            this.m_BlendShapeWeights.GetArrayElementAtIndex(index).floatValue = num3;
          }
        }
      }
      --EditorGUI.indentLevel;
    }

    public void OnSceneGUI()
    {
      SkinnedMeshRenderer target = (SkinnedMeshRenderer) this.target;
      if (target.updateWhenOffscreen)
      {
        Bounds bounds = target.bounds;
        this.m_BoxEditor.DrawWireframeBox(bounds.center, bounds.size);
      }
      else
      {
        Bounds localBounds = target.localBounds;
        Vector3 center = localBounds.center;
        Vector3 size = localBounds.size;
        if (!this.m_BoxEditor.OnSceneGUI(target.actualRootBone, Handles.s_BoundingBoxHandleColor, false, ref center, ref size))
          return;
        Undo.RecordObject((Object) target, "Resize Bounds");
        target.localBounds = new Bounds(center, size);
      }
    }
  }
}
