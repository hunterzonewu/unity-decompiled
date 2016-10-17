// Decompiled with JetBrains decompiler
// Type: UnityEditor.LightProbeGroupInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (LightProbeGroup))]
  internal class LightProbeGroupInspector : Editor
  {
    private LightProbeGroupEditor m_Editor;
    private bool m_EditingProbes;
    private bool m_ShouldFocus;

    public void OnEnable()
    {
      this.m_Editor = new LightProbeGroupEditor(this.target as LightProbeGroup);
      this.m_Editor.PullProbePositions();
      this.m_Editor.DeselectProbes();
      this.m_Editor.PushProbePositions();
      SceneView.onSceneGUIDelegate += new SceneView.OnSceneFunc(this.OnSceneGUIDelegate);
      Undo.undoRedoPerformed += new Undo.UndoRedoCallback(this.UndoRedoPerformed);
    }

    private void StartEditProbes()
    {
      if (this.m_EditingProbes)
        return;
      this.m_EditingProbes = true;
      this.m_Editor.SetEditing(true);
      Tools.s_Hidden = true;
      SceneView.RepaintAll();
    }

    private void EndEditProbes()
    {
      if (!this.m_EditingProbes)
        return;
      this.m_Editor.DeselectProbes();
      this.m_EditingProbes = false;
      Tools.s_Hidden = false;
    }

    public void OnDisable()
    {
      this.EndEditProbes();
      Undo.undoRedoPerformed -= new Undo.UndoRedoCallback(this.UndoRedoPerformed);
      SceneView.onSceneGUIDelegate -= new SceneView.OnSceneFunc(this.OnSceneGUIDelegate);
      if (!(this.target != (Object) null))
        return;
      this.m_Editor.PushProbePositions();
    }

    private void UndoRedoPerformed()
    {
      this.m_Editor.PullProbePositions();
      this.m_Editor.MarkTetrahedraDirty();
    }

    public override void OnInspectorGUI()
    {
      this.m_Editor.PullProbePositions();
      GUILayout.BeginHorizontal();
      GUILayout.BeginVertical();
      if (GUILayout.Button("Add Probe"))
      {
        Vector3 position = Vector3.zero;
        if ((bool) ((Object) SceneView.lastActiveSceneView))
        {
          position = SceneView.lastActiveSceneView.pivot;
          LightProbeGroup target = this.target as LightProbeGroup;
          if ((bool) ((Object) target))
            position = target.transform.InverseTransformPoint(position);
        }
        this.StartEditProbes();
        this.m_Editor.DeselectProbes();
        this.m_Editor.AddProbe(position);
      }
      if (GUILayout.Button("Delete Selected"))
      {
        this.StartEditProbes();
        this.m_Editor.RemoveSelectedProbes();
      }
      GUILayout.EndVertical();
      GUILayout.BeginVertical();
      if (GUILayout.Button("Select All"))
      {
        this.StartEditProbes();
        this.m_Editor.SelectAllProbes();
      }
      if (GUILayout.Button("Duplicate Selected"))
      {
        this.StartEditProbes();
        this.m_Editor.DuplicateSelectedProbes();
      }
      GUILayout.EndVertical();
      GUILayout.EndHorizontal();
      if (this.m_Editor.SelectedCount == 1)
      {
        Vector3 selectedPosition = this.m_Editor.GetSelectedPositions()[0];
        Vector3 position = EditorGUILayout.Vector3Field(new GUIContent("Probe Position", "The local position of this probe relative to the parent group."), selectedPosition);
        if (position != selectedPosition)
        {
          this.StartEditProbes();
          this.m_Editor.UpdateSelectedPosition(0, position);
        }
      }
      this.m_Editor.HandleEditMenuHotKeyCommands();
      this.m_Editor.PushProbePositions();
    }

    private void InternalOnSceneView()
    {
      if (!EditorGUIUtility.IsGizmosAllowedForObject(this.target))
        return;
      if ((Object) SceneView.lastActiveSceneView != (Object) null && this.m_ShouldFocus)
      {
        this.m_ShouldFocus = false;
        SceneView.lastActiveSceneView.FrameSelected();
      }
      this.m_Editor.PullProbePositions();
      LightProbeGroup target = this.target as LightProbeGroup;
      if ((Object) target != (Object) null)
      {
        if (this.m_Editor.OnSceneGUI(target.transform))
          this.StartEditProbes();
        else
          this.EndEditProbes();
      }
      this.m_Editor.PushProbePositions();
    }

    public void OnSceneGUI()
    {
      if (Event.current.type == EventType.Repaint)
        return;
      this.InternalOnSceneView();
    }

    public void OnSceneGUIDelegate(SceneView sceneView)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      this.InternalOnSceneView();
    }

    public bool HasFrameBounds()
    {
      return this.m_Editor.SelectedCount > 0;
    }

    public Bounds OnGetFrameBounds()
    {
      return this.m_Editor.selectedProbeBounds;
    }
  }
}
