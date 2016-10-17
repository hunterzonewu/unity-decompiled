// Decompiled with JetBrains decompiler
// Type: UnityEngine.RuntimeInitializeOnLoadManager
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  internal sealed class RuntimeInitializeOnLoadManager
  {
    internal static extern string[] dontStripClassNames { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal static extern RuntimeInitializeMethodInfo[] methodInfos { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void UpdateMethodExecutionOrders(int[] changedIndices, int[] changedOrder);
  }
}
