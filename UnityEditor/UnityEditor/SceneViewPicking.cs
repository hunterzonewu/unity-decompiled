// Decompiled with JetBrains decompiler
// Type: UnityEditor.SceneViewPicking
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace UnityEditor
{
  internal class SceneViewPicking
  {
    private static bool s_RetainHashes;
    private static int s_PreviousTopmostHash;
    private static int s_PreviousPrefixHash;

    static SceneViewPicking()
    {
      Selection.selectionChanged += new System.Action(SceneViewPicking.ResetHashes);
    }

    private static void ResetHashes()
    {
      if (!SceneViewPicking.s_RetainHashes)
      {
        SceneViewPicking.s_PreviousTopmostHash = 0;
        SceneViewPicking.s_PreviousPrefixHash = 0;
      }
      SceneViewPicking.s_RetainHashes = false;
    }

    public static GameObject PickGameObject(Vector2 mousePosition)
    {
      SceneViewPicking.s_RetainHashes = true;
      IEnumerator<GameObject> enumerator = SceneViewPicking.GetAllOverlapping(mousePosition).GetEnumerator();
      if (!enumerator.MoveNext())
        return (GameObject) null;
      GameObject current = enumerator.Current;
      GameObject selectionBase = HandleUtility.FindSelectionBase(current);
      GameObject gameObject = !((UnityEngine.Object) selectionBase == (UnityEngine.Object) null) ? selectionBase : current;
      int hashCode = current.GetHashCode();
      int hash = hashCode;
      if ((UnityEngine.Object) Selection.activeGameObject == (UnityEngine.Object) null || hashCode != SceneViewPicking.s_PreviousTopmostHash)
      {
        SceneViewPicking.s_PreviousTopmostHash = hashCode;
        SceneViewPicking.s_PreviousPrefixHash = hash;
        return gameObject;
      }
      SceneViewPicking.s_PreviousTopmostHash = hashCode;
      if ((UnityEngine.Object) selectionBase != (UnityEngine.Object) null && (UnityEngine.Object) Selection.activeGameObject == (UnityEngine.Object) selectionBase)
      {
        if (hash == SceneViewPicking.s_PreviousPrefixHash)
          return current;
        SceneViewPicking.s_PreviousPrefixHash = hash;
        return selectionBase;
      }
      if ((UnityEngine.Object) HandleUtility.PickGameObject(mousePosition, 0 != 0, (GameObject[]) null, new GameObject[1]{ Selection.activeGameObject }) == (UnityEngine.Object) Selection.activeGameObject)
      {
        while ((UnityEngine.Object) enumerator.Current != (UnityEngine.Object) Selection.activeGameObject)
        {
          if (!enumerator.MoveNext())
          {
            SceneViewPicking.s_PreviousPrefixHash = hashCode;
            return gameObject;
          }
          SceneViewPicking.UpdateHash(ref hash, (object) enumerator.Current);
        }
      }
      if (hash != SceneViewPicking.s_PreviousPrefixHash)
      {
        SceneViewPicking.s_PreviousPrefixHash = hashCode;
        return gameObject;
      }
      if (!enumerator.MoveNext())
      {
        SceneViewPicking.s_PreviousPrefixHash = hashCode;
        return gameObject;
      }
      SceneViewPicking.UpdateHash(ref hash, (object) enumerator.Current);
      if ((UnityEngine.Object) enumerator.Current == (UnityEngine.Object) selectionBase)
      {
        if (!enumerator.MoveNext())
        {
          SceneViewPicking.s_PreviousPrefixHash = hashCode;
          return gameObject;
        }
        SceneViewPicking.UpdateHash(ref hash, (object) enumerator.Current);
      }
      SceneViewPicking.s_PreviousPrefixHash = hash;
      return enumerator.Current;
    }

    public static GameObject GetHovered(Vector2 screenPosition, GameObject[] gameObjects)
    {
      return HandleUtility.PickGameObject(screenPosition, false, (GameObject[]) null, gameObjects);
    }

    [DebuggerHidden]
    private static IEnumerable<GameObject> GetAllOverlapping(Vector2 position)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SceneViewPicking.\u003CGetAllOverlapping\u003Ec__Iterator8 overlappingCIterator8 = new SceneViewPicking.\u003CGetAllOverlapping\u003Ec__Iterator8() { position = position, \u003C\u0024\u003Eposition = position };
      // ISSUE: reference to a compiler-generated field
      overlappingCIterator8.\u0024PC = -2;
      return (IEnumerable<GameObject>) overlappingCIterator8;
    }

    private static void UpdateHash(ref int hash, object obj)
    {
      hash = hash * 33 + obj.GetHashCode();
    }
  }
}
