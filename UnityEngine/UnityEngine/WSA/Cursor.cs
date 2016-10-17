// Decompiled with JetBrains decompiler
// Type: UnityEngine.WSA.Cursor
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine.WSA
{
  /// <summary>
  ///   <para>Cursor API for Windows Store Apps.</para>
  /// </summary>
  public sealed class Cursor
  {
    /// <summary>
    ///   <para>Set a custom cursor.</para>
    /// </summary>
    /// <param name="id">The cursor resource id.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetCustomCursor(uint id);
  }
}
