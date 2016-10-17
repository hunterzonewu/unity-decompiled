// Decompiled with JetBrains decompiler
// Type: UnityEditor.SnapGuide
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class SnapGuide
  {
    public List<Vector3> lineVertices = new List<Vector3>();
    public bool safe = true;
    public float value;

    public SnapGuide(float value, params Vector3[] vertices)
      : this(value, true, vertices)
    {
    }

    public SnapGuide(float value, bool safe, params Vector3[] vertices)
    {
      this.value = value;
      this.lineVertices.AddRange((IEnumerable<Vector3>) vertices);
      this.safe = safe;
    }

    public void Draw()
    {
      Handles.color = !this.safe ? new Color(1f, 0.5f, 0.0f, 1f) : new Color(0.0f, 0.5f, 1f, 1f);
      int index = 0;
      while (index < this.lineVertices.Count)
      {
        Vector3 lineVertex1 = this.lineVertices[index];
        Vector3 lineVertex2 = this.lineVertices[index + 1];
        if (!(lineVertex1 == lineVertex2))
        {
          Vector3 vector3 = (lineVertex2 - lineVertex1).normalized * 0.05f;
          Handles.DrawLine(lineVertex1 - vector3 * HandleUtility.GetHandleSize(lineVertex1), lineVertex2 + vector3 * HandleUtility.GetHandleSize(lineVertex2));
        }
        index += 2;
      }
    }
  }
}
