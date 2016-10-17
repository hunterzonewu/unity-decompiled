// Decompiled with JetBrains decompiler
// Type: UnityEngine.Behaviour
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Behaviours are Components that can be enabled or disabled.</para>
  /// </summary>
  public class Behaviour : Component
  {
    /// <summary>
    ///   <para>Enabled Behaviours are Updated, disabled Behaviours are not.</para>
    /// </summary>
    public bool enabled { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Has the Behaviour had enabled called.</para>
    /// </summary>
    public bool isActiveAndEnabled { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }
  }
}
