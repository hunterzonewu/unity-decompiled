// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.FrameDebuggerBlendState
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine.Rendering;

namespace UnityEditorInternal
{
  internal struct FrameDebuggerBlendState
  {
    public uint renderTargetWriteMask;
    public BlendMode srcBlend;
    public BlendMode dstBlend;
    public BlendMode srcBlendAlpha;
    public BlendMode dstBlendAlpha;
    public BlendOp blendOp;
    public BlendOp blendOpAlpha;
  }
}
