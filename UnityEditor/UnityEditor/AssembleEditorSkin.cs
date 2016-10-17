// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssembleEditorSkin
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor
{
  internal class AssembleEditorSkin : EditorWindow
  {
    public static void DoIt()
    {
      EditorApplication.ExecuteMenuItem("Tools/Regenerate Editor Skins Now");
    }

    private static void RegenerateAllIconsWithMipLevels()
    {
      GenerateIconsWithMipLevels.GenerateAllIconsWithMipLevels();
    }

    private static void RegenerateSelectedIconsWithMipLevels()
    {
      GenerateIconsWithMipLevels.GenerateSelectedIconsWithMips();
    }
  }
}
