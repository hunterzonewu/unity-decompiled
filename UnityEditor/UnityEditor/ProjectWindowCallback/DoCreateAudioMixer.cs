// Decompiled with JetBrains decompiler
// Type: UnityEditor.ProjectWindowCallback.DoCreateAudioMixer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor.Audio;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Audio;

namespace UnityEditor.ProjectWindowCallback
{
  internal class DoCreateAudioMixer : EndNameEditAction
  {
    public override void Action(int instanceId, string pathName, string resourceFile)
    {
      AudioMixerController controllerAtPath = AudioMixerController.CreateMixerControllerAtPath(pathName);
      int result;
      if (!string.IsNullOrEmpty(resourceFile) && int.TryParse(resourceFile, out result))
      {
        AudioMixerGroupController objectFromInstanceId = InternalEditorUtility.GetObjectFromInstanceID(result) as AudioMixerGroupController;
        if ((Object) objectFromInstanceId != (Object) null)
          controllerAtPath.outputAudioMixerGroup = (AudioMixerGroup) objectFromInstanceId;
      }
      ProjectWindowUtil.ShowCreatedAsset((Object) controllerAtPath);
    }
  }
}
