// Decompiled with JetBrains decompiler
// Type: UnityEngine.Types
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Reflection;

namespace UnityEngine
{
  public static class Types
  {
    public static System.Type GetType(string typeName, string assemblyName)
    {
      try
      {
        return Assembly.Load(assemblyName).GetType(typeName);
      }
      catch (Exception ex)
      {
        return (System.Type) null;
      }
    }
  }
}
