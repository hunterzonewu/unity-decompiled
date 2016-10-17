// Decompiled with JetBrains decompiler
// Type: UnityEditor.MaterialPropertyHandler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UnityEditor
{
  internal class MaterialPropertyHandler
  {
    private static Dictionary<string, MaterialPropertyHandler> s_PropertyHandlers = new Dictionary<string, MaterialPropertyHandler>();
    private MaterialPropertyDrawer m_PropertyDrawer;
    private List<MaterialPropertyDrawer> m_DecoratorDrawers;

    public MaterialPropertyDrawer propertyDrawer
    {
      get
      {
        return this.m_PropertyDrawer;
      }
    }

    public bool IsEmpty()
    {
      if (this.m_PropertyDrawer != null)
        return false;
      if (this.m_DecoratorDrawers != null)
        return this.m_DecoratorDrawers.Count == 0;
      return true;
    }

    public void OnGUI(ref Rect position, MaterialProperty prop, string label, MaterialEditor editor)
    {
      float height = position.height;
      position.height = 0.0f;
      if (this.m_DecoratorDrawers != null)
      {
        using (List<MaterialPropertyDrawer>.Enumerator enumerator = this.m_DecoratorDrawers.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            MaterialPropertyDrawer current = enumerator.Current;
            position.height = current.GetPropertyHeight(prop, label, editor);
            float labelWidth = EditorGUIUtility.labelWidth;
            float fieldWidth = EditorGUIUtility.fieldWidth;
            current.OnGUI(position, prop, label, editor);
            EditorGUIUtility.labelWidth = labelWidth;
            EditorGUIUtility.fieldWidth = fieldWidth;
            position.y += position.height;
            height -= position.height;
          }
        }
      }
      position.height = height;
      if (this.m_PropertyDrawer == null)
        return;
      float labelWidth1 = EditorGUIUtility.labelWidth;
      float fieldWidth1 = EditorGUIUtility.fieldWidth;
      this.m_PropertyDrawer.OnGUI(position, prop, label, editor);
      EditorGUIUtility.labelWidth = labelWidth1;
      EditorGUIUtility.fieldWidth = fieldWidth1;
    }

    public float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
    {
      float num = 0.0f;
      if (this.m_DecoratorDrawers != null)
      {
        using (List<MaterialPropertyDrawer>.Enumerator enumerator = this.m_DecoratorDrawers.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            MaterialPropertyDrawer current = enumerator.Current;
            num += current.GetPropertyHeight(prop, label, editor);
          }
        }
      }
      if (this.m_PropertyDrawer != null)
        num += this.m_PropertyDrawer.GetPropertyHeight(prop, label, editor);
      return num;
    }

    private static string GetPropertyString(Shader shader, string name)
    {
      if ((UnityEngine.Object) shader == (UnityEngine.Object) null)
        return string.Empty;
      return shader.GetInstanceID().ToString() + "_" + name;
    }

    internal static void InvalidatePropertyCache(Shader shader)
    {
      if ((UnityEngine.Object) shader == (UnityEngine.Object) null)
        return;
      string str = shader.GetInstanceID().ToString() + "_";
      List<string> stringList = new List<string>();
      using (Dictionary<string, MaterialPropertyHandler>.KeyCollection.Enumerator enumerator = MaterialPropertyHandler.s_PropertyHandlers.Keys.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          string current = enumerator.Current;
          if (current.StartsWith(str))
            stringList.Add(current);
        }
      }
      using (List<string>.Enumerator enumerator = stringList.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          string current = enumerator.Current;
          MaterialPropertyHandler.s_PropertyHandlers.Remove(current);
        }
      }
    }

    private static MaterialPropertyDrawer CreatePropertyDrawer(System.Type klass, string argsText)
    {
      if (string.IsNullOrEmpty(argsText))
        return Activator.CreateInstance(klass) as MaterialPropertyDrawer;
      string[] strArray = argsText.Split(',');
      object[] objArray = new object[strArray.Length];
      for (int index = 0; index < strArray.Length; ++index)
      {
        string s = strArray[index].Trim();
        float result;
        objArray[index] = !float.TryParse(s, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat, out result) ? (object) s : (object) result;
      }
      return Activator.CreateInstance(klass, objArray) as MaterialPropertyDrawer;
    }

    private static MaterialPropertyDrawer GetShaderPropertyDrawer(string attrib, out bool isDecorator)
    {
      isDecorator = false;
      string str = attrib;
      string argsText = string.Empty;
      Match match = Regex.Match(attrib, "(\\w+)\\s*\\((.*)\\)");
      if (match.Success)
      {
        str = match.Groups[1].Value;
        argsText = match.Groups[2].Value.Trim();
      }
      foreach (System.Type klass in EditorAssemblies.SubclassesOf(typeof (MaterialPropertyDrawer)))
      {
        if (!(klass.Name == str))
        {
          if (!(klass.Name == str + "Drawer"))
          {
            if (!(klass.Name == "Material" + str + "Drawer"))
            {
              if (!(klass.Name == str + "Decorator"))
              {
                if (!(klass.Name == "Material" + str + "Decorator"))
                  continue;
              }
            }
          }
        }
        try
        {
          isDecorator = klass.Name.EndsWith("Decorator");
          return MaterialPropertyHandler.CreatePropertyDrawer(klass, argsText);
        }
        catch (Exception ex)
        {
          Debug.LogWarningFormat("Failed to create material drawer {0} with arguments '{1}'", new object[2]
          {
            (object) str,
            (object) argsText
          });
          return (MaterialPropertyDrawer) null;
        }
      }
      return (MaterialPropertyDrawer) null;
    }

    private static MaterialPropertyHandler GetShaderPropertyHandler(Shader shader, string name)
    {
      string[] propertyAttributes = ShaderUtil.GetShaderPropertyAttributes(shader, name);
      if (propertyAttributes == null || propertyAttributes.Length == 0)
        return (MaterialPropertyHandler) null;
      MaterialPropertyHandler materialPropertyHandler = new MaterialPropertyHandler();
      foreach (string attrib in propertyAttributes)
      {
        bool isDecorator;
        MaterialPropertyDrawer shaderPropertyDrawer = MaterialPropertyHandler.GetShaderPropertyDrawer(attrib, out isDecorator);
        if (shaderPropertyDrawer != null)
        {
          if (isDecorator)
          {
            if (materialPropertyHandler.m_DecoratorDrawers == null)
              materialPropertyHandler.m_DecoratorDrawers = new List<MaterialPropertyDrawer>();
            materialPropertyHandler.m_DecoratorDrawers.Add(shaderPropertyDrawer);
          }
          else
          {
            if (materialPropertyHandler.m_PropertyDrawer != null)
              Debug.LogWarning((object) string.Format("Shader property {0} already has a property drawer", (object) name), (UnityEngine.Object) shader);
            materialPropertyHandler.m_PropertyDrawer = shaderPropertyDrawer;
          }
        }
      }
      return materialPropertyHandler;
    }

    internal static MaterialPropertyHandler GetHandler(Shader shader, string name)
    {
      if ((UnityEngine.Object) shader == (UnityEngine.Object) null)
        return (MaterialPropertyHandler) null;
      string propertyString = MaterialPropertyHandler.GetPropertyString(shader, name);
      MaterialPropertyHandler materialPropertyHandler;
      if (MaterialPropertyHandler.s_PropertyHandlers.TryGetValue(propertyString, out materialPropertyHandler))
        return materialPropertyHandler;
      materialPropertyHandler = MaterialPropertyHandler.GetShaderPropertyHandler(shader, name);
      if (materialPropertyHandler != null && materialPropertyHandler.IsEmpty())
        materialPropertyHandler = (MaterialPropertyHandler) null;
      MaterialPropertyHandler.s_PropertyHandlers[propertyString] = materialPropertyHandler;
      return materialPropertyHandler;
    }
  }
}
