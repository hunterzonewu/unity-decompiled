// Decompiled with JetBrains decompiler
// Type: UnityEditor.SearchFilterTreeItem
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor
{
  internal class SearchFilterTreeItem : TreeViewItem
  {
    private bool m_IsFolder;

    public bool isFolder
    {
      get
      {
        return this.m_IsFolder;
      }
    }

    public SearchFilterTreeItem(int id, int depth, TreeViewItem parent, string displayName, bool isFolder)
      : base(id, depth, parent, displayName)
    {
      this.m_IsFolder = isFolder;
    }
  }
}
