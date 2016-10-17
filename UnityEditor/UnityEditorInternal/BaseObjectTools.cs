// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.BaseObjectTools
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditorInternal
{
  internal sealed class BaseObjectTools
  {
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string ClassIDToString(int ID);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int StringToClassID(string classString);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int StringToClassIDCaseInsensitive(string classString);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool IsBaseObject(int ID);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool IsDerivedFromClassID(int classID, int derivedFromClassID);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetSuperClassID(int ID);
  }
}
