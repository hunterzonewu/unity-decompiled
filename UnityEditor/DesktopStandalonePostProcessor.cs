// Decompiled with JetBrains decompiler
// Type: DesktopStandalonePostProcessor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Modules;
using UnityEditorInternal;
using UnityEngine;

internal abstract class DesktopStandalonePostProcessor
{
  protected BuildPostProcessArgs m_PostProcessArgs;

  protected abstract string StagingAreaPluginsFolder { get; }

  protected abstract string DestinationFolderForInstallingIntoBuildsFolder { get; }

  protected bool InstallingIntoBuildsFolder
  {
    get
    {
      return (this.m_PostProcessArgs.options & BuildOptions.InstallInBuildFolder) != BuildOptions.None;
    }
  }

  protected bool UseIl2Cpp
  {
    get
    {
      int num = 0;
      if (PlayerSettings.GetPropertyOptionalInt("ScriptingBackend", ref num, BuildTargetGroup.Standalone))
        return num == 1;
      return false;
    }
  }

  protected string StagingArea
  {
    get
    {
      return this.m_PostProcessArgs.stagingArea;
    }
  }

  protected string InstallPath
  {
    get
    {
      return this.m_PostProcessArgs.installPath;
    }
  }

  protected string DataFolder
  {
    get
    {
      return this.StagingArea + "/Data";
    }
  }

  protected BuildTarget Target
  {
    get
    {
      return this.m_PostProcessArgs.target;
    }
  }

  protected string DestinationFolder
  {
    get
    {
      return FileUtil.UnityGetDirectoryName(this.m_PostProcessArgs.installPath);
    }
  }

  protected bool Development
  {
    get
    {
      return (this.m_PostProcessArgs.options & BuildOptions.Development) != BuildOptions.None;
    }
  }

  protected DesktopStandalonePostProcessor()
  {
  }

  protected DesktopStandalonePostProcessor(BuildPostProcessArgs postProcessArgs)
  {
    this.m_PostProcessArgs = postProcessArgs;
  }

  public void PostProcess()
  {
    this.SetupStagingArea();
    this.CopyStagingAreaIntoDestination();
  }

  private void CopyNativePlugins()
  {
    string buildTargetName = BuildPipeline.GetBuildTargetName(this.m_PostProcessArgs.target);
    IPluginImporterExtension importerExtension = (IPluginImporterExtension) new DesktopPluginImporterExtension();
    string areaPluginsFolder = this.StagingAreaPluginsFolder;
    string path1 = Path.Combine(areaPluginsFolder, "x86");
    string path2 = Path.Combine(areaPluginsFolder, "x86_64");
    bool flag1 = false;
    bool flag2 = false;
    bool flag3 = false;
    foreach (PluginImporter importer in PluginImporter.GetImporters(this.m_PostProcessArgs.target))
    {
      BuildTarget target = this.m_PostProcessArgs.target;
      if (importer.isNativePlugin)
      {
        if (string.IsNullOrEmpty(importer.assetPath))
        {
          Debug.LogWarning((object) ("Got empty plugin importer path for " + this.m_PostProcessArgs.target.ToString()));
        }
        else
        {
          if (!flag1)
          {
            Directory.CreateDirectory(areaPluginsFolder);
            flag1 = true;
          }
          bool flag4 = Directory.Exists(importer.assetPath);
          string platformData = importer.GetPlatformData(target, "CPU");
          if (platformData != null)
          {
            // ISSUE: reference to a compiler-generated field
            if (DesktopStandalonePostProcessor.\u003C\u003Ef__switch\u0024map16 == null)
            {
              // ISSUE: reference to a compiler-generated field
              DesktopStandalonePostProcessor.\u003C\u003Ef__switch\u0024map16 = new Dictionary<string, int>(3)
              {
                {
                  "x86",
                  0
                },
                {
                  "x86_64",
                  1
                },
                {
                  "None",
                  2
                }
              };
            }
            int num;
            // ISSUE: reference to a compiler-generated field
            if (DesktopStandalonePostProcessor.\u003C\u003Ef__switch\u0024map16.TryGetValue(platformData, out num))
            {
              switch (num)
              {
                case 0:
                  if (target != BuildTarget.StandaloneOSXIntel64 && target != BuildTarget.StandaloneWindows64 && target != BuildTarget.StandaloneLinux64)
                  {
                    if (!flag2)
                    {
                      Directory.CreateDirectory(path1);
                      flag2 = true;
                      break;
                    }
                    break;
                  }
                  continue;
                case 1:
                  if (target == BuildTarget.StandaloneOSXIntel64 || target == BuildTarget.StandaloneOSXUniversal || (target == BuildTarget.StandaloneWindows64 || target == BuildTarget.StandaloneLinux64) || target == BuildTarget.StandaloneLinuxUniversal)
                  {
                    if (!flag3)
                    {
                      Directory.CreateDirectory(path2);
                      flag3 = true;
                      break;
                    }
                    break;
                  }
                  continue;
                case 2:
                  continue;
              }
            }
          }
          string finalPluginPath = importerExtension.CalculateFinalPluginPath(buildTargetName, importer);
          if (!string.IsNullOrEmpty(finalPluginPath))
          {
            string str = Path.Combine(areaPluginsFolder, finalPluginPath);
            if (flag4)
              FileUtil.CopyDirectoryRecursive(importer.assetPath, str);
            else
              FileUtil.UnityFileCopy(importer.assetPath, str);
          }
        }
      }
    }
  }

