// Decompiled with JetBrains decompiler
// Type: UnityEngine.Scripting.PreserveAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine.Scripting
{
  /// <summary>
  ///   <para>PreserveAttribute prevents byte code stripping from removing a class, method, field, or property.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
  public class PreserveAttribute : Attribute
  {
  }
}
