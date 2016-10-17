// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.ScriptUpdatingManager
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditorInternal
{
  public sealed class ScriptUpdatingManager
  {
    public static extern int numberOfTimesAsked { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool WaitForVCSServerConnection(bool reportTimeout);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ReportExpectedUpdateFailure();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ReportGroupedAPIUpdaterFailure(string msg);
  }
}
