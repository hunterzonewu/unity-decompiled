// Decompiled with JetBrains decompiler
// Type: UnityEditor.ProjectWindowUtil
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor.ProjectWindowCallback;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  public class ProjectWindowUtil
  {
    internal static int k_FavoritesStartInstanceID = 1000000000;
    internal static string k_DraggingFavoriteGenericData = "DraggingFavorite";
    internal static string k_IsFolderGenericData = "IsFolder";

    [MenuItem("Assets/Create/GUI Skin", false, 601)]
    public static void CreateNewGUISkin()
    {
      GUISkin instance = ScriptableObject.CreateInstance<GUISkin>();
      GUISkin builtinResource = Resources.GetBuiltinResource(typeof (GUISkin), "GameSkin/GameSkin.guiskin") as GUISkin;
      if ((bool) ((UnityEngine.Object) builtinResource))
        EditorUtility.CopySerialized((UnityEngine.Object) builtinResource, (UnityEngine.Object) instance);
      else
        Debug.LogError((object) "Internal error: unable to load builtin GUIskin");
      ProjectWindowUtil.CreateAsset((UnityEngine.Object) instance, "New GUISkin.guiskin");
    }

    internal static string GetActiveFolderPath()
    {
      ProjectBrowser projectBrowserIfExists = ProjectWindowUtil.GetProjectBrowserIfExists();
      if ((UnityEngine.Object) projectBrowserIfExists == (UnityEngine.Object) null)
        return "Assets";
      return projectBrowserIfExists.GetActiveFolderPath();
    }

    internal static void EndNameEditAction(EndNameEditAction action, int instanceId, string pathName, string resourceFile)
    {
      pathName = AssetDatabase.GenerateUniqueAssetPath(pathName);
      if (!((UnityEngine.Object) action != (UnityEngine.Object) null))
        return;
      action.Action(instanceId, pathName, resourceFile);
      action.CleanUp();
    }

    public static void CreateAsset(UnityEngine.Object asset, string pathName)
    {
      ProjectWindowUtil.StartNameEditingIfProjectWindowExists(asset.GetInstanceID(), (EndNameEditAction) ScriptableObject.CreateInstance<DoCreateNewAsset>(), pathName, AssetPreview.GetMiniThumbnail(asset), (string) null);
    }

    public static void CreateFolder()
    {
      ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, (EndNameEditAction) ScriptableObject.CreateInstance<DoCreateFolder>(), "New Folder", EditorGUIUtility.IconContent(EditorResourcesUtility.emptyFolderIconName).image as Texture2D, (string) null);
    }

    public static void CreateScene()
    {
      ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, (EndNameEditAction) ScriptableObject.CreateInstance<DoCreateScene>(), "New Scene.unity", EditorGUIUtility.FindTexture("SceneAsset Icon"), (string) null);
    }

    public static void CreatePrefab()
    {
      ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, (EndNameEditAction) ScriptableObject.CreateInstance<DoCreatePrefab>(), "New Prefab.prefab", EditorGUIUtility.IconContent("Prefab Icon").image as Texture2D, (string) null);
    }

    private static void CreateScriptAsset(string templatePath, string destName)
    {
      if (Path.GetFileName(templatePath).ToLower().Contains("test"))
      {
        string str1 = AssetDatabase.GetUniquePathNameAtSelectedPath(destName);
        if (!str1.ToLower().Contains("/editor/"))
        {
          string str2 = str1.Substring(0, str1.Length - destName.Length - 1);
          string str3 = Path.Combine(str2, "Editor");
          if (!Directory.Exists(str3))
            AssetDatabase.CreateFolder(str2, "Editor");
          str1 = Path.Combine(str3, destName).Replace("\\", "/");
        }
        destName = str1;
      }
      string extension = Path.GetExtension(destName);
      Texture2D image;
      if (extension != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (ProjectWindowUtil.\u003C\u003Ef__switch\u0024map1F == null)
        {
          // ISSUE: reference to a compiler-generated field
          ProjectWindowUtil.\u003C\u003Ef__switch\u0024map1F = new Dictionary<string, int>(4)
          {
            {
              ".js",
              0
            },
            {
              ".cs",
              1
            },
            {
              ".boo",
              2
            },
            {
              ".shader",
              3
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (ProjectWindowUtil.\u003C\u003Ef__switch\u0024map1F.TryGetValue(extension, out num))
        {
          switch (num)
          {
            case 0:
              image = EditorGUIUtility.IconContent("js Script Icon").image as Texture2D;
              goto label_16;
            case 1:
              image = EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D;
              goto label_16;
            case 2:
              image = EditorGUIUtility.IconContent("boo Script Icon").image as Texture2D;
              goto label_16;
            case 3:
              image = EditorGUIUtility.IconContent("Shader Icon").image as Texture2D;
              goto label_16;
          }
        }
      }
      image = EditorGUIUtility.IconContent("TextAsset Icon").image as Texture2D;
label_16:
      ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, (EndNameEditAction) ScriptableObject.CreateInstance<DoCreateScriptAsset>(), destName, image, templatePath);
    }

    public static void ShowCreatedAsset(UnityEngine.Object o)
    {
      Selection.activeObject = o;
      if (!(bool) o)
        return;
      ProjectWindowUtil.FrameObjectInProjectWindow(o.GetInstanceID());
    }

    private static void CreateAnimatorController()
    {
      ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, (EndNameEditAction) ScriptableObject.CreateInstance<DoCreateAnimatorController>(), "New Animator Controller.controller", EditorGUIUtility.IconContent("AnimatorController Icon").image as Texture2D, (string) null);
    }

    private static void CreateAudioMixer()
    {
      ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, (EndNameEditAction) ScriptableObject.CreateInstance<DoCreateAudioMixer>(), "NewAudioMixer.mixer", EditorGUIUtility.IconContent("AudioMixerController Icon").image as Texture2D, (string) null);
    }

    private static void CreateSpritePolygon(int sides)
    {
      string empty = string.Empty;
      int num = sides;
      string str;
      switch (num)
      {
        case 0:
          str = "Square";
          break;
        case 3:
          str = "Triangle";
          break;
        case 4:
          str = "Diamond";
          break;
        case 6:
          str = "Hexagon";
          break;
        default:
          str = num == 42 ? "Everythingon" : (num == 128 ? "Circle" : "Polygon");
          break;
      }
      Texture2D image = EditorGUIUtility.IconContent("Sprite Icon").image as Texture2D;
      DoCreateSpritePolygon instance = ScriptableObject.CreateInstance<DoCreateSpritePolygon>();
      instance.sides = sides;
      ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, (EndNameEditAction) instance, str + ".png", image, (string) null);
    }

    internal static UnityEngine.Object CreateScriptAssetFromTemplate(string pathName, string resourceFile)
    {
      string fullPath = Path.GetFullPath(pathName);
      StreamReader streamReader = new StreamReader(resourceFile);
      string end = streamReader.ReadToEnd();
      streamReader.Close();
      string withoutExtension = Path.GetFileNameWithoutExtension(pathName);
      string input1 = Regex.Replace(end, "#NAME#", withoutExtension);
      string str1 = Regex.Replace(withoutExtension, " ", string.Empty);
      string input2 = Regex.Replace(input1, "#SCRIPTNAME#", str1);
      string str2;
      if (char.IsUpper(str1, 0))
      {
        string replacement = ((int) char.ToLower(str1[0])).ToString() + str1.Substring(1);
        str2 = Regex.Replace(input2, "#SCRIPTNAME_LOWER#", replacement);
      }
      else
      {
        string replacement = "my" + (object) char.ToUpper(str1[0]) + str1.Substring(1);
        str2 = Regex.Replace(input2, "#SCRIPTNAME_LOWER#", replacement);
      }
      UTF8Encoding utF8Encoding = new UTF8Encoding(true, false);
      bool append = false;
      StreamWriter streamWriter = new StreamWriter(fullPath, append, (Encoding) utF8Encoding);
      streamWriter.Write(str2);
      streamWriter.Close();
      AssetDatabase.ImportAsset(pathName);
      return AssetDatabase.LoadAssetAtPath(pathName, typeof (UnityEngine.Object));
    }

    public static void StartNameEditingIfProjectWindowExists(int instanceID, EndNameEditAction endAction, string pathName, Texture2D icon, string resourceFile)
    {
      ProjectBrowser projectBrowserIfExists = ProjectWindowUtil.GetProjectBrowserIfExists();
      if ((bool) ((UnityEngine.Object) projectBrowserIfExists))
      {
        projectBrowserIfExists.Focus();
        projectBrowserIfExists.BeginPreimportedNameEditing(instanceID, endAction, pathName, icon, resourceFile);
        projectBrowserIfExists.Repaint();
      }
      else
      {
        if (!pathName.StartsWith("assets/", StringComparison.CurrentCultureIgnoreCase))
          pathName = "Assets/" + pathName;
        ProjectWindowUtil.EndNameEditAction(endAction, instanceID, pathName, resourceFile);
        Selection.activeObject = EditorUtility.InstanceIDToObject(instanceID);
      }
    }

    private static ProjectBrowser GetProjectBrowserIfExists()
    {
      return ProjectBrowser.s_LastInteractedProjectBrowser;
    }

    internal static void FrameObjectInProjectWindow(int instanceID)
    {
      ProjectBrowser projectBrowserIfExists = ProjectWindowUtil.GetProjectBrowserIfExists();
      if (!(bool) ((UnityEngine.Object) projectBrowserIfExists))
        return;
      projectBrowserIfExists.FrameObject(instanceID, false);
    }

    internal static bool IsFavoritesItem(int instanceID)
    {
      return instanceID >= ProjectWindowUtil.k_FavoritesStartInstanceID;
    }

    internal static void StartDrag(int draggedInstanceID, List<int> selectedInstanceIDs)
    {
      DragAndDrop.PrepareStartDrag();
      string title = string.Empty;
      if (ProjectWindowUtil.IsFavoritesItem(draggedInstanceID))
      {
        DragAndDrop.SetGenericData(ProjectWindowUtil.k_DraggingFavoriteGenericData, (object) draggedInstanceID);
        DragAndDrop.objectReferences = new UnityEngine.Object[0];
      }
      else
      {
        bool flag = ProjectWindowUtil.IsFolder(draggedInstanceID);
        DragAndDrop.objectReferences = ProjectWindowUtil.GetDragAndDropObjects(draggedInstanceID, selectedInstanceIDs);
        DragAndDrop.SetGenericData(ProjectWindowUtil.k_IsFolderGenericData, !flag ? (object) string.Empty : (object) "isFolder");
        string[] dragAndDropPaths = ProjectWindowUtil.GetDragAndDropPaths(draggedInstanceID, selectedInstanceIDs);
        if (dragAndDropPaths.Length > 0)
          DragAndDrop.paths = dragAndDropPaths;
        title = DragAndDrop.objectReferences.Length <= 1 ? ObjectNames.GetDragAndDropTitle(InternalEditorUtility.GetObjectFromInstanceID(draggedInstanceID)) : "<Multiple>";
      }
      DragAndDrop.StartDrag(title);
    }

    internal static UnityEngine.Object[] GetDragAndDropObjects(int draggedInstanceID, List<int> selectedInstanceIDs)
    {
      List<UnityEngine.Object> objectList = new List<UnityEngine.Object>(selectedInstanceIDs.Count);
      if (selectedInstanceIDs.Contains(draggedInstanceID))
      {
        for (int index = 0; index < selectedInstanceIDs.Count; ++index)
        {
          UnityEngine.Object objectFromInstanceId = InternalEditorUtility.GetObjectFromInstanceID(selectedInstanceIDs[index]);
          if (objectFromInstanceId != (UnityEngine.Object) null)
            objectList.Add(objectFromInstanceId);
        }
      }
      else
      {
        UnityEngine.Object objectFromInstanceId = InternalEditorUtility.GetObjectFromInstanceID(draggedInstanceID);
        if (objectFromInstanceId != (UnityEngine.Object) null)
          objectList.Add(objectFromInstanceId);
      }
      return objectList.ToArray();
    }

    internal static string[] GetDragAndDropPaths(int draggedInstanceID, List<int> selectedInstanceIDs)
    {
      List<string> stringList = new List<string>();
      using (List<int>.Enumerator enumerator = selectedInstanceIDs.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          int current = enumerator.Current;
          if (AssetDatabase.IsMainAsset(current))
          {
            string assetPath = AssetDatabase.GetAssetPath(current);
            stringList.Add(assetPath);
          }
        }
      }
      string assetPath1 = AssetDatabase.GetAssetPath(draggedInstanceID);
      if (string.IsNullOrEmpty(assetPath1))
        return new string[0];
      if (stringList.Contains(assetPath1))
        return stringList.ToArray();
      return new string[1]{ assetPath1 };
    }

    public static int[] GetAncestors(int instanceID)
    {
      List<int> intList = new List<int>();
      int mainAssetInstanceId1 = AssetDatabase.GetMainAssetInstanceID(AssetDatabase.GetAssetPath(instanceID));
      if (mainAssetInstanceId1 != instanceID)
        intList.Add(mainAssetInstanceId1);
      int mainAssetInstanceId2;
      for (string containingFolder = ProjectWindowUtil.GetContainingFolder(AssetDatabase.GetAssetPath(mainAssetInstanceId1)); !string.IsNullOrEmpty(containingFolder); containingFolder = ProjectWindowUtil.GetContainingFolder(AssetDatabase.GetAssetPath(mainAssetInstanceId2)))
      {
        mainAssetInstanceId2 = AssetDatabase.GetMainAssetInstanceID(containingFolder);
        intList.Add(mainAssetInstanceId2);
      }
      return intList.ToArray();
    }

    public static bool IsFolder(int instanceID)
    {
      return AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(instanceID));
    }

    public static string GetContainingFolder(string path)
    {
      if (string.IsNullOrEmpty(path))
        return (string) null;
      path = path.Trim('/');
      int length = path.LastIndexOf("/", StringComparison.Ordinal);
      if (length != -1)
        return path.Substring(0, length);
      return (string) null;
    }

    public static string[] GetBaseFolders(string[] folders)
    {
      if (folders.Length <= 1)
        return folders;
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>((IEnumerable<string>) folders);
      for (int index = 0; index < stringList2.Count; ++index)
        stringList2[index] = stringList2[index].Trim('/');
      stringList2.Sort();
      for (int index = 0; index < stringList2.Count; ++index)
      {
        if (!stringList2[index].EndsWith("/"))
          stringList2[index] = stringList2[index] + "/";
      }
      string str = stringList2[0];
      stringList1.Add(str);
      for (int index = 1; index < stringList2.Count; ++index)
      {
        if (stringList2[index].IndexOf(str, StringComparison.Ordinal) != 0)
        {
          stringList1.Add(stringList2[index]);
          str = stringList2[index];
        }
      }
      for (int index = 0; index < stringList1.Count; ++index)
        stringList1[index] = stringList1[index].Trim('/');
      return stringList1.ToArray();
    }

    internal static void DuplicateSelectedAssets()
    {
      AssetDatabase.Refresh();
      UnityEngine.Object[] objects = Selection.objects;
      bool flag1 = true;
      foreach (UnityEngine.Object @object in objects)
      {
        AnimationClip animationClip = @object as AnimationClip;
        if ((UnityEngine.Object) animationClip == (UnityEngine.Object) null || (animationClip.hideFlags & HideFlags.NotEditable) == HideFlags.None || !AssetDatabase.Contains((UnityEngine.Object) animationClip))
          flag1 = false;
      }
      ArrayList arrayList = new ArrayList();
      bool flag2 = false;
      if (flag1)
      {
        foreach (UnityEngine.Object assetObject in objects)
        {
          AnimationClip animationClip1 = assetObject as AnimationClip;
          if ((UnityEngine.Object) animationClip1 != (UnityEngine.Object) null && (animationClip1.hideFlags & HideFlags.NotEditable) != HideFlags.None)
          {
            string uniqueAssetPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(Path.GetDirectoryName(AssetDatabase.GetAssetPath(assetObject)), animationClip1.name) + ".anim");
            AnimationClip animationClip2 = new AnimationClip();
            EditorUtility.CopySerialized((UnityEngine.Object) animationClip1, (UnityEngine.Object) animationClip2);
            AssetDatabase.CreateAsset((UnityEngine.Object) animationClip2, uniqueAssetPath);
            arrayList.Add((object) uniqueAssetPath);
          }
        }
      }
      else
      {
        foreach (UnityEngine.Object assetObject in Selection.GetFiltered(typeof (UnityEngine.Object), SelectionMode.Assets))
        {
          string assetPath = AssetDatabase.GetAssetPath(assetObject);
          string uniqueAssetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);
          if (uniqueAssetPath.Length != 0)
            flag2 |= !AssetDatabase.CopyAsset(assetPath, uniqueAssetPath);
          else
            flag2 = ((flag2 ? 1 : 0) | 1) != 0;
          if (!flag2)
            arrayList.Add((object) uniqueAssetPath);
        }
      }
      AssetDatabase.Refresh();
      UnityEngine.Object[] objectArray = new UnityEngine.Object[arrayList.Count];
      for (int index = 0; index < arrayList.Count; ++index)
        objectArray[index] = AssetDatabase.LoadMainAssetAtPath(arrayList[index] as string);
      Selection.objects = objectArray;
    }
  }
}
