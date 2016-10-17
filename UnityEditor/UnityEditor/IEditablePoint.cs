// Decompiled with JetBrains decompiler
// Type: UnityEditor.IEditablePoint
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal interface IEditablePoint
  {
    int Count { get; }

    Vector3 GetPosition(int idx);

    void SetPosition(int idx, Vector3 position);

    Color GetDefaultColor();

    Color GetSelectedColor();

    float GetPointScale();

    IEnumerable<Vector3> GetPositions();

    Vector3[] GetUnselectedPositions();

    Vector3[] GetSelectedPositions();
  }
}
