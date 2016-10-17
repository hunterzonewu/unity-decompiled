// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkInstanceId
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>This is used to identify networked objects across all participants of a network. It is assigned at runtime by the server when an object is spawned.</para>
  /// </summary>
  [Serializable]
  public struct NetworkInstanceId
  {
    /// <summary>
    ///   <para>A static invalid NetworkInstanceId that can be used for comparisons.</para>
    /// </summary>
    public static NetworkInstanceId Invalid = new NetworkInstanceId(uint.MaxValue);
    internal static NetworkInstanceId Zero = new NetworkInstanceId(0U);
    [SerializeField]
    private readonly uint m_Value;

    /// <summary>
    ///   <para>The internal value of this identifier.</para>
    /// </summary>
    public uint Value
    {
      get
      {
        return this.m_Value;
      }
    }

    public NetworkInstanceId(uint value)
    {
      this.m_Value = value;
    }

    public static bool operator ==(NetworkInstanceId c1, NetworkInstanceId c2)
    {
      return (int) c1.m_Value == (int) c2.m_Value;
    }

    public static bool operator !=(NetworkInstanceId c1, NetworkInstanceId c2)
    {
      return (int) c1.m_Value != (int) c2.m_Value;
    }

    /// <summary>
    ///   <para>Returns true if the value of the NetworkInstanceId is zero.</para>
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
      if (obj is NetworkInstanceId)
        return this == (NetworkInstanceId) obj;
      return false;
    }

    /// <summary>
    ///   <para>Returns a string of "NetID:value".</para>
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
