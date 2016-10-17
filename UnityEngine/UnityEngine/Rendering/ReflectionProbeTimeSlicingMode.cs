// Decompiled with JetBrains decompiler
// Type: UnityEngine.Rendering.ReflectionProbeTimeSlicingMode
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

namespace UnityEngine.Rendering
{
  /// <summary>
  ///         <para>When a probe's ReflectionProbe.refreshMode is set to ReflectionProbeRefreshMode.EveryFrame, this enum specify whether or not Unity should update the probe's cubemap over several frames or update the whole cubemap in one frame.
  /// Updating a probe's cubemap is a costly operation. Unity needs to render the entire scene for each face of the cubemap, as well as perform special blurring in order to get glossy reflections. The impact on frame rate can be significant. Time-slicing helps maintaning a more constant frame rate during these updates by performing the rendering over several frames.</para>
  ///       </summary>
  public enum ReflectionProbeTimeSlicingMode
  {
    AllFacesAtOnce,
    IndividualFaces,
    NoTimeSlicing,
  }
}
