// Decompiled with JetBrains decompiler
// Type: UnityEditor.ParentViewFolder
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;

namespace UnityEditor
{
  [Serializable]
  internal class ParentViewFolder
  {
    private const string rootDirText = "/";
    private const string assetsFolder = "Assets";
    private const string libraryFolder = "Library";
    public string guid;
    public string name;
    public ChangeFlags changeFlags;
    public ParentViewFile[] files;

    public ParentViewFolder(string name, string guid)
    {
      this.guid = guid;
      this.name = name;
      this.changeFlags = ChangeFlags.None;
      this.files = new ParentViewFile[0];
    }

    public ParentViewFolder(string name, string guid, ChangeFlags flags)
    {
      this.guid = guid;
      this.name = name;
      this.changeFlags = flags;
      this.files = new ParentViewFile[0];
    }

    public static string MakeNiceName(string name)
    {
      if (name.StartsWith("Assets"))
      {
        if (!(name != "Assets"))
          return "/";
        name = name.Substring("Assets".Length + 1);
        if (name == string.Empty)
          return "/";
        return name;
      }
      if (name.StartsWith("Library"))
        return "../" + name;
      if (name == string.Empty)
        return "/";
      return name;
    }

    public ParentViewFolder CloneWithoutFiles()
    {
      return new ParentViewFolder(this.name, this.guid, this.changeFlags);
    }
  }
}
