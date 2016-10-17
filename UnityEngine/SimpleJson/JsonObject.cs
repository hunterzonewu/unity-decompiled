// Decompiled with JetBrains decompiler
// Type: SimpleJson.JsonObject
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace SimpleJson
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  [GeneratedCode("simple-json", "1.0.0")]
  internal class JsonObject : IDictionary<string, object>, IEnumerable<KeyValuePair<string, object>>, IEnumerable, ICollection<KeyValuePair<string, object>>
  {
    private readonly Dictionary<string, object> _members;

    public object this[int index]
    {
      get
      {
        return JsonObject.GetAtIndex((IDictionary<string, object>) this._members, index);
      }
    }

    public ICollection<string> Keys
    {
      get
      {
        return (ICollection<string>) this._members.Keys;
      }
    }

    public ICollection<object> Values
    {
      get
      {
        return (ICollection<object>) this._members.Values;
      }
    }

    public object this[string key]
    {
      get
      {
        return this._members[key];
      }
      set
      {
        this._members[key] = value;
      }
    }

    public int Count
    {
      get
      {
        return this._members.Count;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return false;
      }
    }

    public JsonObject()
    {
      this._members = new Dictionary<string, object>();
    }

    public JsonObject(IEqualityComparer<string> comparer)
    {
      this._members = new Dictionary<string, object>(comparer);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this._members.GetEnumerator();
    }

    internal static object GetAtIndex(IDictionary<string, object> obj, int index)
    {
      if (obj == null)
        throw new ArgumentNullException("obj");
      if (index >= obj.Count)
        throw new ArgumentOutOfRangeException("index");
      int num = 0;
      foreach (KeyValuePair<string, object> keyValuePair in (IEnumerable<KeyValuePair<string, object>>) obj)
      {
        if (num++ == index)
          return keyValuePair.Value;
      }
      return (object) null;
    }

    public void Add(string key, object value)
    {
      this._members.Add(key, value);
    }

    public bool ContainsKey(string key)
    {
      return this._members.ContainsKey(key);
    }

    public bool Remove(string key)
    {
      return this._members.Remove(key);
    }

    public bool TryGetValue(string key, out object value)
    {
      return this._members.TryGetValue(key, out value);
    }

    public void Add(KeyValuePair<string, object> item)
    {
      this._members.Add(item.Key, item.Value);
    }

    public void Clear()
    {
      this._members.Clear();
    }

    public bool Contains(KeyValuePair<string, object> item)
    {
      if (this._members.ContainsKey(item.Key))
        return this._members[item.Key] == item.Value;
      return false;
    }

    public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
    {
      if (array == null)
        throw new ArgumentNullException("array");
      int count = this.Count;
      foreach (KeyValuePair<string, object> keyValuePair in this)
      {
        array[arrayIndex++] = keyValuePair;
        if (--count <= 0)
          break;
      }
    }

    public bool Remove(KeyValuePair<string, object> item)
    {
      return this._members.Remove(item.Key);
    }

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
    {
      return (IEnumerator<KeyValuePair<string, object>>) this._members.GetEnumerator();
    }

    public override string ToString()
    {
      return SimpleJson.SimpleJson.SerializeObject((object) this);
    }
  }
}
