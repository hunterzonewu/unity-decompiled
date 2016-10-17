// Decompiled with JetBrains decompiler
// Type: UnityEditor.PostprocessBuildPlayer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor.Modules;
using UnityEditor.Utils;
using UnityEngine;

namespace UnityEditor
{
  internal class PostprocessBuildPlayer
  {
    internal const string StreamingAssets = "Assets/StreamingAssets";

    public static string subDir32Bit
    {
      get
      {
        return "x86";
      }
    }

    public static string subDir64Bit
    {
      get
      {
        return "x86_64";
      }
    }

    internal static string GenerateBundleIdentifier(string companyName, string productName)
    {
      return "unity." + companyName + "." + productName;
    }

    internal static void InstallPlugins(string destPluginFolder, BuildTarget target)
    {
      string str1 = "Assets/Plugins";
      IBuildPostprocessor buildPostProcessor = ModuleManager.GetBuildPostProcessor(target);
      if (buildPostProcessor != null)
      {
        bool shouldRetainStructure;
        string[] pluginFilesToCopy = buildPostProcessor.FindPluginFilesToCopy(str1, out shouldRetainStructure);
        if (pluginFilesToCopy != null)
        {
          if (pluginFilesToCopy.Length > 0 && !Directory.Exists(destPluginFolder))
            Directory.CreateDirectory(destPluginFolder);
          foreach (string str2 in pluginFilesToCopy)
          {
            if (Directory.Exists(str2))
            {
              string target1 = Path.Combine(destPluginFolder, str2);
              FileUtil.CopyDirectoryRecursive(str2, target1);
            }
            else
            {
              string fileName = Path.GetFileName(str2);
              if (shouldRetainStructure)
              {
                string directoryName = Path.GetDirectoryName(str2.Substring(str1.Length + 1));
                string str3 = Path.Combine(destPluginFolder, directoryName);
                string to = Path.Combine(str3, fileName);
                if (!Directory.Exists(str3))
                  Directory.CreateDirectory(str3);
                FileUtil.UnityFileCopy(str2, to, true);
              }
              else
              {
                string to = Path.Combine(destPluginFolder, fileName);
                FileUtil.UnityFileCopy(str2, to, true);
              }
            }
          }
          return;
        }
      }
      bool flag1 = false;
      List<string> subdirs = new List<string>();
      bool flag2 = target == BuildTarget.StandaloneOSXIntel || target == BuildTarget.StandaloneOSXIntel64 || target == BuildTarget.StandaloneOSXUniversal;
      bool copyDirectories = flag2;
      string extension = string.Empty;
      string debugExtension = string.Empty;
      if (flag2)
      {
        extension = ".bundle";
        subdirs.Add(string.Empty);
      }
      else if (target == BuildTarget.StandaloneWindows)
      {
        extension = ".dll";
        debugExtension = ".pdb";
        PostprocessBuildPlayer.AddPluginSubdirIfExists(subdirs, str1, PostprocessBuildPlayer.subDir32Bit);
      }
      else if (target == BuildTarget.StandaloneWindows64)
      {
        extension = ".dll";
        debugExtension = ".pdb";
        PostprocessBuildPlayer.AddPluginSubdirIfExists(subdirs, str1, PostprocessBuildPlayer.subDir64Bit);
      }
      else if (target == BuildTarget.StandaloneGLESEmu)
      {
        extension = ".dll";
        debugExtension = ".pdb";
        subdirs.Add(string.Empty);
      }
      else if (target == BuildTarget.StandaloneLinux)
      {
        extension = ".so";
        PostprocessBuildPlayer.AddPluginSubdirIfExists(subdirs, str1, PostprocessBuildPlayer.subDir32Bit);
      }
      else if (target == BuildTarget.StandaloneLinux64)
      {
        extension = ".so";
        PostprocessBuildPlayer.AddPluginSubdirIfExists(subdirs, str1, PostprocessBuildPlayer.subDir64Bit);
      }
      else if (target == BuildTarget.StandaloneLinuxUniversal)
      {
        extension = ".so";
        subdirs.Add(PostprocessBuildPlayer.subDir32Bit);
        subdirs.Add(PostprocessBuildPlayer.subDir64Bit);
        flag1 = true;
      }
      else if (target == BuildTarget.PS3)
      {
        extension = ".sprx";
        subdirs.Add(string.Empty);
      }
      else if (target == BuildTarget.Android)
      {
        extension = ".so";
        subdirs.Add("Android");
      }
      else if (target == BuildTarget.BlackBerry)
      {
        extension = ".so";
        subdirs.Add("BlackBerry");
      }
      using (List<string>.Enumerator enumerator = subdirs.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          string current = enumerator.Current;
          if (flag1)
            PostprocessBuildPlayer.InstallPluginsByExtension(Path.Combine(str1, current), extension, debugExtension, Path.Combine(destPluginFolder, current), copyDirectories);
          else
            PostprocessBuildPlayer.InstallPluginsByExtension(Path.Combine(str1, current), extension, debugExtension, destPluginFolder, copyDirectories);
        }
      }
    }

