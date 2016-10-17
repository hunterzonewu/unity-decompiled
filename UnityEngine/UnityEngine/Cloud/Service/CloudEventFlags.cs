// Decompiled with JetBrains decompiler
// Type: UnityEngine.Cloud.Service.CloudEventFlags
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine.Cloud.Service
{
  [Flags]
  internal enum CloudEventFlags
  {
    None = 0,
    HighPriority = 1,
    CacheImmediately = 2,
  }
}
