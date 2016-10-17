// Decompiled with JetBrains decompiler
// Type: UnityEditor.DoubleCurvePresetLibraryEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (DoubleCurvePresetLibrary))]
  internal class DoubleCurvePresetLibraryEditor : Editor
  {
    private GenericPresetLibraryInspector<DoubleCurvePresetLibrary> m_GenericPresetLibraryInspector;

    public void OnEnable()
    {
      this.m_GenericPresetLibraryInspector = new GenericPresetLibraryInspector<DoubleCurvePresetLibrary>(this.target, this.GetHeader(AssetDatabase.GetAssetPath(this.target.GetInstanceID())), (System.Action<string>) null);
      this.m_GenericPresetLibraryInspector.presetSize = new Vector2(72f, 20f);
      this.m_GenericPresetLibraryInspector.lineSpacing = 5f;
    }

    private string GetHeader(string filePath)
    {
      return "Particle Curve Preset Library";
    }

    public void OnDestroy()
    {
      if (this.m_GenericPresetLibraryInspector == null)
        return;
      this.m_GenericPresetLibraryInspector.OnDestroy();
    }

    public override void OnInspectorGUI()
    {
      if (this.m_GenericPresetLibraryInspector == null)
        return;
      this.m_GenericPresetLibraryInspector.OnInspectorGUI();
    }
  }
}
