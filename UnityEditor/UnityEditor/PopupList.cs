// Decompiled with JetBrains decompiler
// Type: UnityEditor.PopupList
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  internal class PopupList : PopupWindowContent
  {
    private static EditorGUI.RecycledTextEditor s_RecycledEditor = new EditorGUI.RecycledTextEditor();
    private static string s_TextFieldName = "ProjectBrowserPopupsTextField";
    private static int s_TextFieldHash = PopupList.s_TextFieldName.GetHashCode();
    private string m_EnteredTextCompletion = string.Empty;
    private string m_EnteredText = string.Empty;
    private const float scrollBarWidth = 14f;
    private const float listElementHeight = 18f;
    private const float gizmoRightAlign = 23f;
    private const float iconRightAlign = 64f;
    private const float frameWidth = 1f;
    private const float k_LineHeight = 16f;
    private const float k_TextFieldHeight = 16f;
    private const float k_Margin = 10f;
    private static PopupList.Styles s_Styles;
    private PopupList.InputData m_Data;
    private Vector2 m_ScrollPosition;
    private Vector2 m_ScreenPos;
    private PopupList.Gravity m_Gravity;
    private int m_SelectedCompletionIndex;

    public PopupList(PopupList.InputData inputData)
      : this(inputData, (string) null)
    {
    }

    public PopupList(PopupList.InputData inputData, string initialSelectionLabel)
    {
      this.m_Data = inputData;
      this.m_Data.ResetScores();
      this.SelectNoCompletion();
      this.m_Gravity = PopupList.Gravity.Top;
      if (initialSelectionLabel == null)
        return;
      this.m_EnteredTextCompletion = initialSelectionLabel;
      this.UpdateCompletion();
    }

    public override void OnClose()
    {
      if (this.m_Data == null)
        return;
      this.m_Data.ResetScores();
    }

    public virtual float GetWindowHeight()
    {
      return (float) ((this.m_Data.m_MaxCount != 0 ? (double) this.m_Data.m_MaxCount : (double) this.m_Data.GetFilteredCount(this.m_EnteredText)) * 16.0 + 20.0 + (!this.m_Data.m_AllowCustom ? 0.0 : 16.0));
    }

    public virtual float GetWindowWidth()
    {
      return 150f;
    }

    public override Vector2 GetWindowSize()
    {
      return new Vector2(this.GetWindowWidth(), this.GetWindowHeight());
    }

    public override void OnGUI(Rect windowRect)
    {
      Event current = Event.current;
      if (current.type == EventType.Layout)
        return;
      if (PopupList.s_Styles == null)
        PopupList.s_Styles = new PopupList.Styles();
      if (current.type == EventType.KeyDown && current.keyCode == KeyCode.Escape)
      {
        this.editorWindow.Close();
        GUIUtility.ExitGUI();
      }
      if (this.m_Gravity == PopupList.Gravity.Bottom)
      {
        this.DrawList(this.editorWindow, windowRect);
        this.DrawCustomTextField(this.editorWindow, windowRect);
      }
      else
      {
        this.DrawCustomTextField(this.editorWindow, windowRect);
        this.DrawList(this.editorWindow, windowRect);
      }
      if (current.type != EventType.Repaint)
        return;
      PopupList.s_Styles.background.Draw(new Rect(windowRect.x, windowRect.y, windowRect.width, windowRect.height), false, false, false, false);
    }

    private void DrawCustomTextField(EditorWindow editorWindow, Rect windowRect)
    {
      if (!this.m_Data.m_AllowCustom)
        return;
      Event current = Event.current;
      bool flag1 = this.m_Data.m_EnableAutoCompletion;
      bool flag2 = false;
      bool flag3 = false;
      bool flag4 = false;
      string str1 = this.CurrentDisplayedText();
      if (current.type == EventType.KeyDown)
      {
        KeyCode keyCode = current.keyCode;
        switch (keyCode)
        {
          case KeyCode.Backspace:
            flag1 = false;
            break;
          case KeyCode.Tab:
          case KeyCode.Return:
            if (str1 != string.Empty)
            {
              if (this.m_Data.m_OnSelectCallback != null)
                this.m_Data.m_OnSelectCallback(this.m_Data.NewOrMatchingElement(str1));
              if (current.keyCode == KeyCode.Tab || current.keyCode == KeyCode.Comma)
                flag4 = true;
              if (this.m_Data.m_CloseOnSelection || current.keyCode == KeyCode.Return)
                flag2 = true;
            }
            flag3 = true;
            break;
          default:
            if (keyCode != KeyCode.UpArrow)
            {
              if (keyCode != KeyCode.DownArrow)
              {
                if (keyCode != KeyCode.None)
                {
                  if (keyCode != KeyCode.Space && keyCode != KeyCode.Comma)
                  {
                    if (keyCode == KeyCode.Delete)
                      goto case KeyCode.Backspace;
                    else
                      break;
                  }
                  else
                    goto case KeyCode.Tab;
                }
                else
                {
                  if ((int) current.character == 32 || (int) current.character == 44)
                  {
                    flag3 = true;
                    break;
                  }
                  break;
                }
              }
              else
              {
                this.ChangeSelectedCompletion(1);
                flag3 = true;
                break;
              }
            }
            else
            {
              this.ChangeSelectedCompletion(-1);
              flag3 = true;
              break;
            }
        }
      }
      bool changed = false;
      Rect position1 = new Rect(windowRect.x + 5f, windowRect.y + (this.m_Gravity != PopupList.Gravity.Top ? (float) ((double) windowRect.height - 16.0 - 5.0) : 5f), (float) ((double) windowRect.width - 10.0 - 14.0), 16f);
      GUI.SetNextControlName(PopupList.s_TextFieldName);
      EditorGUI.FocusTextInControl(PopupList.s_TextFieldName);
      int controlId = GUIUtility.GetControlID(PopupList.s_TextFieldHash, FocusType.Keyboard, position1);
      if (flag3)
        current.Use();
      if (GUIUtility.keyboardControl == 0)
        GUIUtility.keyboardControl = controlId;
      string str2 = EditorGUI.DoTextField(PopupList.s_RecycledEditor, controlId, position1, str1, PopupList.s_Styles.customTextField, (string) null, out changed, false, false, false);
      Rect position2 = position1;
      position2.x += position1.width;
      position2.width = 14f;
      if (GUI.Button(position2, GUIContent.none, !(str2 != string.Empty) ? PopupList.s_Styles.customTextFieldCancelButtonEmpty : PopupList.s_Styles.customTextFieldCancelButton) && str2 != string.Empty || flag4)
      {
        string empty = string.Empty;
        PopupList.s_RecycledEditor.text = empty;
        str2 = EditorGUI.s_OriginalText = empty;
        PopupList.s_RecycledEditor.cursorIndex = 0;
        PopupList.s_RecycledEditor.selectIndex = 0;
        flag1 = false;
      }
      if (str1 != str2)
      {
        this.m_EnteredText = 0 > PopupList.s_RecycledEditor.cursorIndex || PopupList.s_RecycledEditor.cursorIndex >= str2.Length ? str2 : str2.Substring(0, PopupList.s_RecycledEditor.cursorIndex);
        if (flag1)
          this.UpdateCompletion();
        else
          this.SelectNoCompletion();
      }
      if (!flag2)
        return;
      editorWindow.Close();
    }

    private string CurrentDisplayedText()
    {
      if (this.m_EnteredTextCompletion != string.Empty)
        return this.m_EnteredTextCompletion;
      return this.m_EnteredText;
    }

    private void UpdateCompletion()
    {
      if (!this.m_Data.m_EnableAutoCompletion)
        return;
      IEnumerable<string> source = this.m_Data.GetFilteredList(this.m_EnteredText).Select<PopupList.ListElement, string>((Func<PopupList.ListElement, string>) (element => element.text));
      if (this.m_EnteredTextCompletion != string.Empty && this.m_EnteredTextCompletion.StartsWith(this.m_EnteredText, StringComparison.OrdinalIgnoreCase))
      {
        this.m_SelectedCompletionIndex = source.TakeWhile<string>((Func<string, bool>) (element => element != this.m_EnteredTextCompletion)).Count<string>();
      }
      else
      {
        if (this.m_SelectedCompletionIndex < 0)
          this.m_SelectedCompletionIndex = 0;
        else if (this.m_SelectedCompletionIndex >= source.Count<string>())
          this.m_SelectedCompletionIndex = source.Count<string>() - 1;
        this.m_EnteredTextCompletion = source.Skip<string>(this.m_SelectedCompletionIndex).DefaultIfEmpty<string>(string.Empty).FirstOrDefault<string>();
      }
      this.AdjustRecycledEditorSelectionToCompletion();
    }

    private void ChangeSelectedCompletion(int change)
    {
      int filteredCount = this.m_Data.GetFilteredCount(this.m_EnteredText);
      if (this.m_SelectedCompletionIndex == -1 && change < 0)
        this.m_SelectedCompletionIndex = filteredCount;
      this.SelectCompletionWithIndex(filteredCount <= 0 ? 0 : (this.m_SelectedCompletionIndex + change + filteredCount) % filteredCount);
    }

    private void SelectCompletionWithIndex(int index)
    {
      this.m_SelectedCompletionIndex = index;
      this.m_EnteredTextCompletion = string.Empty;
      this.UpdateCompletion();
    }

    private void SelectNoCompletion()
    {
      this.m_SelectedCompletionIndex = -1;
      this.m_EnteredTextCompletion = string.Empty;
      this.AdjustRecycledEditorSelectionToCompletion();
    }

    private void AdjustRecycledEditorSelectionToCompletion()
    {
      if (!(this.m_EnteredTextCompletion != string.Empty))
        return;
      PopupList.s_RecycledEditor.text = this.m_EnteredTextCompletion;
      EditorGUI.s_OriginalText = this.m_EnteredTextCompletion;
      PopupList.s_RecycledEditor.cursorIndex = this.m_EnteredText.Length;
      PopupList.s_RecycledEditor.selectIndex = this.m_EnteredTextCompletion.Length;
    }

    private void DrawList(EditorWindow editorWindow, Rect windowRect)
    {
      Event current = Event.current;
      int index = -1;
      foreach (PopupList.ListElement filtered in this.m_Data.GetFilteredList(this.m_EnteredText))
      {
        ++index;
        Rect position = new Rect(windowRect.x, (float) ((double) windowRect.y + 10.0 + (double) index * 16.0 + (this.m_Gravity != PopupList.Gravity.Top || !this.m_Data.m_AllowCustom ? 0.0 : 16.0)), windowRect.width, 16f);
        EventType type = current.type;
        switch (type)
        {
          case EventType.MouseDown:
            if (Event.current.button == 0 && position.Contains(Event.current.mousePosition) && filtered.enabled)
            {
              if (this.m_Data.m_OnSelectCallback != null)
                this.m_Data.m_OnSelectCallback(filtered);
              current.Use();
              if (this.m_Data.m_CloseOnSelection)
              {
                editorWindow.Close();
                continue;
              }
              continue;
            }
            continue;
          case EventType.MouseMove:
            if (position.Contains(Event.current.mousePosition))
            {
              this.SelectCompletionWithIndex(index);
              current.Use();
              continue;
            }
            continue;
          default:
            if (type == EventType.Repaint)
            {
              GUIStyle guiStyle = !filtered.partiallySelected ? PopupList.s_Styles.menuItem : PopupList.s_Styles.menuItemMixed;
              bool on = filtered.selected || filtered.partiallySelected;
              bool hasKeyboardFocus = false;
              bool isHover = index == this.m_SelectedCompletionIndex;
              bool isActive = on;
              EditorGUI.BeginDisabledGroup(!filtered.enabled);
              GUIContent content = filtered.m_Content;
              guiStyle.Draw(position, content, isHover, isActive, on, hasKeyboardFocus);
              EditorGUI.EndDisabledGroup();
              continue;
            }
            continue;
        }
      }
    }

    public enum Gravity
    {
      Top,
      Bottom,
    }

    public class ListElement
    {
      public GUIContent m_Content;
      private float m_FilterScore;
      private bool m_Selected;
      private bool m_WasSelected;
      private bool m_PartiallySelected;
      private bool m_Enabled;

      public float filterScore
      {
        get
        {
          if (this.m_WasSelected)
            return float.MaxValue;
          return this.m_FilterScore;
        }
        set
        {
          this.m_FilterScore = value;
          this.ResetScore();
        }
      }

      public bool selected
      {
        get
        {
          return this.m_Selected;
        }
        set
        {
          this.m_Selected = value;
          if (!this.m_Selected)
            return;
          this.m_WasSelected = true;
        }
      }

      public bool enabled
      {
        get
        {
          return this.m_Enabled;
        }
        set
        {
          this.m_Enabled = value;
        }
      }

      public bool partiallySelected
      {
        get
        {
          return this.m_PartiallySelected;
        }
        set
        {
          this.m_PartiallySelected = value;
          if (!this.m_PartiallySelected)
            return;
          this.m_WasSelected = true;
        }
      }

      public string text
      {
        get
        {
          return this.m_Content.text;
        }
        set
        {
          this.m_Content.text = value;
        }
      }

      public ListElement(string text, bool selected, float score)
      {
        this.m_Content = new GUIContent(text);
        if (!string.IsNullOrEmpty(this.m_Content.text))
        {
          char[] charArray = this.m_Content.text.ToCharArray();
          charArray[0] = char.ToUpper(charArray[0]);
          this.m_Content.text = new string(charArray);
        }
        this.m_Selected = selected;
        this.filterScore = score;
        this.m_PartiallySelected = false;
        this.m_Enabled = true;
      }

      public ListElement(string text, bool selected)
      {
        this.m_Content = new GUIContent(text);
        this.m_Selected = selected;
        this.filterScore = 0.0f;
        this.m_PartiallySelected = false;
        this.m_Enabled = true;
      }

      public ListElement(string text)
        : this(text, false)
      {
      }

      public void ResetScore()
      {
        this.m_WasSelected = this.m_Selected || this.m_PartiallySelected;
      }
    }

    public class InputData
    {
      public bool m_EnableAutoCompletion = true;
      public List<PopupList.ListElement> m_ListElements;
      public bool m_CloseOnSelection;
      public bool m_AllowCustom;
      public bool m_SortAlphabetically;
      public PopupList.OnSelectCallback m_OnSelectCallback;
      public int m_MaxCount;

      public InputData()
      {
        this.m_ListElements = new List<PopupList.ListElement>();
      }

      public void DeselectAll()
      {
        using (List<PopupList.ListElement>.Enumerator enumerator = this.m_ListElements.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            PopupList.ListElement current = enumerator.Current;
            current.selected = false;
            current.partiallySelected = false;
          }
        }
      }

      public void ResetScores()
      {
        using (List<PopupList.ListElement>.Enumerator enumerator = this.m_ListElements.GetEnumerator())
        {
          while (enumerator.MoveNext())
            enumerator.Current.ResetScore();
        }
      }

      public virtual IEnumerable<PopupList.ListElement> BuildQuery(string prefix)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        PopupList.InputData.\u003CBuildQuery\u003Ec__AnonStorey41 queryCAnonStorey41 = new PopupList.InputData.\u003CBuildQuery\u003Ec__AnonStorey41();
        // ISSUE: reference to a compiler-generated field
        queryCAnonStorey41.prefix = prefix;
        // ISSUE: reference to a compiler-generated field
        if (queryCAnonStorey41.prefix == string.Empty)
          return (IEnumerable<PopupList.ListElement>) this.m_ListElements;
        // ISSUE: reference to a compiler-generated method
        return this.m_ListElements.Where<PopupList.ListElement>(new Func<PopupList.ListElement, bool>(queryCAnonStorey41.\u003C\u003Em__61));
      }

      public IEnumerable<PopupList.ListElement> GetFilteredList(string prefix)
      {
        IEnumerable<PopupList.ListElement> source = this.BuildQuery(prefix);
        if (this.m_MaxCount > 0)
          source = source.OrderByDescending<PopupList.ListElement, float>((Func<PopupList.ListElement, float>) (element => element.filterScore)).Take<PopupList.ListElement>(this.m_MaxCount);
        if (this.m_SortAlphabetically)
          return (IEnumerable<PopupList.ListElement>) source.OrderBy<PopupList.ListElement, string>((Func<PopupList.ListElement, string>) (element => element.text.ToLower()));
        return source;
      }

      public int GetFilteredCount(string prefix)
      {
        IEnumerable<PopupList.ListElement> source = this.BuildQuery(prefix);
        if (this.m_MaxCount > 0)
          source = source.Take<PopupList.ListElement>(this.m_MaxCount);
        return source.Count<PopupList.ListElement>();
      }

      public PopupList.ListElement NewOrMatchingElement(string label)
      {
        using (List<PopupList.ListElement>.Enumerator enumerator = this.m_ListElements.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            PopupList.ListElement current = enumerator.Current;
            if (current.text.Equals(label, StringComparison.OrdinalIgnoreCase))
              return current;
          }
        }
        PopupList.ListElement listElement = new PopupList.ListElement(label, false, -1f);
        this.m_ListElements.Add(listElement);
        return listElement;
      }
    }

    private class Styles
    {
      public GUIStyle menuItem = (GUIStyle) "MenuItem";
      public GUIStyle menuItemMixed = (GUIStyle) "MenuItemMixed";
      public GUIStyle label = (GUIStyle) "PR Label";
      public GUIStyle background = (GUIStyle) "grey_border";
      public GUIStyle customTextField;
      public GUIStyle customTextFieldCancelButton;
      public GUIStyle customTextFieldCancelButtonEmpty;

      public Styles()
      {
        this.customTextField = new GUIStyle(EditorStyles.toolbarSearchField);
        this.customTextFieldCancelButton = new GUIStyle(EditorStyles.toolbarSearchFieldCancelButton);
        this.customTextFieldCancelButtonEmpty = new GUIStyle(EditorStyles.toolbarSearchFieldCancelButtonEmpty);
      }
    }

    public delegate void OnSelectCallback(PopupList.ListElement element);
  }
}
