// Decompiled with JetBrains decompiler
// Type: UnityEditor.EmissionModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class EmissionModuleUI : ModuleUI
  {
    private SerializedProperty[] m_BurstTime = new SerializedProperty[4];
    private SerializedProperty[] m_BurstParticleMinCount = new SerializedProperty[4];
    private SerializedProperty[] m_BurstParticleMaxCount = new SerializedProperty[4];
    private string[] m_GuiNames = new string[2]{ "Time", "Distance" };
    private const int k_MaxNumBursts = 4;
    private SerializedProperty m_Type;
    public SerializedMinMaxCurve m_Rate;
    private SerializedProperty m_BurstCount;
    private static EmissionModuleUI.Texts s_Texts;

    public EmissionModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName)
      : base(owner, o, "EmissionModule", displayName)
    {
      this.m_ToolTip = "Emission of the emitter. This controls the rate at which particles are emitted as well as burst emissions.";
    }

    protected override void Init()
    {
      if (EmissionModuleUI.s_Texts == null)
        EmissionModuleUI.s_Texts = new EmissionModuleUI.Texts();
      if (this.m_BurstCount != null)
        return;
      this.m_Type = this.GetProperty("m_Type");
      this.m_Rate = new SerializedMinMaxCurve((ModuleUI) this, EmissionModuleUI.s_Texts.rate, "rate");
      this.m_BurstTime[0] = this.GetProperty("time0");
      this.m_BurstTime[1] = this.GetProperty("time1");
      this.m_BurstTime[2] = this.GetProperty("time2");
      this.m_BurstTime[3] = this.GetProperty("time3");
      this.m_BurstParticleMinCount[0] = this.GetProperty("cnt0");
      this.m_BurstParticleMinCount[1] = this.GetProperty("cnt1");
      this.m_BurstParticleMinCount[2] = this.GetProperty("cnt2");
      this.m_BurstParticleMinCount[3] = this.GetProperty("cnt3");
      this.m_BurstParticleMaxCount[0] = this.GetProperty("cntmax0");
      this.m_BurstParticleMaxCount[1] = this.GetProperty("cntmax1");
      this.m_BurstParticleMaxCount[2] = this.GetProperty("cntmax2");
      this.m_BurstParticleMaxCount[3] = this.GetProperty("cntmax3");
      this.m_BurstCount = this.GetProperty("m_BurstCount");
    }

    public override void OnInspectorGUI(ParticleSystem s)
    {
      ModuleUI.GUIMinMaxCurve(EmissionModuleUI.s_Texts.rate, this.m_Rate);
      ModuleUI.GUIPopup(GUIContent.none, this.m_Type, this.m_GuiNames);
      if (this.m_Type.intValue == 1)
        return;
      this.DoBurstGUI(s);
    }

    private void DoBurstGUI(ParticleSystem s)
    {
      EditorGUILayout.Space();
      Rect controlRect1 = ModuleUI.GetControlRect(13);
      GUI.Label(controlRect1, EmissionModuleUI.s_Texts.burst, ParticleSystemStyles.Get().label);
      float dragWidth = 20f;
      float num1 = 40f;
      float width = (float) (((double) num1 + (double) dragWidth) * 3.0 + (double) dragWidth - 1.0);
      float num2 = Mathf.Min(controlRect1.width - width, EditorGUIUtility.labelWidth);
      int intValue1 = this.m_BurstCount.intValue;
      Rect position = new Rect(controlRect1.x + num2, controlRect1.y, width, 3f);
      GUI.Label(position, GUIContent.none, ParticleSystemStyles.Get().line);
      Rect rect = new Rect(controlRect1.x + dragWidth + num2, controlRect1.y, num1 + dragWidth, controlRect1.height);
      GUI.Label(rect, "Time", ParticleSystemStyles.Get().label);
      rect.x += dragWidth + num1;
      GUI.Label(rect, "Min", ParticleSystemStyles.Get().label);
      rect.x += dragWidth + num1;
      GUI.Label(rect, "Max", ParticleSystemStyles.Get().label);
      position.y += 12f;
      GUI.Label(position, GUIContent.none, ParticleSystemStyles.Get().line);
      float duration = s.duration;
      int num3 = intValue1;
      for (int index = 0; index < intValue1; ++index)
      {
        SerializedProperty floatProp = this.m_BurstTime[index];
        SerializedProperty serializedProperty1 = this.m_BurstParticleMinCount[index];
        SerializedProperty serializedProperty2 = this.m_BurstParticleMaxCount[index];
        Rect controlRect2 = ModuleUI.GetControlRect(13);
        rect = new Rect(controlRect2.x + num2, controlRect2.y, dragWidth + num1, controlRect2.height);
        float num4 = ModuleUI.FloatDraggable(rect, floatProp, 1f, dragWidth, "n2");
        if ((double) num4 < 0.0)
          floatProp.floatValue = 0.0f;
        if ((double) num4 > (double) duration)
          floatProp.floatValue = duration;
        int intValue2 = serializedProperty1.intValue;
        int intValue3 = serializedProperty2.intValue;
        rect.x += rect.width;
        serializedProperty1.intValue = ModuleUI.IntDraggable(rect, (GUIContent) null, intValue2, dragWidth);
        rect.x += rect.width;
        serializedProperty2.intValue = ModuleUI.IntDraggable(rect, (GUIContent) null, intValue3, dragWidth);
        if (index == intValue1 - 1)
        {
          rect.x = position.xMax - 12f;
          if (ModuleUI.MinusButton(rect))
            --intValue1;
        }
      }
      if (intValue1 < 4)
      {
        rect = ModuleUI.GetControlRect(13);
        rect.xMin = rect.xMax - 12f;
        if (ModuleUI.PlusButton(rect))
          ++intValue1;
      }
      if (intValue1 == num3)
        return;
      this.m_BurstCount.intValue = intValue1;
    }

    public override void UpdateCullingSupportedString(ref string text)
    {
      this.Init();
      if (this.m_Type.intValue != 1)
        return;
      text = text + "\n\tEmission is distance based.";
    }

    private enum EmissionTypes
    {
      Time,
      Distance,
    }

    private class Texts
    {
      public GUIContent rate = new GUIContent("Rate", "The number of particles emitted per second (Time) or per distance unit (Distance)");
      public GUIContent burst = new GUIContent("Bursts", "Emission of extra particles at specific times during the duration of the system.");
    }
  }
}
