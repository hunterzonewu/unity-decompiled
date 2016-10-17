// Decompiled with JetBrains decompiler
// Type: UnityEditor.ObjectInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;

namespace UnityEditor
{
  internal class ObjectInfo
  {
    public int instanceId;
    public int memorySize;
    public int reason;
    public List<ObjectInfo> referencedBy;
    public string name;
    public string className;
  }
}
