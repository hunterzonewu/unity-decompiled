// Decompiled with JetBrains decompiler
// Type: UnityEditor.Callbacks.PostProcessSceneAttribute
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine.Scripting;

namespace UnityEditor.Callbacks
{
  /// <summary>
  ///   <para>Add this attribute to get a callback just before building the scene.</para>
  /// </summary>
  [RequiredByNativeCode]
  public sealed class PostProcessSceneAttribute : CallbackOrderAttribute
  {
    private int m_version;

    internal int version
    {
      get
      {
        return this.m_version;
      }
    }

    public PostProcessSceneAttribute()
    {
      this.m_CallbackOrder = 1;
      this.m_version = 0;
    }

    public PostProcessSceneAttribute(int callbackOrder)
    {
      this.m_CallbackOrder = callbackOrder;
      this.m_version = 0;
    }

    public PostProcessSceneAttribute(int callbackOrder, int version)
    {
      this.m_CallbackOrder = callbackOrder;
      this.m_version = version;
    }
  }
}
