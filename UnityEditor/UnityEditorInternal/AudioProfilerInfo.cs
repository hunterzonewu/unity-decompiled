// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AudioProfilerInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;

namespace UnityEditorInternal
{
  [Serializable]
  public struct AudioProfilerInfo
  {
    public int assetInstanceId;
    public int objectInstanceId;
    public int assetNameOffset;
    public int objectNameOffset;
    public int parentId;
    public int uniqueId;
    public int flags;
    public int playCount;
    public float distanceToListener;
    public float volume;
    public float audibility;
    public float minDist;
    public float maxDist;
    public float time;
    public float duration;
    public float frequency;
  }
}
