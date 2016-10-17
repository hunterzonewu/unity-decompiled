// Decompiled with JetBrains decompiler
// Type: UnityEditor.TreeViewItemExpansionAnimator
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal class TreeViewItemExpansionAnimator
  {
    private TreeViewAnimationInput m_Setup;
    private bool m_InsideGUIClip;
    private Rect m_CurrentClipRect;
    private static bool s_Debug;

    public float expandedValueNormalized
    {
      get
      {
        float elapsedTimeNormalized = this.m_Setup.elapsedTimeNormalized;
        if (this.m_Setup.expanding)
          return elapsedTimeNormalized;
        return 1f - elapsedTimeNormalized;
      }
    }

    public int startRow
    {
      get
      {
        return this.m_Setup.startRow;
      }
    }

    public int endRow
    {
      get
      {
        return this.m_Setup.endRow;
      }
    }

    public float deltaHeight
    {
      get
      {
        return this.m_Setup.rowsRect.height - this.m_Setup.rowsRect.height * this.expandedValueNormalized;
      }
    }

    public bool isAnimating
    {
      get
      {
        return this.m_Setup != null;
      }
    }

    public bool isExpanding
    {
      get
      {
        return this.m_Setup.expanding;
      }
    }

    private bool printDebug
    {
      get
      {
        if (TreeViewItemExpansionAnimator.s_Debug && this.m_Setup != null && this.m_Setup.treeView != null)
          return Event.current.type == EventType.Repaint;
        return false;
      }
    }

    public void BeginAnimating(TreeViewAnimationInput setup)
    {
      if (this.m_Setup != null)
      {
        if (this.m_Setup.item.id == setup.item.id)
        {
          if (this.m_Setup.elapsedTime >= 0.0)
            setup.elapsedTime = this.m_Setup.animationDuration - this.m_Setup.elapsedTime;
          else
            Debug.LogError((object) ("Invaid duration " + (object) this.m_Setup.elapsedTime));
          this.m_Setup = setup;
        }
        else
        {
          this.m_Setup.FireAnimationEndedEvent();
          this.m_Setup = setup;
        }
        this.m_Setup.expanding = setup.expanding;
      }
      this.m_Setup = setup;
      if (this.m_Setup == null)
        Debug.LogError((object) "Setup is null");
      if (this.printDebug)
        Console.WriteLine("Begin animating: " + (object) this.m_Setup);
      this.m_CurrentClipRect = this.GetCurrentClippingRect();
    }

    public bool CullRow(int row, ITreeViewGUI gui)
    {
      if (!this.isAnimating)
        return false;
      if (this.printDebug && row == 0)
        Console.WriteLine("--------");
      if (row <= this.m_Setup.startRow || row > this.m_Setup.endRow || (double) gui.GetRowRect(row, 1f).y - (double) this.m_Setup.startRowRect.y <= (double) this.m_CurrentClipRect.height)
        return false;
      if (this.m_InsideGUIClip)
        this.EndClip();
      return true;
    }

    public void OnRowGUI(int row)
    {
      if (!this.printDebug)
        return;
      Console.WriteLine(row.ToString() + " Do item " + this.DebugItemName(row));
    }

    public Rect OnBeginRowGUI(int row, Rect rowRect)
    {
      if (!this.isAnimating)
        return rowRect;
      if (row == this.m_Setup.startRow)
        this.BeginClip();
      if (row >= this.m_Setup.startRow && row <= this.m_Setup.endRow)
        rowRect.y -= this.m_Setup.startRowRect.y;
      else if (row > this.m_Setup.endRow)
        rowRect.y -= this.m_Setup.rowsRect.height - this.m_CurrentClipRect.height;
      return rowRect;
    }

    public void OnEndRowGUI(int row)
    {
      if (!this.isAnimating || !this.m_InsideGUIClip || row != this.m_Setup.endRow)
        return;
      this.EndClip();
    }

    private void BeginClip()
    {
      GUI.BeginClip(this.m_CurrentClipRect);
      this.m_InsideGUIClip = true;
      if (!this.printDebug)
        return;
      Console.WriteLine("BeginClip startRow: " + (object) this.m_Setup.startRow);
    }

    private void EndClip()
    {
      GUI.EndClip();
      this.m_InsideGUIClip = false;
      if (!this.printDebug)
        return;
      Console.WriteLine("EndClip endRow: " + (object) this.m_Setup.endRow);
    }

    public void OnBeforeAllRowsGUI()
    {
      if (!this.isAnimating)
        return;
      this.m_CurrentClipRect = this.GetCurrentClippingRect();
      if (this.m_Setup.elapsedTime <= this.m_Setup.animationDuration)
        return;
      this.m_Setup.FireAnimationEndedEvent();
      this.m_Setup = (TreeViewAnimationInput) null;
      if (!this.printDebug)
        return;
      Debug.Log((object) "Animation ended");
    }

    public void OnAfterAllRowsGUI()
    {
      if (this.m_InsideGUIClip)
        this.EndClip();
      if (!this.isAnimating)
        return;
      HandleUtility.Repaint();
    }

    public bool IsAnimating(int itemID)
    {
      if (!this.isAnimating)
        return false;
      return this.m_Setup.item.id == itemID;
    }

    private Rect GetCurrentClippingRect()
    {
      Rect rowsRect = this.m_Setup.rowsRect;
      rowsRect.height *= this.expandedValueNormalized;
      return rowsRect;
    }

    private string DebugItemName(int row)
    {
      return this.m_Setup.treeView.data.GetRows()[row].displayName;
    }
  }
}
