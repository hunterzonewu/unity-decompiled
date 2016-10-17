// Decompiled with JetBrains decompiler
// Type: UnityEngineInternal.NetFxCoreExtensions
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Reflection;

namespace UnityEngineInternal
{
  internal static class NetFxCoreExtensions
  {
    public static Delegate CreateDelegate(this MethodInfo self, Type delegateType, object target)
    {
      return Delegate.CreateDelegate(delegateType, target, self);
    }

    public static MethodInfo GetMethodInfo(this Delegate self)
    {
      return self.Method;
    }
  }
}
