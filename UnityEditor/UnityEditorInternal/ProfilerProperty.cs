// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.ProfilerProperty
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditorInternal
{
  public sealed class ProfilerProperty
  {
    private IntPtr m_Ptr;

    public bool HasChildren { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public bool onlyShowGPUSamples { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public int[] instanceIDs { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public int depth { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public string propertyPath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public string frameFPS { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public string frameTime { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public string frameGpuTime { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public bool frameDataReady { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public ProfilerProperty();

    ~ProfilerProperty()
    {
      this.Dispose();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Dispose();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Cleanup();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool Next(bool enterChildren);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetRoot(int frame, ProfilerColumn profilerSortColumn, ProfilerViewType viewType);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InitializeDetailProperty(ProfilerProperty source);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public string GetTooltip(ProfilerColumn column);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public string GetColumn(ProfilerColumn column);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public AudioProfilerInfo[] GetAudioProfilerInfo();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public string GetAudioProfilerNameByOffset(int offset);
  }
}
