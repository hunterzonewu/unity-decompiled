// Decompiled with JetBrains decompiler
// Type: UnityEditor.SceneViewGrid
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor
{
  [Serializable]
  internal class SceneViewGrid
  {
    private static PrefColor kViewGridColor = new PrefColor("Scene/Grid", 0.5f, 0.5f, 0.5f, 0.4f);
    [SerializeField]
    private AnimBool xGrid = new AnimBool();
    [SerializeField]
    private AnimBool yGrid = new AnimBool();
    [SerializeField]
    private AnimBool zGrid = new AnimBool();

    public void Register(SceneView source)
    {
      this.xGrid.valueChanged.AddListener(new UnityAction(((EditorWindow) source).Repaint));
      this.yGrid.valueChanged.AddListener(new UnityAction(((EditorWindow) source).Repaint));
      this.zGrid.valueChanged.AddListener(new UnityAction(((EditorWindow) source).Repaint));
    }

    public DrawGridParameters PrepareGridRender(Camera camera, Vector3 pivot, Quaternion rotation, float size, bool orthoMode, bool gridVisible)
    {
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      if (gridVisible)
      {
        if (orthoMode)
        {
          Vector3 vector3 = rotation * Vector3.forward;
          if ((double) Mathf.Abs(vector3.y) > 0.200000002980232)
            flag2 = true;
          else if (vector3 == Vector3.left || vector3 == Vector3.right)
            flag1 = true;
          else if (vector3 == Vector3.forward || vector3 == Vector3.back)
            flag3 = true;
        }
        else
          flag2 = true;
      }
      this.xGrid.target = flag1;
      this.yGrid.target = flag2;
      this.zGrid.target = flag3;
      DrawGridParameters drawGridParameters;
      drawGridParameters.pivot = pivot;
      drawGridParameters.color = (Color) SceneViewGrid.kViewGridColor;
      drawGridParameters.size = size;
      drawGridParameters.alphaX = this.xGrid.faded;
      drawGridParameters.alphaY = this.yGrid.faded;
      drawGridParameters.alphaZ = this.zGrid.faded;
      return drawGridParameters;
    }
  }
}
