// Decompiled with JetBrains decompiler
// Type: UnityEditor.RestService.ProjectStateRestHandler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Scripting;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.RestService
{
  internal class ProjectStateRestHandler : Handler
  {
    private static string ProjectPath
    {
      get
      {
        return Path.GetDirectoryName(Application.dataPath);
      }
    }

    private static string AssetsPath
    {
      get
      {
        return ProjectStateRestHandler.ProjectPath + "/Assets";
      }
    }

    protected override JSONValue HandleGet(Request request, JSONValue payload)
    {
      AssetDatabase.Refresh();
      return ProjectStateRestHandler.JsonForProject();
    }

    private static JSONValue JsonForProject()
    {
      List<ProjectStateRestHandler.Island> list = ((IEnumerable<MonoIsland>) InternalEditorUtility.GetMonoIslands()).Select<MonoIsland, ProjectStateRestHandler.Island>((Func<MonoIsland, ProjectStateRestHandler.Island>) (i => new ProjectStateRestHandler.Island() { MonoIsland = i, Name = Path.GetFileNameWithoutExtension(i._output), References = ((IEnumerable<string>) i._references).ToList<string>() })).ToList<ProjectStateRestHandler.Island>();
      using (List<ProjectStateRestHandler.Island>.Enumerator enumerator1 = list.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          ProjectStateRestHandler.Island current1 = enumerator1.Current;
          List<string> stringList1 = new List<string>();
          List<string> stringList2 = new List<string>();
          using (List<string>.Enumerator enumerator2 = current1.References.GetEnumerator())
          {
            while (enumerator2.MoveNext())
            {
              string current2 = enumerator2.Current;
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              ProjectStateRestHandler.\u003CJsonForProject\u003Ec__AnonStorey25 projectCAnonStorey25 = new ProjectStateRestHandler.\u003CJsonForProject\u003Ec__AnonStorey25();
              // ISSUE: reference to a compiler-generated field
              projectCAnonStorey25.refName = Path.GetFileNameWithoutExtension(current2);
              // ISSUE: reference to a compiler-generated method
              if (current2.StartsWith("Library/") && list.Any<ProjectStateRestHandler.Island>(new Func<ProjectStateRestHandler.Island, bool>(projectCAnonStorey25.\u003C\u003Em__31)))
              {
                // ISSUE: reference to a compiler-generated field
                stringList1.Add(projectCAnonStorey25.refName);
                stringList2.Add(current2);
              }
              if (current2.EndsWith("/UnityEditor.dll") || current2.EndsWith("/UnityEngine.dll") || (current2.EndsWith("\\UnityEditor.dll") || current2.EndsWith("\\UnityEngine.dll")))
                stringList2.Add(current2);
            }
          }
          current1.References.Add(InternalEditorUtility.GetEditorAssemblyPath());
          current1.References.Add(InternalEditorUtility.GetEngineAssemblyPath());
          using (List<string>.Enumerator enumerator2 = stringList1.GetEnumerator())
          {
            while (enumerator2.MoveNext())
            {
              string current2 = enumerator2.Current;
              current1.References.Add(current2);
            }
          }
          using (List<string>.Enumerator enumerator2 = stringList2.GetEnumerator())
          {
            while (enumerator2.MoveNext())
            {
              string current2 = enumerator2.Current;
              current1.References.Remove(current2);
            }
          }
        }
      }
      string[] array = list.SelectMany<ProjectStateRestHandler.Island, string>((Func<ProjectStateRestHandler.Island, IEnumerable<string>>) (i => (IEnumerable<string>) i.MonoIsland._files)).Concat<string>(ProjectStateRestHandler.GetAllSupportedFiles()).Distinct<string>().ToArray<string>();
      string[] projectPath = ProjectStateRestHandler.RelativeToProjectPath(ProjectStateRestHandler.FindEmptyDirectories(ProjectStateRestHandler.AssetsPath, array));
      JSONValue jsonValue1 = new JSONValue();
      jsonValue1["islands"] = new JSONValue((object) list.Select<ProjectStateRestHandler.Island, JSONValue>((Func<ProjectStateRestHandler.Island, JSONValue>) (i => ProjectStateRestHandler.JsonForIsland(i))).Where<JSONValue>((Func<JSONValue, bool>) (i2 => !i2.IsNull())).ToList<JSONValue>());
      jsonValue1["basedirectory"] = (JSONValue) ProjectStateRestHandler.ProjectPath;
      JSONValue jsonValue2 = new JSONValue();
      jsonValue2["files"] = Handler.ToJSON((IEnumerable<string>) array);
      jsonValue2["emptydirectories"] = Handler.ToJSON((IEnumerable<string>) projectPath);
      jsonValue1["assetdatabase"] = jsonValue2;
      return jsonValue1;
    }

    private static bool IsSupportedExtension(string extension)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ProjectStateRestHandler.\u003CIsSupportedExtension\u003Ec__AnonStorey26 extensionCAnonStorey26 = new ProjectStateRestHandler.\u003CIsSupportedExtension\u003Ec__AnonStorey26();
      // ISSUE: reference to a compiler-generated field
      extensionCAnonStorey26.extension = extension;
      // ISSUE: reference to a compiler-generated field
      if (extensionCAnonStorey26.extension.StartsWith("."))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        extensionCAnonStorey26.extension = extensionCAnonStorey26.extension.Substring(1);
      }
      // ISSUE: reference to a compiler-generated method
      return ((IEnumerable<string>) EditorSettings.projectGenerationBuiltinExtensions).Concat<string>((IEnumerable<string>) EditorSettings.projectGenerationUserExtensions).Any<string>(new Func<string, bool>(extensionCAnonStorey26.\u003C\u003Em__35));
    }

    private static IEnumerable<string> GetAllSupportedFiles()
    {
      return ((IEnumerable<string>) AssetDatabase.GetAllAssetPaths()).Where<string>((Func<string, bool>) (asset => ProjectStateRestHandler.IsSupportedExtension(Path.GetExtension(asset))));
    }

    private static JSONValue JsonForIsland(ProjectStateRestHandler.Island island)
    {
      if (island.Name == "UnityEngine" || island.Name == "UnityEditor")
        return (JSONValue) ((string) null);
      JSONValue jsonValue = new JSONValue();
      jsonValue["name"] = (JSONValue) island.Name;
      jsonValue["language"] = (JSONValue) (!island.Name.Contains("Boo") ? (!island.Name.Contains("UnityScript") ? "C#" : "UnityScript") : "Boo");
      jsonValue["files"] = Handler.ToJSON((IEnumerable<string>) island.MonoIsland._files);
      jsonValue["defines"] = Handler.ToJSON((IEnumerable<string>) island.MonoIsland._defines);
      jsonValue["references"] = Handler.ToJSON((IEnumerable<string>) island.MonoIsland._references);
      jsonValue["basedirectory"] = (JSONValue) ProjectStateRestHandler.ProjectPath;
      return jsonValue;
    }

    private static void FindPotentialEmptyDirectories(string path, ICollection<string> result)
    {
      string[] directories = Directory.GetDirectories(path);
      if (directories.Length == 0)
      {
        result.Add(path.Replace('\\', '/'));
      }
      else
      {
        foreach (string path1 in directories)
          ProjectStateRestHandler.FindPotentialEmptyDirectories(path1, result);
      }
    }

    private static IEnumerable<string> FindPotentialEmptyDirectories(string path)
    {
      List<string> stringList = new List<string>();
      ProjectStateRestHandler.FindPotentialEmptyDirectories(path, (ICollection<string>) stringList);
      return (IEnumerable<string>) stringList;
    }

    private static string[] FindEmptyDirectories(string path, string[] files)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return ProjectStateRestHandler.FindPotentialEmptyDirectories(path).Where<string>(new Func<string, bool>(new ProjectStateRestHandler.\u003CFindEmptyDirectories\u003Ec__AnonStorey27() { files = files }.\u003C\u003Em__37)).ToArray<string>();
    }

    private static string[] RelativeToProjectPath(string[] paths)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return ((IEnumerable<string>) paths).Select<string, string>(new Func<string, string>(new ProjectStateRestHandler.\u003CRelativeToProjectPath\u003Ec__AnonStorey29() { projectPath = !ProjectStateRestHandler.ProjectPath.EndsWith("/") ? ProjectStateRestHandler.ProjectPath + "/" : ProjectStateRestHandler.ProjectPath }.\u003C\u003Em__38)).ToArray<string>();
    }

    internal static void Register()
    {
      Router.RegisterHandler("/unity/projectstate", (Handler) new ProjectStateRestHandler());
    }

    public class Island
    {
      public MonoIsland MonoIsland { get; set; }

      public string Name { get; set; }

      public List<string> References { get; set; }
    }
  }
}
