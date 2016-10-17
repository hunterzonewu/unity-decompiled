// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.SyncListString
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>This is a list of strings that will be synchronized from the server to clients.</para>
  /// </summary>
  public sealed class SyncListString : SyncList<string>
  {
    protected override void SerializeItem(NetworkWriter writer, string item)
    {
      writer.Write(item);
    }

    protected override string DeserializeItem(NetworkReader reader)
    {
      return reader.ReadString();
    }

    [Obsolete("ReadReference is now used instead")]
    public static SyncListString ReadInstance(NetworkReader reader)
    {
      ushort num = reader.ReadUInt16();
      SyncListString syncListString = new SyncListString();
      for (ushort index = 0; (int) index < (int) num; ++index)
        syncListString.AddInternal(reader.ReadString());
      return syncListString;
    }

    /// <summary>
    ///   <para>An internal function used for serializing SyncList member variables.</para>
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="syncList"></param>
    public static void ReadReference(NetworkReader reader, SyncListString syncList)
    {
      ushort num = reader.ReadUInt16();
      syncList.Clear();
      for (ushort index = 0; (int) index < (int) num; ++index)
        syncList.AddInternal(reader.ReadString());
    }

    public static void WriteInstance(NetworkWriter writer, SyncListString items)
    {
      writer.Write((ushort) items.Count);
      foreach (string str in (SyncList<string>) items)
        writer.Write(str);
    }
  }
}
