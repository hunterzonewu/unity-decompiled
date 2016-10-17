// Decompiled with JetBrains decompiler
// Type: UnityEditor.ObjectCopier
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace UnityEditor
{
  internal static class ObjectCopier
  {
    public static T DeepClone<T>(T source)
    {
      if (!typeof (T).IsSerializable)
        throw new ArgumentException("The type must be serializable.", "source");
      if (object.ReferenceEquals((object) source, (object) null))
        return default (T);
      IFormatter formatter = (IFormatter) new BinaryFormatter();
      Stream serializationStream = (Stream) new MemoryStream();
      using (serializationStream)
      {
        formatter.Serialize(serializationStream, (object) source);
        serializationStream.Seek(0L, SeekOrigin.Begin);
        return (T) formatter.Deserialize(serializationStream);
      }
    }
  }
}
