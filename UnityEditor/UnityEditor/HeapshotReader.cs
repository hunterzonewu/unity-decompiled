// Decompiled with JetBrains decompiler
// Type: UnityEditor.HeapshotReader
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;

namespace UnityEditor
{
  internal class HeapshotReader
  {
    private Dictionary<uint, HeapshotReader.TypeInfo> types = new Dictionary<uint, HeapshotReader.TypeInfo>();
    private Dictionary<uint, HeapshotReader.ObjectInfo> objects = new Dictionary<uint, HeapshotReader.ObjectInfo>();
    private List<HeapshotReader.ReferenceInfo> roots = new List<HeapshotReader.ReferenceInfo>();
    private List<HeapshotReader.ObjectInfo> allObjects = new List<HeapshotReader.ObjectInfo>();
    private List<HeapshotReader.TypeInfo> allTypes = new List<HeapshotReader.TypeInfo>();
    private HeapshotReader.ObjectInfo kUnmanagedObject = new HeapshotReader.ObjectInfo(new HeapshotReader.TypeInfo("Unmanaged"), 0U);
    private HeapshotReader.ObjectInfo kUnity = new HeapshotReader.ObjectInfo(new HeapshotReader.TypeInfo("<Unity>"), 0U, HeapshotReader.ObjectType.UnityRoot);
    private const uint kMagicNumber = 1319894481;
    private const int kLogVersion = 6;
    private const string kLogFileLabel = "heap-shot logfile";

    public List<HeapshotReader.ReferenceInfo> Roots
    {
      get
      {
        return this.roots;
      }
    }

    public List<HeapshotReader.ObjectInfo> Objects
    {
      get
      {
        return this.allObjects;
      }
    }

    public List<HeapshotReader.TypeInfo> Types
    {
      get
      {
        return this.allTypes;
      }
    }

    public List<HeapshotReader.ObjectInfo> GetObjectsOfType(string name)
    {
      List<HeapshotReader.ObjectInfo> objectInfoList = new List<HeapshotReader.ObjectInfo>();
      using (List<HeapshotReader.ObjectInfo>.Enumerator enumerator = this.allObjects.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          HeapshotReader.ObjectInfo current = enumerator.Current;
          if (current.typeInfo.name == name)
            objectInfoList.Add(current);
        }
      }
      return objectInfoList;
    }

    public bool Open(string fileName)
    {
      this.types.Clear();
      this.objects.Clear();
      this.roots.Clear();
      this.allObjects.Clear();
      this.allTypes.Clear();
      Stream input;
      try
      {
        input = (Stream) new FileStream(fileName, System.IO.FileMode.Open, FileAccess.Read);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return false;
      }
      BinaryReader reader;
      try
      {
        reader = new BinaryReader(input);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        input.Close();
        return false;
      }
      this.ReadHeader(reader);
      do
        ;
      while (this.ReadData(reader));
      this.ResolveReferences();
      this.ResolveInverseReferences();
      this.ResolveRoots();
      reader.Close();
      input.Close();
      return true;
    }

    private void ReadHeader(BinaryReader reader)
    {
      uint num1 = reader.ReadUInt32();
      if ((int) num1 != 1319894481)
        throw new Exception(string.Format("Bad magic number: expected {0}, found {1}", (object) 1319894481U, (object) num1));
      int num2 = reader.ReadInt32();
      string str = reader.ReadString();
      if (!(str == "heap-shot logfile"))
        throw new Exception("Unknown file label in heap-shot outfile");
      int num3 = 6;
      if (num2 != num3)
        throw new Exception(string.Format("Version error in {0}: expected {1}, found {2}", (object) str, (object) num3, (object) num2));
      int num4 = (int) reader.ReadUInt32();
      int num5 = (int) reader.ReadUInt32();
      int num6 = (int) reader.ReadUInt32();
      int num7 = (int) reader.ReadUInt32();
    }

