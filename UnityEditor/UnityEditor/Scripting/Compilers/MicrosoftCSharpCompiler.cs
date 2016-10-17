// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.MicrosoftCSharpCompiler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor.Utils;
using UnityEditorInternal;

namespace UnityEditor.Scripting.Compilers
{
  internal class MicrosoftCSharpCompiler : ScriptCompilerBase
  {
    private static string[] _uwpReferences;

    internal static string WindowsDirectory
    {
      get
      {
        return Environment.GetEnvironmentVariable("windir");
      }
    }

    internal static string ProgramFilesDirectory
    {
      get
      {
        string environmentVariable = Environment.GetEnvironmentVariable("ProgramFiles(x86)");
        if (Directory.Exists(environmentVariable))
          return environmentVariable;
        UnityEngine.Debug.Log((object) "Env variables ProgramFiles(x86) & ProgramFiles didn't exist, trying hard coded paths");
        string fullPath = Path.GetFullPath(MicrosoftCSharpCompiler.WindowsDirectory + "\\..\\..");
        string path1 = fullPath + "Program Files (x86)";
        string path2 = fullPath + "Program Files";
        if (Directory.Exists(path1))
          return path1;
        if (Directory.Exists(path2))
          return path2;
        throw new Exception("Path '" + path1 + "' or '" + path2 + "' doesn't exist.");
      }
    }

    public MicrosoftCSharpCompiler(MonoIsland island, bool runUpdater)
      : base(island)
    {
    }

    private static ScriptingImplementation GetCurrentScriptingBackend()
    {
      int num = 0;
      if (!PlayerSettings.GetPropertyOptionalInt("ScriptingBackend", ref num, BuildTargetGroup.Metro))
        num = 2;
      return (ScriptingImplementation) num;
    }

    private static string[] GetReferencesFromMonoDistribution()
    {
      return new string[9]{ "mscorlib.dll", "System.dll", "System.Core.dll", "System.Runtime.Serialization.dll", "System.Xml.dll", "System.Xml.Linq.dll", "UnityScript.dll", "UnityScript.Lang.dll", "Boo.Lang.dll" };
    }

    internal static string GetNETCoreFrameworkReferencesDirectory(WSASDK wsaSDK)
    {
      if (MicrosoftCSharpCompiler.GetCurrentScriptingBackend() == ScriptingImplementation.IL2CPP)
        return BuildPipeline.GetMonoLibDirectory(BuildTarget.WSAPlayer);
      switch (wsaSDK)
      {
        case WSASDK.SDK80:
          return MicrosoftCSharpCompiler.ProgramFilesDirectory + "\\Reference Assemblies\\Microsoft\\Framework\\.NETCore\\v4.5";
        case WSASDK.SDK81:
          return MicrosoftCSharpCompiler.ProgramFilesDirectory + "\\Reference Assemblies\\Microsoft\\Framework\\.NETCore\\v4.5.1";
        case WSASDK.PhoneSDK81:
          return MicrosoftCSharpCompiler.ProgramFilesDirectory + "\\Reference Assemblies\\Microsoft\\Framework\\WindowsPhoneApp\\v8.1";
        case WSASDK.UWP:
          return (string) null;
        default:
          throw new Exception("Unknown Windows SDK: " + wsaSDK.ToString());
      }
    }

    internal static bool EnsureProjectLockFile(string projectLockFile)
    {
      return MicrosoftCSharpCompiler.EnsureProjectLockFile(new NuGetPackageResolver() { ProjectLockFile = projectLockFile });
    }

    private static bool EnsureProjectLockFile(NuGetPackageResolver resolver)
    {
      string projectFile = FileUtil.NiceWinPath(Path.Combine(BuildPipeline.GetBuildToolsDirectory(BuildTarget.WSAPlayer), "project.json"));
      return resolver.EnsureProjectLockFile(projectFile);
    }

