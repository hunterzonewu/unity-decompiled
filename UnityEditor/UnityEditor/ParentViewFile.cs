// Decompiled with JetBrains decompiler
// Type: UnityEditor.ParentViewFile
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;

namespace UnityEditor
{
  [Serializable]
  internal class ParentViewFile
  {
    public string guid;
    public string name;
    public ChangeFlags changeFlags;

    public ParentViewFile(string name, string guid)
    {
      this.guid = guid;
      this.name = name;
      this.changeFlags = ChangeFlags.None;
    }

    public ParentViewFile(string name, string guid, ChangeFlags flags)
    {
      this.guid = guid;
      this.name = name;
      this.changeFlags = flags;
    }
  }
}
