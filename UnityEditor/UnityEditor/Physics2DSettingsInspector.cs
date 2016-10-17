// Decompiled with JetBrains decompiler
// Type: UnityEditor.Physics2DSettingsInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (Physics2DSettings))]
  internal class Physics2DSettingsInspector : Editor
  {
    private bool show = true;
    private Vector2 scrollPos;

    private bool GetValue(int layerA, int layerB)
    {
      return !Physics2D.GetIgnoreLayerCollision(layerA, layerB);
    }

    private void SetValue(int layerA, int layerB, bool val)
    {
      Physics2D.IgnoreLayerCollision(layerA, layerB, !val);
    }

    public override void OnInspectorGUI()
    {
      this.DrawDefaultInspector();
      LayerMatrixGUI.DoGUI("Layer Collision Matrix", ref this.show, ref this.scrollPos, new LayerMatrixGUI.GetValueFunc(this.GetValue), new LayerMatrixGUI.SetValueFunc(this.SetValue));
    }
  }
}
