// Decompiled with JetBrains decompiler
// Type: UnityEditor.ClampVelocityModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class ClampVelocityModuleUI : ModuleUI
  {
    private SerializedMinMaxCurve m_X;
    private SerializedMinMaxCurve m_Y;
    private SerializedMinMaxCurve m_Z;
    private SerializedMinMaxCurve m_Magnitude;
    private SerializedProperty m_SeparateAxes;
    private SerializedProperty m_InWorldSpace;
    private SerializedProperty m_Dampen;
    private static ClampVelocityModuleUI.Texts s_Texts;

    public ClampVelocityModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName)
      : base(owner, o, "ClampVelocityModule", displayName)
    {
      this.m_ToolTip = "Controls the velocity limit and damping of each particle during its lifetime.";
    }

    protected override void Init()
    {
      if (this.m_X != null)
        return;
      if (ClampVelocityModuleUI.s_Texts == null)
        ClampVelocityModuleUI.s_Texts = new ClampVelocityModuleUI.Texts();
      this.m_X = new SerializedMinMaxCurve((ModuleUI) this, ClampVelocityModuleUI.s_Texts.x, "x", ModuleUI.kUseSignedRange);
      this.m_Y = new SerializedMinMaxCurve((ModuleUI) this, ClampVelocityModuleUI.s_Texts.y, "y", ModuleUI.kUseSignedRange);
      this.m_Z = new SerializedMinMaxCurve((ModuleUI) this, ClampVelocityModuleUI.s_Texts.z, "z", ModuleUI.kUseSignedRange);
      this.m_Magnitude = new SerializedMinMaxCurve((ModuleUI) this, ClampVelocityModuleUI.s_Texts.magnitude, "magnitude");
      this.m_SeparateAxes = this.GetProperty("separateAxis");
      this.m_InWorldSpace = this.GetProperty("inWorldSpace");
      this.m_Dampen = this.GetProperty("dampen");
    }

    public override void OnInspectorGUI(ParticleSystem s)
    {
      EditorGUI.BeginChangeCheck();
      bool flag = ModuleUI.GUIToggle(ClampVelocityModuleUI.s_Texts.separateAxes, this.m_SeparateAxes);
      if (EditorGUI.EndChangeCheck())
      {
        if (flag)
        {
          this.m_Magnitude.RemoveCurveFromEditor();
        }
        else
        {
          this.m_X.RemoveCurveFromEditor();
          this.m_Y.RemoveCurveFromEditor();
          this.m_Z.RemoveCurveFromEditor();
        }
      }
      if (flag)
      {
        this.GUITripleMinMaxCurve(GUIContent.none, ClampVelocityModuleUI.s_Texts.x, this.m_X, ClampVelocityModuleUI.s_Texts.y, this.m_Y, ClampVelocityModuleUI.s_Texts.z, this.m_Z, (SerializedProperty) null);
        ModuleUI.GUIBoolAsPopup(ClampVelocityModuleUI.s_Texts.space, this.m_InWorldSpace, ClampVelocityModuleUI.s_Texts.spaces);
      }
      else
        ModuleUI.GUIMinMaxCurve(ClampVelocityModuleUI.s_Texts.magnitude, this.m_Magnitude);
      double num = (double) ModuleUI.GUIFloat(ClampVelocityModuleUI.s_Texts.dampen, this.m_Dampen);
    }

    public override void UpdateCullingSupportedString(ref string text)
    {
      text = text + "\n\tLimit velocity is enabled.";
    }

    private class Texts
    {
      public GUIContent x = new GUIContent("X");
      public GUIContent y = new GUIContent("Y");
      public GUIContent z = new GUIContent("Z");
      public GUIContent dampen = new GUIContent("Dampen", "Controls how much the velocity that exceeds the velocity limit should be dampened. A value of 0.5 will dampen the exceeding velocity by 50%.");
      public GUIContent magnitude = new GUIContent("  Speed", "The speed limit of particles over the particle lifetime.");
      public GUIContent separateAxes = new GUIContent("Separate Axes", "If enabled, you can control the velocity limit separately for each axis.");
      public GUIContent space = new GUIContent("  Space", "Specifies if the velocity values are in local space (rotated with the transform) or world space.");
      public string[] spaces = new string[2]{ "Local", "World" };
    }
  }
}
