// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.FloatConversion
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

namespace UnityEngine.Networking
{
  internal class FloatConversion
  {
    public static float ToSingle(uint value)
    {
      return new UIntFloat() { intValue = value }.floatValue;
    }

    public static double ToDouble(ulong value)
    {
      return new UIntFloat() { longValue = value }.doubleValue;
    }
  }
}
