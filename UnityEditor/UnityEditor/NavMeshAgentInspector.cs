// Decompiled with JetBrains decompiler
// Type: UnityEditor.NavMeshAgentInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (NavMeshAgent))]
  internal class NavMeshAgentInspector : Editor
  {
    private SerializedProperty m_WalkableMask;
    private SerializedProperty m_Radius;
    private SerializedProperty m_Speed;
    private SerializedProperty m_Acceleration;
    private SerializedProperty m_AngularSpeed;
    private SerializedProperty m_StoppingDistance;
    private SerializedProperty m_AutoTraverseOffMeshLink;
    private SerializedProperty m_AutoBraking;
    private SerializedProperty m_AutoRepath;
    private SerializedProperty m_Height;
    private SerializedProperty m_BaseOffset;
    private SerializedProperty m_ObstacleAvoidanceType;
    private SerializedProperty m_AvoidancePriority;
    private static NavMeshAgentInspector.Styles s_Styles;

    private void OnEnable()
    {
      this.m_WalkableMask = this.serializedObject.FindProperty("m_WalkableMask");
      this.m_Radius = this.serializedObject.FindProperty("m_Radius");
      this.m_Speed = this.serializedObject.FindProperty("m_Speed");
      this.m_Acceleration = this.serializedObject.FindProperty("m_Acceleration");
      this.m_AngularSpeed = this.serializedObject.FindProperty("m_AngularSpeed");
      this.m_StoppingDistance = this.serializedObject.FindProperty("m_StoppingDistance");
      this.m_AutoTraverseOffMeshLink = this.serializedObject.FindProperty("m_AutoTraverseOffMeshLink");
      this.m_AutoBraking = this.serializedObject.FindProperty("m_AutoBraking");
      this.m_AutoRepath = this.serializedObject.FindProperty("m_AutoRepath");
      this.m_Height = this.serializedObject.FindProperty("m_Height");
      this.m_BaseOffset = this.serializedObject.FindProperty("m_BaseOffset");
      this.m_ObstacleAvoidanceType = this.serializedObject.FindProperty("m_ObstacleAvoidanceType");
      this.m_AvoidancePriority = this.serializedObject.FindProperty("avoidancePriority");
    }

    public override void OnInspectorGUI()
    {
      if (NavMeshAgentInspector.s_Styles == null)
        NavMeshAgentInspector.s_Styles = new NavMeshAgentInspector.Styles();
      this.serializedObject.Update();
      EditorGUILayout.LabelField(NavMeshAgentInspector.s_Styles.m_AgentSizeHeader, EditorStyles.boldLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_Radius);
      EditorGUILayout.PropertyField(this.m_Height);
      EditorGUILayout.PropertyField(this.m_BaseOffset);
      EditorGUILayout.Space();
      EditorGUILayout.LabelField(NavMeshAgentInspector.s_Styles.m_AgentSteeringHeader, EditorStyles.boldLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_Speed);
      EditorGUILayout.PropertyField(this.m_AngularSpeed);
      EditorGUILayout.PropertyField(this.m_Acceleration);
      EditorGUILayout.PropertyField(this.m_StoppingDistance);
      EditorGUILayout.PropertyField(this.m_AutoBraking);
      EditorGUILayout.Space();
      EditorGUILayout.LabelField(NavMeshAgentInspector.s_Styles.m_AgentAvoidanceHeader, EditorStyles.boldLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_ObstacleAvoidanceType, GUIContent.Temp("Quality"), new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_AvoidancePriority, GUIContent.Temp("Priority"), new GUILayoutOption[0]);
      EditorGUILayout.Space();
      EditorGUILayout.LabelField(NavMeshAgentInspector.s_Styles.m_AgentPathFindingHeader, EditorStyles.boldLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_AutoTraverseOffMeshLink);
      EditorGUILayout.PropertyField(this.m_AutoRepath);
      string[] navMeshAreaNames = GameObjectUtility.GetNavMeshAreaNames();
      long longValue = this.m_WalkableMask.longValue;
      int mask = 0;
      for (int index = 0; index < navMeshAreaNames.Length; ++index)
      {
        if (((long) (1 << GameObjectUtility.GetNavMeshAreaFromName(navMeshAreaNames[index])) & longValue) != 0L)
          mask |= 1 << index;
      }
      Rect rect = GUILayoutUtility.GetRect(EditorGUILayout.kLabelFloatMinW, EditorGUILayout.kLabelFloatMaxW, 16f, 16f, EditorStyles.layerMaskField);
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = this.m_WalkableMask.hasMultipleDifferentValues;
      int num1 = EditorGUI.MaskField(rect, "Area Mask", mask, navMeshAreaNames, EditorStyles.layerMaskField);
      EditorGUI.showMixedValue = false;
      if (EditorGUI.EndChangeCheck())
      {
        if (num1 == -1)
        {
          this.m_WalkableMask.longValue = (long) uint.MaxValue;
        }
        else
        {
          uint num2 = 0;
          for (int index = 0; index < navMeshAreaNames.Length; ++index)
          {
            if ((num1 >> index & 1) != 0)
              num2 |= (uint) (1 << GameObjectUtility.GetNavMeshAreaFromName(navMeshAreaNames[index]));
          }
          this.m_WalkableMask.longValue = (long) num2;
        }
      }
      this.serializedObject.ApplyModifiedProperties();
    }

    private class Styles
    {
      public readonly GUIContent m_AgentSizeHeader = new GUIContent("Agent Size");
      public readonly GUIContent m_AgentSteeringHeader = new GUIContent("Steering");
      public readonly GUIContent m_AgentAvoidanceHeader = new GUIContent("Obstacle Avoidance");
      public readonly GUIContent m_AgentPathFindingHeader = new GUIContent("Path Finding");
    }
  }
}
