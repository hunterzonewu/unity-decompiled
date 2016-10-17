// Decompiled with JetBrains decompiler
// Type: UnityEditor.RestService.ScriptEditorSettings
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.RestService
{
  internal class ScriptEditorSettings
  {
    public static string Name { get; set; }

    public static string ServerURL { get; set; }

    public static int ProcessId { get; set; }

    public static List<string> OpenDocuments { get; set; }

    private static string FilePath
    {
      get
      {
        return Application.dataPath + "/../Library/UnityScriptEditorSettings.json";
      }
    }

    static ScriptEditorSettings()
    {
      ScriptEditorSettings.OpenDocuments = new List<string>();
      ScriptEditorSettings.Clear();
    }

    private static void Clear()
    {
      ScriptEditorSettings.Name = (string) null;
      ScriptEditorSettings.ServerURL = (string) null;
      ScriptEditorSettings.ProcessId = -1;
    }

    public static void Save()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendFormat("{{\n\t\"name\" : \"{0}\",\n\t\"serverurl\" : \"{1}\",\n\t\"processid\" : {2},\n\t", (object) ScriptEditorSettings.Name, (object) ScriptEditorSettings.ServerURL, (object) ScriptEditorSettings.ProcessId);
      stringBuilder.AppendFormat("\"opendocuments\" : [{0}]\n}}", (object) string.Join(",", ScriptEditorSettings.OpenDocuments.Select<string, string>((Func<string, string>) (d => "\"" + d + "\"")).ToArray<string>()));
      File.WriteAllText(ScriptEditorSettings.FilePath, stringBuilder.ToString());
    }

    public static void Load()
    {
      try
      {
        JSONValue jsonValue = new JSONParser(File.ReadAllText(ScriptEditorSettings.FilePath)).Parse();
        ScriptEditorSettings.Name = !jsonValue.ContainsKey("name") ? (string) null : jsonValue["name"].AsString();
        ScriptEditorSettings.ServerURL = !jsonValue.ContainsKey("serverurl") ? (string) null : jsonValue["serverurl"].AsString();
        ScriptEditorSettings.ProcessId = !jsonValue.ContainsKey("processid") ? -1 : (int) jsonValue["processid"].AsFloat();
        ScriptEditorSettings.OpenDocuments = !jsonValue.ContainsKey("opendocuments") ? new List<string>() : jsonValue["opendocuments"].AsList().Select<JSONValue, string>((Func<JSONValue, string>) (d => d.AsString())).ToList<string>();
        if (ScriptEditorSettings.ProcessId < 0)
          return;
        Process.GetProcessById(ScriptEditorSettings.ProcessId);
      }
      catch (FileNotFoundException ex)
      {
        ScriptEditorSettings.Clear();
        ScriptEditorSettings.Save();
      }
      catch (Exception ex)
      {
        Logger.Log(ex);
        ScriptEditorSettings.Clear();
        ScriptEditorSettings.Save();
      }
    }
  }
}
