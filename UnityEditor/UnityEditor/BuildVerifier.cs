// Decompiled with JetBrains decompiler
// Type: UnityEditor.BuildVerifier
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.XPath;
using UnityEngine;

namespace UnityEditor
{
  internal class BuildVerifier
  {
    private Dictionary<string, HashSet<string>> m_UnsupportedAssemblies;
    private static BuildVerifier ms_Inst;

    protected BuildVerifier()
    {
      this.m_UnsupportedAssemblies = new Dictionary<string, HashSet<string>>();
      string uri = Path.Combine(Path.Combine(EditorApplication.applicationContentsPath, "Resources"), "BuildVerification.xml");
      XPathNavigator navigator = new XPathDocument(uri).CreateNavigator();
      navigator.MoveToFirstChild();
      XPathNodeIterator xpathNodeIterator = navigator.SelectChildren("assembly", string.Empty);
      while (xpathNodeIterator.MoveNext())
      {
        string attribute = xpathNodeIterator.Current.GetAttribute("name", string.Empty);
        if (attribute == null || attribute.Length < 1)
          throw new ApplicationException(string.Format("Failed to load {0}, <assembly> name attribute is empty", (object) uri));
        string key = xpathNodeIterator.Current.GetAttribute("platform", string.Empty);
        if (key == null || key.Length < 1)
          key = "*";
        if (!this.m_UnsupportedAssemblies.ContainsKey(key))
          this.m_UnsupportedAssemblies.Add(key, new HashSet<string>());
        this.m_UnsupportedAssemblies[key].Add(attribute);
      }
    }

    protected void VerifyBuildInternal(BuildTarget target, string managedDllFolder)
    {
      foreach (string file in Directory.GetFiles(managedDllFolder))
      {
        if (file.EndsWith(".dll"))
        {
          string fileName = Path.GetFileName(file);
          if (!this.VerifyAssembly(target, fileName))
            Debug.LogWarningFormat("{0} assembly is referenced by user code, but is not supported on {1} platform. Various failures might follow.", new object[2]
            {
              (object) fileName,
              (object) target.ToString()
            });
        }
      }
    }

    protected bool VerifyAssembly(BuildTarget target, string assembly)
    {
      return (!this.m_UnsupportedAssemblies.ContainsKey("*") || !this.m_UnsupportedAssemblies["*"].Contains(assembly)) && (!this.m_UnsupportedAssemblies.ContainsKey(target.ToString()) || !this.m_UnsupportedAssemblies[target.ToString()].Contains(assembly));
    }

    public static void VerifyBuild(BuildTarget target, string managedDllFolder)
    {
      if (BuildVerifier.ms_Inst == null)
        BuildVerifier.ms_Inst = new BuildVerifier();
      BuildVerifier.ms_Inst.VerifyBuildInternal(target, managedDllFolder);
    }
  }
}
