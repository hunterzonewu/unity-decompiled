// Decompiled with JetBrains decompiler
// Type: UnityEditor.PostProcessAttribute
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine.Scripting;

namespace UnityEditor
{
  [RequiredByNativeCode]
  [Obsolete("PostProcessAttribute has been renamed to CallbackOrderAttribute.")]
  public abstract class PostProcessAttribute : CallbackOrderAttribute
  {
    [Obsolete("PostProcessAttribute has been renamed. Use m_CallbackOrder of CallbackOrderAttribute.")]
    protected int m_PostprocessOrder;

    [Obsolete("PostProcessAttribute has been renamed. Use callbackOrder of CallbackOrderAttribute.")]
    internal int GetPostprocessOrder
    {
      get
      {
        return this.m_PostprocessOrder;
      }
    }
  }
}
