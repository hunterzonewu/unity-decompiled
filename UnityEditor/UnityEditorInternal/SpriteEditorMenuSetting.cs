// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.SpriteEditorMenuSetting
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditorInternal
{
  [Serializable]
  internal class SpriteEditorMenuSetting : ScriptableObject
  {
    [SerializeField]
    public Vector2 gridCellCount = new Vector2(1f, 1f);
    [SerializeField]
    public Vector2 gridSpriteSize = new Vector2(64f, 64f);
    [SerializeField]
    public Vector2 gridSpriteOffset = new Vector2(0.0f, 0.0f);
    [SerializeField]
    public Vector2 gridSpritePadding = new Vector2(0.0f, 0.0f);
    [SerializeField]
    public Vector2 pivot = Vector2.zero;
    [SerializeField]
    public int autoSlicingMethod;
    [SerializeField]
    public int spriteAlignment;
    [SerializeField]
    public SpriteEditorMenuSetting.SlicingType slicingType;

    public enum SlicingType
    {
      Automatic,
      GridByCellSize,
      GridByCellCount,
    }
  }
}
