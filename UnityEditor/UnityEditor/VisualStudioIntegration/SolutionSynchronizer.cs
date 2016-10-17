// Decompiled with JetBrains decompiler
// Type: UnityEditor.VisualStudioIntegration.SolutionSynchronizer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using UnityEditor.Scripting;
using UnityEditorInternal;

namespace UnityEditor.VisualStudioIntegration
{
  internal class SolutionSynchronizer
  {
    public static readonly ISolutionSynchronizationSettings DefaultSynchronizationSettings = (ISolutionSynchronizationSettings) new DefaultSolutionSynchronizationSettings();
    private static readonly string WindowsNewline = "\r\n";
    internal static readonly Dictionary<string, ScriptingLanguage> BuiltinSupportedExtensions = new Dictionary<string, ScriptingLanguage>() { { "cs", ScriptingLanguage.CSharp }, { "js", ScriptingLanguage.UnityScript }, { "boo", ScriptingLanguage.Boo }, { "shader", ScriptingLanguage.None }, { "compute", ScriptingLanguage.None }, { "cginc", ScriptingLanguage.None }, { "glslinc", ScriptingLanguage.None } };
    private static readonly Dictionary<ScriptingLanguage, string> ProjectExtensions = new Dictionary<ScriptingLanguage, string>() { { ScriptingLanguage.Boo, ".booproj" }, { ScriptingLanguage.CSharp, ".csproj" }, { ScriptingLanguage.UnityScript, ".unityproj" }, { ScriptingLanguage.None, ".csproj" } };
    private static readonly Regex _MonoDevelopPropertyHeader = new Regex("^\\s*GlobalSection\\(MonoDevelopProperties.*\\)");
    public static readonly string MSBuildNamespaceUri = "http://schemas.microsoft.com/developer/msbuild/2003";
    private static readonly string DefaultMonoDevelopSolutionProperties = string.Join("\r\n", new string[3]{ "    GlobalSection(MonoDevelopProperties) = preSolution", "        StartupItem = Assembly-CSharp.csproj", "    EndGlobalSection" }).Replace("    ", "\t");
    public static readonly Regex scriptReferenceExpression = new Regex("^Library.ScriptAssemblies.(?<project>Assembly-(?<language>[^-]+)(?<editor>-Editor)?(?<firstpass>-firstpass)?).dll$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static string[] InternalAssembliesIncludedIntoEditorProject = new string[2]{ "/UnityEditor.iOS.Extensions.Common.dll", "/UnityEditor.iOS.Extensions.Xcode.dll" };
    private string[] ProjectSupportedExtensions = new string[0];
    private readonly string _projectDirectory;
    private readonly ISolutionSynchronizationSettings _settings;
    private readonly string _projectName;

    public SolutionSynchronizer(string projectDirectory, ISolutionSynchronizationSettings settings)
    {
      this._projectDirectory = projectDirectory;
      this._settings = settings;
      this._projectName = Path.GetFileName(this._projectDirectory);
    }

    public SolutionSynchronizer(string projectDirectory)
      : this(projectDirectory, SolutionSynchronizer.DefaultSynchronizationSettings)
    {
    }

    private void SetupProjectSupportedExtensions()
    {
      this.ProjectSupportedExtensions = EditorSettings.projectGenerationUserExtensions;
    }

    public bool ShouldFileBePartOfSolution(string file)
    {
      string extension = Path.GetExtension(file);
      if (extension == ".dll")
        return true;
      return this.IsSupportedExtension(extension);
    }

    private bool IsSupportedExtension(string extension)
    {
      extension = extension.TrimStart('.');
      return SolutionSynchronizer.BuiltinSupportedExtensions.ContainsKey(extension) || ((IEnumerable<string>) this.ProjectSupportedExtensions).Contains<string>(extension);
    }

    private static ScriptingLanguage ScriptingLanguageFor(MonoIsland island)
    {
      return SolutionSynchronizer.ScriptingLanguageFor(island.GetExtensionOfSourceFiles());
    }

    private static ScriptingLanguage ScriptingLanguageFor(string extension)
    {
      ScriptingLanguage scriptingLanguage;
      if (SolutionSynchronizer.BuiltinSupportedExtensions.TryGetValue(extension.TrimStart('.'), out scriptingLanguage))
        return scriptingLanguage;
      return ScriptingLanguage.None;
    }

    public bool ProjectExists(MonoIsland island)
    {
      return File.Exists(this.ProjectFile(island));
    }

    public bool SolutionExists()
    {
      return File.Exists(this.SolutionFile());
    }

    private static void DumpIsland(MonoIsland island)
    {
      Console.WriteLine("{0} ({1})", (object) island._output, (object) island._classlib_profile);
      Console.WriteLine("Files: ");
      Console.WriteLine(string.Join("\n", island._files));
      Console.WriteLine("References: ");
      Console.WriteLine(string.Join("\n", island._references));
      Console.WriteLine(string.Empty);
    }

    public bool SyncIfNeeded(IEnumerable<string> affectedFiles)
    {
      this.SetupProjectSupportedExtensions();
      if (!this.SolutionExists() || !affectedFiles.Any<string>(new Func<string, bool>(this.ShouldFileBePartOfSolution)))
        return false;
      this.Sync();
      return true;
    }

    public void Sync()
    {
      this.SetupProjectSupportedExtensions();
      if (AssetPostprocessingInternal.OnPreGeneratingCSProjectFiles())
        return;
      IEnumerable<MonoIsland> islands = ((IEnumerable<MonoIsland>) InternalEditorUtility.GetMonoIslands()).Where<MonoIsland>((Func<MonoIsland, bool>) (i => 0 < i._files.Length));
      string assetProjectPart = this.GenerateAllAssetProjectPart();
      this.SyncSolution(islands);
      foreach (MonoIsland island in SolutionSynchronizer.RelevantIslandsForMode(islands, SolutionSynchronizer.ModeForCurrentExternalEditor()))
        this.SyncProject(island, assetProjectPart);
      AssetPostprocessingInternal.CallOnGeneratedCSProjectFiles();
    }

    private string GenerateAllAssetProjectPart()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string allAssetPath in AssetDatabase.GetAllAssetPaths())
      {
        string extension = Path.GetExtension(allAssetPath);
        if (this.IsSupportedExtension(extension) && SolutionSynchronizer.ScriptingLanguageFor(extension) == ScriptingLanguage.None)
          stringBuilder.AppendFormat("     <None Include=\"{0}\" />{1}", (object) this.EscapedRelativePathFor(allAssetPath), (object) SolutionSynchronizer.WindowsNewline);
      }
      return stringBuilder.ToString();
    }