  private void CopyVirtualRealityDependencies()
  {
    if (!PlayerSettings.virtualRealitySupported)
      return;
    string source = EditorApplication.applicationContentsPath + "/VR/oculus/" + this.PlatformStringFor(this.m_PostProcessArgs.target);
    try
    {
      FileUtil.CopyDirectoryFiltered(source, this.StagingAreaPluginsFolder, false, (Func<string, bool>) (f => !f.Contains("UnityEngine.mdb")), true);
    }
    catch (IOException ex)
    {
      Debug.LogWarning((object) "VR Plugin overridden by user project.\n");
    }
  }

  protected virtual void SetupStagingArea()
  {
    Directory.CreateDirectory(this.DataFolder);
    this.CopyNativePlugins();
    this.CopyVirtualRealityDependencies();
    PostprocessBuildPlayer.InstallStreamingAssets(this.DataFolder);
    if (this.UseIl2Cpp)
    {
      this.CopyVariationFolderIntoStagingArea();
      string str1 = this.StagingArea + "/Data";
      string destinationFolder = this.DataFolder + "/Managed";
      string str2 = destinationFolder + "/Resources";
      string str3 = destinationFolder + "/Metadata";
      IL2CPPUtils.RunIl2Cpp(str1, this.GetPlatformProvider(this.m_PostProcessArgs.target), (System.Action<string>) (s => {}), this.m_PostProcessArgs.usedClassRegistry, this.Development);
      FileUtil.CreateOrCleanDirectory(str2);
      IL2CPPUtils.CopyEmbeddedResourceFiles(str1, str2);
      FileUtil.CreateOrCleanDirectory(str3);
      IL2CPPUtils.CopyMetadataFiles(str1, str3);
      IL2CPPUtils.CopySymmapFile(str1 + "/Native", destinationFolder);
    }
    if (this.InstallingIntoBuildsFolder)
    {
      this.CopyDataForBuildsFolder();
    }
    else
    {
      if (!this.UseIl2Cpp)
        this.CopyVariationFolderIntoStagingArea();
      this.RenameFilesInStagingArea();
    }
  }

  protected virtual void CopyVariationFolderIntoStagingArea()
  {
    FileUtil.CopyDirectoryFiltered(this.m_PostProcessArgs.playerPackage + "/Variations/" + this.GetVariationName(), this.StagingArea, true, (Func<string, bool>) (f => !f.Contains("UnityEngine.mdb")), true);
  }

  protected void CopyStagingAreaIntoDestination()
  {
    if (this.InstallingIntoBuildsFolder)
    {
      string str = Unsupported.GetBaseUnityDeveloperFolder() + "/" + this.DestinationFolderForInstallingIntoBuildsFolder;
      if (!Directory.Exists(Path.GetDirectoryName(str)))
        throw new Exception("Installing in builds folder failed because the player has not been built (You most likely want to enable 'Development build').");
      FileUtil.CopyDirectoryFiltered(this.DataFolder, str, true, (Func<string, bool>) (f => true), true);
    }
    else
    {
      this.DeleteDestination();
      FileUtil.CopyDirectoryFiltered(this.StagingArea, this.DestinationFolder, true, (Func<string, bool>) (f => true), true);
    }
  }

  protected abstract void DeleteDestination();

  protected abstract void CopyDataForBuildsFolder();

  protected virtual string GetVariationName()
  {
    return string.Format("{0}_{1}", (object) this.PlatformStringFor(this.m_PostProcessArgs.target), !this.Development ? (object) "nondevelopment" : (object) "development");
  }

  protected abstract string PlatformStringFor(BuildTarget target);

  protected abstract void RenameFilesInStagingArea();

  protected abstract IIl2CppPlatformProvider GetPlatformProvider(BuildTarget target);

  internal class ScriptingImplementations : IScriptingImplementations
  {
    public ScriptingImplementation[] Supported()
    {
      return new ScriptingImplementation[2]
      {
        ScriptingImplementation.Mono2x,
        ScriptingImplementation.IL2CPP
      };
    }

    public ScriptingImplementation[] Enabled()
    {
      if (!Unsupported.IsDeveloperBuild())
        return new ScriptingImplementation[1];
      return new ScriptingImplementation[2]
      {
        ScriptingImplementation.Mono2x,
        ScriptingImplementation.IL2CPP
      };
    }
  }
}
