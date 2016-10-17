// Decompiled with JetBrains decompiler
// Type: GendarmeValidationRule
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Scripting;
using UnityEditor.Scripting.Compilers;
using UnityEditor.Utils;

internal abstract class GendarmeValidationRule : IValidationRule
{
  private readonly string _gendarmeExePath;

  protected GendarmeValidationRule(string gendarmeExePath)
  {
    this._gendarmeExePath = gendarmeExePath;
  }

  public ValidationResult Validate(IEnumerable<string> userAssemblies, params object[] options)
  {
    string arguments = this.BuildGendarmeCommandLineArguments(userAssemblies);
    ValidationResult validationResult = new ValidationResult()
    {
      Success = true,
      Rule = (IValidationRule) this,
      CompilerMessages = (IEnumerable<CompilerMessage>) null
    };
    try
    {
      validationResult.Success = GendarmeValidationRule.StartManagedProgram(this._gendarmeExePath, arguments, (CompilerOutputParserBase) new GendarmeOutputParser(), ref validationResult.CompilerMessages);
    }
    catch (Exception ex)
    {
      validationResult.Success = false;
      // ISSUE: explicit reference operation
      // ISSUE: explicit reference operation
      (^@validationResult).CompilerMessages = (IEnumerable<CompilerMessage>) new CompilerMessage[1]
      {
        new CompilerMessage()
        {
          file = "Exception",
          message = ex.Message,
          line = 0,
          column = 0,
          type = CompilerMessageType.Error
        }
      };
    }
    return validationResult;
  }

  protected abstract GendarmeOptions ConfigureGendarme(IEnumerable<string> userAssemblies);

  protected string BuildGendarmeCommandLineArguments(IEnumerable<string> userAssemblies)
  {
    GendarmeOptions gendarmeOptions = this.ConfigureGendarme(userAssemblies);
    if (gendarmeOptions.UserAssemblies == null || gendarmeOptions.UserAssemblies.Length == 0)
      return (string) null;
    List<string> source = new List<string>()
    {
      "--config " + gendarmeOptions.ConfigFilePath,
      "--set " + gendarmeOptions.RuleSet
    };
    source.AddRange((IEnumerable<string>) gendarmeOptions.UserAssemblies);
    return source.Aggregate<string>((Func<string, string, string>) ((agg, i) => agg + " " + i));
  }

  private static bool StartManagedProgram(string exe, string arguments, CompilerOutputParserBase parser, ref IEnumerable<CompilerMessage> compilerMessages)
  {
    using (ManagedProgram managedProgram = GendarmeValidationRule.ManagedProgramFor(exe, arguments))
    {
      managedProgram.LogProcessStartInfo();
      try
      {
        managedProgram.Start();
      }
      catch
      {
        throw new Exception("Could not start " + exe);
      }
      managedProgram.WaitForExit();
      if (managedProgram.ExitCode == 0)
        return true;
      compilerMessages = parser.Parse(managedProgram.GetErrorOutput(), managedProgram.GetStandardOutput(), true);
    }
    return false;
  }

  private static ManagedProgram ManagedProgramFor(string exe, string arguments)
  {
    return new ManagedProgram(MonoInstallationFinder.GetMonoInstallation("MonoBleedingEdge"), "4.0", exe, arguments);
  }
}
