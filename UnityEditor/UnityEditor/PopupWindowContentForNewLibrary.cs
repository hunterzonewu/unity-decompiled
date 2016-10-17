// Decompiled with JetBrains decompiler
// Type: UnityEditor.PopupWindowContentForNewLibrary
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal class PopupWindowContentForNewLibrary : PopupWindowContent
  {
    private string m_NewLibraryName = string.Empty;
    private int m_SelectedIndexInPopup;
    private string m_ErrorString;
    private Rect m_WantedSize;
    private Func<string, PresetFileLocation, string> m_CreateLibraryCallback;
    private static PopupWindowContentForNewLibrary.Texts s_Texts;

    public PopupWindowContentForNewLibrary(Func<string, PresetFileLocation, string> createLibraryCallback)
    {
      this.m_CreateLibraryCallback = createLibraryCallback;
    }

    public override void OnGUI(Rect rect)
    {
      if (PopupWindowContentForNewLibrary.s_Texts == null)
        PopupWindowContentForNewLibrary.s_Texts = new PopupWindowContentForNewLibrary.Texts();
      this.KeyboardHandling(this.editorWindow);
      float width = 80f;
      Rect rect1 = EditorGUILayout.BeginVertical();
      if (Event.current.type != EventType.Layout)
        this.m_WantedSize = rect1;
      GUILayout.BeginHorizontal();
      GUILayout.Label(PopupWindowContentForNewLibrary.s_Texts.header, EditorStyles.boldLabel, new GUILayoutOption[0]);
      GUILayout.EndHorizontal();
      EditorGUI.BeginChangeCheck();
      GUILayout.BeginHorizontal();
      GUILayout.Label(PopupWindowContentForNewLibrary.s_Texts.name, new GUILayoutOption[1]
      {
        GUILayout.Width(width)
      });
      EditorGUI.FocusTextInControl("NewLibraryName");
      GUI.SetNextControlName("NewLibraryName");
      this.m_NewLibraryName = GUILayout.TextField(this.m_NewLibraryName);
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      GUILayout.Label(PopupWindowContentForNewLibrary.s_Texts.location, new GUILayoutOption[1]
      {
        GUILayout.Width(width)
      });
      this.m_SelectedIndexInPopup = EditorGUILayout.Popup(this.m_SelectedIndexInPopup, PopupWindowContentForNewLibrary.s_Texts.fileLocations);
      GUILayout.EndHorizontal();
      if (EditorGUI.EndChangeCheck())
        this.m_ErrorString = (string) null;
      GUILayout.BeginHorizontal();
      if (!string.IsNullOrEmpty(this.m_ErrorString))
      {
        Color color = GUI.color;
        GUI.color = new Color(1f, 0.8f, 0.8f);
        GUILayout.Label(GUIContent.Temp(this.m_ErrorString), EditorStyles.helpBox, new GUILayoutOption[0]);
        GUI.color = color;
      }
      GUILayout.FlexibleSpace();
      if (GUILayout.Button(GUIContent.Temp("Create")))
        this.CreateLibraryAndCloseWindow(this.editorWindow);
      GUILayout.EndHorizontal();
      GUILayout.Space(15f);
      EditorGUILayout.EndVertical();
    }

    public override Vector2 GetWindowSize()
    {
      return new Vector2(350f, (double) this.m_WantedSize.height <= 0.0 ? 90f : this.m_WantedSize.height);
    }

    private void KeyboardHandling(EditorWindow editorWindow)
    {
      Event current = Event.current;
      if (current.type != EventType.KeyDown)
        return;
      switch (current.keyCode)
      {
        case KeyCode.Return:
        case KeyCode.KeypadEnter:
          this.CreateLibraryAndCloseWindow(editorWindow);
          break;
        case KeyCode.Escape:
          editorWindow.Close();
          break;
      }
    }

    private void CreateLibraryAndCloseWindow(EditorWindow editorWindow)
    {
      this.m_ErrorString = this.m_CreateLibraryCallback(this.m_NewLibraryName, PopupWindowContentForNewLibrary.s_Texts.fileLocationOrder[this.m_SelectedIndexInPopup]);
      if (!string.IsNullOrEmpty(this.m_ErrorString))
        return;
      editorWindow.Close();
    }

    private class Texts
    {
      public GUIContent header = new GUIContent("Create New Library");
      public GUIContent name = new GUIContent("Name");
      public GUIContent location = new GUIContent("Location");
      public GUIContent[] fileLocations = new GUIContent[2]{ new GUIContent("Preferences Folder"), new GUIContent("Project Folder") };
      public PresetFileLocation[] fileLocationOrder = new PresetFileLocation[2]{ PresetFileLocation.PreferencesFolder, PresetFileLocation.ProjectFolder };
    }
  }
}
