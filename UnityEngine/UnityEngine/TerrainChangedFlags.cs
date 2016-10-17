// Decompiled with JetBrains decompiler
// Type: UnityEngine.TerrainChangedFlags
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  [Flags]
  internal enum TerrainChangedFlags
  {
    NoChange = 0,
    Heightmap = 1,
    TreeInstances = 2,
    DelayedHeightmapUpdate = 4,
    FlushEverythingImmediately = 8,
    RemoveDirtyDetailsImmediately = 16,
    WillBeDestroyed = 256,
  }
}
