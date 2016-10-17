// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.ProfilerDriver
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditorInternal
{
  public sealed class ProfilerDriver
  {
    public static string directConnectionPort = "54999";

    public static extern int firstFrameIndex { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern int lastFrameIndex { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern int maxHistoryLength { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern string selectedPropertyPath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern bool profileGPU { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern bool profileEditor { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern bool deepProfiling { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern string directConnectionUrl { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern int connectedProfiler { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern string miniMemoryOverview { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern uint usedHeapSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern uint objectCount { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern bool isGPUProfilerBuggyOnDriver { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern bool isGPUProfilerSupportedByOS { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern bool isGPUProfilerSupported { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void BeginFrame();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void EndFrame();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ResetHistory();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetNextFrameIndex(int frame);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetPreviousFrameIndex(int frame);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetFormattedStatisticsValue(int frame, int identifier);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void GetStatisticsValues(int identifier, int firstFrame, float scale, float[] buffer, out float maxValue);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ClearAllFrames();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string[] GetAllStatisticsProperties();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string[] GetGraphStatisticsPropertiesForArea(ProfilerArea area);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetStatisticsIdentifier(string propertyName);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void CaptureHeapshot();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int[] GetAvailableProfilers();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetConnectionIdentifier(int guid);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool IsIdentifierConnectable(int guid);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool IsIdentifierOnLocalhost(int guid);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool IsConnectionEditor();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void DirectIPConnect(string IP);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void DirectURLConnect(string url);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetOverviewText(ProfilerArea profilerArea, int frame);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void RequestMemorySnapshot();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void RequestObjectMemoryInfo(bool gatherObjectReferences);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void QueryInstrumentableFunctions();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void QueryFunctionCallees(string fullName);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetAutoInstrumentedAssemblies(InstrumentedAssemblyTypes value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void BeginInstrumentFunction(string fullName);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void EndInstrumentFunction(string fullName);
  }
}
