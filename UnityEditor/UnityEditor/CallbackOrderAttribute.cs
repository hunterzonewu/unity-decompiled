// Decompiled with JetBrains decompiler
// Type: UnityEditor.CallbackOrderAttribute
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Base class for Attributes that require a callback index.</para>
  /// </summary>
  [RequiredByNativeCode]
  public abstract class CallbackOrderAttribute : Attribute
  {
    protected int m_CallbackOrder;

    internal int callbackOrder
    {
      get
      {
        return this.m_CallbackOrder;
      }
    }
  }
}
