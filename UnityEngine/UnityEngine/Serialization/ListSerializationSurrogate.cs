// Decompiled with JetBrains decompiler
// Type: UnityEngine.Serialization.ListSerializationSurrogate
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Collections;
using System.Runtime.Serialization;

namespace UnityEngine.Serialization
{
  internal class ListSerializationSurrogate : ISerializationSurrogate
  {
    public static readonly ISerializationSurrogate Default = (ISerializationSurrogate) new ListSerializationSurrogate();

    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
      IList list = (IList) obj;
      info.AddValue("_size", list.Count);
      info.AddValue("_items", (object) ListSerializationSurrogate.ArrayFromGenericList(list));
      info.AddValue("_version", 0);
    }

    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
      IList instance = (IList) Activator.CreateInstance(obj.GetType());
      int int32 = info.GetInt32("_size");
      if (int32 == 0)
        return (object) instance;
      IEnumerator enumerator = ((IEnumerable) info.GetValue("_items", typeof (IEnumerable))).GetEnumerator();
      for (int index = 0; index < int32; ++index)
      {
        if (!enumerator.MoveNext())
          throw new InvalidOperationException();
        instance.Add(enumerator.Current);
      }
      return (object) instance;
    }

    private static Array ArrayFromGenericList(IList list)
    {
      Array instance = Array.CreateInstance(list.GetType().GetGenericArguments()[0], list.Count);
      list.CopyTo(instance, 0);
      return instance;
    }
  }
}
