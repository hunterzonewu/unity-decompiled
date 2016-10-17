// Decompiled with JetBrains decompiler
// Type: UnityEngine.ParticleSystemExtensionsImpl
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  internal sealed class ParticleSystemExtensionsImpl
  {
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int GetSafeCollisionEventSize(ParticleSystem ps);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int GetCollisionEvents(ParticleSystem ps, GameObject go, ParticleCollisionEvent[] collisionEvents);
  }
}
