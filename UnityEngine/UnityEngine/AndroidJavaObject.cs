// Decompiled with JetBrains decompiler
// Type: UnityEngine.AndroidJavaObject
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Text;

namespace UnityEngine
{
  /// <summary>
  ///   <para>AndroidJavaObject is the Unity representation of a generic instance of java.lang.Object.</para>
  /// </summary>
  public class AndroidJavaObject : IDisposable
  {
    private static bool enableDebugPrints;
    private bool m_disposed;
    protected IntPtr m_jobject;
    protected IntPtr m_jclass;
    private static AndroidJavaClass s_JavaLangClass;

    protected static AndroidJavaClass JavaLangClass
    {
      get
      {
        if (AndroidJavaObject.s_JavaLangClass == null)
          AndroidJavaObject.s_JavaLangClass = new AndroidJavaClass(AndroidJNISafe.FindClass("java/lang/Class"));
        return AndroidJavaObject.s_JavaLangClass;
      }
    }

    /// <summary>
    ///   <para>Construct an AndroidJavaObject based on the name of the class.</para>
    /// </summary>
    /// <param name="className">Specifies the Java class name (e.g. "&lt;tt&gt;java.lang.String&lt;tt&gt;" or "&lt;tt&gt;javalangString&lt;tt&gt;").</param>
    /// <param name="args">An array of parameters passed to the constructor.</param>
    public AndroidJavaObject(string className, params object[] args)
      : this()
    {
      this._AndroidJavaObject(className, args);
    }

    internal AndroidJavaObject(IntPtr jobject)
      : this()
    {
      if (jobject == IntPtr.Zero)
        throw new Exception("JNI: Init'd AndroidJavaObject with null ptr!");
      IntPtr objectClass = AndroidJNISafe.GetObjectClass(jobject);
      this.m_jobject = AndroidJNI.NewGlobalRef(jobject);
      this.m_jclass = AndroidJNI.NewGlobalRef(objectClass);
      AndroidJNISafe.DeleteLocalRef(objectClass);
    }

    internal AndroidJavaObject()
    {
    }

    ~AndroidJavaObject()
    {
      this.Dispose(true);
    }

    /// <summary>
    ///   <para>IDisposable callback.</para>
    /// </summary>
    public void Dispose()
    {
      this._Dispose();
    }

    /// <summary>
    ///   <para>Call a Java method on an object.</para>
    /// </summary>
    /// <param name="methodName">Specifies which method to call.</param>
    /// <param name="args">An array of parameters passed to the method.</param>
    public void Call(string methodName, params object[] args)
    {
      this._Call(methodName, args);
    }

    /// <summary>
    ///   <para>Call a static Java method on a class.</para>
    /// </summary>
    /// <param name="methodName">Specifies which method to call.</param>
    /// <param name="args">An array of parameters passed to the method.</param>
    public void CallStatic(string methodName, params object[] args)
    {
      this._CallStatic(methodName, args);
    }

    public FieldType Get<FieldType>(string fieldName)
    {
      return this._Get<FieldType>(fieldName);
    }

    public void Set<FieldType>(string fieldName, FieldType val)
    {
      this._Set<FieldType>(fieldName, val);
    }

    public FieldType GetStatic<FieldType>(string fieldName)
    {
      return this._GetStatic<FieldType>(fieldName);
    }

    public void SetStatic<FieldType>(string fieldName, FieldType val)
    {
      this._SetStatic<FieldType>(fieldName, val);
    }

    /// <summary>
    ///   <para>Retrieve the raw jobject pointer to the Java object.</para>
    /// </summary>
    public IntPtr GetRawObject()
    {
      return this._GetRawObject();
    }

    /// <summary>
    ///   <para>Retrieve the raw jclass pointer to the Java class.</para>
    /// </summary>
    public IntPtr GetRawClass()
    {
      return this._GetRawClass();
    }

    public ReturnType Call<ReturnType>(string methodName, params object[] args)
    {
      return this._Call<ReturnType>(methodName, args);
    }

    public ReturnType CallStatic<ReturnType>(string methodName, params object[] args)
    {
      return this._CallStatic<ReturnType>(methodName, args);
    }

