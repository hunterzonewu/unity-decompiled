// Decompiled with JetBrains decompiler
// Type: UnityEditor.RestService.PairingRestHandler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Diagnostics;
using System.IO;
using UnityEditor.Callbacks;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.RestService
{
  internal class PairingRestHandler : Handler
  {
    protected override JSONValue HandlePost(Request request, JSONValue payload)
    {
      ScriptEditorSettings.ServerURL = payload["url"].AsString();
      ScriptEditorSettings.Name = !payload.ContainsKey("name") ? (string) null : payload["name"].AsString();
      ScriptEditorSettings.ProcessId = !payload.ContainsKey("processid") ? -1 : (int) payload["processid"].AsFloat();
      Logger.Log("[Pair] Name: " + (ScriptEditorSettings.Name ?? "<null>") + " ServerURL " + ScriptEditorSettings.ServerURL + " Process id: " + (object) ScriptEditorSettings.ProcessId);
      JSONValue jsonValue = new JSONValue();
      jsonValue["unityprocessid"] = (JSONValue) Process.GetCurrentProcess().Id;
      jsonValue["unityproject"] = (JSONValue) Application.dataPath;
      return jsonValue;
    }

    internal static void Register()
    {
      Router.RegisterHandler("/unity/pair", (Handler) new PairingRestHandler());
    }

    [OnOpenAsset]
    private static bool OnOpenAsset(int instanceID, int line)
    {
      if (ScriptEditorSettings.ServerURL == null)
        return false;
      string str = Path.GetFullPath(Application.dataPath + "/../" + AssetDatabase.GetAssetPath(instanceID)).Replace('\\', '/');
      string lower = str.ToLower();
      if (!lower.EndsWith(".cs") && !lower.EndsWith(".js") && !lower.EndsWith(".boo"))
        return false;
      if (PairingRestHandler.IsScriptEditorRunning())
      {
        if (RestRequest.Send("/openfile", "{ \"file\" : \"" + str + "\", \"line\" : " + (object) line + " }", 5000))
          return true;
      }
      ScriptEditorSettings.ServerURL = (string) null;
      ScriptEditorSettings.Name = (string) null;
      ScriptEditorSettings.ProcessId = -1;
      return false;
    }

    private static bool IsScriptEditorRunning()
    {
      if (ScriptEditorSettings.ProcessId < 0)
        return false;
      try
      {
        return !Process.GetProcessById(ScriptEditorSettings.ProcessId).HasExited;
      }
      catch (Exception ex)
      {
        Logger.Log(ex);
        return false;
      }
    }
  }
}
