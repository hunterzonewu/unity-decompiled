// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AudioProfilerInfoWrapper
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditorInternal
{
  internal class AudioProfilerInfoWrapper
  {
    public AudioProfilerInfo info;
    public string assetName;
    public string objectName;
    public bool addToRoot;

    public AudioProfilerInfoWrapper(AudioProfilerInfo info, string assetName, string objectName, bool addToRoot)
    {
      this.info = info;
      this.assetName = assetName;
      this.objectName = objectName;
      this.addToRoot = addToRoot;
    }
  }
}
