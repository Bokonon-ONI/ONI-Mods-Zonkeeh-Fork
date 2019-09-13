﻿// Decompiled with JetBrains decompiler
// Type: ConduitDiseaseManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Klei.AI.DiseaseGrowthRules;
using System;

public class ConduitDiseaseManager : KCompactedVector<ConduitDiseaseManager.Data>
{
  private ConduitTemperatureManager temperatureManager;

  public ConduitDiseaseManager(ConduitTemperatureManager temperature_manager)
    : base(0)
  {
    this.temperatureManager = temperature_manager;
  }

  private static ElemGrowthInfo GetGrowthInfo(byte disease_idx, byte elem_idx)
  {
    return disease_idx == byte.MaxValue ? Klei.AI.Disease.DEFAULT_GROWTH_INFO : Db.Get().Diseases[(int) disease_idx].elemGrowthInfo[(int) elem_idx];
  }

  public HandleVector<int>.Handle Allocate(
    HandleVector<int>.Handle temperature_handle,
    ref ConduitFlow.ConduitContents contents)
  {
    byte elementIndex = (byte) ElementLoader.GetElementIndex(contents.element);
    return this.Allocate(new ConduitDiseaseManager.Data(temperature_handle, elementIndex, contents.mass, contents.diseaseIdx, contents.diseaseCount));
  }

  public void SetData(HandleVector<int>.Handle handle, ref ConduitFlow.ConduitContents contents)
  {
    ConduitDiseaseManager.Data data = this.GetData(handle);
    data.diseaseCount = contents.diseaseCount;
    if ((int) contents.diseaseIdx != (int) data.diseaseIdx)
    {
      data.diseaseIdx = contents.diseaseIdx;
      byte elementIndex = (byte) ElementLoader.GetElementIndex(contents.element);
      data.growthInfo = ConduitDiseaseManager.GetGrowthInfo(contents.diseaseIdx, elementIndex);
    }
    this.SetData(handle, data);
  }

  public void Sim200ms(float dt)
  {
    using (new KProfiler.Region("ConduitDiseaseManager.SimUpdate", (UnityEngine.Object) null))
    {
      for (int index = 0; index < this.data.Count; ++index)
      {
        ConduitDiseaseManager.Data data = this.data[index];
        if (data.diseaseIdx != byte.MaxValue)
        {
          float num1 = data.accumulatedError + data.growthInfo.CalculateDiseaseCountDelta(data.diseaseCount, data.mass, dt);
          Klei.AI.Disease disease = Db.Get().Diseases[(int) data.diseaseIdx];
          float growthRate = Klei.AI.Disease.HalfLifeToGrowthRate(Klei.AI.Disease.CalculateRangeHalfLife(this.temperatureManager.GetTemperature(data.temperatureHandle), ref disease.temperatureRange, ref disease.temperatureHalfLives), dt);
          float num2 = num1 + ((float) data.diseaseCount * growthRate - (float) data.diseaseCount);
          int num3 = (int) num2;
          data.accumulatedError = num2 - (float) num3;
          data.diseaseCount += num3;
          if (data.diseaseCount <= 0)
          {
            data.diseaseCount = 0;
            data.diseaseIdx = byte.MaxValue;
            data.accumulatedError = 0.0f;
          }
          this.data[index] = data;
        }
      }
    }
  }

  public void ModifyDiseaseCount(HandleVector<int>.Handle h, int disease_count_delta)
  {
    ConduitDiseaseManager.Data data = this.GetData(h);
    data.diseaseCount = Math.Max(0, data.diseaseCount + disease_count_delta);
    if (data.diseaseCount == 0)
      data.diseaseIdx = byte.MaxValue;
    this.SetData(h, data);
  }

  public void AddDisease(HandleVector<int>.Handle h, byte disease_idx, int disease_count)
  {
    ConduitDiseaseManager.Data data = this.GetData(h);
    SimUtil.DiseaseInfo finalDiseaseInfo = SimUtil.CalculateFinalDiseaseInfo(disease_idx, disease_count, data.diseaseIdx, data.diseaseCount);
    data.diseaseIdx = finalDiseaseInfo.idx;
    data.diseaseCount = finalDiseaseInfo.count;
    this.SetData(h, data);
  }

  public struct Data
  {
    public byte diseaseIdx;
    public byte elemIdx;
    public int diseaseCount;
    public float accumulatedError;
    public float mass;
    public HandleVector<int>.Handle temperatureHandle;
    public ElemGrowthInfo growthInfo;

    public Data(
      HandleVector<int>.Handle temperature_handle,
      byte elem_idx,
      float mass,
      byte disease_idx,
      int disease_count)
    {
      this.diseaseIdx = disease_idx;
      this.elemIdx = elem_idx;
      this.mass = mass;
      this.diseaseCount = disease_count;
      this.accumulatedError = 0.0f;
      this.temperatureHandle = temperature_handle;
      this.growthInfo = ConduitDiseaseManager.GetGrowthInfo(disease_idx, elem_idx);
    }
  }
}