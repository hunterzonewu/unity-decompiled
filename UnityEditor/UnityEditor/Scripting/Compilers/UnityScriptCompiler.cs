// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.UnityScriptCompiler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor.Utils;
using UnityEngineInternal;

namespace UnityEditor.Scripting.Compilers
{
  internal class UnityScriptCompiler : MonoScriptCompilerBase
  {
    private static readonly Regex UnityEditorPattern = new Regex("UnityEditor\\.dll$", RegexOptions.ExplicitCapture);

    public UnityScriptCompiler(MonoIsland island, bool runUpdater)
      : base(island, runUpdater)
    {
    }

    protected override CompilerOutputParserBase CreateOutputParser()
    {
      return (CompilerOutputParserBase) new UnityScriptCompilerOutputParser();
    }

    protected override Program StartCompiler()
    {
      List<string> arguments = new List<string>() { "-debug", "-target:library", "-i:UnityEngine", "-i:System.Collections", "-base:UnityEngine.MonoBehaviour", "-nowarn:BCW0016", "-nowarn:BCW0003", "-method:Main", "-out:" + this._island._output, "-x-type-inference-rule-attribute:" + (object) typeof (TypeInferenceRuleAttribute) };
      if (this.StrictBuildTarget())
        arguments.Add("-pragmas:strict,downcast");
      foreach (string str in ((IEnumerable<string>) this._island._defines).Distinct<string>())
        arguments.Add("-define:" + str);
      foreach (string reference in this._island._references)
        arguments.Add("-r:" + ScriptCompilerBase.PrepareFileName(reference));
      if (Array.Exists<string>(this._island._references, new Predicate<string>(UnityScriptCompiler.UnityEditorPattern.IsMatch)))
        arguments.Add("-i:UnityEditor");
      else if (!BuildPipeline.IsUnityScriptEvalSupported(this._island._target))
        arguments.Add(string.Format("-disable-eval:eval is not supported on the current build target ({0}).", (object) this._island._target));
      foreach (string file in this._island._files)
        arguments.Add(ScriptCompilerBase.PrepareFileName(file));
      return (Program) this.StartCompiler(this._island._target, Path.Combine(this.GetProfileDirectory(), "us.exe"), arguments);
    }

    private bool StrictBuildTarget()
    {
      return Array.IndexOf<string>(this._island._defines, "ENABLE_DUCK_TYPING") == -1;
    }

    protected override string[] GetStreamContainingCompilerMessages()
    {
      return this.GetStandardOutput();
    }
  }
}
