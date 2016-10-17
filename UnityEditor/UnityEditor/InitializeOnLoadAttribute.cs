// Decompiled with JetBrains decompiler
// Type: UnityEditor.InitializeOnLoadAttribute
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Allow an editor class to be initialized when Unity loads without action from the user.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Class)]
  public class InitializeOnLoadAttribute : Attribute
  {
  }
}
