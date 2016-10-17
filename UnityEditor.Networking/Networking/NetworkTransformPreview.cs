// Decompiled with JetBrains decompiler
// Type: UnityEditor.Networking.NetworkTransformPreview
// Assembly: UnityEditor.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6916FF53-78D9-4C64-A56C-55C7AE140DAE
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.Networking.dll

using UnityEngine;
using UnityEngine.Networking;

namespace UnityEditor.Networking
{
  [CustomPreview(typeof (GameObject))]
  internal class NetworkTransformPreview : ObjectPreview
  {
    private NetworkTransform m_Transform;
    private Rigidbody m_Rigidbody3D;
    private Rigidbody2D m_Rigidbody2D;
    private GUIContent m_Title;

    public override void Initialize(Object[] targets)
    {
      base.Initialize(targets);
      this.GetNetworkInformation(this.target as GameObject);
    }

    public override GUIContent GetPreviewTitle()
    {
      if (this.m_Title == null)
        this.m_Title = new GUIContent("Network Transform");
      return this.m_Title;
    }

    public override bool HasPreviewGUI()
    {
      return (Object) this.m_Transform != (Object) null;
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
      if (Event.current.type != EventType.Repaint || (Object) this.m_Transform == (Object) null)
        return;
      int num1 = 4;
      float magnitude = (this.m_Transform.transform.position - this.m_Transform.targetSyncPosition).magnitude;
      GUI.Label(new Rect(r.xMin + 4f, r.y + (float) num1, 600f, 20f), "Position: " + (object) this.m_Transform.transform.position + " Target: " + (object) this.m_Transform.targetSyncPosition + " Diff: " + (object) magnitude);
      int num2 = num1 + 20;
      if ((Object) this.m_Rigidbody3D != (Object) null)
      {
        float num3 = Quaternion.Angle(this.m_Transform.rigidbody3D.rotation, this.m_Transform.targetSyncRotation3D);
        GUI.Label(new Rect(r.xMin + 4f, r.y + (float) num2, 600f, 20f), "Angle: " + (object) this.m_Transform.rigidbody3D.rotation + " Target: " + (object) this.m_Transform.targetSyncRotation3D + " Diff: " + (object) num3);
        int num4 = num2 + 20;
        GUI.Label(new Rect(r.xMin + 4f, r.y + (float) num4, 600f, 20f), "Velocity: " + (object) this.m_Transform.rigidbody3D.velocity + " Target: " + (object) this.m_Transform.targetSyncVelocity);
        num2 = num4 + 20;
      }
      if ((Object) this.m_Rigidbody2D != (Object) null)
      {
        float num3 = this.m_Transform.rigidbody2D.rotation - this.m_Transform.targetSyncRotation2D;
        GUI.Label(new Rect(r.xMin + 4f, r.y + (float) num2, 600f, 20f), "Angle: " + (object) this.m_Transform.rigidbody2D.rotation + " Target: " + (object) this.m_Transform.targetSyncRotation2D + " Diff: " + (object) num3);
        int num4 = num2 + 20;
        GUI.Label(new Rect(r.xMin + 4f, r.y + (float) num4, 600f, 20f), "Velocity: " + (object) this.m_Transform.rigidbody2D.velocity + " Target: " + (object) this.m_Transform.targetSyncVelocity);
        num2 = num4 + 20;
      }
      GUI.Label(new Rect(r.xMin + 4f, r.y + (float) num2, 200f, 20f), "Last SyncTime: " + (object) (float) ((double) Time.time - (double) this.m_Transform.lastSyncTime));
      int num5 = num2 + 20;
    }

    private void GetNetworkInformation(GameObject gameObject)
    {
      this.m_Transform = gameObject.GetComponent<NetworkTransform>();
      this.m_Rigidbody3D = gameObject.GetComponent<Rigidbody>();
      this.m_Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    }
  }
}
