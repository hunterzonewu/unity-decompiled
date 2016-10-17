// Decompiled with JetBrains decompiler
// Type: UnityEditor.AnimationWindowClipPopup
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class AnimationWindowClipPopup
  {
    [SerializeField]
    public AnimationWindowState state;
    [SerializeField]
    private int selectedIndex;

    public bool creatingNewClipAllowed
    {
      get
      {
        if ((UnityEngine.Object) this.state.activeRootGameObject != (UnityEngine.Object) null)
          return this.state.animationIsEditable;
        return false;
      }
    }

    public void OnGUI()
    {
      string[] clipMenuContent = this.GetClipMenuContent();
      EditorGUI.BeginChangeCheck();
      this.selectedIndex = EditorGUILayout.Popup(this.ClipToIndex(this.state.activeAnimationClip), clipMenuContent, EditorStyles.toolbarPopup, new GUILayoutOption[0]);
      if (!EditorGUI.EndChangeCheck())
        return;
      if (clipMenuContent[this.selectedIndex] == AnimationWindowStyles.createNewClip.text)
      {
        AnimationClip newClip = AnimationWindowUtility.CreateNewClip(this.state.activeRootGameObject.name);
        if (!(bool) ((UnityEngine.Object) newClip))
          return;
        AnimationWindowUtility.AddClipToAnimationPlayerComponent(this.state.activeAnimationPlayer, newClip);
        this.state.activeAnimationClip = newClip;
      }
      else
        this.state.activeAnimationClip = this.IndexToClip(this.selectedIndex);
    }

    private string[] GetClipMenuContent()
    {
      List<string> stringList = new List<string>();
      stringList.AddRange((IEnumerable<string>) this.GetClipNames());
      if (this.creatingNewClipAllowed)
      {
        stringList.Add(string.Empty);
        stringList.Add(AnimationWindowStyles.createNewClip.text);
      }
      return stringList.ToArray();
    }

    private string[] GetClipNames()
    {
      AnimationClip[] animationClipArray = new AnimationClip[0];
      if (this.state.clipOnlyMode)
        animationClipArray = new AnimationClip[1]
        {
          this.state.activeAnimationClip
        };
      else if ((UnityEngine.Object) this.state.activeRootGameObject != (UnityEngine.Object) null)
        animationClipArray = AnimationUtility.GetAnimationClips(this.state.activeRootGameObject);
      string[] strArray = new string[animationClipArray.Length];
      for (int index = 0; index < animationClipArray.Length; ++index)
        strArray[index] = CurveUtility.GetClipName(animationClipArray[index]);
      return strArray;
    }

    private AnimationClip IndexToClip(int index)
    {
      if ((UnityEngine.Object) this.state.activeRootGameObject != (UnityEngine.Object) null)
      {
        AnimationClip[] animationClips = AnimationUtility.GetAnimationClips(this.state.activeRootGameObject);
        if (index >= 0 && index < animationClips.Length)
          return AnimationUtility.GetAnimationClips(this.state.activeRootGameObject)[index];
      }
      return (AnimationClip) null;
    }

    private int ClipToIndex(AnimationClip clip)
    {
      if ((UnityEngine.Object) this.state.activeRootGameObject != (UnityEngine.Object) null)
      {
        int num = 0;
        foreach (AnimationClip animationClip in AnimationUtility.GetAnimationClips(this.state.activeRootGameObject))
        {
          if ((UnityEngine.Object) clip == (UnityEngine.Object) animationClip)
            return num;
          ++num;
        }
      }
      return 0;
    }
  }
}
