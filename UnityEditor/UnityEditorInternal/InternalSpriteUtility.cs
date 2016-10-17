// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.InternalSpriteUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditorInternal
{
  public sealed class InternalSpriteUtility
  {
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Rect[] GenerateAutomaticSpriteRectangles(Texture2D texture, int minRectSize, int extrudeSize);

    public static Rect[] GenerateGridSpriteRectangles(Texture2D texture, Vector2 offset, Vector2 size, Vector2 padding)
    {
      return InternalSpriteUtility.INTERNAL_CALL_GenerateGridSpriteRectangles(texture, ref offset, ref size, ref padding);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Rect[] INTERNAL_CALL_GenerateGridSpriteRectangles(Texture2D texture, ref Vector2 offset, ref Vector2 size, ref Vector2 padding);
  }
}
