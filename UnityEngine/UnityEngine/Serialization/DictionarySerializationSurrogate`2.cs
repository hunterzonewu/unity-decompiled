// Decompiled with JetBrains decompiler
// Type: UnityEngine.Serialization.DictionarySerializationSurrogate`2
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UnityEngine.Serialization
{
  internal class DictionarySerializationSurrogate<TKey, TValue> : ISerializationSurrogate
  {
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
      ((Dictionary<TKey, TValue>) obj).GetObjectData(info, context);
    }

    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
      Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>((IEqualityComparer<TKey>) info.GetValue("Comparer", typeof (IEqualityComparer<TKey>)));
      if (info.MemberCount > 3)
      {
        KeyValuePair<TKey, TValue>[] keyValuePairArray = (KeyValuePair<TKey, TValue>[]) info.GetValue("KeyValuePairs", typeof (KeyValuePair<TKey, TValue>[]));
        if (keyValuePairArray != null)
        {
          foreach (KeyValuePair<TKey, TValue> keyValuePair in keyValuePairArray)
            dictionary.Add(keyValuePair.Key, keyValuePair.Value);
        }
      }
      return (object) dictionary;
    }
  }
}
