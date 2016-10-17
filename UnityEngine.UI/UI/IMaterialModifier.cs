// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.IMaterialModifier
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

namespace UnityEngine.UI
{
  public interface IMaterialModifier
  {
    /// <summary>
    ///   <para>Perform material modification in this function.</para>
    /// </summary>
    /// <param name="baseMaterial">Configured Material.</param>
    /// <returns>
    ///   <para>Modified material.</para>
    /// </returns>
    Material GetModifiedMaterial(Material baseMaterial);
  }
}
