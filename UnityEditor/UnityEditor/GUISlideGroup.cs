// Decompiled with JetBrains decompiler
// Type: UnityEditor.GUISlideGroup
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class GUISlideGroup
  {
    private Dictionary<int, Rect> animIDs = new Dictionary<int, Rect>();
    private const float kLerp = 0.1f;
    private const float kSnap = 0.5f;
    internal static GUISlideGroup current;

    public void Begin()
    {
      if (GUISlideGroup.current != null)
        Debug.LogError((object) "You cannot nest animGroups");
      else
        GUISlideGroup.current = this;
    }

    public void End()
    {
      GUISlideGroup.current = (GUISlideGroup) null;
    }

    public void Reset()
    {
      GUISlideGroup.current = (GUISlideGroup) null;
      this.animIDs.Clear();
    }

    public Rect BeginHorizontal(int id, params GUILayoutOption[] options)
    {
      GUISlideGroup.SlideGroupInternal slideGroupInternal = (GUISlideGroup.SlideGroupInternal) GUILayoutUtility.BeginLayoutGroup(GUIStyle.none, options, typeof (GUISlideGroup.SlideGroupInternal));
      slideGroupInternal.SetID(this, id);
      slideGroupInternal.isVertical = false;
      return slideGroupInternal.m_FinalRect;
    }

    public void EndHorizontal()
    {
      GUILayoutUtility.EndLayoutGroup();
    }

    public Rect GetRect(int id, Rect r)
    {
      if (Event.current.type != EventType.Repaint)
        return r;
      bool changed;
      return this.GetRect(id, r, out changed);
    }

    private Rect GetRect(int id, Rect r, out bool changed)
    {
      if (!this.animIDs.ContainsKey(id))
      {
        this.animIDs.Add(id, r);
        changed = false;
        return r;
      }
      Rect animId = this.animIDs[id];
      if ((double) animId.y != (double) r.y || (double) animId.height != (double) r.height || ((double) animId.x != (double) r.x || (double) animId.width != (double) r.width))
      {
        float t = 0.1f;
        if ((double) Mathf.Abs(animId.y - r.y) > 0.5)
          r.y = Mathf.Lerp(animId.y, r.y, t);
        if ((double) Mathf.Abs(animId.height - r.height) > 0.5)
          r.height = Mathf.Lerp(animId.height, r.height, t);
        if ((double) Mathf.Abs(animId.x - r.x) > 0.5)
          r.x = Mathf.Lerp(animId.x, r.x, t);
        if ((double) Mathf.Abs(animId.width - r.width) > 0.5)
          r.width = Mathf.Lerp(animId.width, r.width, t);
        this.animIDs[id] = r;
        changed = true;
        HandleUtility.Repaint();
      }
      else
        changed = false;
      return r;
    }

    private class SlideGroupInternal : GUILayoutGroup
    {
      private int m_ID;
      private GUISlideGroup m_Owner;
      internal Rect m_FinalRect;

      public void SetID(GUISlideGroup owner, int id)
      {
        this.m_ID = id;
        this.m_Owner = owner;
      }

      public override void SetHorizontal(float x, float width)
      {
        this.m_FinalRect.x = x;
        this.m_FinalRect.width = width;
        base.SetHorizontal(x, width);
      }

      public override void SetVertical(float y, float height)
      {
        this.m_FinalRect.y = y;
        this.m_FinalRect.height = height;
        bool changed;
        Rect rect = this.m_Owner.GetRect(this.m_ID, new Rect(this.rect.x, y, this.rect.width, height), out changed);
        if (changed)
          base.SetHorizontal(rect.x, rect.width);
        base.SetVertical(rect.y, rect.height);
      }
    }
  }
}
