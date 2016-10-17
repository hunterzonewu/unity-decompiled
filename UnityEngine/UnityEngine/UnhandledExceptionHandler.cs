// Decompiled with JetBrains decompiler
// Type: UnityEngine.UnhandledExceptionHandler
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  internal sealed class UnhandledExceptionHandler
  {
    [RequiredByNativeCode]
    private static void RegisterUECatcher()
    {
      AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionHandler.HandleUnhandledException);
    }

    private static void HandleUnhandledException(object sender, UnhandledExceptionEventArgs args)
    {
      Exception exceptionObject = args.ExceptionObject as Exception;
      if (exceptionObject != null)
        UnhandledExceptionHandler.PrintException("Unhandled Exception: ", exceptionObject);
      UnhandledExceptionHandler.NativeUnhandledExceptionHandler();
    }

    private static void PrintException(string title, Exception e)
    {
      Debug.LogError((object) (title + e.ToString()));
      if (e.InnerException == null)
        return;
      UnhandledExceptionHandler.PrintException("Inner Exception: ", e.InnerException);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void NativeUnhandledExceptionHandler();
  }
}
