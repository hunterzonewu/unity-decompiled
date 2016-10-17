// Decompiled with JetBrains decompiler
// Type: PostProcessWebPlayer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

internal class PostProcessWebPlayer
{
  private static Dictionary<BuildOptions, string> optionNames = new Dictionary<BuildOptions, string>()
  {
    {
      BuildOptions.AllowDebugging,
      "enableDebugging"
    }
  };

  public static void PostProcess(BuildOptions options, string installPath, string downloadWebplayerUrl, int width, int height)
  {
    string fileName = FileUtil.UnityGetFileName(installPath);
    string path1 = installPath;
    string str1 = "Temp/BuildingWebplayerTemplate";
    FileUtil.DeleteFileOrDirectory(str1);
    if (PlayerSettings.webPlayerTemplate == null || !PlayerSettings.webPlayerTemplate.Contains(":"))
    {
      Debug.LogError((object) "Invalid WebPlayer template selection! Select a template in player settings.");
    }
    else
    {
      string[] strArray = PlayerSettings.webPlayerTemplate.Split(':');
      string str2 = Path.Combine(Path.Combine(!strArray[0].Equals("PROJECT") ? Path.Combine(EditorApplication.applicationContentsPath, "Resources") : Application.dataPath, "WebPlayerTemplates"), strArray[1]);
      if (!Directory.Exists(str2))
        Debug.LogError((object) "Invalid WebPlayer template path! Select a template in player settings.");
      else if (Directory.GetFiles(str2, "index.*").Length < 1)
      {
        Debug.LogError((object) "Invalid WebPlayer template selection! Select a template in player settings.");
      }
      else
      {
        FileUtil.CopyDirectoryRecursive(str2, str1);
        string file = Directory.GetFiles(str1, "index.*")[0];
        string extension = Path.GetExtension(file);
        string str3 = Path.Combine(str1, fileName + extension);
        FileUtil.MoveFileOrDirectory(file, str3);
        string[] files = Directory.GetFiles(str1, "thumbnail.*");
        if (files.Length > 0)
          FileUtil.DeleteFileOrDirectory(files[0]);
        bool flag = (options & BuildOptions.WebPlayerOfflineDeployment) != BuildOptions.None;
        string str4 = !flag ? downloadWebplayerUrl + "/3.0/uo/UnityObject2.js" : "UnityObject2.js";
        string str5 = string.Format("<script type='text/javascript' src='{0}'></script>", !flag ? (object) "https://ssl-webplayer.unity3d.com/download_webplayer-3.x/3.0/uo/jquery.min.js" : (object) "jquery.min.js");
        List<string> stringList = new List<string>();
        stringList.Add("%UNITY_UNITYOBJECT_DEPENDENCIES%");
        stringList.Add(str5);
        stringList.Add("%UNITY_UNITYOBJECT_URL%");
        stringList.Add(str4);
        stringList.Add("%UNITY_WIDTH%");
        stringList.Add(width.ToString());
        stringList.Add("%UNITY_HEIGHT%");
        stringList.Add(height.ToString());
        stringList.Add("%UNITY_PLAYER_PARAMS%");
        stringList.Add(PostProcessWebPlayer.GeneratePlayerParamsString(options));
        stringList.Add("%UNITY_WEB_NAME%");
        stringList.Add(PlayerSettings.productName);
        stringList.Add("%UNITY_WEB_PATH%");
        stringList.Add(fileName + ".unity3d");
        if (InternalEditorUtility.IsUnityBeta())
        {
          stringList.Add("%UNITY_BETA_WARNING%");
          stringList.Add("\r\n\t\t<p style=\"color: #c00; font-size: small; font-style: italic;\">Built with beta version of Unity. Will only work on your computer!</p>");
          stringList.Add("%UNITY_SET_BASE_DOWNLOAD_URL%");
          stringList.Add(",baseDownloadUrl: \"" + downloadWebplayerUrl + "/\"");
        }
        else
        {
          stringList.Add("%UNITY_BETA_WARNING%");
          stringList.Add(string.Empty);
          stringList.Add("%UNITY_SET_BASE_DOWNLOAD_URL%");
          stringList.Add(string.Empty);
        }
        foreach (string templateCustomKey in PlayerSettings.templateCustomKeys)
        {
          stringList.Add("%UNITY_CUSTOM_" + templateCustomKey.ToUpper() + "%");
          stringList.Add(PlayerSettings.GetTemplateCustomValue(templateCustomKey));
        }
        FileUtil.ReplaceText(str3, stringList.ToArray());
        if (flag)
        {
          string str6 = Path.Combine(str1, "UnityObject2.js");
          FileUtil.DeleteFileOrDirectory(str6);
          FileUtil.UnityFileCopy(EditorApplication.applicationContentsPath + "/Resources/UnityObject2.js", str6);
          string str7 = Path.Combine(str1, "jquery.min.js");
          FileUtil.DeleteFileOrDirectory(str7);
          FileUtil.UnityFileCopy(EditorApplication.applicationContentsPath + "/Resources/jquery.min.js", str7);
        }
        FileUtil.CopyDirectoryRecursive(str1, installPath, true);
        string str8 = Path.Combine(path1, fileName + ".unity3d");
        FileUtil.DeleteFileOrDirectory(str8);
        FileUtil.MoveFileOrDirectory("Temp/unitystream.unity3d", str8);
        if (!Directory.Exists("Assets/StreamingAssets"))
          return;
        FileUtil.CopyDirectoryRecursiveForPostprocess("Assets/StreamingAssets", Path.Combine(path1, "StreamingAssets"), true);
      }
    }
  }

  private static string GeneratePlayerParamsString(BuildOptions options)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: reference to a compiler-generated method
    return string.Format("{{ {0} }}", (object) string.Join(",", PostProcessWebPlayer.optionNames.Select<KeyValuePair<BuildOptions, string>, string>(new Func<KeyValuePair<BuildOptions, string>, string>(new PostProcessWebPlayer.\u003CGeneratePlayerParamsString\u003Ec__AnonStorey6E()
    {
      options = options
    }.\u003C\u003Em__F1)).ToArray<string>()));
  }
}
