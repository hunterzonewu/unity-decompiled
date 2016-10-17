// Decompiled with JetBrains decompiler
// Type: UnityEditor.RestService.AssetRestHandler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditorInternal;

namespace UnityEditor.RestService
{
  internal class AssetRestHandler
  {
    internal static void Register()
    {
      Router.RegisterHandler("/unity/assets", (Handler) new AssetRestHandler.LibraryHandler());
      Router.RegisterHandler("/unity/assets/*", (Handler) new AssetRestHandler.AssetHandler());
    }

    internal class AssetHandler : Handler
    {
      protected override JSONValue HandleDelete(Request request, JSONValue payload)
      {
        if (!AssetDatabase.DeleteAsset(request.Url.Substring("/unity/".Length)))
          throw new RestRequestException() { HttpStatusCode = HttpStatusCode.InternalServerError, RestErrorString = "FailedDeletingAsset", RestErrorDescription = "DeleteAsset() returned false" };
        return new JSONValue();
      }

      protected override JSONValue HandlePost(Request request, JSONValue payload)
      {
        string str = payload.Get("action").AsString();
        string key = str;
        if (key != null)
        {
          // ISSUE: reference to a compiler-generated field
          if (AssetRestHandler.AssetHandler.\u003C\u003Ef__switch\u0024map9 == null)
          {
            // ISSUE: reference to a compiler-generated field
            AssetRestHandler.AssetHandler.\u003C\u003Ef__switch\u0024map9 = new Dictionary<string, int>(2)
            {
              {
                "move",
                0
              },
              {
                "create",
                1
              }
            };
          }
          int num;
          // ISSUE: reference to a compiler-generated field
          if (AssetRestHandler.AssetHandler.\u003C\u003Ef__switch\u0024map9.TryGetValue(key, out num))
          {
            if (num != 0)
            {
              if (num == 1)
                this.CreateAsset(request.Url.Substring("/unity/".Length), Encoding.UTF8.GetString(Convert.FromBase64String(payload.Get("contents").AsString())));
              else
                goto label_8;
            }
            else
              this.MoveAsset(request.Url.Substring("/unity/".Length), payload.Get("newpath").AsString());
            return new JSONValue();
          }
        }
label_8:
        throw new RestRequestException() { HttpStatusCode = HttpStatusCode.BadRequest, RestErrorString = "Uknown action: " + str };
      }

      internal bool MoveAsset(string from, string to)
      {
        string str = AssetDatabase.MoveAsset(from, to);
        if (str.Length > 0)
          throw new RestRequestException(HttpStatusCode.BadRequest, "MoveAsset failed with error: " + str);
        return str.Length == 0;
      }

      internal void CreateAsset(string assetPath, string contents)
      {
        string fullPath = Path.GetFullPath(assetPath);
        try
        {
          using (StreamWriter streamWriter = new StreamWriter((Stream) File.OpenWrite(fullPath)))
          {
            streamWriter.Write(contents);
            streamWriter.Close();
          }
        }
        catch (Exception ex)
        {
          throw new RestRequestException(HttpStatusCode.BadRequest, "FailedCreatingAsset", "Caught exception: " + (object) ex);
        }
      }

      protected override JSONValue HandleGet(Request request, JSONValue payload)
      {
        int num = request.Url.ToLowerInvariant().IndexOf("/assets/");
        return this.GetAssetText(request.Url.ToLowerInvariant().Substring(num + 1));
      }

      internal JSONValue GetAssetText(string assetPath)
      {
        UnityEngine.Object @object = AssetDatabase.LoadAssetAtPath(assetPath, typeof (UnityEngine.Object));
        if (@object == (UnityEngine.Object) null)
          throw new RestRequestException(HttpStatusCode.BadRequest, "AssetNotFound");
        JSONValue jsonValue = new JSONValue();
        jsonValue["file"] = (JSONValue) assetPath;
        jsonValue["contents"] = (JSONValue) Convert.ToBase64String(Encoding.UTF8.GetBytes(@object.ToString()));
        return jsonValue;
      }
    }

    internal class LibraryHandler : Handler
    {
      protected override JSONValue HandleGet(Request request, JSONValue payload)
      {
        JSONValue jsonValue = new JSONValue();
        jsonValue["assets"] = Handler.ToJSON((IEnumerable<string>) AssetDatabase.FindAssets(string.Empty, new string[1]{ "Assets" }));
        return jsonValue;
      }
    }
  }
}
