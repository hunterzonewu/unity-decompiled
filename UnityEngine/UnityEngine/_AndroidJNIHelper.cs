// Decompiled with JetBrains decompiler
// Type: UnityEngine._AndroidJNIHelper
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Text;
using UnityEngine.Scripting;

namespace UnityEngine
{
  [UsedByNativeCode]
  internal sealed class _AndroidJNIHelper
  {
    public static IntPtr CreateJavaProxy(int delegateHandle, AndroidJavaProxy proxy)
    {
      return AndroidReflection.NewProxyInstance(delegateHandle, proxy.javaInterface.GetRawClass());
    }

    public static IntPtr CreateJavaRunnable(AndroidJavaRunnable jrunnable)
    {
      return AndroidJNIHelper.CreateJavaProxy((AndroidJavaProxy) new AndroidJavaRunnableProxy(jrunnable));
    }

    public static IntPtr InvokeJavaProxyMethod(AndroidJavaProxy proxy, IntPtr jmethodName, IntPtr jargs)
    {
      int length = 0;
      if (jargs != IntPtr.Zero)
        length = AndroidJNISafe.GetArrayLength(jargs);
      AndroidJavaObject[] javaArgs = new AndroidJavaObject[length];
      for (int index = 0; index < length; ++index)
      {
        IntPtr objectArrayElement = AndroidJNISafe.GetObjectArrayElement(jargs, index);
        javaArgs[index] = !(objectArrayElement != IntPtr.Zero) ? (AndroidJavaObject) null : new AndroidJavaObject(objectArrayElement);
      }
      using (AndroidJavaObject androidJavaObject = proxy.Invoke(AndroidJNI.GetStringUTFChars(jmethodName), javaArgs))
      {
        if (androidJavaObject == null)
          return IntPtr.Zero;
        return AndroidJNI.NewLocalRef(androidJavaObject.GetRawObject());
      }
    }

    public static jvalue[] CreateJNIArgArray(object[] args)
    {
      jvalue[] jvalueArray = new jvalue[args.GetLength(0)];
      int index = 0;
      foreach (object obj in args)
      {
        if (obj == null)
          jvalueArray[index].l = IntPtr.Zero;
        else if (AndroidReflection.IsPrimitive(obj.GetType()))
        {
          if (obj is int)
            jvalueArray[index].i = (int) obj;
          else if (obj is bool)
            jvalueArray[index].z = (bool) obj;
          else if (obj is byte)
            jvalueArray[index].b = (byte) obj;
          else if (obj is short)
            jvalueArray[index].s = (short) obj;
          else if (obj is long)
            jvalueArray[index].j = (long) obj;
          else if (obj is float)
            jvalueArray[index].f = (float) obj;
          else if (obj is double)
            jvalueArray[index].d = (double) obj;
          else if (obj is char)
            jvalueArray[index].c = (char) obj;
        }
        else if (obj is string)
          jvalueArray[index].l = AndroidJNISafe.NewStringUTF((string) obj);
        else if (obj is AndroidJavaClass)
          jvalueArray[index].l = ((AndroidJavaObject) obj).GetRawClass();
        else if (obj is AndroidJavaObject)
          jvalueArray[index].l = ((AndroidJavaObject) obj).GetRawObject();
        else if (obj is Array)
          jvalueArray[index].l = _AndroidJNIHelper.ConvertToJNIArray((Array) obj);
        else if (obj is AndroidJavaProxy)
        {
          jvalueArray[index].l = AndroidJNIHelper.CreateJavaProxy((AndroidJavaProxy) obj);
        }
        else
        {
          if (!(obj is AndroidJavaRunnable))
            throw new Exception("JNI; Unknown argument type '" + (object) obj.GetType() + "'");
          jvalueArray[index].l = AndroidJNIHelper.CreateJavaRunnable((AndroidJavaRunnable) obj);
        }
        ++index;
      }
      return jvalueArray;
    }

