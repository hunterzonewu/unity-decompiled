// Decompiled with JetBrains decompiler
// Type: UnityEditor.SerializedMinMaxCurve
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class SerializedMinMaxCurve
  {
    public float m_MaxAllowedScalar = float.PositiveInfinity;
    public SerializedProperty scalar;
    public SerializedProperty maxCurve;
    public SerializedProperty minCurve;
    public SerializedProperty minCurveFirstKeyValue;
    public SerializedProperty maxCurveFirstKeyValue;
    private SerializedProperty minMaxState;
    public ModuleUI m_Module;
    private string m_Name;
    public GUIContent m_DisplayName;
    private bool m_SignedRange;
    public float m_DefaultCurveScalar;
    public float m_RemapValue;
    public bool m_AllowConstant;
    public bool m_AllowRandom;
    public bool m_AllowCurves;

    public MinMaxCurveState state
    {
      get
      {
        return (MinMaxCurveState) this.minMaxState.intValue;
      }
      set
      {
        this.SetMinMaxState(value);
      }
    }

    public bool signedRange
    {
      get
      {
        return this.m_SignedRange;
      }
    }

    public float maxConstant
    {
      get
      {
        return this.maxCurveFirstKeyValue.floatValue * this.scalar.floatValue;
      }
      set
      {
        value = this.ClampValueToMaxAllowed(value);
        if (!this.signedRange)
          value = Mathf.Max(value, 0.0f);
        float minConstant = this.minConstant;
        float num1 = Mathf.Abs(minConstant);
        float num2 = Mathf.Abs(value);
        float newScalar = (double) num2 <= (double) num1 ? num1 : num2;
        if ((double) newScalar != (double) this.scalar.floatValue)
          this.SetScalarAndNormalizedConstants(newScalar, minConstant, value);
        else
          this.SetNormalizedConstant(this.maxCurve, value);
      }
    }

    public float minConstant
    {
      get
      {
        return this.minCurveFirstKeyValue.floatValue * this.scalar.floatValue;
      }
      set
      {
        value = this.ClampValueToMaxAllowed(value);
        if (!this.signedRange)
          value = Mathf.Max(value, 0.0f);
        float maxConstant = this.maxConstant;
        float num1 = Mathf.Abs(value);
        float num2 = Mathf.Abs(maxConstant);
        float newScalar = (double) num2 <= (double) num1 ? num1 : num2;
        if ((double) newScalar != (double) this.scalar.floatValue)
          this.SetScalarAndNormalizedConstants(newScalar, value, maxConstant);
        else
          this.SetNormalizedConstant(this.minCurve, value);
      }
    }

    public SerializedMinMaxCurve(ModuleUI m, GUIContent displayName)
    {
      this.Init(m, displayName, "curve", false, false);
    }

    public SerializedMinMaxCurve(ModuleUI m, GUIContent displayName, string name)
    {
      this.Init(m, displayName, name, false, false);
    }

    public SerializedMinMaxCurve(ModuleUI m, GUIContent displayName, bool signedRange)
    {
      this.Init(m, displayName, "curve", signedRange, false);
    }

    public SerializedMinMaxCurve(ModuleUI m, GUIContent displayName, string name, bool signedRange)
    {
      this.Init(m, displayName, name, signedRange, false);
    }

    public SerializedMinMaxCurve(ModuleUI m, GUIContent displayName, string name, bool signedRange, bool useProp0)
    {
      this.Init(m, displayName, name, signedRange, useProp0);
    }

    private void Init(ModuleUI m, GUIContent displayName, string uniqueName, bool signedRange, bool useProp0)
    {
      this.m_Module = m;
      this.m_DisplayName = displayName;
      this.m_Name = uniqueName;
      this.m_SignedRange = signedRange;
      this.m_RemapValue = 1f;
      this.m_DefaultCurveScalar = 1f;
      this.m_AllowConstant = true;
      this.m_AllowRandom = true;
      this.m_AllowCurves = true;
      this.scalar = !useProp0 ? m.GetProperty(this.m_Name, "scalar") : m.GetProperty0(this.m_Name, "scalar");
      this.maxCurve = !useProp0 ? m.GetProperty(this.m_Name, "maxCurve") : m.GetProperty0(this.m_Name, "maxCurve");
      this.maxCurveFirstKeyValue = this.maxCurve.FindPropertyRelative("m_Curve.Array.data[0].value");
      this.minCurve = !useProp0 ? m.GetProperty(this.m_Name, "minCurve") : m.GetProperty0(this.m_Name, "minCurve");
      this.minCurveFirstKeyValue = this.minCurve.FindPropertyRelative("m_Curve.Array.data[0].value");
      this.minMaxState = !useProp0 ? m.GetProperty(this.m_Name, "minMaxState") : m.GetProperty0(this.m_Name, "minMaxState");
      if ((this.state == MinMaxCurveState.k_Curve || this.state == MinMaxCurveState.k_TwoCurves) && this.m_Module.m_ParticleSystemUI.m_ParticleEffectUI.IsParticleSystemUIVisible(this.m_Module.m_ParticleSystemUI))
        m.GetParticleSystemCurveEditor().AddCurveDataIfNeeded(this.GetUniqueCurveName(), this.CreateCurveData(Color.black));
      m.AddToModuleCurves(this.maxCurve);
    }

    private float ClampValueToMaxAllowed(float val)
    {
      if ((double) Mathf.Abs(val) > (double) this.m_MaxAllowedScalar)
        return this.m_MaxAllowedScalar * Mathf.Sign(val);
      return val;
    }

    public void SetScalarAndNormalizedConstants(float newScalar, float totalMin, float totalMax)
    {
      this.scalar.floatValue = newScalar;
      this.SetNormalizedConstant(this.minCurve, totalMin);
      this.SetNormalizedConstant(this.maxCurve, totalMax);
    }

    private void SetNormalizedConstant(SerializedProperty curve, float totalValue)
    {
      float num1 = Mathf.Max(this.scalar.floatValue, 0.0001f);
      float num2 = totalValue / num1;
      this.SetCurveConstant(curve, num2);
    }

    public Vector2 GetAxisScalars()
    {
      return new Vector2(this.m_Module.GetXAxisScalar(), this.scalar.floatValue * this.m_RemapValue);
    }

    public void SetAxisScalars(Vector2 axisScalars)
    {
      float num = (double) this.m_RemapValue != 0.0 ? this.m_RemapValue : 1f;
      this.scalar.floatValue = axisScalars.y / num;
    }

    public void RemoveCurveFromEditor()
    {
      ParticleSystemCurveEditor systemCurveEditor = this.m_Module.GetParticleSystemCurveEditor();
      if (!systemCurveEditor.IsAdded(this.GetMinCurve(), this.maxCurve))
        return;
      systemCurveEditor.RemoveCurve(this.GetMinCurve(), this.maxCurve);
    }

    public bool OnCurveAreaMouseDown(int button, Rect drawRect, Rect curveRanges)
    {
      if (button == 0)
      {
        this.ToggleCurveInEditor();
        return true;
      }
      if (button != 1)
        return false;
      AnimationCurveContextMenu.Show(drawRect, this.maxCurve, this.GetMinCurve(), this.scalar, curveRanges, this.m_Module.GetParticleSystemCurveEditor());
      return true;
    }

    public ParticleSystemCurveEditor.CurveData CreateCurveData(Color color)
    {
      return new ParticleSystemCurveEditor.CurveData(this.GetUniqueCurveName(), this.m_DisplayName, this.GetMinCurve(), this.maxCurve, color, this.m_SignedRange, new CurveWrapper.GetAxisScalarsCallback(this.GetAxisScalars), new CurveWrapper.SetAxisScalarsCallback(this.SetAxisScalars), this.m_Module.foldout);
    }

    private SerializedProperty GetMinCurve()
    {
      if (this.state == MinMaxCurveState.k_TwoCurves)
        return this.minCurve;
      return (SerializedProperty) null;
    }

    public void ToggleCurveInEditor()
    {
      ParticleSystemCurveEditor systemCurveEditor = this.m_Module.GetParticleSystemCurveEditor();
      if (systemCurveEditor.IsAdded(this.GetMinCurve(), this.maxCurve))
        systemCurveEditor.RemoveCurve(this.GetMinCurve(), this.maxCurve);
      else
        systemCurveEditor.AddCurve(this.CreateCurveData(systemCurveEditor.GetAvailableColor()));
    }

    private void SetMinMaxState(MinMaxCurveState newState)
    {
      if (newState == this.state)
        return;
      MinMaxCurveState state = this.state;
      ParticleSystemCurveEditor systemCurveEditor = this.m_Module.GetParticleSystemCurveEditor();
      if (systemCurveEditor.IsAdded(this.GetMinCurve(), this.maxCurve))
        systemCurveEditor.RemoveCurve(this.GetMinCurve(), this.maxCurve);
      switch (newState)
      {
        case MinMaxCurveState.k_Scalar:
          this.InitSingleScalar(state);
          break;
        case MinMaxCurveState.k_Curve:
          this.InitSingleCurve(state);
          break;
        case MinMaxCurveState.k_TwoCurves:
          this.InitDoubleCurves(state);
          break;
        case MinMaxCurveState.k_TwoScalars:
          this.InitDoubleScalars(state);
          break;
      }
      this.minMaxState.intValue = (int) newState;
      switch (newState)
      {
        case MinMaxCurveState.k_Scalar:
        case MinMaxCurveState.k_TwoScalars:
          AnimationCurvePreviewCache.ClearCache();
          break;
        case MinMaxCurveState.k_Curve:
        case MinMaxCurveState.k_TwoCurves:
          systemCurveEditor.AddCurve(this.CreateCurveData(systemCurveEditor.GetAvailableColor()));
          goto case MinMaxCurveState.k_Scalar;
        default:
          Debug.LogError((object) "Unhandled enum value");
          goto case MinMaxCurveState.k_Scalar;
      }
    }

    private void InitSingleScalar(MinMaxCurveState oldState)
    {
      switch (oldState)
      {
        case MinMaxCurveState.k_Curve:
        case MinMaxCurveState.k_TwoCurves:
        case MinMaxCurveState.k_TwoScalars:
          this.scalar.floatValue *= this.GetMaxKeyValue(this.maxCurve.animationCurveValue.keys);
          break;
      }
      this.SetCurveConstant(this.maxCurve, 1f);
    }

    private void InitDoubleScalars(MinMaxCurveState oldState)
    {
      this.minConstant = this.GetAverageKeyValue(this.minCurve.animationCurveValue.keys) * this.scalar.floatValue;
      switch (oldState)
      {
        case MinMaxCurveState.k_Scalar:
          this.maxConstant = this.scalar.floatValue;
          break;
        case MinMaxCurveState.k_Curve:
        case MinMaxCurveState.k_TwoCurves:
          this.maxConstant = this.GetAverageKeyValue(this.maxCurve.animationCurveValue.keys) * this.scalar.floatValue;
          break;
        default:
          Debug.LogError((object) "Enum not handled!");
          break;
      }
      this.SetCurveRequirements();
    }

    private void InitSingleCurve(MinMaxCurveState oldState)
    {
      switch (oldState)
      {
        case MinMaxCurveState.k_Scalar:
          this.SetCurveConstant(this.maxCurve, this.GetNormalizedValueFromScalar());
          break;
      }
      this.SetCurveRequirements();
    }

    private void InitDoubleCurves(MinMaxCurveState oldState)
    {
      switch (oldState)
      {
        case MinMaxCurveState.k_Scalar:
          this.SetCurveConstant(this.maxCurve, this.GetNormalizedValueFromScalar());
          break;
      }
      this.SetCurveRequirements();
    }

    private float GetNormalizedValueFromScalar()
    {
      if ((double) this.scalar.floatValue < 0.0)
        return -1f;
      return (double) this.scalar.floatValue > 0.0 ? 1f : 0.0f;
    }

    private void SetCurveRequirements()
    {
      this.scalar.floatValue = Mathf.Abs(this.scalar.floatValue);
      if ((double) this.scalar.floatValue != 0.0)
        return;
      this.scalar.floatValue = this.m_DefaultCurveScalar;
    }

    private void SetCurveConstant(SerializedProperty curve, float value)
    {
      Keyframe[] keyframeArray = new Keyframe[1]{ new Keyframe(0.0f, value) };
      curve.animationCurveValue = new AnimationCurve(keyframeArray);
    }

    private float GetAverageKeyValue(Keyframe[] keyFrames)
    {
      float num = 0.0f;
      foreach (Keyframe keyFrame in keyFrames)
        num += keyFrame.value;
      return num / (float) keyFrames.Length;
    }

    private float GetMaxKeyValue(Keyframe[] keyFrames)
    {
      float num = float.NegativeInfinity;
      float f = float.PositiveInfinity;
      foreach (Keyframe keyFrame in keyFrames)
      {
        if ((double) keyFrame.value > (double) num)
          num = keyFrame.value;
        if ((double) keyFrame.value < (double) f)
          f = keyFrame.value;
      }
      if ((double) Mathf.Abs(f) > (double) num)
        return f;
      return num;
    }

    private bool IsCurveConstant(Keyframe[] keyFrames, out float constantValue)
    {
      if (keyFrames.Length == 0)
      {
        constantValue = 0.0f;
        return false;
      }
      constantValue = keyFrames[0].value;
      for (int index = 1; index < keyFrames.Length; ++index)
      {
        if ((double) Mathf.Abs(constantValue - keyFrames[index].value) > 9.99999974737875E-06)
          return false;
      }
      return true;
    }

    public string GetUniqueCurveName()
    {
      return SerializedModule.Concat(this.m_Module.GetUniqueModuleName(), this.m_Name);
    }

    public bool SupportsProcedural()
    {
      bool flag = AnimationUtility.CurveSupportsProcedural(this.maxCurve.animationCurveValue);
      if (this.state != MinMaxCurveState.k_TwoCurves && this.state != MinMaxCurveState.k_TwoScalars)
        return flag;
      if (flag)
        return AnimationUtility.CurveSupportsProcedural(this.minCurve.animationCurveValue);
      return false;
    }
  }
}
