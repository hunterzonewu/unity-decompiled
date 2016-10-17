// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.VersionControl.ProjectContextMenu
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor;
using UnityEditor.VersionControl;

namespace UnityEditorInternal.VersionControl
{
  public class ProjectContextMenu
  {
    private static bool GetLatestTest(MenuCommand cmd)
    {
      AssetList listFromSelection = Provider.GetAssetListFromSelection();
      if (Provider.enabled)
        return Provider.GetLatestIsValid(listFromSelection);
      return false;
    }

    private static void GetLatest(MenuCommand cmd)
    {
      Provider.GetLatest(Provider.GetAssetListFromSelection()).SetCompletionAction(CompletionAction.UpdatePendingWindow);
    }

    private static bool SubmitTest(MenuCommand cmd)
    {
      AssetList listFromSelection = Provider.GetAssetListFromSelection();
      if (Provider.enabled)
        return Provider.SubmitIsValid((ChangeSet) null, listFromSelection);
      return false;
    }

    private static void Submit(MenuCommand cmd)
    {
      WindowChange.Open(Provider.GetAssetListFromSelection(), true);
    }

    private static bool CheckOutTest(MenuCommand cmd)
    {
      AssetList listFromSelection = Provider.GetAssetListFromSelection();
      if (Provider.enabled)
        return Provider.CheckoutIsValid(listFromSelection, CheckoutMode.Both);
      return false;
    }

    private static void CheckOut(MenuCommand cmd)
    {
      Provider.Checkout(Provider.GetAssetListFromSelection(), CheckoutMode.Both);
    }

    private static bool CheckOutAssetTest(MenuCommand cmd)
    {
      AssetList listFromSelection = Provider.GetAssetListFromSelection();
      if (Provider.enabled)
        return Provider.CheckoutIsValid(listFromSelection, CheckoutMode.Asset);
      return false;
    }

    private static void CheckOutAsset(MenuCommand cmd)
    {
      Provider.Checkout(Provider.GetAssetListFromSelection(), CheckoutMode.Asset);
    }

    private static bool CheckOutMetaTest(MenuCommand cmd)
    {
      AssetList listFromSelection = Provider.GetAssetListFromSelection();
      if (Provider.enabled)
        return Provider.CheckoutIsValid(listFromSelection, CheckoutMode.Meta);
      return false;
    }

    private static void CheckOutMeta(MenuCommand cmd)
    {
      Provider.Checkout(Provider.GetAssetListFromSelection(), CheckoutMode.Meta);
    }

    private static bool CheckOutBothTest(MenuCommand cmd)
    {
      AssetList listFromSelection = Provider.GetAssetListFromSelection();
      if (Provider.enabled)
        return Provider.CheckoutIsValid(listFromSelection, CheckoutMode.Both);
      return false;
    }

    private static void CheckOutBoth(MenuCommand cmd)
    {
      Provider.Checkout(Provider.GetAssetListFromSelection(), CheckoutMode.Both);
    }

    private static bool MarkAddTest(MenuCommand cmd)
    {
      AssetList listFromSelection = Provider.GetAssetListFromSelection();
      if (Provider.enabled)
        return Provider.AddIsValid(listFromSelection);
      return false;
    }

    private static void MarkAdd(MenuCommand cmd)
    {
      Provider.Add(Provider.GetAssetListFromSelection(), true).SetCompletionAction(CompletionAction.UpdatePendingWindow);
    }

    private static bool RevertTest(MenuCommand cmd)
    {
      AssetList listFromSelection = Provider.GetAssetListFromSelection();
      if (Provider.enabled)
        return Provider.RevertIsValid(listFromSelection, RevertMode.Normal);
      return false;
    }

    private static void Revert(MenuCommand cmd)
    {
      WindowRevert.Open(Provider.GetAssetListFromSelection());
    }

    private static bool RevertUnchangedTest(MenuCommand cmd)
    {
      AssetList listFromSelection = Provider.GetAssetListFromSelection();
      if (Provider.enabled)
        return Provider.RevertIsValid(listFromSelection, RevertMode.Normal);
      return false;
    }

    private static void RevertUnchanged(MenuCommand cmd)
    {
      AssetList listFromSelection = Provider.GetAssetListFromSelection();
      Provider.Revert(listFromSelection, RevertMode.Unchanged).SetCompletionAction(CompletionAction.UpdatePendingWindow);
      Provider.Status(listFromSelection);
    }

    private static bool ResolveTest(MenuCommand cmd)
    {
      AssetList listFromSelection = Provider.GetAssetListFromSelection();
      if (Provider.enabled)
        return Provider.ResolveIsValid(listFromSelection);
      return false;
    }

    private static void Resolve(MenuCommand cmd)
    {
      WindowResolve.Open(Provider.GetAssetListFromSelection());
    }

    private static bool LockTest(MenuCommand cmd)
    {
      AssetList listFromSelection = Provider.GetAssetListFromSelection();
      if (Provider.enabled)
        return Provider.LockIsValid(listFromSelection);
      return false;
    }

    private static void Lock(MenuCommand cmd)
    {
      Provider.Lock(Provider.GetAssetListFromSelection(), true).SetCompletionAction(CompletionAction.UpdatePendingWindow);
    }

    private static bool UnlockTest(MenuCommand cmd)
    {
      AssetList listFromSelection = Provider.GetAssetListFromSelection();
      if (Provider.enabled)
        return Provider.UnlockIsValid(listFromSelection);
      return false;
    }

    private static void Unlock(MenuCommand cmd)
    {
      Provider.Lock(Provider.GetAssetListFromSelection(), false).SetCompletionAction(CompletionAction.UpdatePendingWindow);
    }

    private static bool DiffHeadTest(MenuCommand cmd)
    {
      AssetList listFromSelection = Provider.GetAssetListFromSelection();
      if (Provider.enabled)
        return Provider.DiffIsValid(listFromSelection);
      return false;
    }

    private static void DiffHead(MenuCommand cmd)
    {
      Provider.DiffHead(Provider.GetAssetListFromSelection(), false);
    }

    private static bool DiffHeadWithMetaTest(MenuCommand cmd)
    {
      AssetList listFromSelection = Provider.GetAssetListFromSelection();
      if (Provider.enabled)
        return Provider.DiffIsValid(listFromSelection);
      return false;
    }

    private static void DiffHeadWithMeta(MenuCommand cmd)
    {
      Provider.DiffHead(Provider.GetAssetListFromSelection(), true);
    }
  }
}
