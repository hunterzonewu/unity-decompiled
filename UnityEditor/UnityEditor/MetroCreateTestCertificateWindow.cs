// Decompiled with JetBrains decompiler
// Type: UnityEditor.MetroCreateTestCertificateWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.IO;
using UnityEngine;

namespace UnityEditor
{
  internal class MetroCreateTestCertificateWindow : EditorWindow
  {
    private static readonly GUILayoutOption kLabelWidth = GUILayout.Width(110f);
    private static readonly GUILayoutOption kButtonWidth = GUILayout.Width(110f);
    private const float kSpace = 5f;
    private const char kPasswordChar = '●';
    private const string kPublisherId = "publisher";
    private const string kPasswordId = "password";
    private const string kConfirmId = "confirm";
    private string path;
    private string publisher;
    private string password;
    private string confirm;
    private GUIContent message;
    private GUIStyle messageStyle;
    private string focus;

    public static void Show(string publisher)
    {
      MetroCreateTestCertificateWindow[] objectsOfTypeAll = (MetroCreateTestCertificateWindow[]) Resources.FindObjectsOfTypeAll(typeof (MetroCreateTestCertificateWindow));
      MetroCreateTestCertificateWindow certificateWindow = objectsOfTypeAll.Length <= 0 ? ScriptableObject.CreateInstance<MetroCreateTestCertificateWindow>() : objectsOfTypeAll[0];
      certificateWindow.path = Path.Combine(Application.dataPath, "WSATestCertificate.pfx").Replace('\\', '/');
      certificateWindow.publisher = publisher;
      certificateWindow.password = string.Empty;
      certificateWindow.confirm = certificateWindow.password;
      certificateWindow.message = !File.Exists(certificateWindow.path) ? GUIContent.none : EditorGUIUtility.TextContent("Current file will be overwritten.");
      certificateWindow.messageStyle = new GUIStyle(GUI.skin.label);
      certificateWindow.messageStyle.fontStyle = FontStyle.Italic;
      certificateWindow.focus = "publisher";
      if (objectsOfTypeAll.Length > 0)
      {
        certificateWindow.Focus();
      }
      else
      {
        certificateWindow.titleContent = EditorGUIUtility.TextContent("Create Test Certificate for Windows Store");
        certificateWindow.position = new Rect(100f, 100f, 350f, 140f);
        certificateWindow.minSize = new Vector2(certificateWindow.position.width, certificateWindow.position.height);
        certificateWindow.maxSize = certificateWindow.minSize;
        certificateWindow.ShowUtility();
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
            GUILayout.Label(EditorGUIUtility.TextContent("Publisher|Publisher of the package."), new GUILayoutOption[1]
            {
              MetroCreateTestCertificateWindow.kLabelWidth
            });
            GUI.SetNextControlName("publisher");
            this.publisher = GUILayout.TextField(this.publisher);
          }
          GUILayout.Space(5f);
          using (HorizontalLayout.DoLayout())
          {
            GUILayout.Label(EditorGUIUtility.TextContent("Password|Certificate password."), new GUILayoutOption[1]
            {
              MetroCreateTestCertificateWindow.kLabelWidth
            });
            GUI.SetNextControlName("password");
            this.password = GUILayout.PasswordField(this.password, '●');
          }
          GUILayout.Space(5f);
          using (HorizontalLayout.DoLayout())
          {
            GUILayout.Label(EditorGUIUtility.TextContent("Confirm password|Re-enter certificate password."), new GUILayoutOption[1]
            {
              MetroCreateTestCertificateWindow.kLabelWidth
            });
            GUI.SetNextControlName("confirm");
            this.confirm = GUILayout.PasswordField(this.confirm, '●');
          }
          GUILayout.Space(10f);
          using (HorizontalLayout.DoLayout())
          {
            GUILayout.Label(this.message, this.messageStyle, new GUILayoutOption[0]);
            GUILayout.FlexibleSpace();
            if (!GUILayout.Button(EditorGUIUtility.TextContent("Create"), new GUILayoutOption[1]{ MetroCreateTestCertificateWindow.kButtonWidth }))
            {
              if (!flag2)
                goto label_36;
            }
            this.message = GUIContent.none;
            if (string.IsNullOrEmpty(this.publisher))
            {
              this.message = EditorGUIUtility.TextContent("Publisher must be specified.");
              this.focus = "publisher";
            }
            else if (this.password != this.confirm)
            {
              if (string.IsNullOrEmpty(this.confirm))
              {
                this.message = EditorGUIUtility.TextContent("Confirm the password.");
                this.focus = "confirm";
              }
              else
              {
                this.message = EditorGUIUtility.TextContent("Passwords do not match.");
                this.password = string.Empty;
                this.confirm = this.password;
                this.focus = "password";
              }
            }
            else
            {
              try
              {
                EditorUtility.WSACreateTestCertificate(this.path, this.publisher, this.password, true);
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                if (!PlayerSettings.WSA.SetCertificate(FileUtil.GetProjectRelativePath(this.path), this.password))
                  this.message = EditorGUIUtility.TextContent("Invalid password.");
                flag1 = true;
              }
              catch (UnityException ex)
              {
                Debug.LogError((object) ex.Message);
              }
            }
          }
label_36:
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
