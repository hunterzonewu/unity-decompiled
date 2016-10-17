// Decompiled with JetBrains decompiler
// Type: UnityEngine.NScreenBridge
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  public sealed class NScreenBridge : Object
  {
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public NScreenBridge();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InitServer(int id);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Update();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void StartWatchdogForPid(int pid);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetResolution(int x, int y);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetInput(int x, int y, int button, int key, int type);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void ResetInput();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public Texture2D GetScreenTexture();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Shutdown();
  }
}
