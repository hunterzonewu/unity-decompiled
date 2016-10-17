// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.UnityEventDrawer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditorInternal
{
  [CustomPropertyDrawer(typeof (UnityEventBase), true)]
  public class UnityEventDrawer : PropertyDrawer
  {
    private Dictionary<string, UnityEventDrawer.State> m_States = new Dictionary<string, UnityEventDrawer.State>();
    private const string kNoFunctionString = "No Function";
    private const string kInstancePath = "m_Target";
    private const string kCallStatePath = "m_CallState";
    private const string kArgumentsPath = "m_Arguments";
    private const string kModePath = "m_Mode";
    private const string kMethodNamePath = "m_MethodName";
    private const string kFloatArgument = "m_FloatArgument";
    private const string kIntArgument = "m_IntArgument";
    private const string kObjectArgument = "m_ObjectArgument";
    private const string kStringArgument = "m_StringArgument";
    private const string kBoolArgument = "m_BoolArgument";
    private const string kObjectArgumentAssemblyTypeName = "m_ObjectArgumentAssemblyTypeName";
    private const int kExtraSpacing = 9;
    private UnityEventDrawer.Styles m_Styles;
    private string m_Text;
    private UnityEventBase m_DummyEvent;
    private SerializedProperty m_Prop;
    private SerializedProperty m_ListenersArray;
    private ReorderableList m_ReorderableList;
    private int m_LastSelectedIndex;

    private static string GetEventParams(UnityEventBase evt)
    {
      MethodInfo method = evt.FindMethod("Invoke", (object) evt, PersistentListenerMode.EventDefined, (System.Type) null);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(" (");
      System.Type[] array = ((IEnumerable<System.Reflection.ParameterInfo>) method.GetParameters()).Select<System.Reflection.ParameterInfo, System.Type>((Func<System.Reflection.ParameterInfo, System.Type>) (x => x.ParameterType)).ToArray<System.Type>();
      for (int index = 0; index < array.Length; ++index)
      {
        stringBuilder.Append(array[index].Name);
        if (index < array.Length - 1)
          stringBuilder.Append(", ");
      }
      stringBuilder.Append(")");
      return stringBuilder.ToString();
    }

    private UnityEventDrawer.State GetState(SerializedProperty prop)
    {
      string propertyPath = prop.propertyPath;
      UnityEventDrawer.State state;
      this.m_States.TryGetValue(propertyPath, out state);
      if (state == null)
      {
        state = new UnityEventDrawer.State();
        SerializedProperty propertyRelative = prop.FindPropertyRelative("m_PersistentCalls.m_Calls");
        state.m_ReorderableList = new ReorderableList(prop.serializedObject, propertyRelative, false, true, true, true);
        state.m_ReorderableList.drawHeaderCallback = new ReorderableList.HeaderCallbackDelegate(this.DrawEventHeader);
        state.m_ReorderableList.drawElementCallback = new ReorderableList.ElementCallbackDelegate(this.DrawEventListener);
        state.m_ReorderableList.onSelectCallback = new ReorderableList.SelectCallbackDelegate(this.SelectEventListener);
        state.m_ReorderableList.onReorderCallback = new ReorderableList.ReorderCallbackDelegate(this.EndDragChild);
        state.m_ReorderableList.onAddCallback = new ReorderableList.AddCallbackDelegate(this.AddEventListener);
        state.m_ReorderableList.onRemoveCallback = new ReorderableList.RemoveCallbackDelegate(this.RemoveButton);
        state.m_ReorderableList.elementHeight = 43f;
        this.m_States[propertyPath] = state;
      }
      return state;
    }

    private UnityEventDrawer.State RestoreState(SerializedProperty property)
    {
      UnityEventDrawer.State state = this.GetState(property);
      this.m_ListenersArray = state.m_ReorderableList.serializedProperty;
      this.m_ReorderableList = state.m_ReorderableList;
      this.m_LastSelectedIndex = state.lastSelectedIndex;
      this.m_ReorderableList.index = this.m_LastSelectedIndex;
      return state;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      this.m_Prop = property;
      this.m_Text = label.text;
      UnityEventDrawer.State state = this.RestoreState(property);
      this.OnGUI(position);
      state.lastSelectedIndex = this.m_LastSelectedIndex;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      this.RestoreState(property);
      float num = 0.0f;
      if (this.m_ReorderableList != null)
        num = this.m_ReorderableList.GetHeight();
      return num;
    }

    public void OnGUI(Rect position)
    {
      if (this.m_ListenersArray == null || !this.m_ListenersArray.isArray)
        return;
      this.m_DummyEvent = UnityEventDrawer.GetDummyEvent(this.m_Prop);
      if (this.m_DummyEvent == null)
        return;
      if (this.m_Styles == null)
        this.m_Styles = new UnityEventDrawer.Styles();
      if (this.m_ReorderableList == null)
        return;
      int indentLevel = EditorGUI.indentLevel;
      EditorGUI.indentLevel = 0;
      this.m_ReorderableList.DoList(position);
      EditorGUI.indentLevel = indentLevel;
    }

    protected virtual void DrawEventHeader(Rect headerRect)
    {
      headerRect.height = 16f;
      string text = (!string.IsNullOrEmpty(this.m_Text) ? this.m_Text : "Event") + UnityEventDrawer.GetEventParams(this.m_DummyEvent);
      GUI.Label(headerRect, text);
    }

    private static PersistentListenerMode GetMode(SerializedProperty mode)
    {
      return (PersistentListenerMode) mode.enumValueIndex;
    }

    private void DrawEventListener(Rect rect, int index, bool isactive, bool isfocused)
    {
      SerializedProperty arrayElementAtIndex = this.m_ListenersArray.GetArrayElementAtIndex(index);
      ++rect.y;
      Rect[] rowRects = this.GetRowRects(rect);
      Rect position1 = rowRects[0];
      Rect position2 = rowRects[1];
      Rect rect1 = rowRects[2];
      Rect position3 = rowRects[3];
      SerializedProperty propertyRelative1 = arrayElementAtIndex.FindPropertyRelative("m_CallState");
      SerializedProperty propertyRelative2 = arrayElementAtIndex.FindPropertyRelative("m_Mode");
      SerializedProperty propertyRelative3 = arrayElementAtIndex.FindPropertyRelative("m_Arguments");
      SerializedProperty propertyRelative4 = arrayElementAtIndex.FindPropertyRelative("m_Target");
      SerializedProperty propertyRelative5 = arrayElementAtIndex.FindPropertyRelative("m_MethodName");
      Color backgroundColor = GUI.backgroundColor;
      GUI.backgroundColor = Color.white;
      EditorGUI.PropertyField(position1, propertyRelative1, GUIContent.none);
      EditorGUI.BeginChangeCheck();
      GUI.Box(position2, GUIContent.none);
      EditorGUI.PropertyField(position2, propertyRelative4, GUIContent.none);
      if (EditorGUI.EndChangeCheck())
        propertyRelative5.stringValue = (string) null;
      PersistentListenerMode persistentListenerMode = UnityEventDrawer.GetMode(propertyRelative2);
      if (propertyRelative4.objectReferenceValue == (UnityEngine.Object) null || string.IsNullOrEmpty(propertyRelative5.stringValue))
        persistentListenerMode = PersistentListenerMode.Void;
      SerializedProperty propertyRelative6;
      switch (persistentListenerMode)
      {
        case PersistentListenerMode.Object:
          propertyRelative6 = propertyRelative3.FindPropertyRelative("m_ObjectArgument");
          break;
        case PersistentListenerMode.Int:
          propertyRelative6 = propertyRelative3.FindPropertyRelative("m_IntArgument");
          break;
        case PersistentListenerMode.Float:
          propertyRelative6 = propertyRelative3.FindPropertyRelative("m_FloatArgument");
          break;
        case PersistentListenerMode.String:
          propertyRelative6 = propertyRelative3.FindPropertyRelative("m_StringArgument");
          break;
        case PersistentListenerMode.Bool:
          propertyRelative6 = propertyRelative3.FindPropertyRelative("m_BoolArgument");
          break;
        default:
          propertyRelative6 = propertyRelative3.FindPropertyRelative("m_IntArgument");
          break;
      }
      string stringValue = propertyRelative3.FindPropertyRelative("m_ObjectArgumentAssemblyTypeName").stringValue;
      System.Type type = typeof (UnityEngine.Object);
      if (!string.IsNullOrEmpty(stringValue))
        type = System.Type.GetType(stringValue, false) ?? typeof (UnityEngine.Object);
      if (persistentListenerMode == PersistentListenerMode.Object)
      {
        EditorGUI.BeginChangeCheck();
        UnityEngine.Object @object = EditorGUI.ObjectField(position3, GUIContent.none, propertyRelative6.objectReferenceValue, type, true);
        if (EditorGUI.EndChangeCheck())
          propertyRelative6.objectReferenceValue = @object;
      }
      else if (persistentListenerMode != PersistentListenerMode.Void && persistentListenerMode != PersistentListenerMode.EventDefined)
        EditorGUI.PropertyField(position3, propertyRelative6, GUIContent.none);
      EditorGUI.BeginDisabledGroup(propertyRelative4.objectReferenceValue == (UnityEngine.Object) null);
      EditorGUI.BeginProperty(rect1, GUIContent.none, propertyRelative5);
      GUIContent content;
      if (EditorGUI.showMixedValue)
      {
        content = EditorGUI.mixedValueContent;
      }
      else
      {
        StringBuilder stringBuilder = new StringBuilder();
        if (propertyRelative4.objectReferenceValue == (UnityEngine.Object) null || string.IsNullOrEmpty(propertyRelative5.stringValue))
          stringBuilder.Append("No Function");
        else if (!UnityEventDrawer.IsPersistantListenerValid(this.m_DummyEvent, propertyRelative5.stringValue, propertyRelative4.objectReferenceValue, UnityEventDrawer.GetMode(propertyRelative2), type))
        {
          string str = "UnknownComponent";
          UnityEngine.Object objectReferenceValue = propertyRelative4.objectReferenceValue;
          if (objectReferenceValue != (UnityEngine.Object) null)
            str = objectReferenceValue.GetType().Name;
          stringBuilder.Append(string.Format("<Missing {0}.{1}>", (object) str, (object) propertyRelative5.stringValue));
        }
        else
        {
          stringBuilder.Append(propertyRelative4.objectReferenceValue.GetType().Name);
          if (!string.IsNullOrEmpty(propertyRelative5.stringValue))
          {
            stringBuilder.Append(".");
            if (propertyRelative5.stringValue.StartsWith("set_"))
              stringBuilder.Append(propertyRelative5.stringValue.Substring(4));
            else
              stringBuilder.Append(propertyRelative5.stringValue);
          }
        }
        content = GUIContent.Temp(stringBuilder.ToString());
      }
      if (GUI.Button(rect1, content, EditorStyles.popup))
        UnityEventDrawer.BuildPopupList(propertyRelative4.objectReferenceValue, this.m_DummyEvent, arrayElementAtIndex).DropDown(rect1);
      EditorGUI.EndProperty();
      EditorGUI.EndDisabledGroup();
      GUI.backgroundColor = backgroundColor;
    }

    private Rect[] GetRowRects(Rect rect)
    {
      Rect[] rectArray = new Rect[4];
      rect.height = 16f;
      rect.y += 2f;
      Rect rect1 = rect;
      rect1.width *= 0.3f;
      Rect rect2 = rect1;
      rect2.y += EditorGUIUtility.singleLineHeight + 2f;
      Rect rect3 = rect;
      rect3.xMin = rect2.xMax + 5f;
      Rect rect4 = rect3;
      rect4.y += EditorGUIUtility.singleLineHeight + 2f;
      rectArray[0] = rect1;
      rectArray[1] = rect2;
      rectArray[2] = rect3;
      rectArray[3] = rect4;
      return rectArray;
    }

    private void RemoveButton(ReorderableList list)
    {
      ReorderableList.defaultBehaviours.DoRemoveButton(list);
      this.m_LastSelectedIndex = list.index;
    }

    private void AddEventListener(ReorderableList list)
    {
      if (this.m_ListenersArray.hasMultipleDifferentValues)
      {
        foreach (UnityEngine.Object targetObject in this.m_ListenersArray.serializedObject.targetObjects)
        {
          SerializedObject serializedObject = new SerializedObject(targetObject);
          ++serializedObject.FindProperty(this.m_ListenersArray.propertyPath).arraySize;
          serializedObject.ApplyModifiedProperties();
        }
        this.m_ListenersArray.serializedObject.SetIsDifferentCacheDirty();
        this.m_ListenersArray.serializedObject.Update();
        list.index = list.serializedProperty.arraySize - 1;
      }
      else
        ReorderableList.defaultBehaviours.DoAddButton(list);
      this.m_LastSelectedIndex = list.index;
      SerializedProperty arrayElementAtIndex = this.m_ListenersArray.GetArrayElementAtIndex(list.index);
      SerializedProperty propertyRelative1 = arrayElementAtIndex.FindPropertyRelative("m_CallState");
      SerializedProperty propertyRelative2 = arrayElementAtIndex.FindPropertyRelative("m_Target");
      SerializedProperty propertyRelative3 = arrayElementAtIndex.FindPropertyRelative("m_MethodName");
      SerializedProperty propertyRelative4 = arrayElementAtIndex.FindPropertyRelative("m_Mode");
      SerializedProperty propertyRelative5 = arrayElementAtIndex.FindPropertyRelative("m_Arguments");
      propertyRelative1.enumValueIndex = 2;
      propertyRelative2.objectReferenceValue = (UnityEngine.Object) null;
      propertyRelative3.stringValue = (string) null;
      propertyRelative4.enumValueIndex = 1;
      propertyRelative5.FindPropertyRelative("m_FloatArgument").floatValue = 0.0f;
      propertyRelative5.FindPropertyRelative("m_IntArgument").intValue = 0;
      propertyRelative5.FindPropertyRelative("m_ObjectArgument").objectReferenceValue = (UnityEngine.Object) null;
      propertyRelative5.FindPropertyRelative("m_StringArgument").stringValue = (string) null;
      propertyRelative5.FindPropertyRelative("m_ObjectArgumentAssemblyTypeName").stringValue = (string) null;
    }

    private void SelectEventListener(ReorderableList list)
    {
      this.m_LastSelectedIndex = list.index;
    }

    private void EndDragChild(ReorderableList list)
    {
      this.m_LastSelectedIndex = list.index;
    }

    private static UnityEventBase GetDummyEvent(SerializedProperty prop)
    {
      System.Type type = System.Type.GetType(prop.FindPropertyRelative("m_TypeName").stringValue, false);
      if (type == null)
        return (UnityEventBase) new UnityEvent();
      return Activator.CreateInstance(type) as UnityEventBase;
    }

    private static IEnumerable<UnityEventDrawer.ValidMethodMap> CalculateMethodMap(UnityEngine.Object target, System.Type[] t, bool allowSubclasses)
    {
      List<UnityEventDrawer.ValidMethodMap> validMethodMapList = new List<UnityEventDrawer.ValidMethodMap>();
      if (target == (UnityEngine.Object) null || t == null)
        return (IEnumerable<UnityEventDrawer.ValidMethodMap>) validMethodMapList;
      System.Type type = target.GetType();
      List<MethodInfo> list = ((IEnumerable<MethodInfo>) type.GetMethods()).Where<MethodInfo>((Func<MethodInfo, bool>) (x => !x.IsSpecialName)).ToList<MethodInfo>();
      IEnumerable<PropertyInfo> source = ((IEnumerable<PropertyInfo>) type.GetProperties()).AsEnumerable<PropertyInfo>().Where<PropertyInfo>((Func<PropertyInfo, bool>) (x =>
      {
        if (x.GetCustomAttributes(typeof (ObsoleteAttribute), true).Length == 0)
          return x.GetSetMethod() != null;
        return false;
      }));
      list.AddRange(source.Select<PropertyInfo, MethodInfo>((Func<PropertyInfo, MethodInfo>) (x => x.GetSetMethod())));
      using (List<MethodInfo>.Enumerator enumerator = list.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          MethodInfo current = enumerator.Current;
          System.Reflection.ParameterInfo[] parameters = current.GetParameters();
          if (parameters.Length == t.Length && current.GetCustomAttributes(typeof (ObsoleteAttribute), true).Length <= 0 && current.ReturnType == typeof (void))
          {
            bool flag = true;
            for (int index = 0; index < t.Length; ++index)
            {
              if (!parameters[index].ParameterType.IsAssignableFrom(t[index]))
                flag = false;
              if (allowSubclasses && t[index].IsAssignableFrom(parameters[index].ParameterType))
                flag = true;
            }
            if (flag)
              validMethodMapList.Add(new UnityEventDrawer.ValidMethodMap()
              {
                target = target,
                methodInfo = current
              });
          }
        }
      }
      return (IEnumerable<UnityEventDrawer.ValidMethodMap>) validMethodMapList;
    }

    public static bool IsPersistantListenerValid(UnityEventBase dummyEvent, string methodName, UnityEngine.Object uObject, PersistentListenerMode modeEnum, System.Type argumentType)
    {
      if (uObject == (UnityEngine.Object) null || string.IsNullOrEmpty(methodName))
        return false;
      return dummyEvent.FindMethod(methodName, (object) uObject, modeEnum, argumentType) != null;
    }

    private static GenericMenu BuildPopupList(UnityEngine.Object target, UnityEventBase dummyEvent, SerializedProperty listener)
    {
      UnityEngine.Object target1 = target;
      if (target1 is Component)
        target1 = (UnityEngine.Object) (target as Component).gameObject;
      SerializedProperty propertyRelative = listener.FindPropertyRelative("m_MethodName");
      GenericMenu menu = new GenericMenu();
      menu.AddItem(new GUIContent("No Function"), string.IsNullOrEmpty(propertyRelative.stringValue), new GenericMenu.MenuFunction2(UnityEventDrawer.ClearEventFunction), (object) new UnityEventDrawer.UnityEventFunction(listener, (UnityEngine.Object) null, (MethodInfo) null, PersistentListenerMode.EventDefined));
      if (target1 == (UnityEngine.Object) null)
        return menu;
      menu.AddSeparator(string.Empty);
      System.Type[] array = ((IEnumerable<System.Reflection.ParameterInfo>) dummyEvent.GetType().GetMethod("Invoke").GetParameters()).Select<System.Reflection.ParameterInfo, System.Type>((Func<System.Reflection.ParameterInfo, System.Type>) (x => x.ParameterType)).ToArray<System.Type>();
      UnityEventDrawer.GeneratePopUpForType(menu, target1, false, listener, array);
      if (target1 is GameObject)
      {
        Component[] components = (target1 as GameObject).GetComponents<Component>();
        List<string> list = ((IEnumerable<Component>) components).Where<Component>((Func<Component, bool>) (c => (UnityEngine.Object) c != (UnityEngine.Object) null)).Select<Component, string>((Func<Component, string>) (c => c.GetType().Name)).GroupBy<string, string>((Func<string, string>) (x => x)).Where<IGrouping<string, string>>((Func<IGrouping<string, string>, bool>) (g => g.Count<string>() > 1)).Select<IGrouping<string, string>, string>((Func<IGrouping<string, string>, string>) (g => g.Key)).ToList<string>();
        foreach (Component component in components)
        {
          if (!((UnityEngine.Object) component == (UnityEngine.Object) null))
            UnityEventDrawer.GeneratePopUpForType(menu, (UnityEngine.Object) component, list.Contains(component.GetType().Name), listener, array);
        }
      }
      return menu;
    }

    private static void GeneratePopUpForType(GenericMenu menu, UnityEngine.Object target, bool useFullTargetName, SerializedProperty listener, System.Type[] delegateArgumentsTypes)
    {
      List<UnityEventDrawer.ValidMethodMap> methods = new List<UnityEventDrawer.ValidMethodMap>();
      string targetName = !useFullTargetName ? target.GetType().Name : target.GetType().FullName;
      bool flag = false;
      if (delegateArgumentsTypes.Length != 0)
      {
        UnityEventDrawer.GetMethodsForTargetAndMode(target, delegateArgumentsTypes, methods, PersistentListenerMode.EventDefined);
        if (methods.Count > 0)
        {
          menu.AddDisabledItem(new GUIContent(targetName + "/Dynamic " + string.Join(", ", ((IEnumerable<System.Type>) delegateArgumentsTypes).Select<System.Type, string>((Func<System.Type, string>) (e => UnityEventDrawer.GetTypeName(e))).ToArray<string>())));
          UnityEventDrawer.AddMethodsToMenu(menu, listener, methods, targetName);
          flag = true;
        }
      }
      methods.Clear();
      UnityEventDrawer.GetMethodsForTargetAndMode(target, new System.Type[1]{ typeof (float) }, methods, PersistentListenerMode.Float);
      UnityEventDrawer.GetMethodsForTargetAndMode(target, new System.Type[1]{ typeof (int) }, methods, PersistentListenerMode.Int);
      UnityEventDrawer.GetMethodsForTargetAndMode(target, new System.Type[1]{ typeof (string) }, methods, PersistentListenerMode.String);
      UnityEventDrawer.GetMethodsForTargetAndMode(target, new System.Type[1]{ typeof (bool) }, methods, PersistentListenerMode.Bool);
      UnityEventDrawer.GetMethodsForTargetAndMode(target, new System.Type[1]{ typeof (UnityEngine.Object) }, methods, PersistentListenerMode.Object);
      UnityEventDrawer.GetMethodsForTargetAndMode(target, new System.Type[0], methods, PersistentListenerMode.Void);
      if (methods.Count <= 0)
        return;
      if (flag)
        menu.AddItem(new GUIContent(targetName + "/ "), false, (GenericMenu.MenuFunction) null);
      if (delegateArgumentsTypes.Length != 0)
        menu.AddDisabledItem(new GUIContent(targetName + "/Static Parameters"));
      UnityEventDrawer.AddMethodsToMenu(menu, listener, methods, targetName);
    }

    private static void AddMethodsToMenu(GenericMenu menu, SerializedProperty listener, List<UnityEventDrawer.ValidMethodMap> methods, string targetName)
    {
      foreach (UnityEventDrawer.ValidMethodMap method in (IEnumerable<UnityEventDrawer.ValidMethodMap>) methods.OrderBy<UnityEventDrawer.ValidMethodMap, int>((Func<UnityEventDrawer.ValidMethodMap, int>) (e => e.methodInfo.Name.StartsWith("set_") ? 0 : 1)).ThenBy<UnityEventDrawer.ValidMethodMap, string>((Func<UnityEventDrawer.ValidMethodMap, string>) (e => e.methodInfo.Name)))
        UnityEventDrawer.AddFunctionsForScript(menu, listener, method, targetName);
    }

    private static void GetMethodsForTargetAndMode(UnityEngine.Object target, System.Type[] delegateArgumentsTypes, List<UnityEventDrawer.ValidMethodMap> methods, PersistentListenerMode mode)
    {
      foreach (UnityEventDrawer.ValidMethodMap method in UnityEventDrawer.CalculateMethodMap(target, delegateArgumentsTypes, mode == PersistentListenerMode.Object))
      {
        method.mode = mode;
        methods.Add(method);
      }
    }

    private static void AddFunctionsForScript(GenericMenu menu, SerializedProperty listener, UnityEventDrawer.ValidMethodMap method, string targetName)
    {
      PersistentListenerMode mode1 = method.mode;
      UnityEngine.Object objectReferenceValue = listener.FindPropertyRelative("m_Target").objectReferenceValue;
      string stringValue = listener.FindPropertyRelative("m_MethodName").stringValue;
      PersistentListenerMode mode2 = UnityEventDrawer.GetMode(listener.FindPropertyRelative("m_Mode"));
      SerializedProperty propertyRelative = listener.FindPropertyRelative("m_Arguments").FindPropertyRelative("m_ObjectArgumentAssemblyTypeName");
      StringBuilder stringBuilder = new StringBuilder();
      int length = method.methodInfo.GetParameters().Length;
      for (int index = 0; index < length; ++index)
      {
        System.Reflection.ParameterInfo parameter = method.methodInfo.GetParameters()[index];
        stringBuilder.Append(string.Format("{0}", (object) UnityEventDrawer.GetTypeName(parameter.ParameterType)));
        if (index < length - 1)
          stringBuilder.Append(", ");
      }
      bool on = objectReferenceValue == method.target && stringValue == method.methodInfo.Name && mode1 == mode2;
      if (on && mode1 == PersistentListenerMode.Object && method.methodInfo.GetParameters().Length == 1)
        on &= method.methodInfo.GetParameters()[0].ParameterType.AssemblyQualifiedName == propertyRelative.stringValue;
      string formattedMethodName = UnityEventDrawer.GetFormattedMethodName(targetName, method.methodInfo.Name, stringBuilder.ToString(), mode1 == PersistentListenerMode.EventDefined);
      menu.AddItem(new GUIContent(formattedMethodName), on, new GenericMenu.MenuFunction2(UnityEventDrawer.SetEventFunction), (object) new UnityEventDrawer.UnityEventFunction(listener, method.target, method.methodInfo, mode1));
    }

    private static string GetTypeName(System.Type t)
    {
      if (t == typeof (int))
        return "int";
      if (t == typeof (float))
        return "float";
      if (t == typeof (string))
        return "string";
      if (t == typeof (bool))
        return "bool";
      return t.Name;
    }

    private static string GetFormattedMethodName(string targetName, string methodName, string args, bool dynamic)
    {
      if (dynamic)
      {
        if (methodName.StartsWith("set_"))
          return string.Format("{0}/{1}", (object) targetName, (object) methodName.Substring(4));
        return string.Format("{0}/{1}", (object) targetName, (object) methodName);
      }
      if (methodName.StartsWith("set_"))
        return string.Format("{0}/{2} {1}", (object) targetName, (object) methodName.Substring(4), (object) args);
      return string.Format("{0}/{1} ({2})", (object) targetName, (object) methodName, (object) args);
    }

    private static void SetEventFunction(object source)
    {
      ((UnityEventDrawer.UnityEventFunction) source).Assign();
    }

    private static void ClearEventFunction(object source)
    {
      ((UnityEventDrawer.UnityEventFunction) source).Clear();
    }

    protected class State
    {
      internal ReorderableList m_ReorderableList;
      public int lastSelectedIndex;
    }

    private class Styles
    {
      public readonly GUIContent iconToolbarMinus = EditorGUIUtility.IconContent("Toolbar Minus");
      public readonly GUIStyle genericFieldStyle = EditorStyles.label;
      public readonly GUIStyle removeButton = (GUIStyle) "InvisibleButton";
    }

    private struct ValidMethodMap
    {
      public UnityEngine.Object target;
      public MethodInfo methodInfo;
      public PersistentListenerMode mode;
    }

    private struct UnityEventFunction
    {
      private readonly SerializedProperty m_Listener;
      private readonly UnityEngine.Object m_Target;
      private readonly MethodInfo m_Method;
      private readonly PersistentListenerMode m_Mode;

      public UnityEventFunction(SerializedProperty listener, UnityEngine.Object target, MethodInfo method, PersistentListenerMode mode)
      {
        this.m_Listener = listener;
        this.m_Target = target;
        this.m_Method = method;
        this.m_Mode = mode;
      }

      public void Assign()
      {
        SerializedProperty propertyRelative1 = this.m_Listener.FindPropertyRelative("m_Target");
        SerializedProperty propertyRelative2 = this.m_Listener.FindPropertyRelative("m_MethodName");
        SerializedProperty propertyRelative3 = this.m_Listener.FindPropertyRelative("m_Mode");
        SerializedProperty propertyRelative4 = this.m_Listener.FindPropertyRelative("m_Arguments");
        propertyRelative1.objectReferenceValue = this.m_Target;
        propertyRelative2.stringValue = this.m_Method.Name;
        propertyRelative3.enumValueIndex = (int) this.m_Mode;
        if (this.m_Mode == PersistentListenerMode.Object)
        {
          SerializedProperty propertyRelative5 = propertyRelative4.FindPropertyRelative("m_ObjectArgumentAssemblyTypeName");
          System.Reflection.ParameterInfo[] parameters = this.m_Method.GetParameters();
          propertyRelative5.stringValue = parameters.Length != 1 || !typeof (UnityEngine.Object).IsAssignableFrom(parameters[0].ParameterType) ? typeof (UnityEngine.Object).AssemblyQualifiedName : parameters[0].ParameterType.AssemblyQualifiedName;
        }
        this.ValidateObjectParamater(propertyRelative4, this.m_Mode);
        this.m_Listener.m_SerializedObject.ApplyModifiedProperties();
      }

      private void ValidateObjectParamater(SerializedProperty arguments, PersistentListenerMode mode)
      {
        SerializedProperty propertyRelative1 = arguments.FindPropertyRelative("m_ObjectArgumentAssemblyTypeName");
        SerializedProperty propertyRelative2 = arguments.FindPropertyRelative("m_ObjectArgument");
        UnityEngine.Object objectReferenceValue = propertyRelative2.objectReferenceValue;
        if (mode != PersistentListenerMode.Object)
        {
          propertyRelative1.stringValue = typeof (UnityEngine.Object).AssemblyQualifiedName;
          propertyRelative2.objectReferenceValue = (UnityEngine.Object) null;
        }
        else
        {
          if (objectReferenceValue == (UnityEngine.Object) null)
            return;
          System.Type type = System.Type.GetType(propertyRelative1.stringValue, false);
          if (typeof (UnityEngine.Object).IsAssignableFrom(type) && type.IsInstanceOfType((object) objectReferenceValue))
            return;
          propertyRelative2.objectReferenceValue = (UnityEngine.Object) null;
        }
      }

      public void Clear()
      {
        this.m_Listener.FindPropertyRelative("m_MethodName").stringValue = (string) null;
        this.m_Listener.FindPropertyRelative("m_Mode").enumValueIndex = 1;
        this.m_Listener.m_SerializedObject.ApplyModifiedProperties();
      }
    }
  }
}
