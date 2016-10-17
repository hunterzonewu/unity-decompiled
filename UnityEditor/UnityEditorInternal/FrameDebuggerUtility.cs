// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.FrameDebuggerUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditorInternal
{
  internal sealed class FrameDebuggerUtility
  {
    public static extern bool receivingRemoteFrameEventData { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern bool locallySupported { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern int count { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern int limit { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern int eventsHash { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetEnabled(bool enabled, int remotePlayerGUID);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool IsLocalEnabled();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool IsRemoteEnabled();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetRemotePlayerGUID();

    public static void SetRenderTargetDisplayOptions(int rtIndex, Vector4 channels, float blackLevel, float whiteLevel)
    {
      FrameDebuggerUtility.INTERNAL_CALL_SetRenderTargetDisplayOptions(rtIndex, ref channels, blackLevel, whiteLevel);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetRenderTargetDisplayOptions(int rtIndex, ref Vector4 channels, float blackLevel, float whiteLevel);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern FrameDebuggerEvent[] GetFrameEvents();

    public static bool GetFrameEventData(int index, out FrameDebuggerEventData frameDebuggerEventData)
    {
      frameDebuggerEventData = FrameDebuggerUtility.GetFrameEventData();
      return frameDebuggerEventData.frameEventIndex == index;
    }

    private static FrameDebuggerEventData GetFrameEventData()
    {
      FrameDebuggerEventData debuggerEventData;
      FrameDebuggerUtility.INTERNAL_CALL_GetFrameEventData(out debuggerEventData);
      return debuggerEventData;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetFrameEventData(out FrameDebuggerEventData value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetFrameEventInfoName(int index);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern GameObject GetFrameEventGameObject(int index);
  }
}
