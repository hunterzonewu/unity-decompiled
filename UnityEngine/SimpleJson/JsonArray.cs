// Decompiled with JetBrains decompiler
// Type: SimpleJson.JsonArray
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;

namespace SimpleJson
{
  [GeneratedCode("simple-json", "1.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  internal class JsonArray : List<object>
  {
    public JsonArray()
    {
    }

    public JsonArray(int capacity)
      : base(capacity)
    {
    }

    public override string ToString()
    {
      return SimpleJson.SimpleJson.SerializeObject((object) this) ?? string.Empty;
    }
  }
}
