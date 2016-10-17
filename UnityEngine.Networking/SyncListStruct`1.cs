// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.SyncListStruct`1
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>This class is used for lists of structs that are synchronized from the server to clients.</para>
  /// </summary>
  public class SyncListStruct<T> : SyncList<T> where T : struct
  {
    public ushort Count
    {
      get
      {
        return (ushort) base.Count;
      }
    }

    public new void AddInternal(T item)
    {
      base.AddInternal(item);
    }

    protected override void SerializeItem(NetworkWriter writer, T item)
    {
    }

    protected override T DeserializeItem(NetworkReader reader)
    {
      return default (T);
    }

    public T GetItem(int i)
    {
      return this[i];
    }
  }
}
