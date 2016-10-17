// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.ClipboardAccess
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.Web
{
  [InitializeOnLoad]
  internal class ClipboardAccess
  {
    static ClipboardAccess()
    {
      JSProxyMgr.GetInstance().AddGlobalObject("unity/ClipboardAccess", (object) new ClipboardAccess());
    }

    private ClipboardAccess()
    {
    }

    public void CopyToClipboard(string value)
    {
      TextEditor textEditor = new TextEditor();
      textEditor.text = value;
      textEditor.SelectAll();
      textEditor.Copy();
    }

    public string PasteFromClipboard()
    {
      TextEditor textEditor = new TextEditor();
      textEditor.Paste();
      return textEditor.text;
    }
  }
}
