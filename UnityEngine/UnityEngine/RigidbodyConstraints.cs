// Decompiled with JetBrains decompiler
// Type: UnityEngine.RigidbodyConstraints
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

namespace UnityEngine
{
  /// <summary>
  ///   <para>Use these flags to constrain motion of Rigidbodies.</para>
  /// </summary>
  public enum RigidbodyConstraints
  {
    None = 0,
    FreezePositionX = 2,
    FreezePositionY = 4,
    FreezePositionZ = 8,
    FreezePosition = 14,
    FreezeRotationX = 16,
    FreezeRotationY = 32,
    FreezeRotationZ = 64,
    FreezeRotation = 112,
    FreezeAll = 126,
  }
}
