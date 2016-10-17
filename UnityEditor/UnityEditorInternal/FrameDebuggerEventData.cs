// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.FrameDebuggerEventData
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditorInternal
{
  internal struct FrameDebuggerEventData
  {
    public int frameEventIndex;
    public int vertexCount;
    public int indexCount;
    public string shaderName;
    public Shader shader;
    public int shaderInstanceID;
    public int shaderPassIndex;
    public string shaderKeywords;
    public int rendererInstanceID;
    public Mesh mesh;
    public int meshInstanceID;
    public int meshSubset;
    public string rtName;
    public int rtWidth;
    public int rtHeight;
    public int rtFormat;
    public int rtDim;
    public int rtFace;
    public short rtCount;
    public short rtHasDepthTexture;
    public FrameDebuggerBlendState blendState;
    public FrameDebuggerRasterState rasterState;
    public FrameDebuggerDepthState depthState;
    public ShaderProperties shaderProperties;
  }
}