    protected void DebugPrint(string msg)
    {
      if (!AndroidJavaObject.enableDebugPrints)
        return;
      Debug.Log((object) msg);
    }

    protected void DebugPrint(string call, string methodName, string signature, object[] args)
    {
      if (!AndroidJavaObject.enableDebugPrints)
        return;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (object obj in args)
      {
        stringBuilder.Append(", ");
        stringBuilder.Append(obj != null ? obj.GetType().ToString() : "<null>");
      }
      Debug.Log((object) (call + "(\"" + methodName + "\"" + stringBuilder.ToString() + ") = " + signature));
    }

    private void _AndroidJavaObject(string className, params object[] args)
    {
      this.DebugPrint("Creating AndroidJavaObject from " + className);
      if (args == null)
        args = new object[1];
      using (AndroidJavaObject androidJavaObject = AndroidJavaObject.FindClass(className))
      {
        this.m_jclass = AndroidJNI.NewGlobalRef(androidJavaObject.GetRawObject());
        jvalue[] jniArgArray = AndroidJNIHelper.CreateJNIArgArray(args);
        try
        {
          IntPtr localref = AndroidJNISafe.NewObject(this.m_jclass, AndroidJNIHelper.GetConstructorID(this.m_jclass, args), jniArgArray);
          this.m_jobject = AndroidJNI.NewGlobalRef(localref);
          AndroidJNISafe.DeleteLocalRef(localref);
        }
        finally
        {
          AndroidJNIHelper.DeleteJNIArgArray(args, jniArgArray);
        }
      }
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.m_disposed)
        return;
      this.m_disposed = true;
      AndroidJNISafe.DeleteGlobalRef(this.m_jobject);
      AndroidJNISafe.DeleteGlobalRef(this.m_jclass);
    }

    protected void _Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected void _Call(string methodName, params object[] args)
    {
      if (args == null)
        args = new object[1];
      IntPtr methodId = AndroidJNIHelper.GetMethodID(this.m_jclass, methodName, args, false);
      jvalue[] jniArgArray = AndroidJNIHelper.CreateJNIArgArray(args);
      try
      {
        AndroidJNISafe.CallVoidMethod(this.m_jobject, methodId, jniArgArray);
      }
      finally
      {
        AndroidJNIHelper.DeleteJNIArgArray(args, jniArgArray);
      }
    }

    protected ReturnType _Call<ReturnType>(string methodName, params object[] args)
    {
      if (args == null)
        args = new object[1];
      IntPtr methodId = AndroidJNIHelper.GetMethodID<ReturnType>(this.m_jclass, methodName, args, false);
      jvalue[] jniArgArray = AndroidJNIHelper.CreateJNIArgArray(args);
      try
      {
        if (AndroidReflection.IsPrimitive(typeof (ReturnType)))
        {
          if (typeof (ReturnType) == typeof (int))
            return (ReturnType) (ValueType) AndroidJNISafe.CallIntMethod(this.m_jobject, methodId, jniArgArray);
          if (typeof (ReturnType) == typeof (bool))
            return (ReturnType) (ValueType) AndroidJNISafe.CallBooleanMethod(this.m_jobject, methodId, jniArgArray);
          if (typeof (ReturnType) == typeof (byte))
            return (ReturnType) (ValueType) AndroidJNISafe.CallByteMethod(this.m_jobject, methodId, jniArgArray);
          if (typeof (ReturnType) == typeof (short))
            return (ReturnType) (ValueType) AndroidJNISafe.CallShortMethod(this.m_jobject, methodId, jniArgArray);
          if (typeof (ReturnType) == typeof (long))
            return (ReturnType) (ValueType) AndroidJNISafe.CallLongMethod(this.m_jobject, methodId, jniArgArray);
          if (typeof (ReturnType) == typeof (float))
            return (ReturnType) (ValueType) AndroidJNISafe.CallFloatMethod(this.m_jobject, methodId, jniArgArray);
          if (typeof (ReturnType) == typeof (double))
            return (ReturnType) (ValueType) AndroidJNISafe.CallDoubleMethod(this.m_jobject, methodId, jniArgArray);
          if (typeof (ReturnType) == typeof (char))
            return (ReturnType) (ValueType) AndroidJNISafe.CallCharMethod(this.m_jobject, methodId, jniArgArray);
          return default (ReturnType);
        }
        if (typeof (ReturnType) == typeof (string))
          return (ReturnType) AndroidJNISafe.CallStringMethod(this.m_jobject, methodId, jniArgArray);
        if (typeof (ReturnType) == typeof (AndroidJavaClass))
          return (ReturnType) AndroidJavaObject.AndroidJavaClassDeleteLocalRef(AndroidJNISafe.CallObjectMethod(this.m_jobject, methodId, jniArgArray));
        if (typeof (ReturnType) == typeof (AndroidJavaObject))
          return (ReturnType) AndroidJavaObject.AndroidJavaObjectDeleteLocalRef(AndroidJNISafe.CallObjectMethod(this.m_jobject, methodId, jniArgArray));
        if (AndroidReflection.IsAssignableFrom(typeof (Array), typeof (ReturnType)))
          return AndroidJNIHelper.ConvertFromJNIArray<ReturnType>(AndroidJNISafe.CallObjectMethod(this.m_jobject, methodId, jniArgArray));
        throw new Exception("JNI: Unknown return type '" + (object) typeof (ReturnType) + "'");
      }
      finally
      {
        AndroidJNIHelper.DeleteJNIArgArray(args, jniArgArray);
      }
    }

