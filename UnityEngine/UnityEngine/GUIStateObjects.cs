// Decompiled with JetBrains decompiler
// Type: UnityEngine.GUIStateObjects
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Security;

namespace UnityEngine
{
  internal class GUIStateObjects
  {
    private static Dictionary<int, object> s_StateCache = new Dictionary<int, object>();

    [SecuritySafeCritical]
    internal static object GetStateObject(System.Type t, int controlID)
    {
      object instance;
      if (!GUIStateObjects.s_StateCache.TryGetValue(controlID, out instance) || instance.GetType() != t)
      {
        instance = Activator.CreateInstance(t);
        GUIStateObjects.s_StateCache[controlID] = instance;
      }
      return instance;
    }

    internal static object QueryStateObject(System.Type t, int controlID)
    {
      object o = GUIStateObjects.s_StateCache[controlID];
      if (t.IsInstanceOfType(o))
        return o;
      return (object) null;
    }
  }
}
