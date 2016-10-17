// Decompiled with JetBrains decompiler
// Type: UnityEditor.OptimizedGUIBlock
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  internal sealed class OptimizedGUIBlock
  {
    [NonSerialized]
    private IntPtr m_Ptr;
    private bool m_Valid;
    private bool m_Recording;
    private bool m_WatchForUsed;
    private int m_KeyboardControl;
    private int m_LastSearchIndex;
    private int m_ActiveDragControl;
    private Color m_GUIColor;
    private Rect m_Rect;

    public bool valid
    {
      get
      {
        return this.m_Valid;
      }
      set
      {
        this.m_Valid = value;
      }
    }

    public OptimizedGUIBlock()
    {
      this.Init();
    }

    ~OptimizedGUIBlock()
    {
      if (!(this.m_Ptr != IntPtr.Zero))
        return;
      Debug.Log((object) "Failed cleaning up Optimized GUI Block");
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Init();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Dispose();

    public bool Begin(bool hasChanged, Rect position)
    {
      if (hasChanged)
        this.m_Valid = false;
      if (Event.current.type == EventType.Repaint)
      {
        if (GUIUtility.keyboardControl != this.m_KeyboardControl)
        {
          this.m_Valid = false;
          this.m_KeyboardControl = GUIUtility.keyboardControl;
        }
        if (DragAndDrop.activeControlID != this.m_ActiveDragControl)
        {
          this.m_Valid = false;
          this.m_ActiveDragControl = DragAndDrop.activeControlID;
        }
        if (GUI.color != this.m_GUIColor)
        {
          this.m_Valid = false;
          this.m_GUIColor = GUI.color;
        }
        position = GUIClip.Unclip(position);
        if (this.m_Valid && position != this.m_Rect)
        {
          this.m_Rect = position;
          this.m_Valid = false;
        }
        if (EditorGUI.isCollectingTooltips)
          return true;
        if (this.m_Valid)
          return false;
        this.m_Recording = true;
        this.BeginRecording();
        return true;
      }
      if (Event.current.type == EventType.Used)
        return false;
      if (Event.current.type != EventType.Used)
        this.m_WatchForUsed = true;
      return true;
    }

    public void End()
    {
      bool recording = this.m_Recording;
      if (this.m_Recording)
      {
        this.EndRecording();
        this.m_Recording = false;
        this.m_Valid = true;
        this.m_LastSearchIndex = EditorGUIUtility.GetSearchIndexOfControlIDList();
      }
      if (Event.current == null)
        Debug.LogError((object) "Event.current is null");
      if (Event.current.type == EventType.Repaint && !EditorGUI.isCollectingTooltips)
      {
        this.Execute();
        if (!recording)
          EditorGUIUtility.SetSearchIndexOfControlIDList(this.m_LastSearchIndex);
      }
      if (this.m_WatchForUsed && Event.current.type == EventType.Used)
        this.m_Valid = false;
      this.m_WatchForUsed = false;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void BeginRecording();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void EndRecording();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Execute();
  }
}
