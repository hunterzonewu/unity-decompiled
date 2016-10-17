// Decompiled with JetBrains decompiler
// Type: UnityEditor.ObjectPreviewPopup
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class ObjectPreviewPopup : PopupWindowContent
  {
    private const float kToolbarHeight = 17f;
    private readonly Editor m_Editor;
    private readonly GUIContent m_ObjectName;
    private ObjectPreviewPopup.Styles s_Styles;

    public ObjectPreviewPopup(Object previewObject)
    {
      if (previewObject == (Object) null)
      {
        Debug.LogError((object) "ObjectPreviewPopup: Check object is not null, before trying to show it!");
      }
      else
      {
        this.m_ObjectName = new GUIContent(previewObject.name, AssetDatabase.GetAssetPath(previewObject));
        this.m_Editor = Editor.CreateEditor(previewObject);
      }
    }

    public override void OnClose()
    {
      if (!((Object) this.m_Editor != (Object) null))
        return;
      Object.DestroyImmediate((Object) this.m_Editor);
    }

    public override void OnGUI(Rect rect)
    {
      if ((Object) this.m_Editor == (Object) null)
      {
        this.editorWindow.Close();
      }
      else
      {
        if (this.s_Styles == null)
          this.s_Styles = new ObjectPreviewPopup.Styles();
        GUILayout.BeginArea(new Rect(rect.x, rect.y, rect.width, 17f), this.s_Styles.toolbar);
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        this.m_Editor.OnPreviewSettings();
        EditorGUILayout.EndHorizontal();
        GUILayout.EndArea();
        GUI.Label(new Rect(rect.x + 5f, rect.y, rect.width - 140f, 17f), this.m_ObjectName, this.s_Styles.toolbarText);
        this.m_Editor.OnPreviewGUI(new Rect(rect.x, rect.y + 17f, rect.width, rect.height - 17f), this.s_Styles.background);
      }
    }

    public override Vector2 GetWindowSize()
    {
      return new Vector2(300f, 317f);
    }

    internal class Styles
    {
      public readonly GUIStyle toolbar = (GUIStyle) "preToolbar";
      public readonly GUIStyle toolbarText = (GUIStyle) "preToolbar2";
      public GUIStyle background = (GUIStyle) "preBackground";
    }
  }
}
