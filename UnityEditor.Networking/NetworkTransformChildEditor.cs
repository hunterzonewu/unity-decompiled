// Decompiled with JetBrains decompiler
// Type: UnityEditor.NetworkTransformChildEditor
// Assembly: UnityEditor.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6916FF53-78D9-4C64-A56C-55C7AE140DAE
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.Networking.dll

using UnityEngine;
using UnityEngine.Networking;

namespace UnityEditor
{
  [CustomEditor(typeof (NetworkTransformChild), true)]
  [CanEditMultipleObjects]
  public class NetworkTransformChildEditor : Editor
  {
    private static string[] axisOptions = new string[8]
    {
      "None",
      "X",
      "Y (Top-Down 2D)",
      "Z (Side-on 2D)",
      "XY (FPS)",
      "XZ",
      "YZ",
      "XYZ (full 3D)"
    };
    private bool m_Initialized;
    private NetworkTransformChild sync;
    private SerializedProperty m_Target;
    private SerializedProperty m_MovementThreshold;
    private SerializedProperty m_InterpolateRotation;
    private SerializedProperty m_InterpolateMovement;
    private SerializedProperty m_RotationSyncCompression;
    protected GUIContent m_MovementThresholdLabel;
    protected GUIContent m_InterpolateRotationLabel;
    protected GUIContent m_InterpolateMovementLabel;
    protected GUIContent m_RotationSyncCompressionLabel;
    private SerializedProperty m_NetworkSendIntervalProperty;
    private GUIContent m_NetworkSendIntervalLabel;

    public void Init()
    {
      if (this.m_Initialized)
        return;
      this.m_Initialized = true;
      this.sync = this.target as NetworkTransformChild;
      this.m_Target = this.serializedObject.FindProperty("m_Target");
      if ((Object) this.sync.GetComponent<NetworkTransform>() == (Object) null)
      {
        if (LogFilter.logError)
          Debug.LogError((object) "NetworkTransformChild must be on the root object with the NetworkTransform, not on the child node");
        this.m_Target.objectReferenceValue = (Object) null;
      }
      this.m_MovementThreshold = this.serializedObject.FindProperty("m_MovementThreshold");
      this.m_InterpolateRotation = this.serializedObject.FindProperty("m_InterpolateRotation");
      this.m_InterpolateMovement = this.serializedObject.FindProperty("m_InterpolateMovement");
      this.m_RotationSyncCompression = this.serializedObject.FindProperty("m_RotationSyncCompression");
      this.m_NetworkSendIntervalProperty = this.serializedObject.FindProperty("m_SendInterval");
      this.m_NetworkSendIntervalLabel = new GUIContent("Network Send Rate (seconds)", "Number of network updates per second");
      ++EditorGUI.indentLevel;
      this.m_MovementThresholdLabel = new GUIContent("Movement Threshold");
      this.m_InterpolateRotationLabel = new GUIContent("Interpolate Rotation Factor");
      this.m_InterpolateMovementLabel = new GUIContent("Interpolate Movement Factor");
      this.m_RotationSyncCompressionLabel = new GUIContent("Compress Rotation");
      --EditorGUI.indentLevel;
    }

    protected void ShowControls()
    {
      if (this.m_Target == null)
        this.m_Initialized = false;
      this.Init();
      this.serializedObject.Update();
      int num1 = 0;
      if ((double) this.m_NetworkSendIntervalProperty.floatValue != 0.0)
        num1 = (int) (1.0 / (double) this.m_NetworkSendIntervalProperty.floatValue);
      int num2 = EditorGUILayout.IntSlider(this.m_NetworkSendIntervalLabel, num1, 0, 30, new GUILayoutOption[0]);
      if (num2 != num1)
        this.m_NetworkSendIntervalProperty.floatValue = num2 != 0 ? 1f / (float) num2 : 0.0f;
      if (EditorGUILayout.PropertyField(this.m_Target) && (Object) this.sync.GetComponent<NetworkTransform>() == (Object) null)
      {
        if (LogFilter.logError)
          Debug.LogError((object) "NetworkTransformChild must be on the root object with the NetworkTransform, not on the child node");
        this.m_Target.objectReferenceValue = (Object) null;
      }
      EditorGUILayout.PropertyField(this.m_MovementThreshold, this.m_MovementThresholdLabel, new GUILayoutOption[0]);
      if ((double) this.m_MovementThreshold.floatValue < 0.0)
      {
        this.m_MovementThreshold.floatValue = 0.0f;
        EditorUtility.SetDirty((Object) this.sync);
      }
      EditorGUILayout.PropertyField(this.m_InterpolateMovement, this.m_InterpolateMovementLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_InterpolateRotation, this.m_InterpolateRotationLabel, new GUILayoutOption[0]);
      int num3 = EditorGUILayout.Popup("Rotation Axis", (int) this.sync.syncRotationAxis, NetworkTransformChildEditor.axisOptions, new GUILayoutOption[0]);
      if ((NetworkTransform.AxisSyncMode) num3 != this.sync.syncRotationAxis)
      {
        this.sync.syncRotationAxis = (NetworkTransform.AxisSyncMode) num3;
        EditorUtility.SetDirty((Object) this.sync);
      }
      EditorGUILayout.PropertyField(this.m_RotationSyncCompression, this.m_RotationSyncCompressionLabel, new GUILayoutOption[0]);
      this.serializedObject.ApplyModifiedProperties();
    }

    public override void OnInspectorGUI()
    {
      this.ShowControls();
    }
  }
}
