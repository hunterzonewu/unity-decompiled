// Decompiled with JetBrains decompiler
// Type: UnityEditor.AnimatorOverrideControllerInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (AnimatorOverrideController))]
  internal class AnimatorOverrideControllerInspector : Editor
  {
    private SerializedProperty m_Controller;
    private AnimationClipPair[] m_Clips;
    private ReorderableList m_ClipList;

    private void OnEnable()
    {
      AnimatorOverrideController target = this.target as AnimatorOverrideController;
      this.m_Controller = this.serializedObject.FindProperty("m_Controller");
      if (this.m_ClipList == null)
      {
        this.m_ClipList = new ReorderableList((IList) target.clips, typeof (AnimationClipPair), false, true, false, false);
        this.m_ClipList.drawElementCallback = new ReorderableList.ElementCallbackDelegate(this.DrawClipElement);
        this.m_ClipList.drawHeaderCallback = new ReorderableList.HeaderCallbackDelegate(this.DrawClipHeader);
        this.m_ClipList.elementHeight = 16f;
      }
      target.OnOverrideControllerDirty += new AnimatorOverrideController.OnOverrideControllerDirtyCallback(((Editor) this).Repaint);
    }

    private void OnDisable()
    {
      (this.target as AnimatorOverrideController).OnOverrideControllerDirty -= new AnimatorOverrideController.OnOverrideControllerDirtyCallback(((Editor) this).Repaint);
    }

    public override void OnInspectorGUI()
    {
      bool flag1 = this.targets.Length > 1;
      bool flag2 = false;
      this.serializedObject.UpdateIfDirtyOrScript();
      AnimatorOverrideController target = this.target as AnimatorOverrideController;
      RuntimeAnimatorController animatorController1 = !this.m_Controller.hasMultipleDifferentValues ? target.runtimeAnimatorController : (RuntimeAnimatorController) null;
      EditorGUI.BeginChangeCheck();
      RuntimeAnimatorController animatorController2 = EditorGUILayout.ObjectField("Controller", (Object) animatorController1, typeof (UnityEditor.Animations.AnimatorController), false, new GUILayoutOption[0]) as RuntimeAnimatorController;
      if (EditorGUI.EndChangeCheck())
      {
        for (int index = 0; index < this.targets.Length; ++index)
          (this.targets[index] as AnimatorOverrideController).runtimeAnimatorController = animatorController2;
        flag2 = true;
      }
      EditorGUI.BeginDisabledGroup(this.m_Controller == null || flag1 && this.m_Controller.hasMultipleDifferentValues || (Object) animatorController2 == (Object) null);
      EditorGUI.BeginChangeCheck();
      this.m_Clips = target.clips;
      this.m_ClipList.list = (IList) this.m_Clips;
      this.m_ClipList.DoLayoutList();
      if (EditorGUI.EndChangeCheck())
      {
        for (int index = 0; index < this.targets.Length; ++index)
          (this.targets[index] as AnimatorOverrideController).clips = this.m_Clips;
        flag2 = true;
      }
      EditorGUI.EndDisabledGroup();
      if (!flag2)
        return;
      target.PerformOverrideClipListCleanup();
    }

    private void DrawClipElement(Rect rect, int index, bool selected, bool focused)
    {
      AnimationClip originalClip = this.m_Clips[index].originalClip;
      AnimationClip overrideClip = this.m_Clips[index].overrideClip;
      rect.xMax = rect.xMax / 2f;
      GUI.Label(rect, originalClip.name, EditorStyles.label);
      rect.xMin = rect.xMax;
      rect.xMax *= 2f;
      EditorGUI.BeginChangeCheck();
      AnimationClip animationClip = EditorGUI.ObjectField(rect, string.Empty, (Object) overrideClip, typeof (AnimationClip), false) as AnimationClip;
      if (!EditorGUI.EndChangeCheck())
        return;
      this.m_Clips[index].overrideClip = animationClip;
    }

    private void DrawClipHeader(Rect rect)
    {
      rect.xMax = rect.xMax / 2f;
      GUI.Label(rect, "Original", EditorStyles.label);
      rect.xMin = rect.xMax;
      rect.xMax *= 2f;
      GUI.Label(rect, "Override", EditorStyles.label);
    }
  }
}
