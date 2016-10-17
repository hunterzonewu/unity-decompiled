// Decompiled with JetBrains decompiler
// Type: UnityEngineInternal.GIDebugVisualisation
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEngineInternal
{
  public sealed class GIDebugVisualisation
  {
    public static extern bool cycleMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern bool pauseCycleMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern GITextureType texType { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ResetRuntimeInputTextures();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void PlayCycleMode();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void PauseCycleMode();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void StopCycleMode();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void CycleSkipInstances(int skip);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void CycleSkipSystems(int skip);
  }
}
