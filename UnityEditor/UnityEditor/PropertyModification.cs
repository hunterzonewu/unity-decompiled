// Decompiled with JetBrains decompiler
// Type: UnityEditor.PropertyModification
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Defines a single modified property.</para>
  /// </summary>
  [RequiredByNativeCode]
  [StructLayout(LayoutKind.Sequential)]
  public sealed class PropertyModification
  {
    /// <summary>
    ///   <para>Object that will be modified.</para>
    /// </summary>
    public Object target;
    /// <summary>
    ///   <para>Property path of the property being modified (Matches as SerializedProperty.propertyPath).</para>
    /// </summary>
    public string propertyPath;
    /// <summary>
    ///   <para>The value being applied.</para>
    /// </summary>
    public string value;
    /// <summary>
    ///   <para>The value being applied when it is a object reference (which can not be represented as a string).</para>
    /// </summary>
    public Object objectReference;
  }
}
