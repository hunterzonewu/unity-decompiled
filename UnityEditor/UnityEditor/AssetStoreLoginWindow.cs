// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetStoreLoginWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class AssetStoreLoginWindow : EditorWindow
  {
    private string m_Username = string.Empty;
    private string m_Password = string.Empty;
    private const float kBaseHeight = 110f;
    private static AssetStoreLoginWindow.Styles styles;
    private static GUIContent s_AssetStoreLogo;
    private string m_LoginReason;
    private string m_LoginRemoteMessage;
    private AssetStoreLoginWindow.LoginCallback m_LoginCallback;

    public static bool IsLoggedIn
    {
      get
      {
        return AssetStoreClient.HasActiveSessionID;
      }
    }

    public static void Login(string loginReason, AssetStoreLoginWindow.LoginCallback callback)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AssetStoreLoginWindow.\u003CLogin\u003Ec__AnonStorey59 loginCAnonStorey59 = new AssetStoreLoginWindow.\u003CLogin\u003Ec__AnonStorey59();
      // ISSUE: reference to a compiler-generated field
      loginCAnonStorey59.callback = callback;
      // ISSUE: reference to a compiler-generated field
      loginCAnonStorey59.loginReason = loginReason;
      if (AssetStoreClient.HasActiveSessionID)
        AssetStoreClient.Logout();
      if (!AssetStoreClient.RememberSession || !AssetStoreClient.HasSavedSessionID)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        AssetStoreLoginWindow.ShowAssetStoreLoginWindow(loginCAnonStorey59.loginReason, loginCAnonStorey59.callback);
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        AssetStoreClient.LoginWithRememberedSession(new AssetStoreClient.DoneLoginCallback(loginCAnonStorey59.\u003C\u003Em__99));
      }
    }

    public static void Logout()
    {
      AssetStoreClient.Logout();
    }

    public static void ShowAssetStoreLoginWindow(string loginReason, AssetStoreLoginWindow.LoginCallback callback)
    {
      AssetStoreLoginWindow windowWithRect = EditorWindow.GetWindowWithRect<AssetStoreLoginWindow>(new Rect(100f, 100f, 360f, 140f), true, "Login to Asset Store");
      windowWithRect.position = new Rect(100f, 100f, windowWithRect.position.width, windowWithRect.position.height);
      windowWithRect.m_Parent.window.m_DontSaveToLayout = true;
      windowWithRect.m_Password = string.Empty;
      windowWithRect.m_LoginCallback = callback;
      windowWithRect.m_LoginReason = loginReason;
      windowWithRect.m_LoginRemoteMessage = (string) null;
      Analytics.Track("/AssetStore/Login");
    }

    private static void LoadLogos()
    {
      if (AssetStoreLoginWindow.s_AssetStoreLogo != null)
        return;
      AssetStoreLoginWindow.s_AssetStoreLogo = new GUIContent(string.Empty);
    }

    public void OnDisable()
    {
      if (this.m_LoginCallback != null)
        this.m_LoginCallback(this.m_LoginRemoteMessage);
      this.m_LoginCallback = (AssetStoreLoginWindow.LoginCallback) null;
      this.m_Password = (string) null;
    }

    public void OnGUI()
    {
      if (AssetStoreLoginWindow.styles == null)
        AssetStoreLoginWindow.styles = new AssetStoreLoginWindow.Styles();
      AssetStoreLoginWindow.LoadLogos();
      if (AssetStoreClient.LoginInProgress() || AssetStoreClient.LoggedIn())
        GUI.enabled = false;
      GUILayout.BeginVertical();
      GUILayout.Space(10f);
      GUILayout.BeginHorizontal();
      GUILayout.Space(5f);
      GUILayout.Label(AssetStoreLoginWindow.s_AssetStoreLogo, GUIStyle.none, new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(false)
      });
      GUILayout.BeginVertical();
      GUILayout.BeginHorizontal();
      GUILayout.Space(6f);
      GUILayout.Label(this.m_LoginReason, EditorStyles.wordWrappedLabel, new GUILayoutOption[0]);
      Rect lastRect = GUILayoutUtility.GetLastRect();
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      GUILayout.Space(6f);
      Rect rect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
      if (this.m_LoginRemoteMessage != null)
      {
        Color color = GUI.color;
        GUI.color = Color.red;
        GUILayout.Label(this.m_LoginRemoteMessage, EditorStyles.wordWrappedLabel, new GUILayoutOption[0]);
        GUI.color = color;
        rect = GUILayoutUtility.GetLastRect();
      }
      float height = (float) ((double) lastRect.height + (double) rect.height + 110.0);
      if (Event.current.type == EventType.Repaint && (double) height != (double) this.position.height)
      {
        this.position = new Rect(this.position.x, this.position.y, this.position.width, height);
        this.Repaint();
      }
      GUILayout.EndHorizontal();
      GUILayout.FlexibleSpace();
      GUILayout.BeginHorizontal();
      GUI.SetNextControlName("username");
      this.m_Username = EditorGUILayout.TextField("Username", this.m_Username, new GUILayoutOption[0]);
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      this.m_Password = EditorGUILayout.PasswordField("Password", this.m_Password, new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(true)
      });
      if (GUILayout.Button(new GUIContent("Forgot?", "Reset your password"), AssetStoreLoginWindow.styles.link, new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(false)
      }))
        Application.OpenURL("https://accounts.unity3d.com/password/new");
      EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
      GUILayout.EndHorizontal();
      bool rememberSession = AssetStoreClient.RememberSession;
      bool flag = EditorGUILayout.Toggle("Remember me", rememberSession, new GUILayoutOption[0]);
      if (flag != rememberSession)
        AssetStoreClient.RememberSession = flag;
      GUILayout.EndVertical();
      GUILayout.Space(5f);
      GUILayout.EndHorizontal();
      GUILayout.Space(8f);
      GUILayout.BeginHorizontal();
      if (GUILayout.Button("Create account"))
      {
        AssetStore.Open("createuser/");
        this.m_LoginRemoteMessage = "Cancelled - create user";
        this.Close();
      }
      GUILayout.FlexibleSpace();
      if (GUILayout.Button("Cancel"))
      {
        this.m_LoginRemoteMessage = "Cancelled";
        this.Close();
      }
      GUILayout.Space(5f);
      if (GUILayout.Button("Login"))
      {
        this.DoLogin();
        this.Repaint();
      }
      GUILayout.Space(5f);
      GUILayout.EndHorizontal();
      GUILayout.Space(5f);
      GUILayout.EndVertical();
      if (Event.current.Equals((object) Event.KeyboardEvent("return")))
      {
        this.DoLogin();
        this.Repaint();
      }
      if (!(this.m_Username == string.Empty))
        return;
      EditorGUI.FocusTextInControl("username");
    }

    private void DoLogin()
    {
      this.m_LoginRemoteMessage = (string) null;
      if (AssetStoreClient.HasActiveSessionID)
        AssetStoreClient.Logout();
      AssetStoreClient.LoginWithCredentials(this.m_Username, this.m_Password, AssetStoreClient.RememberSession, (AssetStoreClient.DoneLoginCallback) (errorMessage =>
      {
        this.m_LoginRemoteMessage = errorMessage;
        if (errorMessage == null)
          this.Close();
        else
          this.Repaint();
      }));
    }

    private class Styles
    {
      public GUIStyle link = new GUIStyle(EditorStyles.miniLabel);

      public Styles()
      {
        this.link.normal.textColor = new Color(0.26f, 0.51f, 0.75f, 1f);
      }
    }

    public delegate void LoginCallback(string errorMessage);
  }
}
