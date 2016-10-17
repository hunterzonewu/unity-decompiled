// Decompiled with JetBrains decompiler
// Type: UnityEngine.UILineInfo
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Information about a generated line of text.</para>
  /// </summary>
  [UsedByNativeCode]
  public struct UILineInfo
  {
    /// <summary>
    ///   <para>Index of the first character in the line.</para>
    /// </summary>
    public int startCharIdx;
    /// <summary>
    ///   <para>Height of the line.</para>
    /// </summary>
    public int height;
    /// <summary>
    ///   <para>The upper Y position of the line in pixels. This is used for text annotation such as the caret and selection box in the InputField.</para>
    /// </summary>
    public float topY;
  }
}
