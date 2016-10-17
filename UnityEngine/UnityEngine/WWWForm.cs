// Decompiled with JetBrains decompiler
// Type: UnityEngine.WWWForm
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.Internal;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Helper class to generate form data to post to web servers using the WWW class.</para>
  /// </summary>
  public sealed class WWWForm
  {
    private List<byte[]> formData;
    private List<string> fieldNames;
    private List<string> fileNames;
    private List<string> types;
    private byte[] boundary;
    private bool containsFiles;

    /// <summary>
    ///   <para>(Read Only) Returns the correct request headers for posting the form using the WWW class.</para>
    /// </summary>
    public Dictionary<string, string> headers
    {
      get
      {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Content-Type"] = !this.containsFiles ? "application/x-www-form-urlencoded" : "multipart/form-data; boundary=\"" + Encoding.UTF8.GetString(this.boundary, 0, this.boundary.Length) + "\"";
        return dictionary;
      }
    }

    /// <summary>
    ///   <para>(Read Only) The raw data to pass as the POST request body when sending the form.</para>
    /// </summary>
    public byte[] data
    {
      get
      {
        if (this.containsFiles)
        {
          byte[] bytes1 = WWW.DefaultEncoding.GetBytes("--");
          byte[] bytes2 = WWW.DefaultEncoding.GetBytes("\r\n");
          byte[] bytes3 = WWW.DefaultEncoding.GetBytes("Content-Type: ");
          byte[] bytes4 = WWW.DefaultEncoding.GetBytes("Content-disposition: form-data; name=\"");
          byte[] bytes5 = WWW.DefaultEncoding.GetBytes("\"");
          byte[] bytes6 = WWW.DefaultEncoding.GetBytes("; filename=\"");
          using (MemoryStream memoryStream = new MemoryStream(1024))
          {
            for (int index = 0; index < this.formData.Count; ++index)
            {
              memoryStream.Write(bytes2, 0, bytes2.Length);
              memoryStream.Write(bytes1, 0, bytes1.Length);
              memoryStream.Write(this.boundary, 0, this.boundary.Length);
              memoryStream.Write(bytes2, 0, bytes2.Length);
              memoryStream.Write(bytes3, 0, bytes3.Length);
              byte[] bytes7 = Encoding.UTF8.GetBytes(this.types[index]);
              memoryStream.Write(bytes7, 0, bytes7.Length);
              memoryStream.Write(bytes2, 0, bytes2.Length);
              memoryStream.Write(bytes4, 0, bytes4.Length);
              string empty = string.Empty;
              string str1 = this.fieldNames[index];
              if (!WWWTranscoder.SevenBitClean(str1, Encoding.UTF8) || str1.IndexOf("=?") > -1)
                str1 = "=?" + empty + "?Q?" + WWWTranscoder.QPEncode(str1, Encoding.UTF8) + "?=";
              byte[] bytes8 = Encoding.UTF8.GetBytes(str1);
              memoryStream.Write(bytes8, 0, bytes8.Length);
              memoryStream.Write(bytes5, 0, bytes5.Length);
              if (this.fileNames[index] != null)
              {
                string str2 = this.fileNames[index];
                if (!WWWTranscoder.SevenBitClean(str2, Encoding.UTF8) || str2.IndexOf("=?") > -1)
                  str2 = "=?" + empty + "?Q?" + WWWTranscoder.QPEncode(str2, Encoding.UTF8) + "?=";
                byte[] bytes9 = Encoding.UTF8.GetBytes(str2);
                memoryStream.Write(bytes6, 0, bytes6.Length);
                memoryStream.Write(bytes9, 0, bytes9.Length);
                memoryStream.Write(bytes5, 0, bytes5.Length);
              }
              memoryStream.Write(bytes2, 0, bytes2.Length);
              memoryStream.Write(bytes2, 0, bytes2.Length);
              byte[] buffer = this.formData[index];
              memoryStream.Write(buffer, 0, buffer.Length);
            }
            memoryStream.Write(bytes2, 0, bytes2.Length);
            memoryStream.Write(bytes1, 0, bytes1.Length);
            memoryStream.Write(this.boundary, 0, this.boundary.Length);
            memoryStream.Write(bytes1, 0, bytes1.Length);
            memoryStream.Write(bytes2, 0, bytes2.Length);
            return memoryStream.ToArray();
          }
        }
        else
        {
          byte[] bytes1 = WWW.DefaultEncoding.GetBytes("&");
          byte[] bytes2 = WWW.DefaultEncoding.GetBytes("=");
          using (MemoryStream memoryStream = new MemoryStream(1024))
          {
            for (int index = 0; index < this.formData.Count; ++index)
            {
              byte[] buffer1 = WWWTranscoder.URLEncode(Encoding.UTF8.GetBytes(this.fieldNames[index]));
              byte[] buffer2 = WWWTranscoder.URLEncode(this.formData[index]);
              if (index > 0)
                memoryStream.Write(bytes1, 0, bytes1.Length);
              memoryStream.Write(buffer1, 0, buffer1.Length);
              memoryStream.Write(bytes2, 0, bytes2.Length);
              memoryStream.Write(buffer2, 0, buffer2.Length);
            }
            return memoryStream.ToArray();
          }
        }
      }
    }

    /// <summary>
    ///   <para>Creates an empty WWWForm object.</para>
    /// </summary>
    public WWWForm()
    {
      this.formData = new List<byte[]>();
      this.fieldNames = new List<string>();
      this.fileNames = new List<string>();
      this.types = new List<string>();
      this.boundary = new byte[40];
      for (int index = 0; index < 40; ++index)
      {
        int num = Random.Range(48, 110);
        if (num > 57)
          num += 7;
        if (num > 90)
          num += 6;
        this.boundary[index] = (byte) num;
      }
    }

    /// <summary>
    ///   <para>Add a simple field to the form.</para>
    /// </summary>
    /// <param name="fieldName"></param>
    /// <param name="value"></param>
    /// <param name="e"></param>
    [ExcludeFromDocs]
    public void AddField(string fieldName, string value)
    {
      Encoding utF8 = Encoding.UTF8;
      this.AddField(fieldName, value, utF8);
    }

    /// <summary>
    ///   <para>Add a simple field to the form.</para>
    /// </summary>
    /// <param name="fieldName"></param>
    /// <param name="value"></param>
    /// <param name="e"></param>
    public void AddField(string fieldName, string value, [DefaultValue("System.Text.Encoding.UTF8")] Encoding e)
    {
      this.fieldNames.Add(fieldName);
      this.fileNames.Add((string) null);
      this.formData.Add(e.GetBytes(value));
      this.types.Add("text/plain; charset=\"" + e.WebName + "\"");
    }

    /// <summary>
    ///   <para>Adds a simple field to the form.</para>
    /// </summary>
    /// <param name="fieldName"></param>
    /// <param name="i"></param>
    public void AddField(string fieldName, int i)
    {
      this.AddField(fieldName, i.ToString());
    }

    /// <summary>
    ///   <para>Add binary data to the form.</para>
    /// </summary>
    /// <param name="fieldName"></param>
    /// <param name="contents"></param>
    /// <param name="fileName"></param>
    /// <param name="mimeType"></param>
    [ExcludeFromDocs]
    public void AddBinaryData(string fieldName, byte[] contents, string fileName)
    {
      string mimeType = (string) null;
      this.AddBinaryData(fieldName, contents, fileName, mimeType);
    }

    /// <summary>
    ///   <para>Add binary data to the form.</para>
    /// </summary>
    /// <param name="fieldName"></param>
    /// <param name="contents"></param>
    /// <param name="fileName"></param>
    /// <param name="mimeType"></param>
    [ExcludeFromDocs]
    public void AddBinaryData(string fieldName, byte[] contents)
    {
      string mimeType = (string) null;
      string fileName = (string) null;
      this.AddBinaryData(fieldName, contents, fileName, mimeType);
    }

    /// <summary>
    ///   <para>Add binary data to the form.</para>
    /// </summary>
    /// <param name="fieldName"></param>
    /// <param name="contents"></param>
    /// <param name="fileName"></param>
    /// <param name="mimeType"></param>
    public void AddBinaryData(string fieldName, byte[] contents, [DefaultValue("null")] string fileName, [DefaultValue("null")] string mimeType)
    {
      this.containsFiles = true;
      bool flag = contents.Length > 8 && (int) contents[0] == 137 && ((int) contents[1] == 80 && (int) contents[2] == 78) && ((int) contents[3] == 71 && (int) contents[4] == 13 && ((int) contents[5] == 10 && (int) contents[6] == 26)) && (int) contents[7] == 10;
      if (fileName == null)
        fileName = fieldName + (!flag ? ".dat" : ".png");
      if (mimeType == null)
        mimeType = !flag ? "application/octet-stream" : "image/png";
      this.fieldNames.Add(fieldName);
      this.fileNames.Add(fileName);
      this.formData.Add(contents);
      this.types.Add(mimeType);
    }
  }
}
