// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.VersionControl.ProjectHooks
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor.VersionControl;
using UnityEngine;

namespace UnityEditorInternal.VersionControl
{
  internal class ProjectHooks
  {
    public static void OnProjectWindowItem(string guid, Rect drawRect)
    {
      if (!Provider.isActive)
        return;
      Asset assetByGuid = Provider.GetAssetByGUID(guid);
      if (assetByGuid == null)
        return;
      Asset assetByPath = Provider.GetAssetByPath(assetByGuid.path.Trim('/') + ".meta");
      Overlay.DrawOverlay(assetByGuid, assetByPath, drawRect);
    }

    public static Rect GetOverlayRect(Rect drawRect)
    {
      return Overlay.GetOverlayRect(drawRect);
    }
  }
}
