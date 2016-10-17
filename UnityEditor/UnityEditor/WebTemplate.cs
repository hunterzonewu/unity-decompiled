// Decompiled with JetBrains decompiler
// Type: UnityEditor.WebTemplate
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class WebTemplate
  {
    public string m_Path;
    public string m_Name;
    public Texture2D m_Thumbnail;
    public string[] m_CustomKeys;

    public string[] CustomKeys
    {
      get
      {
        return this.m_CustomKeys;
      }
    }

    public override bool Equals(object other)
    {
      if (other is WebTemplate)
        return other.ToString().Equals(this.ToString());
      return false;
    }

    public override int GetHashCode()
    {
      return base.GetHashCode() ^ this.m_Path.GetHashCode();
    }

    public override string ToString()
    {
      return this.m_Path;
    }

    public GUIContent ToGUIContent(Texture2D defaultIcon)
    {
      return new GUIContent(this.m_Name, !((Object) this.m_Thumbnail == (Object) null) ? (Texture) this.m_Thumbnail : (Texture) defaultIcon);
    }
  }
}
