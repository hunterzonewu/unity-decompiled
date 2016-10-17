// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.BooLanguage
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using Boo.Lang.Compiler.Ast;
using Boo.Lang.Parser;
using System.Collections.Generic;
using System.Linq;

namespace UnityEditor.Scripting.Compilers
{
  internal class BooLanguage : SupportedLanguage
  {
    public override string GetExtensionICanCompile()
    {
      return "boo";
    }

    public override string GetLanguageName()
    {
      return "Boo";
    }

    public override ScriptCompilerBase CreateCompiler(MonoIsland island, bool buildingForEditor, BuildTarget targetPlatform, bool runUpdater)
    {
      return (ScriptCompilerBase) new BooCompiler(island, runUpdater);
    }

    public override string GetNamespace(string fileName)
    {
      try
      {
        return ((IEnumerable<Module>) BooParser.ParseFile(fileName).get_Modules()).First<Module>().get_Namespace().get_Name();
      }
      catch
      {
      }
      return base.GetNamespace(fileName);
    }
  }
}
