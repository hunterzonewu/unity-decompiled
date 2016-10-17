// Decompiled with JetBrains decompiler
// Type: UnityEditor.AnchoredJoint2DEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (AnchoredJoint2D), true)]
  [CanEditMultipleObjects]
  internal class AnchoredJoint2DEditor : Joint2DEditor
  {
    private const float k_SnapDistance = 0.13f;
    private AnchoredJoint2D anchorJoint2D;

    public void OnSceneGUI()
    {
      this.anchorJoint2D = (AnchoredJoint2D) this.target;
      if (!this.anchorJoint2D.enabled)
        return;
      Vector3 position1 = Joint2DEditor.TransformPoint(this.anchorJoint2D.transform, (Vector3) this.anchorJoint2D.anchor);
      Vector3 position2 = (Vector3) this.anchorJoint2D.connectedAnchor;
      if ((bool) ((Object) this.anchorJoint2D.connectedBody))
        position2 = Joint2DEditor.TransformPoint(this.anchorJoint2D.connectedBody.transform, position2);
      Vector3 vector3 = position1 + (position2 - position1).normalized * HandleUtility.GetHandleSize(position1) * 0.1f;
      Handles.color = Color.green;
      Handles.DrawAAPolyLine(new Vector3[2]
      {
        vector3,
        position2
      });
      if (this.HandleAnchor(ref position2, true))
      {
        position2 = this.SnapToSprites(position2);
        position2 = Joint2DEditor.SnapToPoint(position2, position1, 0.13f);
        if ((bool) ((Object) this.anchorJoint2D.connectedBody))
          position2 = Joint2DEditor.InverseTransformPoint(this.anchorJoint2D.connectedBody.transform, position2);
        Undo.RecordObject((Object) this.anchorJoint2D, "Move Connected Anchor");
        this.anchorJoint2D.connectedAnchor = (Vector2) position2;
      }
      if (!this.HandleAnchor(ref position1, false))
        return;
      Vector3 point = Joint2DEditor.SnapToPoint(this.SnapToSprites(position1), position2, 0.13f);
      Undo.RecordObject((Object) this.anchorJoint2D, "Move Anchor");
      this.anchorJoint2D.anchor = (Vector2) Joint2DEditor.InverseTransformPoint(this.anchorJoint2D.transform, point);
    }

    private Vector3 SnapToSprites(Vector3 position)
    {
      position = Joint2DEditor.SnapToSprite(this.anchorJoint2D.GetComponent<SpriteRenderer>(), position, 0.13f);
      if ((bool) ((Object) this.anchorJoint2D.connectedBody))
        position = Joint2DEditor.SnapToSprite(this.anchorJoint2D.connectedBody.GetComponent<SpriteRenderer>(), position, 0.13f);
      return position;
    }
  }
}
