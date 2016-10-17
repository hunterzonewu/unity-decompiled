// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.MessageBase
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>Network message classes should be derived from this class. These message classes can then be sent using the various Send functions of NetworkConnection, NetworkClient and NetworkServer.</para>
  /// </summary>
  public abstract class MessageBase
  {
    /// <summary>
    ///   <para>This method is used to populate a message object from a NetworkReader stream.</para>
    /// </summary>
    /// <param name="reader">Stream to read from.</param>
    public virtual void Deserialize(NetworkReader reader)
    {
    }

    /// <summary>
    ///   <para>The method is used to populate a NetworkWriter stream from a message object.</para>
    /// </summary>
    /// <param name="writer">Stream to write to.</param>
    public virtual void Serialize(NetworkWriter writer)
    {
    }
  }
}
