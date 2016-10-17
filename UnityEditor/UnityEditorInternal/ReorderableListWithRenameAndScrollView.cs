// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.ReorderableListWithRenameAndScrollView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class ReorderableListWithRenameAndScrollView
  {
    private int m_LastSelectedIndex = -1;
    private int m_FrameIndex = -1;
    private ReorderableList m_ReorderableList;
    private ReorderableListWithRenameAndScrollView.State m_State;
    private bool m_HadKeyFocusAtMouseDown;
    public GUIStyle listElementStyle;
    public GUIStyle renameOverlayStyle;
    public Func<int, string> onGetNameAtIndex;
    public System.Action<int, string> onNameChangedAtIndex;
    public System.Action<int> onSelectionChanged;
    public System.Action<int> onDeleteItemAtIndex;
    public ReorderableList.ElementCallbackDelegate onCustomDrawElement;
    private static ReorderableListWithRenameAndScrollView.Styles s_Styles;

    public ReorderableList list
    {
      get
      {
        return this.m_ReorderableList;
      }
    }

    public GUIStyle elementStyle
    {
      get
      {
        return this.listElementStyle ?? ReorderableListWithRenameAndScrollView.s_Styles.reorderableListLabel;
      }
    }

    public GUIStyle elementStyleRightAligned
    {
      get
      {
        return ReorderableListWithRenameAndScrollView.s_Styles.reorderableListLabelRightAligned;
      }
    }

    public ReorderableListWithRenameAndScrollView(ReorderableList list, ReorderableListWithRenameAndScrollView.State state)
    {
      this.m_State = state;
      this.m_ReorderableList = list;
      this.m_ReorderableList.drawElementCallback += new ReorderableList.ElementCallbackDelegate(this.DrawElement);
      this.m_ReorderableList.onSelectCallback += new ReorderableList.SelectCallbackDelegate(this.SelectCallback);
      this.m_ReorderableList.onMouseUpCallback += new ReorderableList.SelectCallbackDelegate(this.MouseUpCallback);
      this.m_ReorderableList.onReorderCallback += new ReorderableList.ReorderCallbackDelegate(this.ReorderCallback);
    }

    private RenameOverlay GetRenameOverlay()
    {
      return this.m_State.m_RenameOverlay;
    }

    public void OnEvent()
    {
      this.GetRenameOverlay().OnEvent();
    }

    private void EnsureRowIsVisible(int index, float scrollGUIHeight)
    {
      if (index < 0)
        return;
      float max = (float) ((double) this.m_ReorderableList.elementHeight * (double) index + 2.0);
      this.m_State.m_ScrollPos.y = Mathf.Clamp(this.m_State.m_ScrollPos.y, (float) ((double) max - (double) scrollGUIHeight + (double) this.m_ReorderableList.elementHeight + 3.0), max);
    }

    public void OnGUI(Rect rect)
    {
      if (ReorderableListWithRenameAndScrollView.s_Styles == null)
        ReorderableListWithRenameAndScrollView.s_Styles = new ReorderableListWithRenameAndScrollView.Styles();
      if (this.onGetNameAtIndex == null)
        Debug.LogError((object) "Ensure to set: 'onGetNameAtIndex'");
      Event current = Event.current;
      if (current.type == EventType.MouseDown && rect.Contains(current.mousePosition))
        this.m_HadKeyFocusAtMouseDown = this.m_ReorderableList.HasKeyboardControl();
      if (this.m_FrameIndex != -1)
      {
        this.EnsureRowIsVisible(this.m_FrameIndex, rect.height);
        this.m_FrameIndex = -1;
      }
      GUILayout.BeginArea(rect);
      this.m_State.m_ScrollPos = GUILayout.BeginScrollView(this.m_State.m_ScrollPos);
      this.m_ReorderableList.DoLayoutList();
      GUILayout.EndScrollView();
      GUILayout.EndArea();
      AudioMixerDrawUtils.DrawScrollDropShadow(rect, this.m_State.m_ScrollPos.y, this.m_ReorderableList.GetHeight());
      this.KeyboardHandling();
      this.CommandHandling();
    }

    public bool IsRenamingIndex(int index)
    {
      if (this.GetRenameOverlay().IsRenaming() && this.GetRenameOverlay().userData == index)
        return !this.GetRenameOverlay().isWaitingForDelay;
      return false;
    }

    public void DrawElement(Rect r, int index, bool isActive, bool isFocused)
    {
      if (this.IsRenamingIndex(index))
      {
        if ((double) r.width >= 0.0 && (double) r.height >= 0.0)
        {
          r.x -= 2f;
          this.GetRenameOverlay().editFieldRect = r;
        }
        this.DoRenameOverlay();
      }
      else if (this.onCustomDrawElement != null)
        this.onCustomDrawElement(r, index, isActive, isFocused);
      else
        this.DrawElementText(r, index, isActive, index == this.m_ReorderableList.index, isFocused);
    }

    public void DrawElementText(Rect r, int index, bool isActive, bool isSelected, bool isFocused)
    {
      if (Event.current.type != EventType.Repaint || this.onGetNameAtIndex == null)
        return;
      this.elementStyle.Draw(r, this.onGetNameAtIndex(index), false, false, isSelected, true);
    }

    public virtual void DoRenameOverlay()
    {
      if (!this.GetRenameOverlay().IsRenaming() || this.GetRenameOverlay().OnGUI())
        return;
      this.RenameEnded();
    }

    public void BeginRename(int index, float delay)
    {
      this.GetRenameOverlay().BeginRename(this.onGetNameAtIndex(index), index, delay);
      this.m_ReorderableList.index = index;
      this.m_LastSelectedIndex = index;
      this.FrameItem(index);
    }

    private void RenameEnded()
    {
      if (this.GetRenameOverlay().userAcceptedRename && this.onNameChangedAtIndex != null)
        this.onNameChangedAtIndex(this.GetRenameOverlay().userData, !string.IsNullOrEmpty(this.GetRenameOverlay().name) ? this.GetRenameOverlay().name : this.GetRenameOverlay().originalName);
      if (this.GetRenameOverlay().HasKeyboardFocus())
        this.m_ReorderableList.GrabKeyboardFocus();
      this.GetRenameOverlay().Clear();
    }

    public void EndRename(bool acceptChanges)
    {
      if (!this.GetRenameOverlay().IsRenaming())
        return;
      this.GetRenameOverlay().EndRename(acceptChanges);
      this.RenameEnded();
    }

    public void ReorderCallback(ReorderableList list)
    {
      this.m_LastSelectedIndex = list.index;
    }

    public void MouseUpCallback(ReorderableList list)
    {
      if (this.m_HadKeyFocusAtMouseDown && list.index == this.m_LastSelectedIndex)
        this.BeginRename(list.index, 0.5f);
      this.m_LastSelectedIndex = list.index;
    }

    public void SelectCallback(ReorderableList list)
    {
      this.FrameItem(list.index);
      if (this.onSelectionChanged == null)
        return;
      this.onSelectionChanged(list.index);
    }

    private void RemoveSelected()
    {
      if (this.m_ReorderableList.index < 0 || this.m_ReorderableList.index >= this.m_ReorderableList.count)
      {
        Debug.Log((object) ("Invalid index to remove " + (object) this.m_ReorderableList.index));
      }
      else
      {
        if (this.onDeleteItemAtIndex == null)
          return;
        this.onDeleteItemAtIndex(this.m_ReorderableList.index);
      }
    }

    public void FrameItem(int index)
    {
      this.m_FrameIndex = index;
    }

    private bool CanBeginRename()
    {
      if (!this.GetRenameOverlay().IsRenaming())
        return this.m_ReorderableList.index >= 0;
      return false;
    }

    private void CommandHandling()
    {
      Event current = Event.current;
      if (Event.current.type != EventType.ExecuteCommand)
        return;
      string commandName = current.commandName;
      if (commandName == null)
        return;
      // ISSUE: reference to a compiler-generated field
      if (ReorderableListWithRenameAndScrollView.\u003C\u003Ef__switch\u0024map15 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ReorderableListWithRenameAndScrollView.\u003C\u003Ef__switch\u0024map15 = new Dictionary<string, int>(1)
        {
          {
            "OnLostFocus",
            0
          }
        };
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      if (!ReorderableListWithRenameAndScrollView.\u003C\u003Ef__switch\u0024map15.TryGetValue(commandName, out num) || num != 0)
        return;
      this.EndRename(true);
      current.Use();
    }

    private void KeyboardHandling()
    {
      Event current = Event.current;
      if (current.type != EventType.KeyDown || !this.m_ReorderableList.HasKeyboardControl())
        return;
      KeyCode keyCode = Event.current.keyCode;
      switch (keyCode)
      {
        case KeyCode.Home:
          current.Use();
          this.m_ReorderableList.index = 0;
          this.FrameItem(this.m_ReorderableList.index);
          break;
        case KeyCode.End:
          current.Use();
          this.m_ReorderableList.index = this.m_ReorderableList.count - 1;
          this.FrameItem(this.m_ReorderableList.index);
          break;
        case KeyCode.F2:
          if (!this.CanBeginRename() || Application.platform != RuntimePlatform.WindowsEditor)
            break;
          this.BeginRename(this.m_ReorderableList.index, 0.0f);
          current.Use();
          break;
        default:
          if (keyCode != KeyCode.Return)
          {
            if (keyCode != KeyCode.Delete)
            {
              if (keyCode != KeyCode.KeypadEnter)
                break;
            }
            else
            {
              this.RemoveSelected();
              current.Use();
              break;
            }
          }
          if (!this.CanBeginRename() || Application.platform != RuntimePlatform.OSXEditor)
            break;
          this.BeginRename(this.m_ReorderableList.index, 0.0f);
          current.Use();
          break;
      }
    }

    [Serializable]
    public class State
    {
      public Vector2 m_ScrollPos = new Vector2(0.0f, 0.0f);
      public RenameOverlay m_RenameOverlay = new RenameOverlay();
    }

    public class Styles
    {
      public GUIStyle reorderableListLabel = new GUIStyle((GUIStyle) "PR Label");
      public GUIStyle reorderableListLabelRightAligned;

      public Styles()
      {
        Texture2D background = this.reorderableListLabel.hover.background;
        this.reorderableListLabel.normal.background = background;
        this.reorderableListLabel.active.background = background;
        this.reorderableListLabel.focused.background = background;
        this.reorderableListLabel.onNormal.background = background;
        this.reorderableListLabel.onHover.background = background;
        this.reorderableListLabel.onActive.background = background;
        this.reorderableListLabel.onFocused.background = background;
        RectOffset padding = this.reorderableListLabel.padding;
        int num1 = 0;
        this.reorderableListLabel.padding.right = num1;
        int num2 = num1;
        padding.left = num2;
        this.reorderableListLabel.alignment = TextAnchor.MiddleLeft;
        this.reorderableListLabelRightAligned = new GUIStyle(this.reorderableListLabel);
        this.reorderableListLabelRightAligned.alignment = TextAnchor.MiddleRight;
        this.reorderableListLabelRightAligned.clipping = TextClipping.Overflow;
      }
    }
  }
}