    protected FieldType _Get<FieldType>(string fieldName)
    {
      IntPtr fieldId = AndroidJNIHelper.GetFieldID<FieldType>(this.m_jclass, fieldName, false);
      if (AndroidReflection.IsPrimitive(typeof (FieldType)))
      {
        if (typeof (FieldType) == typeof (int))
          return (FieldType) (ValueType) AndroidJNISafe.GetIntField(this.m_jobject, fieldId);
        if (typeof (FieldType) == typeof (bool))
          return (FieldType) (ValueType) AndroidJNISafe.GetBooleanField(this.m_jobject, fieldId);
        if (typeof (FieldType) == typeof (byte))
          return (FieldType) (ValueType) AndroidJNISafe.GetByteField(this.m_jobject, fieldId);
        if (typeof (FieldType) == typeof (short))
          return (FieldType) (ValueType) AndroidJNISafe.GetShortField(this.m_jobject, fieldId);
        if (typeof (FieldType) == typeof (long))
          return (FieldType) (ValueType) AndroidJNISafe.GetLongField(this.m_jobject, fieldId);
        if (typeof (FieldType) == typeof (float))
          return (FieldType) (ValueType) AndroidJNISafe.GetFloatField(this.m_jobject, fieldId);
        if (typeof (FieldType) == typeof (double))
          return (FieldType) (ValueType) AndroidJNISafe.GetDoubleField(this.m_jobject, fieldId);
        if (typeof (FieldType) == typeof (char))
          return (FieldType) (ValueType) AndroidJNISafe.GetCharField(this.m_jobject, fieldId);
        return default (FieldType);
      }
      if (typeof (FieldType) == typeof (string))
        return (FieldType) AndroidJNISafe.GetStringField(this.m_jobject, fieldId);
      if (typeof (FieldType) == typeof (AndroidJavaClass))
        return (FieldType) AndroidJavaObject.AndroidJavaClassDeleteLocalRef(AndroidJNISafe.GetObjectField(this.m_jobject, fieldId));
      if (typeof (FieldType) == typeof (AndroidJavaObject))
        return (FieldType) AndroidJavaObject.AndroidJavaObjectDeleteLocalRef(AndroidJNISafe.GetObjectField(this.m_jobject, fieldId));
      if (AndroidReflection.IsAssignableFrom(typeof (Array), typeof (FieldType)))
        return AndroidJNIHelper.ConvertFromJNIArray<FieldType>(AndroidJNISafe.GetObjectField(this.m_jobject, fieldId));
      throw new Exception("JNI: Unknown field type '" + (object) typeof (FieldType) + "'");
    }

