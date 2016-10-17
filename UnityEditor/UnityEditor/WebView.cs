// Decompiled with JetBrains decompiler
// Type: UnityEditor.WebView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

namespace UnityEditor
{
  [StructLayout(LayoutKind.Sequential)]
  internal sealed class WebView : ScriptableObject
  {
    [SerializeField]
    private MonoReloadableIntPtr WebViewWindow;

    public static implicit operator bool(WebView exists)
    {
      if ((Object) exists != (Object) null)
        return !exists.IntPtrIsNull();
      return false;
    }

    public void OnDestroy()
    {
      this.DestroyWebView();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void DestroyWebView();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InitWebView(GUIView host, int x, int y, int width, int height, bool showResizeHandle);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void ExecuteJavascript(string scriptCode);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void LoadURL(string url);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void LoadFile(string path);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool DefineScriptObject(string path, ScriptableObject obj);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetDelegateObject(ScriptableObject value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetHostView(GUIView view);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetSizeAndPosition(int x, int y, int width, int height);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetFocus(bool value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool HasApplicationFocus();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetApplicationFocus(bool applicationFocus);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Show();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Hide();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Back();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Forward();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SendOnEvent(string jsonStr);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Reload();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void AllowRightClickMenu(bool allowRightClickMenu);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void ShowDevTools();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void ToggleMaximize();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void OnDomainReload();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private bool IntPtrIsNull();
  }
}
