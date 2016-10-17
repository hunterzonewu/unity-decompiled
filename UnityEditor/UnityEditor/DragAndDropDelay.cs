// Decompiled with JetBrains decompiler
// Type: UnityEditor.DragAndDropDelay
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class DragAndDropDelay
  {
    public Vector2 mouseDownPosition;

    public bool CanStartDrag()
    {
      return (double) Vector2.Distance(this.mouseDownPosition, Event.current.mousePosition) > 6.0;
    }
  }
}
