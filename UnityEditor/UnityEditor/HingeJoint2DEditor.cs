// Decompiled with JetBrains decompiler
// Type: UnityEditor.HingeJoint2DEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (HingeJoint2D))]
  [CanEditMultipleObjects]
  internal class HingeJoint2DEditor : AnchoredJoint2DEditor
  {
    public new void OnSceneGUI()
    {
      HingeJoint2D target = (HingeJoint2D) this.target;
      if (!target.enabled)
        return;
      if (target.useLimits)
      {
        Vector3 vector3_1 = Joint2DEditor.TransformPoint(target.transform, (Vector3) target.anchor);
        float num1 = Mathf.Min(target.limits.min, target.limits.max);
        float num2 = Mathf.Max(target.limits.min, target.limits.max);
        float angle = num2 - num1;
        float radius = HandleUtility.GetHandleSize(vector3_1) * 0.8f;
        float rotation = target.GetComponent<Rigidbody2D>().rotation;
        Vector3 vector3_2 = (Vector3) Joint2DEditor.RotateVector2((Vector2) Vector3.right, -num2 - rotation);
        Vector3 end = vector3_1 + (Vector3) (Joint2DEditor.RotateVector2((Vector2) Vector3.right, -target.jointAngle - rotation) * radius);
        Handles.color = new Color(0.0f, 1f, 0.0f, 0.7f);
        Joint2DEditor.DrawAALine(vector3_1, end);
        Handles.color = new Color(0.0f, 1f, 0.0f, 0.03f);
        Handles.DrawSolidArc(vector3_1, Vector3.back, vector3_2, angle, radius);
        Handles.color = new Color(0.0f, 1f, 0.0f, 0.7f);
        Handles.DrawWireArc(vector3_1, Vector3.back, vector3_2, angle, radius);
        this.DrawTick(vector3_1, radius, 0.0f, vector3_2, 1f);
        this.DrawTick(vector3_1, radius, angle, vector3_2, 1f);
      }
      base.OnSceneGUI();
    }

    private void DrawTick(Vector3 center, float radius, float angle, Vector3 up, float length)
    {
      Vector3 normalized = (Vector3) Joint2DEditor.RotateVector2((Vector2) up, angle).normalized;
      Vector3 vector3_1 = center + normalized * radius;
      Vector3 vector3_2 = vector3_1 + (center - vector3_1).normalized * radius * length;
      Handles.DrawAAPolyLine(new Color[2]
      {
        new Color(0.0f, 1f, 0.0f, 0.7f),
        new Color(0.0f, 1f, 0.0f, 0.0f)
      }, new Vector3[2]
      {
        vector3_1,
        vector3_2
      });
    }
  }
}
