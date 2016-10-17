// Decompiled with JetBrains decompiler
// Type: UnityEditor.Modules.DefaultPluginImporterExtension
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityEditor.Modules
{
  internal class DefaultPluginImporterExtension : IPluginImporterExtension
  {
    protected bool hasModified;
    protected DefaultPluginImporterExtension.Property[] properties;
    internal bool propertiesRefreshed;

    public DefaultPluginImporterExtension(DefaultPluginImporterExtension.Property[] properties)
    {
      this.properties = properties;
    }

    public virtual void ResetValues(PluginImporterInspector inspector)
    {
      this.hasModified = false;
      this.RefreshProperties(inspector);
    }

    public virtual bool HasModified(PluginImporterInspector inspector)
    {
      return this.hasModified;
    }

    public virtual void Apply(PluginImporterInspector inspector)
    {
      if (!this.propertiesRefreshed)
        return;
      foreach (DefaultPluginImporterExtension.Property property in this.properties)
        property.Apply(inspector);
    }

    public virtual void OnEnable(PluginImporterInspector inspector)
    {
      this.RefreshProperties(inspector);
    }

    public virtual void OnDisable(PluginImporterInspector inspector)
    {
    }

    public virtual void OnPlatformSettingsGUI(PluginImporterInspector inspector)
    {
      if (!this.propertiesRefreshed)
        this.RefreshProperties(inspector);
      EditorGUI.BeginChangeCheck();
      foreach (DefaultPluginImporterExtension.Property property in this.properties)
        property.OnGUI(inspector);
      if (!EditorGUI.EndChangeCheck())
        return;
      this.hasModified = true;
    }

    protected virtual void RefreshProperties(PluginImporterInspector inspector)
    {
      foreach (DefaultPluginImporterExtension.Property property in this.properties)
        property.Reset(inspector);
      this.propertiesRefreshed = true;
    }

    public virtual string CalculateFinalPluginPath(string platformName, PluginImporter imp)
    {
      return Path.GetFileName(imp.assetPath);
    }

    protected Dictionary<string, List<PluginImporter>> GetCompatiblePlugins(string buildTargetName)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      DefaultPluginImporterExtension.\u003CGetCompatiblePlugins\u003Ec__AnonStorey7D pluginsCAnonStorey7D = new DefaultPluginImporterExtension.\u003CGetCompatiblePlugins\u003Ec__AnonStorey7D();
      // ISSUE: reference to a compiler-generated field
      pluginsCAnonStorey7D.buildTargetName = buildTargetName;
      // ISSUE: reference to a compiler-generated method
      PluginImporter[] array = ((IEnumerable<PluginImporter>) PluginImporter.GetAllImporters()).Where<PluginImporter>(new Func<PluginImporter, bool>(pluginsCAnonStorey7D.\u003C\u003Em__125)).ToArray<PluginImporter>();
      Dictionary<string, List<PluginImporter>> dictionary = new Dictionary<string, List<PluginImporter>>();
      // ISSUE: reference to a compiler-generated field
      BuildTargetGroup targetGroupByName = BuildPipeline.GetBuildTargetGroupByName(pluginsCAnonStorey7D.buildTargetName);
      foreach (PluginImporter imp in array)
      {
        if (!string.IsNullOrEmpty(imp.assetPath))
        {
          // ISSUE: reference to a compiler-generated field
          string finalPluginPath = this.CalculateFinalPluginPath(pluginsCAnonStorey7D.buildTargetName, imp);
          if (!string.IsNullOrEmpty(finalPluginPath) && (!imp.isNativePlugin || targetGroupByName != BuildTargetGroup.WebPlayer))
          {
            List<PluginImporter> pluginImporterList = (List<PluginImporter>) null;
            if (!dictionary.TryGetValue(finalPluginPath, out pluginImporterList))
            {
              pluginImporterList = new List<PluginImporter>();
              dictionary[finalPluginPath] = pluginImporterList;
            }
            pluginImporterList.Add(imp);
          }
        }
      }
      return dictionary;
    }

    public virtual bool CheckFileCollisions(string buildTargetName)
    {
      Dictionary<string, List<PluginImporter>> compatiblePlugins = this.GetCompatiblePlugins(buildTargetName);
      bool flag = false;
      StringBuilder stringBuilder = new StringBuilder();
      using (Dictionary<string, List<PluginImporter>>.Enumerator enumerator1 = compatiblePlugins.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          KeyValuePair<string, List<PluginImporter>> current1 = enumerator1.Current;
          List<PluginImporter> pluginImporterList = current1.Value;
          if (pluginImporterList.Count != 1)
          {
            flag = true;
            stringBuilder.AppendLine(string.Format("Plugin '{0}' is used from several locations:", (object) Path.GetFileName(current1.Key)));
            using (List<PluginImporter>.Enumerator enumerator2 = pluginImporterList.GetEnumerator())
            {
              while (enumerator2.MoveNext())
              {
                PluginImporter current2 = enumerator2.Current;
                stringBuilder.AppendLine(" " + current2.assetPath + " would be copied to <PluginPath>/" + current1.Key.Replace("\\", "/"));
              }
            }
          }
        }
      }
      if (flag)
      {
        stringBuilder.AppendLine("Please fix plugin settings and try again.");
        Debug.LogError((object) stringBuilder.ToString());
      }
      return flag;
    }

    internal class Property
    {
      internal GUIContent name { get; set; }

      internal string key { get; set; }

      internal object defaultValue { get; set; }

      internal System.Type type { get; set; }

      internal string platformName { get; set; }

      internal object value { get; set; }

      internal Property(string name, string key, object defaultValue, string platformName)
        : this(new GUIContent(name), key, defaultValue, platformName)
      {
      }

      internal Property(GUIContent name, string key, object defaultValue, string platformName)
      {
        this.name = name;
        this.key = key;
        this.defaultValue = defaultValue;
        this.type = defaultValue.GetType();
        this.platformName = platformName;
      }

      internal virtual void Reset(PluginImporterInspector inspector)
      {
        string platformData = inspector.importer.GetPlatformData(this.platformName, this.key);
        try
        {
          this.value = TypeDescriptor.GetConverter(this.type).ConvertFromString(platformData);
        }
        catch
        {
          this.value = this.defaultValue;
          if (string.IsNullOrEmpty(platformData))
            return;
          Debug.LogWarning((object) ("Failed to parse value ('" + platformData + "') for " + this.key + ", platform: " + this.platformName + ", type: " + (object) this.type + ". Default value will be set '" + this.defaultValue + "'"));
        }
      }

      internal virtual void Apply(PluginImporterInspector inspector)
      {
        inspector.importer.SetPlatformData(this.platformName, this.key, this.value.ToString());
      }

      internal virtual void OnGUI(PluginImporterInspector inspector)
      {
        if (this.type == typeof (bool))
          this.value = (object) EditorGUILayout.Toggle(this.name, (bool) this.value, new GUILayoutOption[0]);
        else if (this.type.IsEnum)
        {
          this.value = (object) EditorGUILayout.EnumPopup(this.name, (Enum) this.value, new GUILayoutOption[0]);
        }
        else
        {
          if (this.type != typeof (string))
            throw new NotImplementedException("Don't know how to display value.");
          this.value = (object) EditorGUILayout.TextField(this.name, (string) this.value, new GUILayoutOption[0]);
        }
      }
    }
  }
}
