// Decompiled with JetBrains decompiler
// Type: UnityEngine.CameraType
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Describes different types of camera.</para>
  /// </summary>
  [Flags]
  public enum CameraType
  {
    Game = 1,
    SceneView = 2,
    Preview = 4,
  }
}
