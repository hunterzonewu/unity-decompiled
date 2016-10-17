// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AssemblyStripper
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEditor;
using UnityEditor.Scripting.Compilers;
using UnityEditor.Utils;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class AssemblyStripper
  {
    private static string[] Il2CppBlacklistPaths
    {
      get
      {
        return new string[1]{ Path.Combine("..", "platform_native_link.xml") };
      }
    }

    private static string MonoLinkerPath
    {
      get
      {
        if (Application.platform == RuntimePlatform.WindowsEditor)
          return Path.Combine(MonoInstallationFinder.GetFrameWorksFolder(), "Tools/UnusedBytecodeStripper.exe");
        return Path.Combine(MonoInstallationFinder.GetFrameWorksFolder(), "Tools/UnusedByteCodeStripper/UnusedBytecodeStripper.exe");
      }
    }

    private static string ModulesWhitelistPath
    {
      get
      {
        return Path.Combine(Path.GetDirectoryName(AssemblyStripper.MonoLinkerPath), "ModuleStrippingInformation");
      }
    }

    private static string MonoLinker2Path
    {
      get
      {
        return Path.Combine(MonoInstallationFinder.GetFrameWorksFolder(), "Tools/UnusedByteCodeStripper2/UnusedBytecodeStripper2.exe");
      }
    }

    private static string BlacklistPath
    {
      get
      {
        return Path.Combine(Path.GetDirectoryName(AssemblyStripper.MonoLinkerPath), "Core.xml");
      }
    }

    private static string GetModuleWhitelist(string module, string moduleStrippingInformationFolder)
    {
      return Paths.Combine(moduleStrippingInformationFolder, module + ".xml");
    }

    private static bool StripAssembliesTo(string[] assemblies, string[] searchDirs, string outputFolder, string workingDirectory, out string output, out string error, string linkerPath, IIl2CppPlatformProvider platformProvider, IEnumerable<string> additionalBlacklist, bool developmentBuild)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AssemblyStripper.\u003CStripAssembliesTo\u003Ec__AnonStorey65 assembliesToCAnonStorey65 = new AssemblyStripper.\u003CStripAssembliesTo\u003Ec__AnonStorey65();
      // ISSUE: reference to a compiler-generated field
      assembliesToCAnonStorey65.workingDirectory = workingDirectory;
      if (!Directory.Exists(outputFolder))
        Directory.CreateDirectory(outputFolder);
      // ISSUE: reference to a compiler-generated method
      additionalBlacklist = additionalBlacklist.Select<string, string>(new Func<string, string>(assembliesToCAnonStorey65.\u003C\u003Em__D4)).Where<string>(new Func<string, bool>(File.Exists));
      IEnumerable<string> userBlacklistFiles = AssemblyStripper.GetUserBlacklistFiles();
      foreach (string str in userBlacklistFiles)
        Console.WriteLine("UserBlackList: " + str);
      additionalBlacklist = additionalBlacklist.Concat<string>(userBlacklistFiles);
      List<string> stringList = new List<string>() { "-out \"" + outputFolder + "\"", "-l none", "-c link", "-b " + (object) developmentBuild, "-x \"" + AssemblyStripper.GetModuleWhitelist("Core", platformProvider.moduleStrippingInformationFolder) + "\"", "-f \"" + Path.Combine(platformProvider.il2CppFolder, "LinkerDescriptors") + "\"" };
      stringList.AddRange(additionalBlacklist.Select<string, string>((Func<string, string>) (path => "-x \"" + path + "\"")));
      stringList.AddRange(((IEnumerable<string>) searchDirs).Select<string, string>((Func<string, string>) (d => "-d \"" + d + "\"")));
      stringList.AddRange(((IEnumerable<string>) assemblies).Select<string, string>((Func<string, string>) (assembly => "-a  \"" + Path.GetFullPath(assembly) + "\"")));
      // ISSUE: reference to a compiler-generated field
      return AssemblyStripper.RunAssemblyLinker((IEnumerable<string>) stringList, out output, out error, linkerPath, assembliesToCAnonStorey65.workingDirectory);
    }

    private static bool RunAssemblyLinker(IEnumerable<string> args, out string @out, out string err, string linkerPath, string workingDirectory)
    {
      string args1 = args.Aggregate<string>((Func<string, string, string>) ((buff, s) => buff + " " + s));
      Console.WriteLine("Invoking UnusedByteCodeStripper2 with arguments: " + args1);
      Runner.RunManagedProgram(linkerPath, args1, workingDirectory, (CompilerOutputParserBase) null);
      @out = string.Empty;
      err = string.Empty;
      return true;
    }

    private static List<string> GetUserAssemblies(RuntimeClassRegistry rcr, string managedDir)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AssemblyStripper.\u003CGetUserAssemblies\u003Ec__AnonStorey66 assembliesCAnonStorey66 = new AssemblyStripper.\u003CGetUserAssemblies\u003Ec__AnonStorey66();
      // ISSUE: reference to a compiler-generated field
      assembliesCAnonStorey66.rcr = rcr;
      // ISSUE: reference to a compiler-generated field
      assembliesCAnonStorey66.managedDir = managedDir;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      return ((IEnumerable<string>) assembliesCAnonStorey66.rcr.GetUserAssemblies()).Where<string>(new Func<string, bool>(assembliesCAnonStorey66.\u003C\u003Em__D9)).Select<string, string>(new Func<string, string>(assembliesCAnonStorey66.\u003C\u003Em__DA)).ToList<string>();
    }

    internal static void StripAssemblies(string stagingAreaData, IIl2CppPlatformProvider platformProvider, RuntimeClassRegistry rcr, bool developmentBuild)
    {
      string fullPath = Path.GetFullPath(Path.Combine(stagingAreaData, "Managed"));
      List<string> userAssemblies = AssemblyStripper.GetUserAssemblies(rcr, fullPath);
      string[] array = userAssemblies.ToArray();
      string[] searchDirs = new string[1]{ fullPath };
      AssemblyStripper.RunAssemblyStripper(stagingAreaData, (IEnumerable) userAssemblies, fullPath, array, searchDirs, AssemblyStripper.MonoLinker2Path, platformProvider, rcr, developmentBuild);
    }

    internal static IEnumerable<string> GetUserBlacklistFiles()
    {
      return ((IEnumerable<string>) Directory.GetFiles("Assets", "link.xml", SearchOption.AllDirectories)).Select<string, string>((Func<string, string>) (s => Path.Combine(Directory.GetCurrentDirectory(), s)));
    }

    private static List<string> GetDependentModules(string moduleXml)
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.Load(moduleXml);
      List<string> stringList = new List<string>();
      foreach (XmlNode selectNode in xmlDocument.DocumentElement.SelectNodes("/linker/dependencies/module"))
        stringList.Add(selectNode.Attributes["name"].Value);
      return stringList;
    }

    private static bool AddWhiteListsForModules(IEnumerable<string> nativeModules, ref IEnumerable<string> blacklists, string moduleStrippingInformationFolder)
    {
      bool flag = false;
      foreach (string nativeModule in nativeModules)
      {
        string moduleWhitelist = AssemblyStripper.GetModuleWhitelist(nativeModule, moduleStrippingInformationFolder);
        if (File.Exists(moduleWhitelist))
        {
          if (!blacklists.Contains<string>(moduleWhitelist))
          {
            blacklists = blacklists.Concat<string>((IEnumerable<string>) new string[1]
            {
              moduleWhitelist
            });
            flag = true;
          }
          flag = flag || AssemblyStripper.AddWhiteListsForModules((IEnumerable<string>) AssemblyStripper.GetDependentModules(moduleWhitelist), ref blacklists, moduleStrippingInformationFolder);
        }
      }
      return flag;
    }

    private static void RunAssemblyStripper(string stagingAreaData, IEnumerable assemblies, string managedAssemblyFolderPath, string[] assembliesToStrip, string[] searchDirs, string monoLinkerPath, IIl2CppPlatformProvider platformProvider, RuntimeClassRegistry rcr, bool developmentBuild)
    {
      bool flag1 = rcr != null && PlayerSettings.stripEngineCode && platformProvider.supportsEngineStripping;
      IEnumerable<string> blacklists = (IEnumerable<string>) AssemblyStripper.Il2CppBlacklistPaths;
      if (rcr != null)
        blacklists = blacklists.Concat<string>((IEnumerable<string>) new string[2]
        {
          AssemblyStripper.WriteMethodsToPreserveBlackList(stagingAreaData, rcr),
          MonoAssemblyStripping.GenerateLinkXmlToPreserveDerivedTypes(stagingAreaData, managedAssemblyFolderPath, rcr)
        });
      if (!flag1)
      {
        foreach (string file in Directory.GetFiles(platformProvider.moduleStrippingInformationFolder, "*.xml"))
          blacklists = blacklists.Concat<string>((IEnumerable<string>) new string[1]{ file });
      }
      string fullPath1 = Path.GetFullPath(Path.Combine(managedAssemblyFolderPath, "tempStrip"));
      bool flag2;
      do
      {
        flag2 = false;
        if (EditorUtility.DisplayCancelableProgressBar("Building Player", "Stripping assemblies", 0.0f))
          throw new OperationCanceledException();
        string output;
        string error;
        if (!AssemblyStripper.StripAssembliesTo(assembliesToStrip, searchDirs, fullPath1, managedAssemblyFolderPath, out output, out error, monoLinkerPath, platformProvider, blacklists, developmentBuild))
          throw new Exception("Error in stripping assemblies: " + (object) assemblies + ", " + error);
        string icallsListFile = Path.Combine(managedAssemblyFolderPath, "ICallSummary.txt");
        Runner.RunManagedProgram(Path.Combine(MonoInstallationFinder.GetFrameWorksFolder(), "Tools/InternalCallRegistrationWriter/InternalCallRegistrationWriter.exe"), string.Format("-assembly=\"{0}\" -output=\"{1}\" -summary=\"{2}\"", (object) Path.Combine(fullPath1, "UnityEngine.dll"), (object) Path.Combine(managedAssemblyFolderPath, "UnityICallRegistration.cpp"), (object) icallsListFile));
        if (flag1)
        {
          HashSet<string> nativeClasses;
          HashSet<string> nativeModules;
          CodeStrippingUtils.GenerateDependencies(fullPath1, icallsListFile, rcr, out nativeClasses, out nativeModules);
          flag2 = AssemblyStripper.AddWhiteListsForModules((IEnumerable<string>) nativeModules, ref blacklists, platformProvider.moduleStrippingInformationFolder);
        }
      }
      while (flag2);
      string fullPath2 = Path.GetFullPath(Path.Combine(managedAssemblyFolderPath, "tempUnstripped"));
      Directory.CreateDirectory(fullPath2);
      foreach (string file in Directory.GetFiles(managedAssemblyFolderPath))
      {
        string extension = Path.GetExtension(file);
        if (string.Equals(extension, ".dll", StringComparison.InvariantCultureIgnoreCase) || string.Equals(extension, ".mdb", StringComparison.InvariantCultureIgnoreCase))
          File.Move(file, Path.Combine(fullPath2, Path.GetFileName(file)));
      }
      foreach (string file in Directory.GetFiles(fullPath1))
        File.Move(file, Path.Combine(managedAssemblyFolderPath, Path.GetFileName(file)));
      Directory.Delete(fullPath1);
    }

    private static string WriteMethodsToPreserveBlackList(string stagingAreaData, RuntimeClassRegistry rcr)
    {
      string path = (!Path.IsPathRooted(stagingAreaData) ? Directory.GetCurrentDirectory() + "/" : string.Empty) + stagingAreaData + "/methods_pointedto_by_uievents.xml";
      File.WriteAllText(path, AssemblyStripper.GetMethodPreserveBlacklistContents(rcr));
      return path;
    }

    private static string GetMethodPreserveBlacklistContents(RuntimeClassRegistry rcr)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("<linker>");
      foreach (IGrouping<string, RuntimeClassRegistry.MethodDescription> source in rcr.GetMethodsToPreserve().GroupBy<RuntimeClassRegistry.MethodDescription, string>((Func<RuntimeClassRegistry.MethodDescription, string>) (m => m.assembly)))
      {
        stringBuilder.AppendLine(string.Format("\t<assembly fullname=\"{0}\">", (object) source.Key));
        foreach (IGrouping<string, RuntimeClassRegistry.MethodDescription> grouping in source.GroupBy<RuntimeClassRegistry.MethodDescription, string>((Func<RuntimeClassRegistry.MethodDescription, string>) (m => m.fullTypeName)))
        {
          stringBuilder.AppendLine(string.Format("\t\t<type fullname=\"{0}\">", (object) grouping.Key));
          foreach (RuntimeClassRegistry.MethodDescription methodDescription in (IEnumerable<RuntimeClassRegistry.MethodDescription>) grouping)
            stringBuilder.AppendLine(string.Format("\t\t\t<method name=\"{0}\"/>", (object) methodDescription.methodName));
          stringBuilder.AppendLine("\t\t</type>");
        }
        stringBuilder.AppendLine("\t</assembly>");
      }
      stringBuilder.AppendLine("</linker>");
      return stringBuilder.ToString();
    }
  }
}
