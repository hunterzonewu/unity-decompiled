// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.ProfilerInstrumentationPopup
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class ProfilerInstrumentationPopup : PopupWindowContent
  {
    private static GUIContent s_AutoInstrumentScriptsContent = new GUIContent("Auto instrument " + InstrumentedAssemblyTypes.Script.ToString() + " assemblies");
    private const string kAutoInstrumentSettingKey = "ProfilerAutoInstrumentedAssemblyTypes";
    private const int kAutoInstrumentButtonHeight = 20;
    private const int kAutoInstrumentButtonsHeight = 20;
    private static Dictionary<string, int> s_InstrumentableFunctions;
    private static ProfilerInstrumentationPopup s_PendingPopup;
    private PopupList m_FunctionsList;
    private ProfilerInstrumentationPopup.InputData m_FunctionsListInputData;
    private bool m_ShowAllCheckbox;
    private bool m_ShowAutoInstrumemtationParams;
    private InstrumentedAssemblyTypes m_AutoInstrumentedAssemblyTypes;
    private PopupList.ListElement m_AllCheckbox;

    public static bool InstrumentationEnabled
    {
      get
      {
        return false;
      }
    }

    public ProfilerInstrumentationPopup(Dictionary<string, int> functions, bool showAllCheckbox, bool showAutoInstrumemtationParams)
    {
      this.m_ShowAutoInstrumemtationParams = showAutoInstrumemtationParams;
      this.m_ShowAllCheckbox = showAllCheckbox;
      this.m_AutoInstrumentedAssemblyTypes = (InstrumentedAssemblyTypes) SessionState.GetInt("ProfilerAutoInstrumentedAssemblyTypes", 0);
      this.m_FunctionsListInputData = new ProfilerInstrumentationPopup.InputData();
      this.m_FunctionsListInputData.m_CloseOnSelection = false;
      this.m_FunctionsListInputData.m_AllowCustom = true;
      this.m_FunctionsListInputData.m_MaxCount = 0;
      this.m_FunctionsListInputData.m_EnableAutoCompletion = false;
      this.m_FunctionsListInputData.m_SortAlphabetically = true;
      this.m_FunctionsListInputData.m_OnSelectCallback = new PopupList.OnSelectCallback(this.ProfilerInstrumentationPopupCallback);
      this.SetFunctions(functions);
      this.m_FunctionsList = new PopupList((PopupList.InputData) this.m_FunctionsListInputData);
    }

    private void SetFunctions(Dictionary<string, int> functions)
    {
      this.m_FunctionsListInputData.m_ListElements.Clear();
      if (functions == null)
        this.m_FunctionsListInputData.NewOrMatchingElement("Querying instrumentable functions...").enabled = false;
      else if (functions.Count == 0)
      {
        this.m_FunctionsListInputData.NewOrMatchingElement("No instrumentable child functions found").enabled = false;
      }
      else
      {
        this.m_FunctionsListInputData.m_MaxCount = Mathf.Clamp(functions.Count + 1, 0, 30);
        if (this.m_ShowAllCheckbox)
        {
          this.m_AllCheckbox = new PopupList.ListElement(" All", false, float.MaxValue);
          this.m_FunctionsListInputData.m_ListElements.Add(this.m_AllCheckbox);
        }
        using (Dictionary<string, int>.Enumerator enumerator = functions.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<string, int> current = enumerator.Current;
            PopupList.ListElement listElement = new PopupList.ListElement(current.Key, current.Value != 0);
            listElement.ResetScore();
            this.m_FunctionsListInputData.m_ListElements.Add(listElement);
          }
        }
        if (!this.m_ShowAllCheckbox)
          return;
        this.UpdateAllCheckbox();
      }
    }

    public override void OnGUI(Rect rect)
    {
      Rect rect1 = new Rect(rect);
      if (this.m_ShowAutoInstrumemtationParams)
      {
        Rect position = new Rect(rect1);
        position.height = 20f;
        InstrumentedAssemblyTypes instrumentedAssemblyTypes = InstrumentedAssemblyTypes.None;
        if (GUI.Toggle(position, (this.m_AutoInstrumentedAssemblyTypes & InstrumentedAssemblyTypes.Script) != InstrumentedAssemblyTypes.None, ProfilerInstrumentationPopup.s_AutoInstrumentScriptsContent))
          instrumentedAssemblyTypes |= InstrumentedAssemblyTypes.Script;
        if (instrumentedAssemblyTypes != this.m_AutoInstrumentedAssemblyTypes)
        {
          this.m_AutoInstrumentedAssemblyTypes = instrumentedAssemblyTypes;
          ProfilerDriver.SetAutoInstrumentedAssemblies(this.m_AutoInstrumentedAssemblyTypes);
          SessionState.SetInt("ProfilerAutoInstrumentedAssemblyTypes", (int) this.m_AutoInstrumentedAssemblyTypes);
        }
        rect1.y += 20f;
        rect1.height -= 20f;
      }
      this.m_FunctionsList.OnGUI(rect1);
    }

    public override void OnClose()
    {
      this.m_FunctionsList.OnClose();
    }

    public override Vector2 GetWindowSize()
    {
      Vector2 windowSize = this.m_FunctionsList.GetWindowSize();
      windowSize.x = 450f;
      if (this.m_ShowAutoInstrumemtationParams)
        windowSize.y += 20f;
      return windowSize;
    }

    public void UpdateAllCheckbox()
    {
      if (this.m_AllCheckbox == null)
        return;
      bool flag1 = false;
      bool flag2 = true;
      using (List<PopupList.ListElement>.Enumerator enumerator = this.m_FunctionsListInputData.m_ListElements.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          PopupList.ListElement current = enumerator.Current;
          if (current != this.m_AllCheckbox)
          {
            if (current.selected)
              flag1 = true;
            else
              flag2 = false;
          }
        }
      }
      this.m_AllCheckbox.selected = flag2;
      this.m_AllCheckbox.partiallySelected = flag1 && !flag2;
    }

    private static void SetFunctionNamesFromUnity(bool allFunction, string[] functionNames, int[] isInstrumentedFlags)
    {
      Dictionary<string, int> functions = new Dictionary<string, int>(functionNames.Length);
      for (int index = 0; index < functionNames.Length; ++index)
        functions.Add(functionNames[index], isInstrumentedFlags[index]);
      if (allFunction)
        ProfilerInstrumentationPopup.s_InstrumentableFunctions = functions;
      if (ProfilerInstrumentationPopup.s_PendingPopup == null)
        return;
      ProfilerInstrumentationPopup.s_PendingPopup.SetFunctions(functions);
      ProfilerInstrumentationPopup.s_PendingPopup = (ProfilerInstrumentationPopup) null;
    }

    public static void UpdateInstrumentableFunctions()
    {
      ProfilerDriver.QueryInstrumentableFunctions();
    }

    public static void Show(Rect r)
    {
      ProfilerInstrumentationPopup instrumentationPopup = new ProfilerInstrumentationPopup(ProfilerInstrumentationPopup.s_InstrumentableFunctions, false, true);
      if (ProfilerInstrumentationPopup.s_InstrumentableFunctions == null)
      {
        ProfilerInstrumentationPopup.s_PendingPopup = instrumentationPopup;
        ProfilerDriver.QueryInstrumentableFunctions();
      }
      else
        ProfilerInstrumentationPopup.s_PendingPopup = (ProfilerInstrumentationPopup) null;
      PopupWindow.Show(r, (PopupWindowContent) instrumentationPopup);
    }

    public static void Show(Rect r, string funcName)
    {
      ProfilerInstrumentationPopup instrumentationPopup = new ProfilerInstrumentationPopup((Dictionary<string, int>) null, true, false);
      ProfilerInstrumentationPopup.s_PendingPopup = instrumentationPopup;
      ProfilerDriver.QueryFunctionCallees(funcName);
      PopupWindow.Show(r, (PopupWindowContent) instrumentationPopup);
    }

    public static bool FunctionHasInstrumentationPopup(string funcName)
    {
      if (ProfilerInstrumentationPopup.s_InstrumentableFunctions != null)
        return ProfilerInstrumentationPopup.s_InstrumentableFunctions.ContainsKey(funcName);
      return false;
    }

    private void ProfilerInstrumentationPopupCallback(PopupList.ListElement element)
    {
      if (element == this.m_AllCheckbox)
      {
        element.selected = !element.selected;
        using (List<PopupList.ListElement>.Enumerator enumerator = this.m_FunctionsListInputData.m_ListElements.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            PopupList.ListElement current = enumerator.Current;
            if (element.selected)
              ProfilerDriver.BeginInstrumentFunction(current.text);
            else
              ProfilerDriver.EndInstrumentFunction(current.text);
            current.selected = element.selected;
          }
        }
      }
      else
      {
        element.selected = !element.selected;
        if (element.selected)
          ProfilerDriver.BeginInstrumentFunction(element.text);
        else
          ProfilerDriver.EndInstrumentFunction(element.text);
      }
      this.UpdateAllCheckbox();
    }

    private class InputData : PopupList.InputData
    {
      public override IEnumerable<PopupList.ListElement> BuildQuery(string prefix)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ProfilerInstrumentationPopup.InputData.\u003CBuildQuery\u003Ec__AnonStoreyAB queryCAnonStoreyAb = new ProfilerInstrumentationPopup.InputData.\u003CBuildQuery\u003Ec__AnonStoreyAB();
        // ISSUE: reference to a compiler-generated field
        queryCAnonStoreyAb.prefix = prefix;
        // ISSUE: reference to a compiler-generated field
        if (queryCAnonStoreyAb.prefix == string.Empty)
          return (IEnumerable<PopupList.ListElement>) this.m_ListElements;
        // ISSUE: reference to a compiler-generated method
        return this.m_ListElements.Where<PopupList.ListElement>(new Func<PopupList.ListElement, bool>(queryCAnonStoreyAb.\u003C\u003Em__1F4));
      }
    }
  }
}
