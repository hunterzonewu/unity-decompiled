// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioMixerExposedParameterView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEditor.Audio;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class AudioMixerExposedParameterView
  {
    private ReorderableListWithRenameAndScrollView m_ReorderableListWithRenameAndScrollView;
    private AudioMixerController m_Controller;
    private SerializedObject m_ControllerSerialized;
    private ReorderableListWithRenameAndScrollView.State m_State;

    private float height
    {
      get
      {
        return this.m_ReorderableListWithRenameAndScrollView.list.GetHeight();
      }
    }

    public AudioMixerExposedParameterView(ReorderableListWithRenameAndScrollView.State state)
    {
      this.m_State = state;
    }

    public void OnMixerControllerChanged(AudioMixerController controller)
    {
      this.m_Controller = controller;
      if ((bool) ((UnityEngine.Object) this.m_Controller))
        this.m_Controller.ChangedExposedParameter += new ChangedExposedParameterHandler(this.RecreateListControl);
      this.RecreateListControl();
    }

    public void RecreateListControl()
    {
      if (!((UnityEngine.Object) this.m_Controller != (UnityEngine.Object) null))
        return;
      this.m_ControllerSerialized = new SerializedObject((UnityEngine.Object) this.m_Controller);
      ReorderableList list = new ReorderableList(this.m_ControllerSerialized, this.m_ControllerSerialized.FindProperty("m_ExposedParameters"), false, false, false, false);
      list.onReorderCallback = new ReorderableList.ReorderCallbackDelegate(this.EndDragChild);
      list.drawElementCallback += new ReorderableList.ElementCallbackDelegate(this.DrawElement);
      list.elementHeight = 16f;
      list.headerHeight = 0.0f;
      list.footerHeight = 0.0f;
      list.showDefaultBackground = false;
      this.m_ReorderableListWithRenameAndScrollView = new ReorderableListWithRenameAndScrollView(list, this.m_State);
      this.m_ReorderableListWithRenameAndScrollView.onNameChangedAtIndex += new System.Action<int, string>(this.NameChanged);
      this.m_ReorderableListWithRenameAndScrollView.onDeleteItemAtIndex += new System.Action<int>(this.Delete);
      this.m_ReorderableListWithRenameAndScrollView.onGetNameAtIndex += new Func<int, string>(this.GetNameOfElement);
    }

    public void OnGUI(Rect rect)
    {
      if ((UnityEngine.Object) this.m_Controller == (UnityEngine.Object) null)
        return;
      this.m_ReorderableListWithRenameAndScrollView.OnGUI(rect);
    }

    public void OnContextClick(int itemIndex)
    {
      GenericMenu genericMenu = new GenericMenu();
      genericMenu.AddItem(new GUIContent("Unexpose"), false, (GenericMenu.MenuFunction2) (data => this.Delete((int) data)), (object) itemIndex);
      genericMenu.AddItem(new GUIContent("Rename"), false, (GenericMenu.MenuFunction2) (data => this.m_ReorderableListWithRenameAndScrollView.BeginRename((int) data, 0.0f)), (object) itemIndex);
      genericMenu.ShowAsContext();
    }

    private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
    {
      Event current = Event.current;
      if (current.type == EventType.ContextClick && rect.Contains(current.mousePosition))
      {
        this.OnContextClick(index);
        current.Use();
      }
      if (Event.current.type != EventType.Repaint)
        return;
      EditorGUI.BeginDisabledGroup(true);
      this.m_ReorderableListWithRenameAndScrollView.elementStyleRightAligned.Draw(rect, this.GetInfoString(index), false, false, false, false);
      EditorGUI.EndDisabledGroup();
    }

    public Vector2 CalcSize()
    {
      float x = 0.0f;
      for (int index = 0; index < this.m_ReorderableListWithRenameAndScrollView.list.count; ++index)
      {
        float num = this.WidthOfRow(index, this.m_ReorderableListWithRenameAndScrollView.elementStyle, this.m_ReorderableListWithRenameAndScrollView.elementStyleRightAligned);
        if ((double) num > (double) x)
          x = num;
      }
      return new Vector2(x, this.height);
    }

    private string GetInfoString(int index)
    {
      return this.m_Controller.ResolveExposedParameterPath(this.m_Controller.exposedParameters[index].guid, false);
    }

    private float WidthOfRow(int index, GUIStyle leftStyle, GUIStyle rightStyle)
    {
      string infoString = this.GetInfoString(index);
      Vector2 vector2 = rightStyle.CalcSize(GUIContent.Temp(infoString));
      return (float) ((double) leftStyle.CalcSize(GUIContent.Temp(this.GetNameOfElement(index))).x + (double) vector2.x + 25.0);
    }

    private string GetNameOfElement(int index)
    {
      return this.m_Controller.exposedParameters[index].name;
    }

    public void NameChanged(int index, string newName)
    {
      if (newName.Length > 64)
      {
        newName = newName.Substring(0, 64);
        Debug.LogWarning((object) ("Maximum name length of an exposed parameter is " + (object) 64 + " characters. Name truncated to '" + newName + "'"));
      }
      ExposedAudioParameter[] exposedParameters = this.m_Controller.exposedParameters;
      exposedParameters[index].name = newName;
      this.m_Controller.exposedParameters = exposedParameters;
    }

    private void Delete(int index)
    {
      if (!((UnityEngine.Object) this.m_Controller != (UnityEngine.Object) null))
        return;
      Undo.RecordObject((UnityEngine.Object) this.m_Controller, "Unexpose Mixer Parameter");
      this.m_Controller.RemoveExposedParameter(this.m_Controller.exposedParameters[index].guid);
    }

    public void EndDragChild(ReorderableList list)
    {
      this.m_ControllerSerialized.ApplyModifiedProperties();
    }

    public void OnEvent()
    {
      this.m_ReorderableListWithRenameAndScrollView.OnEvent();
    }
  }
}
