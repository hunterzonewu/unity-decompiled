// Decompiled with JetBrains decompiler
// Type: UnityEditor.SpriteEditorTexturePostprocessor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class SpriteEditorTexturePostprocessor : AssetPostprocessor
  {
    public override int GetPostprocessOrder()
    {
      return 1;
    }

    public void OnPostprocessTexture(Texture2D tex)
    {
      if (!((Object) SpriteEditorWindow.s_Instance != (Object) null) || !this.assetPath.Equals(SpriteEditorWindow.s_Instance.m_SelectedAssetPath))
        return;
      if (!SpriteEditorWindow.s_Instance.m_IgnoreNextPostprocessEvent)
      {
        SpriteEditorWindow.s_Instance.m_ResetOnNextRepaint = true;
        SpriteEditorWindow.s_Instance.Repaint();
      }
      else
        SpriteEditorWindow.s_Instance.m_IgnoreNextPostprocessEvent = false;
    }
  }
}
