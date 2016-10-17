// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioMixerInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;
using UnityEngine.Audio;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (AudioMixer))]
  internal class AudioMixerInspector : Editor
  {
    public override void OnInspectorGUI()
    {
      GUILayout.Space(10f);
      EditorGUILayout.HelpBox("Modification and inspection of built AudioMixer assets is disabled. Please modify the source asset and re-build.", MessageType.Info);
    }
  }
}
