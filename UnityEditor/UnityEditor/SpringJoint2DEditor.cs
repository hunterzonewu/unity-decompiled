// Decompiled with JetBrains decompiler
// Type: UnityEditor.SpringJoint2DEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (SpringJoint2D))]
  [CanEditMultipleObjects]
  internal class SpringJoint2DEditor : AnchoredJoint2DEditor
  {
    public new void OnSceneGUI()
    {
      SpringJoint2D target = (SpringJoint2D) this.target;
      if (!target.enabled)
        return;
      Vector3 anchor = Joint2DEditor.TransformPoint(target.transform, (Vector3) target.anchor);
      Vector3 vector3 = (Vector3) target.connectedAnchor;
      if ((bool) ((Object) target.connectedBody))
        vector3 = Joint2DEditor.TransformPoint(target.connectedBody.transform, vector3);
      Joint2DEditor.DrawDistanceGizmo(anchor, vector3, target.distance);
      base.OnSceneGUI();
    }
  }
}
