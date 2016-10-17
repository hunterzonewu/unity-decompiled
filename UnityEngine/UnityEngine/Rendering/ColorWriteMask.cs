// Decompiled with JetBrains decompiler
// Type: UnityEngine.Rendering.ColorWriteMask
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine.Rendering
{
  /// <summary>
  ///   <para>Specifies which color components will get written into the target framebuffer.</para>
  /// </summary>
  [Flags]
  public enum ColorWriteMask
  {
    Alpha = 1,
    Blue = 2,
    Green = 4,
    Red = 8,
    All = Red | Green | Blue | Alpha,
  }
}
