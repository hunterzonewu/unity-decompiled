// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkStartPosition
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>This component is used to make a gameObject a starting position for spawning player objects in multiplayer games.</para>
  /// </summary>
  [DisallowMultipleComponent]
  [AddComponentMenu("Network/NetworkStartPosition")]
  public class NetworkStartPosition : MonoBehaviour
  {
    public void Awake()
    {
      NetworkManager.RegisterStartPosition(this.transform);
    }

    public void OnDestroy()
    {
      NetworkManager.UnRegisterStartPosition(this.transform);
    }
  }
}
