// Decompiled with JetBrains decompiler
// Type: UnityEditor.DisplayUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor.Modules;
using UnityEngine;

namespace UnityEditor
{
  internal class DisplayUtility
  {
    private static GUIContent[] s_GenericDisplayNames = new GUIContent[8]
    {
      EditorGUIUtility.TextContent("Display 1"),
      EditorGUIUtility.TextContent("Display 2"),
      EditorGUIUtility.TextContent("Display 3"),
      EditorGUIUtility.TextContent("Display 4"),
      EditorGUIUtility.TextContent("Display 5"),
      EditorGUIUtility.TextContent("Display 6"),
      EditorGUIUtility.TextContent("Display 7"),
      EditorGUIUtility.TextContent("Display 8")
    };
    private static readonly int[] s_DisplayIndices = new int[8]
    {
      0,
      1,
      2,
      3,
      4,
      5,
      6,
      7
    };

    public static GUIContent[] GetGenericDisplayNames()
    {
      return DisplayUtility.s_GenericDisplayNames;
    }

    public static int[] GetDisplayIndices()
    {
      return DisplayUtility.s_DisplayIndices;
    }

    public static GUIContent[] GetDisplayNames()
    {
      return ModuleManager.GetDisplayNames(EditorUserBuildSettings.activeBuildTarget.ToString()) ?? DisplayUtility.s_GenericDisplayNames;
    }
  }
}