    protected void _Set<FieldType>(string fieldName, FieldType val)
    {
      IntPtr fieldId = AndroidJNIHelper.GetFieldID<FieldType>(this.m_jclass, fieldName, false);
      if (AndroidReflection.IsPrimitive(typeof (FieldType)))
      {
        if (typeof (FieldType) == typeof (int))
          AndroidJNISafe.SetIntField(this.m_jobject, fieldId, (int) (object) val);
        else if (typeof (FieldType) == typeof (bool))
          AndroidJNISafe.SetBooleanField(this.m_jobject, fieldId, (bool) (object) val);
        else if (typeof (FieldType) == typeof (byte))
          AndroidJNISafe.SetByteField(this.m_jobject, fieldId, (byte) (object) val);
        else if (typeof (FieldType) == typeof (short))
          AndroidJNISafe.SetShortField(this.m_jobject, fieldId, (short) (object) val);
        else if (typeof (FieldType) == typeof (long))
          AndroidJNISafe.SetLongField(this.m_jobject, fieldId, (long) (object) val);
        else if (typeof (FieldType) == typeof (float))
          AndroidJNISafe.SetFloatField(this.m_jobject, fieldId, (float) (object) val);
        else if (typeof (FieldType) == typeof (double))
        {
          AndroidJNISafe.SetDoubleField(this.m_jobject, fieldId, (double) (object) val);
        }
        else
        {
          if (typeof (FieldType) != typeof (char))
            return;
          AndroidJNISafe.SetCharField(this.m_jobject, fieldId, (char) (object) val);
        }
      }
      else if (typeof (FieldType) == typeof (string))
        AndroidJNISafe.SetStringField(this.m_jobject, fieldId, (string) (object) val);
      else if (typeof (FieldType) == typeof (AndroidJavaClass))
        AndroidJNISafe.SetObjectField(this.m_jobject, fieldId, ((AndroidJavaObject) (object) val).m_jclass);
      else if (typeof (FieldType) == typeof (AndroidJavaObject))
      {
        AndroidJNISafe.SetObjectField(this.m_jobject, fieldId, ((AndroidJavaObject) (object) val).m_jobject);
      }
      else
      {
        if (!AndroidReflection.IsAssignableFrom(typeof (Array), typeof (FieldType)))
          throw new Exception("JNI: Unknown field type '" + (object) typeof (FieldType) + "'");
        IntPtr jniArray = AndroidJNIHelper.ConvertToJNIArray((Array) (object) val);
        AndroidJNISafe.SetObjectField(this.m_jclass, fieldId, jniArray);
      }
    }

    protected void _CallStatic(string methodName, params object[] args)
    {
      if (args == null)
        args = new object[1];
      IntPtr methodId = AndroidJNIHelper.GetMethodID(this.m_jclass, methodName, args, true);
      jvalue[] jniArgArray = AndroidJNIHelper.CreateJNIArgArray(args);
      try
      {
        AndroidJNISafe.CallStaticVoidMethod(this.m_jclass, methodId, jniArgArray);
      }
      finally
      {
        AndroidJNIHelper.DeleteJNIArgArray(args, jniArgArray);
      }
    }

