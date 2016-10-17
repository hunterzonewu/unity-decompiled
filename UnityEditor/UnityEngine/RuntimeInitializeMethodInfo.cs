// Decompiled with JetBrains decompiler
// Type: UnityEngine.RuntimeInitializeMethodInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.InteropServices;

namespace UnityEngine
{
  [StructLayout(LayoutKind.Sequential)]
  internal sealed class RuntimeInitializeMethodInfo
  {
    private string m_FullClassName;
    private string m_MethodName;
    private int m_OrderNumber;
    private bool m_IsUnityClass;

    internal string fullClassName
    {
      get
      {
        return this.m_FullClassName;
      }
      set
      {
        this.m_FullClassName = value;
      }
    }

    internal string methodName
    {
      get
      {
        return this.m_MethodName;
      }
      set
      {
        this.m_MethodName = value;
      }
    }

    internal int orderNumber
    {
      get
      {
        return this.m_OrderNumber;
      }
      set
      {
        this.m_OrderNumber = value;
      }
    }

    internal bool isUnityClass
    {
      get
      {
        return this.m_IsUnityClass;
      }
      set
      {
        this.m_IsUnityClass = value;
      }
    }
  }
}
