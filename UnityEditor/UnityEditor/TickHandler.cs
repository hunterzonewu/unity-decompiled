// Decompiled with JetBrains decompiler
// Type: UnityEditor.TickHandler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class TickHandler
  {
    [SerializeField]
    private float[] m_TickModulos = new float[0];
    [SerializeField]
    private float[] m_TickStrengths = new float[0];
    [SerializeField]
    private int m_BiggestTick = -1;
    [SerializeField]
    private float m_MaxValue = 1f;
    [SerializeField]
    private float m_PixelRange = 1f;
    [SerializeField]
    private int m_SmallestTick;
    [SerializeField]
    private float m_MinValue;

    public int tickLevels
    {
      get
      {
        return this.m_BiggestTick - this.m_SmallestTick + 1;
      }
    }

    public void SetTickModulos(float[] tickModulos)
    {
      this.m_TickModulos = tickModulos;
    }

    public void SetTickModulosForFrameRate(float frameRate)
    {
      if ((double) frameRate != (double) Mathf.Round(frameRate))
      {
        this.SetTickModulos(new float[12]
        {
          (float) (1.0 / (double) frameRate),
          (float) (5.0 / (double) frameRate),
          (float) (10.0 / (double) frameRate),
          (float) (50.0 / (double) frameRate),
          (float) (100.0 / (double) frameRate),
          (float) (500.0 / (double) frameRate),
          (float) (1000.0 / (double) frameRate),
          (float) (5000.0 / (double) frameRate),
          (float) (10000.0 / (double) frameRate),
          (float) (50000.0 / (double) frameRate),
          (float) (100000.0 / (double) frameRate),
          (float) (500000.0 / (double) frameRate)
        });
      }
      else
      {
        List<int> intList = new List<int>();
        int num1 = 1;
        while ((double) num1 < (double) frameRate && (double) num1 != (double) frameRate)
        {
          int num2 = Mathf.RoundToInt(frameRate / (float) num1);
          if (num2 % 60 == 0)
          {
            num1 *= 2;
            intList.Add(num1);
          }
          else if (num2 % 30 == 0)
          {
            num1 *= 3;
            intList.Add(num1);
          }
          else if (num2 % 20 == 0)
          {
            num1 *= 2;
            intList.Add(num1);
          }
          else if (num2 % 10 == 0)
          {
            num1 *= 2;
            intList.Add(num1);
          }
          else if (num2 % 5 == 0)
          {
            num1 *= 5;
            intList.Add(num1);
          }
          else if (num2 % 2 == 0)
          {
            num1 *= 2;
            intList.Add(num1);
          }
          else if (num2 % 3 == 0)
          {
            num1 *= 3;
            intList.Add(num1);
          }
          else
            num1 = Mathf.RoundToInt(frameRate);
        }
        float[] tickModulos = new float[9 + intList.Count];
        for (int index = 0; index < intList.Count; ++index)
          tickModulos[index] = 1f / (float) intList[intList.Count - index - 1];
        tickModulos[tickModulos.Length - 1] = 3600f;
        tickModulos[tickModulos.Length - 2] = 1800f;
        tickModulos[tickModulos.Length - 3] = 600f;
        tickModulos[tickModulos.Length - 4] = 300f;
        tickModulos[tickModulos.Length - 5] = 60f;
        tickModulos[tickModulos.Length - 6] = 30f;
        tickModulos[tickModulos.Length - 7] = 10f;
        tickModulos[tickModulos.Length - 8] = 5f;
        tickModulos[tickModulos.Length - 9] = 1f;
        this.SetTickModulos(tickModulos);
      }
    }

    public void SetRanges(float minValue, float maxValue, float minPixel, float maxPixel)
    {
      this.m_MinValue = minValue;
      this.m_MaxValue = maxValue;
      this.m_PixelRange = maxPixel - minPixel;
    }

    public float[] GetTicksAtLevel(int level, bool excludeTicksFromHigherlevels)
    {
      int index1 = Mathf.Clamp(this.m_SmallestTick + level, 0, this.m_TickModulos.Length - 1);
      List<float> floatList = new List<float>();
      int num1 = Mathf.FloorToInt(this.m_MinValue / this.m_TickModulos[index1]);
      int num2 = Mathf.CeilToInt(this.m_MaxValue / this.m_TickModulos[index1]);
      for (int index2 = num1; index2 <= num2; ++index2)
      {
        if (!excludeTicksFromHigherlevels || index1 >= this.m_BiggestTick || index2 % Mathf.RoundToInt(this.m_TickModulos[index1 + 1] / this.m_TickModulos[index1]) != 0)
          floatList.Add((float) index2 * this.m_TickModulos[index1]);
      }
      return floatList.ToArray();
    }

    public float GetStrengthOfLevel(int level)
    {
      return this.m_TickStrengths[this.m_SmallestTick + level];
    }

    public float GetPeriodOfLevel(int level)
    {
      return this.m_TickModulos[Mathf.Clamp(this.m_SmallestTick + level, 0, this.m_TickModulos.Length - 1)];
    }

    public int GetLevelWithMinSeparation(float pixelSeparation)
    {
      for (int index = 0; index < this.m_TickModulos.Length; ++index)
      {
        if ((double) this.m_TickModulos[index] * (double) this.m_PixelRange / ((double) this.m_MaxValue - (double) this.m_MinValue) >= (double) pixelSeparation)
          return index - this.m_SmallestTick;
      }
      return -1;
    }

    public void SetTickStrengths(float tickMinSpacing, float tickMaxSpacing, bool sqrt)
    {
      this.m_TickStrengths = new float[this.m_TickModulos.Length];
      this.m_SmallestTick = 0;
      this.m_BiggestTick = this.m_TickModulos.Length - 1;
      for (int index = this.m_TickModulos.Length - 1; index >= 0; --index)
      {
        float num = (float) ((double) this.m_TickModulos[index] * (double) this.m_PixelRange / ((double) this.m_MaxValue - (double) this.m_MinValue));
        this.m_TickStrengths[index] = (float) (((double) num - (double) tickMinSpacing) / ((double) tickMaxSpacing - (double) tickMinSpacing));
        if ((double) this.m_TickStrengths[index] >= 1.0)
          this.m_BiggestTick = index;
        if ((double) num <= (double) tickMinSpacing)
        {
          this.m_SmallestTick = index;
          break;
        }
      }
      for (int smallestTick = this.m_SmallestTick; smallestTick <= this.m_BiggestTick; ++smallestTick)
      {
        this.m_TickStrengths[smallestTick] = Mathf.Clamp01(this.m_TickStrengths[smallestTick]);
        if (sqrt)
          this.m_TickStrengths[smallestTick] = Mathf.Sqrt(this.m_TickStrengths[smallestTick]);
      }
    }
  }
}
