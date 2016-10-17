// Decompiled with JetBrains decompiler
// Type: UnityEditor.HierarchyProperty
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityEditor
{
  public sealed class HierarchyProperty : IHierarchyProperty
  {
    private IntPtr m_Property;

    public int instanceID { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public UnityEngine.Object pptrValue { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public string name { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public bool hasChildren { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public int depth { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public int[] ancestors { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public int row { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public int colorCode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public string guid { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public bool alphaSorted { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public bool isValid { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public bool isMainRepresentation { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public bool hasFullPreviewImage { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public IconDrawStyle iconDrawStyle { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public bool isFolder { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public Texture2D icon { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public HierarchyProperty(HierarchyType hierarchytType);

    ~HierarchyProperty()
    {
      this.Dispose();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Reset();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Dispose();

    public Scene GetScene()
    {
      Scene scene;
      HierarchyProperty.INTERNAL_CALL_GetScene(this, out scene);
      return scene;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetScene(HierarchyProperty self, out Scene value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool IsExpanded(int[] expanded);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool Next(int[] expanded);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool NextWithDepthCheck(int[] expanded, int minDepth);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool Previous(int[] expanded);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool Parent();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool Find(int instanceID, int[] expanded);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool Skip(int count, int[] expanded);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public int CountRemaining(int[] expanded);

    public void SetSearchFilter(string searchString, int mode)
    {
      this.SetSearchFilter(SearchableEditorWindow.CreateFilter(searchString, (SearchableEditorWindow.SearchMode) mode));
    }

    internal void SetSearchFilter(SearchFilter filter)
    {
      this.SetSearchFilterINTERNAL(SearchFilter.Split(filter.nameFilter), filter.classNames, filter.assetLabels, filter.assetBundleNames, filter.referencingInstanceIDs, filter.showAllHits);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void SetSearchFilterINTERNAL(string[] nameFilters, string[] classNames, string[] assetLabels, string[] assetBundleNames, int[] referencingInstanceIDs, bool showAllHits);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public int[] FindAllAncestors(int[] instanceIDs);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void FilterSingleSceneObject(int instanceID, bool otherVisibilityState);
  }
}
