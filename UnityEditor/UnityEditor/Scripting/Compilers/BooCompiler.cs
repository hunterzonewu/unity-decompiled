// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.BooCompiler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Utils;
using UnityEngineInternal;

namespace UnityEditor.Scripting.Compilers
{
  internal class BooCompiler : MonoScriptCompilerBase
  {
    public BooCompiler(MonoIsland island, bool runUpdater)
      : base(island, runUpdater)
    {
    }

    protected override Program StartCompiler()
    {
      List<string> arguments = new List<string>() { "-debug", "-target:library", "-out:" + this._island._output, "-x-type-inference-rule-attribute:" + (object) typeof (TypeInferenceRuleAttribute) };
      foreach (string reference in this._island._references)
        arguments.Add("-r:" + ScriptCompilerBase.PrepareFileName(reference));
      foreach (string str in ((IEnumerable<string>) this._island._defines).Distinct<string>())
        arguments.Add("-define:" + str);
      foreach (string file in this._island._files)
        arguments.Add(ScriptCompilerBase.PrepareFileName(file));
      return (Program) this.StartCompiler(this._island._target, Path.Combine(this.GetProfileDirectory(), "booc.exe"), arguments);
    }

    protected override CompilerOutputParserBase CreateOutputParser()
    {
      return (CompilerOutputParserBase) new BooCompilerOutputParser();
    }
  }
}
