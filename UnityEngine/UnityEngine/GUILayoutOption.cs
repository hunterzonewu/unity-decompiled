// Decompiled with JetBrains decompiler
// Type: UnityEngine.GUILayoutOption
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

namespace UnityEngine
{
  /// <summary>
  ///   <para>Class internally used to pass layout options into GUILayout functions. You don't use these directly, but construct them with the layouting functions in the GUILayout class.</para>
  /// </summary>
  public sealed class GUILayoutOption
  {
    internal GUILayoutOption.Type type;
    internal object value;

    internal GUILayoutOption(GUILayoutOption.Type type, object value)
    {
      this.type = type;
      this.value = value;
    }

    internal enum Type
    {
      fixedWidth,
      fixedHeight,
      minWidth,
      maxWidth,
      minHeight,
      maxHeight,
      stretchWidth,
      stretchHeight,
      alignStart,
      alignMiddle,
      alignEnd,
      alignJustify,
      equalSize,
      spacing,
    }
  }
}
