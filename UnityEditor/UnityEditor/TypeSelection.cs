// Decompiled with JetBrains decompiler
// Type: UnityEditor.TypeSelection
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal class TypeSelection : IComparable
  {
    public GUIContent label;
    public UnityEngine.Object[] objects;

    public TypeSelection(string typeName, UnityEngine.Object[] objects)
    {
      this.objects = objects;
      this.label = new GUIContent(objects.Length.ToString() + " " + ObjectNames.NicifyVariableName(typeName) + (objects.Length <= 1 ? string.Empty : "s"));
      this.label.image = (Texture) AssetPreview.GetMiniTypeThumbnail(objects[0]);
    }

    public int CompareTo(object o)
    {
      TypeSelection typeSelection = (TypeSelection) o;
      if (typeSelection.objects.Length != this.objects.Length)
        return typeSelection.objects.Length.CompareTo(this.objects.Length);
      return this.label.text.CompareTo(typeSelection.label.text);
    }
  }
}
