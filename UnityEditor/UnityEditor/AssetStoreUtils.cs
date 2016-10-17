// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetStoreUtils
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Internal;

namespace UnityEditor
{
  internal sealed class AssetStoreUtils
  {
    private const string kAssetStoreUrl = "https://shawarma.unity3d.com";

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Download(string id, string url, string[] destination, string key, string jsonData, bool resumeOK, [DefaultValue("null")] AssetStoreUtils.DownloadDoneCallback doneCallback);

    [ExcludeFromDocs]
    public static void Download(string id, string url, string[] destination, string key, string jsonData, bool resumeOK)
    {
      AssetStoreUtils.DownloadDoneCallback doneCallback = (AssetStoreUtils.DownloadDoneCallback) null;
      AssetStoreUtils.Download(id, url, destination, key, jsonData, resumeOK, doneCallback);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string CheckDownload(string id, string url, string[] destination, string key);

    private static void HexStringToByteArray(string hex, byte[] array, int offset)
    {
      if (offset + array.Length * 2 > hex.Length)
        throw new ArgumentException("Hex string too short");
      for (int index = 0; index < array.Length; ++index)
      {
        string s = hex.Substring(index * 2 + offset, 2);
        array[index] = byte.Parse(s, NumberStyles.HexNumber);
      }
    }

    public static void DecryptFile(string inputFile, string outputFile, string keyIV)
    {
      byte[] array1 = new byte[32];
      byte[] array2 = new byte[16];
      AssetStoreUtils.HexStringToByteArray(keyIV, array1, 0);
      AssetStoreUtils.HexStringToByteArray(keyIV, array2, 64);
      EditorUtility.DisplayProgressBar("Decrypting", "Decrypting package", 0.0f);
      FileStream fileStream1 = File.Open(inputFile, System.IO.FileMode.Open);
      FileStream fileStream2 = File.Open(outputFile, System.IO.FileMode.CreateNew);
      long length = fileStream1.Length;
      long num = 0;
      AesManaged aesManaged = new AesManaged();
      aesManaged.Key = array1;
      aesManaged.IV = array2;
      CryptoStream cryptoStream = new CryptoStream((Stream) fileStream1, aesManaged.CreateDecryptor(aesManaged.Key, aesManaged.IV), CryptoStreamMode.Read);
      try
      {
        byte[] numArray = new byte[40960];
        int count;
        while ((count = cryptoStream.Read(numArray, 0, numArray.Length)) > 0)
        {
          fileStream2.Write(numArray, 0, count);
          num += (long) count;
          if (EditorUtility.DisplayCancelableProgressBar("Decrypting", "Decrypting package", (float) num / (float) length))
            throw new Exception("User cancelled decryption");
        }
      }
      finally
      {
        cryptoStream.Close();
        fileStream1.Close();
        fileStream2.Close();
        EditorUtility.ClearProgressBar();
      }
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void RegisterDownloadDelegate(ScriptableObject d);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void UnRegisterDownloadDelegate(ScriptableObject d);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetLoaderPath();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void UpdatePreloading();

    public static string GetOfflinePath()
    {
      return Uri.EscapeUriString(EditorApplication.applicationContentsPath + "/Resources/offline.html");
    }

    public static string GetAssetStoreUrl()
    {
      return "https://shawarma.unity3d.com";
    }

    public static string GetAssetStoreSearchUrl()
    {
      return AssetStoreUtils.GetAssetStoreUrl().Replace("https", "http");
    }

    public delegate void DownloadDoneCallback(string package_id, string message, int bytes, int total);
  }
}
