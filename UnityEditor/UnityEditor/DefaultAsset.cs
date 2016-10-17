// Decompiled with JetBrains decompiler
// Type: UnityEditor.DefaultAsset
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>DefaultAsset is used for assets that does not have a specific type (yet).</para>
  /// </summary>
  public sealed class DefaultAsset : Object
  {
    internal string message { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal bool isWarning { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }
  }
}
