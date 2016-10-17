// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.MonoScriptCompilerBase
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using System.IO;
using UnityEditor.Utils;

namespace UnityEditor.Scripting.Compilers
{
  internal abstract class MonoScriptCompilerBase : ScriptCompilerBase
  {
    private readonly bool runUpdater;

    protected MonoScriptCompilerBase(MonoIsland island, bool runUpdater)
      : base(island)
    {
      this.runUpdater = runUpdater;
    }

    protected ManagedProgram StartCompiler(BuildTarget target, string compiler, List<string> arguments)
    {
      return this.StartCompiler(target, compiler, arguments, true);
    }

    protected ManagedProgram StartCompiler(BuildTarget target, string compiler, List<string> arguments, bool setMonoEnvironmentVariables)
    {
      this.AddCustomResponseFileIfPresent(arguments, Path.GetFileNameWithoutExtension(compiler) + ".rsp");
      string responseFile = CommandLineFormatter.GenerateResponseFile((IEnumerable<string>) arguments);
      if (this.runUpdater)
        APIUpdaterHelper.UpdateScripts(responseFile, this._island.GetExtensionOfSourceFiles());
      ManagedProgram managedProgram = new ManagedProgram(MonoInstallationFinder.GetMonoInstallation(), this._island._classlib_profile, compiler, " @" + responseFile, setMonoEnvironmentVariables);
      managedProgram.Start();
      return managedProgram;
    }

    protected string GetProfileDirectory()
    {
      return MonoInstallationFinder.GetProfileDirectory(this._island._target, this._island._classlib_profile);
    }
  }
}
