// Decompiled with JetBrains decompiler
// Type: UnityEditor.ManipulationTool
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal abstract class ManipulationTool
  {
    protected virtual void OnToolGUI(SceneView view)
    {
      if (!(bool) ((Object) Selection.activeTransform) || Tools.s_Hidden)
        return;
      bool flag = !Tools.s_Hidden && EditorApplication.isPlaying && GameObjectUtility.ContainsStatic(Selection.gameObjects);
      EditorGUI.BeginDisabledGroup(flag);
      Vector3 handlePosition = Tools.handlePosition;
      this.ToolGUI(view, handlePosition, flag);
      Handles.ShowStaticLabelIfNeeded(handlePosition);
      EditorGUI.EndDisabledGroup();
    }

    public abstract void ToolGUI(SceneView view, Vector3 handlePosition, bool isStatic);
  }
}
