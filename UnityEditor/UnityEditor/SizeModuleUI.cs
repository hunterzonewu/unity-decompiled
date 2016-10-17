// Decompiled with JetBrains decompiler
// Type: UnityEditor.SizeModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class SizeModuleUI : ModuleUI
  {
    private GUIContent m_SizeText = new GUIContent("Size", "Controls the size of each particle during its lifetime.");
    private SerializedMinMaxCurve m_Curve;

    public SizeModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName)
      : base(owner, o, "SizeModule", displayName)
    {
      this.m_ToolTip = "Controls the size of each particle during its lifetime.";
    }

    protected override void Init()
    {
      if (this.m_Curve != null)
        return;
      this.m_Curve = new SerializedMinMaxCurve((ModuleUI) this, this.m_SizeText);
      this.m_Curve.m_AllowConstant = false;
    }

    public override void OnInspectorGUI(ParticleSystem s)
    {
      ModuleUI.GUIMinMaxCurve(this.m_SizeText, this.m_Curve);
    }
  }
}
