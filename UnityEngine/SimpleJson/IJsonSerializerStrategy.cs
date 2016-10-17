// Decompiled with JetBrains decompiler
// Type: SimpleJson.IJsonSerializerStrategy
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.CodeDom.Compiler;
using System.Diagnostics.CodeAnalysis;

namespace SimpleJson
{
  [GeneratedCode("simple-json", "1.0.0")]
  internal interface IJsonSerializerStrategy
  {
    [SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate", Justification = "Need to support .NET 2")]
    bool TrySerializeNonPrimitiveObject(object input, out object output);

    object DeserializeObject(object value, Type type);
  }
}
