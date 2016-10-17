// Decompiled with JetBrains decompiler
// Type: UnityEditor.ScriptReloadProperties
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal sealed class ScriptReloadProperties : ScriptableObject
  {
    public bool EditorGUI_IsActuallEditing;
    public int EditorGUI_TextEditor_cursorIndex;
    public int EditorGUI_TextEditor_selectIndex;
    public int EditorGUI_TextEditor_controlID;
    public bool EditorGUI_TextEditor_hasHorizontalCursorPos;
    public Vector2 EditorGUI_TextEditor_scrollOffset;
    public bool EditorGUI_TextEditor_hasFocus;
    public Vector2 EditorGUI_TextEditor_graphicalCursorPos;
    public string EditorGUI_TextEditor_content;
    public string EditorGUI_Current_Editing_String;
    public int EditorGUI_DelayedTextEditor_cursorIndex;
    public int EditorGUI_DelayedTextEditor_selectIndex;
    public int EditorGUI_DelayedTextEditor_controlID;
    public bool EditorGUI_DelayedTextEditor_hasHorizontalCursorPos;
    public Vector2 EditorGUI_DelayedTextEditor_scrollOffset;
    public bool EditorGUI_DelayedTextEditor_hasFocus;
    public Vector2 EditorGUI_DelayedTextEditor_graphicalCursorPos;
    public string EditorGUI_DelayedTextEditor_content;
    public string EditorGUI_DelayedControlThatHadFocusValue;

    private static ScriptReloadProperties Store()
    {
      ScriptReloadProperties instance = ScriptableObject.CreateInstance<ScriptReloadProperties>();
      instance.hideFlags = HideFlags.HideAndDontSave;
      instance.ManagedStore();
      return instance;
    }

    private static void Load(ScriptReloadProperties properties)
    {
      properties.ManagedLoad();
    }

    private void ManagedStore()
    {
      this.EditorGUI_IsActuallEditing = EditorGUI.RecycledTextEditor.s_ActuallyEditing;
      this.EditorGUI_TextEditor_cursorIndex = EditorGUI.s_RecycledEditor.cursorIndex;
      this.EditorGUI_TextEditor_selectIndex = EditorGUI.s_RecycledEditor.selectIndex;
      this.EditorGUI_TextEditor_controlID = EditorGUI.s_RecycledEditor.controlID;
      this.EditorGUI_TextEditor_hasHorizontalCursorPos = EditorGUI.s_RecycledEditor.hasHorizontalCursorPos;
      this.EditorGUI_TextEditor_scrollOffset = EditorGUI.s_RecycledEditor.scrollOffset;
      this.EditorGUI_TextEditor_hasFocus = EditorGUI.s_RecycledEditor.m_HasFocus;
      this.EditorGUI_TextEditor_graphicalCursorPos = EditorGUI.s_RecycledEditor.graphicalCursorPos;
      this.EditorGUI_TextEditor_content = EditorGUI.s_RecycledEditor.text;
      this.EditorGUI_Current_Editing_String = EditorGUI.s_RecycledCurrentEditingString;
      this.EditorGUI_DelayedTextEditor_cursorIndex = EditorGUI.s_DelayedTextEditor.cursorIndex;
      this.EditorGUI_DelayedTextEditor_selectIndex = EditorGUI.s_DelayedTextEditor.selectIndex;
      this.EditorGUI_DelayedTextEditor_controlID = EditorGUI.s_DelayedTextEditor.controlID;
      this.EditorGUI_DelayedTextEditor_hasHorizontalCursorPos = EditorGUI.s_DelayedTextEditor.hasHorizontalCursorPos;
      this.EditorGUI_DelayedTextEditor_scrollOffset = EditorGUI.s_DelayedTextEditor.scrollOffset;
      this.EditorGUI_DelayedTextEditor_hasFocus = EditorGUI.s_DelayedTextEditor.m_HasFocus;
      this.EditorGUI_DelayedTextEditor_graphicalCursorPos = EditorGUI.s_DelayedTextEditor.graphicalCursorPos;
      this.EditorGUI_DelayedTextEditor_content = EditorGUI.s_DelayedTextEditor.text;
      this.EditorGUI_DelayedControlThatHadFocusValue = EditorGUI.s_DelayedTextEditor.controlThatHadFocusValue;
    }

    private void ManagedLoad()
    {
      EditorGUI.RecycledTextEditor.s_ActuallyEditing = this.EditorGUI_IsActuallEditing;
      EditorGUI.s_RecycledEditor.cursorIndex = this.EditorGUI_TextEditor_cursorIndex;
      EditorGUI.s_RecycledEditor.selectIndex = this.EditorGUI_TextEditor_selectIndex;
      EditorGUI.s_RecycledEditor.controlID = this.EditorGUI_TextEditor_controlID;
      EditorGUI.s_RecycledEditor.hasHorizontalCursorPos = this.EditorGUI_TextEditor_hasHorizontalCursorPos;
      EditorGUI.s_RecycledEditor.scrollOffset = this.EditorGUI_TextEditor_scrollOffset;
      EditorGUI.s_RecycledEditor.m_HasFocus = this.EditorGUI_TextEditor_hasFocus;
      EditorGUI.s_RecycledEditor.graphicalCursorPos = this.EditorGUI_TextEditor_graphicalCursorPos;
      EditorGUI.s_RecycledEditor.text = this.EditorGUI_TextEditor_content;
      EditorGUI.s_RecycledCurrentEditingString = this.EditorGUI_Current_Editing_String;
      EditorGUI.s_DelayedTextEditor.cursorIndex = this.EditorGUI_DelayedTextEditor_cursorIndex;
      EditorGUI.s_DelayedTextEditor.selectIndex = this.EditorGUI_DelayedTextEditor_selectIndex;
      EditorGUI.s_DelayedTextEditor.controlID = this.EditorGUI_DelayedTextEditor_controlID;
      EditorGUI.s_DelayedTextEditor.hasHorizontalCursorPos = this.EditorGUI_DelayedTextEditor_hasHorizontalCursorPos;
      EditorGUI.s_DelayedTextEditor.scrollOffset = this.EditorGUI_DelayedTextEditor_scrollOffset;
      EditorGUI.s_DelayedTextEditor.m_HasFocus = this.EditorGUI_DelayedTextEditor_hasFocus;
      EditorGUI.s_DelayedTextEditor.graphicalCursorPos = this.EditorGUI_DelayedTextEditor_graphicalCursorPos;
      EditorGUI.s_DelayedTextEditor.text = this.EditorGUI_DelayedTextEditor_content;
      EditorGUI.s_DelayedTextEditor.controlThatHadFocusValue = this.EditorGUI_DelayedControlThatHadFocusValue;
    }
  }
}
