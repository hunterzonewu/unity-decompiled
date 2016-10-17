// Decompiled with JetBrains decompiler
// Type: UnityEditor.IPreviewable
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal interface IPreviewable
  {
    Object target { get; }

    void Initialize(Object[] targets);

    bool MoveNextTarget();

    void ResetTarget();

    bool HasPreviewGUI();

    GUIContent GetPreviewTitle();

    void DrawPreview(Rect previewArea);

    void OnPreviewGUI(Rect r, GUIStyle background);

    void OnInteractivePreviewGUI(Rect r, GUIStyle background);

    void OnPreviewSettings();

    string GetInfoString();

    void ReloadPreviewInstances();
  }
}
