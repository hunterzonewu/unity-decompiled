// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.Match.ResponseBase
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Collections.Generic;

namespace UnityEngine.Networking.Match
{
  /// <summary>
  ///   <para>A response object base.</para>
  /// </summary>
  public abstract class ResponseBase
  {
    public abstract void Parse(object obj);

    internal string ParseJSONString(string name, object obj, IDictionary<string, object> dictJsonObj)
    {
      if (dictJsonObj.TryGetValue(name, out obj))
        return obj as string;
      throw new FormatException(name + " not found in JSON dictionary");
    }

    internal short ParseJSONInt16(string name, object obj, IDictionary<string, object> dictJsonObj)
    {
      if (dictJsonObj.TryGetValue(name, out obj))
        return Convert.ToInt16(obj);
      throw new FormatException(name + " not found in JSON dictionary");
    }

    internal int ParseJSONInt32(string name, object obj, IDictionary<string, object> dictJsonObj)
    {
      if (dictJsonObj.TryGetValue(name, out obj))
        return Convert.ToInt32(obj);
      throw new FormatException(name + " not found in JSON dictionary");
    }

    internal long ParseJSONInt64(string name, object obj, IDictionary<string, object> dictJsonObj)
    {
      if (dictJsonObj.TryGetValue(name, out obj))
        return Convert.ToInt64(obj);
      throw new FormatException(name + " not found in JSON dictionary");
    }

    internal ushort ParseJSONUInt16(string name, object obj, IDictionary<string, object> dictJsonObj)
    {
      if (dictJsonObj.TryGetValue(name, out obj))
        return Convert.ToUInt16(obj);
      throw new FormatException(name + " not found in JSON dictionary");
    }

    internal uint ParseJSONUInt32(string name, object obj, IDictionary<string, object> dictJsonObj)
    {
      if (dictJsonObj.TryGetValue(name, out obj))
        return Convert.ToUInt32(obj);
      throw new FormatException(name + " not found in JSON dictionary");
    }

    internal ulong ParseJSONUInt64(string name, object obj, IDictionary<string, object> dictJsonObj)
    {
      if (dictJsonObj.TryGetValue(name, out obj))
        return Convert.ToUInt64(obj);
      throw new FormatException(name + " not found in JSON dictionary");
    }

    internal bool ParseJSONBool(string name, object obj, IDictionary<string, object> dictJsonObj)
    {
      if (dictJsonObj.TryGetValue(name, out obj))
        return Convert.ToBoolean(obj);
      throw new FormatException(name + " not found in JSON dictionary");
    }

    internal DateTime ParseJSONDateTime(string name, object obj, IDictionary<string, object> dictJsonObj)
    {
      throw new FormatException(name + " DateTime not yet supported");
    }

    internal List<string> ParseJSONListOfStrings(string name, object obj, IDictionary<string, object> dictJsonObj)
    {
      if (dictJsonObj.TryGetValue(name, out obj))
      {
        List<object> objectList = obj as List<object>;
        if (objectList != null)
        {
          List<string> stringList = new List<string>();
          using (List<object>.Enumerator enumerator = objectList.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              foreach (KeyValuePair<string, object> keyValuePair in (IEnumerable<KeyValuePair<string, object>>) enumerator.Current)
              {
                string str = (string) keyValuePair.Value;
                stringList.Add(str);
              }
            }
          }
          return stringList;
        }
      }
      throw new FormatException(name + " not found in JSON dictionary");
    }

    internal List<T> ParseJSONList<T>(string name, object obj, IDictionary<string, object> dictJsonObj) where T : ResponseBase, new()
    {
      if (dictJsonObj.TryGetValue(name, out obj))
      {
        List<object> objectList = obj as List<object>;
        if (objectList != null)
        {
          List<T> objList = new List<T>();
          using (List<object>.Enumerator enumerator = objectList.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              IDictionary<string, object> current = (IDictionary<string, object>) enumerator.Current;
              T instance = Activator.CreateInstance<T>();
              instance.Parse((object) current);
              objList.Add(instance);
            }
          }
          return objList;
        }
      }
      throw new FormatException(name + " not found in JSON dictionary");
    }
  }
}
