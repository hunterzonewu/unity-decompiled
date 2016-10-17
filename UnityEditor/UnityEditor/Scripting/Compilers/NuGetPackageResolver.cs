// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.NuGetPackageResolver
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor.Utils;

namespace UnityEditor.Scripting.Compilers
{
  internal sealed class NuGetPackageResolver
  {
    public string PackagesDirectory { get; set; }

    public string ProjectLockFile { get; set; }

    public string TargetMoniker { get; set; }

    public string[] ResolvedReferences { get; private set; }

    public NuGetPackageResolver()
    {
      this.TargetMoniker = "UAP,Version=v10.0";
    }

    private string ConvertToWindowsPath(string path)
    {
      return path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
    }

    public bool EnsureProjectLockFile(string projectFile)
    {
      string directoryName = Path.GetDirectoryName(this.ProjectLockFile);
      string str1 = FileUtil.NiceWinPath(Path.Combine(directoryName, Path.GetFileName(projectFile)));
      Console.WriteLine("Restoring NuGet packages from \"{0}\".", (object) Path.GetFullPath(str1));
      if (File.Exists(this.ProjectLockFile))
      {
        Console.WriteLine("Done. Reusing existing \"{0}\" file.", (object) Path.GetFullPath(this.ProjectLockFile));
        return true;
      }
      if (!string.IsNullOrEmpty(directoryName))
        Directory.CreateDirectory(directoryName);
      File.Copy(projectFile, str1, true);
      string str2 = FileUtil.NiceWinPath(Path.Combine(BuildPipeline.GetBuildToolsDirectory(BuildTarget.WSAPlayer), "nuget.exe"));
      Program program = new Program(new ProcessStartInfo() { Arguments = string.Format("restore \"{0}\" -NonInteractive -Source https://api.nuget.org/v3/index.json", (object) str1), CreateNoWindow = true, FileName = str2 });
      using (program)
      {
        program.Start();
        for (int index = 0; index < 15; ++index)
        {
          if (!program.WaitForExit(60000))
            Console.WriteLine("Still restoring NuGet packages.");
        }
        if (!program.HasExited)
          throw new Exception(string.Format("Failed to restore NuGet packages:{0}Time out.", (object) Environment.NewLine));
        if (program.ExitCode != 0)
          throw new Exception(string.Format("Failed to restore NuGet packages:{0}{1}", (object) Environment.NewLine, (object) program.GetAllOutput()));
      }
      Console.WriteLine("Done.");
      return false;
    }

    public void Resolve()
    {
      Dictionary<string, object> dictionary1 = (Dictionary<string, object>) ((Dictionary<string, object>) ((Dictionary<string, object>) Json.Deserialize(File.ReadAllText(this.ProjectLockFile)))["targets"])[this.TargetMoniker];
      List<string> stringList = new List<string>();
      string windowsPath = this.ConvertToWindowsPath(this.GetPackagesPath());
      using (Dictionary<string, object>.Enumerator enumerator1 = dictionary1.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          KeyValuePair<string, object> current1 = enumerator1.Current;
          Dictionary<string, object> dictionary2 = (Dictionary<string, object>) current1.Value;
          object obj;
          if (dictionary2.TryGetValue("compile", out obj))
          {
            Dictionary<string, object> dictionary3 = (Dictionary<string, object>) obj;
            string[] strArray = current1.Key.Split('/');
            string path2_1 = strArray[0];
            string path2_2 = strArray[1];
            string str = Path.Combine(Path.Combine(windowsPath, path2_1), path2_2);
            if (!Directory.Exists(str))
              throw new Exception(string.Format("Package directory not found: \"{0}\".", (object) str));
            using (Dictionary<string, object>.KeyCollection.Enumerator enumerator2 = dictionary3.Keys.GetEnumerator())
            {
              while (enumerator2.MoveNext())
              {
                string current2 = enumerator2.Current;
                if (!string.Equals(Path.GetFileName(current2), "_._", StringComparison.InvariantCultureIgnoreCase))
                {
                  string path = Path.Combine(str, this.ConvertToWindowsPath(current2));
                  if (!File.Exists(path))
                    throw new Exception(string.Format("Reference not found: \"{0}\".", (object) path));
                  stringList.Add(path);
                }
              }
            }
            if (dictionary2.ContainsKey("frameworkAssemblies"))
              throw new NotImplementedException("Support for \"frameworkAssemblies\" property has not been implemented yet.");
          }
        }
      }
      this.ResolvedReferences = stringList.ToArray();
    }

    private string GetPackagesPath()
    {
      string packagesDirectory = this.PackagesDirectory;
      if (!string.IsNullOrEmpty(packagesDirectory))
        return packagesDirectory;
      string environmentVariable = Environment.GetEnvironmentVariable("NUGET_PACKAGES");
      if (!string.IsNullOrEmpty(environmentVariable))
        return environmentVariable;
      return Path.Combine(Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), ".nuget"), "packages");
    }
  }
}
