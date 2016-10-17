// Decompiled with JetBrains decompiler
// Type: UnityEngine.MaterialGlobalIlluminationFlags
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>How the material should interact with lightmaps and lightprobes.</para>
  /// </summary>
  [Flags]
  public enum MaterialGlobalIlluminationFlags
  {
    None = 0,
    RealtimeEmissive = 1,
    BakedEmissive = 2,
    EmissiveIsBlack = 4,
  }
}
