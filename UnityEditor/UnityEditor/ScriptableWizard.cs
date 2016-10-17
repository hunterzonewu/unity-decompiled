// Decompiled with JetBrains decompiler
// Type: UnityEditor.ScriptableWizard
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Reflection;
using UnityEngine;
using UnityEngine.Internal;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Derive from this class to create an editor wizard.</para>
  /// </summary>
  public class ScriptableWizard : EditorWindow
  {
    private string m_HelpString = string.Empty;
    private string m_ErrorString = string.Empty;
    private bool m_IsValid = true;
    private string m_CreateButton = "Create";
    private string m_OtherButton = string.Empty;
    private GenericInspector m_Inspector;
    private Vector2 m_ScrollPosition;

    /// <summary>
    ///   <para>Allows you to set the help text of the wizard.</para>
    /// </summary>
    public string helpString
    {
      get
      {
        return this.m_HelpString;
      }
      set
      {
        string str = value ?? string.Empty;
        if (!(this.m_HelpString != str))
          return;
        this.m_HelpString = str;
        this.Repaint();
      }
    }

    /// <summary>
    ///   <para>Allows you to set the error text of the wizard.</para>
    /// </summary>
    public string errorString
    {
      get
      {
        return this.m_ErrorString;
      }
      set
      {
        string str = value ?? string.Empty;
        if (!(this.m_ErrorString != str))
          return;
        this.m_ErrorString = str;
        this.Repaint();
      }
    }

    /// <summary>
    ///   <para>Allows you to set the text shown on the create button of the wizard.</para>
    /// </summary>
    public string createButtonName
    {
      get
      {
        return this.m_CreateButton;
      }
      set
      {
        string str = value ?? string.Empty;
        if (!(this.m_CreateButton != str))
          return;
        this.m_CreateButton = str;
        this.Repaint();
      }
    }

    /// <summary>
    ///   <para>Allows you to set the text shown on the optional other button of the wizard. Leave this parameter out to leave the button out.</para>
    /// </summary>
    public string otherButtonName
    {
      get
      {
        return this.m_OtherButton;
      }
      set
      {
        string str = value ?? string.Empty;
        if (!(this.m_OtherButton != str))
          return;
        this.m_OtherButton = str;
        this.Repaint();
      }
    }

    /// <summary>
    ///   <para>Allows you to enable and disable the wizard create button, so that the user can not click it.</para>
    /// </summary>
    public bool isValid
    {
      get
      {
        return this.m_IsValid;
      }
      set
      {
        this.m_IsValid = value;
      }
    }

    private void OnDestroy()
    {
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_Inspector);
    }

    private void InvokeWizardUpdate()
    {
      MethodInfo method = this.GetType().GetMethod("OnWizardUpdate", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
      if (method == null)
        return;
      method.Invoke((object) this, (object[]) null);
    }

    private void OnGUI()
    {
      EditorGUIUtility.labelWidth = 150f;
      GUILayout.Label(this.m_HelpString, EditorStyles.wordWrappedLabel, new GUILayoutOption[1]
      {
        GUILayout.ExpandHeight(true)
      });
      this.m_ScrollPosition = EditorGUILayout.BeginVerticalScrollView(this.m_ScrollPosition, false, GUI.skin.verticalScrollbar, (GUIStyle) "OL Box");
      GUIUtility.GetControlID(645789, FocusType.Passive);
      bool flag = this.DrawWizardGUI();
      EditorGUILayout.EndScrollView();
      GUILayout.BeginVertical();
      if (this.m_ErrorString != string.Empty)
        GUILayout.Label(this.m_ErrorString, (GUIStyle) ScriptableWizard.Styles.errorText, new GUILayoutOption[1]
        {
          GUILayout.MinHeight(32f)
        });
      else
        GUILayout.Label(string.Empty, new GUILayoutOption[1]
        {
          GUILayout.MinHeight(32f)
        });
      GUILayout.FlexibleSpace();
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      GUI.enabled = this.m_IsValid;
      if (this.m_OtherButton != string.Empty)
      {
        if (GUILayout.Button(this.m_OtherButton, new GUILayoutOption[1]
        {
          GUILayout.MinWidth(100f)
        }))
        {
          MethodInfo method = this.GetType().GetMethod("OnWizardOtherButton", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
          if (method != null)
          {
            method.Invoke((object) this, (object[]) null);
            GUIUtility.ExitGUI();
          }
          else
            Debug.LogError((object) "OnWizardOtherButton has not been implemented in script");
        }
      }
      if (this.m_CreateButton != string.Empty)
      {
        if (GUILayout.Button(this.m_CreateButton, new GUILayoutOption[1]
        {
          GUILayout.MinWidth(100f)
        }))
        {
          MethodInfo method = this.GetType().GetMethod("OnWizardCreate", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
          if (method != null)
            method.Invoke((object) this, (object[]) null);
          else
            Debug.LogError((object) "OnWizardCreate has not been implemented in script");
          this.Close();
          GUIUtility.ExitGUI();
        }
      }
      GUI.enabled = true;
      GUILayout.EndHorizontal();
      GUILayout.EndVertical();
      if (!flag)
        return;
      this.InvokeWizardUpdate();
    }

    /// <summary>
    ///   <para>Will be called for drawing contents when the ScriptableWizard needs to update its GUI.</para>
    /// </summary>
    /// <returns>
    ///   <para>Returns true if any property has been modified.</para>
    /// </returns>
    protected virtual bool DrawWizardGUI()
    {
      if ((UnityEngine.Object) this.m_Inspector == (UnityEngine.Object) null)
      {
        this.m_Inspector = ScriptableObject.CreateInstance<GenericInspector>();
        this.m_Inspector.hideFlags = HideFlags.HideAndDontSave;
        this.m_Inspector.InternalSetTargets(new UnityEngine.Object[1]{ (UnityEngine.Object) this });
      }
      return this.m_Inspector.DrawDefaultInspector();
    }

    public static T DisplayWizard<T>(string title) where T : ScriptableWizard
    {
      return ScriptableWizard.DisplayWizard<T>(title, "Create", string.Empty);
    }

    public static T DisplayWizard<T>(string title, string createButtonName) where T : ScriptableWizard
    {
      return ScriptableWizard.DisplayWizard<T>(title, createButtonName, string.Empty);
    }

    public static T DisplayWizard<T>(string title, string createButtonName, string otherButtonName) where T : ScriptableWizard
    {
      return (T) ScriptableWizard.DisplayWizard(title, typeof (T), createButtonName, otherButtonName);
    }

    [ExcludeFromDocs]
    public static ScriptableWizard DisplayWizard(string title, System.Type klass, string createButtonName)
    {
      string empty = string.Empty;
      return ScriptableWizard.DisplayWizard(title, klass, createButtonName, empty);
    }

    [ExcludeFromDocs]
    public static ScriptableWizard DisplayWizard(string title, System.Type klass)
    {
      string empty = string.Empty;
      string createButtonName = "Create";
      return ScriptableWizard.DisplayWizard(title, klass, createButtonName, empty);
    }

    /// <summary>
    ///   <para>Creates a wizard.</para>
    /// </summary>
    /// <param name="title">The title shown at the top of the wizard window.</param>
    /// <param name="klass">The class implementing the wizard. It has to derive from ScriptableWizard.</param>
    /// <param name="createButtonName">The text shown on the create button.</param>
    /// <param name="otherButtonName">The text shown on the optional other button. Leave this parameter out to leave the button out.</param>
    /// <returns>
    ///   <para>The wizard.</para>
    /// </returns>
    public static ScriptableWizard DisplayWizard(string title, System.Type klass, [DefaultValue("\"Create\"")] string createButtonName, [DefaultValue("\"\"")] string otherButtonName)
    {
      ScriptableWizard instance = ScriptableObject.CreateInstance(klass) as ScriptableWizard;
      instance.m_CreateButton = createButtonName;
      instance.m_OtherButton = otherButtonName;
      instance.titleContent = new GUIContent(title);
      if ((UnityEngine.Object) instance != (UnityEngine.Object) null)
      {
        instance.InvokeWizardUpdate();
        instance.ShowUtility();
      }
      return instance;
    }

    private class Styles
    {
      public static string errorText = "Wizard Error";
      public static string box = "Wizard Box";
    }
  }
}
