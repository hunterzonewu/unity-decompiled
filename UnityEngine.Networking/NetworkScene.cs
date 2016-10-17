// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkScene
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System.Collections.Generic;

namespace UnityEngine.Networking
{
  internal class NetworkScene
  {
    private static Dictionary<NetworkHash128, GameObject> s_GuidToPrefab = new Dictionary<NetworkHash128, GameObject>();
    private static Dictionary<NetworkHash128, SpawnDelegate> s_SpawnHandlers = new Dictionary<NetworkHash128, SpawnDelegate>();
    private static Dictionary<NetworkHash128, UnSpawnDelegate> s_UnspawnHandlers = new Dictionary<NetworkHash128, UnSpawnDelegate>();
    private Dictionary<NetworkInstanceId, NetworkIdentity> m_LocalObjects = new Dictionary<NetworkInstanceId, NetworkIdentity>();

    internal Dictionary<NetworkInstanceId, NetworkIdentity> localObjects
    {
      get
      {
        return this.m_LocalObjects;
      }
    }

    internal static Dictionary<NetworkHash128, GameObject> guidToPrefab
    {
      get
      {
        return NetworkScene.s_GuidToPrefab;
      }
    }

    internal static Dictionary<NetworkHash128, SpawnDelegate> spawnHandlers
    {
      get
      {
        return NetworkScene.s_SpawnHandlers;
      }
    }

    internal static Dictionary<NetworkHash128, UnSpawnDelegate> unspawnHandlers
    {
      get
      {
        return NetworkScene.s_UnspawnHandlers;
      }
    }

    internal void Shutdown()
    {
      this.ClearLocalObjects();
      NetworkScene.ClearSpawners();
    }

    internal void SetLocalObject(NetworkInstanceId netId, GameObject obj, bool isClient, bool isServer)
    {
      if (LogFilter.logDev)
        Debug.Log((object) ("SetLocalObject " + (object) netId + " " + (object) obj));
      if ((Object) obj == (Object) null)
      {
        this.m_LocalObjects[netId] = (NetworkIdentity) null;
      }
      else
      {
        NetworkIdentity networkIdentity = (NetworkIdentity) null;
        if (this.m_LocalObjects.ContainsKey(netId))
          networkIdentity = this.m_LocalObjects[netId];
        if ((Object) networkIdentity == (Object) null)
        {
          networkIdentity = obj.GetComponent<NetworkIdentity>();
          this.m_LocalObjects[netId] = networkIdentity;
        }
        networkIdentity.UpdateClientServer(isClient, isServer);
      }
    }

    internal GameObject FindLocalObject(NetworkInstanceId netId)
    {
      if (this.m_LocalObjects.ContainsKey(netId))
      {
        NetworkIdentity localObject = this.m_LocalObjects[netId];
        if ((Object) localObject != (Object) null)
          return localObject.gameObject;
      }
      return (GameObject) null;
    }

    internal bool GetNetworkIdentity(NetworkInstanceId netId, out NetworkIdentity uv)
    {
      if (this.m_LocalObjects.ContainsKey(netId) && (Object) this.m_LocalObjects[netId] != (Object) null)
      {
        uv = this.m_LocalObjects[netId];
        return true;
      }
      uv = (NetworkIdentity) null;
      return false;
    }

    internal bool RemoveLocalObject(NetworkInstanceId netId)
    {
      return this.m_LocalObjects.Remove(netId);
    }

    internal bool RemoveLocalObjectAndDestroy(NetworkInstanceId netId)
    {
      if (!this.m_LocalObjects.ContainsKey(netId))
        return false;
      Object.Destroy((Object) this.m_LocalObjects[netId].gameObject);
      return this.m_LocalObjects.Remove(netId);
    }

    internal void ClearLocalObjects()
    {
      this.m_LocalObjects.Clear();
    }

    internal static void RegisterPrefab(GameObject prefab, NetworkHash128 newAssetId)
    {
      NetworkIdentity component = prefab.GetComponent<NetworkIdentity>();
      if ((bool) ((Object) component))
      {
        component.SetDynamicAssetId(newAssetId);
        if (LogFilter.logDebug)
          Debug.Log((object) ("Registering prefab '" + prefab.name + "' as asset:" + (object) component.assetId));
        NetworkScene.s_GuidToPrefab[component.assetId] = prefab;
      }
      else
      {
        if (!LogFilter.logError)
          return;
        Debug.LogError((object) ("Could not register '" + prefab.name + "' since it contains no NetworkIdentity component"));
      }
    }

    internal static void RegisterPrefab(GameObject prefab)
    {
      NetworkIdentity component = prefab.GetComponent<NetworkIdentity>();
      if ((bool) ((Object) component))
      {
        if (LogFilter.logDebug)
          Debug.Log((object) ("Registering prefab '" + prefab.name + "' as asset:" + (object) component.assetId));
        NetworkScene.s_GuidToPrefab[component.assetId] = prefab;
        if (prefab.GetComponentsInChildren<NetworkIdentity>().Length <= 1 || !LogFilter.logWarn)
          return;
        Debug.LogWarning((object) ("The prefab '" + prefab.name + "' has multiple NetworkIdentity components. There can only be one NetworkIdentity on a prefab, and it must be on the root object."));
      }
      else
      {
        if (!LogFilter.logError)
          return;
        Debug.LogError((object) ("Could not register '" + prefab.name + "' since it contains no NetworkIdentity component"));
      }
    }

