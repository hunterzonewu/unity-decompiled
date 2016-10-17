// Decompiled with JetBrains decompiler
// Type: UnityEngine.AndroidReflection
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  internal class AndroidReflection
  {
    private static IntPtr s_ReflectionHelperClass = AndroidJNI.NewGlobalRef(AndroidJNISafe.FindClass("com/unity3d/player/ReflectionHelper"));
    private static IntPtr s_ReflectionHelperGetConstructorID = AndroidReflection.GetStaticMethodID("com/unity3d/player/ReflectionHelper", "getConstructorID", "(Ljava/lang/Class;Ljava/lang/String;)Ljava/lang/reflect/Constructor;");
    private static IntPtr s_ReflectionHelperGetMethodID = AndroidReflection.GetStaticMethodID("com/unity3d/player/ReflectionHelper", "getMethodID", "(Ljava/lang/Class;Ljava/lang/String;Ljava/lang/String;Z)Ljava/lang/reflect/Method;");
    private static IntPtr s_ReflectionHelperGetFieldID = AndroidReflection.GetStaticMethodID("com/unity3d/player/ReflectionHelper", "getFieldID", "(Ljava/lang/Class;Ljava/lang/String;Ljava/lang/String;Z)Ljava/lang/reflect/Field;");
    private static IntPtr s_ReflectionHelperNewProxyInstance = AndroidReflection.GetStaticMethodID("com/unity3d/player/ReflectionHelper", "newProxyInstance", "(ILjava/lang/Class;)Ljava/lang/Object;");
    private const string RELECTION_HELPER_CLASS_NAME = "com/unity3d/player/ReflectionHelper";

    public static bool IsPrimitive(System.Type t)
    {
      return t.IsPrimitive;
    }

    public static bool IsAssignableFrom(System.Type t, System.Type from)
    {
      return t.IsAssignableFrom(from);
    }

    private static IntPtr GetStaticMethodID(string clazz, string methodName, string signature)
    {
      IntPtr num = AndroidJNISafe.FindClass(clazz);
      try
      {
        return AndroidJNISafe.GetStaticMethodID(num, methodName, signature);
      }
      finally
      {
        AndroidJNISafe.DeleteLocalRef(num);
      }
    }

    public static IntPtr GetConstructorMember(IntPtr jclass, string signature)
    {
      jvalue[] args = new jvalue[2];
      try
      {
        args[0].l = jclass;
        args[1].l = AndroidJNISafe.NewStringUTF(signature);
        return AndroidJNISafe.CallStaticObjectMethod(AndroidReflection.s_ReflectionHelperClass, AndroidReflection.s_ReflectionHelperGetConstructorID, args);
      }
      finally
      {
        AndroidJNISafe.DeleteLocalRef(args[1].l);
      }
    }

    public static IntPtr GetMethodMember(IntPtr jclass, string methodName, string signature, bool isStatic)
    {
      jvalue[] args = new jvalue[4];
      try
      {
        args[0].l = jclass;
        args[1].l = AndroidJNISafe.NewStringUTF(methodName);
        args[2].l = AndroidJNISafe.NewStringUTF(signature);
        args[3].z = isStatic;
        return AndroidJNISafe.CallStaticObjectMethod(AndroidReflection.s_ReflectionHelperClass, AndroidReflection.s_ReflectionHelperGetMethodID, args);
      }
      finally
      {
        AndroidJNISafe.DeleteLocalRef(args[1].l);
        AndroidJNISafe.DeleteLocalRef(args[2].l);
      }
    }

    public static IntPtr GetFieldMember(IntPtr jclass, string fieldName, string signature, bool isStatic)
    {
      jvalue[] args = new jvalue[4];
      try
      {
        args[0].l = jclass;
        args[1].l = AndroidJNISafe.NewStringUTF(fieldName);
        args[2].l = AndroidJNISafe.NewStringUTF(signature);
        args[3].z = isStatic;
        return AndroidJNISafe.CallStaticObjectMethod(AndroidReflection.s_ReflectionHelperClass, AndroidReflection.s_ReflectionHelperGetFieldID, args);
      }
      finally
      {
        AndroidJNISafe.DeleteLocalRef(args[1].l);
        AndroidJNISafe.DeleteLocalRef(args[2].l);
      }
    }

    public static IntPtr NewProxyInstance(int delegateHandle, IntPtr interfaze)
    {
      jvalue[] args = new jvalue[2];
      args[0].i = delegateHandle;
      args[1].l = interfaze;
      return AndroidJNISafe.CallStaticObjectMethod(AndroidReflection.s_ReflectionHelperClass, AndroidReflection.s_ReflectionHelperNewProxyInstance, args);
    }
  }
}
