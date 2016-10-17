// Decompiled with JetBrains decompiler
// Type: UnityEngine.Events.PersistentListenerMode
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine.Events
{
  /// <summary>
  ///   <para>THe mode that a listener is operating in.</para>
  /// </summary>
  [Serializable]
  public enum PersistentListenerMode
  {
    EventDefined,
    Void,
    Object,
    Int,
    Float,
    String,
    Bool,
  }
}
