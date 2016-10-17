// Decompiled with JetBrains decompiler
// Type: PostProcessDashboardWidget
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor;

internal class PostProcessDashboardWidget
{
  public static void PostProcess(BuildTarget target, string installPath, string stagingArea, string playerPackage, string companyName, string productName, int width, int height)
  {
    string str1 = stagingArea + "/DashboardBuild";
    FileUtil.DeleteFileOrDirectory(str1);
    FileUtil.CopyFileOrDirectory(playerPackage, str1);
    FileUtil.MoveFileOrDirectory("Temp/unitystream.unity3d", str1 + "/widget.unity3d");
    PostprocessBuildPlayer.InstallPlugins(str1 + "/Plugins", target);
    string str2 = PostprocessBuildPlayer.GenerateBundleIdentifier(companyName, productName) + ".widget";
    string[] strArray = new string[12]
    {
      "UNITY_WIDTH_PLUS32",
      (width + 32).ToString(),
      "UNITY_HEIGHT_PLUS32",
      (height + 32).ToString(),
      "UNITY_WIDTH",
      width.ToString(),
      "UNITY_HEIGHT",
      height.ToString(),
      "UNITY_BUNDLE_IDENTIFIER",
      str2,
      "UNITY_BUNDLE_NAME",
      productName
    };
    FileUtil.ReplaceText(str1 + "/UnityWidget.html", strArray);
    FileUtil.ReplaceText(str1 + "/Info.plist", strArray);
    FileUtil.DeleteFileOrDirectory(installPath);
    FileUtil.MoveFileOrDirectory(str1, installPath);
  }
}
