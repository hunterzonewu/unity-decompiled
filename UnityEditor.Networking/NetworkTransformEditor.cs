// Decompiled with JetBrains decompiler
// Type: UnityEditor.NetworkTransformEditor
// Assembly: UnityEditor.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6916FF53-78D9-4C64-A56C-55C7AE140DAE
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.Networking.dll

using UnityEngine;
using UnityEngine.Networking;

namespace UnityEditor
{
  [CustomEditor(typeof (NetworkTransform), true)]
  [CanEditMultipleObjects]
  public class NetworkTransformEditor : Editor
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
    private NetworkTransform m_SyncTransform;
    private SerializedProperty m_TransformSyncMode;
    private SerializedProperty m_MovementTheshold;
    private SerializedProperty m_SnapThreshold;
    private SerializedProperty m_InterpolateRotation;
    private SerializedProperty m_InterpolateMovement;
    private SerializedProperty m_RotationSyncCompression;
    private SerializedProperty m_SyncSpin;
    protected GUIContent m_MovementThesholdLabel;
    protected GUIContent m_SnapThresholdLabel;
    protected GUIContent m_InterpolateRotationLabel;
    protected GUIContent m_InterpolateMovementLabel;
    protected GUIContent m_RotationSyncCompressionLabel;
    protected GUIContent m_SyncSpinLabel;
    private SerializedProperty m_NetworkSendIntervalProperty;
    private GUIContent m_NetworkSendIntervalLabel;

    public void Init()
    {
      if (this.m_Initialized)
        return;
      this.m_Initialized = true;
      this.m_SyncTransform = this.target as NetworkTransform;
      if (this.m_SyncTransform.transformSyncMode == NetworkTransform.TransformSyncMode.SyncNone)
      {
        if ((Object) this.m_SyncTransform.GetComponent<Rigidbody>() != (Object) null)
        {
          this.m_SyncTransform.transformSyncMode = NetworkTransform.TransformSyncMode.SyncRigidbody3D;
          this.m_SyncTransform.syncRotationAxis = NetworkTransform.AxisSyncMode.AxisXYZ;
          EditorUtility.SetDirty((Object) this.m_SyncTransform);
        }
        else if ((Object) this.m_SyncTransform.GetComponent<Rigidbody2D>() != (Object) null)
        {
          this.m_SyncTransform.transformSyncMode = NetworkTransform.TransformSyncMode.SyncRigidbody2D;
          this.m_SyncTransform.syncRotationAxis = NetworkTransform.AxisSyncMode.AxisZ;
          EditorUtility.SetDirty((Object) this.m_SyncTransform);
        }
        else if ((Object) this.m_SyncTransform.GetComponent<CharacterController>() != (Object) null)
        {
          this.m_SyncTransform.transformSyncMode = NetworkTransform.TransformSyncMode.SyncCharacterController;
          this.m_SyncTransform.syncRotationAxis = NetworkTransform.AxisSyncMode.AxisXYZ;
          EditorUtility.SetDirty((Object) this.m_SyncTransform);
        }
        else
        {
          this.m_SyncTransform.transformSyncMode = NetworkTransform.TransformSyncMode.SyncTransform;
          this.m_SyncTransform.syncRotationAxis = NetworkTransform.AxisSyncMode.AxisXYZ;
          EditorUtility.SetDirty((Object) this.m_SyncTransform);
        }
      }
      this.m_TransformSyncMode = this.serializedObject.FindProperty("m_TransformSyncMode");
      this.m_MovementTheshold = this.serializedObject.FindProperty("m_MovementTheshold");
      this.m_SnapThreshold = this.serializedObject.FindProperty("m_SnapThreshold");
      this.m_InterpolateRotation = this.serializedObject.FindProperty("m_InterpolateRotation");
      this.m_InterpolateMovement = this.serializedObject.FindProperty("m_InterpolateMovement");
      this.m_RotationSyncCompression = this.serializedObject.FindProperty("m_RotationSyncCompression");
      this.m_SyncSpin = this.serializedObject.FindProperty("m_SyncSpin");
      this.m_NetworkSendIntervalProperty = this.serializedObject.FindProperty("m_SendInterval");
      this.m_NetworkSendIntervalLabel = new GUIContent("Network Send Rate (seconds)", "Number of network updates per second");
      ++EditorGUI.indentLevel;
      this.m_MovementThesholdLabel = new GUIContent("Movement Threshold");
      this.m_SnapThresholdLabel = new GUIContent("Snap Threshold");
      this.m_InterpolateRotationLabel = new GUIContent("Interpolate Rotation Factor");
      this.m_InterpolateMovementLabel = new GUIContent("Interpolate Movement Factor");
      this.m_RotationSyncCompressionLabel = new GUIContent("Compress Rotation");
      this.m_SyncSpinLabel = new GUIContent("Sync Angular Velocity");
      --EditorGUI.indentLevel;
    }

