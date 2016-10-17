// Decompiled with JetBrains decompiler
// Type: UnityEngineInternal.MathfInternal
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

namespace UnityEngineInternal
{
  public struct MathfInternal
  {
    public static volatile float FloatMinNormal = 1.175494E-38f;
    public static volatile float FloatMinDenormal = float.Epsilon;
    public static bool IsFlushToZeroEnabled = (double) MathfInternal.FloatMinDenormal == 0.0;
  }
}
