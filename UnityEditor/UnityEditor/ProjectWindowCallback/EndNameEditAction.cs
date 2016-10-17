// Decompiled with JetBrains decompiler
// Type: UnityEditor.ProjectWindowCallback.EndNameEditAction
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.ProjectWindowCallback
{
  public abstract class EndNameEditAction : ScriptableObject
  {
    public virtual void OnEnable()
    {
      this.hideFlags = HideFlags.HideAndDontSave;
    }

    public abstract void Action(int instanceId, string pathName, string resourceFile);

    public virtual void CleanUp()
    {
      Object.DestroyImmediate((Object) this);
    }
  }
}
