// Decompiled with JetBrains decompiler
// Type: UnityEngineInternal.APIUpdaterRuntimeServices
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace UnityEngineInternal
{
  public sealed class APIUpdaterRuntimeServices
  {
    private static IList<System.Type> ComponentsFromUnityEngine;

    static APIUpdaterRuntimeServices()
    {
      System.Type type = typeof (Component);
      APIUpdaterRuntimeServices.ComponentsFromUnityEngine = (IList<System.Type>) ((IEnumerable<System.Type>) type.Assembly.GetTypes()).Where<System.Type>(new Func<System.Type, bool>(type.IsAssignableFrom)).ToList<System.Type>();
    }

    [Obsolete("AddComponent(string) has been deprecated. Use GameObject.AddComponent<T>() / GameObject.AddComponent(Type) instead.\nAPI Updater could not automatically update the original call to AddComponent(string name), because it was unable to resolve the type specified in parameter 'name'.\nInstead, this call has been replaced with a call to APIUpdaterRuntimeServices.AddComponent() so you can try to test your game in the editor.\nIn order to be able to build the game, replace this call (APIUpdaterRuntimeServices.AddComponent()) with a call to GameObject.AddComponent<T>() / GameObject.AddComponent(Type).")]
    public static Component AddComponent(GameObject go, string sourceInfo, string name)
    {
      Debug.LogWarningFormat("Performing a potentially slow search for component {0}.", (object) name);
      System.Type componentType = APIUpdaterRuntimeServices.ResolveType(name, Assembly.GetCallingAssembly(), sourceInfo);
      if (componentType == null)
        return (Component) null;
      return go.AddComponent(componentType);
    }

    private static System.Type ResolveType(string name, Assembly callingAssembly, string sourceInfo)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      APIUpdaterRuntimeServices.\u003CResolveType\u003Ec__AnonStorey7 typeCAnonStorey7 = new APIUpdaterRuntimeServices.\u003CResolveType\u003Ec__AnonStorey7();
      // ISSUE: reference to a compiler-generated field
      typeCAnonStorey7.name = name;
      // ISSUE: reference to a compiler-generated method
      System.Type type1 = APIUpdaterRuntimeServices.ComponentsFromUnityEngine.FirstOrDefault<System.Type>(new Func<System.Type, bool>(typeCAnonStorey7.\u003C\u003Em__D));
      if (type1 != null)
      {
        // ISSUE: reference to a compiler-generated field
        Debug.LogWarningFormat("[{1}] Type '{0}' found in UnityEngine, consider replacing with go.AddComponent<{0}>();", new object[2]
        {
          (object) typeCAnonStorey7.name,
          (object) sourceInfo
        });
        return type1;
      }
      // ISSUE: reference to a compiler-generated field
      System.Type type2 = callingAssembly.GetType(typeCAnonStorey7.name);
      if (type2 != null)
      {
        Debug.LogWarningFormat("[{1}] Component type '{0}' found on caller assembly. Consider replacing the call method call with: AddComponent<{0}>()", new object[2]
        {
          (object) type2.FullName,
          (object) sourceInfo
        });
        return type2;
      }
      // ISSUE: reference to a compiler-generated method
      System.Type type3 = ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).SelectMany<Assembly, System.Type>((Func<Assembly, IEnumerable<System.Type>>) (a => (IEnumerable<System.Type>) a.GetTypes())).SingleOrDefault<System.Type>(new Func<System.Type, bool>(typeCAnonStorey7.\u003C\u003Em__F));
      if (type3 != null)
      {
        Debug.LogWarningFormat("[{2}] Component type '{0}' found on assembly {1}. Consider replacing the call method with: AddComponent<{0}>()", (object) type3.FullName, (object) type3.Assembly.Location, (object) sourceInfo);
        return type3;
      }
      // ISSUE: reference to a compiler-generated field
      Debug.LogErrorFormat("[{1}] Component Type '{0}' not found.", new object[2]
      {
        (object) typeCAnonStorey7.name,
        (object) sourceInfo
      });
      return (System.Type) null;
    }

    private static bool IsMarkedAsObsolete(System.Type t)
    {
      return ((IEnumerable<object>) t.GetCustomAttributes(typeof (ObsoleteAttribute), false)).Any<object>();
    }
  }
}
