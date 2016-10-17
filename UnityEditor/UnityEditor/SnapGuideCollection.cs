// Decompiled with JetBrains decompiler
// Type: UnityEditor.SnapGuideCollection
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class SnapGuideCollection
  {
    private Dictionary<float, List<SnapGuide>> guides = new Dictionary<float, List<SnapGuide>>();
    private List<SnapGuide> currentGuides;

    public void Clear()
    {
      this.guides.Clear();
    }

    public void AddGuide(SnapGuide guide)
    {
      List<SnapGuide> snapGuideList;
      if (!this.guides.TryGetValue(guide.value, out snapGuideList))
      {
        snapGuideList = new List<SnapGuide>();
        this.guides.Add(guide.value, snapGuideList);
      }
      snapGuideList.Add(guide);
    }

    public float SnapToGuides(float value, float snapDistance)
    {
      if (this.guides.Count == 0)
        return value;
      KeyValuePair<float, List<SnapGuide>> keyValuePair = new KeyValuePair<float, List<SnapGuide>>();
      float num1 = float.PositiveInfinity;
      using (Dictionary<float, List<SnapGuide>>.Enumerator enumerator = this.guides.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<float, List<SnapGuide>> current = enumerator.Current;
          float num2 = Mathf.Abs(value - current.Key);
          if ((double) num2 < (double) num1)
          {
            keyValuePair = current;
            num1 = num2;
          }
        }
      }
      if ((double) num1 <= (double) snapDistance)
      {
        value = keyValuePair.Key;
        this.currentGuides = keyValuePair.Value;
      }
      else
        this.currentGuides = (List<SnapGuide>) null;
      return value;
    }

    public void OnGUI()
    {
      if (Event.current.type != EventType.MouseUp)
        return;
      this.currentGuides = (List<SnapGuide>) null;
    }

    public void DrawGuides()
    {
      if (this.currentGuides == null)
        return;
      using (List<SnapGuide>.Enumerator enumerator = this.currentGuides.GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.Draw();
      }
    }
  }
}
