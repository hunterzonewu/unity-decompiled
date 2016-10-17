// Decompiled with JetBrains decompiler
// Type: UnityEditor.MovieImporter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>AssetImporter for importing MovieTextures.</para>
  /// </summary>
  public sealed class MovieImporter : AssetImporter
  {
    /// <summary>
    ///   <para>Quality setting to use when importing the movie. This is a float value from 0 to 1.</para>
    /// </summary>
    public float quality { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is the movie texture storing non-color data?</para>
    /// </summary>
    public bool linearTexture { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Duration of the Movie to be imported in seconds.</para>
    /// </summary>
    public float duration { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }
  }
}
