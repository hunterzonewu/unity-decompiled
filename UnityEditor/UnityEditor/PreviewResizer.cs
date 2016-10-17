// Decompiled with JetBrains decompiler
// Type: UnityEditor.PreviewResizer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class PreviewResizer
  {
    private static float s_DraggedPreviewSize;
    private static float s_CachedPreviewSizeWhileDragging;
    private static float s_MouseDownLocation;
    private static float s_MouseDownValue;
    private static bool s_MouseDragged;
    [SerializeField]
    private float m_CachedPref;
    [SerializeField]
    private int m_ControlHash;
    [SerializeField]
    private string m_PrefName;
    private int m_Id;

    private int id
    {
      get
      {
        if (this.m_Id == 0)
          this.m_Id = GUIUtility.GetControlID(this.m_ControlHash, FocusType.Passive, new Rect());
        return this.m_Id;
      }
    }

    public void Init(string prefName)
    {
      if (this.m_ControlHash != 0 && !string.IsNullOrEmpty(this.m_PrefName))
        return;
      this.m_ControlHash = prefName.GetHashCode();
      this.m_PrefName = "Preview_" + prefName;
      this.m_CachedPref = EditorPrefs.GetFloat(this.m_PrefName, 1f);
    }

    public float ResizeHandle(Rect windowPosition, float minSize, float minRemainingSize, float resizerHeight)
    {
      return this.ResizeHandle(windowPosition, minSize, minRemainingSize, resizerHeight, new Rect());
    }

    public float ResizeHandle(Rect windowPosition, float minSize, float minRemainingSize, float resizerHeight, Rect dragRect)
    {
      if ((double) Mathf.Abs(this.m_CachedPref) < (double) minSize)
        this.m_CachedPref = minSize * Mathf.Sign(this.m_CachedPref);
      float b = windowPosition.height - minRemainingSize;
      float num1 = GUIUtility.hotControl != this.id ? Mathf.Max(0.0f, this.m_CachedPref) : PreviewResizer.s_DraggedPreviewSize;
      bool expanded = (double) this.m_CachedPref > 0.0;
      float num2 = Mathf.Abs(this.m_CachedPref);
      Rect position = new Rect(0.0f, windowPosition.height - num1 - resizerHeight, windowPosition.width, resizerHeight);
      if ((double) dragRect.width != 0.0)
      {
        position.x = dragRect.x;
        position.width = dragRect.width;
      }
      bool flag1 = expanded;
      float num3 = Mathf.Min(-PreviewResizer.PixelPreciseCollapsibleSlider(this.id, position, -num1, -b, 0.0f, ref expanded), b);
      if (GUIUtility.hotControl == this.id)
        PreviewResizer.s_DraggedPreviewSize = num3;
      if ((double) num3 < (double) minSize)
        num3 = (double) num3 >= (double) minSize * 0.5 ? minSize : 0.0f;
      if (expanded != flag1)
        num3 = !expanded ? 0.0f : num2;
      bool flag2 = (double) num3 >= (double) minSize / 2.0;
      if (GUIUtility.hotControl == 0)
      {
        if ((double) num3 > 0.0)
          num2 = num3;
        float num4 = num2 * (!flag2 ? -1f : 1f);
        if ((double) num4 != (double) this.m_CachedPref)
        {
          this.m_CachedPref = num4;
          EditorPrefs.SetFloat(this.m_PrefName, this.m_CachedPref);
        }
      }
      PreviewResizer.s_CachedPreviewSizeWhileDragging = num3;
      return num3;
    }

    public bool GetExpanded()
    {
      if (GUIUtility.hotControl == this.id)
        return (double) PreviewResizer.s_CachedPreviewSizeWhileDragging > 0.0;
      return (double) this.m_CachedPref > 0.0;
    }

    public float GetPreviewSize()
    {
      if (GUIUtility.hotControl == this.id)
        return Mathf.Max(0.0f, PreviewResizer.s_CachedPreviewSizeWhileDragging);
      return Mathf.Max(0.0f, this.m_CachedPref);
    }

    public bool GetExpandedBeforeDragging()
    {
      return (double) this.m_CachedPref > 0.0;
    }

    public void SetExpanded(bool expanded)
    {
      this.m_CachedPref = Mathf.Abs(this.m_CachedPref) * (!expanded ? -1f : 1f);
      EditorPrefs.SetFloat(this.m_PrefName, this.m_CachedPref);
    }

    public void ToggleExpanded()
    {
      this.m_CachedPref = -this.m_CachedPref;
      EditorPrefs.SetFloat(this.m_PrefName, this.m_CachedPref);
    }

    public static float PixelPreciseCollapsibleSlider(int id, Rect position, float value, float min, float max, ref bool expanded)
    {
      Event current = Event.current;
      switch (current.GetTypeForControl(id))
      {
        case EventType.MouseDown:
          if (GUIUtility.hotControl == 0 && current.button == 0 && position.Contains(current.mousePosition))
          {
            GUIUtility.hotControl = id;
            PreviewResizer.s_MouseDownLocation = current.mousePosition.y;
            PreviewResizer.s_MouseDownValue = value;
            PreviewResizer.s_MouseDragged = false;
            current.Use();
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == id)
          {
            GUIUtility.hotControl = 0;
            if (!PreviewResizer.s_MouseDragged)
              expanded = !expanded;
            current.Use();
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == id)
          {
            value = Mathf.Clamp(current.mousePosition.y - PreviewResizer.s_MouseDownLocation + PreviewResizer.s_MouseDownValue, min, max - 1f);
            GUI.changed = true;
            PreviewResizer.s_MouseDragged = true;
            current.Use();
            break;
          }
          break;
        case EventType.Repaint:
          if (GUIUtility.hotControl == 0)
            EditorGUIUtility.AddCursorRect(position, MouseCursor.SplitResizeUpDown);
          if (GUIUtility.hotControl == id)
          {
            EditorGUIUtility.AddCursorRect(new Rect(position.x, position.y - 100f, position.width, position.height + 200f), MouseCursor.SplitResizeUpDown);
            break;
          }
          break;
      }
      return value;
    }
  }
}
