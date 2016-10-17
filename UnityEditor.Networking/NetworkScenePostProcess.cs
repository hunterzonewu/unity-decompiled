// Decompiled with JetBrains decompiler
// Type: UnityEditor.NetworkScenePostProcess
// Assembly: UnityEditor.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6916FF53-78D9-4C64-A56C-55C7AE140DAE
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.Networking.dll

using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityEditor
{
  public class NetworkScenePostProcess : MonoBehaviour
  {
    [PostProcessScene]
    public static void OnPostProcessScene()
    {
      int num = 1;
      foreach (NetworkIdentity networkIdentity in Object.FindObjectsOfType<NetworkIdentity>())
      {
        if ((Object) networkIdentity.GetComponent<NetworkManager>() != (Object) null)
          Debug.LogError((object) "NetworkManager has a NetworkIdentity component. This will cause the NetworkManager object to be disabled, so it is not recommended.");
        if (!networkIdentity.isClient && !networkIdentity.isServer)
        {
          networkIdentity.gameObject.SetActive(false);
          networkIdentity.ForceSceneId(num++);
        }
      }
    }
  }
}
