// Decompiled with JetBrains decompiler
// Type: UnityEditor.ClipAnimationInfoCurve
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Stores a curve and its name that will be used to create additionnal curves during the import process.</para>
  /// </summary>
  public struct ClipAnimationInfoCurve
  {
    /// <summary>
    ///   <para>The name of the animation curve.</para>
    /// </summary>
    public string name;
    /// <summary>
    ///   <para>The animation curve.</para>
    /// </summary>
    public AnimationCurve curve;
  }
}
