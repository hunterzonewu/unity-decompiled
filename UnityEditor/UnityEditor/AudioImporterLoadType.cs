// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioImporterLoadType
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.ComponentModel;

namespace UnityEditor
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  [Obsolete("UnityEditor.AudioImporterLoadType has been deprecated. Use UnityEngine.AudioClipLoadType instead (UnityUpgradable) -> [UnityEngine] UnityEngine.AudioClipLoadType", true)]
  public enum AudioImporterLoadType
  {
    CompressedInMemory = -1,
    DecompressOnLoad = -1,
    [Obsolete("UnityEditor.AudioImporterLoadType.StreamFromDisc has been deprecated. Use UnityEngine.AudioClipLoadType.Streaming instead (UnityUpgradable) -> UnityEngine.AudioClipLoadType.Streaming", true)] StreamFromDisc = -1,
  }
}
