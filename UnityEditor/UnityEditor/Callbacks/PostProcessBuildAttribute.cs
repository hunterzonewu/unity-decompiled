// Decompiled with JetBrains decompiler
// Type: UnityEditor.Callbacks.PostProcessBuildAttribute
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine.Scripting;

namespace UnityEditor.Callbacks
{
  /// <summary>
  ///   <para>Add this attribute to a method to get a notification just after building the player.</para>
  /// </summary>
  [RequiredByNativeCode]
  public sealed class PostProcessBuildAttribute : CallbackOrderAttribute
  {
    public PostProcessBuildAttribute()
    {
      this.m_CallbackOrder = 1;
    }

    public PostProcessBuildAttribute(int callbackOrder)
    {
      this.m_CallbackOrder = callbackOrder;
    }
  }
}
