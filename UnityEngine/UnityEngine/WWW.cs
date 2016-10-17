// Decompiled with JetBrains decompiler
// Type: UnityEngine.WWW
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Simple access to web pages.</para>
  /// </summary>
  [UsedByNativeCode]
  public sealed class WWW : IDisposable
  {
    private static readonly char[] forbiddenCharacters = new char[33]{ char.MinValue, '\x0001', '\x0002', '\x0003', '\x0004', '\x0005', '\x0006', '\a', '\b', '\t', '\n', '\v', '\f', '\r', '\x000E', '\x000F', '\x0010', '\x0011', '\x0012', '\x0013', '\x0014', '\x0015', '\x0016', '\x0017', '\x0018', '\x0019', '\x001A', '\x001B', '\x001C', '\x001D', '\x001E', '\x001F', '\x007F' };
    private static readonly char[] forbiddenCharactersForNames = new char[1]{ ' ' };
    private static readonly string[] forbiddenHeaderKeys = new string[22]{ "Accept-Charset", "Accept-Encoding", "Access-Control-Request-Headers", "Access-Control-Request-Method", "Connection", "Content-Length", "Cookie", "Cookie2", "Date", "DNT", "Expect", "Host", "Keep-Alive", "Origin", "Referer", "TE", "Trailer", "Transfer-Encoding", "Upgrade", "User-Agent", "Via", "X-Unity-Version" };
    internal IntPtr m_Ptr;

    /// <summary>
    ///   <para>Dictionary of headers returned by the request.</para>
    /// </summary>
    public Dictionary<string, string> responseHeaders
    {
      get
      {
        if (!this.isDone)
          throw new UnityException("WWW is not finished downloading yet");
        return WWW.ParseHTTPHeaderString(this.responseHeadersString);
      }
    }

    private string responseHeadersString { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns the contents of the fetched web page as a string (Read Only).</para>
    /// </summary>
    public string text
    {
      get
      {
        if (!this.isDone)
          throw new UnityException("WWW is not ready downloading yet");
        byte[] bytes = this.bytes;
        return this.GetTextEncoder().GetString(bytes, 0, bytes.Length);
      }
    }

    internal static Encoding DefaultEncoding
    {
      get
      {
        return Encoding.ASCII;
      }
    }

    [Obsolete("Please use WWW.text instead")]
    public string data
    {
      get
      {
        return this.text;
      }
    }

    /// <summary>
    ///   <para>Returns the contents of the fetched web page as a byte array (Read Only).</para>
    /// </summary>
    public byte[] bytes { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public int size { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns an error message if there was an error during the download (Read Only).</para>
    /// </summary>
    public string error { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns a Texture2D generated from the downloaded data (Read Only).</para>
    /// </summary>
    public Texture2D texture
    {
      get
      {
        return this.GetTexture(false);
      }
    }

    /// <summary>
    ///   <para>Returns a non-readable Texture2D generated from the downloaded data (Read Only).</para>
    /// </summary>
    public Texture2D textureNonReadable
    {
      get
      {
        return this.GetTexture(true);
      }
    }

    /// <summary>
    ///   <para>Returns a AudioClip generated from the downloaded data (Read Only).</para>
    /// </summary>
    public AudioClip audioClip
    {
      get
      {
        return this.GetAudioClip(true);
      }
    }

    /// <summary>
    ///   <para>Returns a MovieTexture generated from the downloaded data (Read Only).</para>
    /// </summary>
    public MovieTexture movie { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Is the download already finished? (Read Only)</para>
    /// </summary>
    public bool isDone { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>How far has the download progressed (Read Only).</para>
    /// </summary>
    public float progress { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>How far has the upload progressed (Read Only).</para>
    /// </summary>
    public float uploadProgress { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The number of bytes downloaded by this WWW query (read only).</para>
    /// </summary>
    public int bytesDownloaded { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Load an Ogg Vorbis file into the audio clip.</para>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Property WWW.oggVorbis has been deprecated. Use WWW.audioClip instead (UnityUpgradable).", true)]
    public AudioClip oggVorbis
    {
      get
      {
        return (AudioClip) null;
      }
    }

    /// <summary>
    ///   <para>The URL of this WWW request (Read Only).</para>
    /// </summary>
    public string url { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Streams an AssetBundle that can contain any kind of asset from the project folder.</para>
    /// </summary>
    public AssetBundle assetBundle { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Priority of AssetBundle decompression thread.</para>
    /// </summary>
    public ThreadPriority threadPriority { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Creates a WWW request with the given URL.</para>
    /// </summary>
    /// <param name="url">The url to download. Must be '%' escaped.</param>
    /// <returns>
    ///   <para>A new WWW object. When it has been downloaded, the results can be fetched from the returned object.</para>
    /// </returns>
    public WWW(string url)
    {
      this.InitWWW(url, (byte[]) null, (string[]) null);
    }

    /// <summary>
    ///   <para>Creates a WWW request with the given URL.</para>
    /// </summary>
    /// <param name="url">The url to download. Must be '%' escaped.</param>
    /// <param name="form">A WWWForm instance containing the form data to post.</param>
    /// <returns>
    ///   <para>A new WWW object. When it has been downloaded, the results can be fetched from the returned object.</para>
    /// </returns>
    public WWW(string url, WWWForm form)
    {
      string[] strArray = WWW.FlattenedHeadersFrom(form.headers);
      if (this.enforceWebSecurityRestrictions())
        WWW.CheckSecurityOnHeaders(strArray);
      this.InitWWW(url, form.data, strArray);
    }

    /// <summary>
    ///   <para>Creates a WWW request with the given URL.</para>
    /// </summary>
    /// <param name="url">The url to download. Must be '%' escaped.</param>
    /// <param name="postData">A byte array of data to be posted to the url.</param>
    /// <returns>
    ///   <para>A new WWW object. When it has been downloaded, the results can be fetched from the returned object.</para>
    /// </returns>
    public WWW(string url, byte[] postData)
    {
      this.InitWWW(url, postData, (string[]) null);
    }

    /// <summary>
    ///   <para>Creates a WWW request with the given URL.</para>
    /// </summary>
    /// <param name="url">The url to download. Must be '%' escaped.</param>
    /// <param name="postData">A byte array of data to be posted to the url.</param>
    /// <param name="headers">A hash table of custom headers to send with the request.</param>
    /// <returns>
    ///   <para>A new WWW object. When it has been downloaded, the results can be fetched from the returned object.</para>
    /// </returns>
    [Obsolete("This overload is deprecated. Use UnityEngine.WWW.WWW(string, byte[], System.Collections.Generic.Dictionary<string, string>) instead.", true)]
    public WWW(string url, byte[] postData, Hashtable headers)
    {
      Debug.LogError((object) "This overload is deprecated. Use UnityEngine.WWW.WWW(string, byte[], System.Collections.Generic.Dictionary<string, string>) instead");
    }

    public WWW(string url, byte[] postData, Dictionary<string, string> headers)
    {
      string[] strArray = WWW.FlattenedHeadersFrom(headers);
      if (this.enforceWebSecurityRestrictions())
        WWW.CheckSecurityOnHeaders(strArray);
      this.InitWWW(url, postData, strArray);
    }

    internal WWW(string url, Hash128 hash, uint crc)
    {
      WWW.INTERNAL_CALL_WWW(this, url, ref hash, crc);
    }

    ~WWW()
    {
      this.DestroyWWW(false);
    }

    /// <summary>
    ///   <para>Disposes of an existing WWW object.</para>
    /// </summary>
    public void Dispose()
    {
      this.DestroyWWW(true);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void DestroyWWW(bool cancel);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InitWWW(string url, byte[] postData, string[] iHeaders);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal bool enforceWebSecurityRestrictions();

    /// <summary>
    ///   <para>Escapes characters in a string to ensure they are URL-friendly.</para>
    /// </summary>
    /// <param name="s">A string with characters to be escaped.</param>
    /// <param name="e">The text encoding to use.</param>
    [ExcludeFromDocs]
    public static string EscapeURL(string s)
    {
      Encoding utF8 = Encoding.UTF8;
      return WWW.EscapeURL(s, utF8);
    }

    /// <summary>
    ///   <para>Escapes characters in a string to ensure they are URL-friendly.</para>
    /// </summary>
    /// <param name="s">A string with characters to be escaped.</param>
    /// <param name="e">The text encoding to use.</param>
    public static string EscapeURL(string s, [DefaultValue("System.Text.Encoding.UTF8")] Encoding e)
    {
      if (s == null)
        return (string) null;
      if (s == string.Empty)
        return string.Empty;
      if (e == null)
        return (string) null;
      return WWWTranscoder.URLEncode(s, e);
    }

    /// <summary>
    ///   <para>Converts URL-friendly escape sequences back to normal text.</para>
    /// </summary>
    /// <param name="s">A string containing escaped characters.</param>
    /// <param name="e">The text encoding to use.</param>
    [ExcludeFromDocs]
    public static string UnEscapeURL(string s)
    {
      Encoding utF8 = Encoding.UTF8;
      return WWW.UnEscapeURL(s, utF8);
    }

    /// <summary>
    ///   <para>Converts URL-friendly escape sequences back to normal text.</para>
    /// </summary>
    /// <param name="s">A string containing escaped characters.</param>
    /// <param name="e">The text encoding to use.</param>
    public static string UnEscapeURL(string s, [DefaultValue("System.Text.Encoding.UTF8")] Encoding e)
    {
      if (s == null)
        return (string) null;
      if (s.IndexOf('%') == -1 && s.IndexOf('+') == -1)
        return s;
      return WWWTranscoder.URLDecode(s, e);
    }

    private Encoding GetTextEncoder()
    {
      string str = (string) null;
      if (this.responseHeaders.TryGetValue("CONTENT-TYPE", out str))
      {
        int startIndex = str.IndexOf("charset", StringComparison.OrdinalIgnoreCase);
        if (startIndex > -1)
        {
          int num = str.IndexOf('=', startIndex);
          if (num > -1)
          {
            string name = str.Substring(num + 1).Trim().Trim('\'', '"').Trim();
            int length = name.IndexOf(';');
            if (length > -1)
              name = name.Substring(0, length);
            try
            {
              return Encoding.GetEncoding(name);
            }
            catch (Exception ex)
            {
              Debug.Log((object) ("Unsupported encoding: '" + name + "'"));
            }
          }
        }
      }
      return Encoding.UTF8;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private Texture2D GetTexture(bool markNonReadable);

    /// <summary>
    ///   <para>Returns an AudioClip generated from the downloaded data (Read Only).</para>
    /// </summary>
    /// <param name="threeD">Use this to specify whether the clip should be a 2D or 3D clip
    /// the .audioClip property defaults to 3D.</param>
    /// <param name="stream">Sets whether the clip should be completely downloaded before it's ready to play (false) or the stream can be played even if only part of the clip is downloaded (true).
    /// The latter will disable seeking on the clip (with .time and/or .timeSamples).</param>
    /// <param name="audioType">The AudioType of the content your downloading. If this is not set Unity will try to determine the type from URL.</param>
    /// <returns>
    ///   <para>The returned AudioClip.</para>
    /// </returns>
    public AudioClip GetAudioClip(bool threeD)
    {
      return this.GetAudioClip(threeD, false);
    }

    /// <summary>
    ///   <para>Returns an AudioClip generated from the downloaded data (Read Only).</para>
    /// </summary>
    /// <param name="threeD">Use this to specify whether the clip should be a 2D or 3D clip
    /// the .audioClip property defaults to 3D.</param>
    /// <param name="stream">Sets whether the clip should be completely downloaded before it's ready to play (false) or the stream can be played even if only part of the clip is downloaded (true).
    /// The latter will disable seeking on the clip (with .time and/or .timeSamples).</param>
    /// <param name="audioType">The AudioType of the content your downloading. If this is not set Unity will try to determine the type from URL.</param>
    /// <returns>
    ///   <para>The returned AudioClip.</para>
    /// </returns>
    public AudioClip GetAudioClip(bool threeD, bool stream)
    {
      return this.GetAudioClip(threeD, stream, AudioType.UNKNOWN);
    }

    /// <summary>
    ///   <para>Returns an AudioClip generated from the downloaded data (Read Only).</para>
    /// </summary>
    /// <param name="threeD">Use this to specify whether the clip should be a 2D or 3D clip
    /// the .audioClip property defaults to 3D.</param>
    /// <param name="stream">Sets whether the clip should be completely downloaded before it's ready to play (false) or the stream can be played even if only part of the clip is downloaded (true).
    /// The latter will disable seeking on the clip (with .time and/or .timeSamples).</param>
    /// <param name="audioType">The AudioType of the content your downloading. If this is not set Unity will try to determine the type from URL.</param>
    /// <returns>
    ///   <para>The returned AudioClip.</para>
    /// </returns>
    public AudioClip GetAudioClip(bool threeD, bool stream, AudioType audioType)
    {
      return this.GetAudioClipInternal(threeD, stream, false, audioType);
    }

    /// <summary>
    ///   <para>Returns an AudioClip generated from the downloaded data that is compressed in memory (Read Only).</para>
    /// </summary>
    /// <param name="threeD">Use this to specify whether the clip should be a 2D or 3D clip.</param>
    /// <param name="audioType">The AudioType of the content your downloading. If this is not set Unity will try to determine the type from URL.</param>
    /// <returns>
    ///   <para>The returned AudioClip.</para>
    /// </returns>
    public AudioClip GetAudioClipCompressed()
    {
      return this.GetAudioClipCompressed(true);
    }

    /// <summary>
    ///   <para>Returns an AudioClip generated from the downloaded data that is compressed in memory (Read Only).</para>
    /// </summary>
    /// <param name="threeD">Use this to specify whether the clip should be a 2D or 3D clip.</param>
    /// <param name="audioType">The AudioType of the content your downloading. If this is not set Unity will try to determine the type from URL.</param>
    /// <returns>
    ///   <para>The returned AudioClip.</para>
    /// </returns>
    public AudioClip GetAudioClipCompressed(bool threeD)
    {
      return this.GetAudioClipCompressed(threeD, AudioType.UNKNOWN);
    }

    /// <summary>
    ///   <para>Returns an AudioClip generated from the downloaded data that is compressed in memory (Read Only).</para>
    /// </summary>
    /// <param name="threeD">Use this to specify whether the clip should be a 2D or 3D clip.</param>
    /// <param name="audioType">The AudioType of the content your downloading. If this is not set Unity will try to determine the type from URL.</param>
    /// <returns>
    ///   <para>The returned AudioClip.</para>
    /// </returns>
    public AudioClip GetAudioClipCompressed(bool threeD, AudioType audioType)
    {
      return this.GetAudioClipInternal(threeD, false, true, audioType);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private AudioClip GetAudioClipInternal(bool threeD, bool stream, bool compressed, AudioType audioType);

    /// <summary>
    ///   <para>Replaces the contents of an existing Texture2D with an image from the downloaded data.</para>
    /// </summary>
    /// <param name="tex">An existing texture object to be overwritten with the image data.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void LoadImageIntoTexture(Texture2D tex);

    [WrapperlessIcall]
    [Obsolete("All blocking WWW functions have been deprecated, please use one of the asynchronous functions instead.", true)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetURL(string url);

    [Obsolete("All blocking WWW functions have been deprecated, please use one of the asynchronous functions instead.", true)]
    public static Texture2D GetTextureFromURL(string url)
    {
      return new WWW(url).texture;
    }

    /// <summary>
    ///   <para>Loads the new web player data file.</para>
    /// </summary>
    [Obsolete("LoadUnityWeb is no longer supported. Please use javascript to reload the web player on a different url instead", true)]
    public void LoadUnityWeb()
    {
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_WWW(WWW self, string url, ref Hash128 hash, uint crc);

    /// <summary>
    ///   <para>Loads an AssetBundle with the specified version number from the cache. If the AssetBundle is not currently cached, it will automatically be downloaded and stored in the cache for future retrieval from local storage.</para>
    /// </summary>
    /// <param name="url">The URL to download the AssetBundle from, if it is not present in the cache. Must be '%' escaped.</param>
    /// <param name="version">Version of the AssetBundle. The file will only be loaded from the disk cache if it has previously been downloaded with the same version parameter. By incrementing the version number requested by your application, you can force Caching to download a new copy of the AssetBundle from url.</param>
    /// <param name="crc">An optional CRC-32 Checksum of the uncompressed contents. If this is non-zero, then the content will be compared against the checksum before loading it, and give an error if it does not match. You can use this to avoid data corruption from bad downloads or users tampering with the cached files on disk. If the CRC does not match, Unity will try to redownload the data, and if the CRC on the server does not match it will fail with an error. Look at the error string returned to see the correct CRC value to use for an AssetBundle.</param>
    /// <returns>
    ///   <para>A WWW instance, which can be used to access the data once the load/download operation is completed.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static WWW LoadFromCacheOrDownload(string url, int version)
    {
      uint crc = 0;
      return WWW.LoadFromCacheOrDownload(url, version, crc);
    }

    /// <summary>
    ///   <para>Loads an AssetBundle with the specified version number from the cache. If the AssetBundle is not currently cached, it will automatically be downloaded and stored in the cache for future retrieval from local storage.</para>
    /// </summary>
    /// <param name="url">The URL to download the AssetBundle from, if it is not present in the cache. Must be '%' escaped.</param>
    /// <param name="version">Version of the AssetBundle. The file will only be loaded from the disk cache if it has previously been downloaded with the same version parameter. By incrementing the version number requested by your application, you can force Caching to download a new copy of the AssetBundle from url.</param>
    /// <param name="crc">An optional CRC-32 Checksum of the uncompressed contents. If this is non-zero, then the content will be compared against the checksum before loading it, and give an error if it does not match. You can use this to avoid data corruption from bad downloads or users tampering with the cached files on disk. If the CRC does not match, Unity will try to redownload the data, and if the CRC on the server does not match it will fail with an error. Look at the error string returned to see the correct CRC value to use for an AssetBundle.</param>
    /// <returns>
    ///   <para>A WWW instance, which can be used to access the data once the load/download operation is completed.</para>
    /// </returns>
    public static WWW LoadFromCacheOrDownload(string url, int version, [DefaultValue("0")] uint crc)
    {
      Hash128 hash = new Hash128(0U, 0U, 0U, (uint) version);
      return WWW.LoadFromCacheOrDownload(url, hash, crc);
    }

    [ExcludeFromDocs]
    public static WWW LoadFromCacheOrDownload(string url, Hash128 hash)
    {
      uint crc = 0;
      return WWW.LoadFromCacheOrDownload(url, hash, crc);
    }

    public static WWW LoadFromCacheOrDownload(string url, Hash128 hash, [DefaultValue("0")] uint crc)
    {
      return new WWW(url, hash, crc);
    }

    private static void CheckSecurityOnHeaders(string[] headers)
    {
      bool flag = Application.GetBuildUnityVersion() >= Application.GetNumericUnityVersion("4.3.0b1");
      int index = 0;
      while (index < headers.Length)
      {
        foreach (string forbiddenHeaderKey in WWW.forbiddenHeaderKeys)
        {
          if (string.Equals(headers[index], forbiddenHeaderKey, StringComparison.CurrentCultureIgnoreCase))
          {
            if (flag)
              throw new ArgumentException("Cannot overwrite header: " + headers[index]);
            Debug.LogError((object) ("Illegal header overwrite, this will fail in 4.3 and above: " + headers[index]));
          }
        }
        if (headers[index].StartsWith("Sec-") || headers[index].StartsWith("Proxy-"))
        {
          if (flag)
            throw new ArgumentException("Cannot overwrite header: " + headers[index]);
          Debug.LogError((object) ("Illegal header overwrite, this will fail in 4.3 and above: " + headers[index]));
        }
        if (headers[index].IndexOfAny(WWW.forbiddenCharacters) > -1 || headers[index].IndexOfAny(WWW.forbiddenCharactersForNames) > -1 || headers[index + 1].IndexOfAny(WWW.forbiddenCharacters) > -1)
        {
          if (flag)
            throw new ArgumentException("Cannot include control characters in a HTTP header, either as key or value.");
          Debug.LogError((object) "Illegal control characters in header, this will fail in 4.3 and above");
        }
        index += 2;
      }
    }

    private static string[] FlattenedHeadersFrom(Dictionary<string, string> headers)
    {
      if (headers == null)
        return (string[]) null;
      string[] strArray1 = new string[headers.Count * 2];
      int num1 = 0;
      using (Dictionary<string, string>.Enumerator enumerator = headers.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<string, string> current = enumerator.Current;
          string[] strArray2 = strArray1;
          int index1 = num1;
          int num2 = 1;
          int num3 = index1 + num2;
          string str1 = current.Key.ToString();
          strArray2[index1] = str1;
          string[] strArray3 = strArray1;
          int index2 = num3;
          int num4 = 1;
          num1 = index2 + num4;
          string str2 = current.Value.ToString();
          strArray3[index2] = str2;
        }
      }
      return strArray1;
    }

    internal static Dictionary<string, string> ParseHTTPHeaderString(string input)
    {
      if (input == null)
        throw new ArgumentException("input was null to ParseHTTPHeaderString");
      Dictionary<string, string> dictionary = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      StringReader stringReader = new StringReader(input);
      int num = 0;
      while (true)
      {
        string str1;
        int length;
        do
        {
          str1 = stringReader.ReadLine();
          if (str1 != null)
          {
            if (num++ == 0 && str1.StartsWith("HTTP"))
              dictionary["STATUS"] = str1;
            else
              length = str1.IndexOf(": ");
          }
          else
            goto label_8;
        }
        while (length == -1);
        string upper = str1.Substring(0, length).ToUpper();
        string str2 = str1.Substring(length + 2);
        dictionary[upper] = str2;
      }
label_8:
      return dictionary;
    }
  }
}
