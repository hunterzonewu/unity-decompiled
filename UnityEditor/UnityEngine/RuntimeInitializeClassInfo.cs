// Decompiled with JetBrains decompiler
// Type: UnityEngine.RuntimeInitializeClassInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.InteropServices;

namespace UnityEngine
{
  [StructLayout(LayoutKind.Sequential)]
  internal sealed class RuntimeInitializeClassInfo
  {
    private string m_AssemblyName;
    private string m_ClassName;
    private string[] m_MethodNames;
    private RuntimeInitializeLoadType[] m_LoadTypes;

    internal string assemblyName
    {
      get
      {
        return this.m_AssemblyName;
      }
      set
      {
        this.m_AssemblyName = value;
      }
    }

    internal string className
    {
      get
      {
        return this.m_ClassName;
      }
      set
      {
        this.m_ClassName = value;
      }
    }

    internal string[] methodNames
    {
      get
      {
        return this.m_MethodNames;
      }
      set
      {
        this.m_MethodNames = value;
      }
    }

    internal RuntimeInitializeLoadType[] loadTypes
    {
      get
      {
        return this.m_LoadTypes;
      }
      set
      {
        this.m_LoadTypes = value;
      }
    }
  }
}
