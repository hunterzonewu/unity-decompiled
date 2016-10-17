// Decompiled with JetBrains decompiler
// Type: UnityEngine.Tree
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Tree Component for the tree creator.</para>
  /// </summary>
  public sealed class Tree : Component
  {
    /// <summary>
    ///   <para>Data asociated to the Tree.</para>
    /// </summary>
    public ScriptableObject data { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Tells if there is wind data exported from SpeedTree are saved on this component.</para>
    /// </summary>
    public bool hasSpeedTreeWind { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }
  }
}
