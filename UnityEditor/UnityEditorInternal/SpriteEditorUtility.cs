// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.SpriteEditorUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal static class SpriteEditorUtility
  {
    public static Vector2 GetPivotValue(SpriteAlignment alignment, Vector2 customOffset)
    {
      switch (alignment)
      {
        case SpriteAlignment.Center:
          return new Vector2(0.5f, 0.5f);
        case SpriteAlignment.TopLeft:
          return new Vector2(0.0f, 1f);
        case SpriteAlignment.TopCenter:
          return new Vector2(0.5f, 1f);
        case SpriteAlignment.TopRight:
          return new Vector2(1f, 1f);
        case SpriteAlignment.LeftCenter:
          return new Vector2(0.0f, 0.5f);
        case SpriteAlignment.RightCenter:
          return new Vector2(1f, 0.5f);
        case SpriteAlignment.BottomLeft:
          return new Vector2(0.0f, 0.0f);
        case SpriteAlignment.BottomCenter:
          return new Vector2(0.5f, 0.0f);
        case SpriteAlignment.BottomRight:
          return new Vector2(1f, 0.0f);
        case SpriteAlignment.Custom:
          return customOffset;
        default:
          return Vector2.zero;
      }
    }

    public static Rect RoundedRect(Rect rect)
    {
      return new Rect((float) Mathf.RoundToInt(rect.xMin), (float) Mathf.RoundToInt(rect.yMin), (float) Mathf.RoundToInt(rect.width), (float) Mathf.RoundToInt(rect.height));
    }

    public static Rect RoundToInt(Rect r)
    {
      r.xMin = (float) Mathf.RoundToInt(r.xMin);
      r.yMin = (float) Mathf.RoundToInt(r.yMin);
      r.xMax = (float) Mathf.RoundToInt(r.xMax);
      r.yMax = (float) Mathf.RoundToInt(r.yMax);
      return r;
    }

    public static Rect ClampedRect(Rect rect, Rect clamp, bool maintainSize)
    {
      Rect rect1 = new Rect(rect);
      if (maintainSize)
      {
        Vector2 center = rect.center;
        if ((double) center.x + (double) Mathf.Abs(rect.width) * 0.5 > (double) clamp.xMax)
          center.x = clamp.xMax - rect.width * 0.5f;
        if ((double) center.x - (double) Mathf.Abs(rect.width) * 0.5 < (double) clamp.xMin)
          center.x = clamp.xMin + rect.width * 0.5f;
        if ((double) center.y + (double) Mathf.Abs(rect.height) * 0.5 > (double) clamp.yMax)
          center.y = clamp.yMax - rect.height * 0.5f;
        if ((double) center.y - (double) Mathf.Abs(rect.height) * 0.5 < (double) clamp.yMin)
          center.y = clamp.yMin + rect.height * 0.5f;
        rect1.center = center;
      }
      else
      {
        if ((double) rect1.width > 0.0)
        {
          rect1.xMin = Mathf.Max(rect.xMin, clamp.xMin);
          rect1.xMax = Mathf.Min(rect.xMax, clamp.xMax);
        }
        else
        {
          rect1.xMin = Mathf.Min(rect.xMin, clamp.xMax);
          rect1.xMax = Mathf.Max(rect.xMax, clamp.xMin);
        }
        if ((double) rect1.height > 0.0)
        {
          rect1.yMin = Mathf.Max(rect.yMin, clamp.yMin);
          rect1.yMax = Mathf.Min(rect.yMax, clamp.yMax);
        }
        else
        {
          rect1.yMin = Mathf.Min(rect.yMin, clamp.yMax);
          rect1.yMax = Mathf.Max(rect.yMax, clamp.yMin);
        }
      }
      rect1.width = Mathf.Abs(rect1.width);
      rect1.height = Mathf.Abs(rect1.height);
      return rect1;
    }

    public static void DrawBox(Rect position)
    {
      Vector3[] vector3Array1 = new Vector3[5];
      int num1 = 0;
      Vector3[] vector3Array2 = vector3Array1;
      int index1 = num1;
      int num2 = 1;
      int num3 = index1 + num2;
      vector3Array2[index1] = new Vector3(position.xMin, position.yMin, 0.0f);
      Vector3[] vector3Array3 = vector3Array1;
      int index2 = num3;
      int num4 = 1;
      int num5 = index2 + num4;
      vector3Array3[index2] = new Vector3(position.xMax, position.yMin, 0.0f);
      Vector3[] vector3Array4 = vector3Array1;
      int index3 = num5;
      int num6 = 1;
      int num7 = index3 + num6;
      vector3Array4[index3] = new Vector3(position.xMax, position.yMax, 0.0f);
      Vector3[] vector3Array5 = vector3Array1;
      int index4 = num7;
      int num8 = 1;
      int num9 = index4 + num8;
      vector3Array5[index4] = new Vector3(position.xMin, position.yMax, 0.0f);
      SpriteEditorUtility.DrawLine(vector3Array1[0], vector3Array1[1]);
      SpriteEditorUtility.DrawLine(vector3Array1[1], vector3Array1[2]);
      SpriteEditorUtility.DrawLine(vector3Array1[2], vector3Array1[3]);
      SpriteEditorUtility.DrawLine(vector3Array1[3], vector3Array1[0]);
    }

    public static void DrawLine(Vector3 p1, Vector3 p2)
    {
      GL.Vertex(p1);
      GL.Vertex(p2);
    }

    public static void BeginLines(Color color)
    {
      HandleUtility.ApplyWireMaterial();
      GL.PushMatrix();
      GL.MultMatrix(Handles.matrix);
      GL.Begin(1);
      GL.Color(color);
    }

    public static void EndLines()
    {
      GL.End();
      GL.PopMatrix();
    }
  }
}
