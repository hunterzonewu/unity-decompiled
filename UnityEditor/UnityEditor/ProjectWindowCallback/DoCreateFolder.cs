// Decompiled with JetBrains decompiler
// Type: UnityEditor.ProjectWindowCallback.DoCreateFolder
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.IO;
using UnityEngine;

namespace UnityEditor.ProjectWindowCallback
{
  internal class DoCreateFolder : EndNameEditAction
  {
    public override void Action(int instanceId, string pathName, string resourceFile)
    {
      ProjectWindowUtil.ShowCreatedAsset(AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.CreateFolder(Path.GetDirectoryName(pathName), Path.GetFileName(pathName))), typeof (Object)));
    }
  }
}
