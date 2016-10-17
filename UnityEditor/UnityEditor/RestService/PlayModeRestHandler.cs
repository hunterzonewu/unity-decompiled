// Decompiled with JetBrains decompiler
// Type: UnityEditor.RestService.PlayModeRestHandler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEditorInternal;

namespace UnityEditor.RestService
{
  internal class PlayModeRestHandler : Handler
  {
    protected override JSONValue HandlePost(Request request, JSONValue payload)
    {
      string str1 = payload.Get("action").AsString();
      string str2 = this.CurrentState();
      string key = str1;
      if (key != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (PlayModeRestHandler.\u003C\u003Ef__switch\u0024mapA == null)
        {
          // ISSUE: reference to a compiler-generated field
          PlayModeRestHandler.\u003C\u003Ef__switch\u0024mapA = new Dictionary<string, int>(3)
          {
            {
              "play",
              0
            },
            {
              "pause",
              1
            },
            {
              "stop",
              2
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (PlayModeRestHandler.\u003C\u003Ef__switch\u0024mapA.TryGetValue(key, out num))
        {
          switch (num)
          {
            case 0:
              EditorApplication.isPlaying = true;
              EditorApplication.isPaused = false;
              break;
            case 1:
              EditorApplication.isPaused = true;
              break;
            case 2:
              EditorApplication.isPlaying = false;
              break;
            default:
              goto label_8;
          }
          JSONValue jsonValue = new JSONValue();
          jsonValue["oldstate"] = (JSONValue) str2;
          jsonValue["newstate"] = (JSONValue) this.CurrentState();
          return jsonValue;
        }
      }
label_8:
      throw new RestRequestException() { HttpStatusCode = HttpStatusCode.BadRequest, RestErrorString = "Invalid action: " + str1 };
    }

    protected override JSONValue HandleGet(Request request, JSONValue payload)
    {
      JSONValue jsonValue = new JSONValue();
      jsonValue["state"] = (JSONValue) this.CurrentState();
      return jsonValue;
    }

    internal static void Register()
    {
      Router.RegisterHandler("/unity/playmode", (Handler) new PlayModeRestHandler());
    }

    internal string CurrentState()
    {
      if (!EditorApplication.isPlayingOrWillChangePlaymode)
        return "stopped";
      return EditorApplication.isPaused ? "paused" : "playing";
    }
  }
}
