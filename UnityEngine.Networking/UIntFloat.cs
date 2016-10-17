// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.UIntFloat
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System.Runtime.InteropServices;

namespace UnityEngine.Networking
{
  [StructLayout(LayoutKind.Explicit)]
  internal struct UIntFloat
  {
    [FieldOffset(0)]
    public float floatValue;
    [FieldOffset(0)]
    public uint intValue;
    [FieldOffset(0)]
    public double doubleValue;
    [FieldOffset(0)]
    public ulong longValue;
  }
}