    protected ReturnType _CallStatic<ReturnType>(string methodName, params object[] args)
    {
      if (args == null)
        args = new object[1];
      IntPtr methodId = AndroidJNIHelper.GetMethodID<ReturnType>(this.m_jclass, methodName, args, true);
      jvalue[] jniArgArray = AndroidJNIHelper.CreateJNIArgArray(args);
      try
      {
        if (AndroidReflection.IsPrimitive(typeof (ReturnType)))
        {
          if (typeof (ReturnType) == typeof (int))
            return (ReturnType) (ValueType) AndroidJNISafe.CallStaticIntMethod(this.m_jclass, methodId, jniArgArray);
          if (typeof (ReturnType) == typeof (bool))
            return (ReturnType) (ValueType) AndroidJNISafe.CallStaticBooleanMethod(this.m_jclass, methodId, jniArgArray);
          if (typeof (ReturnType) == typeof (byte))
            return (ReturnType) (ValueType) AndroidJNISafe.CallStaticByteMethod(this.m_jclass, methodId, jniArgArray);
          if (typeof (ReturnType) == typeof (short))
            return (ReturnType) (ValueType) AndroidJNISafe.CallStaticShortMethod(this.m_jclass, methodId, jniArgArray);
          if (typeof (ReturnType) == typeof (long))
            return (ReturnType) (ValueType) AndroidJNISafe.CallStaticLongMethod(this.m_jclass, methodId, jniArgArray);
          if (typeof (ReturnType) == typeof (float))
            return (ReturnType) (ValueType) AndroidJNISafe.CallStaticFloatMethod(this.m_jclass, methodId, jniArgArray);
          if (typeof (ReturnType) == typeof (double))
            return (ReturnType) (ValueType) AndroidJNISafe.CallStaticDoubleMethod(this.m_jclass, methodId, jniArgArray);
          if (typeof (ReturnType) == typeof (char))
            return (ReturnType) (ValueType) AndroidJNISafe.CallStaticCharMethod(this.m_jclass, methodId, jniArgArray);
          return default (ReturnType);
        }
        if (typeof (ReturnType) == typeof (string))
          return (ReturnType) AndroidJNISafe.CallStaticStringMethod(this.m_jclass, methodId, jniArgArray);
        if (typeof (ReturnType) == typeof (AndroidJavaClass))
          return (ReturnType) AndroidJavaObject.AndroidJavaClassDeleteLocalRef(AndroidJNISafe.CallStaticObjectMethod(this.m_jclass, methodId, jniArgArray));
        if (typeof (ReturnType) == typeof (AndroidJavaObject))
          return (ReturnType) AndroidJavaObject.AndroidJavaObjectDeleteLocalRef(AndroidJNISafe.CallStaticObjectMethod(this.m_jclass, methodId, jniArgArray));
        if (AndroidReflection.IsAssignableFrom(typeof (Array), typeof (ReturnType)))
          return AndroidJNIHelper.ConvertFromJNIArray<ReturnType>(AndroidJNISafe.CallStaticObjectMethod(this.m_jclass, methodId, jniArgArray));
        throw new Exception("JNI: Unknown return type '" + (object) typeof (ReturnType) + "'");
      }
      finally
      {
        AndroidJNIHelper.DeleteJNIArgArray(args, jniArgArray);
      }
    }

    protected FieldType _GetStatic<FieldType>(string fieldName)
    {
      IntPtr fieldId = AndroidJNIHelper.GetFieldID<FieldType>(this.m_jclass, fieldName, true);
      if (AndroidReflection.IsPrimitive(typeof (FieldType)))
      {
        if (typeof (FieldType) == typeof (int))
          return (FieldType) (ValueType) AndroidJNISafe.GetStaticIntField(this.m_jclass, fieldId);
        if (typeof (FieldType) == typeof (bool))
          return (FieldType) (ValueType) AndroidJNISafe.GetStaticBooleanField(this.m_jclass, fieldId);
        if (typeof (FieldType) == typeof (byte))
          return (FieldType) (ValueType) AndroidJNISafe.GetStaticByteField(this.m_jclass, fieldId);
        if (typeof (FieldType) == typeof (short))
          return (FieldType) (ValueType) AndroidJNISafe.GetStaticShortField(this.m_jclass, fieldId);
        if (typeof (FieldType) == typeof (long))
          return (FieldType) (ValueType) AndroidJNISafe.GetStaticLongField(this.m_jclass, fieldId);
        if (typeof (FieldType) == typeof (float))
          return (FieldType) (ValueType) AndroidJNISafe.GetStaticFloatField(this.m_jclass, fieldId);
        if (typeof (FieldType) == typeof (double))
          return (FieldType) (ValueType) AndroidJNISafe.GetStaticDoubleField(this.m_jclass, fieldId);
        if (typeof (FieldType) == typeof (char))
          return (FieldType) (ValueType) AndroidJNISafe.GetStaticCharField(this.m_jclass, fieldId);
        return default (FieldType);
      }
      if (typeof (FieldType) == typeof (string))
        return (FieldType) AndroidJNISafe.GetStaticStringField(this.m_jclass, fieldId);
      if (typeof (FieldType) == typeof (AndroidJavaClass))
        return (FieldType) AndroidJavaObject.AndroidJavaClassDeleteLocalRef(AndroidJNISafe.GetStaticObjectField(this.m_jclass, fieldId));
      if (typeof (FieldType) == typeof (AndroidJavaObject))
        return (FieldType) AndroidJavaObject.AndroidJavaObjectDeleteLocalRef(AndroidJNISafe.GetStaticObjectField(this.m_jclass, fieldId));
      if (AndroidReflection.IsAssignableFrom(typeof (Array), typeof (FieldType)))
        return AndroidJNIHelper.ConvertFromJNIArray<FieldType>(AndroidJNISafe.GetStaticObjectField(this.m_jclass, fieldId));
      throw new Exception("JNI: Unknown field type '" + (object) typeof (FieldType) + "'");
    }

