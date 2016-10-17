// Decompiled with JetBrains decompiler
// Type: UnityEditor.Collider2DEditorBase
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor
{
  internal class Collider2DEditorBase : ColliderEditorBase
  {
    private readonly AnimBool m_ShowDensity = new AnimBool();
    private SerializedProperty m_Density;

    public override void OnEnable()
    {
      base.OnEnable();
      this.m_Density = this.serializedObject.FindProperty("m_Density");
      Collider2D target = this.target as Collider2D;
      this.m_ShowDensity.value = (bool) ((Object) target.attachedRigidbody) && target.attachedRigidbody.useAutoMass;
      this.m_ShowDensity.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
    }

    public override void OnDisable()
    {
      this.m_ShowDensity.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      base.OnDisable();
    }

    public override void OnInspectorGUI()
    {
      Collider2D target = this.target as Collider2D;
      this.serializedObject.Update();
      this.m_ShowDensity.target = (bool) ((Object) target.attachedRigidbody) && target.attachedRigidbody.useAutoMass;
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowDensity.faded))
        EditorGUILayout.PropertyField(this.m_Density);
      EditorGUILayout.EndFadeGroup();
      this.serializedObject.ApplyModifiedProperties();
      base.OnInspectorGUI();
      this.CheckColliderErrorState();
      Effector2DEditor.CheckEffectorWarnings(this.target as Collider2D);
    }

    internal override void OnForceReloadInspector()
    {
      base.OnForceReloadInspector();
      if (!this.editingCollider)
        return;
      this.ForceQuitEditMode();
    }

    protected void CheckColliderErrorState()
    {
      switch ((this.target as Collider2D).errorState)
      {
        case ColliderErrorState2D.NoShapes:
          EditorGUILayout.HelpBox("The collider did not create any collision shapes as they all failed verification.  This could be because they were deemed too small or the vertices were too close.  Vertices can also become close under certain rotations or very small scaling.", MessageType.Warning);
          break;
        case ColliderErrorState2D.RemovedShapes:
          EditorGUILayout.HelpBox("The collider created collision shape(s) but some were removed as they failed verification.  This could be because they were deemed too small or the vertices were too close.  Vertices can also become close under certain rotations or very small scaling.", MessageType.Warning);
          break;
      }
    }

    protected void BeginColliderInspector()
    {
      this.serializedObject.Update();
      EditorGUI.BeginDisabledGroup(this.targets.Length > 1);
      this.InspectorEditButtonGUI();
      EditorGUI.EndDisabledGroup();
    }

    protected void EndColliderInspector()
    {
      this.serializedObject.ApplyModifiedProperties();
    }
  }
}
