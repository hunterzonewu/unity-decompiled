// Decompiled with JetBrains decompiler
// Type: UnityEditor.GenericInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class GenericInspector : Editor
  {
    private AudioFilterGUI m_AudioFilterGUI;

    internal override bool GetOptimizedGUIBlock(bool isDirty, bool isVisible, out OptimizedGUIBlock block, out float height)
    {
      bool blockImplementation = this.GetOptimizedGUIBlockImplementation(isDirty, isVisible, out block, out height);
      if (this.target is MonoBehaviour && AudioUtil.HaveAudioCallback(this.target as MonoBehaviour) && AudioUtil.GetCustomFilterChannelCount(this.target as MonoBehaviour) > 0 || this.IsMissingMonoBehaviourTarget())
        return false;
      return blockImplementation;
    }

    internal override bool OnOptimizedInspectorGUI(Rect contentRect)
    {
      return this.OptimizedInspectorGUIImplementation(contentRect);
    }

    public bool MissingMonoBehaviourGUI()
    {
      this.serializedObject.Update();
      SerializedProperty property = this.serializedObject.FindProperty("m_Script");
      if (property == null)
        return false;
      EditorGUILayout.PropertyField(property);
      MonoScript objectReferenceValue = property.objectReferenceValue as MonoScript;
      bool flag = true;
      if ((Object) objectReferenceValue != (Object) null && objectReferenceValue.GetScriptTypeWasJustCreatedFromComponentMenu())
        flag = false;
      if (flag)
        EditorGUILayout.HelpBox(EditorGUIUtility.TextContent("The associated script can not be loaded.\nPlease fix any compile errors\nand assign a valid script.").text, MessageType.Warning, true);
      if (this.serializedObject.ApplyModifiedProperties())
        EditorUtility.ForceRebuildInspectors();
      return true;
    }

    private bool IsMissingMonoBehaviourTarget()
    {
      if (this.target.GetType() != typeof (MonoBehaviour))
        return this.target.GetType() == typeof (ScriptableObject);
      return true;
    }

    public override void OnInspectorGUI()
    {
      if (this.IsMissingMonoBehaviourTarget() && this.MissingMonoBehaviourGUI())
        return;
      base.OnInspectorGUI();
      if (!(this.target is MonoBehaviour) || !AudioUtil.HaveAudioCallback(this.target as MonoBehaviour) || AudioUtil.GetCustomFilterChannelCount(this.target as MonoBehaviour) <= 0)
        return;
      if (this.m_AudioFilterGUI == null)
        this.m_AudioFilterGUI = new AudioFilterGUI();
      this.m_AudioFilterGUI.DrawAudioFilterGUI(this.target as MonoBehaviour);
    }
  }
}