    protected void ShowControls()
    {
      if (this.m_TransformSyncMode == null)
        this.m_Initialized = false;
      this.Init();
      this.serializedObject.Update();
      int num1 = 0;
      if ((double) this.m_NetworkSendIntervalProperty.floatValue != 0.0)
        num1 = (int) (1.0 / (double) this.m_NetworkSendIntervalProperty.floatValue);
      int num2 = EditorGUILayout.IntSlider(this.m_NetworkSendIntervalLabel, num1, 0, 30, new GUILayoutOption[0]);
      if (num2 != num1)
        this.m_NetworkSendIntervalProperty.floatValue = num2 != 0 ? 1f / (float) num2 : 0.0f;
      EditorGUILayout.PropertyField(this.m_TransformSyncMode);
      if (this.m_TransformSyncMode.enumValueIndex == 3 && (Object) this.m_SyncTransform.GetComponent<Rigidbody>() == (Object) null)
      {
        Debug.LogError((object) "Object has no Rigidbody component.");
        this.m_TransformSyncMode.enumValueIndex = 1;
        EditorUtility.SetDirty((Object) this.m_SyncTransform);
      }
      if (this.m_TransformSyncMode.enumValueIndex == 2 && (Object) this.m_SyncTransform.GetComponent<Rigidbody2D>() == (Object) null)
      {
        Debug.LogError((object) "Object has no Rigidbody2D component.");
        this.m_TransformSyncMode.enumValueIndex = 1;
        EditorUtility.SetDirty((Object) this.m_SyncTransform);
      }
      if (this.m_TransformSyncMode.enumValueIndex == 4 && (Object) this.m_SyncTransform.GetComponent<CharacterController>() == (Object) null)
      {
        Debug.LogError((object) "Object has no CharacterController component.");
        this.m_TransformSyncMode.enumValueIndex = 1;
        EditorUtility.SetDirty((Object) this.m_SyncTransform);
      }
      EditorGUILayout.LabelField("Movement:");
      ++EditorGUI.indentLevel;
      EditorGUILayout.PropertyField(this.m_MovementTheshold, this.m_MovementThesholdLabel, new GUILayoutOption[0]);
      if ((double) this.m_MovementTheshold.floatValue < 0.0)
      {
        this.m_MovementTheshold.floatValue = 0.0f;
        EditorUtility.SetDirty((Object) this.m_SyncTransform);
      }
      EditorGUILayout.PropertyField(this.m_SnapThreshold, this.m_SnapThresholdLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_InterpolateMovement, this.m_InterpolateMovementLabel, new GUILayoutOption[0]);
      --EditorGUI.indentLevel;
      EditorGUILayout.LabelField("Rotation:");
      ++EditorGUI.indentLevel;
      int num3 = EditorGUILayout.Popup("Rotation Axis", (int) this.m_SyncTransform.syncRotationAxis, NetworkTransformEditor.axisOptions, new GUILayoutOption[0]);
      if ((NetworkTransform.AxisSyncMode) num3 != this.m_SyncTransform.syncRotationAxis)
      {
        this.m_SyncTransform.syncRotationAxis = (NetworkTransform.AxisSyncMode) num3;
        EditorUtility.SetDirty((Object) this.m_SyncTransform);
      }
      EditorGUILayout.PropertyField(this.m_InterpolateRotation, this.m_InterpolateRotationLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_RotationSyncCompression, this.m_RotationSyncCompressionLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_SyncSpin, this.m_SyncSpinLabel, new GUILayoutOption[0]);
      --EditorGUI.indentLevel;
      this.serializedObject.ApplyModifiedProperties();
    }

    public override void OnInspectorGUI()
    {
      this.ShowControls();
    }
  }
}
