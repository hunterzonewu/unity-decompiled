// Decompiled with JetBrains decompiler
// Type: UnityEditor.ProjectWindowCallback.DoCreateNewAsset
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor.ProjectWindowCallback
{
  internal class DoCreateNewAsset : EndNameEditAction
  {
    public override void Action(int instanceId, string pathName, string resourceFile)
    {
      AssetDatabase.CreateAsset(EditorUtility.InstanceIDToObject(instanceId), AssetDatabase.GenerateUniqueAssetPath(pathName));
      ProjectWindowUtil.FrameObjectInProjectWindow(instanceId);
    }
  }
}
