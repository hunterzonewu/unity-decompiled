// Decompiled with JetBrains decompiler
// Type: UnityEditor.ColorClipboard
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal static class ColorClipboard
  {
    public static void SetColor(Color color)
    {
      EditorGUIUtility.systemCopyBuffer = string.Empty;
      EditorGUIUtility.SetPasteboardColor(color);
    }

    public static bool HasColor()
    {
      Color color;
      return ColorClipboard.TryGetColor(false, out color);
    }

    public static bool TryGetColor(bool allowHDR, out Color color)
    {
      bool flag = false;
      if (ColorUtility.TryParseHtmlString(EditorGUIUtility.systemCopyBuffer, out color))
        flag = true;
      else if (EditorGUIUtility.HasPasteboardColor())
      {
        color = EditorGUIUtility.GetPasteboardColor();
        flag = true;
      }
      if (!flag)
        return false;
      if (!allowHDR && (double) color.maxColorComponent > 1.0)
        color = color.RGBMultiplied(1f / color.maxColorComponent);
      return true;
    }
  }
}
