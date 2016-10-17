// Decompiled with JetBrains decompiler
// Type: UnityEditor.RestService.OpenDocumentsRestHandler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;

namespace UnityEditor.RestService
{
  internal class OpenDocumentsRestHandler : Handler
  {
    protected override JSONValue HandlePost(Request request, JSONValue payload)
    {
      ScriptEditorSettings.OpenDocuments = !payload.ContainsKey("documents") ? new List<string>() : payload["documents"].AsList().Select<JSONValue, string>((Func<JSONValue, string>) (d => d.AsString())).ToList<string>();
      ScriptEditorSettings.Save();
      return new JSONValue();
    }

    protected override JSONValue HandleGet(Request request, JSONValue payload)
    {
      JSONValue jsonValue = new JSONValue();
      jsonValue["documents"] = Handler.ToJSON((IEnumerable<string>) ScriptEditorSettings.OpenDocuments);
      return jsonValue;
    }

    internal static void Register()
    {
      Router.RegisterHandler("/unity/opendocuments", (Handler) new OpenDocumentsRestHandler());
    }
  }
}
