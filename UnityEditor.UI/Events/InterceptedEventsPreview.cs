// Decompiled with JetBrains decompiler
// Type: UnityEditor.Events.InterceptedEventsPreview
// Assembly: UnityEditor.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02C415E4-CA90-4A5B-B8EB-A397F164EE08
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.UI.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityEditor.Events
{
  [CustomPreview(typeof (GameObject))]
  internal class InterceptedEventsPreview : ObjectPreview
  {
    private static List<System.Type> s_EventSystemInterfaces = (List<System.Type>) null;
    private static List<GUIContent> s_PossibleEvents = (List<GUIContent>) null;
    private static Dictionary<System.Type, List<int>> s_InterfaceEventSystemEvents = (Dictionary<System.Type, List<int>>) null;
    private static readonly Dictionary<System.Type, InterceptedEventsPreview.ComponentInterceptedEvents> s_ComponentEvents2 = new Dictionary<System.Type, InterceptedEventsPreview.ComponentInterceptedEvents>();
    private InterceptedEventsPreview.Styles m_Styles = new InterceptedEventsPreview.Styles();
    private Dictionary<GameObject, List<InterceptedEventsPreview.ComponentInterceptedEvents>> m_TargetEvents;
    private bool m_InterceptsAnyEvent;
    private GUIContent m_Title;

    public override void Initialize(UnityEngine.Object[] targets)
    {
      base.Initialize(targets);
      this.m_TargetEvents = new Dictionary<GameObject, List<InterceptedEventsPreview.ComponentInterceptedEvents>>(((IEnumerable<UnityEngine.Object>) targets).Count<UnityEngine.Object>());
      this.m_InterceptsAnyEvent = false;
      for (int index = 0; index < targets.Length; ++index)
      {
        GameObject target = targets[index] as GameObject;
        List<InterceptedEventsPreview.ComponentInterceptedEvents> eventsInfo = InterceptedEventsPreview.GetEventsInfo(target);
        this.m_TargetEvents.Add(target, eventsInfo);
        if (eventsInfo.Any<InterceptedEventsPreview.ComponentInterceptedEvents>())
          this.m_InterceptsAnyEvent = true;
      }
    }

    public override GUIContent GetPreviewTitle()
    {
      if (this.m_Title == null)
        this.m_Title = new GUIContent("Intercepted Events");
      return this.m_Title;
    }

    public override bool HasPreviewGUI()
    {
      if (this.m_TargetEvents != null)
        return this.m_InterceptsAnyEvent;
      return false;
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      if (this.m_Styles == null)
        this.m_Styles = new InterceptedEventsPreview.Styles();
      Vector2 zero = Vector2.zero;
      int num1 = 0;
      List<InterceptedEventsPreview.ComponentInterceptedEvents> targetEvent = this.m_TargetEvents[this.target as GameObject];
      using (List<InterceptedEventsPreview.ComponentInterceptedEvents>.Enumerator enumerator = targetEvent.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          foreach (int interceptedEvent in enumerator.Current.interceptedEvents)
          {
            GUIContent possibleEvent = InterceptedEventsPreview.s_PossibleEvents[interceptedEvent];
            ++num1;
            Vector2 vector2 = this.m_Styles.labelStyle.CalcSize(possibleEvent);
            if ((double) zero.x < (double) vector2.x)
              zero.x = vector2.x;
            if ((double) zero.y < (double) vector2.y)
              zero.y = vector2.y;
          }
        }
      }
      r = new RectOffset(-5, -5, -5, -5).Add(r);
      int num2 = Mathf.Max(Mathf.FloorToInt(r.width / zero.x), 1);
      int num3 = Mathf.Max(num1 / num2, 1) + targetEvent.Count;
      float x = r.x + Mathf.Max(0.0f, (float) (((double) r.width - (double) zero.x * (double) num2) / 2.0));
      float y = r.y + Mathf.Max(0.0f, (float) (((double) r.height - (double) zero.y * (double) num3) / 2.0));
      Rect position = new Rect(x, y, zero.x, zero.y);
      int num4 = 0;
      using (List<InterceptedEventsPreview.ComponentInterceptedEvents>.Enumerator enumerator = targetEvent.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          InterceptedEventsPreview.ComponentInterceptedEvents current = enumerator.Current;
          GUI.Label(position, current.componentName, this.m_Styles.componentName);
          position.y += position.height;
          position.x = x;
          foreach (int interceptedEvent in current.interceptedEvents)
          {
            GUIContent possibleEvent = InterceptedEventsPreview.s_PossibleEvents[interceptedEvent];
            GUI.Label(position, possibleEvent, this.m_Styles.labelStyle);
            if (num4 < num2 - 1)
            {
              position.x += position.width;
            }
            else
            {
              position.y += position.height;
              position.x = x;
            }
            num4 = (num4 + 1) % num2;
          }
          if ((double) position.x != (double) x)
          {
            position.y += position.height;
            position.x = x;
          }
        }
      }
    }

    protected static List<InterceptedEventsPreview.ComponentInterceptedEvents> GetEventsInfo(GameObject gameObject)
    {
      InterceptedEventsPreview.InitializeEvetnsInterfaceCacheIfNeeded();
      List<InterceptedEventsPreview.ComponentInterceptedEvents> interceptedEventsList = new List<InterceptedEventsPreview.ComponentInterceptedEvents>();
      MonoBehaviour[] components = gameObject.GetComponents<MonoBehaviour>();
      int index1 = 0;
      for (int length = components.Length; index1 < length; ++index1)
      {
        InterceptedEventsPreview.ComponentInterceptedEvents interceptedEvents = (InterceptedEventsPreview.ComponentInterceptedEvents) null;
        MonoBehaviour monoBehaviour = components[index1];
        if (!((UnityEngine.Object) monoBehaviour == (UnityEngine.Object) null))
        {
          System.Type type = monoBehaviour.GetType();
          if (!InterceptedEventsPreview.s_ComponentEvents2.ContainsKey(type))
          {
            List<int> source = (List<int>) null;
            if (typeof (IEventSystemHandler).IsAssignableFrom(type))
            {
              for (int index2 = 0; index2 < InterceptedEventsPreview.s_EventSystemInterfaces.Count; ++index2)
              {
                System.Type eventSystemInterface = InterceptedEventsPreview.s_EventSystemInterfaces[index2];
                if (eventSystemInterface.IsAssignableFrom(type))
                {
                  if (source == null)
                    source = new List<int>();
                  source.AddRange((IEnumerable<int>) InterceptedEventsPreview.s_InterfaceEventSystemEvents[eventSystemInterface]);
                }
              }
            }
            if (source != null)
            {
              interceptedEvents = new InterceptedEventsPreview.ComponentInterceptedEvents();
              interceptedEvents.componentName = new GUIContent(type.Name);
              interceptedEvents.interceptedEvents = source.OrderBy<int, string>((Func<int, string>) (index => InterceptedEventsPreview.s_PossibleEvents[index].text)).ToArray<int>();
            }
            InterceptedEventsPreview.s_ComponentEvents2.Add(type, interceptedEvents);
          }
          else
            interceptedEvents = InterceptedEventsPreview.s_ComponentEvents2[type];
          if (interceptedEvents != null)
            interceptedEventsList.Add(interceptedEvents);
        }
      }
      return interceptedEventsList;
    }

    private static void InitializeEvetnsInterfaceCacheIfNeeded()
    {
      if (InterceptedEventsPreview.s_EventSystemInterfaces != null)
        return;
      InterceptedEventsPreview.s_EventSystemInterfaces = new List<System.Type>();
      InterceptedEventsPreview.s_PossibleEvents = new List<GUIContent>();
      InterceptedEventsPreview.s_InterfaceEventSystemEvents = new Dictionary<System.Type, List<int>>();
      foreach (System.Type inLoadedAssembly in InterceptedEventsPreview.GetAccessibleTypesInLoadedAssemblies())
      {
        if (inLoadedAssembly.IsInterface && typeof (IEventSystemHandler).IsAssignableFrom(inLoadedAssembly))
        {
          InterceptedEventsPreview.s_EventSystemInterfaces.Add(inLoadedAssembly);
          List<int> intList = new List<int>();
          foreach (MethodInfo method in inLoadedAssembly.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
          {
            intList.Add(InterceptedEventsPreview.s_PossibleEvents.Count);
            InterceptedEventsPreview.s_PossibleEvents.Add(new GUIContent(method.Name));
          }
          InterceptedEventsPreview.s_InterfaceEventSystemEvents.Add(inLoadedAssembly, intList);
        }
      }
    }

    [DebuggerHidden]
    private static IEnumerable<System.Type> GetAccessibleTypesInLoadedAssemblies()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      InterceptedEventsPreview.\u003CGetAccessibleTypesInLoadedAssemblies\u003Ec__Iterator0 assembliesCIterator0_1 = new InterceptedEventsPreview.\u003CGetAccessibleTypesInLoadedAssemblies\u003Ec__Iterator0();
      // ISSUE: variable of a compiler-generated type
      InterceptedEventsPreview.\u003CGetAccessibleTypesInLoadedAssemblies\u003Ec__Iterator0 assembliesCIterator0_2 = assembliesCIterator0_1;
      int num = -2;
      // ISSUE: reference to a compiler-generated field
      assembliesCIterator0_2.\u0024PC = num;
      return (IEnumerable<System.Type>) assembliesCIterator0_2;
    }

    protected class ComponentInterceptedEvents
    {
      public GUIContent componentName;
      public int[] interceptedEvents;
    }

    private class Styles
    {
      public GUIStyle labelStyle = new GUIStyle(EditorStyles.label);
      public GUIStyle componentName = new GUIStyle(EditorStyles.boldLabel);

      public Styles()
      {
        Color color = new Color(0.7f, 0.7f, 0.7f);
        this.labelStyle.padding.right += 20;
        this.labelStyle.normal.textColor = color;
        this.labelStyle.active.textColor = color;
        this.labelStyle.focused.textColor = color;
        this.labelStyle.hover.textColor = color;
        this.labelStyle.onNormal.textColor = color;
        this.labelStyle.onActive.textColor = color;
        this.labelStyle.onFocused.textColor = color;
        this.labelStyle.onHover.textColor = color;
        this.componentName.normal.textColor = color;
        this.componentName.active.textColor = color;
        this.componentName.focused.textColor = color;
        this.componentName.hover.textColor = color;
        this.componentName.onNormal.textColor = color;
        this.componentName.onActive.textColor = color;
        this.componentName.onFocused.textColor = color;
        this.componentName.onHover.textColor = color;
      }
    }
  }
}
