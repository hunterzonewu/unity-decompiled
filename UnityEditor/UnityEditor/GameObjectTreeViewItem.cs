// Decompiled with JetBrains decompiler
// Type: UnityEditor.GameObjectTreeViewItem
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityEditor
{
  internal class GameObjectTreeViewItem : TreeViewItem
  {
    private int m_ColorCode;
    private Object m_ObjectPPTR;
    private bool m_ShouldDisplay;
    private Scene m_UnityScene;

    public override string displayName
    {
      get
      {
        if (string.IsNullOrEmpty(base.displayName))
          this.displayName = !(this.m_ObjectPPTR != (Object) null) ? "deleted gameobject" : this.objectPPTR.name;
        return base.displayName;
      }
      set
      {
        base.displayName = value;
      }
    }

    public virtual int colorCode
    {
      get
      {
        return this.m_ColorCode;
      }
      set
      {
        this.m_ColorCode = value;
      }
    }

    public virtual Object objectPPTR
    {
      get
      {
        return this.m_ObjectPPTR;
      }
      set
      {
        this.m_ObjectPPTR = value;
      }
    }

    public virtual bool shouldDisplay
    {
      get
      {
        return this.m_ShouldDisplay;
      }
      set
      {
        this.m_ShouldDisplay = value;
      }
    }

    public bool isSceneHeader { get; set; }

    public Scene scene
    {
      get
      {
        return this.m_UnityScene;
      }
      set
      {
        this.m_UnityScene = value;
      }
    }

    public GameObjectTreeViewItem(int id, int depth, TreeViewItem parent, string displayName)
      : base(id, depth, parent, displayName)
    {
    }
  }
}
