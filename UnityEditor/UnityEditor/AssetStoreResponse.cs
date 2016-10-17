// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetStoreResponse
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEditorInternal;

namespace UnityEditor
{
  internal class AssetStoreResponse
  {
    internal AsyncHTTPClient job;
    public Dictionary<string, JSONValue> dict;
    public bool ok;

    public bool failed
    {
      get
      {
        return !this.ok;
      }
    }

    public string message
    {
      get
      {
        if (this.dict == null || !this.dict.ContainsKey("message"))
          return (string) null;
        return this.dict["message"].AsString(true);
      }
    }

    private static string EncodeString(string str)
    {
      str = str.Replace("\"", "\\\"");
      str = str.Replace("\\", "\\\\");
      str = str.Replace("\b", "\\b");
      str = str.Replace("\f", "\\f");
      str = str.Replace("\n", "\\n");
      str = str.Replace("\r", "\\r");
      str = str.Replace("\t", "\\t");
      return str;
    }

    public override string ToString()
    {
      string str1 = "{";
      string str2 = string.Empty;
      using (Dictionary<string, JSONValue>.Enumerator enumerator = this.dict.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<string, JSONValue> current = enumerator.Current;
          str1 = str1 + str2 + (object) '"' + AssetStoreResponse.EncodeString(current.Key) + "\" : " + current.Value.ToString();
          str2 = ", ";
        }
      }
      return str1 + "}";
    }
  }
}
