// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.APIUpdaterHelper
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Unity.DataContract;
using UnityEditor.Modules;
using UnityEditor.Scripting.Compilers;
using UnityEditor.Utils;
using UnityEngine;

namespace UnityEditor.Scripting
{
  internal class APIUpdaterHelper
  {
    public static bool IsReferenceToMissingObsoleteMember(string namespaceName, string className)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      APIUpdaterHelper.\u003CIsReferenceToMissingObsoleteMember\u003Ec__AnonStoreyB1 memberCAnonStoreyB1 = new APIUpdaterHelper.\u003CIsReferenceToMissingObsoleteMember\u003Ec__AnonStoreyB1();
      // ISSUE: reference to a compiler-generated field
      memberCAnonStoreyB1.className = className;
      // ISSUE: reference to a compiler-generated field
      memberCAnonStoreyB1.namespaceName = namespaceName;
      try
      {
        // ISSUE: reference to a compiler-generated method
        return ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).SelectMany<Assembly, System.Type>((Func<Assembly, IEnumerable<System.Type>>) (a => (IEnumerable<System.Type>) a.GetTypes())).FirstOrDefault<System.Type>(new Func<System.Type, bool>(memberCAnonStoreyB1.\u003C\u003Em__1FF)) != null;
      }
      catch (ReflectionTypeLoadException ex)
      {
        throw new Exception(ex.Message + ((IEnumerable<Exception>) ex.LoaderExceptions).Aggregate<Exception, string>(string.Empty, (Func<string, Exception, string>) ((acc, curr) => acc + "\r\n\t" + curr.Message)));
      }
    }

    public static void Run(string commaSeparatedListOfAssemblies)
    {
      string[] strArray = commaSeparatedListOfAssemblies.Split(new char[1]{ ',' }, StringSplitOptions.RemoveEmptyEntries);
      APIUpdaterLogger.WriteToFile("Started to update {0} assemblie(s)", (object) ((IEnumerable<string>) strArray).Count<string>());
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      foreach (string str1 in strArray)
      {
        if (AssemblyHelper.IsManagedAssembly(str1))
        {
          string str2 = APIUpdaterHelper.ResolveAssemblyPath(str1);
          string stdOut;
          string stdErr;
          int num = APIUpdaterHelper.RunUpdatingProgram("AssemblyUpdater.exe", "-u -a " + str2 + APIUpdaterHelper.APIVersionArgument() + APIUpdaterHelper.AssemblySearchPathArgument() + APIUpdaterHelper.ConfigurationProviderAssembliesPathArgument(), out stdOut, out stdErr);
          if (stdOut.Length > 0)
            APIUpdaterLogger.WriteToFile("Assembly update output ({0})\r\n{1}", (object) str2, (object) stdOut);
          if (num < 0)
            APIUpdaterLogger.WriteErrorToConsole("Error {0} running AssemblyUpdater. Its output is: `{1}`", (object) num, (object) stdErr);
        }
      }
      APIUpdaterLogger.WriteToFile("Update finished in {0}s", (object) stopwatch.Elapsed.TotalSeconds);
    }

    private static string ResolveAssemblyPath(string assemblyPath)
    {
      return CommandLineFormatter.PrepareFileName(assemblyPath);
    }

    public static bool DoesAssemblyRequireUpgrade(string assemblyFullPath)
    {
      if (!File.Exists(assemblyFullPath) || !AssemblyHelper.IsManagedAssembly(assemblyFullPath) || !APIUpdaterHelper.MayContainUpdatableReferences(assemblyFullPath))
        return false;
      string stdOut;
      string stdErr;
      int num = APIUpdaterHelper.RunUpdatingProgram("AssemblyUpdater.exe", APIUpdaterHelper.TimeStampArgument() + APIUpdaterHelper.APIVersionArgument() + "--check-update-required -a " + CommandLineFormatter.PrepareFileName(assemblyFullPath) + APIUpdaterHelper.AssemblySearchPathArgument() + APIUpdaterHelper.ConfigurationProviderAssembliesPathArgument(), out stdOut, out stdErr);
      Console.WriteLine("{0}{1}", (object) stdOut, (object) stdErr);
      switch (num)
      {
        case 0:
        case 1:
          return false;
        case 2:
          return true;
        default:
          UnityEngine.Debug.LogError((object) (stdOut + Environment.NewLine + stdErr));
          return false;
      }
    }

    private static string AssemblySearchPathArgument()
    {
      return " -s \"" + (Path.Combine(MonoInstallationFinder.GetFrameWorksFolder(), "Managed") + ",+" + Path.Combine(EditorApplication.applicationContentsPath, "UnityExtensions/Unity") + ",+" + Application.dataPath) + "\"";
    }

    private static string ConfigurationProviderAssembliesPathArgument()
    {
      StringBuilder stringBuilder = new StringBuilder();
      using (IEnumerator<PackageInfo> enumerator = ModuleManager.packageManager.get_unityExtensions().GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          PackageInfo current = enumerator.Current;
          foreach (string path2 in ((IEnumerable<KeyValuePair<string, PackageFileData>>) current.get_files()).Where<KeyValuePair<string, PackageFileData>>((Func<KeyValuePair<string, PackageFileData>, bool>) (f => f.Value.type == 3)).Select<KeyValuePair<string, PackageFileData>, string>((Func<KeyValuePair<string, PackageFileData>, string>) (pi => pi.Key)))
            stringBuilder.AppendFormat(" {0}", (object) CommandLineFormatter.PrepareFileName(Path.Combine((string) current.basePath, path2)));
        }
      }
      string editorManagedPath = APIUpdaterHelper.GetUnityEditorManagedPath();
      stringBuilder.AppendFormat(" {0}", (object) CommandLineFormatter.PrepareFileName(Path.Combine(editorManagedPath, "UnityEngine.dll")));
      stringBuilder.AppendFormat(" {0}", (object) CommandLineFormatter.PrepareFileName(Path.Combine(editorManagedPath, "UnityEditor.dll")));
      return stringBuilder.ToString();
    }

    private static string GetUnityEditorManagedPath()
    {
      return Path.Combine(MonoInstallationFinder.GetFrameWorksFolder(), "Managed");
    }

    private static int RunUpdatingProgram(string executable, string arguments, out string stdOut, out string stdErr)
    {
      ManagedProgram managedProgram = new ManagedProgram(MonoInstallationFinder.GetMonoInstallation("MonoBleedingEdge"), "4.0", EditorApplication.applicationContentsPath + "/Tools/ScriptUpdater/" + executable, arguments);
      managedProgram.LogProcessStartInfo();
      managedProgram.Start();
      managedProgram.WaitForExit();
      stdOut = managedProgram.GetStandardOutputAsString();
      stdErr = string.Join("\r\n", managedProgram.GetErrorOutput());
      return managedProgram.ExitCode;
    }

    private static string APIVersionArgument()
    {
      return " --api-version " + Application.unityVersion + " ";
    }

    private static string TimeStampArgument()
    {
      return " --timestamp " + (object) DateTime.Now.Ticks + " ";
    }

    private static bool IsUpdateable(System.Type type)
    {
      object[] customAttributes = type.GetCustomAttributes(typeof (ObsoleteAttribute), false);
      if (customAttributes.Length != 1)
        return false;
      return ((ObsoleteAttribute) customAttributes[0]).Message.Contains("UnityUpgradable");
    }

    internal static bool MayContainUpdatableReferences(string assemblyPath)
    {
      using (FileStream fileStream = File.Open(assemblyPath, System.IO.FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
      {
        AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly((Stream) fileStream);
        if (((AssemblyNameReference) assembly.get_Name()).get_IsWindowsRuntime())
          return false;
        if (!APIUpdaterHelper.IsTargetFrameworkValidOnCurrentOS(assembly))
          return false;
      }
      return true;
    }

    private static bool IsTargetFrameworkValidOnCurrentOS(AssemblyDefinition assembly)
    {
      if (Environment.OSVersion.Platform != PlatformID.Win32NT)
        return (!assembly.get_HasCustomAttributes() ? 0 : (((IEnumerable<CustomAttribute>) assembly.get_CustomAttributes()).Any<CustomAttribute>((Func<CustomAttribute, bool>) (attr => APIUpdaterHelper.TargetsWindowsSpecificFramework(attr))) ? 1 : 0)) == 0;
      return true;
    }

    private static bool TargetsWindowsSpecificFramework(CustomAttribute targetFrameworkAttr)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      APIUpdaterHelper.\u003CTargetsWindowsSpecificFramework\u003Ec__AnonStoreyB2 frameworkCAnonStoreyB2 = new APIUpdaterHelper.\u003CTargetsWindowsSpecificFramework\u003Ec__AnonStoreyB2();
      if (!targetFrameworkAttr.get_AttributeType().get_FullName().Contains("System.Runtime.Versioning.TargetFrameworkAttribute"))
        return false;
      // ISSUE: reference to a compiler-generated field
      frameworkCAnonStoreyB2.regex = new Regex("\\.NETCore|\\.NETPortable");
      // ISSUE: reference to a compiler-generated method
      return ((IEnumerable<CustomAttributeArgument>) targetFrameworkAttr.get_ConstructorArguments()).Any<CustomAttributeArgument>(new Func<CustomAttributeArgument, bool>(frameworkCAnonStoreyB2.\u003C\u003Em__204));
    }
  }
}
