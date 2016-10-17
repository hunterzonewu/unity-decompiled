// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.State
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditorInternal
{
  [Obsolete("State is obsolete. Use UnityEditor.Animations.AnimatorState instead (UnityUpgradable) -> UnityEditor.Animations.AnimatorState", true)]
  public class State : UnityEngine.Object
  {
    public string uniqueName
    {
      get
      {
        return string.Empty;
      }
    }

    public int uniqueNameHash
    {
      get
      {
        return -1;
      }
    }

    public float speed
    {
      get
      {
        return -1f;
      }
      set
      {
      }
    }

    public bool mirror
    {
      get
      {
        return false;
      }
      set
      {
      }
    }

    public bool iKOnFeet
    {
      get
      {
        return false;
      }
      set
      {
      }
    }

    public string tag
    {
      get
      {
        return string.Empty;
      }
      set
      {
      }
    }

    public Motion GetMotion()
    {
      return (Motion) null;
    }

    public Motion GetMotion(AnimatorControllerLayer layer)
    {
      return (Motion) null;
    }

    public BlendTree CreateBlendTree()
    {
      return (BlendTree) null;
    }

    public BlendTree CreateBlendTree(AnimatorControllerLayer layer)
    {
      return (BlendTree) null;
    }
  }
}
