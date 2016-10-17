// Decompiled with JetBrains decompiler
// Type: UnityEditor.Utils.MonoInstallationFinder
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.IO;
using UnityEngine;

namespace UnityEditor.Utils
{
  internal class MonoInstallationFinder
  {
    public static string GetFrameWorksFolder()
    {
      string str = FileUtil.NiceWinPath(EditorApplication.applicationPath);
      if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform != RuntimePlatform.OSXEditor)
        return Path.Combine(Path.GetDirectoryName(str), "Data");
      return Path.Combine(str, Path.Combine("Contents", "Frameworks"));
    }

    public static string GetProfileDirectory(BuildTarget target, string profile)
    {
      return Path.Combine(MonoInstallationFinder.GetMonoInstallation(), Path.Combine("lib", Path.Combine("mono", profile)));
    }

    public static string GetEtcDirectory(string monoInstallation)
    {
      return Path.Combine(MonoInstallationFinder.GetMonoInstallation(monoInstallation), Path.Combine("etc", "mono"));
    }

    public static string GetMonoInstallation()
    {
      return MonoInstallationFinder.GetMonoInstallation("Mono");
    }

    public static string GetMonoInstallation(string monoName)
    {
      return Path.Combine(MonoInstallationFinder.GetFrameWorksFolder(), monoName);
    }
  }
}
