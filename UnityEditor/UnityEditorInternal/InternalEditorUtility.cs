// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.InternalEditorUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.Scripting;
using UnityEngine;
using UnityEngine.Internal;

namespace UnityEditorInternal
{
  public sealed class InternalEditorUtility
  {
    public static extern bool isApplicationActive { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern bool inBatchMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern bool isHumanControllingUs { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern int[] expandedProjectWindowItems { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern string[] tags { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern string[] layers { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal static extern string[] sortingLayerNames { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal static extern int[] sortingLayerUniqueIDs { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern string unityPreferencesFolder { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern float defaultScreenWidth { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern float defaultScreenHeight { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern float defaultWebScreenWidth { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern float defaultWebScreenHeight { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern float remoteScreenWidth { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern float remoteScreenHeight { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern bool ignoreInspectorChanges { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void BumpMapSettingsFixingWindowReportResult(int result);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool BumpMapTextureNeedsFixingInternal(Material material, string propName, bool flaggedAsNormal);

    internal static bool BumpMapTextureNeedsFixing(MaterialProperty prop)
    {
      if (prop.type != MaterialProperty.PropType.Texture)
        return false;
      bool flaggedAsNormal = (prop.flags & MaterialProperty.PropFlags.Normal) != MaterialProperty.PropFlags.None;
      foreach (Material target in prop.targets)
      {
        if (InternalEditorUtility.BumpMapTextureNeedsFixingInternal(target, prop.name, flaggedAsNormal))
          return true;
      }
      return false;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void FixNormalmapTextureInternal(Material material, string propName);

    internal static void FixNormalmapTexture(MaterialProperty prop)
    {
      foreach (Material target in prop.targets)
        InternalEditorUtility.FixNormalmapTextureInternal(target, prop.name);
    }

    internal static bool HDRTextureNeedsFixing(MaterialProperty prop, out bool canBeFixedAutomatically)
    {
      if ((prop.flags & MaterialProperty.PropFlags.HDR) != MaterialProperty.PropFlags.None || prop.displayName.Contains("(HDR"))
      {
        Texture textureValue = prop.textureValue;
        if ((bool) ((UnityEngine.Object) textureValue))
        {
          TextureImporter atPath = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath((UnityEngine.Object) textureValue)) as TextureImporter;
          canBeFixedAutomatically = (UnityEngine.Object) atPath != (UnityEngine.Object) null;
          if (TextureUtil.HasAlphaTextureFormat(TextureUtil.GetTextureFormat(textureValue)) && TextureUtil.GetUsageMode(textureValue) != TextureUsageMode.RGBMEncoded)
            return true;
        }
      }
      canBeFixedAutomatically = false;
      return false;
    }

    internal static void FixHDRTexture(MaterialProperty prop)
    {
      string assetPath = AssetDatabase.GetAssetPath((UnityEngine.Object) prop.textureValue);
      TextureImporter atPath = AssetImporter.GetAtPath(assetPath) as TextureImporter;
      if (!(bool) ((UnityEngine.Object) atPath))
        return;
      TextureImporterFormat textureFormat1 = TextureImporterFormat.RGB24;
      atPath.textureFormat = textureFormat1;
      using (List<BuildPlayerWindow.BuildPlatform>.Enumerator enumerator = BuildPlayerWindow.GetValidPlatforms().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          BuildPlayerWindow.BuildPlatform current = enumerator.Current;
          int maxTextureSize;
          TextureImporterFormat textureFormat2;
          int compressionQuality;
          if (atPath.GetPlatformTextureSettings(current.name, out maxTextureSize, out textureFormat2, out compressionQuality))
            atPath.SetPlatformTextureSettings(current.name, maxTextureSize, textureFormat1, compressionQuality, false);
        }
      }
      AssetDatabase.ImportAsset(assetPath);
      foreach (UnityEngine.Object target in prop.targets)
        EditorUtility.SetDirty(target);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetEditorAssemblyPath();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetEngineAssemblyPath();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string CalculateHashForObjectsAndDependencies(UnityEngine.Object[] objects);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ExecuteCommandOnKeyWindow(string commandName);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Material[] InstantiateMaterialsInEditMode(Renderer renderer);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern CanAppendBuild BuildCanBeAppended(BuildTarget target, string location);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void RegisterExtensionDll(string dllLocation, string guid);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetupCustomDll(string dllName, string dllLocation);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Assembly LoadAssemblyWrapper(string dllName, string dllLocation);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetPlatformPath(string path);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int AddScriptComponentUncheckedUndoable(GameObject gameObject, MonoScript script);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int CreateScriptableObjectUnchecked(MonoScript script);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void RequestScriptReload();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SwitchSkinAndRepaintAllViews();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void RepaintAllViews();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GetIsInspectorExpanded(UnityEngine.Object obj);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetIsInspectorExpanded(UnityEngine.Object obj, bool isExpanded);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SaveToSerializedFileAndForget(UnityEngine.Object[] obj, string path, bool allowTextSerialization);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern UnityEngine.Object[] LoadSerializedFileAndForget(string path);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern DragAndDropVisualMode ProjectWindowDrag(HierarchyProperty property, bool perform);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern DragAndDropVisualMode HierarchyWindowDrag(HierarchyProperty property, bool perform, InternalEditorUtility.HierarchyDropMode dropMode);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern DragAndDropVisualMode InspectorWindowDrag(UnityEngine.Object[] targets, bool perform);

    public static DragAndDropVisualMode SceneViewDrag(UnityEngine.Object dropUpon, Vector3 worldPosition, Vector2 viewportPosition, bool perform)
    {
      return InternalEditorUtility.INTERNAL_CALL_SceneViewDrag(dropUpon, ref worldPosition, ref viewportPosition, perform);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern DragAndDropVisualMode INTERNAL_CALL_SceneViewDrag(UnityEngine.Object dropUpon, ref Vector3 worldPosition, ref Vector2 viewportPosition, bool perform);

    public static void SetRectTransformTemporaryRect(RectTransform rectTransform, Rect rect)
    {
      InternalEditorUtility.INTERNAL_CALL_SetRectTransformTemporaryRect(rectTransform, ref rect);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetRectTransformTemporaryRect(RectTransform rectTransform, ref Rect rect);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool HasTeamLicense();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool HasPro();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool HasFreeLicense();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool HasEduLicense();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool HasAdvancedLicenseOnBuildTarget(BuildTarget target);

    public static Rect GetBoundsOfDesktopAtPoint(Vector2 pos)
    {
      Rect rect;
      InternalEditorUtility.INTERNAL_CALL_GetBoundsOfDesktopAtPoint(ref pos, out rect);
      return rect;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetBoundsOfDesktopAtPoint(ref Vector2 pos, out Rect value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string GetSortingLayerName(int index);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int GetSortingLayerUniqueID(int index);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string GetSortingLayerNameFromUniqueID(int id);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int GetSortingLayerCount();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetSortingLayerName(int index, string name);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetSortingLayerLocked(int index, bool locked);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool GetSortingLayerLocked(int index);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool IsSortingLayerDefault(int index);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void AddSortingLayer();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void UpdateSortingLayersOrder();

    public static Vector4 GetSpriteOuterUV(Sprite sprite, bool getAtlasData)
    {
      Vector4 vector4;
      InternalEditorUtility.INTERNAL_CALL_GetSpriteOuterUV(sprite, getAtlasData, out vector4);
      return vector4;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetSpriteOuterUV(Sprite sprite, bool getAtlasData, out Vector4 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern UnityEngine.Object GetObjectFromInstanceID(int instanceID);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetClassIDWithoutLoadingObject(int instanceID);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern UnityEngine.Object GetLoadedObjectFromInstanceID(int instanceID);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetLayerName(int layer);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetExternalScriptEditor();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetExternalScriptEditorArgs();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ReloadWindowLayoutMenu();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void RevertFactoryLayoutSettings(bool quitOnCancel);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void LoadDefaultLayout();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void CalculateAmbientProbeFromSkybox();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetupShaderMenu(Material material);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetUnityVersionFull();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetFullUnityVersion();

    public static Version GetUnityVersion()
    {
      Version version = new Version(InternalEditorUtility.GetUnityVersionDigits());
      return new Version(version.Major, version.Minor, version.Build, InternalEditorUtility.GetUnityRevision());
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetUnityVersionDigits();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetUnityBuildBranch();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetUnityVersionDate();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetUnityRevision();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool IsUnityBeta();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetUnityCopyright();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetLicenseInfo();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int[] GetLicenseFlags();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetAuthToken();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void OpenEditorConsole();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetGameObjectInstanceIDFromComponent(int instanceID);

    public static Color[] ReadScreenPixel(Vector2 pixelPos, int sizex, int sizey)
    {
      return InternalEditorUtility.INTERNAL_CALL_ReadScreenPixel(ref pixelPos, sizex, sizey);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Color[] INTERNAL_CALL_ReadScreenPixel(ref Vector2 pixelPos, int sizex, int sizey);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void OpenPlayerConsole();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Resolution GetDesktopResolution();

    public static string TextifyEvent(Event evt)
    {
      if (evt == null)
        return "none";
      KeyCode keyCode = evt.keyCode;
      string str;
      switch (keyCode)
      {
        case KeyCode.Keypad0:
          str = "[0]";
          break;
        case KeyCode.Keypad1:
          str = "[1]";
          break;
        case KeyCode.Keypad2:
          str = "[2]";
          break;
        case KeyCode.Keypad3:
          str = "[3]";
          break;
        case KeyCode.Keypad4:
          str = "[4]";
          break;
        case KeyCode.Keypad5:
          str = "[5]";
          break;
        case KeyCode.Keypad6:
          str = "[6]";
          break;
        case KeyCode.Keypad7:
          str = "[7]";
          break;
        case KeyCode.Keypad8:
          str = "[8]";
          break;
        case KeyCode.Keypad9:
          str = "[9]";
          break;
        case KeyCode.KeypadPeriod:
          str = "[.]";
          break;
        case KeyCode.KeypadDivide:
          str = "[/]";
          break;
        case KeyCode.KeypadMinus:
          str = "[-]";
          break;
        case KeyCode.KeypadPlus:
          str = "[+]";
          break;
        case KeyCode.KeypadEnter:
          str = "enter";
          break;
        case KeyCode.KeypadEquals:
          str = "[=]";
          break;
        case KeyCode.UpArrow:
          str = "up";
          break;
        case KeyCode.DownArrow:
          str = "down";
          break;
        case KeyCode.RightArrow:
          str = "right";
          break;
        case KeyCode.LeftArrow:
          str = "left";
          break;
        case KeyCode.Insert:
          str = "insert";
          break;
        case KeyCode.Home:
          str = "home";
          break;
        case KeyCode.End:
          str = "end";
          break;
        case KeyCode.PageUp:
          str = "page up";
          break;
        case KeyCode.PageDown:
          str = "page down";
          break;
        case KeyCode.F1:
          str = "F1";
          break;
        case KeyCode.F2:
          str = "F2";
          break;
        case KeyCode.F3:
          str = "F3";
          break;
        case KeyCode.F4:
          str = "F4";
          break;
        case KeyCode.F5:
          str = "F5";
          break;
        case KeyCode.F6:
          str = "F6";
          break;
        case KeyCode.F7:
          str = "F7";
          break;
        case KeyCode.F8:
          str = "F8";
          break;
        case KeyCode.F9:
          str = "F9";
          break;
        case KeyCode.F10:
          str = "F10";
          break;
        case KeyCode.F11:
          str = "F11";
          break;
        case KeyCode.F12:
          str = "F12";
          break;
        case KeyCode.F13:
          str = "F13";
          break;
        case KeyCode.F14:
          str = "F14";
          break;
        case KeyCode.F15:
          str = "F15";
          break;
        default:
          str = keyCode == KeyCode.Backspace ? "backspace" : (keyCode == KeyCode.Return ? "return" : (keyCode == KeyCode.Escape ? "[esc]" : (keyCode == KeyCode.Delete ? "delete" : string.Empty + (object) evt.keyCode)));
          break;
      }
      string empty = string.Empty;
      if (evt.alt)
        empty += "Alt+";
      if (evt.command)
        empty += Application.platform != RuntimePlatform.OSXEditor ? "Ctrl+" : "Cmd+";
      if (evt.control)
        empty += "Ctrl+";
      if (evt.shift)
        empty += "Shift+";
      return empty + str;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string[] GetAvailableDiffTools();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetNoDiffToolsDetectedMessage();

    public static Bounds TransformBounds(Bounds b, Transform t)
    {
      Bounds bounds;
      InternalEditorUtility.INTERNAL_CALL_TransformBounds(ref b, t, out bounds);
      return bounds;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_TransformBounds(ref Bounds b, Transform t, out Bounds value);

    public static void SetCustomLighting(Light[] lights, Color ambient)
    {
      InternalEditorUtility.INTERNAL_CALL_SetCustomLighting(lights, ref ambient);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetCustomLighting(Light[] lights, ref Color ambient);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void ClearSceneLighting();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void RemoveCustomLighting();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void DrawSkyboxMaterial(Material mat, Camera cam);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool HasFullscreenCamera();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ResetCursor();

    public static Bounds CalculateSelectionBounds(bool usePivotOnlyForParticles, bool onlyUseActiveSelection)
    {
      Bounds bounds;
      InternalEditorUtility.INTERNAL_CALL_CalculateSelectionBounds(usePivotOnlyForParticles, onlyUseActiveSelection, out bounds);
      return bounds;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_CalculateSelectionBounds(bool usePivotOnlyForParticles, bool onlyUseActiveSelection, out Bounds value);

    internal static Bounds CalculateSelectionBoundsInSpace(Vector3 position, Quaternion rotation, bool rectBlueprintMode)
    {
      Quaternion quaternion = Quaternion.Inverse(rotation);
      Vector3 vector3_1 = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
      Vector3 vector3_2 = new Vector3(float.MinValue, float.MinValue, float.MinValue);
      Vector3[] vector3Array = new Vector3[2];
      foreach (GameObject gameObject in Selection.gameObjects)
      {
        Bounds localBounds = InternalEditorUtility.GetLocalBounds(gameObject);
        vector3Array[0] = localBounds.min;
        vector3Array[1] = localBounds.max;
        for (int index1 = 0; index1 < 2; ++index1)
        {
          for (int index2 = 0; index2 < 2; ++index2)
          {
            for (int index3 = 0; index3 < 2; ++index3)
            {
              Vector3 position1 = new Vector3(vector3Array[index1].x, vector3Array[index2].y, vector3Array[index3].z);
              if (rectBlueprintMode && InternalEditorUtility.SupportsRectLayout(gameObject.transform))
              {
                Vector3 localPosition = gameObject.transform.localPosition;
                localPosition.z = 0.0f;
                position1 = gameObject.transform.parent.TransformPoint(position1 + localPosition);
              }
              else
                position1 = gameObject.transform.TransformPoint(position1);
              position1 = quaternion * (position1 - position);
              for (int index4 = 0; index4 < 3; ++index4)
              {
                vector3_1[index4] = Mathf.Min(vector3_1[index4], position1[index4]);
                vector3_2[index4] = Mathf.Max(vector3_2[index4], position1[index4]);
              }
            }
          }
        }
      }
      return new Bounds((vector3_1 + vector3_2) * 0.5f, vector3_2 - vector3_1);
    }

    internal static bool SupportsRectLayout(Transform tr)
    {
      return !((UnityEngine.Object) tr == (UnityEngine.Object) null) && !((UnityEngine.Object) tr.parent == (UnityEngine.Object) null) && (!((UnityEngine.Object) tr.GetComponent<RectTransform>() == (UnityEngine.Object) null) && !((UnityEngine.Object) tr.parent.GetComponent<RectTransform>() == (UnityEngine.Object) null));
    }

    private static Bounds GetLocalBounds(GameObject gameObject)
    {
      RectTransform component1 = gameObject.GetComponent<RectTransform>();
      if ((bool) ((UnityEngine.Object) component1))
        return new Bounds((Vector3) component1.rect.center, (Vector3) component1.rect.size);
      Renderer component2 = gameObject.GetComponent<Renderer>();
      if (component2 is MeshRenderer)
      {
        MeshFilter component3 = component2.GetComponent<MeshFilter>();
        if ((UnityEngine.Object) component3 != (UnityEngine.Object) null && (UnityEngine.Object) component3.sharedMesh != (UnityEngine.Object) null)
          return component3.sharedMesh.bounds;
      }
      if (component2 is SpriteRenderer)
      {
        SpriteRenderer spriteRenderer = component2 as SpriteRenderer;
        if ((UnityEngine.Object) spriteRenderer.sprite != (UnityEngine.Object) null)
        {
          Bounds bounds = spriteRenderer.sprite.bounds;
          Vector3 size = bounds.size;
          size.z = 0.0f;
          bounds.size = size;
          return bounds;
        }
      }
      return new Bounds(Vector3.zero, Vector3.zero);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void OnGameViewFocus(bool focus);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool OpenFileAtLineExternal(string filename, int line);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern MonoIsland[] GetMonoIslands();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool Xbox360GenerateSPAConfig(string spaPath);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool Xbox360SaveSplashScreenToFile(Texture2D image, string spaPath);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool WiiUSaveStartupScreenToFile(Texture2D image, string path, int outputWidth, int outputHeight);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool CanConnectToCacheServer();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern ulong VerifyCacheServerIntegrity();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern ulong FixCacheServerIntegrityErrors();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern DllType DetectDotNetDll(string path);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetCrashReportFolder();

    [ExcludeFromDocs]
    internal static bool RunningUnderWindows8()
    {
      return InternalEditorUtility.RunningUnderWindows8(true);
    }

    internal static bool RunningUnderWindows8([DefaultValue("true")] bool orHigher)
    {
      if (Application.platform != RuntimePlatform.WindowsEditor)
        return false;
      OperatingSystem osVersion = Environment.OSVersion;
      int major = osVersion.Version.Major;
      int minor = osVersion.Version.Minor;
      if (orHigher)
      {
        if (major > 6)
          return true;
        if (major == 6)
          return minor >= 2;
        return false;
      }
      if (major == 6)
        return minor == 2;
      return false;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int DetermineDepthOrder(Transform lhs, Transform rhs);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ShowPackageManagerWindow();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void AuxWindowManager_OnAssemblyReload();

    public static Vector2 PassAndReturnVector2(Vector2 v)
    {
      Vector2 vector2;
      InternalEditorUtility.INTERNAL_CALL_PassAndReturnVector2(ref v, out vector2);
      return vector2;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_PassAndReturnVector2(ref Vector2 v, out Vector2 value);

    public static Color32 PassAndReturnColor32(Color32 c)
    {
      Color32 color32;
      InternalEditorUtility.INTERNAL_CALL_PassAndReturnColor32(ref c, out color32);
      return color32;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_PassAndReturnColor32(ref Color32 c, out Color32 value);

    [Obsolete("use EditorSceneManager.EnsureUntitledSceneHasBeenSaved")]
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool EnsureSceneHasBeenSaved(string operation);

    public static Texture2D GetIconForFile(string fileName)
    {
      int num1 = fileName.LastIndexOf('.');
      string key = num1 != -1 ? fileName.Substring(num1 + 1).ToLower() : string.Empty;
      if (key != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (InternalEditorUtility.\u003C\u003Ef__switch\u0024map7 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InternalEditorUtility.\u003C\u003Ef__switch\u0024map7 = new Dictionary<string, int>(129)
          {
            {
              "boo",
              0
            },
            {
              "cginc",
              1
            },
            {
              "cs",
              2
            },
            {
              "guiskin",
              3
            },
            {
              "js",
              4
            },
            {
              "mat",
              5
            },
            {
              "physicmaterial",
              6
            },
            {
              "prefab",
              7
            },
            {
              "shader",
              8
            },
            {
              "txt",
              9
            },
            {
              "unity",
              10
            },
            {
              "asset",
              11
            },
            {
              "prefs",
              11
            },
            {
              "anim",
              12
            },
            {
              "meta",
              13
            },
            {
              "mixer",
              14
            },
            {
              "ttf",
              15
            },
            {
              "otf",
              15
            },
            {
              "fon",
              15
            },
            {
              "fnt",
              15
            },
            {
              "aac",
              16
            },
            {
              "aif",
              16
            },
            {
              "aiff",
              16
            },
            {
              "au",
              16
            },
            {
              "mid",
              16
            },
            {
              "midi",
              16
            },
            {
              "mp3",
              16
            },
            {
              "mpa",
              16
            },
            {
              "ra",
              16
            },
            {
              "ram",
              16
            },
            {
              "wma",
              16
            },
            {
              "wav",
              16
            },
            {
              "wave",
              16
            },
            {
              "ogg",
              16
            },
            {
              "ai",
              17
            },
            {
              "apng",
              17
            },
            {
              "png",
              17
            },
            {
              "bmp",
              17
            },
            {
              "cdr",
              17
            },
            {
              "dib",
              17
            },
            {
              "eps",
              17
            },
            {
              "exif",
              17
            },
            {
              "gif",
              17
            },
            {
              "ico",
              17
            },
            {
              "icon",
              17
            },
            {
              "j",
              17
            },
            {
              "j2c",
              17
            },
            {
              "j2k",
              17
            },
            {
              "jas",
              17
            },
            {
              "jiff",
              17
            },
            {
              "jng",
              17
            },
            {
              "jp2",
              17
            },
            {
              "jpc",
              17
            },
            {
              "jpe",
              17
            },
            {
              "jpeg",
              17
            },
            {
              "jpf",
              17
            },
            {
              "jpg",
              17
            },
            {
              "jpw",
              17
            },
            {
              "jpx",
              17
            },
            {
              "jtf",
              17
            },
            {
              "mac",
              17
            },
            {
              "omf",
              17
            },
            {
              "qif",
              17
            },
            {
              "qti",
              17
            },
            {
              "qtif",
              17
            },
            {
              "tex",
              17
            },
            {
              "tfw",
              17
            },
            {
              "tga",
              17
            },
            {
              "tif",
              17
            },
            {
              "tiff",
              17
            },
            {
              "wmf",
              17
            },
            {
              "psd",
              17
            },
            {
              "exr",
              17
            },
            {
              "hdr",
              17
            },
            {
              "3df",
              18
            },
            {
              "3dm",
              18
            },
            {
              "3dmf",
              18
            },
            {
              "3ds",
              18
            },
            {
              "3dv",
              18
            },
            {
              "3dx",
              18
            },
            {
              "blend",
              18
            },
            {
              "c4d",
              18
            },
            {
              "lwo",
              18
            },
            {
              "lws",
              18
            },
            {
              "ma",
              18
            },
            {
              "max",
              18
            },
            {
              "mb",
              18
            },
            {
              "mesh",
              18
            },
            {
              "obj",
              18
            },
            {
              "vrl",
              18
            },
            {
              "wrl",
              18
            },
            {
              "wrz",
              18
            },
            {
              "fbx",
              18
            },
            {
              "asf",
              19
            },
            {
              "asx",
              19
            },
            {
              "avi",
              19
            },
            {
              "dat",
              19
            },
            {
              "divx",
              19
            },
            {
              "dvx",
              19
            },
            {
              "mlv",
              19
            },
            {
              "m2l",
              19
            },
            {
              "m2t",
              19
            },
            {
              "m2ts",
              19
            },
            {
              "m2v",
              19
            },
            {
              "m4e",
              19
            },
            {
              "m4v",
              19
            },
            {
              "mjp",
              19
            },
            {
              "mov",
              19
            },
            {
              "movie",
              19
            },
            {
              "mp21",
              19
            },
            {
              "mp4",
              19
            },
            {
              "mpe",
              19
            },
            {
              "mpeg",
              19
            },
            {
              "mpg",
              19
            },
            {
              "mpv2",
              19
            },
            {
              "ogm",
              19
            },
            {
              "qt",
              19
            },
            {
              "rm",
              19
            },
            {
              "rmvb",
              19
            },
            {
              "wmw",
              19
            },
            {
              "xvid",
              19
            },
            {
              "colors",
              20
            },
            {
              "gradients",
              20
            },
            {
              "curves",
              20
            },
            {
              "curvesnormalized",
              20
            },
            {
              "particlecurves",
              20
            },
            {
              "particlecurvessigned",
              20
            },
            {
              "particledoublecurves",
              20
            },
            {
              "particledoublecurvessigned",
              20
            }
          };
        }
        int num2;
        // ISSUE: reference to a compiler-generated field
        if (InternalEditorUtility.\u003C\u003Ef__switch\u0024map7.TryGetValue(key, out num2))
        {
          switch (num2)
          {
            case 0:
              return EditorGUIUtility.FindTexture("boo Script Icon");
            case 1:
              return EditorGUIUtility.FindTexture("CGProgram Icon");
            case 2:
              return EditorGUIUtility.FindTexture("cs Script Icon");
            case 3:
              return EditorGUIUtility.FindTexture("GUISkin Icon");
            case 4:
              return EditorGUIUtility.FindTexture("Js Script Icon");
            case 5:
              return EditorGUIUtility.FindTexture("Material Icon");
            case 6:
              return EditorGUIUtility.FindTexture("PhysicMaterial Icon");
            case 7:
              return EditorGUIUtility.FindTexture("PrefabNormal Icon");
            case 8:
              return EditorGUIUtility.FindTexture("Shader Icon");
            case 9:
              return EditorGUIUtility.FindTexture("TextAsset Icon");
            case 10:
              return EditorGUIUtility.FindTexture("SceneAsset Icon");
            case 11:
              return EditorGUIUtility.FindTexture("GameManager Icon");
            case 12:
              return EditorGUIUtility.FindTexture("Animation Icon");
            case 13:
              return EditorGUIUtility.FindTexture("MetaFile Icon");
            case 14:
              return EditorGUIUtility.FindTexture("AudioMixerController Icon");
            case 15:
              return EditorGUIUtility.FindTexture("Font Icon");
            case 16:
              return EditorGUIUtility.FindTexture("AudioClip Icon");
            case 17:
              return EditorGUIUtility.FindTexture("Texture Icon");
            case 18:
              return EditorGUIUtility.FindTexture("Mesh Icon");
            case 19:
              return EditorGUIUtility.FindTexture("MovieTexture Icon");
            case 20:
              return EditorGUIUtility.FindTexture("ScriptableObject Icon");
          }
        }
      }
      return EditorGUIUtility.FindTexture("DefaultAsset Icon");
    }

    public static string[] GetEditorSettingsList(string prefix, int count)
    {
      ArrayList arrayList = new ArrayList();
      for (int index = 1; index <= count; ++index)
      {
        string str = EditorPrefs.GetString(prefix + (object) index, "defaultValue");
        if (!(str == "defaultValue"))
          arrayList.Add((object) str);
        else
          break;
      }
      return arrayList.ToArray(typeof (string)) as string[];
    }

    public static void SaveEditorSettingsList(string prefix, string[] aList, int count)
    {
      for (int index = 0; index < aList.Length; ++index)
        EditorPrefs.SetString(prefix + (object) (index + 1), aList[index]);
      for (int index = aList.Length + 1; index <= count; ++index)
        EditorPrefs.DeleteKey(prefix + (object) index);
    }

    public static string TextAreaForDocBrowser(Rect position, string text, GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID("TextAreaWithTabHandling".GetHashCode(), FocusType.Keyboard, position);
      EditorGUI.RecycledTextEditor recycledEditor = EditorGUI.s_RecycledEditor;
      Event current = Event.current;
      if (recycledEditor.IsEditingControl(controlId) && current.type == EventType.KeyDown)
      {
        if ((int) current.character == 9)
        {
          recycledEditor.Insert('\t');
          current.Use();
          GUI.changed = true;
          text = recycledEditor.text;
        }
        if ((int) current.character == 10)
        {
          recycledEditor.Insert('\n');
          current.Use();
          GUI.changed = true;
          text = recycledEditor.text;
        }
      }
      bool changed;
      text = EditorGUI.DoTextField(recycledEditor, controlId, EditorGUI.IndentedRect(position), text, style, (string) null, out changed, false, true, false);
      return text;
    }

    public static Camera[] GetSceneViewCameras()
    {
      return SceneView.GetAllSceneCameras();
    }

    public static void ShowGameView()
    {
      WindowLayout.ShowAppropriateViewOnEnterExitPlaymode(true);
    }

    public static List<int> GetNewSelection(int clickedInstanceID, List<int> allInstanceIDs, List<int> selectedInstanceIDs, int lastClickedInstanceID, bool keepMultiSelection, bool useShiftAsActionKey, bool allowMultiSelection)
    {
      List<int> intList = new List<int>();
      bool flag1 = Event.current.shift || EditorGUI.actionKey && useShiftAsActionKey;
      bool flag2 = EditorGUI.actionKey && !useShiftAsActionKey;
      if (!allowMultiSelection)
        flag1 = flag2 = false;
      if (flag2)
      {
        intList.AddRange((IEnumerable<int>) selectedInstanceIDs);
        if (intList.Contains(clickedInstanceID))
          intList.Remove(clickedInstanceID);
        else
          intList.Add(clickedInstanceID);
      }
      else if (flag1)
      {
        if (clickedInstanceID == lastClickedInstanceID)
          return selectedInstanceIDs;
        int firstIndex;
        int lastIndex;
        if (!InternalEditorUtility.GetFirstAndLastSelected(allInstanceIDs, selectedInstanceIDs, out firstIndex, out lastIndex))
        {
          intList.Add(clickedInstanceID);
          return intList;
        }
        int num1 = -1;
        int num2 = -1;
        for (int index = 0; index < allInstanceIDs.Count; ++index)
        {
          if (allInstanceIDs[index] == clickedInstanceID)
            num1 = index;
          if (lastClickedInstanceID != 0 && allInstanceIDs[index] == lastClickedInstanceID)
            num2 = index;
        }
        int num3 = 0;
        if (num2 != -1)
          num3 = num1 <= num2 ? -1 : 1;
        int num4;
        int num5;
        if (num1 > lastIndex)
        {
          num4 = firstIndex;
          num5 = num1;
        }
        else if (num1 >= firstIndex && num1 < lastIndex)
        {
          if (num3 > 0)
          {
            num4 = num1;
            num5 = lastIndex;
          }
          else
          {
            num4 = firstIndex;
            num5 = num1;
          }
        }
        else
        {
          num4 = num1;
          num5 = lastIndex;
        }
        for (int index = num4; index <= num5; ++index)
          intList.Add(allInstanceIDs[index]);
      }
      else
      {
        if (keepMultiSelection && selectedInstanceIDs.Contains(clickedInstanceID))
        {
          intList.AddRange((IEnumerable<int>) selectedInstanceIDs);
          return intList;
        }
        intList.Add(clickedInstanceID);
      }
      return intList;
    }

    private static bool GetFirstAndLastSelected(List<int> allInstanceIDs, List<int> selectedInstanceIDs, out int firstIndex, out int lastIndex)
    {
      firstIndex = -1;
      lastIndex = -1;
      for (int index = 0; index < allInstanceIDs.Count; ++index)
      {
        if (selectedInstanceIDs.Contains(allInstanceIDs[index]))
        {
          if (firstIndex == -1)
            firstIndex = index;
          lastIndex = index;
        }
      }
      if (firstIndex != -1)
        return lastIndex != -1;
      return false;
    }

    public static bool IsValidFileName(string filename)
    {
      string str = InternalEditorUtility.RemoveInvalidCharsFromFileName(filename, false);
      return !(str != filename) && !string.IsNullOrEmpty(str);
    }

    public static string RemoveInvalidCharsFromFileName(string filename, bool logIfInvalidChars)
    {
      if (string.IsNullOrEmpty(filename))
        return filename;
      filename = filename.Trim();
      if (string.IsNullOrEmpty(filename))
        return filename;
      string str = new string(Path.GetInvalidFileNameChars());
      string empty = string.Empty;
      bool flag = false;
      foreach (char ch in filename)
      {
        if (str.IndexOf(ch) == -1)
          empty += (string) (object) ch;
        else
          flag = true;
      }
      if (flag && logIfInvalidChars)
      {
        string invalidCharsOfFileName = InternalEditorUtility.GetDisplayStringOfInvalidCharsOfFileName(filename);
        if (invalidCharsOfFileName.Length > 0)
          Debug.LogWarningFormat("A filename cannot contain the following character{0}:  {1}", new object[2]
          {
            (object) (invalidCharsOfFileName.Length <= 1 ? string.Empty : "s"),
            (object) invalidCharsOfFileName
          });
      }
      return empty;
    }

    public static string GetDisplayStringOfInvalidCharsOfFileName(string filename)
    {
      if (string.IsNullOrEmpty(filename))
        return string.Empty;
      string str = new string(Path.GetInvalidFileNameChars());
      string empty = string.Empty;
      foreach (char ch in filename)
      {
        if (str.IndexOf(ch) >= 0 && empty.IndexOf(ch) == -1)
        {
          if (empty.Length > 0)
            empty += " ";
          empty += (string) (object) ch;
        }
      }
      return empty;
    }

    internal static bool IsScriptOrAssembly(string filename)
    {
      if (string.IsNullOrEmpty(filename))
        return false;
      string lower = Path.GetExtension(filename).ToLower();
      if (lower != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (InternalEditorUtility.\u003C\u003Ef__switch\u0024map8 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InternalEditorUtility.\u003C\u003Ef__switch\u0024map8 = new Dictionary<string, int>(5)
          {
            {
              ".cs",
              0
            },
            {
              ".js",
              0
            },
            {
              ".boo",
              0
            },
            {
              ".dll",
              1
            },
            {
              ".exe",
              1
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (InternalEditorUtility.\u003C\u003Ef__switch\u0024map8.TryGetValue(lower, out num))
        {
          if (num == 0)
            return true;
          if (num == 1)
            return AssemblyHelper.IsManagedAssembly(filename);
        }
      }
      return false;
    }

    internal static T ParentHasComponent<T>(Transform trans) where T : Component
    {
      if (!((UnityEngine.Object) trans != (UnityEngine.Object) null))
        return (T) null;
      T component = trans.GetComponent<T>();
      if ((bool) ((UnityEngine.Object) component))
        return component;
      return InternalEditorUtility.ParentHasComponent<T>(trans.parent);
    }

    internal static IEnumerable<string> GetAllScriptGUIDs()
    {
      return ((IEnumerable<string>) AssetDatabase.GetAllAssetPaths()).Where<string>((Func<string, bool>) (asset => InternalEditorUtility.IsScriptOrAssembly(asset))).Select<string, string>((Func<string, string>) (asset => AssetDatabase.AssetPathToGUID(asset)));
    }

    public enum HierarchyDropMode
    {
      kHierarchyDragNormal = 0,
      kHierarchyDropUpon = 1,
      kHierarchyDropBetween = 2,
      kHierarchyDropAfterParent = 4,
    }
  }
}
