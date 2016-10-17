// Decompiled with JetBrains decompiler
// Type: UnityEditor.PostprocessScene
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor.Callbacks;
using UnityEngine;

namespace UnityEditor
{
  internal class PostprocessScene
  {
    internal class UnityBuildPostprocessor
    {
      [PostProcessScene(0)]
      public static void OnPostprocessScene()
      {
        int staticBatching;
        int dynamicBatching;
        PlayerSettings.GetBatchingForPlatform(EditorUserBuildSettings.activeBuildTarget, out staticBatching, out dynamicBatching);
        if (staticBatching == 0)
          return;
        InternalStaticBatchingUtility.Combine((GameObject) null, true, true);
      }
    }
  }
}
