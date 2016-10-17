// Decompiled with JetBrains decompiler
// Type: UnityEngine.AndroidJavaRunnableProxy
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

namespace UnityEngine
{
  internal class AndroidJavaRunnableProxy : AndroidJavaProxy
  {
    private AndroidJavaRunnable mRunnable;

    public AndroidJavaRunnableProxy(AndroidJavaRunnable runnable)
      : base("java/lang/Runnable")
    {
      this.mRunnable = runnable;
    }

    public void run()
    {
      this.mRunnable();
    }
  }
}
