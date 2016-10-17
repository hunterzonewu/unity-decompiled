// Decompiled with JetBrains decompiler
// Type: UnityEngine.ClothSkinningCoefficient
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

namespace UnityEngine
{
  /// <summary>
  ///   <para>The ClothSkinningCoefficient struct is used to set up how a Cloth component is allowed to move with respect to the SkinnedMeshRenderer it is attached to.</para>
  /// </summary>
  public struct ClothSkinningCoefficient
  {
    /// <summary>
    ///   <para>Distance a vertex is allowed to travel from the skinned mesh vertex position.</para>
    /// </summary>
    public float maxDistance;
    /// <summary>
    ///   <para>Definition of a sphere a vertex is not allowed to enter. This allows collision against the animated cloth.</para>
    /// </summary>
    public float collisionSphereDistance;
  }
}
