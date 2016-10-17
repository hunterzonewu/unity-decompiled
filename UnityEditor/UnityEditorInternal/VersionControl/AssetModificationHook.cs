// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.VersionControl.AssetModificationHook
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

namespace UnityEditorInternal.VersionControl
{
  public class AssetModificationHook
  {
    private static Asset GetStatusCachedIfPossible(string from)
    {
      Asset asset = Provider.CacheStatus(from);
      if (asset == null || asset.IsState(Asset.States.Updating))
      {
        Provider.Status(from, false).Wait();
        asset = Provider.CacheStatus(from);
      }
      return asset;
    }

    public static AssetMoveResult OnWillMoveAsset(string from, string to)
    {
      if (!Provider.enabled)
        return AssetMoveResult.DidNotMove;
      Asset cachedIfPossible = AssetModificationHook.GetStatusCachedIfPossible(from);
      if (cachedIfPossible == null || !cachedIfPossible.IsUnderVersionControl)
        return AssetMoveResult.DidNotMove;
      if (cachedIfPossible.IsState(Asset.States.OutOfSync))
      {
        Debug.LogError((object) "Cannot move version controlled file that is not up to date. Please get latest changes from server");
        return AssetMoveResult.FailedMove;
      }
      if (cachedIfPossible.IsState(Asset.States.DeletedRemote))
      {
        Debug.LogError((object) "Cannot move version controlled file that is deleted on server. Please get latest changes from server");
        return AssetMoveResult.FailedMove;
      }
      if (cachedIfPossible.IsState(Asset.States.CheckedOutRemote))
      {
        Debug.LogError((object) "Cannot move version controlled file that is checked out on server. Please get latest changes from server");
        return AssetMoveResult.FailedMove;
      }
      if (cachedIfPossible.IsState(Asset.States.LockedRemote))
      {
        Debug.LogError((object) "Cannot move version controlled file that is locked on server. Please get latest changes from server");
        return AssetMoveResult.FailedMove;
      }
      Task task = Provider.Move(from, to);
      task.Wait();
      if (task.success)
        return (AssetMoveResult) task.resultCode;
      return AssetMoveResult.FailedMove;
    }

    public static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions option)
    {
      if (!Provider.enabled)
        return AssetDeleteResult.DidNotDelete;
      Task task = Provider.Delete(assetPath);
      task.SetCompletionAction(CompletionAction.UpdatePendingWindow);
      task.Wait();
      return task.success ? AssetDeleteResult.DidNotDelete : AssetDeleteResult.FailedDelete;
    }

    public static bool IsOpenForEdit(string assetPath, out string message)
    {
      message = string.Empty;
      if (!Provider.enabled || string.IsNullOrEmpty(assetPath))
        return true;
      Asset asset = Provider.GetAssetByPath(assetPath);
      if (asset == null)
      {
        Task task = Provider.Status(assetPath, false);
        task.Wait();
        asset = task.assetList.Count <= 0 ? (Asset) null : task.assetList[0];
      }
      if (asset == null)
        return false;
      return Provider.IsOpenForEdit(asset);
    }
  }
}
