// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.SyncListBool
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>A list of booleans that will be synchronized from server to clients.</para>
  /// </summary>
  public class SyncListBool : SyncList<bool>
  {
    protected override void SerializeItem(NetworkWriter writer, bool item)
    {
      writer.Write(item);
    }

    protected override bool DeserializeItem(NetworkReader reader)
    {
      return reader.ReadBoolean();
    }

    [Obsolete("ReadReference is now used instead")]
    public static SyncListBool ReadInstance(NetworkReader reader)
    {
      ushort num = reader.ReadUInt16();
      SyncListBool syncListBool = new SyncListBool();
      for (ushort index = 0; (int) index < (int) num; ++index)
        syncListBool.AddInternal(reader.ReadBoolean());
      return syncListBool;
    }

    /// <summary>
    ///   <para>An internal function used for serializing SyncList member variables.</para>
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="syncList"></param>
    public static void ReadReference(NetworkReader reader, SyncListBool syncList)
    {
      ushort num = reader.ReadUInt16();
      syncList.Clear();
      for (ushort index = 0; (int) index < (int) num; ++index)
        syncList.AddInternal(reader.ReadBoolean());
    }

    public static void WriteInstance(NetworkWriter writer, SyncListBool items)
    {
      writer.Write((ushort) items.Count);
      foreach (bool flag in (SyncList<bool>) items)
        writer.Write(flag);
    }
  }
}
