// Decompiled with JetBrains decompiler
// Type: UnityEngine.jvalue
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.InteropServices;

namespace UnityEngine
{
  [StructLayout(LayoutKind.Explicit)]
  public struct jvalue
  {
    [FieldOffset(0)]
    public bool z;
    [FieldOffset(0)]
    public byte b;
    [FieldOffset(0)]
    public char c;
    [FieldOffset(0)]
    public short s;
    [FieldOffset(0)]
    public int i;
    [FieldOffset(0)]
    public long j;
    [FieldOffset(0)]
    public float f;
    [FieldOffset(0)]
    public double d;
    [FieldOffset(0)]
    public IntPtr l;
  }
}
