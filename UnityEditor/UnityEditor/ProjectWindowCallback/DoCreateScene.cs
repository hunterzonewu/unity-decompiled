// Decompiled with JetBrains decompiler
// Type: UnityEditor.ProjectWindowCallback.DoCreateScene
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor.SceneManagement;

namespace UnityEditor.ProjectWindowCallback
{
  internal class DoCreateScene : EndNameEditAction
  {
    public override void Action(int instanceId, string pathName, string resourceFile)
    {
      bool createDefaultGameObjects = true;
      if (!EditorSceneManager.CreateSceneAsset(pathName, createDefaultGameObjects))
        return;
      ProjectWindowUtil.ShowCreatedAsset(AssetDatabase.LoadAssetAtPath(pathName, typeof (SceneAsset)));
    }
  }
}
