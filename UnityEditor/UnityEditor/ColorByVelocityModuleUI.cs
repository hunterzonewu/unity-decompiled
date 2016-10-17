// Decompiled with JetBrains decompiler
// Type: UnityEditor.ColorByVelocityModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class ColorByVelocityModuleUI : ModuleUI
  {
    private static ColorByVelocityModuleUI.Texts s_Texts;
    private SerializedMinMaxGradient m_Gradient;
    private SerializedProperty m_Range;
    private SerializedProperty m_Scale;

    public ColorByVelocityModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName)
      : base(owner, o, "ColorBySpeedModule", displayName)
    {
      this.m_ToolTip = "Controls the color of each particle based on its speed.";
    }

    protected override void Init()
    {
      if (this.m_Gradient != null)
        return;
      this.m_Gradient = new SerializedMinMaxGradient((SerializedModule) this);
      this.m_Gradient.m_AllowColor = false;
      this.m_Gradient.m_AllowRandomBetweenTwoColors = false;
      this.m_Range = this.GetProperty("range");
    }

    public override void OnInspectorGUI(ParticleSystem s)
    {
      if (ColorByVelocityModuleUI.s_Texts == null)
        ColorByVelocityModuleUI.s_Texts = new ColorByVelocityModuleUI.Texts();
      this.GUIMinMaxGradient(ColorByVelocityModuleUI.s_Texts.color, this.m_Gradient);
      ModuleUI.GUIMinMaxRange(ColorByVelocityModuleUI.s_Texts.velocityRange, this.m_Range);
    }

    private class Texts
    {
      public GUIContent color = new GUIContent("Color", "Controls the color of each particle based on its speed.");
      public GUIContent velocityRange = new GUIContent("Speed Range", "Remaps speed in the defined range to a color.");
    }
  }
}