    public static object UnboxArray(AndroidJavaObject obj)
    {
      if (obj == null)
        return (object) null;
      AndroidJavaClass androidJavaClass = new AndroidJavaClass("java/lang/reflect/Array");
      AndroidJavaObject androidJavaObject = obj.Call<AndroidJavaObject>("getClass").Call<AndroidJavaObject>("getComponentType");
      string str = androidJavaObject.Call<string>("getName");
      int length = androidJavaClass.Call<int>("getLength", new object[1]{ (object) obj });
      Array array;
      if (androidJavaObject.Call<bool>("IsPrimitive"))
      {
        if ("I" == str)
          array = (Array) new int[length];
        else if ("Z" == str)
          array = (Array) new bool[length];
        else if ("B" == str)
          array = (Array) new byte[length];
        else if ("S" == str)
          array = (Array) new short[length];
        else if ("J" == str)
          array = (Array) new long[length];
        else if ("F" == str)
          array = (Array) new float[length];
        else if ("D" == str)
        {
          array = (Array) new double[length];
        }
        else
        {
          if (!("C" == str))
            throw new Exception("JNI; Unknown argument type '" + str + "'");
          array = (Array) new char[length];
        }
      }
      else
        array = !("java.lang.String" == str) ? (!("java.lang.Class" == str) ? (Array) new AndroidJavaObject[length] : (Array) new AndroidJavaClass[length]) : (Array) new string[length];
      for (int index = 0; index < length; ++index)
        array.SetValue(_AndroidJNIHelper.Unbox(androidJavaClass.CallStatic<AndroidJavaObject>("get", (object) obj, (object) index)), index);
      return (object) array;
    }

    public static object Unbox(AndroidJavaObject obj)
    {
      if (obj == null)
        return (object) null;
      AndroidJavaObject androidJavaObject = obj.Call<AndroidJavaObject>("getClass");
      string str = androidJavaObject.Call<string>("getName");
      if ("java.lang.Integer" == str)
        return (object) obj.Call<int>("intValue");
      if ("java.lang.Boolean" == str)
        return (object) obj.Call<bool>("booleanValue");
      if ("java.lang.Byte" == str)
        return (object) obj.Call<byte>("byteValue");
      if ("java.lang.Short" == str)
        return (object) obj.Call<short>("shortValue");
      if ("java.lang.Long" == str)
        return (object) obj.Call<long>("longValue");
      if ("java.lang.Float" == str)
        return (object) obj.Call<float>("floatValue");
      if ("java.lang.Double" == str)
        return (object) obj.Call<double>("doubleValue");
      if ("java.lang.Character" == str)
        return (object) obj.Call<char>("charValue");
      if ("java.lang.String" == str)
        return (object) obj.Call<string>("toString");
      if ("java.lang.Class" == str)
        return (object) new AndroidJavaClass(obj.GetRawObject());
      if (androidJavaObject.Call<bool>("isArray"))
        return _AndroidJNIHelper.UnboxArray(obj);
      return (object) obj;
    }

    public static AndroidJavaObject Box(object obj)
    {
      if (obj == null)
        return (AndroidJavaObject) null;
      if (AndroidReflection.IsPrimitive(obj.GetType()))
      {
        if (obj is int)
          return new AndroidJavaObject("java.lang.Integer", new object[1]{ (object) (int) obj });
        if (obj is bool)
          return new AndroidJavaObject("java.lang.Boolean", new object[1]{ (object) (bool) obj });
        if (obj is byte)
          return new AndroidJavaObject("java.lang.Byte", new object[1]{ (object) (byte) obj });
        if (obj is short)
          return new AndroidJavaObject("java.lang.Short", new object[1]{ (object) (short) obj });
        if (obj is long)
          return new AndroidJavaObject("java.lang.Long", new object[1]{ (object) (long) obj });
        if (obj is float)
          return new AndroidJavaObject("java.lang.Float", new object[1]{ (object) (float) obj });
        if (obj is double)
          return new AndroidJavaObject("java.lang.Double", new object[1]{ (object) (double) obj });
        if (!(obj is char))
          throw new Exception("JNI; Unknown argument type '" + (object) obj.GetType() + "'");
        return new AndroidJavaObject("java.lang.Character", new object[1]{ (object) (char) obj });
      }
      if (obj is string)
        return new AndroidJavaObject("java.lang.String", new object[1]{ (object) (string) obj });
      if (obj is AndroidJavaClass)
        return new AndroidJavaObject(((AndroidJavaObject) obj).GetRawClass());
      if (obj is AndroidJavaObject)
        return (AndroidJavaObject) obj;
      if (obj is Array)
        return AndroidJavaObject.AndroidJavaObjectDeleteLocalRef(_AndroidJNIHelper.ConvertToJNIArray((Array) obj));
      if (obj is AndroidJavaProxy)
        return AndroidJavaObject.AndroidJavaObjectDeleteLocalRef(AndroidJNIHelper.CreateJavaProxy((AndroidJavaProxy) obj));
      if (obj is AndroidJavaRunnable)
        return AndroidJavaObject.AndroidJavaObjectDeleteLocalRef(AndroidJNIHelper.CreateJavaRunnable((AndroidJavaRunnable) obj));
      throw new Exception("JNI; Unknown argument type '" + (object) obj.GetType() + "'");
    }

