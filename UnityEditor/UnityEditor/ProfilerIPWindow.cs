// Decompiled with JetBrains decompiler
// Type: UnityEditor.ProfilerIPWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class ProfilerIPWindow : EditorWindow
  {
    internal string m_IPString = ProfilerIPWindow.GetLastIPString();
    private const string kTextFieldId = "IPWindow";
    private const string kLastIP = "ProfilerLastIP";
    internal bool didFocus;

    public static void Show(Rect buttonScreenRect)
    {
      Rect rect = new Rect(buttonScreenRect.x, buttonScreenRect.yMax, 300f, 50f);
      ProfilerIPWindow windowWithRect = EditorWindow.GetWindowWithRect<ProfilerIPWindow>(rect, true, "Enter Player IP");
      windowWithRect.position = rect;
      windowWithRect.m_Parent.window.m_DontSaveToLayout = true;
    }

    public static string GetLastIPString()
    {
      return EditorPrefs.GetString("ProfilerLastIP", string.Empty);
    }

    private void OnGUI()
    {
      Event current = Event.current;
      bool flag = current.type == EventType.KeyDown && (current.keyCode == KeyCode.Return || current.keyCode == KeyCode.KeypadEnter);
      GUI.SetNextControlName("IPWindow");
      EditorGUILayout.BeginVertical();
      GUILayout.Space(5f);
      this.m_IPString = EditorGUILayout.TextField(this.m_IPString);
      if (!this.didFocus)
      {
        this.didFocus = true;
        EditorGUI.FocusTextInControl("IPWindow");
      }
      GUI.enabled = this.m_IPString.Length != 0;
      if (GUILayout.Button("Connect") || flag)
      {
        this.Close();
        EditorPrefs.SetString("ProfilerLastIP", this.m_IPString);
        AttachProfilerUI.DirectIPConnect(this.m_IPString);
        GUIUtility.ExitGUI();
      }
      EditorGUILayout.EndVertical();
    }
  }
}
