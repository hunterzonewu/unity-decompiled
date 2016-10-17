// Decompiled with JetBrains decompiler
// Type: UnityEngine.Events.BaseInvokableCall
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Reflection;

namespace UnityEngine.Events
{
  internal abstract class BaseInvokableCall
  {
    protected BaseInvokableCall()
    {
    }

    protected BaseInvokableCall(object target, MethodInfo function)
    {
      if (target == null)
        throw new ArgumentNullException("target");
      if (function == null)
        throw new ArgumentNullException("function");
    }

    public abstract void Invoke(object[] args);

    protected static void ThrowOnInvalidArg<T>(object arg)
    {
      if (arg != null && !(arg is T))
        throw new ArgumentException(UnityString.Format("Passed argument 'args[0]' is of the wrong type. Type:{0} Expected:{1}", (object) arg.GetType(), (object) typeof (T)));
    }

    protected static bool AllowInvoke(Delegate @delegate)
    {
      object target = @delegate.Target;
      if (target == null)
        return true;
      UnityEngine.Object @object = target as UnityEngine.Object;
      if (!object.ReferenceEquals((object) @object, (object) null))
        return @object != (UnityEngine.Object) null;
      return true;
    }

    public abstract bool Find(object targetObj, MethodInfo method);
  }
}
