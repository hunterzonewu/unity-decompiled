// Decompiled with JetBrains decompiler
// Type: UnityEngine.CppDefineAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
  internal class CppDefineAttribute : Attribute
  {
    public CppDefineAttribute(string symbol, string value)
    {
    }
  }
}