    private static void AddPluginSubdirIfExists(List<string> subdirs, string basedir, string subdir)
    {
      if (Directory.Exists(Path.Combine(basedir, subdir)))
        subdirs.Add(subdir);
      else
        subdirs.Add(string.Empty);
    }

    internal static bool IsPlugin(string path, string targetExtension)
    {
      if (string.Compare(Path.GetExtension(path), targetExtension, true) != 0)
        return string.Compare(Path.GetFileName(path), targetExtension, true) == 0;
      return true;
    }

    internal static bool InstallPluginsByExtension(string pluginSourceFolder, string extension, string debugExtension, string destPluginFolder, bool copyDirectories)
    {
      bool flag = false;
      if (!Directory.Exists(pluginSourceFolder))
        return flag;
      foreach (string fileSystemEntry in Directory.GetFileSystemEntries(pluginSourceFolder))
      {
        string fileName = Path.GetFileName(fileSystemEntry);
        string extension1 = Path.GetExtension(fileSystemEntry);
        if (extension1.Equals(extension, StringComparison.OrdinalIgnoreCase) || fileName.Equals(extension, StringComparison.OrdinalIgnoreCase) || debugExtension != null && debugExtension.Length != 0 && (extension1.Equals(debugExtension, StringComparison.OrdinalIgnoreCase) || fileName.Equals(debugExtension, StringComparison.OrdinalIgnoreCase)))
        {
          if (!Directory.Exists(destPluginFolder))
            Directory.CreateDirectory(destPluginFolder);
          string str = Path.Combine(destPluginFolder, fileName);
          if (copyDirectories)
            FileUtil.CopyDirectoryRecursive(fileSystemEntry, str);
          else if (!Directory.Exists(fileSystemEntry))
            FileUtil.UnityFileCopy(fileSystemEntry, str);
          flag = true;
        }
      }
      return flag;
    }

    internal static void InstallStreamingAssets(string stagingAreaDataPath)
    {
      if (!Directory.Exists("Assets/StreamingAssets"))
        return;
      FileUtil.CopyDirectoryRecursiveForPostprocess("Assets/StreamingAssets", Path.Combine(stagingAreaDataPath, "StreamingAssets"), true);
    }

    public static string GetScriptLayoutFileFromBuild(BuildOptions options, BuildTarget target, string installPath, string fileName)
    {
      IBuildPostprocessor buildPostProcessor = ModuleManager.GetBuildPostProcessor(target);
      if (buildPostProcessor != null)
        return buildPostProcessor.GetScriptLayoutFileFromBuild(options, installPath, fileName);
      return string.Empty;
    }

    public static bool SupportsScriptsOnlyBuild(BuildTarget target)
    {
      IBuildPostprocessor buildPostProcessor = ModuleManager.GetBuildPostProcessor(target);
      if (buildPostProcessor != null)
        return buildPostProcessor.SupportsScriptsOnlyBuild();
      return false;
    }

    public static string GetExtensionForBuildTarget(BuildTarget target, BuildOptions options)
    {
      IBuildPostprocessor buildPostProcessor = ModuleManager.GetBuildPostProcessor(target);
      if (buildPostProcessor == null)
        return string.Empty;
      return buildPostProcessor.GetExtension(target, options);
    }

