// Decompiled with JetBrains decompiler
// Type: UnityEditor.SliderJoint2DEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (SliderJoint2D))]
  [CanEditMultipleObjects]
  internal class SliderJoint2DEditor : AnchoredJoint2DEditor
  {
    public new void OnSceneGUI()
    {
      SliderJoint2D target = (SliderJoint2D) this.target;
      if (!target.enabled)
        return;
      Vector3 position = Joint2DEditor.TransformPoint(target.transform, (Vector3) target.anchor);
      Vector3 vector3_1 = position;
      Vector3 vector3_2 = position;
      Vector3 lhs = (Vector3) Joint2DEditor.RotateVector2((Vector2) Vector3.right, -target.angle - target.transform.eulerAngles.z);
      Handles.color = Color.green;
      Vector3 vector3_3;
      Vector3 vector3_4;
      if (target.useLimits)
      {
        vector3_3 = position + lhs * target.limits.max;
        vector3_4 = position + lhs * target.limits.min;
        Vector3 vector3_5 = Vector3.Cross(lhs, Vector3.forward);
        float num1 = HandleUtility.GetHandleSize(vector3_3) * 0.16f;
        float num2 = HandleUtility.GetHandleSize(vector3_4) * 0.16f;
        Joint2DEditor.DrawAALine(vector3_3 + vector3_5 * num1, vector3_3 - vector3_5 * num1);
        Joint2DEditor.DrawAALine(vector3_4 + vector3_5 * num2, vector3_4 - vector3_5 * num2);
      }
      else
      {
        Vector3 vector3_5 = lhs * (HandleUtility.GetHandleSize(position) * 0.3f);
        vector3_3 = vector3_1 + vector3_5;
        vector3_4 = vector3_2 - vector3_5;
      }
      Joint2DEditor.DrawAALine(vector3_3, vector3_4);
      base.OnSceneGUI();
    }
  }
}
