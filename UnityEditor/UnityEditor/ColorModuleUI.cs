// Decompiled with JetBrains decompiler
// Type: UnityEditor.ColorModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class ColorModuleUI : ModuleUI
  {
    private static ColorModuleUI.Texts s_Texts;
    private SerializedMinMaxGradient m_Gradient;
    private SerializedProperty m_Scale;

    public ColorModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName)
      : base(owner, o, "ColorModule", displayName)
    {
      this.m_ToolTip = "Controls the color of each particle during its lifetime.";
    }

    protected override void Init()
    {
      if (this.m_Gradient != null)
        return;
      this.m_Gradient = new SerializedMinMaxGradient((SerializedModule) this);
      this.m_Gradient.m_AllowColor = false;
      this.m_Gradient.m_AllowRandomBetweenTwoColors = false;
    }

    public override void OnInspectorGUI(ParticleSystem s)
    {
      if (ColorModuleUI.s_Texts == null)
        ColorModuleUI.s_Texts = new ColorModuleUI.Texts();
      this.GUIMinMaxGradient(ColorModuleUI.s_Texts.color, this.m_Gradient);
    }

    private class Texts
    {
      public GUIContent color = new GUIContent("Color", "Controls the color of each particle during its lifetime.");
    }
  }
}
