// Decompiled with JetBrains decompiler
// Type: UnityEngine.LODFadeMode
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

namespace UnityEngine
{
  /// <summary>
  ///   <para>The LOD fade modes. Modes other than LODFadeMode.None will result in Unity calculating a blend factor for blending/interpolating between two neighbouring LODs and pass it to your shader.</para>
  /// </summary>
  public enum LODFadeMode
  {
    None,
    CrossFade,
    SpeedTree,
  }
}
