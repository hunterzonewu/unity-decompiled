// Decompiled with JetBrains decompiler
// Type: UnityEditor.AnimationEventPopup
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace UnityEditor
{
  [EditorWindowTitle(title = "Edit Animation Event", useTypeNameAsIconName = false)]
  internal class AnimationEventPopup : EditorWindow
  {
    private const string kAmbiguousPostFix = " (Function Is Ambiguous)";
    private const string kNotSupportedPostFix = " (Function Not Supported)";
    private const string kNoneSelected = "(No Function Selected)";
    private GameObject m_Root;
    private AnimationClip m_Clip;
    private int m_EventIndex;
    private AnimationClipInfoProperties m_ClipInfo;
    private EditorWindow m_Owner;

    public AnimationClipInfoProperties clipInfo
    {
      get
      {
        return this.m_ClipInfo;
      }
      set
      {
        this.m_ClipInfo = value;
      }
    }

    private int eventIndex
    {
      get
      {
        return this.m_EventIndex;
      }
      set
      {
        this.m_EventIndex = value;
      }
    }

    private void OnEnable()
    {
      this.titleContent = this.GetLocalizedTitleContent();
    }

    internal static void InitWindow(AnimationEventPopup popup)
    {
      popup.minSize = new Vector2(400f, 140f);
      popup.maxSize = new Vector2(400f, 140f);
      popup.titleContent = EditorGUIUtility.TextContent("Edit Animation Event");
    }

    internal static void Edit(GameObject root, AnimationClip clip, int index, EditorWindow owner)
    {
      UnityEngine.Object[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll(typeof (AnimationEventPopup));
      AnimationEventPopup popup = objectsOfTypeAll.Length <= 0 ? (AnimationEventPopup) null : (AnimationEventPopup) objectsOfTypeAll[0];
      if ((UnityEngine.Object) popup == (UnityEngine.Object) null)
      {
        popup = EditorWindow.GetWindow<AnimationEventPopup>(true);
        AnimationEventPopup.InitWindow(popup);
      }
      popup.m_Root = root;
      popup.m_Clip = clip;
      popup.eventIndex = index;
      popup.m_Owner = owner;
      popup.Repaint();
    }

    internal static void Edit(AnimationClipInfoProperties clipInfo, int index)
    {
      UnityEngine.Object[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll(typeof (AnimationEventPopup));
      AnimationEventPopup popup = objectsOfTypeAll.Length <= 0 ? (AnimationEventPopup) null : (AnimationEventPopup) objectsOfTypeAll[0];
      if ((UnityEngine.Object) popup == (UnityEngine.Object) null)
      {
        popup = EditorWindow.GetWindow<AnimationEventPopup>(true);
        AnimationEventPopup.InitWindow(popup);
      }
      popup.m_Root = (GameObject) null;
      popup.m_Clip = (AnimationClip) null;
      popup.m_ClipInfo = clipInfo;
      popup.eventIndex = index;
      popup.Repaint();
    }

    internal static void UpdateSelection(GameObject root, AnimationClip clip, int index, EditorWindow owner)
    {
      UnityEngine.Object[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll(typeof (AnimationEventPopup));
      AnimationEventPopup animationEventPopup = objectsOfTypeAll.Length <= 0 ? (AnimationEventPopup) null : (AnimationEventPopup) objectsOfTypeAll[0];
      if ((UnityEngine.Object) animationEventPopup == (UnityEngine.Object) null)
        return;
      animationEventPopup.m_Root = root;
      animationEventPopup.m_Clip = clip;
      animationEventPopup.eventIndex = index;
      animationEventPopup.m_Owner = owner;
      animationEventPopup.Repaint();
    }

    internal static int Create(GameObject root, AnimationClip clip, float time, EditorWindow owner)
    {
      AnimationEvent evt = new AnimationEvent();
      evt.time = time;
      AnimationEvent[] animationEvents = AnimationUtility.GetAnimationEvents(clip);
      int num = AnimationEventPopup.InsertAnimationEvent(ref animationEvents, clip, evt);
      AnimationEventPopup window = EditorWindow.GetWindow<AnimationEventPopup>(true);
      AnimationEventPopup.InitWindow(window);
      window.m_Root = root;
      window.m_Clip = clip;
      window.eventIndex = num;
      window.m_Owner = owner;
      return num;
    }

    internal static void ClosePopup()
    {
      UnityEngine.Object[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll(typeof (AnimationEventPopup));
      AnimationEventPopup animationEventPopup = objectsOfTypeAll.Length <= 0 ? (AnimationEventPopup) null : (AnimationEventPopup) objectsOfTypeAll[0];
      if (!((UnityEngine.Object) animationEventPopup != (UnityEngine.Object) null))
        return;
      animationEventPopup.Close();
    }

    public static string FormatEvent(GameObject root, AnimationEvent evt)
    {
      if (string.IsNullOrEmpty(evt.functionName))
        return "(No Function Selected)";
      if (!AnimationEventPopup.IsSupportedMethodName(evt.functionName))
        return evt.functionName + " (Function Not Supported)";
      foreach (MonoBehaviour component in root.GetComponents<MonoBehaviour>())
      {
        if (!((UnityEngine.Object) component == (UnityEngine.Object) null))
        {
          System.Type type = component.GetType();
          if (type != typeof (MonoBehaviour) && (type.BaseType == null || !(type.BaseType.Name == "GraphBehaviour")))
          {
            MethodInfo method = type.GetMethod(evt.functionName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (method != null)
            {
              IEnumerable<System.Type> paramTypes = ((IEnumerable<System.Reflection.ParameterInfo>) method.GetParameters()).Select<System.Reflection.ParameterInfo, System.Type>((Func<System.Reflection.ParameterInfo, System.Type>) (p => p.ParameterType));
              return evt.functionName + AnimationEventPopup.FormatEventArguments(paramTypes, evt);
            }
          }
        }
      }
      return evt.functionName + " (Function Not Supported)";
    }

    private static bool IsSupportedMethodName(string name)
    {
      return !(name == "Main") && !(name == "Start") && (!(name == "Awake") && !(name == "Update"));
    }

    private static string FormatEventArguments(IEnumerable<System.Type> paramTypes, AnimationEvent evt)
    {
      if (!paramTypes.Any<System.Type>())
        return " ( )";
      if (paramTypes.Count<System.Type>() > 1)
        return " (Function Not Supported)";
      System.Type enumType = paramTypes.First<System.Type>();
      if (enumType == typeof (string))
        return " ( \"" + evt.stringParameter + "\" )";
      if (enumType == typeof (float))
        return " ( " + (object) evt.floatParameter + " )";
      if (enumType == typeof (int))
        return " ( " + (object) evt.intParameter + " )";
      if (enumType == typeof (int))
        return " ( " + (object) evt.intParameter + " )";
      if (enumType.IsEnum)
        return " ( " + enumType.Name + "." + Enum.GetName(enumType, (object) evt.intParameter) + " )";
      if (enumType == typeof (AnimationEvent))
        return " ( " + (object) evt.floatParameter + " / " + (object) evt.intParameter + " / \"" + evt.stringParameter + "\" / " + (!(evt.objectReferenceParameter == (UnityEngine.Object) null) ? evt.objectReferenceParameter.name : "null") + " )";
      if (enumType.IsSubclassOf(typeof (UnityEngine.Object)) || enumType == typeof (UnityEngine.Object))
        return " ( " + (!(evt.objectReferenceParameter == (UnityEngine.Object) null) ? evt.objectReferenceParameter.name : "null") + " )";
      return " (Function Not Supported)";
    }

    private static void CollectSupportedMethods(GameObject root, out List<string> supportedMethods, out List<System.Type> supportedMethodsParameter)
    {
      supportedMethods = new List<string>();
      supportedMethodsParameter = new List<System.Type>();
      MonoBehaviour[] components = root.GetComponents<MonoBehaviour>();
      HashSet<string> stringSet = new HashSet<string>();
      foreach (MonoBehaviour monoBehaviour in components)
      {
        if (!((UnityEngine.Object) monoBehaviour == (UnityEngine.Object) null))
        {
          for (System.Type type = monoBehaviour.GetType(); type != typeof (MonoBehaviour) && type != null; type = type.BaseType)
          {
            foreach (MethodInfo method in type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
              string name = method.Name;
              if (AnimationEventPopup.IsSupportedMethodName(name))
              {
                System.Reflection.ParameterInfo[] parameters = method.GetParameters();
                if (parameters.Length <= 1)
                {
                  if (parameters.Length == 1)
                  {
                    System.Type parameterType = parameters[0].ParameterType;
                    if (parameterType == typeof (string) || parameterType == typeof (float) || (parameterType == typeof (int) || parameterType == typeof (AnimationEvent)) || (parameterType == typeof (UnityEngine.Object) || parameterType.IsSubclassOf(typeof (UnityEngine.Object)) || parameterType.IsEnum))
                      supportedMethodsParameter.Add(parameterType);
                    else
                      continue;
                  }
                  else
                    supportedMethodsParameter.Add((System.Type) null);
                  if (supportedMethods.Contains(name))
                    stringSet.Add(name);
                  supportedMethods.Add(name);
                }
              }
            }
          }
        }
      }
      using (HashSet<string>.Enumerator enumerator = stringSet.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          string current = enumerator.Current;
          for (int index = 0; index < supportedMethods.Count; ++index)
          {
            if (supportedMethods[index].Equals(current))
            {
              supportedMethods.RemoveAt(index);
              supportedMethodsParameter.RemoveAt(index);
              --index;
            }
          }
        }
      }
    }

    internal static int InsertAnimationEvent(ref AnimationEvent[] events, AnimationClip clip, AnimationEvent evt)
    {
      Undo.RegisterCompleteObjectUndo((UnityEngine.Object) clip, "Add Event");
      int index1 = events.Length;
      for (int index2 = 0; index2 < events.Length; ++index2)
      {
        if ((double) events[index2].time > (double) evt.time)
        {
          index1 = index2;
          break;
        }
      }
      ArrayUtility.Insert<AnimationEvent>(ref events, index1, evt);
      AnimationUtility.SetAnimationEvents(clip, events);
      events = AnimationUtility.GetAnimationEvents(clip);
      if ((double) events[index1].time != (double) evt.time || events[index1].functionName != events[index1].functionName)
        Debug.LogError((object) "Failed insertion");
      return index1;
    }

    public void OnGUI()
    {
      AnimationEvent[] events = (AnimationEvent[]) null;
      if ((UnityEngine.Object) this.m_Clip != (UnityEngine.Object) null)
        events = AnimationUtility.GetAnimationEvents(this.m_Clip);
      else if (this.m_ClipInfo != null)
        events = this.m_ClipInfo.GetEvents();
      if (events == null || this.eventIndex < 0 || this.eventIndex >= events.Length)
        return;
      GUI.changed = false;
      AnimationEvent animationEvent = events[this.eventIndex];
      if ((bool) ((UnityEngine.Object) this.m_Root))
      {
        List<string> supportedMethods;
        List<System.Type> supportedMethodsParameter;
        AnimationEventPopup.CollectSupportedMethods(this.m_Root, out supportedMethods, out supportedMethodsParameter);
        List<string> stringList = new List<string>(supportedMethods.Count);
        for (int index = 0; index < supportedMethods.Count; ++index)
        {
          string str = " ( )";
          if (supportedMethodsParameter[index] != null)
            str = supportedMethodsParameter[index] != typeof (float) ? (supportedMethodsParameter[index] != typeof (int) ? string.Format(" ( {0} )", (object) supportedMethodsParameter[index].Name) : " ( int )") : " ( float )";
          stringList.Add(supportedMethods[index] + str);
        }
        int count = supportedMethods.Count;
        int selectedIndex = supportedMethods.IndexOf(animationEvent.functionName);
        if (selectedIndex == -1)
        {
          selectedIndex = supportedMethods.Count;
          supportedMethods.Add(animationEvent.functionName);
          if (string.IsNullOrEmpty(animationEvent.functionName))
            stringList.Add("(No Function Selected)");
          else
            stringList.Add(animationEvent.functionName + " (Function Not Supported)");
          supportedMethodsParameter.Add((System.Type) null);
        }
        EditorGUIUtility.labelWidth = 130f;
        int num = selectedIndex;
        int index1 = EditorGUILayout.Popup("Function: ", selectedIndex, stringList.ToArray(), new GUILayoutOption[0]);
        if (num != index1 && index1 != -1 && index1 != count)
        {
          animationEvent.functionName = supportedMethods[index1];
          animationEvent.stringParameter = string.Empty;
        }
        System.Type selectedParameter = supportedMethodsParameter[index1];
        if (selectedParameter != null)
        {
          EditorGUILayout.Space();
          if (selectedParameter == typeof (AnimationEvent))
            EditorGUILayout.PrefixLabel("Event Data");
          else
            EditorGUILayout.PrefixLabel("Parameters");
          AnimationEventPopup.DoEditRegularParameters(animationEvent, selectedParameter);
        }
      }
      else
      {
        animationEvent.functionName = EditorGUILayout.TextField(new GUIContent("Function"), animationEvent.functionName, new GUILayoutOption[0]);
        AnimationEventPopup.DoEditRegularParameters(animationEvent, typeof (AnimationEvent));
      }
      if (!GUI.changed)
        return;
      if ((UnityEngine.Object) this.m_Clip != (UnityEngine.Object) null)
      {
        Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this.m_Clip, "Animation Event Change");
        AnimationUtility.SetAnimationEvents(this.m_Clip, events);
      }
      else
      {
        if (this.m_ClipInfo == null)
          return;
        this.m_ClipInfo.SetEvent(this.m_EventIndex, animationEvent);
      }
    }

    private static bool EscapePressed()
    {
      if (Event.current.type == EventType.KeyDown)
        return Event.current.keyCode == KeyCode.Escape;
      return false;
    }

    private static bool EnterPressed()
    {
      if (Event.current.type == EventType.KeyDown)
        return Event.current.keyCode == KeyCode.Return;
      return false;
    }

    private static void DoEditRegularParameters(AnimationEvent evt, System.Type selectedParameter)
    {
      if (selectedParameter == typeof (AnimationEvent) || selectedParameter == typeof (float))
        evt.floatParameter = EditorGUILayout.FloatField("Float", evt.floatParameter, new GUILayoutOption[0]);
      if (selectedParameter.IsEnum)
        evt.intParameter = AnimationEventPopup.EnumPopup("Enum", selectedParameter, evt.intParameter);
      else if (selectedParameter == typeof (AnimationEvent) || selectedParameter == typeof (int))
        evt.intParameter = EditorGUILayout.IntField("Int", evt.intParameter, new GUILayoutOption[0]);
      if (selectedParameter == typeof (AnimationEvent) || selectedParameter == typeof (string))
        evt.stringParameter = EditorGUILayout.TextField("String", evt.stringParameter, new GUILayoutOption[0]);
      if (selectedParameter != typeof (AnimationEvent) && !selectedParameter.IsSubclassOf(typeof (UnityEngine.Object)) && selectedParameter != typeof (UnityEngine.Object))
        return;
      System.Type objType = typeof (UnityEngine.Object);
      if (selectedParameter != typeof (AnimationEvent))
        objType = selectedParameter;
      bool allowSceneObjects = false;
      evt.objectReferenceParameter = EditorGUILayout.ObjectField(ObjectNames.NicifyVariableName(objType.Name), evt.objectReferenceParameter, objType, allowSceneObjects, new GUILayoutOption[0]);
    }

    public static int EnumPopup(string label, System.Type enumType, int selected)
    {
      if (!enumType.IsEnum)
        throw new Exception("parameter _enum must be of type System.Enum");
      string[] names = Enum.GetNames(enumType);
      int selectedIndex = Array.IndexOf<string>(names, Enum.GetName(enumType, (object) selected));
      int index = EditorGUILayout.Popup(label, selectedIndex, names, EditorStyles.popup, new GUILayoutOption[0]);
      if (index == -1)
        return selected;
      return Convert.ToInt32((object) (Enum) Enum.Parse(enumType, names[index]));
    }

    private void OnDestroy()
    {
      if (!(bool) ((UnityEngine.Object) this.m_Owner))
        return;
      this.m_Owner.Focus();
    }
  }
}
