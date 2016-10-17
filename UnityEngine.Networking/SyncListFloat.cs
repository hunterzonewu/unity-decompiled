// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.SyncListFloat
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>A list of floats that will be synchronized from server to clients.</para>
  /// </summary>
  public sealed class SyncListFloat : SyncList<float>
  {
    protected override void SerializeItem(NetworkWriter writer, float item)
    {
      writer.Write(item);
    }

    protected override float DeserializeItem(NetworkReader reader)
    {
      return reader.ReadSingle();
    }

    [Obsolete("ReadReference is now used instead")]
    public static SyncListFloat ReadInstance(NetworkReader reader)
    {
      ushort num = reader.ReadUInt16();
      SyncListFloat syncListFloat = new SyncListFloat();
      for (ushort index = 0; (int) index < (int) num; ++index)
        syncListFloat.AddInternal(reader.ReadSingle());
      return syncListFloat;
    }

    /// <summary>
    ///   <para>An internal function used for serializing SyncList member variables.</para>
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="syncList"></param>
    public static void ReadReference(NetworkReader reader, SyncListFloat syncList)
    {
      ushort num = reader.ReadUInt16();
      syncList.Clear();
      for (ushort index = 0; (int) index < (int) num; ++index)
        syncList.AddInternal(reader.ReadSingle());
    }

    public static void WriteInstance(NetworkWriter writer, SyncListFloat items)
    {
      writer.Write((ushort) items.Count);
      foreach (float num in (SyncList<float>) items)
        writer.Write(num);
    }
  }
}
