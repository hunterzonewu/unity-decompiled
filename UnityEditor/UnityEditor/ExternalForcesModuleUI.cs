// Decompiled with JetBrains decompiler
// Type: UnityEditor.ExternalForcesModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class ExternalForcesModuleUI : ModuleUI
  {
    private SerializedProperty m_Multiplier;
    private static ExternalForcesModuleUI.Texts s_Texts;

    public ExternalForcesModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName)
      : base(owner, o, "ExternalForcesModule", displayName)
    {
      this.m_ToolTip = "Controls the wind zones that each particle is affected by.";
    }

    protected override void Init()
    {
      if (this.m_Multiplier != null)
        return;
      if (ExternalForcesModuleUI.s_Texts == null)
        ExternalForcesModuleUI.s_Texts = new ExternalForcesModuleUI.Texts();
      this.m_Multiplier = this.GetProperty("multiplier");
    }

    public override void OnInspectorGUI(ParticleSystem s)
    {
      if (ExternalForcesModuleUI.s_Texts == null)
        ExternalForcesModuleUI.s_Texts = new ExternalForcesModuleUI.Texts();
      double num = (double) ModuleUI.GUIFloat(ExternalForcesModuleUI.s_Texts.multiplier, this.m_Multiplier);
    }

    public override void UpdateCullingSupportedString(ref string text)
    {
      text = text + "\n\tExternal Forces is enabled.";
    }

    private class Texts
    {
      public GUIContent multiplier = new GUIContent("Multiplier", "Used to scale the force applied to this particle system.");
    }
  }
}
