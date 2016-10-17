// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.APIUpdaterHelper
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Utils;
using UnityEditor.VersionControl;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.Scripting.Compilers
{
  internal class APIUpdaterHelper
  {
    private const string tempOutputPath = "Temp/ScriptUpdater/";

    public static void UpdateScripts(string responseFile, string sourceExtension)
    {
      if (!ScriptUpdatingManager.WaitForVCSServerConnection(true))
        return;
      string str = !Provider.enabled ? "." : "Temp/ScriptUpdater/";
      APIUpdaterHelper.RunUpdatingProgram("ScriptUpdater.exe", sourceExtension + " " + CommandLineFormatter.PrepareFileName(MonoInstallationFinder.GetFrameWorksFolder()) + " " + str + " " + responseFile);
    }

    private static void RunUpdatingProgram(string executable, string arguments)
    {
      ManagedProgram program = new ManagedProgram(MonoInstallationFinder.GetMonoInstallation("MonoBleedingEdge"), "4.5", EditorApplication.applicationContentsPath + "/Tools/ScriptUpdater/" + executable, arguments);
      program.LogProcessStartInfo();
      program.Start();
      program.WaitForExit();
      Console.WriteLine(string.Join(Environment.NewLine, program.GetStandardOutput()));
      APIUpdaterHelper.HandleUpdaterReturnValue(program);
    }

    private static void HandleUpdaterReturnValue(ManagedProgram program)
    {
      if (program.ExitCode == 0)
      {
        APIUpdaterHelper.UpdateFilesInVCIfNeeded();
      }
      else
      {
        ScriptUpdatingManager.ReportExpectedUpdateFailure();
        if (program.ExitCode > 0)
          APIUpdaterHelper.ReportAPIUpdaterFailure((IEnumerable<string>) program.GetErrorOutput());
        else
          APIUpdaterHelper.ReportAPIUpdaterCrash((IEnumerable<string>) program.GetErrorOutput());
      }
    }

    private static void ReportAPIUpdaterCrash(IEnumerable<string> errorOutput)
    {
      Debug.LogErrorFormat("Failed to run script updater.{0}Please, report a bug to Unity with these details{0}{1}", new object[2]
      {
        (object) Environment.NewLine,
        (object) errorOutput.Aggregate<string, string>(string.Empty, (Func<string, string, string>) ((acc, curr) => acc + Environment.NewLine + "\t" + curr))
      });
    }

    private static void ReportAPIUpdaterFailure(IEnumerable<string> errorOutput)
    {
      ScriptUpdatingManager.ReportGroupedAPIUpdaterFailure(string.Format("APIUpdater encountered some issues and was not able to finish.{0}{1}", (object) Environment.NewLine, (object) errorOutput.Aggregate<string, string>(string.Empty, (Func<string, string, string>) ((acc, curr) => acc + Environment.NewLine + "\t" + curr))));
    }

    private static void UpdateFilesInVCIfNeeded()
    {
      if (!Provider.enabled)
        return;
      string[] files = Directory.GetFiles("Temp/ScriptUpdater/", "*.*", SearchOption.AllDirectories);
      AssetList assets = new AssetList();
      foreach (string str in files)
        assets.Add(Provider.GetAssetByPath(str.Replace("Temp/ScriptUpdater/", string.Empty)));
      Task task = Provider.Checkout(assets, CheckoutMode.Exact);
      task.Wait();
      IEnumerable<Asset> source = task.assetList.Where<Asset>((Func<Asset, bool>) (a => (a.state & Asset.States.ReadOnly) == Asset.States.ReadOnly));
      if (!task.success || source.Any<Asset>())
      {
        Debug.LogErrorFormat("[API Updater] Files cannot be updated (failed to check out): {0}", (object) source.Select<Asset, string>((Func<Asset, string>) (a => a.fullName + " (" + (object) a.state + ")")).Aggregate<string>((Func<string, string, string>) ((acc, curr) => acc + Environment.NewLine + "\t" + curr)));
        ScriptUpdatingManager.ReportExpectedUpdateFailure();
      }
      else
      {
        FileUtil.CopyDirectoryRecursive("Temp/ScriptUpdater/", ".", true);
        FileUtil.DeleteFileOrDirectory("Temp/ScriptUpdater/");
      }
    }
  }
}
