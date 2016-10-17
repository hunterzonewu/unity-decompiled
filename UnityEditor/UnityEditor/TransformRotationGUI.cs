// Decompiled with JetBrains decompiler
// Type: UnityEditor.TransformRotationGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class TransformRotationGUI
  {
    private static int s_FoldoutHash = "Foldout".GetHashCode();
    private GUIContent rotationContent = new GUIContent("Rotation", "The local rotation of this Game Object relative to the parent.");
    private Vector3 m_OldEulerAngles = new Vector3(1000000f, 1E+07f, 1000000f);
    private RotationOrder m_OldRotationOrder = RotationOrder.OrderZXY;
    private Vector3 m_EulerAngles;
    private SerializedProperty m_Rotation;
    private UnityEngine.Object[] targets;

    public void OnEnable(SerializedProperty m_Rotation, GUIContent label)
    {
      this.m_Rotation = m_Rotation;
      this.targets = m_Rotation.serializedObject.targetObjects;
      this.m_OldRotationOrder = (this.targets[0] as Transform).rotationOrder;
      this.rotationContent = label;
    }

    public void RotationField()
    {
      this.RotationField(false);
    }

    public void RotationField(bool disabled)
    {
      Transform target1 = this.targets[0] as Transform;
      Vector3 localEulerAngles1 = target1.GetLocalEulerAngles(target1.rotationOrder);
      if ((double) this.m_OldEulerAngles.x != (double) localEulerAngles1.x || (double) this.m_OldEulerAngles.y != (double) localEulerAngles1.y || ((double) this.m_OldEulerAngles.z != (double) localEulerAngles1.z || this.m_OldRotationOrder != target1.rotationOrder))
      {
        this.m_EulerAngles = target1.GetLocalEulerAngles(target1.rotationOrder);
        this.m_OldRotationOrder = target1.rotationOrder;
      }
      bool flag1 = false;
      bool flag2 = false;
      for (int index = 1; index < this.targets.Length; ++index)
      {
        Transform target2 = this.targets[index] as Transform;
        Vector3 localEulerAngles2 = target2.GetLocalEulerAngles(target2.rotationOrder);
        flag1 = ((flag1 ? 1 : 0) | ((double) localEulerAngles2.x != (double) localEulerAngles1.x || (double) localEulerAngles2.y != (double) localEulerAngles1.y ? 1 : ((double) localEulerAngles2.z != (double) localEulerAngles1.z ? 1 : 0))) != 0;
        flag2 |= target2.rotationOrder != target1.rotationOrder;
      }
      Rect controlRect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight * (!EditorGUIUtility.wideMode ? 2f : 1f), new GUILayoutOption[0]);
      GUIContent label = EditorGUI.BeginProperty(controlRect, this.rotationContent, this.m_Rotation);
      EditorGUI.showMixedValue = flag1;
      EditorGUI.BeginChangeCheck();
      int controlId = GUIUtility.GetControlID(TransformRotationGUI.s_FoldoutHash, EditorGUIUtility.native, controlRect);
      string empty = string.Empty;
      if (AnimationMode.InAnimationMode() && target1.rotationOrder != RotationOrder.OrderZXY)
      {
        string str1;
        if (flag2)
        {
          str1 = "Mixed";
        }
        else
        {
          string str2 = target1.rotationOrder.ToString();
          str1 = str2.Substring(str2.Length - 3);
        }
        label.text = label.text + " (" + str1 + ")";
      }
      Rect position = EditorGUI.MultiFieldPrefixLabel(controlRect, controlId, label, 3);
      position.height = EditorGUIUtility.singleLineHeight;
      EditorGUI.BeginDisabledGroup(disabled);
      this.m_EulerAngles = EditorGUI.Vector3Field(position, GUIContent.none, this.m_EulerAngles);
      EditorGUI.EndDisabledGroup();
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObjects(this.targets, "Inspector");
        foreach (Transform target2 in this.targets)
        {
          target2.SetLocalEulerAngles(this.m_EulerAngles, target2.rotationOrder);
          if ((UnityEngine.Object) target2.parent != (UnityEngine.Object) null)
            target2.SendTransformChangedScale();
        }
        this.m_Rotation.serializedObject.SetIsDifferentCacheDirty();
      }
      EditorGUI.showMixedValue = false;
      if (flag2)
        EditorGUILayout.HelpBox("Transforms have different rotation orders, keyframes saved will have the same value but not the same local rotation", MessageType.Warning);
      EditorGUI.EndProperty();
    }
  }
}
