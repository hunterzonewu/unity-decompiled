// Decompiled with JetBrains decompiler
// Type: UnityEditor.ExposablePopupMenu
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class ExposablePopupMenu
  {
    private List<ExposablePopupMenu.ItemData> m_Items;
    private float m_WidthOfButtons;
    private float m_ItemSpacing;
    private ExposablePopupMenu.PopupButtonData m_PopupButtonData;
    private float m_WidthOfPopup;
    private float m_MinWidthOfPopup;
    private System.Action<ExposablePopupMenu.ItemData> m_SelectionChangedCallback;

    public void Init(List<ExposablePopupMenu.ItemData> items, float itemSpacing, float minWidthOfPopup, ExposablePopupMenu.PopupButtonData popupButtonData, System.Action<ExposablePopupMenu.ItemData> selectionChangedCallback)
    {
      this.m_Items = items;
      this.m_ItemSpacing = itemSpacing;
      this.m_PopupButtonData = popupButtonData;
      this.m_SelectionChangedCallback = selectionChangedCallback;
      this.m_MinWidthOfPopup = minWidthOfPopup;
      this.CalcWidths();
    }

    public float OnGUI(Rect rect)
    {
      if ((double) rect.width >= (double) this.m_WidthOfButtons && (double) rect.width > (double) this.m_MinWidthOfPopup)
      {
        Rect position = rect;
        using (List<ExposablePopupMenu.ItemData>.Enumerator enumerator = this.m_Items.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            ExposablePopupMenu.ItemData current = enumerator.Current;
            position.width = current.m_Width;
            EditorGUI.BeginChangeCheck();
            EditorGUI.BeginDisabledGroup(!current.m_Enabled);
            GUI.Toggle(position, current.m_On, current.m_GUIContent, current.m_Style);
            EditorGUI.EndDisabledGroup();
            if (EditorGUI.EndChangeCheck())
            {
              this.SelectionChanged(current);
              GUIUtility.ExitGUI();
            }
            position.x += current.m_Width + this.m_ItemSpacing;
          }
        }
        return this.m_WidthOfButtons;
      }
      if ((double) this.m_WidthOfPopup < (double) rect.width)
        rect.width = this.m_WidthOfPopup;
      if (EditorGUI.ButtonMouseDown(rect, this.m_PopupButtonData.m_GUIContent, FocusType.Passive, this.m_PopupButtonData.m_Style))
        ExposablePopupMenu.PopUpMenu.Show(rect, this.m_Items, this);
      return this.m_WidthOfPopup;
    }

    private void CalcWidths()
    {
      this.m_WidthOfButtons = 0.0f;
      using (List<ExposablePopupMenu.ItemData>.Enumerator enumerator = this.m_Items.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ExposablePopupMenu.ItemData current = enumerator.Current;
          current.m_Width = current.m_Style.CalcSize(current.m_GUIContent).x;
          this.m_WidthOfButtons += current.m_Width;
        }
      }
      this.m_WidthOfButtons += (float) (this.m_Items.Count - 1) * this.m_ItemSpacing;
      Vector2 vector2 = this.m_PopupButtonData.m_Style.CalcSize(this.m_PopupButtonData.m_GUIContent);
      vector2.x += 3f;
      this.m_WidthOfPopup = vector2.x;
    }

    private void SelectionChanged(ExposablePopupMenu.ItemData item)
    {
      if (this.m_SelectionChangedCallback != null)
        this.m_SelectionChangedCallback(item);
      else
        Debug.LogError((object) "Callback is null");
    }

    public class ItemData
    {
      public GUIContent m_GUIContent;
      public GUIStyle m_Style;
      public bool m_On;
      public bool m_Enabled;
      public object m_UserData;
      public float m_Width;

      public ItemData(GUIContent content, GUIStyle style, bool on, bool enabled, object userData)
      {
        this.m_GUIContent = content;
        this.m_Style = style;
        this.m_On = on;
        this.m_Enabled = enabled;
        this.m_UserData = userData;
      }
    }

    public class PopupButtonData
    {
      public GUIContent m_GUIContent;
      public GUIStyle m_Style;

      public PopupButtonData(GUIContent content, GUIStyle style)
      {
        this.m_GUIContent = content;
        this.m_Style = style;
      }
    }

    internal class PopUpMenu
    {
      private static List<ExposablePopupMenu.ItemData> m_Data;
      private static ExposablePopupMenu m_Caller;

      internal static void Show(Rect activatorRect, List<ExposablePopupMenu.ItemData> buttonData, ExposablePopupMenu caller)
      {
        ExposablePopupMenu.PopUpMenu.m_Data = buttonData;
        ExposablePopupMenu.PopUpMenu.m_Caller = caller;
        GenericMenu genericMenu = new GenericMenu();
        using (List<ExposablePopupMenu.ItemData>.Enumerator enumerator = ExposablePopupMenu.PopUpMenu.m_Data.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            ExposablePopupMenu.ItemData current = enumerator.Current;
            if (current.m_Enabled)
              genericMenu.AddItem(current.m_GUIContent, current.m_On, new GenericMenu.MenuFunction2(ExposablePopupMenu.PopUpMenu.SelectionCallback), (object) current);
            else
              genericMenu.AddDisabledItem(current.m_GUIContent);
          }
        }
        genericMenu.DropDown(activatorRect);
      }

      private static void SelectionCallback(object userData)
      {
        ExposablePopupMenu.ItemData itemData = (ExposablePopupMenu.ItemData) userData;
        ExposablePopupMenu.PopUpMenu.m_Caller.SelectionChanged(itemData);
        ExposablePopupMenu.PopUpMenu.m_Caller = (ExposablePopupMenu) null;
        ExposablePopupMenu.PopUpMenu.m_Data = (List<ExposablePopupMenu.ItemData>) null;
      }
    }
  }
}
