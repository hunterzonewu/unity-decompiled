// Decompiled with JetBrains decompiler
// Type: UnityEditor.TargetGlesGraphics
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;

namespace UnityEditor
{
  [Obsolete("TargetGlesGraphics is ignored, use SetGraphicsAPIs/GetGraphicsAPIs APIs")]
  public enum TargetGlesGraphics
  {
    Automatic = -1,
    [Obsolete("OpenGL ES 1.x is deprecated, ES 2.0 will be used instead")] OpenGLES_1_x = 0,
    OpenGLES_2_0 = 1,
    OpenGLES_3_0 = 2,
  }
}
