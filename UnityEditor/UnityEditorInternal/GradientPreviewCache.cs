// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.GradientPreviewCache
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal sealed class GradientPreviewCache
  {
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ClearCache();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Texture2D GetPropertyPreview(SerializedProperty property);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Texture2D GetGradientPreview(Gradient curve);
  }
}
