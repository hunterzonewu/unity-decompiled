// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.SyncListUInt
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>A list of unsigned integers that will be synchronized from server to clients.</para>
  /// </summary>
  public class SyncListUInt : SyncList<uint>
  {
    protected override void SerializeItem(NetworkWriter writer, uint item)
    {
      writer.WritePackedUInt32(item);
    }

    protected override uint DeserializeItem(NetworkReader reader)
    {
      return reader.ReadPackedUInt32();
    }

    [Obsolete("ReadReference is now used instead")]
    public static SyncListUInt ReadInstance(NetworkReader reader)
    {
      ushort num = reader.ReadUInt16();
      SyncListUInt syncListUint = new SyncListUInt();
      for (ushort index = 0; (int) index < (int) num; ++index)
        syncListUint.AddInternal(reader.ReadPackedUInt32());
      return syncListUint;
    }

    /// <summary>
    ///   <para>An internal function used for serializing SyncList member variables.</para>
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="syncList"></param>
    public static void ReadReference(NetworkReader reader, SyncListUInt syncList)
    {
      ushort num = reader.ReadUInt16();
      syncList.Clear();
      for (ushort index = 0; (int) index < (int) num; ++index)
        syncList.AddInternal(reader.ReadPackedUInt32());
    }

    public static void WriteInstance(NetworkWriter writer, SyncListUInt items)
    {
      writer.Write((ushort) items.Count);
      foreach (uint num in (SyncList<uint>) items)
        writer.WritePackedUInt32(num);
    }
  }
}
