// Decompiled with JetBrains decompiler
// Type: UnityEngine.DisallowMultipleComponent
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Prevents MonoBehaviour of same type (or subtype) to be added more than once to a GameObject.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Class, Inherited = false)]
  public sealed class DisallowMultipleComponent : Attribute
  {
  }
}
