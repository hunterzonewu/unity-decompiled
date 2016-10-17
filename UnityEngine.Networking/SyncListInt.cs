// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.SyncListInt
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>A list of integers that will be synchronized from server to clients.</para>
  /// </summary>
  public class SyncListInt : SyncList<int>
  {
    protected override void SerializeItem(NetworkWriter writer, int item)
    {
      writer.WritePackedUInt32((uint) item);
    }

    protected override int DeserializeItem(NetworkReader reader)
    {
      return (int) reader.ReadPackedUInt32();
    }

    [Obsolete("ReadReference is now used instead")]
    public static SyncListInt ReadInstance(NetworkReader reader)
    {
      ushort num = reader.ReadUInt16();
      SyncListInt syncListInt = new SyncListInt();
      for (ushort index = 0; (int) index < (int) num; ++index)
        syncListInt.AddInternal((int) reader.ReadPackedUInt32());
      return syncListInt;
    }

    /// <summary>
    ///   <para>An internal function used for serializing SyncList member variables.</para>
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="syncList"></param>
    public static void ReadReference(NetworkReader reader, SyncListInt syncList)
    {
      ushort num = reader.ReadUInt16();
      syncList.Clear();
      for (ushort index = 0; (int) index < (int) num; ++index)
        syncList.AddInternal((int) reader.ReadPackedUInt32());
    }

    public static void WriteInstance(NetworkWriter writer, SyncListInt items)
    {
      writer.Write((ushort) items.Count);
      foreach (int num in (SyncList<int>) items)
        writer.WritePackedUInt32((uint) num);
    }
  }
}
