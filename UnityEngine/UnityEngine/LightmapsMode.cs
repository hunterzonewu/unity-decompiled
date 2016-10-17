// Decompiled with JetBrains decompiler
// Type: UnityEngine.LightmapsMode
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.ComponentModel;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Lightmap (and lighting) configuration mode, controls how lightmaps interact with lighting and what kind of information they store.</para>
  /// </summary>
  public enum LightmapsMode
  {
    NonDirectional = 0,
    [Obsolete("Enum member LightmapsMode.Single has been deprecated. Use NonDirectional instead (UnityUpgradable) -> NonDirectional", true), EditorBrowsable(EditorBrowsableState.Never)] Single = 0,
    CombinedDirectional = 1,
    [Obsolete("Enum member LightmapsMode.Dual has been deprecated. Use CombinedDirectional instead (UnityUpgradable) -> CombinedDirectional", true), EditorBrowsable(EditorBrowsableState.Never)] Dual = 1,
    [EditorBrowsable(EditorBrowsableState.Never), Obsolete("Enum member LightmapsMode.Directional has been deprecated. Use SeparateDirectional instead (UnityUpgradable) -> SeparateDirectional", true)] Directional = 2,
    SeparateDirectional = 2,
  }
}
