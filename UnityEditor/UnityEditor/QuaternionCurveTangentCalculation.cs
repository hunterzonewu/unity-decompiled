// Decompiled with JetBrains decompiler
// Type: UnityEditor.QuaternionCurveTangentCalculation
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal struct QuaternionCurveTangentCalculation
  {
    private AnimationCurve eulerX;
    private AnimationCurve eulerY;
    private AnimationCurve eulerZ;

    public AnimationCurve GetCurve(int index)
    {
      if (index == 0)
        return this.eulerX;
      if (index == 1)
        return this.eulerY;
      return this.eulerZ;
    }

    public void SetCurve(int index, AnimationCurve curve)
    {
      if (index == 0)
        this.eulerX = curve;
      else if (index == 1)
        this.eulerY = curve;
      else
        this.eulerZ = curve;
    }

    private Vector3 EvaluateEulerCurvesDirectly(float time)
    {
      return new Vector3(this.eulerX.Evaluate(time), this.eulerY.Evaluate(time), this.eulerZ.Evaluate(time));
    }

    public float CalculateLinearTangent(int fromIndex, int toIndex, int componentIndex)
    {
      AnimationCurve curve = this.GetCurve(componentIndex);
      return this.CalculateLinearTangent(curve[fromIndex], curve[toIndex], componentIndex);
    }

    public float CalculateLinearTangent(Keyframe from, Keyframe to, int component)
    {
      float t = 0.01f;
      Vector3 eulerCurvesDirectly1 = this.EvaluateEulerCurvesDirectly(to.time);
      Vector3 eulerCurvesDirectly2 = this.EvaluateEulerCurvesDirectly(from.time);
      Vector3 eulerFromQuaternion = QuaternionCurveTangentCalculation.GetEulerFromQuaternion(Quaternion.Slerp(Quaternion.Euler(eulerCurvesDirectly1), Quaternion.Euler(eulerCurvesDirectly2), t), eulerCurvesDirectly1);
      switch (component)
      {
        case 0:
          return (float) (((double) eulerFromQuaternion.x - (double) eulerCurvesDirectly1.x) / (double) t / -((double) to.time - (double) from.time));
        case 1:
          return (float) (((double) eulerFromQuaternion.y - (double) eulerCurvesDirectly1.y) / (double) t / -((double) to.time - (double) from.time));
        case 2:
          return (float) (((double) eulerFromQuaternion.z - (double) eulerCurvesDirectly1.z) / (double) t / -((double) to.time - (double) from.time));
        default:
          return 0.0f;
      }
    }

    public float CalculateSmoothTangent(int index, int component)
    {
      AnimationCurve curve = this.GetCurve(component);
      if (curve.length < 2)
        return 0.0f;
      if (index <= 0)
        return this.CalculateLinearTangent(curve[0], curve[1], component);
      if (index >= curve.length - 1)
        return this.CalculateLinearTangent(curve[curve.length - 1], curve[curve.length - 2], component);
      float time1 = curve[index - 1].time;
      float time2 = curve[index].time;
      float time3 = curve[index + 1].time;
      Vector3 eulerCurvesDirectly1 = this.EvaluateEulerCurvesDirectly(time1);
      Vector3 eulerCurvesDirectly2 = this.EvaluateEulerCurvesDirectly(time2);
      Vector3 eulerCurvesDirectly3 = this.EvaluateEulerCurvesDirectly(time3);
      Quaternion quaternion1 = Quaternion.Euler(eulerCurvesDirectly1);
      Quaternion quaternion2 = Quaternion.Euler(eulerCurvesDirectly2);
      Quaternion quaternion3 = Quaternion.Euler(eulerCurvesDirectly3);
      if ((double) quaternion1.x * (double) quaternion2.x + (double) quaternion1.y * (double) quaternion2.y + (double) quaternion1.z * (double) quaternion2.z + (double) quaternion1.w * (double) quaternion2.w < 0.0)
        quaternion1 = new Quaternion(-quaternion1.x, -quaternion1.y, -quaternion1.z, -quaternion1.w);
      if ((double) quaternion3.x * (double) quaternion2.x + (double) quaternion3.y * (double) quaternion2.y + (double) quaternion3.z * (double) quaternion2.z + (double) quaternion3.w * (double) quaternion2.w < 0.0)
        quaternion3 = new Quaternion(-quaternion3.x, -quaternion3.y, -quaternion3.z, -quaternion3.w);
      Quaternion quaternion4 = new Quaternion();
      float dx1 = time2 - time1;
      float dx2 = time3 - time2;
      for (int index1 = 0; index1 < 4; ++index1)
      {
        float dy1 = quaternion2[index1] - quaternion1[index1];
        float dy2 = quaternion3[index1] - quaternion2[index1];
        float num1 = QuaternionCurveTangentCalculation.SafeDeltaDivide(dy1, dx1);
        float num2 = QuaternionCurveTangentCalculation.SafeDeltaDivide(dy2, dx2);
        quaternion4[index1] = (float) (0.5 * (double) num1 + 0.5 * (double) num2);
      }
      float num = Mathf.Abs(time3 - time1) * 0.01f;
      Quaternion q1 = new Quaternion(quaternion2.x - quaternion4.x * num, quaternion2.y - quaternion4.y * num, quaternion2.z - quaternion4.z * num, quaternion2.w - quaternion4.w * num);
      Quaternion q2 = new Quaternion(quaternion2.x + quaternion4.x * num, quaternion2.y + quaternion4.y * num, quaternion2.z + quaternion4.z * num, quaternion2.w + quaternion4.w * num);
      Vector3 eulerFromQuaternion = QuaternionCurveTangentCalculation.GetEulerFromQuaternion(q1, eulerCurvesDirectly2);
      return ((QuaternionCurveTangentCalculation.GetEulerFromQuaternion(q2, eulerCurvesDirectly2) - eulerFromQuaternion) / (num * 2f))[component];
    }

    public static Vector3[] GetEquivalentEulerAngles(Quaternion quat)
    {
      Vector3 eulerAngles = quat.eulerAngles;
      return new Vector3[2]
      {
        eulerAngles,
        new Vector3(180f - eulerAngles.x, eulerAngles.y + 180f, eulerAngles.z + 180f)
      };
    }

    public static Vector3 GetEulerFromQuaternion(Quaternion q, Vector3 refEuler)
    {
      Vector3[] equivalentEulerAngles = QuaternionCurveTangentCalculation.GetEquivalentEulerAngles(q);
      for (int index = 0; index < equivalentEulerAngles.Length; ++index)
      {
        equivalentEulerAngles[index] = new Vector3((float) ((double) Mathf.Repeat((float) ((double) equivalentEulerAngles[index].x - (double) refEuler.x + 180.0), 360f) + (double) refEuler.x - 180.0), (float) ((double) Mathf.Repeat((float) ((double) equivalentEulerAngles[index].y - (double) refEuler.y + 180.0), 360f) + (double) refEuler.y - 180.0), (float) ((double) Mathf.Repeat((float) ((double) equivalentEulerAngles[index].z - (double) refEuler.z + 180.0), 360f) + (double) refEuler.z - 180.0));
        float num1 = Mathf.Repeat(equivalentEulerAngles[index].x, 360f);
        if ((double) Mathf.Abs(num1 - 90f) < 1.0)
        {
          float num2 = equivalentEulerAngles[index].z - equivalentEulerAngles[index].y - (refEuler.z - refEuler.y);
          equivalentEulerAngles[index].z = refEuler.z + num2 * 0.5f;
          equivalentEulerAngles[index].y = refEuler.y - num2 * 0.5f;
        }
        if ((double) Mathf.Abs(num1 - 270f) < 1.0)
        {
          float num2 = equivalentEulerAngles[index].z + equivalentEulerAngles[index].y - (refEuler.z + refEuler.y);
          equivalentEulerAngles[index].z = refEuler.z + num2 * 0.5f;
          equivalentEulerAngles[index].y = refEuler.y + num2 * 0.5f;
        }
      }
      Vector3 vector3 = equivalentEulerAngles[0];
      float num = (equivalentEulerAngles[0] - refEuler).sqrMagnitude;
      for (int index = 1; index < equivalentEulerAngles.Length; ++index)
      {
        float sqrMagnitude = (equivalentEulerAngles[index] - refEuler).sqrMagnitude;
        if ((double) sqrMagnitude < (double) num)
        {
          num = sqrMagnitude;
          vector3 = equivalentEulerAngles[index];
        }
      }
      return vector3;
    }

    public static float SafeDeltaDivide(float dy, float dx)
    {
      if ((double) dx == 0.0)
        return 0.0f;
      return dy / dx;
    }

    public void UpdateTangentsFromMode(int componentIndex)
    {
      AnimationCurve curve = this.GetCurve(componentIndex);
      for (int index = 0; index < curve.length; ++index)
        this.UpdateTangentsFromMode(index, componentIndex);
    }

    public void UpdateTangentsFromMode(int index, int componentIndex)
    {
      AnimationCurve curve = this.GetCurve(componentIndex);
      if (index < 0 || index >= curve.length)
        return;
      Keyframe key = curve[index];
      if (CurveUtility.GetKeyTangentMode(key, 0) == TangentMode.Linear && index >= 1)
      {
        key.inTangent = this.CalculateLinearTangent(index, index - 1, componentIndex);
        curve.MoveKey(index, key);
      }
      if (CurveUtility.GetKeyTangentMode(key, 1) == TangentMode.Linear && index + 1 < curve.length)
      {
        key.outTangent = this.CalculateLinearTangent(index, index + 1, componentIndex);
        curve.MoveKey(index, key);
      }
      if (CurveUtility.GetKeyTangentMode(key, 0) != TangentMode.Smooth && CurveUtility.GetKeyTangentMode(key, 1) != TangentMode.Smooth)
        return;
      // ISSUE: explicit reference operation
      // ISSUE: variable of a reference type
      Keyframe& local = @key;
      float smoothTangent = this.CalculateSmoothTangent(index, componentIndex);
      key.outTangent = smoothTangent;
      double num = (double) smoothTangent;
      // ISSUE: explicit reference operation
      (^local).inTangent = (float) num;
      curve.MoveKey(index, key);
    }

    public static void UpdateTangentsFromMode(AnimationCurve curve, AnimationClip clip, EditorCurveBinding curveBinding)
    {
      CurveUtility.UpdateTangentsFromMode(curve);
    }
  }
}
