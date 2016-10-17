// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.PreviewGenerator
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.Web
{
  internal class PreviewGenerator
  {
    private const string kPreviewBuildFolder = "builds";
    protected static PreviewGenerator s_Instance;

    public static PreviewGenerator GetInstance()
    {
      if (PreviewGenerator.s_Instance == null)
        return new PreviewGenerator();
      return PreviewGenerator.s_Instance;
    }

    public byte[] GeneratePreview(string assetPath, int width, int height)
    {
      Object targetObject = AssetDatabase.LoadMainAssetAtPath(assetPath);
      if (targetObject == (Object) null)
        return (byte[]) null;
      Editor editor = Editor.CreateEditor(targetObject);
      if ((Object) editor == (Object) null)
        return (byte[]) null;
      Texture2D texture2D = editor.RenderStaticPreview(assetPath, (Object[]) null, width, height);
      if ((Object) texture2D == (Object) null)
      {
        Object.DestroyImmediate((Object) editor);
        return (byte[]) null;
      }
      byte[] png = texture2D.EncodeToPNG();
      Object.DestroyImmediate((Object) texture2D);
      Object.DestroyImmediate((Object) editor);
      return png;
    }
  }
}
