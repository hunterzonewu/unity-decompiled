// Decompiled with JetBrains decompiler
// Type: UnityEditor.AnimationWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  [EditorWindowTitle(title = "Animation", useTypeNameAsIconName = true)]
  internal class AnimationWindow : EditorWindow
  {
    private static List<AnimationWindow> s_AnimationWindows = new List<AnimationWindow>();
    [SerializeField]
    private AnimEditor m_AnimEditor;
    private GUIStyle m_LockButtonStyle;

    public static List<AnimationWindow> GetAllAnimationWindows()
    {
      return AnimationWindow.s_AnimationWindows;
    }

    public void OnEnable()
    {
      if ((Object) this.m_AnimEditor == (Object) null)
      {
        this.m_AnimEditor = ScriptableObject.CreateInstance(typeof (AnimEditor)) as AnimEditor;
        this.m_AnimEditor.hideFlags = HideFlags.HideAndDontSave;
      }
      AnimationWindow.s_AnimationWindows.Add(this);
      this.titleContent = this.GetLocalizedTitleContent();
    }

    public void OnDisable()
    {
      AnimationWindow.s_AnimationWindows.Remove(this);
      this.m_AnimEditor.OnDisable();
    }

    public void Update()
    {
      this.m_AnimEditor.Update();
    }

    public void OnGUI()
    {
      this.m_AnimEditor.OnBreadcrumbGUI((EditorWindow) this, this.position);
    }

    public void OnSelectionChange()
    {
      this.m_AnimEditor.OnSelectionChange();
    }

    protected virtual void ShowButton(Rect r)
    {
      if (this.m_LockButtonStyle == null)
        this.m_LockButtonStyle = (GUIStyle) "IN LockButton";
      EditorGUI.BeginDisabledGroup(this.m_AnimEditor.stateDisabled);
      this.m_AnimEditor.locked = GUI.Toggle(r, this.m_AnimEditor.locked, GUIContent.none, this.m_LockButtonStyle);
      EditorGUI.EndDisabledGroup();
    }
  }
}
