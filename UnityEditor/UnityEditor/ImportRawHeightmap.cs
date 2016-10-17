// Decompiled with JetBrains decompiler
// Type: UnityEditor.ImportRawHeightmap
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.IO;
using UnityEngine;

namespace UnityEditor
{
  internal class ImportRawHeightmap : TerrainWizard
  {
    public ImportRawHeightmap.Depth m_Depth = ImportRawHeightmap.Depth.Bit16;
    public int m_Width = 1;
    public int m_Height = 1;
    public ImportRawHeightmap.ByteOrder m_ByteOrder = ImportRawHeightmap.ByteOrder.Windows;
    public Vector3 m_TerrainSize = new Vector3(2000f, 600f, 2000f);
    public bool m_FlipVertically;
    private string m_Path;

    private void PickRawDefaults(string path)
    {
      FileStream fileStream = File.Open(path, System.IO.FileMode.Open, FileAccess.Read);
      int length = (int) fileStream.Length;
      fileStream.Close();
      this.m_TerrainSize = this.terrainData.size;
      if (this.terrainData.heightmapWidth * this.terrainData.heightmapHeight == length)
      {
        this.m_Width = this.terrainData.heightmapWidth;
        this.m_Height = this.terrainData.heightmapHeight;
        this.m_Depth = ImportRawHeightmap.Depth.Bit8;
      }
      else if (this.terrainData.heightmapWidth * this.terrainData.heightmapHeight * 2 == length)
      {
        this.m_Width = this.terrainData.heightmapWidth;
        this.m_Height = this.terrainData.heightmapHeight;
        this.m_Depth = ImportRawHeightmap.Depth.Bit16;
      }
      else
      {
        this.m_Depth = ImportRawHeightmap.Depth.Bit16;
        int num1 = length / (int) this.m_Depth;
        int num2 = Mathf.RoundToInt(Mathf.Sqrt((float) num1));
        int num3 = Mathf.RoundToInt(Mathf.Sqrt((float) num1));
        if (num2 * num3 * (int) this.m_Depth == length)
        {
          this.m_Width = num2;
          this.m_Height = num3;
        }
        else
        {
          this.m_Depth = ImportRawHeightmap.Depth.Bit8;
          int num4 = length / (int) this.m_Depth;
          int num5 = Mathf.RoundToInt(Mathf.Sqrt((float) num4));
          int num6 = Mathf.RoundToInt(Mathf.Sqrt((float) num4));
          if (num5 * num6 * (int) this.m_Depth == length)
          {
            this.m_Width = num5;
            this.m_Height = num6;
          }
          else
            this.m_Depth = ImportRawHeightmap.Depth.Bit16;
        }
      }
    }

    internal void OnWizardCreate()
    {
      if ((UnityEngine.Object) this.m_Terrain == (UnityEngine.Object) null)
      {
        this.isValid = false;
        this.errorString = "Terrain does not exist";
      }
      if (this.m_Width > 4097 || this.m_Height > 4097)
      {
        this.isValid = false;
        this.errorString = "Heightmaps above 4097x4097 in resolution are not supported";
        Debug.LogError((object) this.errorString);
      }
      if (!File.Exists(this.m_Path) || !this.isValid)
        return;
      Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this.terrainData, "Import Raw heightmap");
      this.terrainData.heightmapResolution = Mathf.Max(this.m_Width, this.m_Height);
      this.terrainData.size = this.m_TerrainSize;
      this.ReadRaw(this.m_Path);
      this.FlushHeightmapModification();
    }

    private void ReadRaw(string path)
    {
      byte[] numArray;
      using (BinaryReader binaryReader = new BinaryReader((Stream) File.Open(path, System.IO.FileMode.Open, FileAccess.Read)))
      {
        numArray = binaryReader.ReadBytes(this.m_Width * this.m_Height * (int) this.m_Depth);
        binaryReader.Close();
      }
      int heightmapWidth = this.terrainData.heightmapWidth;
      int heightmapHeight = this.terrainData.heightmapHeight;
      float[,] heights = new float[heightmapHeight, heightmapWidth];
      if (this.m_Depth == ImportRawHeightmap.Depth.Bit16)
      {
        float num1 = 1.525879E-05f;
        for (int index1 = 0; index1 < heightmapHeight; ++index1)
        {
          for (int index2 = 0; index2 < heightmapWidth; ++index2)
          {
            int num2 = Mathf.Clamp(index2, 0, this.m_Width - 1) + Mathf.Clamp(index1, 0, this.m_Height - 1) * this.m_Width;
            if (this.m_ByteOrder == ImportRawHeightmap.ByteOrder.Mac == BitConverter.IsLittleEndian)
            {
              byte num3 = numArray[num2 * 2];
              numArray[num2 * 2] = numArray[num2 * 2 + 1];
              numArray[num2 * 2 + 1] = num3;
            }
            float num4 = (float) BitConverter.ToUInt16(numArray, num2 * 2) * num1;
            int index3 = !this.m_FlipVertically ? index1 : heightmapHeight - 1 - index1;
            heights[index3, index2] = num4;
          }
        }
      }
      else
      {
        float num1 = 1f / 256f;
        for (int index1 = 0; index1 < heightmapHeight; ++index1)
        {
          for (int index2 = 0; index2 < heightmapWidth; ++index2)
          {
            int index3 = Mathf.Clamp(index2, 0, this.m_Width - 1) + Mathf.Clamp(index1, 0, this.m_Height - 1) * this.m_Width;
            float num2 = (float) numArray[index3] * num1;
            int index4 = !this.m_FlipVertically ? index1 : heightmapHeight - 1 - index1;
            heights[index4, index2] = num2;
          }
        }
      }
      this.terrainData.SetHeights(0, 0, heights);
    }

    internal void InitializeImportRaw(Terrain terrain, string path)
    {
      this.m_Terrain = terrain;
      this.m_Path = path;
      this.PickRawDefaults(this.m_Path);
      this.helpString = "Raw files must use a single channel and be either 8 or 16 bit.";
      this.OnWizardUpdate();
    }

    internal enum Depth
    {
      Bit8 = 1,
      Bit16 = 2,
    }

    internal enum ByteOrder
    {
      Mac = 1,
      Windows = 2,
    }
  }
}
