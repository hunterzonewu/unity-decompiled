// Decompiled with JetBrains decompiler
// Type: GenerateIconsWithMipLevels
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

internal class GenerateIconsWithMipLevels
{
  private static string k_IconSourceFolder = "Assets/MipLevels For Icons/";
  private static string k_IconTargetFolder = "Assets/Editor Default Resources/Icons/Generated";

  public static void GenerateAllIconsWithMipLevels()
  {
    GenerateIconsWithMipLevels.InputData inputData = new GenerateIconsWithMipLevels.InputData();
    inputData.sourceFolder = GenerateIconsWithMipLevels.k_IconSourceFolder;
    inputData.targetFolder = GenerateIconsWithMipLevels.k_IconTargetFolder;
    inputData.mipIdentifier = "@";
    inputData.mipFileExtension = "png";
    if (AssetDatabase.GetMainAssetInstanceID(inputData.targetFolder) != 0)
    {
      AssetDatabase.DeleteAsset(inputData.targetFolder);
      AssetDatabase.Refresh();
    }
    GenerateIconsWithMipLevels.EnsureFolderIsCreated(inputData.targetFolder);
    float realtimeSinceStartup = Time.realtimeSinceStartup;
    GenerateIconsWithMipLevels.GenerateIconsWithMips(inputData);
    Debug.Log((object) string.Format("Generated {0} icons with mip levels in {1} seconds", (object) inputData.generatedFileNames.Count, (object) (float) ((double) Time.realtimeSinceStartup - (double) realtimeSinceStartup)));
    GenerateIconsWithMipLevels.RemoveUnusedFiles(inputData.generatedFileNames);
    AssetDatabase.Refresh();
    InternalEditorUtility.RepaintAllViews();
  }

  public static void GenerateSelectedIconsWithMips()
  {
    if (Selection.activeInstanceID == 0)
    {
      Debug.Log((object) ("Ensure to select a mip texture..." + (object) Selection.activeInstanceID));
    }
    else
    {
      GenerateIconsWithMipLevels.InputData inputData = new GenerateIconsWithMipLevels.InputData();
      inputData.sourceFolder = GenerateIconsWithMipLevels.k_IconSourceFolder;
      inputData.targetFolder = GenerateIconsWithMipLevels.k_IconTargetFolder;
      inputData.mipIdentifier = "@";
      inputData.mipFileExtension = "png";
      string assetPath = AssetDatabase.GetAssetPath(Selection.activeInstanceID);
      if (assetPath.IndexOf(inputData.sourceFolder) < 0)
        Debug.Log((object) ("Selection is not a valid mip texture, it should be located in: " + inputData.sourceFolder));
      else if (assetPath.IndexOf(inputData.mipIdentifier) < 0)
      {
        Debug.Log((object) ("Selection does not have a valid mip identifier " + assetPath + "  " + inputData.mipIdentifier));
      }
      else
      {
        float realtimeSinceStartup = Time.realtimeSinceStartup;
        string str = assetPath.Replace(inputData.sourceFolder, string.Empty);
        string baseName = str.Substring(0, str.LastIndexOf(inputData.mipIdentifier));
        List<string> iconAssetPaths = GenerateIconsWithMipLevels.GetIconAssetPaths(inputData.sourceFolder, inputData.mipIdentifier, inputData.mipFileExtension);
        GenerateIconsWithMipLevels.EnsureFolderIsCreated(inputData.targetFolder);
        GenerateIconsWithMipLevels.GenerateIcon(inputData, baseName, iconAssetPaths);
        Debug.Log((object) string.Format("Generated {0} icon with mip levels in {1} seconds", (object) baseName, (object) (float) ((double) Time.realtimeSinceStartup - (double) realtimeSinceStartup)));
        InternalEditorUtility.RepaintAllViews();
      }
    }
  }

  private static void GenerateIconsWithMips(GenerateIconsWithMipLevels.InputData inputData)
  {
    List<string> iconAssetPaths = GenerateIconsWithMipLevels.GetIconAssetPaths(inputData.sourceFolder, inputData.mipIdentifier, inputData.mipFileExtension);
    if (iconAssetPaths.Count == 0)
      Debug.LogWarning((object) ("No mip files found for generating icons! Searching in: " + inputData.sourceFolder + ", for files with extension: " + inputData.mipFileExtension));
    foreach (string baseName in GenerateIconsWithMipLevels.GetBaseNames(inputData, iconAssetPaths))
      GenerateIconsWithMipLevels.GenerateIcon(inputData, baseName, iconAssetPaths);
  }

