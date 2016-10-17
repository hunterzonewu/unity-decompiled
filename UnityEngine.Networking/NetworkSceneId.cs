// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkSceneId
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>This is used to identify networked objects in a scene. These values are allocated in the editor and are persistent for the lifetime of the object in the scene.</para>
  /// </summary>
  [Serializable]
  public struct NetworkSceneId
  {
    [SerializeField]
    private uint m_Value;

    /// <summary>
    ///   <para>The internal value for this object.</para>
    /// </summary>
    public uint Value
    {
      get
      {
        return this.m_Value;
      }
    }

    public NetworkSceneId(uint value)
    {
      this.m_Value = value;
    }

    public static bool operator ==(NetworkSceneId c1, NetworkSceneId c2)
    {
      return (int) c1.m_Value == (int) c2.m_Value;
    }

    public static bool operator !=(NetworkSceneId c1, NetworkSceneId c2)
    {
      return (int) c1.m_Value != (int) c2.m_Value;
    }

    /// <summary>
    ///   <para>Returns true if the value is zero. Non-scene objects - ones which are spawned at runtime will have a sceneId of zero.</para>
    /// </summary>
    /// <returns>
    ///   <para>True if zero.</para>
    /// </returns>
    public bool IsEmpty()
    {
      return (int) this.m_Value == 0;
    }

    public override int GetHashCode()
    {
      return (int) this.m_Value;
    }

    public override bool Equals(object obj)
    {
      if (obj is NetworkSceneId)
        return this == (NetworkSceneId) obj;
      return false;
    }

    /// <summary>
    ///   <para>Returns a string like SceneId:value.</para>
    /// </summary>
    /// <returns>
    ///   <para>String representation of this object.</para>
    /// </returns>
    public override string ToString()
    {
      return this.m_Value.ToString();
    }
  }
}
