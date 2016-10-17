// Decompiled with JetBrains decompiler
// Type: UnityEditor.VelocityModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class VelocityModuleUI : ModuleUI
  {
    private SerializedMinMaxCurve m_X;
    private SerializedMinMaxCurve m_Y;
    private SerializedMinMaxCurve m_Z;
    private SerializedProperty m_InWorldSpace;
    private static VelocityModuleUI.Texts s_Texts;

    public VelocityModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName)
      : base(owner, o, "VelocityModule", displayName)
    {
      this.m_ToolTip = "Controls the velocity of each particle during its lifetime.";
    }

    protected override void Init()
    {
      if (this.m_X != null)
        return;
      if (VelocityModuleUI.s_Texts == null)
        VelocityModuleUI.s_Texts = new VelocityModuleUI.Texts();
      this.m_X = new SerializedMinMaxCurve((ModuleUI) this, VelocityModuleUI.s_Texts.x, "x", ModuleUI.kUseSignedRange);
      this.m_Y = new SerializedMinMaxCurve((ModuleUI) this, VelocityModuleUI.s_Texts.y, "y", ModuleUI.kUseSignedRange);
      this.m_Z = new SerializedMinMaxCurve((ModuleUI) this, VelocityModuleUI.s_Texts.z, "z", ModuleUI.kUseSignedRange);
      this.m_InWorldSpace = this.GetProperty("inWorldSpace");
    }

    public override void OnInspectorGUI(ParticleSystem s)
    {
      if (VelocityModuleUI.s_Texts == null)
        VelocityModuleUI.s_Texts = new VelocityModuleUI.Texts();
      this.GUITripleMinMaxCurve(GUIContent.none, VelocityModuleUI.s_Texts.x, this.m_X, VelocityModuleUI.s_Texts.y, this.m_Y, VelocityModuleUI.s_Texts.z, this.m_Z, (SerializedProperty) null);
      ModuleUI.GUIBoolAsPopup(VelocityModuleUI.s_Texts.space, this.m_InWorldSpace, VelocityModuleUI.s_Texts.spaces);
    }

    public override void UpdateCullingSupportedString(ref string text)
    {
      this.Init();
      if (this.m_X.SupportsProcedural() && this.m_Y.SupportsProcedural() && this.m_Z.SupportsProcedural())
        return;
      text = text + "\n\tLifetime velocity curves use too many keys.";
    }

    private class Texts
    {
      public GUIContent x = new GUIContent("X");
      public GUIContent y = new GUIContent("Y");
      public GUIContent z = new GUIContent("Z");
      public GUIContent space = new GUIContent("Space", "Specifies if the velocity values are in local space (rotated with the transform) or world space.");
      public string[] spaces = new string[2]{ "Local", "World" };
    }
  }
}
