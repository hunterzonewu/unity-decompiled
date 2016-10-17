// Decompiled with JetBrains decompiler
// Type: UnityEngine.UICharInfo
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Class that specifes some information about a renderable character.</para>
  /// </summary>
  [UsedByNativeCode]
  public struct UICharInfo
  {
    /// <summary>
    ///   <para>Position of the character cursor in local (text generated) space.</para>
    /// </summary>
    public Vector2 cursorPos;
    /// <summary>
    ///   <para>Character width.</para>
    /// </summary>
    public float charWidth;
  }
}
