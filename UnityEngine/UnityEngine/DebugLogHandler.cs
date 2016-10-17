// Decompiled with JetBrains decompiler
// Type: UnityEngine.DebugLogHandler
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
  internal sealed class DebugLogHandler : ILogHandler
  {
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void Internal_Log(LogType level, string msg, [Writable] Object obj);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void Internal_LogException(Exception exception, [Writable] Object obj);

    public void LogFormat(LogType logType, Object context, string format, params object[] args)
    {
      DebugLogHandler.Internal_Log(logType, string.Format(format, args), context);
    }

    public void LogException(Exception exception, Object context)
    {
      DebugLogHandler.Internal_LogException(exception, context);
    }
  }
}
