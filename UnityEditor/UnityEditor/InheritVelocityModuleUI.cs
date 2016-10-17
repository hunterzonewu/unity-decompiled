// Decompiled with JetBrains decompiler
// Type: UnityEditor.InheritVelocityModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  internal class InheritVelocityModuleUI : ModuleUI
  {
    private SerializedProperty m_Mode;
    private SerializedMinMaxCurve m_Curve;
    private static InheritVelocityModuleUI.Texts s_Texts;

    public InheritVelocityModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName)
      : base(owner, o, "InheritVelocityModule", displayName)
    {
      this.m_ToolTip = "Controls the velocity inherited from the emitter, for each particle.";
    }

    protected override void Init()
    {
      if (this.m_Curve != null)
        return;
      if (InheritVelocityModuleUI.s_Texts == null)
        InheritVelocityModuleUI.s_Texts = new InheritVelocityModuleUI.Texts();
      this.m_Mode = this.GetProperty("m_Mode");
      this.m_Curve = new SerializedMinMaxCurve((ModuleUI) this, GUIContent.none, "m_Curve", ModuleUI.kUseSignedRange);
    }

    private bool CanInheritVelocity(ParticleSystem s)
    {
      return !((IEnumerable<Rigidbody>) s.GetComponentsInParent<Rigidbody>()).Any<Rigidbody>((Func<Rigidbody, bool>) (o =>
      {
        if (!o.isKinematic)
          return o.interpolation == RigidbodyInterpolation.None;
        return false;
      })) && !((IEnumerable<Rigidbody2D>) s.GetComponentsInParent<Rigidbody2D>()).Any<Rigidbody2D>((Func<Rigidbody2D, bool>) (o =>
      {
        if (!o.isKinematic)
          return o.interpolation == RigidbodyInterpolation2D.None;
        return false;
      }));
    }

    public override void OnInspectorGUI(ParticleSystem s)
    {
      if (InheritVelocityModuleUI.s_Texts == null)
        InheritVelocityModuleUI.s_Texts = new InheritVelocityModuleUI.Texts();
      ModuleUI.GUIPopup(InheritVelocityModuleUI.s_Texts.mode, this.m_Mode, InheritVelocityModuleUI.s_Texts.modes);
      ModuleUI.GUIMinMaxCurve(GUIContent.none, this.m_Curve);
      if ((double) this.m_Curve.scalar.floatValue == 0.0 || this.CanInheritVelocity(s))
        return;
      EditorGUILayout.HelpBox(EditorGUIUtility.TextContent("Inherit velocity requires interpolation enabled on the rigidbody to function correctly.").text, MessageType.Warning, true);
    }

    public override void UpdateCullingSupportedString(ref string text)
    {
      this.Init();
      if (this.m_Curve.SupportsProcedural())
        return;
      text = text + "\n\tInherited velocity curves use too many keys.";
    }

    private enum Modes
    {
      Initial,
      Current,
    }

    private class Texts
    {
      public GUIContent mode = new GUIContent("Mode", "Specifies whether the emitter velocity is inherited as a one-shot when a particle is born, always using the current emitter velocity, or using the emitter velocity when the particle was born.");
      public string[] modes = new string[2]{ "Initial", "Current" };
    }
  }
}
