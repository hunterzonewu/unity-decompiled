// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.BaseIl2CppPlatformProvider
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.IO;
using System.Linq;
using Unity.DataContract;
using UnityEditor;
using UnityEditor.Modules;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class BaseIl2CppPlatformProvider : IIl2CppPlatformProvider
  {
    public virtual BuildTarget target { get; private set; }

    public virtual string libraryFolder { get; private set; }

    public virtual bool developmentMode
    {
      get
      {
        return false;
      }
    }

    public virtual bool emitNullChecks
    {
      get
      {
        return true;
      }
    }

    public virtual bool enableStackTraces
    {
      get
      {
        return true;
      }
    }

    public virtual bool enableArrayBoundsCheck
    {
      get
      {
        return true;
      }
    }

    public virtual bool loadSymbols
    {
      get
      {
        return false;
      }
    }

    public virtual bool supportsEngineStripping
    {
      get
      {
        return false;
      }
    }

    public virtual string[] includePaths
    {
      get
      {
        return new string[2]{ this.GetFolderInPackageOrDefault("bdwgc/include"), this.GetFolderInPackageOrDefault("libil2cpp/include") };
      }
    }

    public virtual string[] libraryPaths
    {
      get
      {
        return new string[2]{ this.GetFileInPackageOrDefault("bdwgc/lib/bdwgc." + this.staticLibraryExtension), this.GetFileInPackageOrDefault("libil2cpp/lib/libil2cpp." + this.staticLibraryExtension) };
      }
    }

    public virtual string nativeLibraryFileName
    {
      get
      {
        return (string) null;
      }
    }

    public virtual string staticLibraryExtension
    {
      get
      {
        return "a";
      }
    }

    public virtual string il2CppFolder
    {
      get
      {
        PackageInfo il2CppPackage = BaseIl2CppPlatformProvider.FindIl2CppPackage();
        if (PackageInfo.op_Equality(il2CppPackage, (PackageInfo) null))
          return Path.GetFullPath(Path.Combine(EditorApplication.applicationContentsPath, Application.platform != RuntimePlatform.OSXEditor ? "il2cpp" : "Frameworks/il2cpp"));
        return (string) il2CppPackage.basePath;
      }
    }

    public virtual string moduleStrippingInformationFolder
    {
      get
      {
        return Path.Combine(BuildPipeline.GetPlaybackEngineDirectory(EditorUserBuildSettings.activeBuildTarget, BuildOptions.None), "Whitelists");
      }
    }

    public BaseIl2CppPlatformProvider(BuildTarget target, string libraryFolder)
    {
      this.target = target;
      this.libraryFolder = libraryFolder;
    }

    public virtual INativeCompiler CreateNativeCompiler()
    {
      return (INativeCompiler) null;
    }

    protected string GetFolderInPackageOrDefault(string path)
    {
      PackageInfo il2CppPackage = BaseIl2CppPlatformProvider.FindIl2CppPackage();
      if (PackageInfo.op_Equality(il2CppPackage, (PackageInfo) null))
        return Path.Combine(this.libraryFolder, path);
      string path1 = Path.Combine((string) il2CppPackage.basePath, path);
      if (!Directory.Exists(path1))
        return Path.Combine(this.libraryFolder, path);
      return path1;
    }

    protected string GetFileInPackageOrDefault(string path)
    {
      PackageInfo il2CppPackage = BaseIl2CppPlatformProvider.FindIl2CppPackage();
      if (PackageInfo.op_Equality(il2CppPackage, (PackageInfo) null))
        return Path.Combine(this.libraryFolder, path);
      string path1 = Path.Combine((string) il2CppPackage.basePath, path);
      if (!File.Exists(path1))
        return Path.Combine(this.libraryFolder, path);
      return path1;
    }

    private static PackageInfo FindIl2CppPackage()
    {
      return ModuleManager.packageManager.get_unityExtensions().FirstOrDefault<PackageInfo>((Func<PackageInfo, bool>) (e => (string) e.name == "IL2CPP"));
    }
  }
}
