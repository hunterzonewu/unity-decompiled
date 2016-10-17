// Decompiled with JetBrains decompiler
// Type: UnityEngine.UserAuthorizationDialog
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

namespace UnityEngine
{
  [AddComponentMenu("")]
  internal class UserAuthorizationDialog : MonoBehaviour
  {
    private const int width = 385;
    private const int height = 155;
    private Rect windowRect;
    private Texture warningIcon;

    private void Start()
    {
      this.warningIcon = (Texture) (Resources.GetBuiltinResource(typeof (Texture2D), "WarningSign.psd") as Texture2D);
      if (Screen.width < 385 || Screen.height < 155)
      {
        Debug.LogError((object) "Screen is to small to display authorization dialog. Authorization denied.");
        Application.ReplyToUserAuthorizationRequest(false);
      }
      this.windowRect = new Rect((float) (Screen.width / 2 - 192), (float) (Screen.height / 2 - 77), 385f, 155f);
    }

    private void OnGUI()
    {
      GUISkin skin = GUI.skin;
      GUISkin instance = ScriptableObject.CreateInstance("GUISkin") as GUISkin;
      instance.box.normal.background = (Texture2D) Resources.GetBuiltinResource(typeof (Texture2D), "GameSkin/box.png");
      instance.box.normal.textColor = new Color(0.9f, 0.9f, 0.9f, 1f);
      instance.box.padding.left = 6;
      instance.box.padding.right = 6;
      instance.box.padding.top = 4;
      instance.box.padding.bottom = 4;
      instance.box.border.left = 6;
      instance.box.border.right = 6;
      instance.box.border.top = 6;
      instance.box.border.bottom = 6;
      instance.box.margin.left = 4;
      instance.box.margin.right = 4;
      instance.box.margin.top = 4;
      instance.box.margin.bottom = 4;
      instance.button.normal.background = (Texture2D) Resources.GetBuiltinResource(typeof (Texture2D), "GameSkin/button.png");
      instance.button.normal.textColor = new Color(0.9f, 0.9f, 0.9f, 1f);
      instance.button.hover.background = (Texture2D) Resources.GetBuiltinResource(typeof (Texture2D), "GameSkin/button hover.png");
      instance.button.hover.textColor = Color.white;
      instance.button.active.background = (Texture2D) Resources.GetBuiltinResource(typeof (Texture2D), "GameSkin/button active.png");
      instance.button.active.textColor = new Color(0.9f, 0.9f, 0.9f, 1f);
      instance.button.border.left = 6;
      instance.button.border.right = 6;
      instance.button.border.top = 6;
      instance.button.border.bottom = 6;
      instance.button.padding.left = 8;
      instance.button.padding.right = 8;
      instance.button.padding.top = 4;
      instance.button.padding.bottom = 4;
      instance.button.margin.left = 4;
      instance.button.margin.right = 4;
      instance.button.margin.top = 4;
      instance.button.margin.bottom = 4;
      instance.label.normal.textColor = new Color(0.9f, 0.9f, 0.9f, 1f);
      instance.label.padding.left = 6;
      instance.label.padding.right = 6;
      instance.label.padding.top = 4;
      instance.label.padding.bottom = 4;
      instance.label.margin.left = 4;
      instance.label.margin.right = 4;
      instance.label.margin.top = 4;
      instance.label.margin.bottom = 4;
      instance.label.alignment = TextAnchor.UpperLeft;
      instance.window.normal.background = (Texture2D) Resources.GetBuiltinResource(typeof (Texture2D), "GameSkin/window.png");
      instance.window.normal.textColor = Color.white;
      instance.window.border.left = 8;
      instance.window.border.right = 8;
      instance.window.border.top = 18;
      instance.window.border.bottom = 8;
      instance.window.padding.left = 8;
      instance.window.padding.right = 8;
      instance.window.padding.top = 20;
      instance.window.padding.bottom = 5;
      instance.window.alignment = TextAnchor.UpperCenter;
      instance.window.contentOffset = new Vector2(0.0f, -18f);
      GUI.skin = instance;
      this.windowRect = GUI.Window(0, this.windowRect, new GUI.WindowFunction(this.DoUserAuthorizationDialog), "Unity Web Player Authorization Request");
      GUI.skin = skin;
    }

    private void DoUserAuthorizationDialog(int windowID)
    {
      UserAuthorization authorizationRequestMode = Application.GetUserAuthorizationRequestMode();
      GUILayout.FlexibleSpace();
      GUI.backgroundColor = new Color(0.9f, 0.9f, 0.9f, 0.7f);
      GUILayout.BeginHorizontal((GUIStyle) "box", new GUILayoutOption[0]);
      GUILayout.FlexibleSpace();
      GUILayout.BeginVertical();
      GUILayout.FlexibleSpace();
      GUILayout.Label(this.warningIcon);
      GUILayout.FlexibleSpace();
      GUILayout.EndVertical();
      GUILayout.FlexibleSpace();
      GUILayout.BeginVertical();
      GUILayout.FlexibleSpace();
      if (authorizationRequestMode == (UserAuthorization.WebCam | UserAuthorization.Microphone))
        GUILayout.Label("The content on this site would like to use your\ncomputer's web camera and microphone.\nThese images and sounds may be recorded.");
      else if (authorizationRequestMode == UserAuthorization.WebCam)
      {
        GUILayout.Label("The content on this site would like to use\nyour computer's web camera. The images\nfrom your web camera may be recorded.");
      }
      else
      {
        if (authorizationRequestMode != UserAuthorization.Microphone)
          return;
        GUILayout.Label("The content on this site would like to use\nyour computer's microphone. The sounds\nfrom your microphone may be recorded.");
      }
      GUILayout.FlexibleSpace();
      GUILayout.EndVertical();
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
      GUILayout.FlexibleSpace();
      GUI.backgroundColor = Color.white;
      GUILayout.BeginHorizontal();
      if (GUILayout.Button("Deny"))
        Application.ReplyToUserAuthorizationRequest(false);
      GUILayout.FlexibleSpace();
      if (GUILayout.Button("Always Allow for this Site"))
        Application.ReplyToUserAuthorizationRequest(true, true);
      GUILayout.Space(5f);
      if (GUILayout.Button("Allow"))
        Application.ReplyToUserAuthorizationRequest(true);
      GUILayout.EndHorizontal();
      GUILayout.FlexibleSpace();
    }
  }
}