    private string[] GetNETWSAAssemblies(WSASDK wsaSDK)
    {
      if (MicrosoftCSharpCompiler.GetCurrentScriptingBackend() == ScriptingImplementation.IL2CPP)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: reference to a compiler-generated method
        return ((IEnumerable<string>) MicrosoftCSharpCompiler.GetReferencesFromMonoDistribution()).Select<string, string>(new Func<string, string>(new MicrosoftCSharpCompiler.\u003CGetNETWSAAssemblies\u003Ec__AnonStoreyB3() { monoAssemblyDirectory = BuildPipeline.GetMonoLibDirectory(BuildTarget.WSAPlayer) }.\u003C\u003Em__20C)).ToArray<string>();
      }
      if (wsaSDK != WSASDK.UWP)
        return Directory.GetFiles(MicrosoftCSharpCompiler.GetNETCoreFrameworkReferencesDirectory(wsaSDK), "*.dll");
      NuGetPackageResolver resolver = new NuGetPackageResolver() { ProjectLockFile = "UWP\\project.lock.json" };
      for (int index = !MicrosoftCSharpCompiler.EnsureProjectLockFile(resolver) ? 1 : 2; index != 0; --index)
      {
        try
        {
          resolver.Resolve();
        }
        catch (Exception ex)
        {
          if (index > 1)
          {
            Console.WriteLine("Failed to resolve NuGet packages. Deleting \"{0}\" and retrying.", (object) Path.GetFullPath(resolver.ProjectLockFile));
            File.Delete(resolver.ProjectLockFile);
            MicrosoftCSharpCompiler.EnsureProjectLockFile(resolver);
          }
          else
            throw;
        }
      }
      return resolver.ResolvedReferences;
    }

    private string GetNetWSAAssemblyInfoWindows80()
    {
      return string.Join("\r\n", new string[3]{ "using System;", " using System.Reflection;", "[assembly: global::System.Runtime.Versioning.TargetFrameworkAttribute(\".NETCore,Version=v4.5\", FrameworkDisplayName = \".NET for Windows Store apps\")]" });
    }

    private string GetNetWSAAssemblyInfoWindows81()
    {
      return string.Join("\r\n", new string[3]{ "using System;", " using System.Reflection;", "[assembly: global::System.Runtime.Versioning.TargetFrameworkAttribute(\".NETCore,Version=v4.5.1\", FrameworkDisplayName = \".NET for Windows Store apps (Windows 8.1)\")]" });
    }

    private string GetNetWSAAssemblyInfoWindowsPhone81()
    {
      return string.Join("\r\n", new string[3]{ "using System;", " using System.Reflection;", "[assembly: global::System.Runtime.Versioning.TargetFrameworkAttribute(\"WindowsPhoneApp,Version=v8.1\", FrameworkDisplayName = \"Windows Phone 8.1\")]" });
    }

    private string GetNetWSAAssemblyInfoUWP()
    {
      return string.Join("\r\n", new string[3]{ "using System;", "using System.Reflection;", "[assembly: global::System.Runtime.Versioning.TargetFrameworkAttribute(\".NETCore,Version=v5.0\", FrameworkDisplayName = \".NET for Windows Universal\")]" });
    }

