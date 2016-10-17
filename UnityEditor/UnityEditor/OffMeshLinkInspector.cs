// Decompiled with JetBrains decompiler
// Type: UnityEditor.OffMeshLinkInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (OffMeshLink))]
  internal class OffMeshLinkInspector : Editor
  {
    private SerializedProperty m_AreaIndex;
    private SerializedProperty m_Start;
    private SerializedProperty m_End;
    private SerializedProperty m_CostOverride;
    private SerializedProperty m_BiDirectional;
    private SerializedProperty m_Activated;
    private SerializedProperty m_AutoUpdatePositions;

    private void OnEnable()
    {
      this.m_AreaIndex = this.serializedObject.FindProperty("m_AreaIndex");
      this.m_Start = this.serializedObject.FindProperty("m_Start");
      this.m_End = this.serializedObject.FindProperty("m_End");
      this.m_CostOverride = this.serializedObject.FindProperty("m_CostOverride");
      this.m_BiDirectional = this.serializedObject.FindProperty("m_BiDirectional");
      this.m_Activated = this.serializedObject.FindProperty("m_Activated");
      this.m_AutoUpdatePositions = this.serializedObject.FindProperty("m_AutoUpdatePositions");
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_Start);
      EditorGUILayout.PropertyField(this.m_End);
      EditorGUILayout.PropertyField(this.m_CostOverride);
      EditorGUILayout.PropertyField(this.m_BiDirectional);
      EditorGUILayout.PropertyField(this.m_Activated);
      EditorGUILayout.PropertyField(this.m_AutoUpdatePositions);
      this.SelectNavMeshArea();
      this.serializedObject.ApplyModifiedProperties();
    }

    private void SelectNavMeshArea()
    {
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = this.m_AreaIndex.hasMultipleDifferentValues;
      string[] navMeshAreaNames = GameObjectUtility.GetNavMeshAreaNames();
      int intValue = this.m_AreaIndex.intValue;
      int selectedIndex = -1;
      for (int index = 0; index < navMeshAreaNames.Length; ++index)
      {
        if (GameObjectUtility.GetNavMeshAreaFromName(navMeshAreaNames[index]) == intValue)
        {
          selectedIndex = index;
          break;
        }
      }
      int index1 = EditorGUILayout.Popup("Navigation Area", selectedIndex, navMeshAreaNames, new GUILayoutOption[0]);
      EditorGUI.showMixedValue = false;
      if (!EditorGUI.EndChangeCheck())
        return;
      this.m_AreaIndex.intValue = GameObjectUtility.GetNavMeshAreaFromName(navMeshAreaNames[index1]);
    }
  }
}
