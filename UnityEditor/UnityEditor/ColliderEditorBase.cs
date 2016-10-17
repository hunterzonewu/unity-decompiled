// Decompiled with JetBrains decompiler
// Type: UnityEditor.ColliderEditorBase
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class ColliderEditorBase : Editor
  {
    public bool editingCollider
    {
      get
      {
        if (UnityEditorInternal.EditMode.editMode == UnityEditorInternal.EditMode.SceneViewEditMode.Collider)
          return UnityEditorInternal.EditMode.IsOwner((Editor) this);
        return false;
      }
    }

    protected virtual void OnEditStart()
    {
    }

    protected virtual void OnEditEnd()
    {
    }

    public virtual void OnEnable()
    {
      UnityEditorInternal.EditMode.onEditModeStartDelegate += new UnityEditorInternal.EditMode.OnEditModeStartFunc(this.OnEditModeStart);
    }

    public virtual void OnDisable()
    {
      UnityEditorInternal.EditMode.onEditModeEndDelegate -= new UnityEditorInternal.EditMode.OnEditModeStopFunc(this.OnEditModeEnd);
    }

    protected void ForceQuitEditMode()
    {
      UnityEditorInternal.EditMode.QuitEditMode();
    }

    protected void InspectorEditButtonGUI()
    {
      UnityEditorInternal.EditMode.DoEditModeInspectorModeButton(UnityEditorInternal.EditMode.SceneViewEditMode.Collider, "Edit Collider", EditorGUIUtility.IconContent("EditCollider"), ColliderEditorBase.GetColliderBounds(this.target), (Editor) this);
    }

    private static Bounds GetColliderBounds(Object collider)
    {
      if (collider is Collider2D)
        return (collider as Collider2D).bounds;
      if (collider is Collider)
        return (collider as Collider).bounds;
      return new Bounds();
    }

    protected void OnEditModeStart(Editor editor, UnityEditorInternal.EditMode.SceneViewEditMode mode)
    {
      if (mode != UnityEditorInternal.EditMode.SceneViewEditMode.Collider || !((Object) editor == (Object) this))
        return;
      this.OnEditStart();
    }

    protected void OnEditModeEnd(Editor editor)
    {
      if (!((Object) editor == (Object) this))
        return;
      this.OnEditEnd();
    }
  }
}
