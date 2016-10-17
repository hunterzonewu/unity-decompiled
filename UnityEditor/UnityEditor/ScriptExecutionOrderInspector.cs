// Decompiled with JetBrains decompiler
// Type: UnityEditor.ScriptExecutionOrderInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (MonoManager))]
  internal class ScriptExecutionOrderInspector : Editor
  {
    private static int s_DropFieldHash = "DropField".GetHashCode();
    private int[] kRoundingAmounts = new int[7]{ 1000, 500, 100, 50, 10, 5, 1 };
    private Vector2 m_Scroll = Vector2.zero;
    private const int kOrderRangeMin = -32000;
    private const int kOrderRangeMax = 32000;
    private const int kListElementHeight = 21;
    private const int kIntFieldWidth = 50;
    private const int kPreferredSpacing = 100;
    private const string kOrderValuesEditorPrefString = "ScriptExecutionOrderShowOrderValues";
    private MonoScript m_Edited;
    private List<MonoScript> m_CustomTimeScripts;
    private List<MonoScript> m_DefaultTimeScripts;
    private static MonoScript sDummyScript;
    private MonoScript[] m_AllScripts;
    private int[] m_AllOrders;
    private bool m_DirtyOrders;
    public static ScriptExecutionOrderInspector.Styles m_Styles;

    public void OnEnable()
    {
      if ((UnityEngine.Object) ScriptExecutionOrderInspector.sDummyScript == (UnityEngine.Object) null)
        ScriptExecutionOrderInspector.sDummyScript = new MonoScript();
      if (this.m_AllScripts != null && this.m_DirtyOrders)
        return;
      this.PopulateScriptArray();
    }

    private static UnityEngine.Object MonoScriptValidatorCallback(UnityEngine.Object[] references, System.Type objType, SerializedProperty property)
    {
      foreach (UnityEngine.Object reference in references)
      {
        MonoScript script = reference as MonoScript;
        if ((UnityEngine.Object) script != (UnityEngine.Object) null && ScriptExecutionOrderInspector.IsValidScript(script))
          return (UnityEngine.Object) script;
      }
      return (UnityEngine.Object) null;
    }

    private static bool IsValidScript(MonoScript script)
    {
      return !((UnityEngine.Object) script == (UnityEngine.Object) null) && script.GetClass() != null && ((typeof (MonoBehaviour).IsAssignableFrom(script.GetClass()) || typeof (ScriptableObject).IsAssignableFrom(script.GetClass())) && AssetDatabase.GetAssetPath((UnityEngine.Object) script).IndexOf("Assets/") == 0);
    }

    private void PopulateScriptArray()
    {
      this.m_AllScripts = MonoImporter.GetAllRuntimeMonoScripts();
      this.m_AllOrders = new int[this.m_AllScripts.Length];
      this.m_CustomTimeScripts = new List<MonoScript>();
      this.m_DefaultTimeScripts = new List<MonoScript>();
      for (int index = 0; index < this.m_AllScripts.Length; ++index)
      {
        MonoScript allScript = this.m_AllScripts[index];
        this.m_AllOrders[index] = MonoImporter.GetExecutionOrder(allScript);
        if (ScriptExecutionOrderInspector.IsValidScript(allScript))
        {
          if (this.GetExecutionOrder(allScript) == 0)
            this.m_DefaultTimeScripts.Add(allScript);
          else
            this.m_CustomTimeScripts.Add(allScript);
        }
      }
      this.m_CustomTimeScripts.Add(ScriptExecutionOrderInspector.sDummyScript);
      this.m_CustomTimeScripts.Add(ScriptExecutionOrderInspector.sDummyScript);
      this.m_CustomTimeScripts.Sort((IComparer<MonoScript>) new ScriptExecutionOrderInspector.SortMonoScriptExecutionOrder(this));
      this.m_DefaultTimeScripts.Sort((IComparer<MonoScript>) new ScriptExecutionOrderInspector.SortMonoScriptNameOrder());
      this.m_Edited = (MonoScript) null;
      this.m_DirtyOrders = false;
    }

    private int GetExecutionOrder(MonoScript script)
    {
      int index = Array.IndexOf<MonoScript>(this.m_AllScripts, script);
      if (index >= 0)
        return this.m_AllOrders[index];
      return 0;
    }

    private void SetExecutionOrder(MonoScript script, int order)
    {
      int index = Array.IndexOf<MonoScript>(this.m_AllScripts, script);
      if (index < 0)
        return;
      this.m_AllOrders[index] = Mathf.Clamp(order, -32000, 32000);
      this.m_DirtyOrders = true;
    }

    private void Apply()
    {
      List<int> intList = new List<int>();
      List<MonoScript> monoScriptList = new List<MonoScript>();
      for (int index = 0; index < this.m_AllScripts.Length; ++index)
      {
        if (MonoImporter.GetExecutionOrder(this.m_AllScripts[index]) != this.m_AllOrders[index])
        {
          intList.Add(index);
          monoScriptList.Add(this.m_AllScripts[index]);
        }
      }
      bool flag = true;
      if (Provider.enabled)
      {
        Task task = Provider.Checkout((UnityEngine.Object[]) monoScriptList.ToArray(), CheckoutMode.Meta);
        task.Wait();
        flag = task.success;
      }
      if (flag)
      {
        using (List<int>.Enumerator enumerator = intList.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            int current = enumerator.Current;
            MonoImporter.SetExecutionOrder(this.m_AllScripts[current], this.m_AllOrders[current]);
          }
        }
        this.PopulateScriptArray();
      }
      else
        Debug.LogError((object) "Could not checkout scrips in version control for changing script execution order");
    }

    private void Revert()
    {
      this.PopulateScriptArray();
    }

    private void OnDestroy()
    {
      if (!this.m_DirtyOrders || !EditorUtility.DisplayDialog("Unapplied execution order", "Unapplied script execution order", "Apply", "Revert"))
        return;
      this.Apply();
    }

    private void ApplyRevertGUI()
    {
      EditorGUILayout.Space();
      bool enabled = GUI.enabled;
      GUI.enabled = this.m_DirtyOrders;
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      if (GUILayout.Button("Revert"))
        this.Revert();
      if (GUILayout.Button("Apply"))
        this.Apply();
      GUILayout.EndHorizontal();
      GUI.enabled = enabled;
    }

    private void MenuSelection(object userData, string[] options, int selected)
    {
      this.AddScriptToCustomOrder(this.m_DefaultTimeScripts[selected]);
    }

    private void AddScriptToCustomOrder(MonoScript script)
    {
      if (!ScriptExecutionOrderInspector.IsValidScript(script) || this.m_CustomTimeScripts.Contains(script))
        return;
      int order = this.RoundByAmount(this.GetExecutionOrderAtIndex(this.m_CustomTimeScripts.Count - 1) + 100, 100);
      this.SetExecutionOrder(script, order);
      this.m_CustomTimeScripts.Add(script);
      this.m_DefaultTimeScripts.Remove(script);
    }

    private void ShowScriptPopup(Rect r)
    {
      int count = this.m_DefaultTimeScripts.Count;
      string[] options = new string[count];
      bool[] enabled = new bool[count];
      for (int index = 0; index < count; ++index)
      {
        options[index] = this.m_DefaultTimeScripts[index].GetClass().FullName;
        enabled[index] = true;
      }
      EditorUtility.DisplayCustomMenu(r, options, enabled, (int[]) null, new EditorUtility.SelectMenuItemFunction(this.MenuSelection), (object) null);
    }

    private int RoundBasedOnContext(int val, int lowerBound, int upperBound)
    {
      int num1 = Mathf.Max(0, (upperBound - lowerBound) / 6);
      lowerBound += num1;
      upperBound -= num1;
      for (int index = 0; index < this.kRoundingAmounts.Length; ++index)
      {
        int num2 = this.RoundByAmount(val, this.kRoundingAmounts[index]);
        if (num2 > lowerBound && num2 < upperBound)
          return num2;
      }
      return val;
    }

    private int RoundByAmount(int val, int rounding)
    {
      return Mathf.RoundToInt((float) val / (float) rounding) * rounding;
    }

    private int GetAverageRoundedAwayFromZero(int a, int b)
    {
      if ((a + b) % 2 == 0)
        return (a + b) / 2;
      return (a + b + Math.Sign(a + b)) / 2;
    }

    private void SetExecutionOrderAtIndexAccordingToNeighbors(int indexOfChangedItem, int pushDirection)
    {
      if (indexOfChangedItem < 0 || indexOfChangedItem >= this.m_CustomTimeScripts.Count)
        return;
      if (indexOfChangedItem == 0)
        this.SetExecutionOrderAtIndex(indexOfChangedItem, this.RoundByAmount(this.GetExecutionOrderAtIndex(indexOfChangedItem + 1) - 100, 100));
      else if (indexOfChangedItem == this.m_CustomTimeScripts.Count - 1)
      {
        this.SetExecutionOrderAtIndex(indexOfChangedItem, this.RoundByAmount(this.GetExecutionOrderAtIndex(indexOfChangedItem - 1) + 100, 100));
      }
      else
      {
        int executionOrderAtIndex1 = this.GetExecutionOrderAtIndex(indexOfChangedItem - 1);
        int executionOrderAtIndex2 = this.GetExecutionOrderAtIndex(indexOfChangedItem + 1);
        int num = this.RoundBasedOnContext(this.GetAverageRoundedAwayFromZero(executionOrderAtIndex1, executionOrderAtIndex2), executionOrderAtIndex1, executionOrderAtIndex2);
        if (num != 0)
        {
          if (pushDirection == 0)
            pushDirection = this.GetBestPushDirectionForOrderValue(num);
          num = pushDirection <= 0 ? Mathf.Min(num, executionOrderAtIndex2 - 1) : Mathf.Max(num, executionOrderAtIndex1 + 1);
        }
        this.SetExecutionOrderAtIndex(indexOfChangedItem, num);
      }
    }

    private void UpdateOrder(MonoScript changedScript)
    {
      this.m_CustomTimeScripts.Remove(changedScript);
      int executionOrder = this.GetExecutionOrder(changedScript);
      if (executionOrder == 0)
      {
        this.m_DefaultTimeScripts.Add(changedScript);
        this.m_DefaultTimeScripts.Sort((IComparer<MonoScript>) new ScriptExecutionOrderInspector.SortMonoScriptNameOrder());
      }
      else
      {
        int num = -1;
        for (int idx = 0; idx < this.m_CustomTimeScripts.Count; ++idx)
        {
          if (this.GetExecutionOrderAtIndex(idx) == executionOrder)
          {
            num = idx;
            break;
          }
        }
        if (num == -1)
        {
          this.m_CustomTimeScripts.Add(changedScript);
          this.m_CustomTimeScripts.Sort((IComparer<MonoScript>) new ScriptExecutionOrderInspector.SortMonoScriptExecutionOrder(this));
        }
        else
        {
          int directionForOrderValue = this.GetBestPushDirectionForOrderValue(executionOrder);
          if (directionForOrderValue == 1)
          {
            this.m_CustomTimeScripts.Insert(num, changedScript);
            ++num;
          }
          else
            this.m_CustomTimeScripts.Insert(num + 1, changedScript);
          this.PushAwayToAvoidConflicts(num, directionForOrderValue);
        }
      }
    }

    private void PushAwayToAvoidConflicts(int startIndex, int pushDirection)
    {
      int num = startIndex;
      while (num >= 0 && num < this.m_CustomTimeScripts.Count && (this.GetExecutionOrderAtIndex(num) - this.GetExecutionOrderAtIndex(num - pushDirection)) * pushDirection < 1)
      {
        this.SetExecutionOrderAtIndexAccordingToNeighbors(num, pushDirection);
        num += pushDirection;
      }
    }

    private int GetBestPushDirectionForOrderValue(int order)
    {
      int num = (int) Mathf.Sign((float) order);
      if (order < -16000 || order > 16000)
        num = -num;
      return num;
    }

    public override bool UseDefaultMargins()
    {
      return false;
    }

    public override void OnInspectorGUI()
    {
      if (ScriptExecutionOrderInspector.m_Styles == null)
        ScriptExecutionOrderInspector.m_Styles = new ScriptExecutionOrderInspector.Styles();
      if ((bool) ((UnityEngine.Object) this.m_Edited))
      {
        this.UpdateOrder(this.m_Edited);
        this.m_Edited = (MonoScript) null;
      }
      EditorGUILayout.BeginVertical(EditorStyles.inspectorFullWidthMargins, new GUILayoutOption[0]);
      GUILayout.Label(ScriptExecutionOrderInspector.m_Styles.helpText, EditorStyles.helpBox, new GUILayoutOption[0]);
      EditorGUILayout.Space();
      Rect position = EditorGUILayout.BeginVertical();
      int controlId = GUIUtility.GetControlID(ScriptExecutionOrderInspector.s_DropFieldHash, FocusType.Passive, position);
      MonoScript script = EditorGUI.DoDropField(position, controlId, typeof (MonoScript), new EditorGUI.ObjectFieldValidator(ScriptExecutionOrderInspector.MonoScriptValidatorCallback), false, ScriptExecutionOrderInspector.m_Styles.dropField) as MonoScript;
      if ((bool) ((UnityEngine.Object) script))
        this.AddScriptToCustomOrder(script);
      EditorGUILayout.BeginVertical(ScriptExecutionOrderInspector.m_Styles.boxBackground, new GUILayoutOption[0]);
      this.m_Scroll = EditorGUILayout.BeginVerticalScrollView(this.m_Scroll);
      int indexOfChangedItem = ScriptExecutionOrderInspector.DragReorderGUI.DragReorder(GUILayoutUtility.GetRect(10f, (float) (21 * this.m_CustomTimeScripts.Count), new GUILayoutOption[1]{ GUILayout.ExpandWidth(true) }), 21, this.m_CustomTimeScripts, new ScriptExecutionOrderInspector.DragReorderGUI.DrawElementDelegate(this.DrawElement));
      if (indexOfChangedItem >= 0)
      {
        this.SetExecutionOrderAtIndexAccordingToNeighbors(indexOfChangedItem, 0);
        this.UpdateOrder(this.m_CustomTimeScripts[indexOfChangedItem]);
        this.SetExecutionOrderAtIndexAccordingToNeighbors(indexOfChangedItem, 0);
      }
      EditorGUILayout.EndScrollView();
      EditorGUILayout.EndVertical();
      GUILayout.BeginHorizontal(ScriptExecutionOrderInspector.m_Styles.toolbar, new GUILayoutOption[0]);
      GUILayout.FlexibleSpace();
      GUIContent iconToolbarPlus = ScriptExecutionOrderInspector.m_Styles.iconToolbarPlus;
      Rect rect = GUILayoutUtility.GetRect(iconToolbarPlus, ScriptExecutionOrderInspector.m_Styles.toolbarDropDown);
      if (EditorGUI.ButtonMouseDown(rect, iconToolbarPlus, FocusType.Native, ScriptExecutionOrderInspector.m_Styles.toolbarDropDown))
        this.ShowScriptPopup(rect);
      GUILayout.EndHorizontal();
      GUILayout.EndVertical();
      this.ApplyRevertGUI();
      GUILayout.EndVertical();
      GUILayout.FlexibleSpace();
    }

    private int GetExecutionOrderAtIndex(int idx)
    {
      return this.GetExecutionOrder(this.m_CustomTimeScripts[idx]);
    }

    private void SetExecutionOrderAtIndex(int idx, int order)
    {
      this.SetExecutionOrder(this.m_CustomTimeScripts[idx], order);
    }

    private Rect GetButtonLabelRect(Rect r)
    {
      return new Rect(r.x + 20f, r.y + 1f, (float) ((double) r.width - (double) this.GetMinusButtonSize().x - 10.0 - 20.0 - 55.0), r.height);
    }

    private Rect GetAddRemoveButtonRect(Rect r)
    {
      Vector2 minusButtonSize = this.GetMinusButtonSize();
      return new Rect((float) ((double) r.xMax - (double) minusButtonSize.x - 5.0), r.y + 1f, minusButtonSize.x, minusButtonSize.y);
    }

    private Rect GetFieldRect(Rect r)
    {
      return new Rect((float) ((double) r.xMax - 50.0 - (double) this.GetMinusButtonSize().x - 10.0), r.y + 2f, 50f, r.height - 5f);
    }

    private Vector2 GetMinusButtonSize()
    {
      return ScriptExecutionOrderInspector.m_Styles.removeButton.CalcSize(ScriptExecutionOrderInspector.m_Styles.iconToolbarMinus);
    }

    private Rect GetDraggingHandleRect(Rect r)
    {
      return new Rect(r.x + 5f, r.y + 7f, 10f, r.height - 14f);
    }

    public void DrawElement(Rect r, object obj, bool dragging)
    {
      MonoScript script = obj as MonoScript;
      if (Event.current.type == EventType.Repaint)
      {
        ScriptExecutionOrderInspector.m_Styles.elementBackground.Draw(r, false, false, false, false);
        ScriptExecutionOrderInspector.m_Styles.draggingHandle.Draw(this.GetDraggingHandleRect(r), false, false, false, false);
      }
      GUI.Label(this.GetButtonLabelRect(r), script.GetClass().FullName);
      int executionOrder = this.GetExecutionOrder(script);
      string s = EditorGUI.DelayedTextFieldInternal(this.GetFieldRect(r), executionOrder.ToString(), "0123456789-", EditorStyles.textField);
      int result = executionOrder;
      if (int.TryParse(s, out result) && result != executionOrder)
      {
        this.SetExecutionOrder(script, result);
        this.m_Edited = script;
      }
      if (!GUI.Button(this.GetAddRemoveButtonRect(r), ScriptExecutionOrderInspector.m_Styles.iconToolbarMinus, ScriptExecutionOrderInspector.m_Styles.removeButton))
        return;
      this.SetExecutionOrder(script, 0);
      this.m_Edited = script;
    }

    public class SortMonoScriptExecutionOrder : IComparer<MonoScript>
    {
      private ScriptExecutionOrderInspector inspector;

      public SortMonoScriptExecutionOrder(ScriptExecutionOrderInspector inspector)
      {
        this.inspector = inspector;
      }

      public int Compare(MonoScript x, MonoScript y)
      {
        if (!((UnityEngine.Object) x != (UnityEngine.Object) null) || !((UnityEngine.Object) y != (UnityEngine.Object) null))
          return -1;
        int executionOrder1 = this.inspector.GetExecutionOrder(x);
        int executionOrder2 = this.inspector.GetExecutionOrder(y);
        if (executionOrder1 == executionOrder2)
          return x.name.CompareTo(y.name);
        return executionOrder1.CompareTo(executionOrder2);
      }
    }

    public class SortMonoScriptNameOrder : IComparer<MonoScript>
    {
      public int Compare(MonoScript x, MonoScript y)
      {
        if ((UnityEngine.Object) x != (UnityEngine.Object) null && (UnityEngine.Object) y != (UnityEngine.Object) null)
          return x.name.CompareTo(y.name);
        return -1;
      }
    }

    public class Styles
    {
      public GUIContent helpText = EditorGUIUtility.TextContent("Add scripts to the custom order and drag them to reorder.\n\nScripts in the custom order can execute before or after the default time and are executed from top to bottom. All other scripts execute at the default time in the order they are loaded.\n\n(Changing the order of a script may modify the meta data for more than one script.)");
      public GUIContent iconToolbarPlus = EditorGUIUtility.IconContent("Toolbar Plus", "Add script to custom order");
      public GUIContent iconToolbarMinus = EditorGUIUtility.IconContent("Toolbar Minus", "Remove script from custom order");
      public GUIContent defaultTimeContent = EditorGUIUtility.TextContent("Default Time|All scripts not in the custom order are executed at the default time.");
      public GUIStyle toolbar = (GUIStyle) "TE Toolbar";
      public GUIStyle toolbarDropDown = (GUIStyle) "TE ToolbarDropDown";
      public GUIStyle boxBackground = (GUIStyle) "TE NodeBackground";
      public GUIStyle removeButton = (GUIStyle) "InvisibleButton";
      public GUIStyle elementBackground = new GUIStyle((GUIStyle) "OL Box");
      public GUIStyle defaultTime = new GUIStyle(EditorStyles.inspectorBig);
      public GUIStyle draggingHandle = (GUIStyle) "WindowBottomResize";
      public GUIStyle dropField = new GUIStyle(EditorStyles.objectFieldThumb);

      public Styles()
      {
        this.boxBackground.margin = new RectOffset();
        this.boxBackground.padding = new RectOffset(1, 1, 1, 0);
        this.elementBackground.overflow = new RectOffset(1, 1, 1, 0);
        this.defaultTime.alignment = TextAnchor.MiddleCenter;
        this.defaultTime.overflow = new RectOffset(0, 0, 1, 0);
        this.dropField.overflow = new RectOffset(2, 2, 2, 2);
        this.dropField.normal.background = (Texture2D) null;
        this.dropField.hover.background = (Texture2D) null;
        this.dropField.active.background = (Texture2D) null;
        this.dropField.focused.background = (Texture2D) null;
      }
    }

    private class DragReorderGUI
    {
      private static int s_DragReorderGUIHash = "DragReorderGUI".GetHashCode();
      private static int s_ReorderingDraggedElement;
      private static float[] s_ReorderingPositions;
      private static int[] s_ReorderingGoals;

      private static bool IsDefaultTimeElement(MonoScript element)
      {
        return element.name == string.Empty;
      }

      public static int DragReorder(Rect position, int elementHeight, List<MonoScript> elements, ScriptExecutionOrderInspector.DragReorderGUI.DrawElementDelegate drawElementDelegate)
      {
        int controlId = GUIUtility.GetControlID(ScriptExecutionOrderInspector.DragReorderGUI.s_DragReorderGUIHash, FocusType.Passive);
        Rect r = position;
        r.height = (float) elementHeight;
        int index1 = 0;
        Rect position1;
        if (GUIUtility.hotControl == controlId && Event.current.type == EventType.Repaint)
        {
          for (int index2 = 0; index2 < elements.Count; ++index2)
          {
            if (index2 != ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingDraggedElement)
            {
              if (ScriptExecutionOrderInspector.DragReorderGUI.IsDefaultTimeElement(elements[index2]))
              {
                index1 = index2;
                ++index2;
              }
              else
              {
                r.y = position.y + ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingPositions[index2] * (float) elementHeight;
                drawElementDelegate(r, (object) elements[index2], false);
              }
            }
          }
          position1 = new Rect(r.x, position.y + ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingPositions[index1] * (float) elementHeight, r.width, (float) ((double) ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingPositions[index1 + 1] - (double) ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingPositions[index1] + 1.0) * (float) elementHeight);
        }
        else
        {
          for (int index2 = 0; index2 < elements.Count; ++index2)
          {
            r.y = position.y + (float) (index2 * elementHeight);
            if (ScriptExecutionOrderInspector.DragReorderGUI.IsDefaultTimeElement(elements[index2]))
            {
              index1 = index2;
              ++index2;
            }
            else
              drawElementDelegate(r, (object) elements[index2], false);
          }
          position1 = new Rect(r.x, position.y + (float) (index1 * elementHeight), r.width, (float) (elementHeight * 2));
        }
        GUI.Label(position1, ScriptExecutionOrderInspector.m_Styles.defaultTimeContent, ScriptExecutionOrderInspector.m_Styles.defaultTime);
        bool flag = (double) position1.height > (double) elementHeight * 2.5;
        if (GUIUtility.hotControl == controlId)
        {
          if (flag)
            GUI.color = new Color(1f, 1f, 1f, 0.5f);
          r.y = position.y + ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingPositions[ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingDraggedElement] * (float) elementHeight;
          drawElementDelegate(r, (object) elements[ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingDraggedElement], true);
          GUI.color = Color.white;
        }
        int num1 = -1;
        switch (Event.current.GetTypeForControl(controlId))
        {
          case EventType.MouseDown:
            if (position.Contains(Event.current.mousePosition))
            {
              GUIUtility.keyboardControl = 0;
              EditorGUI.EndEditingActiveTextField();
              ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingDraggedElement = Mathf.FloorToInt((Event.current.mousePosition.y - position.y) / (float) elementHeight);
              if (!ScriptExecutionOrderInspector.DragReorderGUI.IsDefaultTimeElement(elements[ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingDraggedElement]))
              {
                ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingPositions = new float[elements.Count];
                ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingGoals = new int[elements.Count];
                for (int index2 = 0; index2 < elements.Count; ++index2)
                {
                  ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingGoals[index2] = index2;
                  ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingPositions[index2] = (float) index2;
                }
                GUIUtility.hotControl = controlId;
                Event.current.Use();
                break;
              }
              break;
            }
            break;
          case EventType.MouseUp:
            if (GUIUtility.hotControl == controlId)
            {
              if (ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingGoals[ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingDraggedElement] != ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingDraggedElement)
              {
                List<MonoScript> monoScriptList = new List<MonoScript>((IEnumerable<MonoScript>) elements);
                for (int index2 = 0; index2 < elements.Count; ++index2)
                  elements[ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingGoals[index2]] = monoScriptList[index2];
                num1 = ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingGoals[ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingDraggedElement];
              }
              ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingGoals = (int[]) null;
              ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingPositions = (float[]) null;
              ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingDraggedElement = -1;
              GUIUtility.hotControl = 0;
              Event.current.Use();
              break;
            }
            break;
          case EventType.MouseDrag:
            if (GUIUtility.hotControl == controlId)
            {
              ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingPositions[ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingDraggedElement] = (float) (((double) Event.current.mousePosition.y - (double) position.y) / (double) elementHeight - 0.5);
              ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingPositions[ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingDraggedElement] = Mathf.Clamp(ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingPositions[ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingDraggedElement], 0.0f, (float) (elements.Count - 1));
              int num2 = Mathf.RoundToInt(ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingPositions[ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingDraggedElement]);
              if (num2 != ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingGoals[ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingDraggedElement])
              {
                for (int index2 = 0; index2 < elements.Count; ++index2)
                  ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingGoals[index2] = index2;
                int num3 = num2 <= ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingDraggedElement ? 1 : -1;
                int reorderingDraggedElement = ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingDraggedElement;
                while (reorderingDraggedElement != num2)
                {
                  ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingGoals[reorderingDraggedElement - num3] = reorderingDraggedElement;
                  reorderingDraggedElement -= num3;
                }
                ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingGoals[ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingDraggedElement] = num2;
              }
              Event.current.Use();
              break;
            }
            break;
          case EventType.Repaint:
            if (GUIUtility.hotControl == controlId)
            {
              for (int index2 = 0; index2 < elements.Count; ++index2)
              {
                if (index2 != ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingDraggedElement)
                  ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingPositions[index2] = Mathf.MoveTowards(ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingPositions[index2], (float) ScriptExecutionOrderInspector.DragReorderGUI.s_ReorderingGoals[index2], 0.075f);
              }
              GUIView.current.Repaint();
              break;
            }
            break;
        }
        return num1;
      }

      public delegate void DrawElementDelegate(Rect r, object obj, bool dragging);
    }
  }
}
