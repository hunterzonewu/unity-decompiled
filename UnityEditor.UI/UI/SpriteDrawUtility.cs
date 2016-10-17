// Decompiled with JetBrains decompiler
// Type: UnityEditor.UI.SpriteDrawUtility
// Assembly: UnityEditor.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02C415E4-CA90-4A5B-B8EB-A397F164EE08
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.UI.dll

using UnityEngine;
using UnityEngine.Sprites;

namespace UnityEditor.UI
{
  internal class SpriteDrawUtility
  {
    private static Texture2D s_ContrastTex;

    private static Texture2D contrastTexture
    {
      get
      {
        if ((Object) SpriteDrawUtility.s_ContrastTex == (Object) null)
          SpriteDrawUtility.s_ContrastTex = SpriteDrawUtility.CreateCheckerTex(new Color(0.0f, 0.0f, 0.0f, 0.5f), new Color(1f, 1f, 1f, 0.5f));
        return SpriteDrawUtility.s_ContrastTex;
      }
    }

    private static Texture2D CreateCheckerTex(Color c0, Color c1)
    {
      Texture2D texture2D = new Texture2D(16, 16);
      texture2D.name = "[Generated] Checker Texture";
      texture2D.hideFlags = HideFlags.DontSave;
      for (int y = 0; y < 8; ++y)
      {
        for (int x = 0; x < 8; ++x)
          texture2D.SetPixel(x, y, c1);
      }
      for (int y = 8; y < 16; ++y)
      {
        for (int x = 0; x < 8; ++x)
          texture2D.SetPixel(x, y, c0);
      }
      for (int y = 0; y < 8; ++y)
      {
        for (int x = 8; x < 16; ++x)
          texture2D.SetPixel(x, y, c0);
      }
      for (int y = 8; y < 16; ++y)
      {
        for (int x = 8; x < 16; ++x)
          texture2D.SetPixel(x, y, c1);
      }
      texture2D.Apply();
      texture2D.filterMode = UnityEngine.FilterMode.Point;
      return texture2D;
    }

    private static Texture2D CreateGradientTex()
    {
      Texture2D texture2D = new Texture2D(1, 16);
      texture2D.name = "[Generated] Gradient Texture";
      texture2D.hideFlags = HideFlags.DontSave;
      Color a = new Color(1f, 1f, 1f, 0.0f);
      Color b = new Color(1f, 1f, 1f, 0.4f);
      for (int y = 0; y < 16; ++y)
      {
        float num = Mathf.Abs((float) ((double) y / 15.0 * 2.0 - 1.0));
        float t = num * num;
        texture2D.SetPixel(0, y, Color.Lerp(a, b, t));
      }
      texture2D.Apply();
      texture2D.filterMode = UnityEngine.FilterMode.Bilinear;
      return texture2D;
    }

    private static void DrawTiledTexture(Rect rect, Texture tex)
    {
      Rect texCoords = new Rect(0.0f, 0.0f, rect.width / (float) tex.width, rect.height / (float) tex.height);
      TextureWrapMode wrapMode = tex.wrapMode;
      tex.wrapMode = TextureWrapMode.Repeat;
      GUI.DrawTextureWithTexCoords(rect, tex, texCoords);
      tex.wrapMode = wrapMode;
    }

    public static void DrawSprite(Sprite sprite, Rect drawArea, Color color)
    {
      if ((Object) sprite == (Object) null)
        return;
      Texture2D texture = sprite.texture;
      if ((Object) texture == (Object) null)
        return;
      Rect rect = sprite.rect;
      Rect inner = rect;
      inner.xMin += sprite.border.x;
      inner.yMin += sprite.border.y;
      inner.xMax -= sprite.border.z;
      inner.yMax -= sprite.border.w;
      Vector4 outerUv = DataUtility.GetOuterUV(sprite);
      Rect uv = new Rect(outerUv.x, outerUv.y, outerUv.z - outerUv.x, outerUv.w - outerUv.y);
      Vector4 padding = DataUtility.GetPadding(sprite);
      padding.x /= rect.width;
      padding.y /= rect.height;
      padding.z /= rect.width;
      padding.w /= rect.height;
      SpriteDrawUtility.DrawSprite((Texture) texture, drawArea, padding, rect, inner, uv, color, (Material) null);
    }

    public static void DrawSprite(Texture tex, Rect drawArea, Rect outer, Rect uv, Color color)
    {
      SpriteDrawUtility.DrawSprite(tex, drawArea, Vector4.zero, outer, outer, uv, color, (Material) null);
    }

    private static void DrawSprite(Texture tex, Rect drawArea, Vector4 padding, Rect outer, Rect inner, Rect uv, Color color, Material mat)
    {
      Rect position1 = drawArea;
      position1.width = Mathf.Abs(outer.width);
      position1.height = Mathf.Abs(outer.height);
      if ((double) position1.width > 0.0)
      {
        float num = drawArea.width / position1.width;
        position1.width *= num;
        position1.height *= num;
      }
      if ((double) drawArea.height > (double) position1.height)
        position1.y += (float) (((double) drawArea.height - (double) position1.height) * 0.5);
      else if ((double) position1.height > (double) drawArea.height)
      {
        float num = drawArea.height / position1.height;
        position1.width *= num;
        position1.height *= num;
      }
      if ((double) drawArea.width > (double) position1.width)
        position1.x += (float) (((double) drawArea.width - (double) position1.width) * 0.5);
      EditorGUI.DrawTextureTransparent(position1, (Texture) null, ScaleMode.ScaleToFit, outer.width / outer.height);
      GUI.color = color;
      Rect position2 = new Rect(position1.x + position1.width * padding.x, position1.y + position1.height * padding.w, position1.width - position1.width * (padding.z + padding.x), position1.height - position1.height * (padding.w + padding.y));
      if ((Object) mat == (Object) null)
        GUI.DrawTextureWithTexCoords(position2, tex, uv, true);
      else
        EditorGUI.DrawPreviewTexture(position2, tex, mat);
      GUI.BeginGroup(position1);
      tex = (Texture) SpriteDrawUtility.contrastTexture;
      GUI.color = Color.white;
      if ((double) inner.xMin != (double) outer.xMin)
        SpriteDrawUtility.DrawTiledTexture(new Rect((float) (((double) inner.xMin - (double) outer.xMin) / (double) outer.width * (double) position1.width - 1.0), 0.0f, 1f, position1.height), tex);
      if ((double) inner.xMax != (double) outer.xMax)
        SpriteDrawUtility.DrawTiledTexture(new Rect((float) (((double) inner.xMax - (double) outer.xMin) / (double) outer.width * (double) position1.width - 1.0), 0.0f, 1f, position1.height), tex);
      if ((double) inner.yMin != (double) outer.yMin)
      {
        float num = (float) (((double) inner.yMin - (double) outer.yMin) / (double) outer.height * (double) position1.height - 1.0);
        SpriteDrawUtility.DrawTiledTexture(new Rect(0.0f, position1.height - num, position1.width, 1f), tex);
      }
      if ((double) inner.yMax != (double) outer.yMax)
      {
        float num = (float) (((double) inner.yMax - (double) outer.yMin) / (double) outer.height * (double) position1.height - 1.0);
        SpriteDrawUtility.DrawTiledTexture(new Rect(0.0f, position1.height - num, position1.width, 1f), tex);
      }
      GUI.EndGroup();
    }
  }
}