    public static void DeleteJNIArgArray(object[] args, jvalue[] jniArgs)
    {
      int index = 0;
      foreach (object obj in args)
      {
        if (obj is string || obj is AndroidJavaRunnable || (obj is AndroidJavaProxy || obj is Array))
          AndroidJNISafe.DeleteLocalRef(jniArgs[index].l);
        ++index;
      }
    }

    public static IntPtr ConvertToJNIArray(Array array)
    {
      System.Type elementType = array.GetType().GetElementType();
      if (AndroidReflection.IsPrimitive(elementType))
      {
        if (elementType == typeof (int))
          return AndroidJNISafe.ToIntArray((int[]) array);
        if (elementType == typeof (bool))
          return AndroidJNISafe.ToBooleanArray((bool[]) array);
        if (elementType == typeof (byte))
          return AndroidJNISafe.ToByteArray((byte[]) array);
        if (elementType == typeof (short))
          return AndroidJNISafe.ToShortArray((short[]) array);
        if (elementType == typeof (long))
          return AndroidJNISafe.ToLongArray((long[]) array);
        if (elementType == typeof (float))
          return AndroidJNISafe.ToFloatArray((float[]) array);
        if (elementType == typeof (double))
          return AndroidJNISafe.ToDoubleArray((double[]) array);
        if (elementType == typeof (char))
          return AndroidJNISafe.ToCharArray((char[]) array);
        return IntPtr.Zero;
      }
      if (elementType == typeof (string))
      {
        string[] strArray = (string[]) array;
        int length = array.GetLength(0);
        IntPtr num = AndroidJNISafe.FindClass("java/lang/String");
        IntPtr array1 = AndroidJNI.NewObjectArray(length, num, IntPtr.Zero);
        for (int index = 0; index < length; ++index)
        {
          IntPtr localref = AndroidJNISafe.NewStringUTF(strArray[index]);
          AndroidJNI.SetObjectArrayElement(array1, index, localref);
          AndroidJNISafe.DeleteLocalRef(localref);
        }
        AndroidJNISafe.DeleteLocalRef(num);
        return array1;
      }
      if (elementType != typeof (AndroidJavaObject))
        throw new Exception("JNI; Unknown array type '" + (object) elementType + "'");
      AndroidJavaObject[] androidJavaObjectArray = (AndroidJavaObject[]) array;
      int length1 = array.GetLength(0);
      IntPtr[] array2 = new IntPtr[length1];
      IntPtr localref1 = AndroidJNISafe.FindClass("java/lang/Object");
      IntPtr type = IntPtr.Zero;
      for (int index = 0; index < length1; ++index)
      {
        if (androidJavaObjectArray[index] != null)
        {
          array2[index] = androidJavaObjectArray[index].GetRawObject();
          IntPtr rawClass = androidJavaObjectArray[index].GetRawClass();
          if (type != rawClass)
            type = !(type == IntPtr.Zero) ? localref1 : rawClass;
        }
        else
          array2[index] = IntPtr.Zero;
      }
      IntPtr objectArray = AndroidJNISafe.ToObjectArray(array2, type);
      AndroidJNISafe.DeleteLocalRef(localref1);
      return objectArray;
    }