    private void FillNETCoreCompilerOptions(WSASDK wsaSDK, List<string> arguments, ref string argsPrefix)
    {
      argsPrefix = "/noconfig ";
      arguments.Add("/nostdlib+");
      if (MicrosoftCSharpCompiler.GetCurrentScriptingBackend() != ScriptingImplementation.IL2CPP)
        arguments.Add("/define:NETFX_CORE");
      arguments.Add("/preferreduilang:en-US");
      string platformAssemblyPath = MicrosoftCSharpCompiler.GetPlatformAssemblyPath(wsaSDK);
      string str1;
      switch (wsaSDK)
      {
        case WSASDK.SDK80:
          str1 = "8.0";
          break;
        case WSASDK.SDK81:
          str1 = "8.1";
          break;
        case WSASDK.PhoneSDK81:
          str1 = "Phone 8.1";
          break;
        case WSASDK.UWP:
          str1 = "UAP";
          if (MicrosoftCSharpCompiler.GetCurrentScriptingBackend() != ScriptingImplementation.IL2CPP)
          {
            arguments.Add("/define:WINDOWS_UWP");
            break;
          }
          break;
        default:
          throw new Exception("Unknown Windows SDK: " + EditorUserBuildSettings.wsaSDK.ToString());
      }
      if (!File.Exists(platformAssemblyPath))
        throw new Exception(string.Format("'{0}' not found, do you have Windows {1} SDK installed?", (object) platformAssemblyPath, (object) str1));
      arguments.Add("/reference:\"" + platformAssemblyPath + "\"");
      string[] additionalReferences = MicrosoftCSharpCompiler.GetAdditionalReferences(wsaSDK);
      if (additionalReferences != null)
      {
        foreach (string str2 in additionalReferences)
          arguments.Add("/reference:\"" + str2 + "\"");
      }
      foreach (string netwsaAssembly in this.GetNETWSAAssemblies(wsaSDK))
        arguments.Add("/reference:\"" + netwsaAssembly + "\"");
      if (MicrosoftCSharpCompiler.GetCurrentScriptingBackend() == ScriptingImplementation.IL2CPP)
        return;
      string path;
      string contents;
      string path2;
      switch (wsaSDK)
      {
        case WSASDK.SDK80:
          path = Path.Combine(Path.GetTempPath(), ".NETCore,Version=v4.5.AssemblyAttributes.cs");
          contents = this.GetNetWSAAssemblyInfoWindows80();
          path2 = "Managed\\WinRTLegacy.dll";
          break;
        case WSASDK.SDK81:
          path = Path.Combine(Path.GetTempPath(), ".NETCore,Version=v4.5.1.AssemblyAttributes.cs");
          contents = this.GetNetWSAAssemblyInfoWindows81();
          path2 = "Managed\\WinRTLegacy.dll";
          break;
        case WSASDK.PhoneSDK81:
          path = Path.Combine(Path.GetTempPath(), "WindowsPhoneApp,Version=v8.1.AssemblyAttributes.cs");
          contents = this.GetNetWSAAssemblyInfoWindowsPhone81();
          path2 = "Managed\\Phone\\WinRTLegacy.dll";
          break;
        case WSASDK.UWP:
          path = Path.Combine(Path.GetTempPath(), ".NETCore,Version=v5.0.AssemblyAttributes.cs");
          contents = this.GetNetWSAAssemblyInfoUWP();
          path2 = "Managed\\UAP\\WinRTLegacy.dll";
          break;
        default:
          throw new Exception("Unknown Windows SDK: " + EditorUserBuildSettings.wsaSDK.ToString());
      }
      string str3 = Path.Combine(BuildPipeline.GetPlaybackEngineDirectory(this._island._target, BuildOptions.None), path2);
      arguments.Add("/reference:\"" + str3.Replace('/', '\\') + "\"");
      if (File.Exists(path))
        File.Delete(path);
      File.WriteAllText(path, contents);
      arguments.Add(path);
    }

    private Program StartCompilerImpl(List<string> arguments, string argsPrefix, bool msBuildCompiler)
    {
      foreach (string reference in this._island._references)
        arguments.Add("/reference:" + ScriptCompilerBase.PrepareFileName(reference));
      foreach (string str in ((IEnumerable<string>) this._island._defines).Distinct<string>())
        arguments.Add("/define:" + str);
      foreach (string file in this._island._files)
        arguments.Add(ScriptCompilerBase.PrepareFileName(file).Replace('/', '\\'));
      string path = !msBuildCompiler ? Path.Combine(MicrosoftCSharpCompiler.WindowsDirectory, "Microsoft.NET\\Framework\\v4.0.30319\\Csc.exe") : Path.Combine(MicrosoftCSharpCompiler.ProgramFilesDirectory, "MSBuild\\14.0\\Bin\\csc.exe");
      if (!File.Exists(path))
        throw new Exception("'" + path + "' not found, either .NET 4.5 is not installed or your OS is not Windows 8/8.1.");
      this.AddCustomResponseFileIfPresent(arguments, "csc.rsp");
      string responseFile = CommandLineFormatter.GenerateResponseFile((IEnumerable<string>) arguments);
      Program program = new Program(new ProcessStartInfo() { Arguments = argsPrefix + "@" + responseFile, FileName = path, CreateNoWindow = true });
      program.Start();
      return program;
    }

