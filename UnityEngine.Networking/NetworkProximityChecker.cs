// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkProximityChecker
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System.Collections.Generic;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>Component that controls visibility of networked objects for players.</para>
  /// </summary>
  [RequireComponent(typeof (NetworkIdentity))]
  [AddComponentMenu("Network/NetworkProximityChecker")]
  public class NetworkProximityChecker : NetworkBehaviour
  {
    /// <summary>
    ///   <para>The maximim range that objects will be visible at.</para>
    /// </summary>
    public int visRange = 10;
    /// <summary>
    ///   <para>How often (in seconds) that this object should update the set of players that can see it.</para>
    /// </summary>
    public float visUpdateInterval = 1f;
    /// <summary>
    ///   <para>Which method to use for checking proximity of players.</para>
    /// </summary>
    public NetworkProximityChecker.CheckMethod checkMethod;
    /// <summary>
    ///   <para>Flag to force this object to be hidden for players.</para>
    /// </summary>
    public bool forceHidden;
    private float m_VisUpdateTime;

    private void Update()
    {
      if (!NetworkServer.active || (double) Time.time - (double) this.m_VisUpdateTime <= (double) this.visUpdateInterval)
        return;
      this.GetComponent<NetworkIdentity>().RebuildObservers(false);
      this.m_VisUpdateTime = Time.time;
    }

    public override bool OnCheckObserver(NetworkConnection newObserver)
    {
      if (this.forceHidden)
        return false;
      GameObject gameObject = (GameObject) null;
      using (List<PlayerController>.Enumerator enumerator = newObserver.playerControllers.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          PlayerController current = enumerator.Current;
          if (current != null && (Object) current.gameObject != (Object) null)
          {
            gameObject = current.gameObject;
            break;
          }
        }
      }
      if ((Object) gameObject == (Object) null)
        return false;
      return (double) (gameObject.transform.position - this.transform.position).magnitude < (double) this.visRange;
    }

    public override bool OnRebuildObservers(HashSet<NetworkConnection> observers, bool initial)
    {
      if (this.forceHidden)
      {
        NetworkIdentity component = this.GetComponent<NetworkIdentity>();
        if (component.connectionToClient != null)
          observers.Add(component.connectionToClient);
        return true;
      }
      switch (this.checkMethod)
      {
        case NetworkProximityChecker.CheckMethod.Physics3D:
          foreach (Component component1 in Physics.OverlapSphere(this.transform.position, (float) this.visRange))
          {
            NetworkIdentity component2 = component1.GetComponent<NetworkIdentity>();
            if ((Object) component2 != (Object) null && component2.connectionToClient != null)
              observers.Add(component2.connectionToClient);
          }
          return true;
        case NetworkProximityChecker.CheckMethod.Physics2D:
          foreach (Component component1 in Physics2D.OverlapCircleAll((Vector2) this.transform.position, (float) this.visRange))
          {
            NetworkIdentity component2 = component1.GetComponent<NetworkIdentity>();
            if ((Object) component2 != (Object) null && component2.connectionToClient != null)
              observers.Add(component2.connectionToClient);
          }
          return true;
        default:
          return false;
      }
    }

    public override void OnSetLocalVisibility(bool vis)
    {
      NetworkProximityChecker.SetVis(this.gameObject, vis);
    }

    private static void SetVis(GameObject go, bool vis)
    {
      foreach (Renderer component in go.GetComponents<Renderer>())
        component.enabled = vis;
      for (int index = 0; index < go.transform.childCount; ++index)
        NetworkProximityChecker.SetVis(go.transform.GetChild(index).gameObject, vis);
    }

    /// <summary>
    ///   <para>Enumeration of methods to use to check proximity.</para>
    /// </summary>
    public enum CheckMethod
    {
      Physics3D,
      Physics2D,
    }
  }
}
