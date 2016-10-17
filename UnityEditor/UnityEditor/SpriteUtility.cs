// Decompiled with JetBrains decompiler
// Type: UnityEditor.SpriteUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal static class SpriteUtility
  {
    private static List<UnityEngine.Object> s_SceneDragObjects;
    private static SpriteUtility.DragType s_DragType;

    public static void OnSceneDrag(SceneView sceneView)
    {
      Event current1 = Event.current;
      if (current1.type != EventType.DragUpdated && current1.type != EventType.DragPerform && current1.type != EventType.DragExited)
        return;
      if (!sceneView.in2DMode)
      {
        GameObject gameObject = HandleUtility.PickGameObject(Event.current.mousePosition, true);
        if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null && DragAndDrop.objectReferences.Length == 1 && ((UnityEngine.Object) (DragAndDrop.objectReferences[0] as Texture) != (UnityEngine.Object) null && (UnityEngine.Object) gameObject.GetComponent<Renderer>() != (UnityEngine.Object) null))
          return;
      }
      switch (current1.type)
      {
        case EventType.DragUpdated:
          SpriteUtility.DragType dragType = !current1.alt ? SpriteUtility.DragType.SpriteAnimation : SpriteUtility.DragType.CreateMultiple;
          if (SpriteUtility.s_DragType != dragType || SpriteUtility.s_SceneDragObjects == null)
          {
            Sprite[] draggedPathsOrObjects = SpriteUtility.GetSpriteFromDraggedPathsOrObjects();
            if (draggedPathsOrObjects == null || draggedPathsOrObjects.Length == 0 || (UnityEngine.Object) draggedPathsOrObjects[0] == (UnityEngine.Object) null)
              break;
            if (SpriteUtility.s_DragType != SpriteUtility.DragType.NotInitialized)
              SpriteUtility.CleanUp();
            SpriteUtility.s_DragType = dragType;
            SpriteUtility.s_SceneDragObjects = new List<UnityEngine.Object>();
            if (SpriteUtility.s_DragType == SpriteUtility.DragType.CreateMultiple)
            {
              foreach (Sprite frame in draggedPathsOrObjects)
                SpriteUtility.s_SceneDragObjects.Add((UnityEngine.Object) SpriteUtility.CreateDragGO(frame, Vector3.zero));
            }
            else
              SpriteUtility.s_SceneDragObjects.Add((UnityEngine.Object) SpriteUtility.CreateDragGO(draggedPathsOrObjects[0], Vector3.zero));
            List<Transform> transformList = new List<Transform>();
            using (List<UnityEngine.Object>.Enumerator enumerator = SpriteUtility.s_SceneDragObjects.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                GameObject current2 = (GameObject) enumerator.Current;
                transformList.AddRange((IEnumerable<Transform>) current2.GetComponentsInChildren<Transform>());
                current2.hideFlags = HideFlags.HideInHierarchy;
              }
            }
            HandleUtility.ignoreRaySnapObjects = transformList.ToArray();
          }
          Vector3 zero = Vector3.zero;
          Vector3 point = HandleUtility.GUIPointToWorldRay(current1.mousePosition).GetPoint(10f);
          if (sceneView.in2DMode)
          {
            point.z = 0.0f;
          }
          else
          {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            object obj = HandleUtility.RaySnap(HandleUtility.GUIPointToWorldRay(current1.mousePosition));
            if (obj != null)
              point = ((RaycastHit) obj).point;
          }
          using (List<UnityEngine.Object>.Enumerator enumerator = SpriteUtility.s_SceneDragObjects.GetEnumerator())
          {
            while (enumerator.MoveNext())
              ((GameObject) enumerator.Current).transform.position = point;
          }
          DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
          current1.Use();
          break;
        case EventType.DragPerform:
          Sprite[] draggedPathsOrObjects1 = SpriteUtility.GetSpriteFromDraggedPathsOrObjects();
          if (draggedPathsOrObjects1 == null || SpriteUtility.s_SceneDragObjects == null)
            break;
          if (SpriteUtility.s_DragType == SpriteUtility.DragType.SpriteAnimation)
            SpriteUtility.AddAnimationToGO((GameObject) SpriteUtility.s_SceneDragObjects[0], draggedPathsOrObjects1);
          using (List<UnityEngine.Object>.Enumerator enumerator = SpriteUtility.s_SceneDragObjects.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              GameObject current2 = (GameObject) enumerator.Current;
              Undo.RegisterCreatedObjectUndo((UnityEngine.Object) current2, "Create Sprite");
              current2.hideFlags = HideFlags.None;
            }
          }
          Selection.objects = SpriteUtility.s_SceneDragObjects.ToArray();
          SpriteUtility.CleanUp();
          current1.Use();
          break;
        case EventType.DragExited:
          if (SpriteUtility.s_SceneDragObjects == null || SpriteUtility.s_SceneDragObjects == null)
            break;
          using (List<UnityEngine.Object>.Enumerator enumerator = SpriteUtility.s_SceneDragObjects.GetEnumerator())
          {
            while (enumerator.MoveNext())
              UnityEngine.Object.DestroyImmediate(enumerator.Current, false);
          }
          SpriteUtility.CleanUp();
          current1.Use();
          break;
      }
    }

    private static void CleanUp()
    {
      if (SpriteUtility.s_SceneDragObjects != null)
      {
        SpriteUtility.s_SceneDragObjects.Clear();
        SpriteUtility.s_SceneDragObjects = (List<UnityEngine.Object>) null;
      }
      HandleUtility.ignoreRaySnapObjects = (Transform[]) null;
      SpriteUtility.s_DragType = SpriteUtility.DragType.NotInitialized;
    }

    private static bool CreateAnimation(GameObject gameObject, UnityEngine.Object[] frames)
    {
      Array.Sort<UnityEngine.Object>(frames, (Comparison<UnityEngine.Object>) ((a, b) => EditorUtility.NaturalCompare(a.name, b.name)));
      if (!(bool) ((UnityEngine.Object) AnimationWindowUtility.EnsureActiveAnimationPlayer(gameObject)))
        return false;
      Animator animatorInParents = AnimationWindowUtility.GetClosestAnimatorInParents(gameObject.transform);
      if ((UnityEngine.Object) animatorInParents == (UnityEngine.Object) null)
        return false;
      AnimationClip newClip = AnimationWindowUtility.CreateNewClip(gameObject.name);
      if ((UnityEngine.Object) newClip == (UnityEngine.Object) null)
        return false;
      SpriteUtility.AddSpriteAnimationToClip(newClip, frames);
      return AnimationWindowUtility.AddClipToAnimatorComponent(animatorInParents, newClip);
    }

    private static void AddSpriteAnimationToClip(AnimationClip newClip, UnityEngine.Object[] frames)
    {
      newClip.frameRate = 12f;
      ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[frames.Length];
      for (int index = 0; index < keyframes.Length; ++index)
      {
        keyframes[index] = new ObjectReferenceKeyframe();
        keyframes[index].value = (UnityEngine.Object) SpriteUtility.RemapObjectToSprite(frames[index]);
        keyframes[index].time = (float) index / newClip.frameRate;
      }
      EditorCurveBinding binding = EditorCurveBinding.PPtrCurve(string.Empty, typeof (SpriteRenderer), "m_Sprite");
      AnimationUtility.SetObjectReferenceCurve(newClip, binding, keyframes);
    }

    public static Sprite[] GetSpriteFromDraggedPathsOrObjects()
    {
      List<Sprite> spriteList = new List<Sprite>();
      foreach (UnityEngine.Object objectReference in DragAndDrop.objectReferences)
      {
        if (AssetDatabase.Contains(objectReference))
        {
          if (objectReference is Sprite)
            spriteList.Add(objectReference as Sprite);
          else if (objectReference is Texture2D)
            spriteList.AddRange((IEnumerable<Sprite>) SpriteUtility.TextureToSprites(objectReference as Texture2D));
        }
      }
      if (spriteList.Count > 0)
        return spriteList.ToArray();
      Sprite sprite = SpriteUtility.HandleExternalDrag(Event.current.type == EventType.DragPerform);
      if (!((UnityEngine.Object) sprite != (UnityEngine.Object) null))
        return (Sprite[]) null;
      return new Sprite[1]{ sprite };
    }

    public static Sprite[] GetSpritesFromDraggedObjects()
    {
      List<Sprite> spriteList = new List<Sprite>();
      foreach (UnityEngine.Object objectReference in DragAndDrop.objectReferences)
      {
        if (objectReference.GetType() == typeof (Sprite))
          spriteList.Add(objectReference as Sprite);
        else if (objectReference.GetType() == typeof (Texture2D))
          spriteList.AddRange((IEnumerable<Sprite>) SpriteUtility.TextureToSprites(objectReference as Texture2D));
      }
      return spriteList.ToArray();
    }

    private static Sprite HandleExternalDrag(bool perform)
    {
      if (DragAndDrop.paths.Length == 0)
        return (Sprite) null;
      string path = DragAndDrop.paths[0];
      if (!SpriteUtility.ValidPathForTextureAsset(path))
        return (Sprite) null;
      DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
      if (!perform)
        return (Sprite) null;
      string uniqueAssetPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine("Assets", FileUtil.GetLastPathNameComponent(path)));
      if (uniqueAssetPath.Length <= 0)
        return (Sprite) null;
      FileUtil.CopyFileOrDirectory(path, uniqueAssetPath);
      SpriteUtility.ForcedImportFor(uniqueAssetPath);
      return SpriteUtility.GenerateDefaultSprite(AssetDatabase.LoadMainAssetAtPath(uniqueAssetPath) as Texture2D);
    }

    private static void ForcedImportFor(string newPath)
    {
      try
      {
        AssetDatabase.StartAssetEditing();
        AssetDatabase.ImportAsset(newPath);
      }
      finally
      {
        AssetDatabase.StopAssetEditing();
      }
    }

    private static Sprite GenerateDefaultSprite(Texture2D texture)
    {
      string assetPath = AssetDatabase.GetAssetPath((UnityEngine.Object) texture);
      TextureImporter atPath = AssetImporter.GetAtPath(assetPath) as TextureImporter;
      if ((UnityEngine.Object) atPath == (UnityEngine.Object) null)
        return (Sprite) null;
      if (atPath.textureType != TextureImporterType.Sprite && atPath.textureType != TextureImporterType.Advanced)
        return (Sprite) null;
      if (atPath.spriteImportMode == SpriteImportMode.None)
      {
        if (atPath.textureType == TextureImporterType.Advanced)
          return (Sprite) null;
        atPath.spriteImportMode = SpriteImportMode.Single;
        AssetDatabase.WriteImportSettingsIfDirty(assetPath);
        SpriteUtility.ForcedImportFor(assetPath);
      }
      UnityEngine.Object @object = (UnityEngine.Object) null;
      try
      {
        @object = ((IEnumerable<UnityEngine.Object>) AssetDatabase.LoadAllAssetsAtPath(assetPath)).First<UnityEngine.Object>((Func<UnityEngine.Object, bool>) (t => t is Sprite));
      }
      catch (Exception ex)
      {
        Debug.LogWarning((object) "Texture being dragged has no Sprites.");
      }
      return @object as Sprite;
    }

    public static GameObject CreateDragGO(Sprite frame, Vector3 position)
    {
      GameObject gameObject = new GameObject(GameObjectUtility.GetUniqueNameForSibling((Transform) null, !string.IsNullOrEmpty(frame.name) ? frame.name : "Sprite"));
      gameObject.AddComponent<SpriteRenderer>().sprite = frame;
      gameObject.transform.position = position;
      return gameObject;
    }

    public static void AddAnimationToGO(GameObject go, Sprite[] frames)
    {
      if (frames == null || frames.Length <= 0)
        return;
      SpriteRenderer spriteRenderer = go.GetComponent<SpriteRenderer>();
      if ((UnityEngine.Object) spriteRenderer == (UnityEngine.Object) null)
      {
        Debug.LogWarning((object) "There should be a SpriteRenderer in dragged object");
        spriteRenderer = go.AddComponent<SpriteRenderer>();
      }
      spriteRenderer.sprite = frames[0];
      if (frames.Length > 1)
      {
        Analytics.Event("Sprite Drag and Drop", "Drop multiple sprites to scene", "null", 1);
        if (SpriteUtility.CreateAnimation(go, (UnityEngine.Object[]) frames))
          return;
        Debug.LogError((object) "Failed to create animation for dragged object");
      }
      else
        Analytics.Event("Sprite Drag and Drop", "Drop single sprite to scene", "null", 1);
    }

    public static GameObject DropSpriteToSceneToCreateGO(Sprite sprite, Vector3 position)
    {
      GameObject gameObject = new GameObject(!string.IsNullOrEmpty(sprite.name) ? sprite.name : "Sprite");
      gameObject.AddComponent<SpriteRenderer>().sprite = sprite;
      gameObject.transform.position = position;
      Selection.activeObject = (UnityEngine.Object) gameObject;
      return gameObject;
    }

    public static Sprite RemapObjectToSprite(UnityEngine.Object obj)
    {
      if (obj is Sprite)
        return (Sprite) obj;
      if (obj is Texture2D)
      {
        UnityEngine.Object[] objectArray = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(obj));
        for (int index = 0; index < objectArray.Length; ++index)
        {
          if (objectArray[index].GetType() == typeof (Sprite))
            return objectArray[index] as Sprite;
        }
      }
      return (Sprite) null;
    }

    public static Sprite[] TextureToSprites(Texture2D tex)
    {
      UnityEngine.Object[] objectArray = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath((UnityEngine.Object) tex));
      List<Sprite> spriteList = new List<Sprite>();
      for (int index = 0; index < objectArray.Length; ++index)
      {
        if (objectArray[index].GetType() == typeof (Sprite))
          spriteList.Add(objectArray[index] as Sprite);
      }
      if (spriteList.Count > 0)
        return spriteList.ToArray();
      return new Sprite[1]{ SpriteUtility.GenerateDefaultSprite(tex) };
    }

    public static Sprite TextureToSprite(Texture2D tex)
    {
      Sprite[] sprites = SpriteUtility.TextureToSprites(tex);
      if (sprites.Length > 0)
        return sprites[0];
      return (Sprite) null;
    }

    private static bool ValidPathForTextureAsset(string path)
    {
      string lower = FileUtil.GetPathExtension(path).ToLower();
      if (!(lower == "jpg") && !(lower == "jpeg") && (!(lower == "tif") && !(lower == "tiff")) && (!(lower == "tga") && !(lower == "gif") && (!(lower == "png") && !(lower == "psd"))) && (!(lower == "bmp") && !(lower == "iff") && (!(lower == "pict") && !(lower == "pic")) && (!(lower == "pct") && !(lower == "exr"))))
        return lower == "hdr";
      return true;
    }

    private enum DragType
    {
      NotInitialized,
      SpriteAnimation,
      CreateMultiple,
    }
  }
}