    private bool ReadData(BinaryReader reader)
    {
      HeapshotReader.Tag tag = (HeapshotReader.Tag) reader.ReadByte();
      switch (tag)
      {
        case HeapshotReader.Tag.Type:
          this.ReadType(reader);
          break;
        case HeapshotReader.Tag.Object:
          this.ReadObject(reader);
          break;
        case HeapshotReader.Tag.UnityObjects:
          this.ReadUnityObjects(reader);
          break;
        case HeapshotReader.Tag.EndOfFile:
          return false;
        default:
          throw new Exception("Unknown tag! " + (object) tag);
      }
      return true;
    }

    private void ReadType(BinaryReader reader)
    {
      uint key = reader.ReadUInt32();
      HeapshotReader.TypeInfo typeInfo = new HeapshotReader.TypeInfo();
      typeInfo.name = reader.ReadString();
      uint index;
      while ((int) (index = reader.ReadUInt32()) != 0)
        typeInfo.fields[index] = new HeapshotReader.FieldInfo()
        {
          name = reader.ReadString()
        };
      if (this.types.ContainsKey(key))
        throw new Exception(string.Format("Type info for object {0} was already loaded!!!", (object) key));
      this.types[key] = typeInfo;
      this.allTypes.Add(typeInfo);
    }

    private void ReadObject(BinaryReader reader)
    {
      uint key1 = reader.ReadUInt32();
      uint key2 = reader.ReadUInt32();
      HeapshotReader.ObjectInfo objectInfo = new HeapshotReader.ObjectInfo();
      objectInfo.code = key1;
      objectInfo.size = reader.ReadUInt32();
      if (!this.types.ContainsKey(key2))
        throw new Exception(string.Format("Failed to find type info {0} for object {1}!!!", (object) key2, (object) key1));
      objectInfo.typeInfo = this.types[key2];
      uint num;
      while ((int) (num = reader.ReadUInt32()) != 0)
      {
        HeapshotReader.ReferenceInfo referenceInfo = new HeapshotReader.ReferenceInfo();
        referenceInfo.code = num;
        uint key3 = reader.ReadUInt32();
        referenceInfo.fieldInfo = (int) key3 != 0 ? (!objectInfo.typeInfo.fields.ContainsKey(key3) ? (HeapshotReader.FieldInfo) null : objectInfo.typeInfo.fields[key3]) : (HeapshotReader.FieldInfo) null;
        objectInfo.references.Add(referenceInfo);
      }
      if (this.objects.ContainsKey(key1))
        throw new Exception(string.Format("Object {0} was already loaded?!", (object) key1));
      objectInfo.type = (int) key1 != (int) key2 ? HeapshotReader.ObjectType.Managed : HeapshotReader.ObjectType.Root;
      this.objects[key1] = objectInfo;
      this.allObjects.Add(objectInfo);
    }

    private void ReadUnityObjects(BinaryReader reader)
    {
      uint key;
      while ((int) (key = reader.ReadUInt32()) != 0)
      {
        if (this.objects.ContainsKey(key))
          this.objects[key].inverseReferences.Add(new HeapshotReader.BackReferenceInfo()
          {
            parentObject = this.kUnity,
            fieldInfo = new HeapshotReader.FieldInfo(this.objects[key].typeInfo.name)
          });
      }
    }

