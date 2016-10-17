// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AudioProfilerBackend
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;

namespace UnityEditorInternal
{
  internal class AudioProfilerBackend
  {
    public AudioProfilerBackend.DataUpdateDelegate OnUpdate;
    public AudioProfilerTreeViewState m_TreeViewState;

    public List<AudioProfilerInfoWrapper> items { get; private set; }

    public AudioProfilerBackend(AudioProfilerTreeViewState state)
    {
      this.m_TreeViewState = state;
      this.items = new List<AudioProfilerInfoWrapper>();
    }

    public void SetData(List<AudioProfilerInfoWrapper> data)
    {
      this.items = data;
      this.UpdateSorting();
    }

    public void UpdateSorting()
    {
      this.items.Sort((IComparer<AudioProfilerInfoWrapper>) new AudioProfilerInfoHelper.AudioProfilerInfoComparer((AudioProfilerInfoHelper.ColumnIndices) this.m_TreeViewState.selectedColumn, (AudioProfilerInfoHelper.ColumnIndices) this.m_TreeViewState.prevSelectedColumn, this.m_TreeViewState.sortByDescendingOrder));
      if (this.OnUpdate == null)
        return;
      this.OnUpdate();
    }

    public delegate void DataUpdateDelegate();
  }
}
