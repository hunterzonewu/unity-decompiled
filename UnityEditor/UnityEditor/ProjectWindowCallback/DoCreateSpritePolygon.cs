// Decompiled with JetBrains decompiler
// Type: UnityEditor.ProjectWindowCallback.DoCreateSpritePolygon
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor.ProjectWindowCallback
{
  internal class DoCreateSpritePolygon : EndNameEditAction
  {
    public int sides;

    public override void Action(int instanceId, string pathName, string resourceFile)
    {
      bool flag = false;
      if (this.sides < 0)
      {
        this.sides = 5;
        flag = true;
      }
      UnityEditor.Sprites.SpriteUtility.CreateSpritePolygonAssetAtPath(pathName, this.sides);
      if (!flag)
        return;
      Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(pathName);
      SpriteEditorWindow.GetWindow();
    }
  }
}
