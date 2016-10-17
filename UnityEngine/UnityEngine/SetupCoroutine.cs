// Decompiled with JetBrains decompiler
// Type: UnityEngine.SetupCoroutine
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Security;
using UnityEngine.Scripting;

namespace UnityEngine
{
  [RequiredByNativeCode]
  internal class SetupCoroutine
  {
    [RequiredByNativeCode]
    [SecuritySafeCritical]
    public static unsafe void InvokeMoveNext(IEnumerator enumerator, IntPtr returnValueAddress)
    {
      if (returnValueAddress == IntPtr.Zero)
        throw new ArgumentException("Return value address cannot be 0.", "returnValueAddress");
      *(sbyte*) (void*) returnValueAddress = (sbyte) enumerator.MoveNext();
    }

    [RequiredByNativeCode]
    public static object InvokeMember(object behaviour, string name, object variable)
    {
      object[] args = (object[]) null;
      if (variable != null)
        args = new object[1]{ variable };
      return behaviour.GetType().InvokeMember(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod, (Binder) null, behaviour, args, (ParameterModifier[]) null, (CultureInfo) null, (string[]) null);
    }

    public static object InvokeStatic(System.Type klass, string name, object variable)
    {
      object[] args = (object[]) null;
      if (variable != null)
        args = new object[1]{ variable };
      return klass.InvokeMember(name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod, (Binder) null, (object) null, args, (ParameterModifier[]) null, (CultureInfo) null, (string[]) null);
    }
  }
}
