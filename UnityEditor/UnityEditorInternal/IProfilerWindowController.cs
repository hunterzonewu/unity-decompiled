// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.IProfilerWindowController
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditorInternal
{
  internal interface IProfilerWindowController
  {
    void SetSelectedPropertyPath(string path);

    void ClearSelectedPropertyPath();

    ProfilerProperty CreateProperty(bool details);

    int GetActiveVisibleFrameIndex();

    void SetSearch(string searchString);

    string GetSearch();

    bool IsSearching();

    void Repaint();
  }
}
