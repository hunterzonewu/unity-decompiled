// Decompiled with JetBrains decompiler
// Type: UnityEngine.RuntimeInitializeOnLoadMethodAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Allow an runtime class method to be initialized when Unity game loads runtime without action from the user.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
  public class RuntimeInitializeOnLoadMethodAttribute : PreserveAttribute
  {
    /// <summary>
    ///   <para>Set RuntimeInitializeOnLoadMethod type.</para>
    /// </summary>
    public RuntimeInitializeLoadType loadType { get; private set; }

    /// <summary>
    ///   <para>Allow an runtime class method to be initialized when Unity game loads runtime without action from the user.</para>
    /// </summary>
    /// <param name="loadType">RuntimeInitializeLoadType: Before or After scene is loaded.</param>
    public RuntimeInitializeOnLoadMethodAttribute()
    {
      this.loadType = RuntimeInitializeLoadType.AfterSceneLoad;
    }

    /// <summary>
    ///   <para>Allow an runtime class method to be initialized when Unity game loads runtime without action from the user.</para>
    /// </summary>
    /// <param name="loadType">RuntimeInitializeLoadType: Before or After scene is loaded.</param>
    public RuntimeInitializeOnLoadMethodAttribute(RuntimeInitializeLoadType loadType)
    {
      this.loadType = loadType;
    }
  }
}
