// Decompiled with JetBrains decompiler
// Type: UnityEditor.GUID
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  [RequiredByNativeCode]
  public struct GUID
  {
    private uint m_Value0;
    private uint m_Value1;
    private uint m_Value2;
    private uint m_Value3;

    public GUID(string hexRepresentation)
    {
      this.m_Value0 = 0U;
      this.m_Value1 = 0U;
      this.m_Value2 = 0U;
      this.m_Value3 = 0U;
      this.ParseExact(hexRepresentation);
    }

    public static bool operator ==(GUID x, GUID y)
    {
      if ((int) x.m_Value0 == (int) y.m_Value0 && (int) x.m_Value1 == (int) y.m_Value1 && (int) x.m_Value2 == (int) y.m_Value2)
        return (int) x.m_Value3 == (int) y.m_Value3;
      return false;
    }

    public static bool operator !=(GUID x, GUID y)
    {
      return !(x == y);
    }

    public override bool Equals(object obj)
    {
      return (GUID) obj == this;
    }

    public override int GetHashCode()
    {
      return this.m_Value0.GetHashCode();
    }

    public bool Empty()
    {
      if ((int) this.m_Value0 == 0 && (int) this.m_Value1 == 0 && (int) this.m_Value2 == 0)
        return (int) this.m_Value3 == 0;
      return false;
    }

    public bool ParseExact(string hex)
    {
      this.HexToGUIDInternal(hex, ref this);
      return !this.Empty();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void HexToGUIDInternal(string hex, ref GUID guid);
  }
}
