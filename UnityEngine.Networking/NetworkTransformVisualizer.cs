// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkTransformVisualizer
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System.ComponentModel;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>This is a helper component to help understand and debug networked movement synchronization with the NetworkTransform component.</para>
  /// </summary>
  [RequireComponent(typeof (NetworkTransform))]
  [DisallowMultipleComponent]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [AddComponentMenu("Network/NetworkTransformVisualizer")]
  public class NetworkTransformVisualizer : NetworkBehaviour
  {
    [SerializeField]
    private GameObject m_VisualizerPrefab;
    private NetworkTransform m_NetworkTransform;
    private GameObject m_Visualizer;
    private static Material s_LineMaterial;

    /// <summary>
    ///   <para>The prefab to use for the visualization object.</para>
    /// </summary>
    public GameObject visualizerPrefab
    {
      get
      {
        return this.m_VisualizerPrefab;
      }
      set
      {
        this.m_VisualizerPrefab = value;
      }
    }

    public override void OnStartClient()
    {
      if (!((Object) this.m_VisualizerPrefab != (Object) null))
        return;
      this.m_NetworkTransform = this.GetComponent<NetworkTransform>();
      NetworkTransformVisualizer.CreateLineMaterial();
      this.m_Visualizer = (GameObject) Object.Instantiate((Object) this.m_VisualizerPrefab, this.transform.position, Quaternion.identity);
    }

    public override void OnStartLocalPlayer()
    {
      if ((Object) this.m_Visualizer == (Object) null || !this.m_NetworkTransform.localPlayerAuthority && !this.isServer)
        return;
      Object.Destroy((Object) this.m_Visualizer);
    }

    private void OnDestroy()
    {
      if (!((Object) this.m_Visualizer != (Object) null))
        return;
      Object.Destroy((Object) this.m_Visualizer);
    }

    [ClientCallback]
    private void FixedUpdate()
    {
      if ((Object) this.m_Visualizer == (Object) null || !NetworkServer.active && !NetworkClient.active || (!this.isServer && !this.isClient || this.hasAuthority && this.m_NetworkTransform.localPlayerAuthority))
        return;
      this.m_Visualizer.transform.position = this.m_NetworkTransform.targetSyncPosition;
      if ((Object) this.m_NetworkTransform.rigidbody3D != (Object) null && (Object) this.m_Visualizer.GetComponent<Rigidbody>() != (Object) null)
        this.m_Visualizer.GetComponent<Rigidbody>().velocity = this.m_NetworkTransform.targetSyncVelocity;
      if ((Object) this.m_NetworkTransform.rigidbody2D != (Object) null && (Object) this.m_Visualizer.GetComponent<Rigidbody2D>() != (Object) null)
        this.m_Visualizer.GetComponent<Rigidbody2D>().velocity = (Vector2) this.m_NetworkTransform.targetSyncVelocity;
      Quaternion quaternion = Quaternion.identity;
      if ((Object) this.m_NetworkTransform.rigidbody3D != (Object) null)
        quaternion = this.m_NetworkTransform.targetSyncRotation3D;
      if ((Object) this.m_NetworkTransform.rigidbody2D != (Object) null)
        quaternion = Quaternion.Euler(0.0f, 0.0f, this.m_NetworkTransform.targetSyncRotation2D);
      this.m_Visualizer.transform.rotation = quaternion;
    }

    private void OnRenderObject()
    {
      if ((Object) this.m_Visualizer == (Object) null || this.m_NetworkTransform.localPlayerAuthority && this.hasAuthority || (double) this.m_NetworkTransform.lastSyncTime == 0.0)
        return;
      NetworkTransformVisualizer.s_LineMaterial.SetPass(0);
      GL.Begin(1);
      GL.Color(Color.white);
      GL.Vertex3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
      GL.Vertex3(this.m_NetworkTransform.targetSyncPosition.x, this.m_NetworkTransform.targetSyncPosition.y, this.m_NetworkTransform.targetSyncPosition.z);
      GL.End();
      this.DrawRotationInterpolation();
    }

    private void DrawRotationInterpolation()
    {
      Quaternion quaternion = Quaternion.identity;
      if ((Object) this.m_NetworkTransform.rigidbody3D != (Object) null)
        quaternion = this.m_NetworkTransform.targetSyncRotation3D;
      if ((Object) this.m_NetworkTransform.rigidbody2D != (Object) null)
        quaternion = Quaternion.Euler(0.0f, 0.0f, this.m_NetworkTransform.targetSyncRotation2D);
      if (quaternion == Quaternion.identity)
        return;
      GL.Begin(1);
      GL.Color(Color.yellow);
      GL.Vertex3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
      Vector3 vector3_1 = this.transform.position + this.transform.right;
      GL.Vertex3(vector3_1.x, vector3_1.y, vector3_1.z);
      GL.End();
      GL.Begin(1);
      GL.Color(Color.green);
      GL.Vertex3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
      Vector3 vector3_2 = this.transform.position + quaternion * Vector3.right;
      GL.Vertex3(vector3_2.x, vector3_2.y, vector3_2.z);
      GL.End();
    }

    private static void CreateLineMaterial()
    {
      if ((bool) ((Object) NetworkTransformVisualizer.s_LineMaterial))
        return;
      Shader shader = Shader.Find("Hidden/Internal-Colored");
      if (!(bool) ((Object) shader))
      {
        Debug.LogWarning((object) "Could not find Colored builtin shader");
      }
      else
      {
        NetworkTransformVisualizer.s_LineMaterial = new Material(shader);
        NetworkTransformVisualizer.s_LineMaterial.hideFlags = HideFlags.HideAndDontSave;
        NetworkTransformVisualizer.s_LineMaterial.SetInt("_ZWrite", 0);
      }
    }
  }
}
