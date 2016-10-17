// Decompiled with JetBrains decompiler
// Type: UnityEditor.ParticleSystemEditorUtils
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Internal;

namespace UnityEditor
{
  internal sealed class ParticleSystemEditorUtils
  {
    internal static extern float editorSimulationSpeed { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static extern float editorPlaybackTime { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static extern bool editorIsScrubbing { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static extern bool editorIsPlaying { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static extern bool editorIsPaused { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static extern bool editorResimulation { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static extern bool editorUpdateAll { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static extern ParticleSystem lockedParticleSystem { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string CheckCircularReferences(ParticleSystem subEmitter);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void PerformCompleteResimulation();

    public static ParticleSystem GetRoot(ParticleSystem ps)
    {
      if ((Object) ps == (Object) null)
        return (ParticleSystem) null;
      Transform transform = ps.transform;
      while ((bool) ((Object) transform.parent) && (Object) transform.parent.gameObject.GetComponent<ParticleSystem>() != (Object) null)
        transform = transform.parent;
      return transform.gameObject.GetComponent<ParticleSystem>();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void StopEffect([DefaultValue("true")] bool stop, [DefaultValue("true")] bool clear);

    [ExcludeFromDocs]
    internal static void StopEffect(bool stop)
    {
      bool clear = true;
      ParticleSystemEditorUtils.StopEffect(stop, clear);
    }

    [ExcludeFromDocs]
    internal static void StopEffect()
    {
      ParticleSystemEditorUtils.StopEffect(true, true);
    }
  }
}
