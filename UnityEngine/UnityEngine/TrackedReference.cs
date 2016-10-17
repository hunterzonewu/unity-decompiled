// Decompiled with JetBrains decompiler
// Type: UnityEngine.TrackedReference
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.InteropServices;

namespace UnityEngine
{
  [StructLayout(LayoutKind.Sequential)]
  public class TrackedReference
  {
    internal IntPtr m_Ptr;

    protected TrackedReference()
    {
    }

    public static implicit operator bool(TrackedReference exists)
    {
      return exists != (TrackedReference) null;
    }

    public static bool operator ==(TrackedReference x, TrackedReference y)
    {
      object obj1 = (object) x;
      object obj2 = (object) y;
      if (obj2 == null && obj1 == null)
        return true;
      if (obj2 == null)
        return x.m_Ptr == IntPtr.Zero;
      if (obj1 == null)
        return y.m_Ptr == IntPtr.Zero;
      return x.m_Ptr == y.m_Ptr;
    }

    public static bool operator !=(TrackedReference x, TrackedReference y)
    {
      return !(x == y);
    }

    public override bool Equals(object o)
    {
      return o as TrackedReference == this;
    }

    public override int GetHashCode()
    {
      return (int) this.m_Ptr;
    }
  }
}
