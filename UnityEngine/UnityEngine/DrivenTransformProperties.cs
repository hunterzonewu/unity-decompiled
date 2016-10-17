// Decompiled with JetBrains decompiler
// Type: UnityEngine.DrivenTransformProperties
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>An enumeration of transform properties that can be driven on a RectTransform by an object.</para>
  /// </summary>
  [Flags]
  public enum DrivenTransformProperties
  {
    None = 0,
    All = -1,
    AnchoredPositionX = 2,
    AnchoredPositionY = 4,
    AnchoredPositionZ = 8,
    Rotation = 16,
    ScaleX = 32,
    ScaleY = 64,
    ScaleZ = 128,
    AnchorMinX = 256,
    AnchorMinY = 512,
    AnchorMaxX = 1024,
    AnchorMaxY = 2048,
    SizeDeltaX = 4096,
    SizeDeltaY = 8192,
    PivotX = 16384,
    PivotY = 32768,
    AnchoredPosition = AnchoredPositionY | AnchoredPositionX,
    AnchoredPosition3D = AnchoredPosition | AnchoredPositionZ,
    Scale = ScaleZ | ScaleY | ScaleX,
    AnchorMin = AnchorMinY | AnchorMinX,
    AnchorMax = AnchorMaxY | AnchorMaxX,
    Anchors = AnchorMax | AnchorMin,
    SizeDelta = SizeDeltaY | SizeDeltaX,
    Pivot = PivotY | PivotX,
  }
}
