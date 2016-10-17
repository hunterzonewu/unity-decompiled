// Decompiled with JetBrains decompiler
// Type: UnityEditor.HumanTemplate
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  public sealed class HumanTemplate : Object
  {
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public HumanTemplate();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Insert(string name, string templateName);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public string Find(string name);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void ClearTemplate();
  }
}