    public static ArrayType ConvertFromJNIArray<ArrayType>(IntPtr array)
    {
      System.Type elementType = typeof (ArrayType).GetElementType();
      if (AndroidReflection.IsPrimitive(elementType))
      {
        if (elementType == typeof (int))
          return (ArrayType) AndroidJNISafe.FromIntArray(array);
        if (elementType == typeof (bool))
          return (ArrayType) AndroidJNISafe.FromBooleanArray(array);
        if (elementType == typeof (byte))
          return (ArrayType) AndroidJNISafe.FromByteArray(array);
        if (elementType == typeof (short))
          return (ArrayType) AndroidJNISafe.FromShortArray(array);
        if (elementType == typeof (long))
          return (ArrayType) AndroidJNISafe.FromLongArray(array);
        if (elementType == typeof (float))
          return (ArrayType) AndroidJNISafe.FromFloatArray(array);
        if (elementType == typeof (double))
          return (ArrayType) AndroidJNISafe.FromDoubleArray(array);
        if (elementType == typeof (char))
          return (ArrayType) AndroidJNISafe.FromCharArray(array);
        return default (ArrayType);
      }
      if (elementType == typeof (string))
      {
        int arrayLength = AndroidJNISafe.GetArrayLength(array);
        string[] strArray = new string[arrayLength];
        for (int index = 0; index < arrayLength; ++index)
        {
          IntPtr objectArrayElement = AndroidJNI.GetObjectArrayElement(array, index);
          strArray[index] = AndroidJNISafe.GetStringUTFChars(objectArrayElement);
          AndroidJNISafe.DeleteLocalRef(objectArrayElement);
        }
        return (ArrayType) strArray;
      }
      if (elementType != typeof (AndroidJavaObject))
        throw new Exception("JNI: Unknown generic array type '" + (object) elementType + "'");
      int arrayLength1 = AndroidJNISafe.GetArrayLength(array);
      AndroidJavaObject[] androidJavaObjectArray = new AndroidJavaObject[arrayLength1];
      for (int index = 0; index < arrayLength1; ++index)
      {
        IntPtr objectArrayElement = AndroidJNI.GetObjectArrayElement(array, index);
        androidJavaObjectArray[index] = new AndroidJavaObject(objectArrayElement);
        AndroidJNISafe.DeleteLocalRef(objectArrayElement);
      }
      return (ArrayType) androidJavaObjectArray;
    }

    public static IntPtr GetConstructorID(IntPtr jclass, object[] args)
    {
      return AndroidJNIHelper.GetConstructorID(jclass, _AndroidJNIHelper.GetSignature(args));
    }

    public static IntPtr GetMethodID(IntPtr jclass, string methodName, object[] args, bool isStatic)
    {
      return AndroidJNIHelper.GetMethodID(jclass, methodName, _AndroidJNIHelper.GetSignature(args), isStatic);
    }

    public static IntPtr GetMethodID<ReturnType>(IntPtr jclass, string methodName, object[] args, bool isStatic)
    {
      return AndroidJNIHelper.GetMethodID(jclass, methodName, _AndroidJNIHelper.GetSignature<ReturnType>(args), isStatic);
    }

    public static IntPtr GetFieldID<ReturnType>(IntPtr jclass, string fieldName, bool isStatic)
    {
      return AndroidJNIHelper.GetFieldID(jclass, fieldName, _AndroidJNIHelper.GetSignature((object) typeof (ReturnType)), isStatic);
    }

    public static IntPtr GetConstructorID(IntPtr jclass, string signature)
    {
      IntPtr num = IntPtr.Zero;
      try
      {
        num = AndroidReflection.GetConstructorMember(jclass, signature);
        return AndroidJNISafe.FromReflectedMethod(num);
      }
      catch (Exception ex)
      {
        IntPtr methodId = AndroidJNISafe.GetMethodID(jclass, "<init>", signature);
        if (methodId != IntPtr.Zero)
          return methodId;
        throw ex;
      }
      finally
      {
        AndroidJNISafe.DeleteLocalRef(num);
      }
    }

    public static IntPtr GetMethodID(IntPtr jclass, string methodName, string signature, bool isStatic)
    {
      IntPtr num = IntPtr.Zero;
      try
      {
        num = AndroidReflection.GetMethodMember(jclass, methodName, signature, isStatic);
        return AndroidJNISafe.FromReflectedMethod(num);
      }
      catch (Exception ex)
      {
        IntPtr methodIdFallback = _AndroidJNIHelper.GetMethodIDFallback(jclass, methodName, signature, isStatic);
        if (methodIdFallback != IntPtr.Zero)
          return methodIdFallback;
        throw ex;
      }
      finally
      {
        AndroidJNISafe.DeleteLocalRef(num);
      }
    }

