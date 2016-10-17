// Decompiled with JetBrains decompiler
// Type: UnityEditor.Effector2DEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor
{
  [CustomEditor(typeof (Effector2D), true)]
  [CanEditMultipleObjects]
  internal class Effector2DEditor : Editor
  {
    private readonly AnimBool m_ShowColliderMask = new AnimBool();
    private SerializedProperty m_UseColliderMask;
    private SerializedProperty m_ColliderMask;

    public void OnEnable()
    {
      this.m_UseColliderMask = this.serializedObject.FindProperty("m_UseColliderMask");
      this.m_ColliderMask = this.serializedObject.FindProperty("m_ColliderMask");
      this.m_ShowColliderMask.value = (this.target as Effector2D).useColliderMask;
      this.m_ShowColliderMask.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
    }

    public void OnDisable()
    {
      this.m_ShowColliderMask.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      Effector2D target = this.target as Effector2D;
      this.m_ShowColliderMask.target = target.useColliderMask;
      EditorGUILayout.PropertyField(this.m_UseColliderMask);
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowColliderMask.faded))
        EditorGUILayout.PropertyField(this.m_ColliderMask);
      EditorGUILayout.EndFadeGroup();
      this.serializedObject.ApplyModifiedProperties();
      base.OnInspectorGUI();
      if (((IEnumerable<Collider2D>) target.GetComponents<Collider2D>()).Any<Collider2D>((Func<Collider2D, bool>) (collider =>
      {
        if (collider.enabled)
          return collider.usedByEffector;
        return false;
      })))
        return;
      if (target.requiresCollider)
        EditorGUILayout.HelpBox("This effector will not function until there is at least one enabled 2D collider with 'Used by Effector' checked on this GameObject.", MessageType.Warning);
      else
        EditorGUILayout.HelpBox("This effector can optionally work without a 2D collider.", MessageType.Info);
    }

    public static void CheckEffectorWarnings(Collider2D collider)
    {
      if (!collider.usedByEffector)
        return;
      Effector2D component = collider.GetComponent<Effector2D>();
      if ((UnityEngine.Object) component == (UnityEngine.Object) null || !component.enabled)
      {
        EditorGUILayout.HelpBox("This collider will not function with an effector until there is at least one enabled 2D effector on this GameObject.", MessageType.Warning);
        if ((UnityEngine.Object) component == (UnityEngine.Object) null)
          return;
      }
      if (component.designedForNonTrigger && collider.isTrigger)
      {
        EditorGUILayout.HelpBox("This collider has 'Is Trigger' checked but this should be unchecked when used with the '" + component.GetType().Name + "' component which is designed to work with collisions.", MessageType.Warning);
      }
      else
      {
        if (!component.designedForTrigger || collider.isTrigger)
          return;
        EditorGUILayout.HelpBox("This collider has 'Is Trigger' unchecked but this should be checked when used with the '" + component.GetType().Name + "' component which is designed to work with triggers.", MessageType.Warning);
      }
    }
  }
}
