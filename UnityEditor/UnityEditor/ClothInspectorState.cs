// Decompiled with JetBrains decompiler
// Type: UnityEditor.ClothInspectorState
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class ClothInspectorState : ScriptableSingleton<ClothInspectorState>
  {
    [SerializeField]
    public ClothInspector.DrawMode DrawMode = ClothInspector.DrawMode.MaxDistance;
    [SerializeField]
    public bool PaintMaxDistanceEnabled = true;
    [SerializeField]
    public float PaintMaxDistance = 0.2f;
    [SerializeField]
    public bool ManipulateBackfaces;
    [SerializeField]
    public bool PaintCollisionSphereDistanceEnabled;
    [SerializeField]
    public float PaintCollisionSphereDistance;
    [SerializeField]
    public ClothInspector.ToolMode ToolMode;
  }
}
