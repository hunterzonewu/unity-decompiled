// Decompiled with JetBrains decompiler
// Type: UnityEditor.MetroCertificatePasswordWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class MetroCertificatePasswordWindow : EditorWindow
  {
    private static readonly GUILayoutOption kLabelWidth = GUILayout.Width(110f);
    private static readonly GUILayoutOption kButtonWidth = GUILayout.Width(110f);
    private const float kSpace = 5f;
    private const char kPasswordChar = '●';
    private const string kPasswordId = "password";
    private string path;
    private string password;
    private GUIContent message;
    private GUIStyle messageStyle;
    private string focus;

    public static void Show(string path)
    {
      MetroCertificatePasswordWindow[] objectsOfTypeAll = (MetroCertificatePasswordWindow[]) Resources.FindObjectsOfTypeAll(typeof (MetroCertificatePasswordWindow));
      MetroCertificatePasswordWindow certificatePasswordWindow = objectsOfTypeAll.Length <= 0 ? ScriptableObject.CreateInstance<MetroCertificatePasswordWindow>() : objectsOfTypeAll[0];
      certificatePasswordWindow.path = path;
      certificatePasswordWindow.password = string.Empty;
      certificatePasswordWindow.message = GUIContent.none;
      certificatePasswordWindow.messageStyle = new GUIStyle(GUI.skin.label);
      certificatePasswordWindow.messageStyle.fontStyle = FontStyle.Italic;
      certificatePasswordWindow.focus = "password";
      if (objectsOfTypeAll.Length > 0)
      {
        certificatePasswordWindow.Focus();
      }
      else
      {
        certificatePasswordWindow.titleContent = EditorGUIUtility.TextContent("Enter Windows Store Certificate Password");
        certificatePasswordWindow.position = new Rect(100f, 100f, 350f, 90f);
        certificatePasswordWindow.minSize = new Vector2(certificatePasswordWindow.position.width, certificatePasswordWindow.position.height);
        certificatePasswordWindow.maxSize = certificatePasswordWindow.minSize;
        certificatePasswordWindow.ShowUtility();
      }
    }

    public void OnGUI()
    {
      Event current = Event.current;
      bool flag1 = false;
      bool flag2 = false;
      if (current.type == EventType.KeyDown)
      {
        flag1 = current.keyCode == KeyCode.Escape;
        flag2 = current.keyCode == KeyCode.Return || current.keyCode == KeyCode.KeypadEnter;
      }
      using (HorizontalLayout.DoLayout())
      {
        GUILayout.Space(10f);
        using (VerticalLayout.DoLayout())
        {
          GUILayout.FlexibleSpace();
          using (HorizontalLayout.DoLayout())
          {
            GUILayout.Label(EditorGUIUtility.TextContent("Password|Certificate password."), new GUILayoutOption[1]
            {
              MetroCertificatePasswordWindow.kLabelWidth
            });
            GUI.SetNextControlName("password");
            this.password = GUILayout.PasswordField(this.password, '●');
          }
          GUILayout.Space(10f);
          using (HorizontalLayout.DoLayout())
          {
            GUILayout.Label(this.message, this.messageStyle, new GUILayoutOption[0]);
            GUILayout.FlexibleSpace();
            if (!GUILayout.Button(EditorGUIUtility.TextContent("Ok"), new GUILayoutOption[1]{ MetroCertificatePasswordWindow.kButtonWidth }))
            {
              if (!flag2)
                goto label_21;
            }
            this.message = GUIContent.none;
            try
            {
              if (PlayerSettings.WSA.SetCertificate(this.path, this.password))
                flag1 = true;
              else
                this.message = EditorGUIUtility.TextContent("Invalid password.");
            }
            catch (UnityException ex)
            {
              Debug.LogError((object) ex.Message);
            }
          }
label_21:
          GUILayout.FlexibleSpace();
        }
        GUILayout.Space(10f);
      }
      if (flag1)
      {
        this.Close();
      }
      else
      {
        if (this.focus == null)
          return;
        EditorGUI.FocusTextInControl(this.focus);
        this.focus = (string) null;
      }
    }
  }
}