    protected override Program StartCompiler()
    {
      List<string> arguments = new List<string>() { "/target:library", "/nowarn:0169", "/out:" + ScriptCompilerBase.PrepareFileName(this._island._output) };
      arguments.InsertRange(0, (IEnumerable<string>) new string[2]
      {
        "/debug:pdbonly",
        "/optimize+"
      });
      string empty = string.Empty;
      if (this.CompilingForWSA() && (PlayerSettings.WSA.compilationOverrides == PlayerSettings.WSACompilationOverrides.UseNetCore || PlayerSettings.WSA.compilationOverrides == PlayerSettings.WSACompilationOverrides.UseNetCorePartially))
        this.FillNETCoreCompilerOptions(EditorUserBuildSettings.wsaSDK, arguments, ref empty);
      return this.StartCompilerImpl(arguments, empty, EditorUserBuildSettings.wsaSDK == WSASDK.UWP);
    }

    protected override string[] GetStreamContainingCompilerMessages()
    {
      return this.GetStandardOutput();
    }

    protected override CompilerOutputParserBase CreateOutputParser()
    {
      return (CompilerOutputParserBase) new MicrosoftCSharpCompilerOutputParser();
    }

    internal static string GetWindowsKitDirectory(WSASDK wsaSDK)
    {
      string str;
      string path2;
      switch (wsaSDK)
      {
        case WSASDK.SDK80:
          str = RegistryUtil.GetRegistryStringValue32("SOFTWARE\\Microsoft\\Microsoft SDKs\\Windows\\v8.0", "InstallationFolder", (string) null);
          path2 = "Windows Kits\\8.0";
          break;
        case WSASDK.SDK81:
          str = RegistryUtil.GetRegistryStringValue32("SOFTWARE\\Microsoft\\Microsoft SDKs\\Windows\\v8.1", "InstallationFolder", (string) null);
          path2 = "Windows Kits\\8.1";
          break;
        case WSASDK.PhoneSDK81:
          str = RegistryUtil.GetRegistryStringValue32("SOFTWARE\\Microsoft\\Microsoft SDKs\\WindowsPhoneApp\\v8.1", "InstallationFolder", (string) null);
          path2 = "Windows Phone Kits\\8.1";
          break;
        case WSASDK.UWP:
          str = RegistryUtil.GetRegistryStringValue32("SOFTWARE\\Microsoft\\Microsoft SDKs\\Windows\\v10.0", "InstallationFolder", (string) null);
          path2 = "Windows Kits\\10.0";
          break;
        default:
          throw new Exception("Unknown Windows SDK: " + wsaSDK.ToString());
      }
      if (str == null)
        str = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), path2);
      return str;
    }

    internal static string[] GetAdditionalReferences(WSASDK wsaSDK)
    {
      if (wsaSDK != WSASDK.UWP)
        return (string[]) null;
      if (MicrosoftCSharpCompiler._uwpReferences != null)
        return MicrosoftCSharpCompiler._uwpReferences;
      MicrosoftCSharpCompiler._uwpReferences = UWPReferences.GetReferences();
      return MicrosoftCSharpCompiler._uwpReferences;
    }

    protected static string GetPlatformAssemblyPath(WSASDK wsaSDK)
    {
      string windowsKitDirectory = MicrosoftCSharpCompiler.GetWindowsKitDirectory(wsaSDK);
      if (wsaSDK == WSASDK.UWP)
        return Path.Combine(windowsKitDirectory, "UnionMetadata\\Facade\\Windows.winmd");
      return Path.Combine(windowsKitDirectory, "References\\CommonConfiguration\\Neutral\\Windows.winmd");
    }
  }
}
