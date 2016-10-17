// Decompiled with JetBrains decompiler
// Type: UnityEngine.EventModifiers
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Types of modifier key that can be active during a keystroke event.</para>
  /// </summary>
  [Flags]
  public enum EventModifiers
  {
    None = 0,
    Shift = 1,
    Control = 2,
    Alt = 4,
    Command = 8,
    Numeric = 16,
    CapsLock = 32,
    FunctionKey = 64,
  }
}