    private void SyncProject(MonoIsland island, string otherAssetsProjectPart)
    {
      SolutionSynchronizer.SyncFileIfNotChanged(this.ProjectFile(island), this.ProjectText(island, SolutionSynchronizer.ModeForCurrentExternalEditor(), otherAssetsProjectPart));
    }

    private static void SyncFileIfNotChanged(string filename, string newContents)
    {
      if (File.Exists(filename) && newContents == File.ReadAllText(filename))
        return;
      File.WriteAllText(filename, newContents);
    }

    private static bool IsInternalAssemblyThatShouldBeReferenced(bool isBuildingEditorProject, string reference)
    {
      if (!isBuildingEditorProject)
        return false;
      foreach (string str in SolutionSynchronizer.InternalAssembliesIncludedIntoEditorProject)
      {
        if (reference.EndsWith(str))
          return true;
      }
      return false;
    }

    private string ProjectText(MonoIsland island, SolutionSynchronizer.Mode mode, string allAssetsProject)
    {
      StringBuilder stringBuilder = new StringBuilder(this.ProjectHeader(island));
      List<string> first = new List<string>();
      List<Match> matchList = new List<Match>();
      bool isBuildingEditorProject = island._output.EndsWith("-Editor.dll");
      foreach (string file1 in island._files)
      {
        string lower = Path.GetExtension(file1).ToLower();
        string file2 = !Path.IsPathRooted(file1) ? Path.Combine(this._projectDirectory, file1) : file1;
        if (".dll" != lower)
        {
          string str = "Compile";
          stringBuilder.AppendFormat("     <{0} Include=\"{1}\" />{2}", (object) str, (object) this.EscapedRelativePathFor(file2), (object) SolutionSynchronizer.WindowsNewline);
        }
        else
          first.Add(file2);
      }
      stringBuilder.Append(allAssetsProject);
      foreach (string str1 in first.Union<string>((IEnumerable<string>) island._references))
      {
        if (!str1.EndsWith("/UnityEditor.dll") && !str1.EndsWith("/UnityEngine.dll") && (!str1.EndsWith("\\UnityEditor.dll") && !str1.EndsWith("\\UnityEngine.dll")))
        {
          Match match = SolutionSynchronizer.scriptReferenceExpression.Match(str1);
          if (match.Success && (mode == SolutionSynchronizer.Mode.UnityScriptAsUnityProj || (int) Enum.Parse(typeof (ScriptingLanguage), match.Groups["language"].Value, true) == 2))
          {
            matchList.Add(match);
          }
          else
          {
            string str2 = !Path.IsPathRooted(str1) ? Path.Combine(this._projectDirectory, str1) : str1;
            if (AssemblyHelper.IsManagedAssembly(str2) && (!AssemblyHelper.IsInternalAssembly(str2) || SolutionSynchronizer.IsInternalAssemblyThatShouldBeReferenced(isBuildingEditorProject, str2)))
            {
              string path = str2.Replace("\\", "/").Replace("\\\\", "/");
              stringBuilder.AppendFormat(" <Reference Include=\"{0}\">{1}", (object) Path.GetFileNameWithoutExtension(path), (object) SolutionSynchronizer.WindowsNewline);
              stringBuilder.AppendFormat(" <HintPath>{0}</HintPath>{1}", (object) path, (object) SolutionSynchronizer.WindowsNewline);
              stringBuilder.AppendFormat(" </Reference>{0}", (object) SolutionSynchronizer.WindowsNewline);
            }
          }
        }
      }
      if (0 < matchList.Count)
      {
        stringBuilder.AppendLine("  </ItemGroup>");
        stringBuilder.AppendLine("  <ItemGroup>");
        using (List<Match>.Enumerator enumerator = matchList.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Match current = enumerator.Current;
            string str = current.Groups["project"].Value;
            stringBuilder.AppendFormat("    <ProjectReference Include=\"{0}{1}\">{2}", (object) str, (object) SolutionSynchronizer.GetProjectExtension((ScriptingLanguage) Enum.Parse(typeof (ScriptingLanguage), current.Groups["language"].Value, true)), (object) SolutionSynchronizer.WindowsNewline);
            stringBuilder.AppendFormat("      <Project>{{{0}}}</Project>", (object) this.ProjectGuid(Path.Combine("Temp", current.Groups["project"].Value + ".dll")), (object) SolutionSynchronizer.WindowsNewline);
            stringBuilder.AppendFormat("      <Name>{0}</Name>", (object) str, (object) SolutionSynchronizer.WindowsNewline);
            stringBuilder.AppendLine("    </ProjectReference>");
          }
        }
      }
      stringBuilder.Append(this.ProjectFooter(island));
      return stringBuilder.ToString();
    }

    public string ProjectFile(MonoIsland island)
    {
      ScriptingLanguage index = SolutionSynchronizer.ScriptingLanguageFor(island);
      return Path.Combine(this._projectDirectory, string.Format("{0}{1}", (object) Path.GetFileNameWithoutExtension(island._output), (object) SolutionSynchronizer.ProjectExtensions[index]));
    }

    internal string SolutionFile()
    {
      return Path.Combine(this._projectDirectory, string.Format("{0}.sln", (object) this._projectName));
    }

    private string ProjectHeader(MonoIsland island)
    {
      string str1 = "4.0";
      string str2 = "10.0.20506";
      ScriptingLanguage language = SolutionSynchronizer.ScriptingLanguageFor(island);
      if (this._settings.VisualStudioVersion == 9)
      {
        str1 = "3.5";
        str2 = "9.0.21022";
      }
      object[] objArray = new object[9]{ (object) str1, (object) str2, (object) this.ProjectGuid(island._output), (object) this._settings.EngineAssemblyPath, (object) this._settings.EditorAssemblyPath, (object) string.Join(";", ((IEnumerable<string>) new string[2]{ "DEBUG", "TRACE" }).Concat<string>((IEnumerable<string>) this._settings.Defines).Concat<string>((IEnumerable<string>) island._defines).Distinct<string>().ToArray<string>()), (object) SolutionSynchronizer.MSBuildNamespaceUri, (object) Path.GetFileNameWithoutExtension(island._output), (object) EditorSettings.projectGenerationRootNamespace };
      try
      {
        return string.Format(this._settings.GetProjectHeaderTemplate(language), objArray);
      }
      catch (Exception ex)
      {
        throw new NotSupportedException("Failed creating c# project because the c# project header did not have the correct amount of arguments, which is " + (object) objArray.Length);
      }
    }

    private void SyncSolution(IEnumerable<MonoIsland> islands)
    {
      SolutionSynchronizer.SyncFileIfNotChanged(this.SolutionFile(), this.SolutionText(islands, SolutionSynchronizer.ModeForCurrentExternalEditor()));
    }

    private static SolutionSynchronizer.Mode ModeForCurrentExternalEditor()
    {
      return SolutionSynchronizer.IsSelectedEditorVisualStudio() || !SolutionSynchronizer.IsSelectedEditorInternalMonoDevelop() && !EditorPrefs.GetBool("kExternalEditorSupportsUnityProj", false) ? SolutionSynchronizer.Mode.UntiyScriptAsPrecompiledAssembly : SolutionSynchronizer.Mode.UnityScriptAsUnityProj;
    }

    private static bool IsSelectedEditorVisualStudio()
    {
      string externalScriptEditor = InternalEditorUtility.GetExternalScriptEditor();
      if (!externalScriptEditor.EndsWith("devenv.exe"))
        return externalScriptEditor.EndsWith("vcsexpress.exe");
      return true;
    }

    private static bool IsSelectedEditorInternalMonoDevelop()
    {
      return InternalEditorUtility.GetExternalScriptEditor() == "internal";
    }

    private string SolutionText(IEnumerable<MonoIsland> islands, SolutionSynchronizer.Mode mode)
    {
      string str1 = "11.00";
      if (this._settings.VisualStudioVersion == 9)
        str1 = "10.00";
      IEnumerable<MonoIsland> monoIslands = SolutionSynchronizer.RelevantIslandsForMode(islands, mode);
      string projectEntries = this.GetProjectEntries(monoIslands);
      string str2 = string.Join(SolutionSynchronizer.WindowsNewline, monoIslands.Select<MonoIsland, string>((Func<MonoIsland, string>) (i => this.GetProjectActiveConfigurations(this.ProjectGuid(i._output)))).ToArray<string>());
      return string.Format(this._settings.SolutionTemplate, (object) str1, (object) projectEntries, (object) str2, (object) this.ReadExistingMonoDevelopSolutionProperties());
    }

    private static IEnumerable<MonoIsland> RelevantIslandsForMode(IEnumerable<MonoIsland> islands, SolutionSynchronizer.Mode mode)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return islands.Where<MonoIsland>(new Func<MonoIsland, bool>(new SolutionSynchronizer.\u003CRelevantIslandsForMode\u003Ec__AnonStoreyC3() { mode = mode }.\u003C\u003Em__235));
    }

    private string GetProjectEntries(IEnumerable<MonoIsland> islands)
    {
      IEnumerable<string> source = islands.Select<MonoIsland, string>((Func<MonoIsland, string>) (i => string.Format(SolutionSynchronizer.DefaultSynchronizationSettings.SolutionProjectEntryTemplate, (object) this.SolutionGuid(), (object) this._projectName, (object) Path.GetFileName(this.ProjectFile(i)), (object) this.ProjectGuid(i._output))));
      return string.Join(SolutionSynchronizer.WindowsNewline, source.ToArray<string>());
    }

    private string GetProjectActiveConfigurations(string projectGuid)
    {
      return string.Format(SolutionSynchronizer.DefaultSynchronizationSettings.SolutionProjectConfigurationTemplate, (object) projectGuid);
    }

    private string EscapedRelativePathFor(string file)
    {
      string str = this._projectDirectory.Replace("/", "\\");
      file = file.Replace("/", "\\");
      return SecurityElement.Escape(!file.StartsWith(str) ? file : file.Substring(this._projectDirectory.Length + 1));
    }

    private string ProjectGuid(string assembly)
    {
      return SolutionGuidGenerator.GuidForProject(this._projectName + Path.GetFileNameWithoutExtension(assembly));
    }

    private string SolutionGuid()
    {
      return SolutionGuidGenerator.GuidForSolution(this._projectName);
    }

    private string ProjectFooter(MonoIsland island)
    {
      return string.Format(this._settings.GetProjectFooterTemplate(SolutionSynchronizer.ScriptingLanguageFor(island)), (object) this.ReadExistingMonoDevelopProjectProperties(island));
    }

    private string ReadExistingMonoDevelopSolutionProperties()
    {
      if (!this.SolutionExists())
        return SolutionSynchronizer.DefaultMonoDevelopSolutionProperties;
      string[] strArray;
      try
      {
        strArray = File.ReadAllLines(this.SolutionFile());
      }
      catch (IOException ex)
      {
        return SolutionSynchronizer.DefaultMonoDevelopSolutionProperties;
      }
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = false;
      foreach (string input in strArray)
      {
        if (SolutionSynchronizer._MonoDevelopPropertyHeader.IsMatch(input))
          flag = true;
        if (flag)
        {
          if (input.Contains("EndGlobalSection"))
          {
            stringBuilder.Append(input);
            flag = false;
          }
          else
            stringBuilder.AppendFormat("{0}{1}", (object) input, (object) SolutionSynchronizer.WindowsNewline);
        }
      }
      if (0 < stringBuilder.Length)
        return stringBuilder.ToString();
      return SolutionSynchronizer.DefaultMonoDevelopSolutionProperties;
    }

    private string ReadExistingMonoDevelopProjectProperties(MonoIsland island)
    {
      if (!this.ProjectExists(island))
        return string.Empty;
      XmlDocument xmlDocument = new XmlDocument();
      XmlNamespaceManager nsmgr;
      try
      {
        xmlDocument.Load(this.ProjectFile(island));
        nsmgr = new XmlNamespaceManager(xmlDocument.NameTable);
        nsmgr.AddNamespace("msb", SolutionSynchronizer.MSBuildNamespaceUri);
      }
      catch (Exception ex)
      {
        if (ex is IOException || ex is XmlException)
          return string.Empty;
        throw;
      }
      XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/msb:Project/msb:ProjectExtensions", nsmgr);
      if (xmlNodeList.Count == 0)
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (XmlNode xmlNode in xmlNodeList)
        stringBuilder.AppendLine(xmlNode.OuterXml);
      return stringBuilder.ToString();
    }

    [Obsolete("Use AssemblyHelper.IsManagedAssembly")]
    public static bool IsManagedAssembly(string file)
    {
      return AssemblyHelper.IsManagedAssembly(file);
    }

    public static string GetProjectExtension(ScriptingLanguage language)
    {
      if (!SolutionSynchronizer.ProjectExtensions.ContainsKey(language))
        throw new ArgumentException("Unsupported language", "language");
      return SolutionSynchronizer.ProjectExtensions[language];
    }

    private enum Mode
    {
      UnityScriptAsUnityProj,
      UntiyScriptAsPrecompiledAssembly,
    }
  }
}
