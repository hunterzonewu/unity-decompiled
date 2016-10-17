// Decompiled with JetBrains decompiler
// Type: UnityEditor.PackageImport
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  internal class PackageImport : EditorWindow
  {
    private static readonly char[] s_InvalidPathChars = Path.GetInvalidPathChars();
    [SerializeField]
    private ImportPackageItem[] m_ImportPackageItems;
    [SerializeField]
    private string m_PackageName;
    [SerializeField]
    private string m_PackageIconPath;
    [SerializeField]
    private TreeViewState m_TreeViewState;
    [NonSerialized]
    private PackageImportTreeView m_Tree;
    private bool m_ShowReInstall;
    private bool m_ReInstallPackage;
    private static Texture2D s_PackageIcon;
    private static Texture2D s_Preview;
    private static string s_LastPreviewPath;
    private static PackageImport.Constants ms_Constants;

    public bool canReInstall
    {
      get
      {
        return this.m_ShowReInstall;
      }
    }

    public bool doReInstall
    {
      get
      {
        if (this.m_ShowReInstall)
          return this.m_ReInstallPackage;
        return false;
      }
    }

    public ImportPackageItem[] packageItems
    {
      get
      {
        return this.m_ImportPackageItems;
      }
    }

    public PackageImport()
    {
      this.minSize = new Vector2(350f, 350f);
    }

    public static void ShowImportPackage(string packagePath, ImportPackageItem[] items, string packageIconPath, bool allowReInstall)
    {
      if (!PackageImport.ValidateInput(items))
        return;
      EditorWindow.GetWindow<PackageImport>(true, "Import Unity Package").Init(packagePath, items, packageIconPath, allowReInstall);
    }

    private void OnDisable()
    {
      this.DestroyCreatedIcons();
    }

    private void DestroyCreatedIcons()
    {
      if ((UnityEngine.Object) PackageImport.s_Preview != (UnityEngine.Object) null)
      {
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) PackageImport.s_Preview);
        PackageImport.s_Preview = (Texture2D) null;
        PackageImport.s_LastPreviewPath = (string) null;
      }
      if (!((UnityEngine.Object) PackageImport.s_PackageIcon != (UnityEngine.Object) null))
        return;
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) PackageImport.s_PackageIcon);
      PackageImport.s_PackageIcon = (Texture2D) null;
    }

    private void Init(string packagePath, ImportPackageItem[] items, string packageIconPath, bool allowReInstall)
    {
      this.DestroyCreatedIcons();
      this.m_ShowReInstall = allowReInstall;
      this.m_ReInstallPackage = true;
      this.m_TreeViewState = (TreeViewState) null;
      this.m_Tree = (PackageImportTreeView) null;
      this.m_ImportPackageItems = items;
      this.m_PackageName = Path.GetFileNameWithoutExtension(packagePath);
      this.m_PackageIconPath = packageIconPath;
      this.Repaint();
    }

    private bool ShowTreeGUI(bool reInstalling, ImportPackageItem[] items)
    {
      if (reInstalling)
        return true;
      if (items.Length == 0)
        return false;
      for (int index = 0; index < items.Length; ++index)
      {
        if (!items[index].isFolder && items[index].assetChanged)
          return true;
      }
      return false;
    }

    public void OnGUI()
    {
      if (PackageImport.ms_Constants == null)
        PackageImport.ms_Constants = new PackageImport.Constants();
      if (this.m_TreeViewState == null)
        this.m_TreeViewState = new TreeViewState();
      if (this.m_Tree == null)
        this.m_Tree = new PackageImportTreeView(this, this.m_TreeViewState, new Rect());
      if (this.m_ImportPackageItems != null && this.ShowTreeGUI(this.doReInstall, this.m_ImportPackageItems))
      {
        this.TopArea();
        this.m_Tree.OnGUI(GUILayoutUtility.GetRect(1f, 9999f, 1f, 99999f));
        this.BottomArea();
      }
      else
      {
        GUILayout.Label("Nothing to import!", EditorStyles.boldLabel, new GUILayoutOption[0]);
        GUILayout.Label("All assets from this package are already in your project.", (GUIStyle) "WordWrappedLabel", new GUILayoutOption[0]);
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(PackageImport.ms_Constants.bottomBarBg, new GUILayoutOption[0]);
        GUILayout.Space(8f);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        this.ReInstallToggle();
        if (GUILayout.Button("OK"))
        {
          this.Close();
          GUIUtility.ExitGUI();
        }
        GUILayout.Space(10f);
        GUILayout.EndHorizontal();
        GUILayout.Space(5f);
        GUILayout.EndVertical();
      }
    }

    private void ReInstallToggle()
    {
      if (!this.m_ShowReInstall)
        return;
      EditorGUI.BeginChangeCheck();
      bool flag = GUILayout.Toggle(this.m_ReInstallPackage, "Re-Install Package");
      if (!EditorGUI.EndChangeCheck())
        return;
      this.m_ReInstallPackage = flag;
    }

    private void TopArea()
    {
      if ((UnityEngine.Object) PackageImport.s_PackageIcon == (UnityEngine.Object) null && !string.IsNullOrEmpty(this.m_PackageIconPath))
        PackageImport.LoadTexture(this.m_PackageIconPath, ref PackageImport.s_PackageIcon);
      bool flag = (UnityEngine.Object) PackageImport.s_PackageIcon != (UnityEngine.Object) null;
      Rect rect = GUILayoutUtility.GetRect(this.position.width, !flag ? 52f : 84f);
      GUI.Label(rect, GUIContent.none, PackageImport.ms_Constants.topBarBg);
      Rect position;
      if (flag)
      {
        Rect r = new Rect(rect.x + 10f, rect.y + 10f, 64f, 64f);
        PackageImport.DrawTexture(r, PackageImport.s_PackageIcon, true);
        position = new Rect(r.xMax + 10f, r.yMin, rect.width, r.height);
      }
      else
        position = new Rect(rect.x + 5f, rect.yMin, rect.width, rect.height);
      GUI.Label(position, this.m_PackageName, PackageImport.ms_Constants.title);
    }

    private void BottomArea()
    {
      GUILayout.BeginVertical(PackageImport.ms_Constants.bottomBarBg, new GUILayoutOption[0]);
      GUILayout.Space(8f);
      GUILayout.BeginHorizontal();
      GUILayout.Space(10f);
      if (GUILayout.Button(EditorGUIUtility.TextContent("All"), new GUILayoutOption[1]{ GUILayout.Width(50f) }))
        this.m_Tree.SetAllEnabled(PackageImportTreeView.EnabledState.All);
      if (GUILayout.Button(EditorGUIUtility.TextContent("None"), new GUILayoutOption[1]{ GUILayout.Width(50f) }))
        this.m_Tree.SetAllEnabled(PackageImportTreeView.EnabledState.None);
      this.ReInstallToggle();
      GUILayout.FlexibleSpace();
      if (GUILayout.Button(EditorGUIUtility.TextContent("Cancel")))
      {
        PopupWindowWithoutFocus.Hide();
        this.Close();
        GUIUtility.ExitGUI();
      }
      if (GUILayout.Button(EditorGUIUtility.TextContent("Import")))
      {
        bool flag = true;
        if (this.doReInstall)
          flag = EditorUtility.DisplayDialog("Re-Install?", "Highlighted folders will be completely deleted first! Recommend backing up your project first. Are you sure?", "Do It", "Cancel");
        if (flag)
        {
          if (this.m_ImportPackageItems != null)
            PackageUtility.ImportPackageAssets(this.m_ImportPackageItems, this.doReInstall);
          PopupWindowWithoutFocus.Hide();
          this.Close();
          GUIUtility.ExitGUI();
        }
      }
      GUILayout.Space(10f);
      GUILayout.EndHorizontal();
      GUILayout.Space(5f);
      GUILayout.EndVertical();
    }

    private static void LoadTexture(string filepath, ref Texture2D texture)
    {
      if (!(bool) ((UnityEngine.Object) texture))
        texture = new Texture2D(128, 128);
      byte[] data = (byte[]) null;
      try
      {
        data = File.ReadAllBytes(filepath);
      }
      catch
      {
      }
      if (!(filepath == string.Empty) && data != null && texture.LoadImage(data))
        return;
      Color[] pixels = texture.GetPixels();
      for (int index = 0; index < pixels.Length; ++index)
        pixels[index] = new Color(0.5f, 0.5f, 0.5f, 0.0f);
      texture.SetPixels(pixels);
      texture.Apply();
    }

    public static void DrawTexture(Rect r, Texture2D tex, bool useDropshadow)
    {
      if ((UnityEngine.Object) tex == (UnityEngine.Object) null)
        return;
      float width = (float) tex.width;
      float height = (float) tex.height;
      if ((double) width >= (double) height && (double) width > (double) r.width)
      {
        height = height * r.width / width;
        width = r.width;
      }
      else if ((double) height > (double) width && (double) height > (double) r.height)
      {
        width = width * r.height / height;
        height = r.height;
      }
      r = new Rect(r.x + Mathf.Round((float) (((double) r.width - (double) width) / 2.0)), r.y + Mathf.Round((float) (((double) r.height - (double) height) / 2.0)), width, height);
      if (useDropshadow && Event.current.type == EventType.Repaint)
      {
        Rect position = new RectOffset(1, 1, 1, 1).Remove(PackageImport.ms_Constants.textureIconDropShadow.border.Add(r));
        PackageImport.ms_Constants.textureIconDropShadow.Draw(position, GUIContent.none, false, false, false, false);
      }
      GUI.DrawTexture(r, (Texture) tex, ScaleMode.ScaleToFit, true);
    }

    public static Texture2D GetPreview(string previewPath)
    {
      if (previewPath != PackageImport.s_LastPreviewPath)
      {
        PackageImport.s_LastPreviewPath = previewPath;
        PackageImport.LoadTexture(previewPath, ref PackageImport.s_Preview);
      }
      return PackageImport.s_Preview;
    }

    private static bool ValidateInput(ImportPackageItem[] items)
    {
      string errorMessage;
      if (!PackageImport.IsAllFilePathsValid(items, out errorMessage))
        return EditorUtility.DisplayDialog("Invalid file path found", errorMessage + "\nDo you want to import the valid file paths of the package or cancel importing?", "Import", "Cancel importing");
      return true;
    }

    private static bool IsAllFilePathsValid(ImportPackageItem[] assetItems, out string errorMessage)
    {
      foreach (ImportPackageItem assetItem in assetItems)
      {
        char invalidChar;
        int invalidCharIndex;
        if (!assetItem.isFolder && PackageImport.HasInvalidCharInFilePath(assetItem.destinationAssetPath, out invalidChar, out invalidCharIndex))
        {
          errorMessage = string.Format("Invalid character found in file path: '{0}'. Invalid ascii value: {1} (at character index {2}).", (object) assetItem.destinationAssetPath, (object) (int) invalidChar, (object) invalidCharIndex);
          return false;
        }
      }
      errorMessage = string.Empty;
      return true;
    }

    private static bool HasInvalidCharInFilePath(string filePath, out char invalidChar, out int invalidCharIndex)
    {
      for (int index = 0; index < filePath.Length; ++index)
      {
        char ch = filePath[index];
        if (((IEnumerable<char>) PackageImport.s_InvalidPathChars).Contains<char>(ch))
        {
          invalidChar = ch;
          invalidCharIndex = index;
          return true;
        }
      }
      invalidChar = ' ';
      invalidCharIndex = -1;
      return false;
    }

    public static bool HasInvalidCharInFilePath(string filePath)
    {
      char invalidChar;
      int invalidCharIndex;
      return PackageImport.HasInvalidCharInFilePath(filePath, out invalidChar, out invalidCharIndex);
    }

    internal class Constants
    {
      public GUIStyle ConsoleEntryBackEven = (GUIStyle) "CN EntryBackEven";
      public GUIStyle ConsoleEntryBackOdd = (GUIStyle) "CN EntryBackOdd";
      public GUIStyle title = new GUIStyle(EditorStyles.largeLabel);
      public GUIStyle bottomBarBg = (GUIStyle) "ProjectBrowserBottomBarBg";
      public GUIStyle topBarBg = new GUIStyle((GUIStyle) "ProjectBrowserHeaderBgTop");
      public GUIStyle textureIconDropShadow = (GUIStyle) "ProjectBrowserTextureIconDropShadow";
      public Color lineColor;

      public Constants()
      {
        this.lineColor = !EditorGUIUtility.isProSkin ? new Color(0.4f, 0.4f, 0.4f) : new Color(0.1f, 0.1f, 0.1f);
        this.topBarBg.fixedHeight = 0.0f;
        RectOffset border = this.topBarBg.border;
        int num1 = 2;
        this.topBarBg.border.bottom = num1;
        int num2 = num1;
        border.top = num2;
        this.title.fontStyle = FontStyle.Bold;
        this.title.alignment = TextAnchor.MiddleLeft;
      }
    }
  }
}
