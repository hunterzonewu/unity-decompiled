// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.IL2CPPBuilder
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Scripting.Compilers;

namespace UnityEditorInternal
{
  internal class IL2CPPBuilder
  {
    private readonly LinkXmlReader m_linkXmlReader = new LinkXmlReader();
    private readonly string m_TempFolder;
    private readonly string m_StagingAreaData;
    private readonly IIl2CppPlatformProvider m_PlatformProvider;
    private readonly System.Action<string> m_ModifyOutputBeforeCompile;
    private readonly RuntimeClassRegistry m_RuntimeClassRegistry;
    private readonly bool m_DevelopmentBuild;

    public IL2CPPBuilder(string tempFolder, string stagingAreaData, IIl2CppPlatformProvider platformProvider, System.Action<string> modifyOutputBeforeCompile, RuntimeClassRegistry runtimeClassRegistry, bool developmentBuild)
    {
      this.m_TempFolder = tempFolder;
      this.m_StagingAreaData = stagingAreaData;
      this.m_PlatformProvider = platformProvider;
      this.m_ModifyOutputBeforeCompile = modifyOutputBeforeCompile;
      this.m_RuntimeClassRegistry = runtimeClassRegistry;
      this.m_DevelopmentBuild = developmentBuild;
    }

    public void Run()
    {
      string directoryInStagingArea = this.GetCppOutputDirectoryInStagingArea();
      string fullPath = Path.GetFullPath(Path.Combine(this.m_StagingAreaData, "Managed"));
      foreach (string file in Directory.GetFiles(fullPath))
        new FileInfo(file).IsReadOnly = false;
      AssemblyStripper.StripAssemblies(this.m_StagingAreaData, this.m_PlatformProvider, this.m_RuntimeClassRegistry, this.m_DevelopmentBuild);
      this.ConvertPlayerDlltoCpp((ICollection<string>) this.GetUserAssembliesToConvert(fullPath), directoryInStagingArea, fullPath);
      if (this.m_ModifyOutputBeforeCompile != null)
        this.m_ModifyOutputBeforeCompile(directoryInStagingArea);
      if (this.m_PlatformProvider.CreateNativeCompiler() == null)
        return;
      string str = Path.Combine(this.m_StagingAreaData, "Native");
      Directory.CreateDirectory(str);
      this.m_PlatformProvider.CreateNativeCompiler().CompileDynamicLibrary(Path.Combine(str, this.m_PlatformProvider.nativeLibraryFileName), NativeCompiler.AllSourceFilesIn(directoryInStagingArea), (IEnumerable<string>) new List<string>((IEnumerable<string>) this.m_PlatformProvider.includePaths)
      {
        directoryInStagingArea
      }, (IEnumerable<string>) this.m_PlatformProvider.libraryPaths, (IEnumerable<string>) new string[0]);
    }

    internal List<string> GetUserAssembliesToConvert(string managedDir)
    {
      HashSet<string> userAssemblies = this.GetUserAssemblies(managedDir);
      userAssemblies.Add(((IEnumerable<string>) Directory.GetFiles(managedDir, "UnityEngine.dll", SearchOption.TopDirectoryOnly)).Single<string>());
      userAssemblies.UnionWith(this.FilterUserAssemblies((IEnumerable<string>) Directory.GetFiles(managedDir, "*.dll", SearchOption.TopDirectoryOnly), new Predicate<string>(this.m_linkXmlReader.IsDLLUsed), managedDir));
      return userAssemblies.ToList<string>();
    }

    private HashSet<string> GetUserAssemblies(string managedDir)
    {
      HashSet<string> stringSet = new HashSet<string>();
      stringSet.UnionWith(this.FilterUserAssemblies((IEnumerable<string>) this.m_RuntimeClassRegistry.GetUserAssemblies(), new Predicate<string>(this.m_RuntimeClassRegistry.IsDLLUsed), managedDir));
      stringSet.UnionWith(this.FilterUserAssemblies((IEnumerable<string>) Directory.GetFiles(managedDir, "I18N*.dll", SearchOption.TopDirectoryOnly), (Predicate<string>) (assembly => true), managedDir));
      return stringSet;
    }

