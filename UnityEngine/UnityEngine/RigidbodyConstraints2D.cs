// Decompiled with JetBrains decompiler
// Type: UnityEngine.RigidbodyConstraints2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Use these flags to constrain motion of the Rigidbody2D.</para>
  /// </summary>
  [Flags]
  public enum RigidbodyConstraints2D
  {
    None = 0,
    FreezePositionX = 1,
    FreezePositionY = 2,
    FreezeRotation = 4,
    FreezePosition = FreezePositionY | FreezePositionX,
    FreezeAll = FreezePosition | FreezeRotation,
  }
}