    internal static bool GetPrefab(NetworkHash128 assetId, out GameObject prefab)
    {
      if (!assetId.IsValid())
      {
        prefab = (GameObject) null;
        return false;
      }
      if (NetworkScene.s_GuidToPrefab.ContainsKey(assetId) && (Object) NetworkScene.s_GuidToPrefab[assetId] != (Object) null)
      {
        prefab = NetworkScene.s_GuidToPrefab[assetId];
        return true;
      }
      prefab = (GameObject) null;
      return false;
    }

    internal static void ClearSpawners()
    {
      NetworkScene.s_GuidToPrefab.Clear();
      NetworkScene.s_SpawnHandlers.Clear();
      NetworkScene.s_UnspawnHandlers.Clear();
    }

    public static void UnregisterSpawnHandler(NetworkHash128 assetId)
    {
      NetworkScene.s_SpawnHandlers.Remove(assetId);
      NetworkScene.s_UnspawnHandlers.Remove(assetId);
    }

    internal static void RegisterSpawnHandler(NetworkHash128 assetId, SpawnDelegate spawnHandler, UnSpawnDelegate unspawnHandler)
    {
      if (spawnHandler == null || unspawnHandler == null)
      {
        if (!LogFilter.logError)
          return;
        Debug.LogError((object) ("RegisterSpawnHandler custom spawn function null for " + (object) assetId));
      }
      else
      {
        if (LogFilter.logDebug)
          Debug.Log((object) ("RegisterSpawnHandler asset '" + (object) assetId + "' " + spawnHandler.Method.Name + "/" + unspawnHandler.Method.Name));
        NetworkScene.s_SpawnHandlers[assetId] = spawnHandler;
        NetworkScene.s_UnspawnHandlers[assetId] = unspawnHandler;
      }
    }

    internal static void UnregisterPrefab(GameObject prefab)
    {
      NetworkIdentity component = prefab.GetComponent<NetworkIdentity>();
      if ((Object) component == (Object) null)
      {
        if (!LogFilter.logError)
          return;
        Debug.LogError((object) ("Could not unregister '" + prefab.name + "' since it contains no NetworkIdentity component"));
      }
      else
      {
        NetworkScene.s_SpawnHandlers.Remove(component.assetId);
        NetworkScene.s_UnspawnHandlers.Remove(component.assetId);
      }
    }

    internal static void RegisterPrefab(GameObject prefab, SpawnDelegate spawnHandler, UnSpawnDelegate unspawnHandler)
    {
      NetworkIdentity component = prefab.GetComponent<NetworkIdentity>();
      if ((Object) component == (Object) null)
      {
        if (!LogFilter.logError)
          return;
        Debug.LogError((object) ("Could not register '" + prefab.name + "' since it contains no NetworkIdentity component"));
      }
      else if (spawnHandler == null || unspawnHandler == null)
      {
        if (!LogFilter.logError)
          return;
        Debug.LogError((object) ("RegisterPrefab custom spawn function null for " + (object) component.assetId));
      }
      else if (!component.assetId.IsValid())
      {
        if (!LogFilter.logError)
          return;
        Debug.LogError((object) ("RegisterPrefab game object " + prefab.name + " has no prefab. Use RegisterSpawnHandler() instead?"));
      }
      else
      {
        if (LogFilter.logDebug)
          Debug.Log((object) ("Registering custom prefab '" + prefab.name + "' as asset:" + (object) component.assetId + " " + spawnHandler.Method.Name + "/" + unspawnHandler.Method.Name));
        NetworkScene.s_SpawnHandlers[component.assetId] = spawnHandler;
        NetworkScene.s_UnspawnHandlers[component.assetId] = unspawnHandler;
      }
    }

    internal static bool GetSpawnHandler(NetworkHash128 assetId, out SpawnDelegate handler)
    {
      if (NetworkScene.s_SpawnHandlers.ContainsKey(assetId))
      {
        handler = NetworkScene.s_SpawnHandlers[assetId];
        return true;
      }
      handler = (SpawnDelegate) null;
      return false;
    }

    internal static bool InvokeUnSpawnHandler(NetworkHash128 assetId, GameObject obj)
    {
      if (!NetworkScene.s_UnspawnHandlers.ContainsKey(assetId) || NetworkScene.s_UnspawnHandlers[assetId] == null)
        return false;
      NetworkScene.s_UnspawnHandlers[assetId](obj);
      return true;
    }

    internal void DestroyAllClientObjects()
    {
      using (Dictionary<NetworkInstanceId, NetworkIdentity>.KeyCollection.Enumerator enumerator = this.m_LocalObjects.Keys.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          NetworkIdentity localObject = this.m_LocalObjects[enumerator.Current];
          if ((Object) localObject != (Object) null && (Object) localObject.gameObject != (Object) null)
          {
            if (localObject.sceneId.IsEmpty())
              Object.Destroy((Object) localObject.gameObject);
            else
              localObject.gameObject.SetActive(false);
          }
        }
      }
      this.ClearLocalObjects();
    }

    internal void DumpAllClientObjects()
    {
      using (Dictionary<NetworkInstanceId, NetworkIdentity>.KeyCollection.Enumerator enumerator = this.m_LocalObjects.Keys.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          NetworkInstanceId current = enumerator.Current;
          NetworkIdentity localObject = this.m_LocalObjects[current];
          if ((Object) localObject != (Object) null)
            Debug.Log((object) ("ID:" + (object) current + " OBJ:" + (object) localObject.gameObject + " AS:" + (object) localObject.assetId));
          else
            Debug.Log((object) ("ID:" + (object) current + " OBJ: null"));
        }
      }
    }
  }
}
