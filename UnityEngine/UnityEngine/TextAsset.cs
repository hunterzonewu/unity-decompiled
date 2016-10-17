// Decompiled with JetBrains decompiler
// Type: UnityEngine.TextAsset
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Text file assets.</para>
  /// </summary>
  public class TextAsset : Object
  {
    /// <summary>
    ///   <para>The text contents of the .txt file as a string. (Read Only)</para>
    /// </summary>
    public string text { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The raw bytes of the text asset. (Read Only)</para>
    /// </summary>
    public byte[] bytes { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public override string ToString()
    {
      return this.text;
    }
  }
}
