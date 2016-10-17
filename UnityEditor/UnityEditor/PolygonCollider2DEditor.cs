// Decompiled with JetBrains decompiler
// Type: UnityEditor.PolygonCollider2DEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (PolygonCollider2D))]
  internal class PolygonCollider2DEditor : Collider2DEditorBase
  {
    private readonly PolygonEditorUtility m_PolyUtility = new PolygonEditorUtility();
    private bool m_ShowColliderInfo;

    public override void OnEnable()
    {
      base.OnEnable();
    }

    public override void OnInspectorGUI()
    {
      EditorGUILayout.BeginVertical();
      this.BeginColliderInspector();
      base.OnInspectorGUI();
      this.EndColliderInspector();
      EditorGUILayout.EndVertical();
      this.HandleDragAndDrop(GUILayoutUtility.GetLastRect());
    }

    private void HandleDragAndDrop(Rect targetRect)
    {
      if (Event.current.type != EventType.DragPerform && Event.current.type != EventType.DragUpdated || !targetRect.Contains(Event.current.mousePosition))
        return;
      using (IEnumerator<UnityEngine.Object> enumerator1 = ((IEnumerable<UnityEngine.Object>) DragAndDrop.objectReferences).Where<UnityEngine.Object>((Func<UnityEngine.Object, bool>) (obj =>
      {
        if (!(obj is Sprite))
          return obj is Texture2D;
        return true;
      })).GetEnumerator())
      {
        if (enumerator1.MoveNext())
        {
          UnityEngine.Object current1 = enumerator1.Current;
          DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
          if (Event.current.type != EventType.DragPerform)
            return;
          Sprite sprite = !(current1 is Sprite) ? SpriteUtility.TextureToSprite(current1 as Texture2D) : current1 as Sprite;
          using (IEnumerator<PolygonCollider2D> enumerator2 = ((IEnumerable<UnityEngine.Object>) this.targets).Select<UnityEngine.Object, PolygonCollider2D>((Func<UnityEngine.Object, PolygonCollider2D>) (target => target as PolygonCollider2D)).GetEnumerator())
          {
            while (enumerator2.MoveNext())
            {
              PolygonCollider2D current2 = enumerator2.Current;
              Vector2[][] paths;
              UnityEditor.Sprites.SpriteUtility.GenerateOutlineFromSprite(sprite, 0.25f, (byte) 200, true, out paths);
              current2.pathCount = paths.Length;
              for (int index = 0; index < paths.Length; ++index)
                current2.SetPath(index, paths[index]);
              this.m_PolyUtility.StopEditing();
              DragAndDrop.AcceptDrag();
            }
            return;
          }
        }
      }
      DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
    }

    protected override void OnEditStart()
    {
      if (this.target == (UnityEngine.Object) null)
        return;
      this.m_PolyUtility.StartEditing(this.target as Collider2D);
    }

    protected override void OnEditEnd()
    {
      this.m_PolyUtility.StopEditing();
    }

    public void OnSceneGUI()
    {
      if (!this.editingCollider)
        return;
      this.m_PolyUtility.OnSceneGUI();
    }
  }
}
