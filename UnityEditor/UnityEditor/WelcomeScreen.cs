// Decompiled with JetBrains decompiler
// Type: UnityEditor.WelcomeScreen
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class WelcomeScreen : EditorWindow
  {
    private const string kVideoTutURL = "http://unity3d.com/learn/tutorials/modules/";
    private const string kUnityBasicsHelp = "file:///unity/Manual/UnityBasics.html";
    private const string kUnityAnswersURL = "http://answers.unity3d.com/";
    private const string kUnityForumURL = "http://forum.unity3d.com/";
    private const string kAssetStoreURL = "home/?ref=http%3a%2f%2fUnityEditor.unity3d.com%2fWelcomeScreen";
    private const float kItemHeight = 55f;
    private static WelcomeScreen.Styles styles;
    private static bool s_ShowAtStartup;
    private static int s_ShowCount;

    private static void DoShowWelcomeScreen(string how)
    {
      ++WelcomeScreen.s_ShowCount;
      EditorPrefs.SetInt("WelcomeScreenShowCount", WelcomeScreen.s_ShowCount);
      Analytics.Track(string.Format("/WelcomeScreen/Show/{0}/{1}", (object) how, (object) WelcomeScreen.s_ShowCount));
      EditorWindow.GetWindowWithRect<WelcomeScreen>(new Rect(0.0f, 0.0f, 570f, 440f), true, "Welcome To Unity");
    }

    private static void ShowWelcomeScreen()
    {
      WelcomeScreen.DoShowWelcomeScreen("Manual");
    }

    private static void ShowWelcomeScreenAtStartup()
    {
      WelcomeScreen.LoadLogos();
      if (!WelcomeScreen.s_ShowAtStartup)
        return;
      WelcomeScreen.DoShowWelcomeScreen("Startup");
    }

    private static void LoadLogos()
    {
      if (WelcomeScreen.styles != null)
        return;
      WelcomeScreen.s_ShowAtStartup = EditorPrefs.GetInt("ShowWelcomeAtStartup4", 1) != 0;
      WelcomeScreen.s_ShowCount = EditorPrefs.GetInt("WelcomeScreenShowCount", 0);
      WelcomeScreen.styles = new WelcomeScreen.Styles();
    }

    public void OnGUI()
    {
      WelcomeScreen.LoadLogos();
      GUILayout.BeginVertical();
      GUI.Box(new Rect(13f, 8f, (float) WelcomeScreen.styles.unityLogo.image.width, (float) WelcomeScreen.styles.unityLogo.image.height), WelcomeScreen.styles.unityLogo, GUIStyle.none);
      GUILayout.Space(15f);
      GUILayout.BeginHorizontal();
      GUILayout.Space(120f);
      GUILayout.BeginVertical();
      GUILayout.Label(WelcomeScreen.styles.mainHeader);
      GUILayout.Label(WelcomeScreen.styles.mainText, (GUIStyle) "WordWrappedLabel", new GUILayoutOption[1]
      {
        GUILayout.Width(300f)
      });
      GUILayout.EndVertical();
      GUILayout.EndHorizontal();
      GUILayout.Space(8f);
      this.ShowEntry(WelcomeScreen.styles.videoTutLogo, "http://unity3d.com/learn/tutorials/modules/", WelcomeScreen.styles.videoTutHeader, WelcomeScreen.styles.videoTutText, "VideoTutorials");
      this.ShowEntry(WelcomeScreen.styles.unityBasicsLogo, "file:///unity/Manual/UnityBasics.html", WelcomeScreen.styles.unityBasicsHeader, WelcomeScreen.styles.unityBasicsText, "UnityBasics");
      this.ShowEntry(WelcomeScreen.styles.unityAnswersLogo, "http://answers.unity3d.com/", WelcomeScreen.styles.unityAnswersHeader, WelcomeScreen.styles.unityAnswersText, "UnityAnswers");
      this.ShowEntry(WelcomeScreen.styles.unityForumLogo, "http://forum.unity3d.com/", WelcomeScreen.styles.unityForumHeader, WelcomeScreen.styles.unityForumText, "UnityForum");
      this.ShowEntry(WelcomeScreen.styles.assetStoreLogo, "home/?ref=http%3a%2f%2fUnityEditor.unity3d.com%2fWelcomeScreen", WelcomeScreen.styles.assetStoreHeader, WelcomeScreen.styles.assetStoreText, "AssetStore");
      GUILayout.FlexibleSpace();
      GUILayout.BeginHorizontal(GUILayout.Height(20f));
      GUILayout.FlexibleSpace();
      GUI.changed = false;
      WelcomeScreen.s_ShowAtStartup = GUILayout.Toggle(WelcomeScreen.s_ShowAtStartup, WelcomeScreen.styles.showAtStartupText);
      if (GUI.changed)
      {
        EditorPrefs.SetInt("ShowWelcomeAtStartup4", !WelcomeScreen.s_ShowAtStartup ? 0 : 1);
        if (WelcomeScreen.s_ShowAtStartup)
          Analytics.Track(string.Format("/WelcomeScreen/EnableAtStartup/{0}", (object) WelcomeScreen.s_ShowCount));
        else
          Analytics.Track(string.Format("/WelcomeScreen/DisableAtStartup/{0}", (object) WelcomeScreen.s_ShowCount));
        WelcomeScreen.s_ShowCount = 0;
        EditorPrefs.SetInt("WelcomeScreenShowCount", 0);
      }
      GUILayout.Space(10f);
      GUILayout.EndHorizontal();
      GUILayout.EndVertical();
    }

    private void ShowEntry(GUIContent logo, string url, GUIContent header, GUIContent text, string analyticsAction)
    {
      GUILayout.BeginHorizontal(GUILayout.Height(55f));
      GUILayout.BeginHorizontal(GUILayout.Width(120f));
      GUILayout.FlexibleSpace();
      if (GUILayout.Button(logo, GUIStyle.none, new GUILayoutOption[0]))
        this.ShowHelpPageOrBrowseURL(url, analyticsAction);
      EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
      GUILayout.Space(10f);
      GUILayout.EndHorizontal();
      GUILayout.BeginVertical();
      if (GUILayout.Button(header, (GUIStyle) "HeaderLabel", new GUILayoutOption[0]))
        this.ShowHelpPageOrBrowseURL(url, analyticsAction);
      EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
      GUILayout.Label(text, (GUIStyle) "WordWrappedLabel", new GUILayoutOption[1]
      {
        GUILayout.Width(400f)
      });
      GUILayout.EndVertical();
      GUILayout.EndHorizontal();
    }

    private void ShowHelpPageOrBrowseURL(string url, string analyticsAction)
    {
      Analytics.Track(string.Format("/WelcomeScreen/OpenURL/{0}/{1}", (object) analyticsAction, (object) WelcomeScreen.s_ShowCount));
      if (url.StartsWith("file"))
        Help.ShowHelpPage(url);
      else if (url.StartsWith("home/"))
      {
        AssetStore.Open(url);
        GUIUtility.ExitGUI();
      }
      else
        Help.BrowseURL(url);
    }

    private class Styles
    {
      public GUIContent unityLogo = EditorGUIUtility.IconContent("UnityLogo");
      public GUIContent mainHeader = EditorGUIUtility.IconContent("WelcomeScreen.MainHeader");
      public GUIContent mainText = EditorGUIUtility.TextContent("As you dive into Unity, may we suggest a few hints to get you off to a good start?");
      public GUIContent videoTutLogo = EditorGUIUtility.IconContent("WelcomeScreen.VideoTutLogo");
      public GUIContent videoTutHeader = EditorGUIUtility.TextContent("Video Tutorials");
      public GUIContent videoTutText = EditorGUIUtility.TextContent("We have collected some videos to make you productive immediately.");
      public GUIContent unityBasicsLogo = EditorGUIUtility.IconContent("WelcomeScreen.UnityBasicsLogo");
      public GUIContent unityBasicsHeader = EditorGUIUtility.TextContent("Unity Basics");
      public GUIContent unityBasicsText = EditorGUIUtility.TextContent("Take a look at our manual for a quick startup guide.");
      public GUIContent unityForumLogo = EditorGUIUtility.IconContent("WelcomeScreen.UnityForumLogo");
      public GUIContent unityForumHeader = EditorGUIUtility.TextContent("Unity Forum");
      public GUIContent unityForumText = EditorGUIUtility.TextContent("Meet the other Unity users here - the friendliest people in the industry.");
      public GUIContent unityAnswersLogo = EditorGUIUtility.IconContent("WelcomeScreen.UnityAnswersLogo");
      public GUIContent unityAnswersHeader = EditorGUIUtility.TextContent("Unity Answers");
      public GUIContent unityAnswersText = EditorGUIUtility.TextContent("Have a question about how to use Unity? Check our answers site for precise how-to knowledge.");
      public GUIContent assetStoreLogo = EditorGUIUtility.IconContent("WelcomeScreen.AssetStoreLogo");
      public GUIContent assetStoreHeader = EditorGUIUtility.TextContent("Unity Asset Store");
      public GUIContent assetStoreText = EditorGUIUtility.TextContent("The Asset Store is the place to find art assets, game code and extensions directly inside the Unity editor. It's like having a complete supermarket under your desk.");
      public GUIContent showAtStartupText = EditorGUIUtility.TextContent("Show at Startup");
    }
  }
}
