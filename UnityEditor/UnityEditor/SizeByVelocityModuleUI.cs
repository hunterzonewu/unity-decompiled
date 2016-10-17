// Decompiled with JetBrains decompiler
// Type: UnityEditor.SizeByVelocityModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class SizeByVelocityModuleUI : ModuleUI
  {
    private static SizeByVelocityModuleUI.Texts s_Texts;
    private SerializedMinMaxCurve m_Curve;
    private SerializedProperty m_Range;

    public SizeByVelocityModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName)
      : base(owner, o, "SizeBySpeedModule", displayName)
    {
      this.m_ToolTip = "Controls the size of each particle based on its speed.";
    }

    protected override void Init()
    {
      if (this.m_Curve != null)
        return;
      if (SizeByVelocityModuleUI.s_Texts == null)
        SizeByVelocityModuleUI.s_Texts = new SizeByVelocityModuleUI.Texts();
      this.m_Curve = new SerializedMinMaxCurve((ModuleUI) this, SizeByVelocityModuleUI.s_Texts.size);
      this.m_Curve.m_AllowConstant = false;
      this.m_Range = this.GetProperty("range");
    }

    public override void OnInspectorGUI(ParticleSystem s)
    {
      if (SizeByVelocityModuleUI.s_Texts == null)
        SizeByVelocityModuleUI.s_Texts = new SizeByVelocityModuleUI.Texts();
      ModuleUI.GUIMinMaxCurve(SizeByVelocityModuleUI.s_Texts.size, this.m_Curve);
      ModuleUI.GUIMinMaxRange(SizeByVelocityModuleUI.s_Texts.velocityRange, this.m_Range);
    }

    private class Texts
    {
      public GUIContent velocityRange = new GUIContent("Speed Range", "Remaps speed in the defined range to a size.");
      public GUIContent size = new GUIContent("Size", "Controls the size of each particle based on its speed.");
    }
  }
}
