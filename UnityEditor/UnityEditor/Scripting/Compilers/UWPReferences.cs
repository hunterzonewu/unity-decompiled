// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.UWPReferences
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEditorInternal;

namespace UnityEditor.Scripting.Compilers
{
  internal static class UWPReferences
  {
    public static string[] GetReferences()
    {
      string folder;
      Version version1;
      UWPReferences.GetWindowsKit10(out folder, out version1);
      string version2 = version1.ToString();
      if (version1.Minor == -1)
        version2 += ".0";
      if (version1.Build == -1)
        version2 += ".0";
      if (version1.Revision == -1)
        version2 += ".0";
      HashSet<string> source = new HashSet<string>((IEqualityComparer<string>) StringComparer.InvariantCultureIgnoreCase);
      source.Add(Path.Combine(folder, "UnionMetadata\\Facade\\Windows.winmd"));
      foreach (string str in UWPReferences.GetPlatform(folder, version2))
        source.Add(str);
      foreach (UWPReferences.UWPExtension extension in UWPReferences.GetExtensions(folder, version2))
      {
        foreach (string reference in extension.References)
          source.Add(reference);
      }
      return source.ToArray<string>();
    }

    private static string[] GetPlatform(string folder, string version)
    {
      string referencesFolder = Path.Combine(folder, "References");
      string uri = FileUtil.CombinePaths(folder, "Platforms\\UAP", version, "Platform.xml");
      XElement xelement = XDocument.Load(uri).Element((XName) "ApplicationPlatform");
      if (xelement.Attribute((XName) "name").Value != "UAP")
        throw new Exception(string.Format("Invalid platform manifest at \"{0}\".", (object) uri));
      XElement containedApiContractsElement = xelement.Element((XName) "ContainedApiContracts");
      return UWPReferences.GetReferences(referencesFolder, containedApiContractsElement);
    }

    private static UWPReferences.UWPExtension[] GetExtensions(string folder, string version)
    {
      string path = Path.Combine(folder, "Extension SDKs");
      string referencesFolder = Path.Combine(folder, "References");
      List<UWPReferences.UWPExtension> uwpExtensionList = new List<UWPReferences.UWPExtension>();
      foreach (string directory in Directory.GetDirectories(path))
      {
        string str = FileUtil.CombinePaths(directory, version, "SDKManifest.xml");
        if (File.Exists(str))
        {
          try
          {
            UWPReferences.UWPExtension uwpExtension = new UWPReferences.UWPExtension(str, referencesFolder);
            uwpExtensionList.Add(uwpExtension);
          }
          catch
          {
          }
        }
      }
      return uwpExtensionList.ToArray();
    }

    private static string[] GetReferences(string referencesFolder, XElement containedApiContractsElement)
    {
      List<string> stringList = new List<string>();
      foreach (XElement element in containedApiContractsElement.Elements((XName) "ApiContract"))
      {
        string str1 = element.Attribute((XName) "name").Value;
        string str2 = element.Attribute((XName) "version").Value;
        string path = FileUtil.CombinePaths(referencesFolder, str1, str2, str1 + ".winmd");
        if (File.Exists(path))
          stringList.Add(path);
      }
      return stringList.ToArray();
    }

    private static void GetWindowsKit10(out string folder, out Version version)
    {
      string environmentVariable = Environment.GetEnvironmentVariable("ProgramFiles(x86)");
      folder = Path.Combine(environmentVariable, "Windows Kits\\10\\");
      version = new Version(10, 0, 10240);
      try
      {
        folder = RegistryUtil.GetRegistryStringValue32("SOFTWARE\\Microsoft\\Microsoft SDKs\\Windows\\v10.0", "InstallationFolder", folder);
        string registryStringValue32 = RegistryUtil.GetRegistryStringValue32("SOFTWARE\\Microsoft\\Microsoft SDKs\\Windows\\v10.0", "ProductVersion", version.ToString());
        version = new Version(registryStringValue32);
      }
      catch
      {
      }
    }

    private sealed class UWPExtension
    {
      public string Name { get; private set; }

      public string[] References { get; private set; }

      public UWPExtension(string manifest, string referencesFolder)
      {
        XElement xelement = XDocument.Load(manifest).Element((XName) "FileList");
        if (xelement.Attribute((XName) "TargetPlatform").Value != "UAP")
          throw new Exception(string.Format("Invalid extension manifest at \"{0}\".", (object) manifest));
        this.Name = xelement.Attribute((XName) "DisplayName").Value;
        XElement containedApiContractsElement = xelement.Element((XName) "ContainedApiContracts");
        this.References = UWPReferences.GetReferences(referencesFolder, containedApiContractsElement);
      }
    }
  }
}