    public static bool SupportsInstallInBuildFolder(BuildTarget target)
    {
      IBuildPostprocessor buildPostProcessor = ModuleManager.GetBuildPostProcessor(target);
      if (buildPostProcessor != null)
        return buildPostProcessor.SupportsInstallInBuildFolder();
      BuildTarget buildTarget = target;
      switch (buildTarget)
      {
        case BuildTarget.PS3:
        case BuildTarget.Android:
label_5:
          return true;
        default:
          switch (buildTarget - 30)
          {
            case ~BuildTarget.iPhone:
            case BuildTarget.StandaloneOSXUniversal:
              goto label_5;
            default:
              if (buildTarget != BuildTarget.WSAPlayer && buildTarget != BuildTarget.WP8Player)
                return false;
              goto label_5;
          }
      }
    }

    public static void Launch(BuildTarget target, string path, string productName, BuildOptions options)
    {
      IBuildPostprocessor buildPostProcessor = ModuleManager.GetBuildPostProcessor(target);
      if (buildPostProcessor == null)
        throw new UnityException(string.Format("Launching {0} build target via mono is not supported", (object) target));
      BuildLaunchPlayerArgs args;
      args.target = target;
      args.playerPackage = BuildPipeline.GetPlaybackEngineDirectory(target, options);
      args.installPath = path;
      args.productName = productName;
      args.options = options;
      buildPostProcessor.LaunchPlayer(args);
    }

    public static void Postprocess(BuildTarget target, string installPath, string companyName, string productName, int width, int height, string downloadWebplayerUrl, string manualDownloadWebplayerUrl, BuildOptions options, RuntimeClassRegistry usedClassRegistry)
    {
      string str1 = "Temp/StagingArea";
      string str2 = "Temp/StagingArea/Data";
      string str3 = "Temp/StagingArea/Data/Managed";
      string playbackEngineDirectory = BuildPipeline.GetPlaybackEngineDirectory(target, options);
      bool flag = (options & BuildOptions.InstallInBuildFolder) != BuildOptions.None && PostprocessBuildPlayer.SupportsInstallInBuildFolder(target);
      if (installPath == string.Empty && !flag)
        throw new Exception(installPath + " must not be an empty string");
      IBuildPostprocessor buildPostProcessor = ModuleManager.GetBuildPostProcessor(target);
      if (buildPostProcessor != null)
      {
        BuildPostProcessArgs args;
        args.target = target;
        args.stagingAreaData = str2;
        args.stagingArea = str1;
        args.stagingAreaDataManaged = str3;
        args.playerPackage = playbackEngineDirectory;
        args.installPath = installPath;
        args.companyName = companyName;
        args.productName = productName;
        args.productGUID = PlayerSettings.productGUID;
        args.options = options;
        args.usedClassRegistry = usedClassRegistry;
        buildPostProcessor.PostProcess(args);
      }
      else
      {
        switch (target)
        {
          case BuildTarget.WebPlayer:
          case BuildTarget.WebPlayerStreamed:
            PostProcessWebPlayer.PostProcess(options, installPath, downloadWebplayerUrl, width, height);
            break;
          default:
            throw new UnityException(string.Format("Build target '{0}' not supported", (object) target));
        }
      }
    }

    internal static string ExecuteSystemProcess(string command, string args, string workingdir)
    {
      Program program = new Program(new ProcessStartInfo() { FileName = command, Arguments = args, WorkingDirectory = workingdir, CreateNoWindow = true });
      program.Start();
      do
        ;
      while (!program.WaitForExit(100));
      string standardOutputAsString = program.GetStandardOutputAsString();
      program.Dispose();
      return standardOutputAsString;
    }

    internal static string GetArchitectureForTarget(BuildTarget target)
    {
      BuildTarget buildTarget = target;
      switch (buildTarget)
      {
        case BuildTarget.StandaloneLinux:
          return "x86";
        case BuildTarget.StandaloneWindows64:
          return "x86_64";
        default:
          if (buildTarget != BuildTarget.StandaloneOSXIntel && buildTarget != BuildTarget.StandaloneWindows)
          {
            if (buildTarget != BuildTarget.StandaloneLinux64)
            {
              if (buildTarget != BuildTarget.StandaloneLinuxUniversal)
                return string.Empty;
              goto case BuildTarget.StandaloneLinux;
            }
            else
              goto case BuildTarget.StandaloneWindows64;
          }
          else
            goto case BuildTarget.StandaloneLinux;
      }
    }
  }
}
