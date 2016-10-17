// Decompiled with JetBrains decompiler
// Type: UnityEditor.SavedRenderTargetState
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class SavedRenderTargetState
  {
    private RenderTexture renderTexture;
    private Rect viewport;
    private Rect scissor;

    internal SavedRenderTargetState()
    {
      GL.PushMatrix();
      if (ShaderUtil.hardwareSupportsRectRenderTexture)
        this.renderTexture = RenderTexture.active;
      this.viewport = ShaderUtil.rawViewportRect;
      this.scissor = ShaderUtil.rawScissorRect;
    }

    internal void Restore()
    {
      if (ShaderUtil.hardwareSupportsRectRenderTexture)
        EditorGUIUtility.SetRenderTextureNoViewport(this.renderTexture);
      ShaderUtil.rawViewportRect = this.viewport;
      ShaderUtil.rawScissorRect = this.scissor;
      GL.PopMatrix();
    }
  }
}
