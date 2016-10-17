// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.ProfilerFrameDataIterator
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditorInternal
{
  public sealed class ProfilerFrameDataIterator
  {
    private IntPtr m_Ptr;

    public int group { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public int depth { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public string path { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public string name { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public int id { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public int instanceId { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public float frameTimeMS { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public float startTimeMS { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public float durationMS { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public ProfilerFrameDataIterator();

    ~ProfilerFrameDataIterator()
    {
      this.Dispose();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Dispose();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool Next(bool enterChildren);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public int GetThreadCount(int frame);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public double GetFrameStartS(int frame);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public int GetGroupCount(int frame);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public string GetGroupName();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public string GetThreadName();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetRoot(int frame, int threadIdx);
  }
}