    private IEnumerable<string> FilterUserAssemblies(IEnumerable<string> assemblies, Predicate<string> isUsed, string managedDir)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      IL2CPPBuilder.\u003CFilterUserAssemblies\u003Ec__AnonStorey74 assembliesCAnonStorey74 = new IL2CPPBuilder.\u003CFilterUserAssemblies\u003Ec__AnonStorey74();
      // ISSUE: reference to a compiler-generated field
      assembliesCAnonStorey74.isUsed = isUsed;
      // ISSUE: reference to a compiler-generated field
      assembliesCAnonStorey74.managedDir = managedDir;
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      return assemblies.Where<string>(new Func<string, bool>(assembliesCAnonStorey74.\u003C\u003Em__102)).Select<string, string>(new Func<string, string>(assembliesCAnonStorey74.\u003C\u003Em__103));
    }

    public string GetCppOutputDirectoryInStagingArea()
    {
      return IL2CPPBuilder.GetCppOutputPath(this.m_TempFolder);
    }

    public static string GetCppOutputPath(string tempFolder)
    {
      return Path.Combine(tempFolder, "il2cppOutput");
    }

    private void ConvertPlayerDlltoCpp(ICollection<string> userAssemblies, string outputDirectory, string workingDirectory)
    {
      FileUtil.CreateOrCleanDirectory(outputDirectory);
      if (userAssemblies.Count == 0)
        return;
      string[] array = ((IEnumerable<string>) Directory.GetFiles("Assets", "il2cpp_extra_types.txt", SearchOption.AllDirectories)).Select<string, string>((Func<string, string>) (s => Path.Combine(Directory.GetCurrentDirectory(), s))).ToArray<string>();
      string il2CppExe = this.GetIl2CppExe();
      List<string> source1 = new List<string>();
      source1.Add("--convert-to-cpp");
      if (this.m_PlatformProvider.emitNullChecks)
        source1.Add("--emit-null-checks");
      if (this.m_PlatformProvider.enableStackTraces)
        source1.Add("--enable-stacktrace");
      if (this.m_PlatformProvider.enableArrayBoundsCheck)
        source1.Add("--enable-array-bounds-check");
      if (this.m_PlatformProvider.loadSymbols)
        source1.Add("--enable-symbol-loading");
      if (this.m_PlatformProvider.developmentMode)
        source1.Add("--development-mode");
      if (array.Length > 0)
      {
        foreach (string str in array)
          source1.Add(string.Format("--extra-types.file=\"{0}\"", (object) str));
      }
      string path = Path.Combine(this.m_PlatformProvider.il2CppFolder, "il2cpp_default_extra_types.txt");
      if (File.Exists(path))
        source1.Add(string.Format("--extra-types.file=\"{0}\"", (object) path));
      string empty = string.Empty;
      if (PlayerSettings.GetPropertyOptionalString("additionalIl2CppArgs", ref empty))
        source1.Add(empty);
      string environmentVariable = Environment.GetEnvironmentVariable("IL2CPP_ADDITIONAL_ARGS");
      if (!string.IsNullOrEmpty(environmentVariable))
        source1.Add(environmentVariable);
      List<string> source2 = new List<string>((IEnumerable<string>) userAssemblies);
      source1.AddRange(source2.Select<string, string>((Func<string, string>) (arg => "--assembly=\"" + Path.GetFullPath(arg) + "\"")));
      source1.Add(string.Format("--generatedcppdir=\"{0}\"", (object) Path.GetFullPath(outputDirectory)));
      string args = source1.Aggregate<string, string>(string.Empty, (Func<string, string, string>) ((current, arg) => current + arg + " "));
      Console.WriteLine("Invoking il2cpp with arguments: " + args);
      if (EditorUtility.DisplayCancelableProgressBar("Building Player", "Converting managed assemblies to C++", 0.3f))
        throw new OperationCanceledException();
      Runner.RunManagedProgram(il2CppExe, args, workingDirectory, (CompilerOutputParserBase) new Il2CppOutputParser());
    }

    private string GetIl2CppExe()
    {
      return this.m_PlatformProvider.il2CppFolder + "/build/il2cpp.exe";
    }
  }
}