  private static void GenerateIcon(GenerateIconsWithMipLevels.InputData inputData, string baseName, List<string> assetPathsOfAllIcons)
  {
    string path = inputData.targetFolder + "/" + baseName + " Icon.asset";
    GenerateIconsWithMipLevels.EnsureFolderIsCreatedRecursively(Path.GetDirectoryName(path));
    Texture2D iconWithMipLevels = GenerateIconsWithMipLevels.CreateIconWithMipLevels(inputData, baseName, assetPathsOfAllIcons);
    if ((UnityEngine.Object) iconWithMipLevels == (UnityEngine.Object) null)
    {
      Debug.Log((object) "CreateIconWithMipLevels failed");
    }
    else
    {
      iconWithMipLevels.name = baseName + " Icon.png";
      AssetDatabase.CreateAsset((UnityEngine.Object) iconWithMipLevels, path);
      inputData.generatedFileNames.Add(path);
    }
  }

  private static Texture2D CreateIconWithMipLevels(GenerateIconsWithMipLevels.InputData inputData, string baseName, List<string> assetPathsOfAllIcons)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    GenerateIconsWithMipLevels.\u003CCreateIconWithMipLevels\u003Ec__AnonStorey39 levelsCAnonStorey39 = new GenerateIconsWithMipLevels.\u003CCreateIconWithMipLevels\u003Ec__AnonStorey39();
    // ISSUE: reference to a compiler-generated field
    levelsCAnonStorey39.baseName = baseName;
    // ISSUE: reference to a compiler-generated field
    levelsCAnonStorey39.inputData = inputData;
    // ISSUE: reference to a compiler-generated method
    List<string> all = assetPathsOfAllIcons.FindAll(new Predicate<string>(levelsCAnonStorey39.\u003C\u003Em__4F));
    List<Texture2D> texture2DList = new List<Texture2D>();
    using (List<string>.Enumerator enumerator = all.GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        string current = enumerator.Current;
        Texture2D texture2D = GenerateIconsWithMipLevels.GetTexture2D(current);
        if ((UnityEngine.Object) texture2D != (UnityEngine.Object) null)
          texture2DList.Add(texture2D);
        else
          Debug.LogError((object) ("Mip not found " + current));
      }
    }
    int num1 = 99999;
    int num2 = 0;
    using (List<Texture2D>.Enumerator enumerator = texture2DList.GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        int width = enumerator.Current.width;
        if (width > num2)
          num2 = width;
        if (width < num1)
          num1 = width;
      }
    }
    if (num2 == 0)
      return (Texture2D) null;
    Texture2D iconWithMips = new Texture2D(num2, num2, TextureFormat.ARGB32, true, true);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    if (!GenerateIconsWithMipLevels.BlitMip(iconWithMips, levelsCAnonStorey39.inputData.GetMipFileName(levelsCAnonStorey39.baseName, num2), 0))
      return iconWithMips;
    iconWithMips.Apply(true);
    int mipResolution = num2;
    for (int mipLevel = 1; mipLevel < iconWithMips.mipmapCount; ++mipLevel)
    {
      mipResolution /= 2;
      if (mipResolution >= num1)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        GenerateIconsWithMipLevels.BlitMip(iconWithMips, levelsCAnonStorey39.inputData.GetMipFileName(levelsCAnonStorey39.baseName, mipResolution), mipLevel);
      }
      else
        break;
    }
    iconWithMips.Apply(false, true);
    return iconWithMips;
  }

  private static bool BlitMip(Texture2D iconWithMips, string mipFile, int mipLevel)
  {
    Texture2D texture2D = GenerateIconsWithMipLevels.GetTexture2D(mipFile);
    if ((bool) ((UnityEngine.Object) texture2D))
    {
      GenerateIconsWithMipLevels.Blit(texture2D, iconWithMips, mipLevel);
      return true;
    }
    Debug.Log((object) ("Mip file NOT found: " + mipFile));
    return false;
  }

  private static Texture2D GetTexture2D(string path)
  {
    return AssetDatabase.LoadAssetAtPath(path, typeof (Texture2D)) as Texture2D;
  }

  private static List<string> GetIconAssetPaths(string folderPath, string mustHaveIdentifier, string extension)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    GenerateIconsWithMipLevels.\u003CGetIconAssetPaths\u003Ec__AnonStorey3A pathsCAnonStorey3A = new GenerateIconsWithMipLevels.\u003CGetIconAssetPaths\u003Ec__AnonStorey3A();
    // ISSUE: reference to a compiler-generated field
    pathsCAnonStorey3A.mustHaveIdentifier = mustHaveIdentifier;
    string str = Path.Combine(Directory.GetCurrentDirectory(), folderPath);
    Uri uri1 = new Uri(str);
    List<string> stringList = new List<string>((IEnumerable<string>) Directory.GetFiles(str, "*." + extension, SearchOption.AllDirectories));
    // ISSUE: reference to a compiler-generated method
    stringList.RemoveAll(new Predicate<string>(pathsCAnonStorey3A.\u003C\u003Em__50));
    for (int index = 0; index < stringList.Count; ++index)
    {
      Uri uri2 = new Uri(stringList[index]);
      Uri uri3 = uri1.MakeRelativeUri(uri2);
      stringList[index] = folderPath + uri3.ToString();
    }
    return stringList;
  }

  private static void Blit(Texture2D source, Texture2D dest, int mipLevel)
  {
    Color32[] pixels32 = source.GetPixels32();
    for (int index = 0; index < pixels32.Length; ++index)
    {
      Color32 color32 = pixels32[index];
      if ((int) color32.a >= 3)
        color32.a -= (byte) 3;
      pixels32[index] = color32;
    }
    dest.SetPixels32(pixels32, mipLevel);
  }

  private static void EnsureFolderIsCreatedRecursively(string targetFolder)
  {
    if (AssetDatabase.GetMainAssetInstanceID(targetFolder) != 0)
      return;
    GenerateIconsWithMipLevels.EnsureFolderIsCreatedRecursively(Path.GetDirectoryName(targetFolder));
    Debug.Log((object) ("Created target folder " + targetFolder));
    AssetDatabase.CreateFolder(Path.GetDirectoryName(targetFolder), Path.GetFileName(targetFolder));
  }

  private static void EnsureFolderIsCreated(string targetFolder)
  {
    if (AssetDatabase.GetMainAssetInstanceID(targetFolder) != 0)
      return;
    Debug.Log((object) ("Created target folder " + targetFolder));
    AssetDatabase.CreateFolder(Path.GetDirectoryName(targetFolder), Path.GetFileName(targetFolder));
  }

  private static void DeleteFile(string file)
  {
    if (AssetDatabase.GetMainAssetInstanceID(file) == 0)
      return;
    Debug.Log((object) ("Deleted unused file: " + file));
    AssetDatabase.DeleteAsset(file);
  }

  private static void RemoveUnusedFiles(List<string> generatedFiles)
  {
    for (int index = 0; index < generatedFiles.Count; ++index)
    {
      string str = generatedFiles[index].Replace("Icons/Generated", "Icons").Replace(".asset", ".png");
      GenerateIconsWithMipLevels.DeleteFile(str);
      string withoutExtension = Path.GetFileNameWithoutExtension(str);
      if (!withoutExtension.StartsWith("d_"))
        GenerateIconsWithMipLevels.DeleteFile(str.Replace(withoutExtension, "d_" + withoutExtension));
    }
    AssetDatabase.Refresh();
  }

  private static string[] GetBaseNames(GenerateIconsWithMipLevels.InputData inputData, List<string> files)
  {
    string[] strArray = new string[files.Count];
    int length = inputData.sourceFolder.Length;
    for (int index = 0; index < files.Count; ++index)
      strArray[index] = files[index].Substring(length, files[index].IndexOf(inputData.mipIdentifier) - length);
    HashSet<string> stringSet = new HashSet<string>((IEnumerable<string>) strArray);
    string[] array = new string[stringSet.Count];
    stringSet.CopyTo(array);
    return array;
  }

  private class InputData
  {
    public List<string> generatedFileNames = new List<string>();
    public string sourceFolder;
    public string targetFolder;
    public string mipIdentifier;
    public string mipFileExtension;

    public string GetMipFileName(string baseName, int mipResolution)
    {
      return this.sourceFolder + baseName + this.mipIdentifier + (object) mipResolution + "." + this.mipFileExtension;
    }
  }
}
