// Decompiled with JetBrains decompiler
// Type: UnityEditor.LegacyIlluminShaderGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class LegacyIlluminShaderGUI : ShaderGUI
  {
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
    {
      base.OnGUI(materialEditor, props);
      materialEditor.LightmapEmissionProperty(0);
      foreach (Material target in materialEditor.targets)
        target.globalIlluminationFlags &= ~MaterialGlobalIlluminationFlags.EmissiveIsBlack;
    }
  }
}
