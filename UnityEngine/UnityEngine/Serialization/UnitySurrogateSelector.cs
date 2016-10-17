// Decompiled with JetBrains decompiler
// Type: UnityEngine.Serialization.UnitySurrogateSelector
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UnityEngine.Serialization
{
  public class UnitySurrogateSelector : ISurrogateSelector
  {
    public ISerializationSurrogate GetSurrogate(System.Type type, StreamingContext context, out ISurrogateSelector selector)
    {
      if (type.IsGenericType)
      {
        System.Type genericTypeDefinition = type.GetGenericTypeDefinition();
        if (genericTypeDefinition == typeof (List<>))
        {
          selector = (ISurrogateSelector) this;
          return ListSerializationSurrogate.Default;
        }
        if (genericTypeDefinition == typeof (Dictionary<,>))
        {
          selector = (ISurrogateSelector) this;
          return (ISerializationSurrogate) Activator.CreateInstance(typeof (DictionarySerializationSurrogate<,>).MakeGenericType(type.GetGenericArguments()));
        }
      }
      selector = (ISurrogateSelector) null;
      return (ISerializationSurrogate) null;
    }

    public void ChainSelector(ISurrogateSelector selector)
    {
      throw new NotImplementedException();
    }

    public ISurrogateSelector GetNextSelector()
    {
      throw new NotImplementedException();
    }
  }
}