    protected void _SetStatic<FieldType>(string fieldName, FieldType val)
    {
      IntPtr fieldId = AndroidJNIHelper.GetFieldID<FieldType>(this.m_jclass, fieldName, true);
      if (AndroidReflection.IsPrimitive(typeof (FieldType)))
      {
        if (typeof (FieldType) == typeof (int))
          AndroidJNISafe.SetStaticIntField(this.m_jclass, fieldId, (int) (object) val);
        else if (typeof (FieldType) == typeof (bool))
          AndroidJNISafe.SetStaticBooleanField(this.m_jclass, fieldId, (bool) (object) val);
        else if (typeof (FieldType) == typeof (byte))
          AndroidJNISafe.SetStaticByteField(this.m_jclass, fieldId, (byte) (object) val);
        else if (typeof (FieldType) == typeof (short))
          AndroidJNISafe.SetStaticShortField(this.m_jclass, fieldId, (short) (object) val);
        else if (typeof (FieldType) == typeof (long))
          AndroidJNISafe.SetStaticLongField(this.m_jclass, fieldId, (long) (object) val);
        else if (typeof (FieldType) == typeof (float))
          AndroidJNISafe.SetStaticFloatField(this.m_jclass, fieldId, (float) (object) val);
        else if (typeof (FieldType) == typeof (double))
        {
          AndroidJNISafe.SetStaticDoubleField(this.m_jclass, fieldId, (double) (object) val);
        }
        else
        {
          if (typeof (FieldType) != typeof (char))
            return;
          AndroidJNISafe.SetStaticCharField(this.m_jclass, fieldId, (char) (object) val);
        }
      }
      else if (typeof (FieldType) == typeof (string))
        AndroidJNISafe.SetStaticStringField(this.m_jclass, fieldId, (string) (object) val);
      else if (typeof (FieldType) == typeof (AndroidJavaClass))
        AndroidJNISafe.SetStaticObjectField(this.m_jclass, fieldId, ((AndroidJavaObject) (object) val).m_jclass);
      else if (typeof (FieldType) == typeof (AndroidJavaObject))
      {
        AndroidJNISafe.SetStaticObjectField(this.m_jclass, fieldId, ((AndroidJavaObject) (object) val).m_jobject);
      }
      else
      {
        if (!AndroidReflection.IsAssignableFrom(typeof (Array), typeof (FieldType)))
          throw new Exception("JNI: Unknown field type '" + (object) typeof (FieldType) + "'");
        IntPtr jniArray = AndroidJNIHelper.ConvertToJNIArray((Array) (object) val);
        AndroidJNISafe.SetStaticObjectField(this.m_jclass, fieldId, jniArray);
      }
    }

    internal static AndroidJavaObject AndroidJavaObjectDeleteLocalRef(IntPtr jobject)
    {
      try
      {
        return new AndroidJavaObject(jobject);
      }
      finally
      {
        AndroidJNISafe.DeleteLocalRef(jobject);
      }
    }

    internal static AndroidJavaClass AndroidJavaClassDeleteLocalRef(IntPtr jclass)
    {
      try
      {
        return new AndroidJavaClass(jclass);
      }
      finally
      {
        AndroidJNISafe.DeleteLocalRef(jclass);
      }
    }

    protected IntPtr _GetRawObject()
    {
      return this.m_jobject;
    }

    protected IntPtr _GetRawClass()
    {
      return this.m_jclass;
    }

    protected static AndroidJavaObject FindClass(string name)
    {
      return AndroidJavaObject.JavaLangClass.CallStatic<AndroidJavaObject>("forName", new object[1]{ (object) name.Replace('/', '.') });
    }
  }
}
