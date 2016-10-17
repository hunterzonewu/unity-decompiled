// Decompiled with JetBrains decompiler
// Type: UnityEditor.WheelJoint2DEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (WheelJoint2D))]
  internal class WheelJoint2DEditor : AnchoredJoint2DEditor
  {
    public new void OnSceneGUI()
    {
      WheelJoint2D target = (WheelJoint2D) this.target;
      if (!target.enabled)
        return;
      Vector3 position = Joint2DEditor.TransformPoint(target.transform, (Vector3) target.anchor);
      Vector3 vector3_1 = position;
      Vector3 vector3_2 = position;
      Vector3 vector3_3 = (Vector3) Joint2DEditor.RotateVector2((Vector2) Vector3.right, -target.suspension.angle - target.transform.eulerAngles.z);
      Handles.color = Color.green;
      Vector3 vector3_4 = vector3_3 * (HandleUtility.GetHandleSize(position) * 0.3f);
      Joint2DEditor.DrawAALine(vector3_1 + vector3_4, vector3_2 - vector3_4);
      base.OnSceneGUI();
    }
  }
}
