// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AssetStoreToolUtils
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  public sealed class AssetStoreToolUtils
  {
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool BuildAssetStoreAssetBundle(Object targetObject, string targetPath);

    public static bool PreviewAssetStoreAssetBundleInInspector(AssetBundle bundle, AssetStoreAsset info)
    {
      info.id = 0;
      info.previewAsset = bundle.mainAsset;
      AssetStoreAssetSelection.Clear();
      AssetStoreAssetSelection.AddAssetInternal(info);
      Selection.activeObject = (Object) AssetStoreAssetInspector.Instance;
      AssetStoreAssetInspector.Instance.Repaint();
      return true;
    }

    public static void UpdatePreloadingInternal()
    {
      AssetStoreUtils.UpdatePreloading();
    }
  }
}
