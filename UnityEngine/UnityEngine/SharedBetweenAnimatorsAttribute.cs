// Decompiled with JetBrains decompiler
// Type: UnityEngine.SharedBetweenAnimatorsAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>SharedBetweenAnimatorsAttribute is an attribute that specify that this StateMachineBehaviour should be instantiate only once and shared among all Animator instance. This attribute reduce the memory footprint for each controller instance.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  [RequiredByNativeCode]
  public sealed class SharedBetweenAnimatorsAttribute : Attribute
  {
  }
}
