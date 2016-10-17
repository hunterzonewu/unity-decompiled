// Decompiled with JetBrains decompiler
// Type: UnityEditor.ITreeViewDataSource
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal interface ITreeViewDataSource
  {
    TreeViewItem root { get; }

    int rowCount { get; }

    void OnInitialize();

    void ReloadData();

    void InitIfNeeded();

    TreeViewItem FindItem(int id);

    int GetRow(int id);

    TreeViewItem GetItem(int row);

    List<TreeViewItem> GetRows();

    bool IsRevealed(int id);

    void RevealItem(int id);

    void SetExpandedWithChildren(TreeViewItem item, bool expand);

    void SetExpanded(TreeViewItem item, bool expand);

    bool IsExpanded(TreeViewItem item);

    bool IsExpandable(TreeViewItem item);

    int[] GetExpandedIDs();

    void SetExpandedIDs(int[] ids);

    bool CanBeMultiSelected(TreeViewItem item);

    bool CanBeParent(TreeViewItem item);

    bool IsRenamingItemAllowed(TreeViewItem item);

    void InsertFakeItem(int id, int parentID, string name, Texture2D icon);

    void RemoveFakeItem();

    bool HasFakeItem();

    void OnSearchChanged();
  }
}
