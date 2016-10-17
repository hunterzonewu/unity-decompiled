// Decompiled with JetBrains decompiler
// Type: UnityEngine.CollisionDetectionMode2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Controls how collisions are detected when a Rigidbody2D moves.</para>
  /// </summary>
  public enum CollisionDetectionMode2D
  {
    Discrete = 0,
    [Obsolete("Enum member CollisionDetectionMode2D.None has been deprecated. Use CollisionDetectionMode2D.Discrete instead (UnityUpgradable) -> Discrete", true)] None = 0,
    Continuous = 1,
  }
}
