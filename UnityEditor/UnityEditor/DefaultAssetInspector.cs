// Decompiled with JetBrains decompiler
// Type: UnityEditor.DefaultAssetInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor
{
  [CustomEditor(typeof (DefaultAsset), isFallback = true)]
  internal class DefaultAssetInspector : Editor
  {
    public override void OnInspectorGUI()
    {
      DefaultAsset target = (DefaultAsset) this.target;
      if (target.message.Length <= 0)
        return;
      EditorGUILayout.HelpBox(target.message, !target.isWarning ? MessageType.Info : MessageType.Warning);
    }
  }
}
