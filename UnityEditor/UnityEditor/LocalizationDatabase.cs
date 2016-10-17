// Decompiled with JetBrains decompiler
// Type: UnityEditor.LocalizationDatabase
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  public sealed class LocalizationDatabase
  {
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern SystemLanguage GetDefaultEditorLanguage();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetCurrentEditorLanguage(SystemLanguage lang);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern SystemLanguage GetCurrentEditorLanguage();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void ReadEditorLocalizationResources();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern SystemLanguage[] GetAvailableEditorLanguages();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetLocalizedString(string original);

    public static string MarkForTranslation(string value)
    {
      return value;
    }
  }
}
