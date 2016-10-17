// Decompiled with JetBrains decompiler
// Type: UnityEditor.SyncVS
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Utils;
using UnityEditor.VisualStudioIntegration;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [InitializeOnLoad]
  internal class SyncVS : AssetPostprocessor
  {
    private static readonly SolutionSynchronizer Synchronizer = new SolutionSynchronizer(Directory.GetParent(Application.dataPath).FullName, (ISolutionSynchronizationSettings) new SyncVS.SolutionSynchronizationSettings());
    private static bool s_AlreadySyncedThisDomainReload;

    internal static Dictionary<VisualStudioVersion, string> InstalledVisualStudios { get; private set; }

    static SyncVS()
    {
      EditorUserBuildSettings.activeBuildTargetChanged += new System.Action(SyncVS.SyncVisualStudioProjectIfItAlreadyExists);
      try
      {
        SyncVS.InstalledVisualStudios = SyncVS.GetInstalledVisualStudios() as Dictionary<VisualStudioVersion, string>;
      }
      catch (Exception ex)
      {
        Console.WriteLine("Error detecting Visual Studio installations: {0}{1}{2}", (object) ex.Message, (object) Environment.NewLine, (object) ex.StackTrace);
        SyncVS.InstalledVisualStudios = new Dictionary<VisualStudioVersion, string>();
      }
      SyncVS.SetVisualStudioAsEditorIfNoEditorWasSet();
      UnityVSSupport.Initialize();
    }

    private static void SetVisualStudioAsEditorIfNoEditorWasSet()
    {
      string str = EditorPrefs.GetString("kScriptsDefaultApp");
      string bestVisualStudio = SyncVS.FindBestVisualStudio();
      if (!(str == string.Empty) || bestVisualStudio == null)
        return;
      EditorPrefs.SetString("kScriptsDefaultApp", bestVisualStudio);
    }

    public static string FindBestVisualStudio()
    {
      return SyncVS.InstalledVisualStudios.OrderByDescending<KeyValuePair<VisualStudioVersion, string>, VisualStudioVersion>((Func<KeyValuePair<VisualStudioVersion, string>, VisualStudioVersion>) (kvp => kvp.Key)).Select<KeyValuePair<VisualStudioVersion, string>, string>((Func<KeyValuePair<VisualStudioVersion, string>, string>) (kvp2 => kvp2.Value)).FirstOrDefault<string>();
    }

    public static bool ProjectExists()
    {
      return SyncVS.Synchronizer.SolutionExists();
    }

    public static void CreateIfDoesntExist()
    {
      if (SyncVS.Synchronizer.SolutionExists())
        return;
      SyncVS.Synchronizer.Sync();
    }

    public static void SyncVisualStudioProjectIfItAlreadyExists()
    {
      if (!SyncVS.Synchronizer.SolutionExists())
        return;
      SyncVS.Synchronizer.Sync();
    }

    public static void PostprocessSyncProject(string[] importedAssets, string[] addedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
      SyncVS.Synchronizer.SyncIfNeeded(((IEnumerable<string>) addedAssets).Union<string>(((IEnumerable<string>) deletedAssets).Union<string>(((IEnumerable<string>) movedAssets).Union<string>((IEnumerable<string>) movedFromAssetPaths))));
    }

    [MenuItem("Assets/Open C# Project")]
    private static void SyncAndOpenSolution()
    {
      SyncVS.SyncSolution();
      SyncVS.OpenProjectFileUnlessInBatchMode();
    }

    public static void SyncSolution()
    {
      AssetDatabase.Refresh();
      SyncVS.Synchronizer.Sync();
    }

    public static void SyncIfFirstFileOpenSinceDomainLoad()
    {
      if (SyncVS.s_AlreadySyncedThisDomainReload)
        return;
      SyncVS.s_AlreadySyncedThisDomainReload = true;
      SyncVS.Synchronizer.Sync();
    }

    private static void OpenProjectFileUnlessInBatchMode()
    {
      if (InternalEditorUtility.inBatchMode)
        return;
      InternalEditorUtility.OpenFileAtLineExternal(string.Empty, -1);
    }

    private static IDictionary<VisualStudioVersion, string> GetInstalledVisualStudios()
    {
      Dictionary<VisualStudioVersion, string> dictionary = new Dictionary<VisualStudioVersion, string>();
      if (SyncVS.SolutionSynchronizationSettings.IsWindows)
      {
        foreach (int num in Enum.GetValues(typeof (VisualStudioVersion)))
        {
          VisualStudioVersion index = (VisualStudioVersion) num;
          try
          {
            string environmentVariable = Environment.GetEnvironmentVariable(string.Format("VS{0}0COMNTOOLS", (object) index));
            if (!string.IsNullOrEmpty(environmentVariable))
            {
              string path = Paths.Combine(environmentVariable, "..", "IDE", "devenv.exe");
              if (File.Exists(path))
              {
                dictionary[index] = path;
                continue;
              }
            }
            string debuggerPath = Registry.GetValue(string.Format("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\VisualStudio\\{0}.0\\Debugger", (object) index), "FEQARuntimeImplDll", (object) null) as string;
            if (!string.IsNullOrEmpty(debuggerPath))
            {
              string path = SyncVS.DeriveVisualStudioPath(debuggerPath);
              if (!string.IsNullOrEmpty(path))
              {
                if (File.Exists(path))
                  dictionary[index] = SyncVS.DeriveVisualStudioPath(debuggerPath);
              }
            }
          }
          catch
          {
          }
        }
      }
      return (IDictionary<VisualStudioVersion, string>) dictionary;
    }

    private static string DeriveVisualStudioPath(string debuggerPath)
    {
      string a1 = SyncVS.DeriveProgramFilesSentinel();
      string a2 = "Common7";
      bool flag = false;
      string[] strArray = debuggerPath.Split(new char[2]
      {
        Path.DirectorySeparatorChar,
        Path.AltDirectorySeparatorChar
      }, StringSplitOptions.RemoveEmptyEntries);
      string path1 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
      foreach (string str in strArray)
      {
        if (!flag && string.Equals(a1, str, StringComparison.OrdinalIgnoreCase))
          flag = true;
        else if (flag)
        {
          path1 = Path.Combine(path1, str);
          if (string.Equals(a2, str, StringComparison.OrdinalIgnoreCase))
            break;
        }
      }
      return Paths.Combine(path1, "IDE", "devenv.exe");
    }

    private static string DeriveProgramFilesSentinel()
    {
      string str = ((IEnumerable<string>) Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles).Split(new char[2]
      {
        Path.DirectorySeparatorChar,
        Path.AltDirectorySeparatorChar
      })).LastOrDefault<string>();
      if (string.IsNullOrEmpty(str))
        return "Program Files";
      int startIndex = str.LastIndexOf("(x86)");
      if (0 <= startIndex)
        str = str.Remove(startIndex);
      return str.TrimEnd();
    }

    private static bool PathsAreEquivalent(string aPath, string zPath)
    {
      if (aPath == null && zPath == null)
        return true;
      if (string.IsNullOrEmpty(aPath) || string.IsNullOrEmpty(zPath))
        return false;
      aPath = Path.GetFullPath(aPath);
      zPath = Path.GetFullPath(zPath);
      StringComparison comparisonType = StringComparison.OrdinalIgnoreCase;
      if (!SyncVS.SolutionSynchronizationSettings.IsOSX && !SyncVS.SolutionSynchronizationSettings.IsWindows)
        comparisonType = StringComparison.Ordinal;
      aPath = aPath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
      zPath = zPath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
      return string.Equals(aPath, zPath, comparisonType);
    }

    internal static bool CheckVisualStudioVersion(int major, int minor, int build)
    {
      int num = -1;
      if (major != 11)
        return false;
      RegistryKey registryKey1 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\Microsoft\\DevDiv\\vc\\Servicing");
      if (registryKey1 == null)
        return false;
      foreach (string subKeyName in registryKey1.GetSubKeyNames())
      {
        if (subKeyName.StartsWith("11."))
        {
          if (subKeyName.Length > 3)
          {
            try
            {
              int int32 = Convert.ToInt32(subKeyName.Substring(3));
              if (int32 > num)
                num = int32;
            }
            catch (Exception ex)
            {
            }
          }
        }
      }
      if (num < 0)
        return false;
      RegistryKey registryKey2 = registryKey1.OpenSubKey(string.Format("11.{0}\\RuntimeDebug", (object) num));
      if (registryKey2 == null)
        return false;
      string str = registryKey2.GetValue("Version", (object) null) as string;
      if (str == null)
        return false;
      string[] strArray = str.Split('.');
      if (strArray != null)
      {
        if (strArray.Length >= 3)
        {
          int int32;
          try
          {
            int32 = Convert.ToInt32(strArray[2]);
          }
          catch (Exception ex)
          {
            return false;
          }
          if (num > minor)
            return true;
          if (num == minor)
            return int32 >= build;
          return false;
        }
      }
      return false;
    }

    private class SolutionSynchronizationSettings : DefaultSolutionSynchronizationSettings
    {
      public override int VisualStudioVersion
      {
        get
        {
          string externalScriptEditor = InternalEditorUtility.GetExternalScriptEditor();
          return SyncVS.InstalledVisualStudios.ContainsKey(VisualStudioVersion.VisualStudio2008) && externalScriptEditor != string.Empty && SyncVS.PathsAreEquivalent(SyncVS.InstalledVisualStudios[VisualStudioVersion.VisualStudio2008], externalScriptEditor) ? 9 : 10;
        }
      }

      public override string SolutionTemplate
      {
        get
        {
          return EditorPrefs.GetString("VSSolutionText", base.SolutionTemplate);
        }
      }

      public override string EditorAssemblyPath
      {
        get
        {
          return InternalEditorUtility.GetEditorAssemblyPath();
        }
      }

      public override string EngineAssemblyPath
      {
        get
        {
          return InternalEditorUtility.GetEngineAssemblyPath();
        }
      }

      public override string[] Defines
      {
        get
        {
          return EditorUserBuildSettings.activeScriptCompilationDefines;
        }
      }

      internal static bool IsOSX
      {
        get
        {
          return Environment.OSVersion.Platform == PlatformID.Unix;
        }
      }

      internal static bool IsWindows
      {
        get
        {
          if (!SyncVS.SolutionSynchronizationSettings.IsOSX && (int) Path.DirectorySeparatorChar == 92)
            return Environment.NewLine == "\r\n";
          return false;
        }
      }

      public override string GetProjectHeaderTemplate(ScriptingLanguage language)
      {
        return EditorPrefs.GetString("VSProjectHeader", base.GetProjectHeaderTemplate(language));
      }

      public override string GetProjectFooterTemplate(ScriptingLanguage language)
      {
        return EditorPrefs.GetString("VSProjectFooter", base.GetProjectFooterTemplate(language));
      }

      protected override string FrameworksPath()
      {
        string applicationContentsPath = EditorApplication.applicationContentsPath;
        if (SyncVS.SolutionSynchronizationSettings.IsOSX)
          return applicationContentsPath + "/Frameworks";
        return applicationContentsPath;
      }
    }
  }
}