    private void ResolveReferences()
    {
      using (Dictionary<uint, HeapshotReader.ObjectInfo>.Enumerator enumerator1 = this.objects.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          using (List<HeapshotReader.ReferenceInfo>.Enumerator enumerator2 = enumerator1.Current.Value.references.GetEnumerator())
          {
            while (enumerator2.MoveNext())
            {
              HeapshotReader.ReferenceInfo current = enumerator2.Current;
              if (!this.objects.ContainsKey(current.code))
              {
                current.referencedObject = this.kUnmanagedObject;
              }
              else
              {
                current.referencedObject = this.objects[current.code];
                if (current.fieldInfo == null)
                {
                  current.fieldInfo = new HeapshotReader.FieldInfo();
                  current.fieldInfo.name = current.referencedObject.typeInfo.name;
                }
              }
            }
          }
        }
      }
    }

    private void ResolveInverseReferences()
    {
      using (Dictionary<uint, HeapshotReader.ObjectInfo>.Enumerator enumerator1 = this.objects.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          KeyValuePair<uint, HeapshotReader.ObjectInfo> current1 = enumerator1.Current;
          using (List<HeapshotReader.ReferenceInfo>.Enumerator enumerator2 = current1.Value.references.GetEnumerator())
          {
            while (enumerator2.MoveNext())
            {
              HeapshotReader.ReferenceInfo current2 = enumerator2.Current;
              current2.referencedObject.inverseReferences.Add(new HeapshotReader.BackReferenceInfo()
              {
                parentObject = current1.Value,
                fieldInfo = current2.fieldInfo
              });
            }
          }
        }
      }
    }

    private void ResolveRoots()
    {
      using (Dictionary<uint, HeapshotReader.ObjectInfo>.Enumerator enumerator1 = this.objects.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          KeyValuePair<uint, HeapshotReader.ObjectInfo> current = enumerator1.Current;
          if (current.Value.type == HeapshotReader.ObjectType.Root)
          {
            using (List<HeapshotReader.ReferenceInfo>.Enumerator enumerator2 = current.Value.references.GetEnumerator())
            {
              while (enumerator2.MoveNext())
                this.roots.Add(enumerator2.Current);
            }
          }
        }
      }
    }

    public enum Tag
    {
      Type = 1,
      Object = 2,
      UnityObjects = 3,
      EndOfFile = 255,
    }

    public enum ObjectType
    {
      None,
      Root,
      Managed,
      UnityRoot,
    }

    public class FieldInfo
    {
      public string name = string.Empty;

      public FieldInfo()
      {
      }

      public FieldInfo(string name)
      {
        this.name = name;
      }
    }

    public class TypeInfo
    {
      public string name = string.Empty;
      public Dictionary<uint, HeapshotReader.FieldInfo> fields = new Dictionary<uint, HeapshotReader.FieldInfo>();

      public TypeInfo()
      {
      }

      public TypeInfo(string name)
      {
        this.name = name;
      }
    }

    public class ReferenceInfo
    {
      public uint code;
      public HeapshotReader.ObjectInfo referencedObject;
      public HeapshotReader.FieldInfo fieldInfo;

      public ReferenceInfo()
      {
      }

      public ReferenceInfo(HeapshotReader.ObjectInfo refObj, HeapshotReader.FieldInfo field)
      {
        this.referencedObject = refObj;
        this.fieldInfo = field;
      }
    }

    public class BackReferenceInfo
    {
      public HeapshotReader.ObjectInfo parentObject;
      public HeapshotReader.FieldInfo fieldInfo;
    }

    public class ObjectInfo
    {
      public List<HeapshotReader.ReferenceInfo> references = new List<HeapshotReader.ReferenceInfo>();
      public List<HeapshotReader.BackReferenceInfo> inverseReferences = new List<HeapshotReader.BackReferenceInfo>();
      public uint code;
      public HeapshotReader.TypeInfo typeInfo;
      public uint size;
      public HeapshotReader.ObjectType type;

      public ObjectInfo()
      {
      }

      public ObjectInfo(HeapshotReader.TypeInfo typeInfo, uint size)
      {
        this.typeInfo = typeInfo;
        this.size = size;
      }

      public ObjectInfo(HeapshotReader.TypeInfo typeInfo, uint size, HeapshotReader.ObjectType type)
      {
        this.typeInfo = typeInfo;
        this.size = size;
        this.type = type;
      }
    }
  }
}
