// Decompiled with JetBrains decompiler
// Type: UnityEngine.WeakListenerBindings
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine
{
  internal sealed class WeakListenerBindings
  {
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void InvokeCallbacks(object inst, GCHandle gchandle, object[] parameters);
  }
}
