// Decompiled with JetBrains decompiler
// Type: UnityEditor.MonoImporter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  public sealed class MonoImporter : AssetImporter
  {
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetDefaultReferences(string[] name, Object[] target);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern MonoScript[] GetAllRuntimeMonoScripts();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetExecutionOrder(MonoScript script, int order);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void CopyMonoScriptIconToImporters(MonoScript script);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetExecutionOrder(MonoScript script);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public MonoScript GetScript();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public Object GetDefaultReference(string name);
  }
}
