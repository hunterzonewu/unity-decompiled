// Decompiled with JetBrains decompiler
// Type: UnityEditor.FallbackEditorWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class FallbackEditorWindow : EditorWindow
  {
    private FallbackEditorWindow()
    {
    }

    private void OnEnable()
    {
      this.titleContent = new GUIContent("Failed to load");
    }

    private void OnGUI()
    {
      GUILayout.BeginVertical();
      GUILayout.FlexibleSpace();
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      GUILayout.Label("EditorWindow could not be loaded because the script is not found in the project", (GUIStyle) "WordWrapLabel", new GUILayoutOption[0]);
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
      GUILayout.FlexibleSpace();
      GUILayout.EndVertical();
    }
  }
}
