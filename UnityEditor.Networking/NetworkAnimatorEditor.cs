// Decompiled with JetBrains decompiler
// Type: UnityEditor.NetworkAnimatorEditor
// Assembly: UnityEditor.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6916FF53-78D9-4C64-A56C-55C7AE140DAE
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.Networking.dll

using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (NetworkAnimator), true)]
  public class NetworkAnimatorEditor : Editor
  {
    private NetworkAnimator m_AnimSync;
    private bool m_Initialized;
    private SerializedProperty m_AnimatorProperty;

    private void Init()
    {
      if (this.m_Initialized)
        return;
      this.m_Initialized = true;
      this.m_AnimSync = this.target as NetworkAnimator;
      this.m_AnimatorProperty = this.serializedObject.FindProperty("m_Animator");
    }

    public override void OnInspectorGUI()
    {
      this.Init();
      this.serializedObject.Update();
      this.DrawControls();
      this.serializedObject.ApplyModifiedProperties();
    }

    private void DrawControls()
    {
      EditorGUI.BeginChangeCheck();
      EditorGUILayout.PropertyField(this.m_AnimatorProperty);
      if (EditorGUI.EndChangeCheck())
        this.m_AnimSync.ResetParameterOptions();
      if ((Object) this.m_AnimSync.animator == (Object) null)
        return;
      AnimatorController animatorController = this.m_AnimSync.animator.runtimeAnimatorController as AnimatorController;
      if ((Object) animatorController != (Object) null)
      {
        ++EditorGUI.indentLevel;
        int index = 0;
        foreach (AnimatorControllerParameter parameter in animatorController.parameters)
        {
          bool parameterAutoSend = this.m_AnimSync.GetParameterAutoSend(index);
          bool flag = EditorGUILayout.Toggle(parameter.name, parameterAutoSend, new GUILayoutOption[0]);
          if (flag != parameterAutoSend)
          {
            this.m_AnimSync.SetParameterAutoSend(index, flag);
            EditorUtility.SetDirty(this.target);
          }
          ++index;
        }
        --EditorGUI.indentLevel;
      }
      if (!Application.isPlaying)
        return;
      EditorGUILayout.Separator();
      if (this.m_AnimSync.param0 != string.Empty)
        EditorGUILayout.LabelField("Param 0", this.m_AnimSync.param0, new GUILayoutOption[0]);
      if (this.m_AnimSync.param1 != string.Empty)
        EditorGUILayout.LabelField("Param 1", this.m_AnimSync.param1, new GUILayoutOption[0]);
      if (this.m_AnimSync.param2 != string.Empty)
        EditorGUILayout.LabelField("Param 2", this.m_AnimSync.param2, new GUILayoutOption[0]);
      if (this.m_AnimSync.param3 != string.Empty)
        EditorGUILayout.LabelField("Param 3", this.m_AnimSync.param3, new GUILayoutOption[0]);
      if (!(this.m_AnimSync.param4 != string.Empty))
        return;
      EditorGUILayout.LabelField("Param 4", this.m_AnimSync.param4, new GUILayoutOption[0]);
    }
  }
}
