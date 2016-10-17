// Decompiled with JetBrains decompiler
// Type: UnityEditor.Modules.ModuleManager
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Unity.DataContract;
using UnityEditor.Hardware;
using UnityEditor.Utils;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.Modules
{
  internal static class ModuleManager
  {
    [NonSerialized]
    private static List<IPlatformSupportModule> s_PlatformModules;
    [NonSerialized]
    private static bool s_PlatformModulesInitialized;
    [NonSerialized]
    private static List<IEditorModule> s_EditorModules;
    [NonSerialized]
    private static IPackageManagerModule s_PackageManager;
    [NonSerialized]
    private static IPlatformSupportModule s_ActivePlatformModule;

    internal static IPackageManagerModule packageManager
    {
      get
      {
        ModuleManager.Initialize();
        return ModuleManager.s_PackageManager;
      }
    }

    internal static IEnumerable<IPlatformSupportModule> platformSupportModules
    {
      get
      {
        ModuleManager.Initialize();
        if (ModuleManager.s_PlatformModules == null)
          ModuleManager.RegisterPlatformSupportModules();
        return (IEnumerable<IPlatformSupportModule>) ModuleManager.s_PlatformModules;
      }
    }

    private static List<IEditorModule> editorModules
    {
      get
      {
        if (ModuleManager.s_EditorModules == null)
          return new List<IEditorModule>();
        return ModuleManager.s_EditorModules;
      }
    }

    static ModuleManager()
    {
      EditorUserBuildSettings.activeBuildTargetChanged += new System.Action(ModuleManager.OnActiveBuildTargetChanged);
    }

    private static void OnActiveBuildTargetChanged()
    {
      ModuleManager.ChangeActivePlatformModuleTo(ModuleManager.GetTargetStringFromBuildTarget(EditorUserBuildSettings.activeBuildTarget));
    }

    private static void DeactivateActivePlatformModule()
    {
      if (ModuleManager.s_ActivePlatformModule == null)
        return;
      ModuleManager.s_ActivePlatformModule.OnDeactivate();
      ModuleManager.s_ActivePlatformModule = (IPlatformSupportModule) null;
    }

    private static void ChangeActivePlatformModuleTo(string target)
    {
      ModuleManager.DeactivateActivePlatformModule();
      foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModules)
      {
        if (platformSupportModule.TargetName == target)
        {
          ModuleManager.s_ActivePlatformModule = platformSupportModule;
          platformSupportModule.OnActivate();
          break;
        }
      }
    }

    internal static bool IsRegisteredModule(string file)
    {
      if (ModuleManager.s_PackageManager != null)
        return ((object) ModuleManager.s_PackageManager).GetType().Assembly.Location.NormalizePath() == file.NormalizePath();
      return false;
    }

    internal static bool IsPlatformSupportLoaded(string target)
    {
      foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModules)
      {
        if (platformSupportModule.TargetName == target)
          return true;
      }
      return false;
    }

    internal static void Initialize()
    {
      if (ModuleManager.s_PackageManager != null)
        return;
      ModuleManager.RegisterPackageManager();
      if (ModuleManager.s_PackageManager != null)
        ModuleManager.LoadUnityExtensions();
      else
        UnityEngine.Debug.LogError((object) "Failed to load package manager");
    }

    private static string CombinePaths(params string[] paths)
    {
      if (paths == null)
        throw new ArgumentNullException("paths");
      if (paths.Length == 1)
        return paths[0];
      StringBuilder stringBuilder = new StringBuilder(paths[0]);
      for (int index = 1; index < paths.Length; ++index)
        stringBuilder.AppendFormat("{0}{1}", (object) "/", (object) paths[index]);
      return stringBuilder.ToString();
    }

    public static string RemapDllLocation(string dllLocation)
    {
      string fileName = Path.GetFileName(dllLocation);
      string path = ModuleManager.CombinePaths(Path.GetDirectoryName(dllLocation), "Standalone", fileName);
      if (File.Exists(path))
        return path;
      return dllLocation;
    }

    private static void LoadUnityExtensions()
    {
      using (IEnumerator<PackageInfo> enumerator1 = ModuleManager.s_PackageManager.get_unityExtensions().GetEnumerator())
      {
        while (((IEnumerator) enumerator1).MoveNext())
        {
          PackageInfo current1 = enumerator1.Current;
          Console.WriteLine("Setting {0} v{1} for Unity v{2} to {3}", new object[4]
          {
            (object) current1.name,
            (object) current1.version,
            (object) current1.unityVersion,
            (object) current1.basePath
          });
          using (IEnumerator<KeyValuePair<string, PackageFileData>> enumerator2 = ((IEnumerable<KeyValuePair<string, PackageFileData>>) current1.get_files()).Where<KeyValuePair<string, PackageFileData>>((Func<KeyValuePair<string, PackageFileData>, bool>) (f => f.Value.type == 3)).GetEnumerator())
          {
            while (((IEnumerator) enumerator2).MoveNext())
            {
              KeyValuePair<string, PackageFileData> current2 = enumerator2.Current;
              string str = Path.Combine((string) current1.basePath, current2.Key).NormalizePath();
              if (!File.Exists(str))
              {
                UnityEngine.Debug.LogWarningFormat("Missing assembly \t{0} for {1}. Extension support may be incomplete.", new object[2]
                {
                  (object) current2.Key,
                  (object) current1.name
                });
              }
              else
              {
                bool flag = !string.IsNullOrEmpty((string) current2.Value.guid);
                Console.WriteLine("  {0} ({1}) GUID: {2}", (object) current2.Key, !flag ? (object) "Custom" : (object) "Extension", (object) current2.Value.guid);
                if (flag)
                  InternalEditorUtility.RegisterExtensionDll(str.Replace('\\', '/'), (string) current2.Value.guid);
                else
                  InternalEditorUtility.SetupCustomDll(Path.GetFileName(str), str);
              }
            }
          }
          ModuleManager.s_PackageManager.LoadPackage(current1);
        }
      }
    }

    internal static void InitializePlatformSupportModules()
    {
      if (ModuleManager.s_PlatformModulesInitialized)
      {
        Console.WriteLine("Platform modules already initialized, skipping");
      }
      else
      {
        ModuleManager.Initialize();
        ModuleManager.RegisterPlatformSupportModules();
        foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModules)
        {
          foreach (string nativeLibrary in platformSupportModule.NativeLibraries)
            EditorUtility.LoadPlatformSupportNativeLibrary(nativeLibrary);
          EditorUtility.LoadPlatformSupportModuleNativeDllInternal(platformSupportModule.TargetName);
          platformSupportModule.OnLoad();
        }
        ModuleManager.OnActiveBuildTargetChanged();
        ModuleManager.s_PlatformModulesInitialized = true;
      }
    }

    internal static void ShutdownPlatformSupportModules()
    {
      ModuleManager.DeactivateActivePlatformModule();
      using (List<IPlatformSupportModule>.Enumerator enumerator = ModuleManager.s_PlatformModules.GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.OnUnload();
      }
    }

    internal static void Shutdown()
    {
      if (ModuleManager.s_PackageManager != null)
        ((IEditorModule) ModuleManager.s_PackageManager).Shutdown(true);
      ModuleManager.s_PackageManager = (IPackageManagerModule) null;
      ModuleManager.s_PlatformModules = (List<IPlatformSupportModule>) null;
      ModuleManager.s_EditorModules = (List<IEditorModule>) null;
    }

    private static void RegisterPackageManager()
    {
      ModuleManager.s_EditorModules = new List<IEditorModule>();
      try
      {
        Assembly assembly = ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).FirstOrDefault<Assembly>((Func<Assembly, bool>) (a => null != a.GetType("Unity.PackageManager.PackageManager")));
        if (assembly != null)
        {
          if (ModuleManager.InitializePackageManager(assembly, (PackageInfo) null))
            return;
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine("Error enumerating assemblies looking for package manager. {0}", (object) ex);
      }
      System.Type type = ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).Where<Assembly>((Func<Assembly, bool>) (a => a.GetName().Name == "Unity.Locator")).Select<Assembly, System.Type>((Func<Assembly, System.Type>) (a => a.GetType("Unity.PackageManager.Locator"))).FirstOrDefault<System.Type>();
      try
      {
        type.InvokeMember("Scan", BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod, (Binder) null, (object) null, new object[2]
        {
          (object) new string[1]
          {
            FileUtil.NiceWinPath(EditorApplication.applicationContentsPath)
          },
          (object) Application.unityVersion
        });
      }
      catch (Exception ex)
      {
        Console.WriteLine("Error scanning for packages. {0}", (object) ex);
        return;
      }
      PackageInfo package;
      try
      {
        package = type.InvokeMember("GetPackageManager", BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod, (Binder) null, (object) null, (object[]) new string[1]
        {
          Application.unityVersion
        }) as PackageInfo;
        if (PackageInfo.op_Equality(package, (PackageInfo) null))
        {
          Console.WriteLine("No package manager found!");
          return;
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine("Error scanning for packages. {0}", (object) ex);
        return;
      }
      try
      {
        ModuleManager.InitializePackageManager(package);
      }
      catch (Exception ex)
      {
        Console.WriteLine("Error initializing package manager. {0}", (object) ex);
      }
      if (ModuleManager.s_PackageManager == null)
        return;
      ModuleManager.s_PackageManager.CheckForUpdates();
    }

    private static bool InitializePackageManager(PackageInfo package)
    {
      string str = ((IEnumerable<KeyValuePair<string, PackageFileData>>) package.get_files()).Where<KeyValuePair<string, PackageFileData>>((Func<KeyValuePair<string, PackageFileData>, bool>) (x => x.Value.type == 3)).Select<KeyValuePair<string, PackageFileData>, string>((Func<KeyValuePair<string, PackageFileData>, string>) (x => x.Key)).FirstOrDefault<string>();
      if (str == null || !File.Exists(Path.Combine((string) package.basePath, str)))
        return false;
      InternalEditorUtility.SetPlatformPath((string) package.basePath);
      return ModuleManager.InitializePackageManager(InternalEditorUtility.LoadAssemblyWrapper(Path.GetFileName(str), Path.Combine((string) package.basePath, str)), package);
    }

    private static bool InitializePackageManager(Assembly assembly, PackageInfo package)
    {
      ModuleManager.s_PackageManager = AssemblyHelper.FindImplementors<IPackageManagerModule>(assembly).FirstOrDefault<IPackageManagerModule>();
      if (ModuleManager.s_PackageManager == null)
        return false;
      string location = assembly.Location;
      if (PackageInfo.op_Inequality(package, (PackageInfo) null))
        InternalEditorUtility.SetupCustomDll(Path.GetFileName(location), location);
      else
        package = new PackageInfo()
        {
          basePath = (__Null) Path.GetDirectoryName(location)
        };
      ((IEditorModule) ModuleManager.s_PackageManager).set_moduleInfo(package);
      ModuleManager.s_PackageManager.set_editorInstallPath(EditorApplication.applicationContentsPath);
      ModuleManager.s_PackageManager.set_unityVersion(PackageVersion.op_Implicit(new PackageVersion(Application.unityVersion)));
      ((IEditorModule) ModuleManager.s_PackageManager).Initialize();
      using (IEnumerator<PackageInfo> enumerator1 = ModuleManager.s_PackageManager.get_playbackEngines().GetEnumerator())
      {
        while (((IEnumerator) enumerator1).MoveNext())
        {
          PackageInfo current1 = enumerator1.Current;
          BuildTarget target = BuildTarget.StandaloneWindows;
          if (ModuleManager.TryParseBuildTarget((string) current1.name, out target))
          {
            Console.WriteLine("Setting {0} v{1} for Unity v{2} to {3}", new object[4]
            {
              (object) target,
              (object) current1.version,
              (object) current1.unityVersion,
              (object) current1.basePath
            });
            using (IEnumerator<KeyValuePair<string, PackageFileData>> enumerator2 = ((IEnumerable<KeyValuePair<string, PackageFileData>>) current1.get_files()).Where<KeyValuePair<string, PackageFileData>>((Func<KeyValuePair<string, PackageFileData>, bool>) (f => f.Value.type == 3)).GetEnumerator())
            {
              while (((IEnumerator) enumerator2).MoveNext())
              {
                KeyValuePair<string, PackageFileData> current2 = enumerator2.Current;
                if (!File.Exists(Path.Combine((string) current1.basePath, current2.Key).NormalizePath()))
                  UnityEngine.Debug.LogWarningFormat("Missing assembly \t{0} for {1}. Player support may be incomplete.", new object[2]
                  {
                    (object) current1.basePath,
                    (object) current1.name
                  });
                else
                  InternalEditorUtility.SetupCustomDll(Path.GetFileName(location), location);
              }
            }
            BuildPipeline.SetPlaybackEngineDirectory(target, BuildOptions.None, (string) current1.basePath);
            InternalEditorUtility.SetPlatformPath((string) current1.basePath);
            ModuleManager.s_PackageManager.LoadPackage(current1);
          }
        }
      }
      return true;
    }

    private static bool TryParseBuildTarget(string targetString, out BuildTarget target)
    {
      target = BuildTarget.StandaloneWindows;
      try
      {
        target = (BuildTarget) Enum.Parse(typeof (BuildTarget), targetString);
        return true;
      }
      catch
      {
        UnityEngine.Debug.LogWarning((object) string.Format("Couldn't find build target for {0}", (object) targetString));
      }
      return false;
    }

    private static void RegisterPlatformSupportModules()
    {
      if (ModuleManager.s_PlatformModules != null)
      {
        Console.WriteLine("Modules already registered, not loading");
      }
      else
      {
        Console.WriteLine("Registering platform support modules:");
        Stopwatch stopwatch = Stopwatch.StartNew();
        ModuleManager.s_PlatformModules = ModuleManager.RegisterModulesFromLoadedAssemblies<IPlatformSupportModule>(new Func<Assembly, IEnumerable<IPlatformSupportModule>>(ModuleManager.RegisterPlatformSupportModulesFromAssembly)).ToList<IPlatformSupportModule>();
        stopwatch.Stop();
        Console.WriteLine("Registered platform support modules in: " + (object) stopwatch.Elapsed.TotalSeconds + "s.");
      }
    }

    private static IEnumerable<T> RegisterModulesFromLoadedAssemblies<T>(Func<Assembly, IEnumerable<T>> processAssembly)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ModuleManager.\u003CRegisterModulesFromLoadedAssemblies\u003Ec__AnonStoreyA5<T> assembliesCAnonStoreyA5 = new ModuleManager.\u003CRegisterModulesFromLoadedAssemblies\u003Ec__AnonStoreyA5<T>();
      // ISSUE: reference to a compiler-generated field
      assembliesCAnonStoreyA5.processAssembly = processAssembly;
      // ISSUE: reference to a compiler-generated field
      if (assembliesCAnonStoreyA5.processAssembly == null)
        throw new ArgumentNullException("processAssembly");
      // ISSUE: reference to a compiler-generated method
      return (IEnumerable<T>) ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).Aggregate<Assembly, List<T>>(new List<T>(), new Func<List<T>, Assembly, List<T>>(assembliesCAnonStoreyA5.\u003C\u003Em__1E2));
    }

    internal static IEnumerable<IPlatformSupportModule> RegisterPlatformSupportModulesFromAssembly(Assembly assembly)
    {
      return AssemblyHelper.FindImplementors<IPlatformSupportModule>(assembly);
    }

    private static IEnumerable<IEditorModule> RegisterEditorModulesFromAssembly(Assembly assembly)
    {
      return AssemblyHelper.FindImplementors<IEditorModule>(assembly);
    }

    internal static List<string> GetJamTargets()
    {
      List<string> stringList = new List<string>();
      foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModules)
        stringList.Add(platformSupportModule.JamTarget);
      return stringList;
    }

    private static IPlatformSupportModule FindPlatformSupportModule(string moduleName)
    {
      foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModules)
      {
        if (platformSupportModule.TargetName == moduleName)
          return platformSupportModule;
      }
      return (IPlatformSupportModule) null;
    }

    internal static IDevice GetDevice(string deviceId)
    {
      DevDevice device;
      if (!DevDeviceList.FindDevice(deviceId, out device))
        throw new ApplicationException("Couldn't create device API for device: " + deviceId);
      IPlatformSupportModule platformSupportModule = ModuleManager.FindPlatformSupportModule(device.module);
      if (platformSupportModule != null)
        return platformSupportModule.CreateDevice(deviceId);
      throw new ApplicationException("Couldn't find module for target: " + device.module);
    }

    internal static IUserAssembliesValidator GetUserAssembliesValidator(string target)
    {
      if (target == null)
        return (IUserAssembliesValidator) null;
      foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModules)
      {
        if (platformSupportModule.TargetName == target)
          return platformSupportModule.CreateUserAssembliesValidatorExtension();
      }
      return (IUserAssembliesValidator) null;
    }

    internal static IBuildPostprocessor GetBuildPostProcessor(string target)
    {
      if (target == null)
        return (IBuildPostprocessor) null;
      foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModules)
      {
        if (platformSupportModule.TargetName == target)
          return platformSupportModule.CreateBuildPostprocessor();
      }
      return (IBuildPostprocessor) null;
    }

    internal static IBuildPostprocessor GetBuildPostProcessor(BuildTarget target)
    {
      return ModuleManager.GetBuildPostProcessor(ModuleManager.GetTargetStringFromBuildTarget(target));
    }

    internal static ISettingEditorExtension GetEditorSettingsExtension(string target)
    {
      if (string.IsNullOrEmpty(target))
        return (ISettingEditorExtension) null;
      foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModules)
      {
        if (platformSupportModule.TargetName == target)
          return platformSupportModule.CreateSettingsEditorExtension();
      }
      return (ISettingEditorExtension) null;
    }

    internal static List<IPreferenceWindowExtension> GetPreferenceWindowExtensions()
    {
      List<IPreferenceWindowExtension> preferenceWindowExtensionList = new List<IPreferenceWindowExtension>();
      foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModules)
      {
        IPreferenceWindowExtension preferenceWindowExtension = platformSupportModule.CreatePreferenceWindowExtension();
        if (preferenceWindowExtension != null)
          preferenceWindowExtensionList.Add(preferenceWindowExtension);
      }
      return preferenceWindowExtensionList;
    }

    internal static IBuildWindowExtension GetBuildWindowExtension(string target)
    {
      if (string.IsNullOrEmpty(target))
        return (IBuildWindowExtension) null;
      foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModules)
      {
        if (platformSupportModule.TargetName == target)
          return platformSupportModule.CreateBuildWindowExtension();
      }
      return (IBuildWindowExtension) null;
    }

    internal static ICompilationExtension GetCompilationExtension(string target)
    {
      foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModules)
      {
        if (platformSupportModule.TargetName == target)
          return platformSupportModule.CreateCompilationExtension();
      }
      return (ICompilationExtension) new DefaultCompilationExtension();
    }

    private static IScriptingImplementations GetScriptingImplementations(string target)
    {
      if (string.IsNullOrEmpty(target))
        return (IScriptingImplementations) null;
      foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModules)
      {
        if (platformSupportModule.TargetName == target)
          return platformSupportModule.CreateScriptingImplementations();
      }
      return (IScriptingImplementations) null;
    }

    internal static IScriptingImplementations GetScriptingImplementations(BuildTargetGroup target)
    {
      if (target == BuildTargetGroup.Standalone)
        return (IScriptingImplementations) new DesktopStandalonePostProcessor.ScriptingImplementations();
      return ModuleManager.GetScriptingImplementations(ModuleManager.GetTargetStringFromBuildTargetGroup(target));
    }

    internal static IPluginImporterExtension GetPluginImporterExtension(string target)
    {
      if (target == null)
        return (IPluginImporterExtension) null;
      foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModules)
      {
        if (platformSupportModule.TargetName == target)
          return platformSupportModule.CreatePluginImporterExtension();
      }
      return (IPluginImporterExtension) null;
    }

    internal static IPluginImporterExtension GetPluginImporterExtension(BuildTarget target)
    {
      return ModuleManager.GetPluginImporterExtension(ModuleManager.GetTargetStringFromBuildTarget(target));
    }

    internal static IPluginImporterExtension GetPluginImporterExtension(BuildTargetGroup target)
    {
      return ModuleManager.GetPluginImporterExtension(ModuleManager.GetTargetStringFromBuildTargetGroup(target));
    }

    internal static string GetTargetStringFromBuildTarget(BuildTarget target)
    {
      switch (target)
      {
        case BuildTarget.StandaloneOSXUniversal:
        case BuildTarget.StandaloneOSXIntel:
        case BuildTarget.StandaloneOSXIntel64:
          return "OSXStandalone";
        case BuildTarget.StandaloneWindows:
        case BuildTarget.StandaloneGLESEmu:
        case BuildTarget.StandaloneWindows64:
          return "WindowsStandalone";
        case BuildTarget.iOS:
          return "iOS";
        case BuildTarget.PS3:
          return "PS3";
        case BuildTarget.XBOX360:
          return "Xbox360";
        case BuildTarget.Android:
          return "Android";
        case BuildTarget.StandaloneLinux:
        case BuildTarget.StandaloneLinux64:
        case BuildTarget.StandaloneLinuxUniversal:
          return "LinuxStandalone";
        case BuildTarget.WebGL:
          return "WebGL";
        case BuildTarget.WSAPlayer:
          return "Metro";
        case BuildTarget.WP8Player:
          return "WP8";
        case BuildTarget.BlackBerry:
          return "BlackBerry";
        case BuildTarget.Tizen:
          return "Tizen";
        case BuildTarget.PSP2:
          return "PSP2";
        case BuildTarget.PS4:
          return "PS4";
        case BuildTarget.PSM:
          return "PSM";
        case BuildTarget.XboxOne:
          return "XboxOne";
        case BuildTarget.SamsungTV:
          return "SamsungTV";
        case BuildTarget.Nintendo3DS:
          return "N3DS";
        case BuildTarget.WiiU:
          return "WiiU";
        case BuildTarget.tvOS:
          return "tvOS";
        default:
          return (string) null;
      }
    }

    internal static string GetTargetStringFromBuildTargetGroup(BuildTargetGroup target)
    {
      switch (target)
      {
        case BuildTargetGroup.iPhone:
          return "iOS";
        case BuildTargetGroup.PS3:
          return "PS3";
        case BuildTargetGroup.XBOX360:
          return "Xbox360";
        case BuildTargetGroup.Android:
          return "Android";
        case BuildTargetGroup.WebGL:
          return "WebGL";
        case BuildTargetGroup.Metro:
          return "Metro";
        case BuildTargetGroup.WP8:
          return "WP8";
        case BuildTargetGroup.BlackBerry:
          return "BlackBerry";
        case BuildTargetGroup.Tizen:
          return "Tizen";
        case BuildTargetGroup.PSP2:
          return "PSP2";
        case BuildTargetGroup.PS4:
          return "PS4";
        case BuildTargetGroup.PSM:
          return "PSM";
        case BuildTargetGroup.XboxOne:
          return "XboxOne";
        case BuildTargetGroup.SamsungTV:
          return "SamsungTV";
        case BuildTargetGroup.Nintendo3DS:
          return "N3DS";
        case BuildTargetGroup.WiiU:
          return "WiiU";
        case BuildTargetGroup.tvOS:
          return "tvOS";
        default:
          return (string) null;
      }
    }

    internal static bool IsPlatformSupported(BuildTarget target)
    {
      return ModuleManager.GetTargetStringFromBuildTarget(target) != null;
    }

    internal static bool HaveLicenseForBuildTarget(string targetString)
    {
      BuildTarget target = BuildTarget.StandaloneWindows;
      if (!ModuleManager.TryParseBuildTarget(targetString, out target))
        return false;
      return BuildPipeline.LicenseCheck(target);
    }

    internal static string GetExtensionVersion(string target)
    {
      if (string.IsNullOrEmpty(target))
        return (string) null;
      foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModules)
      {
        if (platformSupportModule.TargetName == target)
          return platformSupportModule.ExtensionVersion;
      }
      return (string) null;
    }

    internal static GUIContent[] GetDisplayNames(string target)
    {
      if (string.IsNullOrEmpty(target))
        return (GUIContent[]) null;
      foreach (IPlatformSupportModule platformSupportModule in ModuleManager.platformSupportModules)
      {
        if (platformSupportModule.TargetName == target)
          return platformSupportModule.GetDisplayNames();
      }
      return (GUIContent[]) null;
    }
  }
}