    private static IntPtr GetMethodIDFallback(IntPtr jclass, string methodName, string signature, bool isStatic)
    {
      try
      {
        return !isStatic ? AndroidJNISafe.GetMethodID(jclass, methodName, signature) : AndroidJNISafe.GetStaticMethodID(jclass, methodName, signature);
      }
      catch (Exception ex)
      {
      }
      return IntPtr.Zero;
    }

    public static IntPtr GetFieldID(IntPtr jclass, string fieldName, string signature, bool isStatic)
    {
      IntPtr num1 = IntPtr.Zero;
      try
      {
        num1 = AndroidReflection.GetFieldMember(jclass, fieldName, signature, isStatic);
        return AndroidJNISafe.FromReflectedField(num1);
      }
      catch (Exception ex)
      {
        IntPtr num2 = !isStatic ? AndroidJNISafe.GetFieldID(jclass, fieldName, signature) : AndroidJNISafe.GetStaticFieldID(jclass, fieldName, signature);
        if (num2 != IntPtr.Zero)
          return num2;
        throw ex;
      }
      finally
      {
        AndroidJNISafe.DeleteLocalRef(num1);
      }
    }

    public static string GetSignature(object obj)
    {
      if (obj == null)
        return "Ljava/lang/Object;";
      System.Type type = !(obj is System.Type) ? obj.GetType() : (System.Type) obj;
      if (AndroidReflection.IsPrimitive(type))
      {
        if (type.Equals(typeof (int)))
          return "I";
        if (type.Equals(typeof (bool)))
          return "Z";
        if (type.Equals(typeof (byte)))
          return "B";
        if (type.Equals(typeof (short)))
          return "S";
        if (type.Equals(typeof (long)))
          return "J";
        if (type.Equals(typeof (float)))
          return "F";
        if (type.Equals(typeof (double)))
          return "D";
        if (type.Equals(typeof (char)))
          return "C";
        return string.Empty;
      }
      if (type.Equals(typeof (string)))
        return "Ljava/lang/String;";
      if (obj is AndroidJavaProxy)
        return "L" + new AndroidJavaObject(((AndroidJavaProxy) obj).javaInterface.GetRawClass()).Call<string>("getName") + ";";
      if (type.Equals(typeof (AndroidJavaRunnable)))
        return "Ljava/lang/Runnable;";
      if (type.Equals(typeof (AndroidJavaClass)))
        return "Ljava/lang/Class;";
      if (type.Equals(typeof (AndroidJavaObject)))
      {
        if (obj == type)
          return "Ljava/lang/Object;";
        using (AndroidJavaObject androidJavaObject = ((AndroidJavaObject) obj).Call<AndroidJavaObject>("getClass"))
          return "L" + androidJavaObject.Call<string>("getName") + ";";
      }
      else
      {
        if (AndroidReflection.IsAssignableFrom(typeof (Array), type))
        {
          if (type.GetArrayRank() != 1)
            throw new Exception("JNI: System.Array in n dimensions is not allowed");
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.Append('[');
          stringBuilder.Append(_AndroidJNIHelper.GetSignature((object) type.GetElementType()));
          return stringBuilder.ToString();
        }
        throw new Exception("JNI: Unknown signature for type '" + (object) type + "' (obj = " + obj + ") " + (type != obj ? "instance" : "equal"));
      }
    }

    public static string GetSignature(object[] args)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append('(');
      foreach (object obj in args)
        stringBuilder.Append(_AndroidJNIHelper.GetSignature(obj));
      stringBuilder.Append(")V");
      return stringBuilder.ToString();
    }

    public static string GetSignature<ReturnType>(object[] args)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append('(');
      foreach (object obj in args)
        stringBuilder.Append(_AndroidJNIHelper.GetSignature(obj));
      stringBuilder.Append(')');
      stringBuilder.Append(_AndroidJNIHelper.GetSignature((object) typeof (ReturnType)));
      return stringBuilder.ToString();
    }
  }
}
