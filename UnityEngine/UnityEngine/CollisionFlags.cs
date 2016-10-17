// Decompiled with JetBrains decompiler
// Type: UnityEngine.CollisionFlags
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

namespace UnityEngine
{
  /// <summary>
  ///   <para>CollisionFlags is a bitmask returned by CharacterController.Move.</para>
  /// </summary>
  public enum CollisionFlags
  {
    None = 0,
    CollidedSides = 1,
    Sides = 1,
    Above = 2,
    CollidedAbove = 2,
    Below = 4,
    CollidedBelow = 4,
  }
}
