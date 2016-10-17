// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.ScriptCompilers
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Scripting.Compilers;

namespace UnityEditor.Scripting
{
  internal static class ScriptCompilers
  {
    private static List<SupportedLanguage> _supportedLanguages = new List<SupportedLanguage>();

    static ScriptCompilers()
    {
      using (List<System.Type>.Enumerator enumerator = new List<System.Type>() { typeof (CSharpLanguage), typeof (BooLanguage), typeof (UnityScriptLanguage) }.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          System.Type current = enumerator.Current;
          ScriptCompilers._supportedLanguages.Add((SupportedLanguage) Activator.CreateInstance(current));
        }
      }
    }

    internal static SupportedLanguageStruct[] GetSupportedLanguageStructs()
    {
      return ScriptCompilers._supportedLanguages.Select<SupportedLanguage, SupportedLanguageStruct>((Func<SupportedLanguage, SupportedLanguageStruct>) (lang => new SupportedLanguageStruct() { extension = lang.GetExtensionICanCompile(), languageName = lang.GetLanguageName() })).ToArray<SupportedLanguageStruct>();
    }

    internal static string GetNamespace(string file)
    {
      if (string.IsNullOrEmpty(file))
        throw new ArgumentException("Invalid file");
      string extensionOfSourceFile = ScriptCompilers.GetExtensionOfSourceFile(file);
      using (List<SupportedLanguage>.Enumerator enumerator = ScriptCompilers._supportedLanguages.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          SupportedLanguage current = enumerator.Current;
          if (current.GetExtensionICanCompile() == extensionOfSourceFile)
            return current.GetNamespace(file);
        }
      }
      throw new ApplicationException("Unable to find a suitable compiler");
    }

    internal static ScriptCompilerBase CreateCompilerInstance(MonoIsland island, bool buildingForEditor, BuildTarget targetPlatform, bool runUpdater)
    {
      if (island._files.Length == 0)
        throw new ArgumentException("Cannot compile MonoIsland with no files");
      using (List<SupportedLanguage>.Enumerator enumerator = ScriptCompilers._supportedLanguages.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          SupportedLanguage current = enumerator.Current;
          if (current.GetExtensionICanCompile() == island.GetExtensionOfSourceFiles())
            return current.CreateCompiler(island, buildingForEditor, targetPlatform, runUpdater);
        }
      }
      throw new ApplicationException(string.Format("Unable to find a suitable compiler for sources with extension '{0}' (Output assembly: {1})", (object) island.GetExtensionOfSourceFiles(), (object) island._output));
    }

    public static string GetExtensionOfSourceFile(string file)
    {
      return Path.GetExtension(file).ToLower().Substring(1);
    }
  }
}
