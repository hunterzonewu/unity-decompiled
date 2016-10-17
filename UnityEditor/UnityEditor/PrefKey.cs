// Decompiled with JetBrains decompiler
// Type: UnityEditor.PrefKey
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class PrefKey : IPrefType
  {
    private string m_name;
    private Event m_event;
    private string m_DefaultShortcut;

    public string Name
    {
      get
      {
        return this.m_name;
      }
    }

    public Event KeyboardEvent
    {
      get
      {
        return this.m_event;
      }
      set
      {
        this.m_event = value;
      }
    }

    public bool activated
    {
      get
      {
        if (Event.current.Equals((object) (Event) this))
          return !GUIUtility.textFieldInput;
        return false;
      }
    }

    public PrefKey()
    {
    }

    public PrefKey(string name, string shortcut)
    {
      this.m_name = name;
      this.m_event = Event.KeyboardEvent(shortcut);
      this.m_DefaultShortcut = shortcut;
      PrefKey prefKey = Settings.Get<PrefKey>(name, this);
      this.m_name = prefKey.Name;
      this.m_event = prefKey.KeyboardEvent;
    }

    public static implicit operator Event(PrefKey pkey)
    {
      return pkey.m_event;
    }

    public string ToUniqueString()
    {
      return this.m_name + ";" + (!this.m_event.alt ? string.Empty : "&") + (!this.m_event.command ? string.Empty : "%") + (!this.m_event.shift ? string.Empty : "#") + (!this.m_event.control ? string.Empty : "^") + (object) this.m_event.keyCode;
    }

    public void FromUniqueString(string s)
    {
      int length = s.IndexOf(";");
      if (length < 0)
      {
        Debug.LogError((object) "Malformed string in Keyboard preferences");
      }
      else
      {
        this.m_name = s.Substring(0, length);
        this.m_event = Event.KeyboardEvent(s.Substring(length + 1));
      }
    }

    internal void ResetToDefault()
    {
      this.m_event = Event.KeyboardEvent(this.m_DefaultShortcut);
    }
  }
}
