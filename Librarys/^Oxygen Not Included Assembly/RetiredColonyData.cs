﻿// Decompiled with JetBrains decompiler
// Type: RetiredColonyData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RetiredColonyData
{
  public RetiredColonyData()
  {
  }

  public RetiredColonyData(
    string colonyName,
    int cycleCount,
    string date,
    string[] achievements,
    MinionAssignablesProxy[] minions,
    BuildingComplete[] buildingCompletes)
  {
    this.colonyName = colonyName;
    this.cycleCount = cycleCount;
    this.achievements = achievements;
    this.date = date;
    this.Duplicants = new RetiredColonyData.RetiredDuplicantData[minions != null ? minions.Length : 0];
    for (int index = 0; index < this.Duplicants.Length; ++index)
    {
      this.Duplicants[index] = new RetiredColonyData.RetiredDuplicantData();
      this.Duplicants[index].name = minions[index].GetProperName();
      this.Duplicants[index].age = (int) Mathf.Floor((float) GameClock.Instance.GetCycle() - minions[index].GetArrivalTime());
      this.Duplicants[index].skillPointsGained = minions[index].GetTotalSkillpoints();
      this.Duplicants[index].accessories = new Dictionary<string, string>();
      if ((UnityEngine.Object) minions[index].GetTargetGameObject().GetComponent<Accessorizer>() != (UnityEngine.Object) null)
      {
        foreach (ResourceRef<Accessory> accessory in minions[index].GetTargetGameObject().GetComponent<Accessorizer>().GetAccessories())
        {
          if (accessory.Get() != null)
            this.Duplicants[index].accessories.Add(accessory.Get().slot.Id, accessory.Get().Id);
        }
      }
      else
      {
        StoredMinionIdentity component = minions[index].GetTargetGameObject().GetComponent<StoredMinionIdentity>();
        this.Duplicants[index].accessories.Add(Db.Get().AccessorySlots.Eyes.Id, Db.Get().Accessories.Get(component.bodyData.eyes).Id);
        this.Duplicants[index].accessories.Add(Db.Get().AccessorySlots.Arm.Id, Db.Get().Accessories.Get(component.bodyData.arms).Id);
        this.Duplicants[index].accessories.Add(Db.Get().AccessorySlots.Body.Id, Db.Get().Accessories.Get(component.bodyData.body).Id);
        this.Duplicants[index].accessories.Add(Db.Get().AccessorySlots.Hair.Id, Db.Get().Accessories.Get(component.bodyData.hair).Id);
        if (component.bodyData.hat != HashedString.Invalid)
          this.Duplicants[index].accessories.Add(Db.Get().AccessorySlots.Hat.Id, Db.Get().Accessories.Get(component.bodyData.hat).Id);
        this.Duplicants[index].accessories.Add(Db.Get().AccessorySlots.HeadShape.Id, Db.Get().Accessories.Get(component.bodyData.headShape).Id);
        this.Duplicants[index].accessories.Add(Db.Get().AccessorySlots.Mouth.Id, Db.Get().Accessories.Get(component.bodyData.mouth).Id);
      }
    }
    this.buildings = new List<Tuple<string, int>>();
    if (buildingCompletes != null)
    {
      foreach (BuildingComplete buildingComplete in buildingCompletes)
      {
        BuildingComplete b = buildingComplete;
        int index = this.buildings.FindIndex((Predicate<Tuple<string, int>>) (match => (Tag) match.first == b.PrefabID()));
        if (index == -1)
        {
          this.buildings.Add(new Tuple<string, int>(b.PrefabID().ToString(), 0));
          index = this.buildings.Count - 1;
        }
        ++this.buildings[index].second;
      }
    }
    this.Stats = (RetiredColonyData.RetiredColonyStatistic[]) null;
    if (!((UnityEngine.Object) ReportManager.Instance != (UnityEngine.Object) null))
      return;
    Tuple<float, float>[] data1 = new Tuple<float, float>[ReportManager.Instance.reports.Count];
    for (int index = 0; index < data1.Length; ++index)
      data1[index] = new Tuple<float, float>((float) ReportManager.Instance.reports[index].day, ReportManager.Instance.reports[index].GetEntry(ReportManager.ReportType.OxygenCreated).accPositive);
    Tuple<float, float>[] data2 = new Tuple<float, float>[ReportManager.Instance.reports.Count];
    for (int index = 0; index < data2.Length; ++index)
      data2[index] = new Tuple<float, float>((float) ReportManager.Instance.reports[index].day, ReportManager.Instance.reports[index].GetEntry(ReportManager.ReportType.OxygenCreated).accNegative * -1f);
    Tuple<float, float>[] data3 = new Tuple<float, float>[ReportManager.Instance.reports.Count];
    for (int index = 0; index < data3.Length; ++index)
      data3[index] = new Tuple<float, float>((float) ReportManager.Instance.reports[index].day, ReportManager.Instance.reports[index].GetEntry(ReportManager.ReportType.CaloriesCreated).accPositive * (1f / 1000f));
    Tuple<float, float>[] data4 = new Tuple<float, float>[ReportManager.Instance.reports.Count];
    for (int index = 0; index < data4.Length; ++index)
      data4[index] = new Tuple<float, float>((float) ReportManager.Instance.reports[index].day, (float) ((double) ReportManager.Instance.reports[index].GetEntry(ReportManager.ReportType.CaloriesCreated).accNegative * (1.0 / 1000.0) * -1.0));
    Tuple<float, float>[] data5 = new Tuple<float, float>[ReportManager.Instance.reports.Count];
    for (int index = 0; index < data5.Length; ++index)
      data5[index] = new Tuple<float, float>((float) ReportManager.Instance.reports[index].day, ReportManager.Instance.reports[index].GetEntry(ReportManager.ReportType.EnergyCreated).accPositive * (1f / 1000f));
    Tuple<float, float>[] data6 = new Tuple<float, float>[ReportManager.Instance.reports.Count];
    for (int index = 0; index < data6.Length; ++index)
      data6[index] = new Tuple<float, float>((float) ReportManager.Instance.reports[index].day, (float) ((double) ReportManager.Instance.reports[index].GetEntry(ReportManager.ReportType.EnergyWasted).accNegative * -1.0 * (1.0 / 1000.0)));
    Tuple<float, float>[] data7 = new Tuple<float, float>[ReportManager.Instance.reports.Count];
    for (int index = 0; index < data7.Length; ++index)
      data7[index] = new Tuple<float, float>((float) ReportManager.Instance.reports[index].day, ReportManager.Instance.reports[index].GetEntry(ReportManager.ReportType.WorkTime).accPositive);
    Tuple<float, float>[] data8 = new Tuple<float, float>[ReportManager.Instance.reports.Count];
    for (int index1 = 0; index1 < data7.Length; ++index1)
    {
      int num1 = 0;
      float num2 = 0.0f;
      ReportManager.ReportEntry entry = ReportManager.Instance.reports[index1].GetEntry(ReportManager.ReportType.WorkTime);
      for (int index2 = 0; index2 < entry.contextEntries.Count; ++index2)
      {
        ++num1;
        num2 += entry.contextEntries[index2].accPositive;
      }
      float b = num2 / (float) num1 / 600f * 100f;
      data8[index1] = new Tuple<float, float>((float) ReportManager.Instance.reports[index1].day, b);
    }
    Tuple<float, float>[] data9 = new Tuple<float, float>[ReportManager.Instance.reports.Count];
    for (int index = 0; index < data9.Length; ++index)
      data9[index] = new Tuple<float, float>((float) ReportManager.Instance.reports[index].day, ReportManager.Instance.reports[index].GetEntry(ReportManager.ReportType.TravelTime).accPositive);
    Tuple<float, float>[] data10 = new Tuple<float, float>[ReportManager.Instance.reports.Count];
    for (int index1 = 0; index1 < data9.Length; ++index1)
    {
      int num1 = 0;
      float num2 = 0.0f;
      ReportManager.ReportEntry entry = ReportManager.Instance.reports[index1].GetEntry(ReportManager.ReportType.TravelTime);
      for (int index2 = 0; index2 < entry.contextEntries.Count; ++index2)
      {
        ++num1;
        num2 += entry.contextEntries[index2].accPositive;
      }
      float b = num2 / (float) num1 / 600f * 100f;
      data10[index1] = new Tuple<float, float>((float) ReportManager.Instance.reports[index1].day, b);
    }
    Tuple<float, float>[] data11 = new Tuple<float, float>[ReportManager.Instance.reports.Count];
    for (int index = 0; index < data7.Length; ++index)
      data11[index] = new Tuple<float, float>((float) ReportManager.Instance.reports[index].day, (float) ReportManager.Instance.reports[index].GetEntry(ReportManager.ReportType.WorkTime).contextEntries.Count);
    Tuple<float, float>[] data12 = new Tuple<float, float>[ReportManager.Instance.reports.Count];
    for (int index1 = 0; index1 < data12.Length; ++index1)
    {
      int num1 = 0;
      float num2 = 0.0f;
      ReportManager.ReportEntry entry = ReportManager.Instance.reports[index1].GetEntry(ReportManager.ReportType.StressDelta);
      for (int index2 = 0; index2 < entry.contextEntries.Count; ++index2)
      {
        ++num1;
        num2 += entry.contextEntries[index2].accPositive;
      }
      data12[index1] = new Tuple<float, float>((float) ReportManager.Instance.reports[index1].day, num2 / (float) num1);
    }
    Tuple<float, float>[] data13 = new Tuple<float, float>[ReportManager.Instance.reports.Count];
    for (int index1 = 0; index1 < data13.Length; ++index1)
    {
      int num1 = 0;
      float num2 = 0.0f;
      ReportManager.ReportEntry entry = ReportManager.Instance.reports[index1].GetEntry(ReportManager.ReportType.StressDelta);
      for (int index2 = 0; index2 < entry.contextEntries.Count; ++index2)
      {
        ++num1;
        num2 += entry.contextEntries[index2].accNegative;
      }
      float num3 = num2 * -1f;
      data13[index1] = new Tuple<float, float>((float) ReportManager.Instance.reports[index1].day, num3 / (float) num1);
    }
    Tuple<float, float>[] data14 = new Tuple<float, float>[ReportManager.Instance.reports.Count];
    for (int index = 0; index < data14.Length; ++index)
      data14[index] = new Tuple<float, float>((float) ReportManager.Instance.reports[index].day, ReportManager.Instance.reports[index].GetEntry(ReportManager.ReportType.DomesticatedCritters).accPositive);
    Tuple<float, float>[] data15 = new Tuple<float, float>[ReportManager.Instance.reports.Count];
    for (int index = 0; index < data15.Length; ++index)
      data15[index] = new Tuple<float, float>((float) ReportManager.Instance.reports[index].day, ReportManager.Instance.reports[index].GetEntry(ReportManager.ReportType.WildCritters).accPositive);
    Tuple<float, float>[] data16 = new Tuple<float, float>[ReportManager.Instance.reports.Count];
    for (int index = 0; index < data16.Length; ++index)
      data16[index] = new Tuple<float, float>((float) ReportManager.Instance.reports[index].day, ReportManager.Instance.reports[index].GetEntry(ReportManager.ReportType.RocketsInFlight).accPositive);
    this.Stats = new RetiredColonyData.RetiredColonyStatistic[16]
    {
      new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.OxygenProduced, data1, (string) UI.RETIRED_COLONY_INFO_SCREEN.STATS.OXYGEN_CREATED, (string) UI.MATH_PICTURES.AXIS_LABELS.CYCLES, (string) UI.UNITSUFFIXES.MASS.KILOGRAM),
      new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.OxygenConsumed, data2, (string) UI.RETIRED_COLONY_INFO_SCREEN.STATS.OXYGEN_CONSUMED, (string) UI.MATH_PICTURES.AXIS_LABELS.CYCLES, (string) UI.UNITSUFFIXES.MASS.KILOGRAM),
      new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.CaloriesProduced, data3, (string) UI.RETIRED_COLONY_INFO_SCREEN.STATS.CALORIES_CREATED, (string) UI.MATH_PICTURES.AXIS_LABELS.CYCLES, (string) UI.UNITSUFFIXES.CALORIES.KILOCALORIE),
      new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.CaloriesRemoved, data4, (string) UI.RETIRED_COLONY_INFO_SCREEN.STATS.CALORIES_CONSUMED, (string) UI.MATH_PICTURES.AXIS_LABELS.CYCLES, (string) UI.UNITSUFFIXES.CALORIES.KILOCALORIE),
      new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.PowerProduced, data5, (string) UI.RETIRED_COLONY_INFO_SCREEN.STATS.POWER_CREATED, (string) UI.MATH_PICTURES.AXIS_LABELS.CYCLES, (string) UI.UNITSUFFIXES.ELECTRICAL.KILOJOULE),
      new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.PowerWasted, data6, (string) UI.RETIRED_COLONY_INFO_SCREEN.STATS.POWER_WASTED, (string) UI.MATH_PICTURES.AXIS_LABELS.CYCLES, (string) UI.UNITSUFFIXES.ELECTRICAL.KILOJOULE),
      new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.WorkTime, data7, (string) UI.RETIRED_COLONY_INFO_SCREEN.STATS.WORK_TIME, (string) UI.MATH_PICTURES.AXIS_LABELS.CYCLES, (string) UI.UNITSUFFIXES.SECONDS),
      new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.AverageWorkTime, data8, (string) UI.RETIRED_COLONY_INFO_SCREEN.STATS.AVERAGE_WORK_TIME, (string) UI.MATH_PICTURES.AXIS_LABELS.CYCLES, (string) UI.UNITSUFFIXES.PERCENT),
      new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.TravelTime, data9, (string) UI.RETIRED_COLONY_INFO_SCREEN.STATS.TRAVEL_TIME, (string) UI.MATH_PICTURES.AXIS_LABELS.CYCLES, (string) UI.UNITSUFFIXES.SECONDS),
      new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.AverageTravelTime, data10, (string) UI.RETIRED_COLONY_INFO_SCREEN.STATS.AVERAGE_TRAVEL_TIME, (string) UI.MATH_PICTURES.AXIS_LABELS.CYCLES, (string) UI.UNITSUFFIXES.PERCENT),
      new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.LiveDuplicants, data11, (string) UI.RETIRED_COLONY_INFO_SCREEN.STATS.LIVE_DUPLICANTS, (string) UI.MATH_PICTURES.AXIS_LABELS.CYCLES, (string) UI.UNITSUFFIXES.DUPLICANTS),
      new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.RocketsInFlight, data16, (string) UI.RETIRED_COLONY_INFO_SCREEN.STATS.ROCKET_MISSIONS, (string) UI.MATH_PICTURES.AXIS_LABELS.CYCLES, (string) UI.UNITSUFFIXES.ROCKET_MISSIONS),
      new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.AverageStressCreated, data12, (string) UI.RETIRED_COLONY_INFO_SCREEN.STATS.AVERAGE_STRESS_CREATED, (string) UI.MATH_PICTURES.AXIS_LABELS.CYCLES, (string) UI.UNITSUFFIXES.PERCENT),
      new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.AverageStressRemoved, data13, (string) UI.RETIRED_COLONY_INFO_SCREEN.STATS.AVERAGE_STRESS_REMOVED, (string) UI.MATH_PICTURES.AXIS_LABELS.CYCLES, (string) UI.UNITSUFFIXES.PERCENT),
      new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.DomesticatedCritters, data14, (string) UI.RETIRED_COLONY_INFO_SCREEN.STATS.NUMBER_DOMESTICATED_CRITTERS, (string) UI.MATH_PICTURES.AXIS_LABELS.CYCLES, (string) UI.UNITSUFFIXES.CRITTERS),
      new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.WildCritters, data15, (string) UI.RETIRED_COLONY_INFO_SCREEN.STATS.NUMBER_WILD_CRITTERS, (string) UI.MATH_PICTURES.AXIS_LABELS.CYCLES, (string) UI.UNITSUFFIXES.CRITTERS)
    };
  }

  public string colonyName { get; set; }

  public int cycleCount { get; set; }

  public string date { get; set; }

  public string[] achievements { get; set; }

  public RetiredColonyData.RetiredDuplicantData[] Duplicants { get; set; }

  public List<Tuple<string, int>> buildings { get; set; }

  public RetiredColonyData.RetiredColonyStatistic[] Stats { get; set; }

  public static class DataIDs
  {
    public static string OxygenProduced = "oxygenProduced";
    public static string OxygenConsumed = "oxygenConsumed";
    public static string CaloriesProduced = "caloriesProduced";
    public static string CaloriesRemoved = "caloriesRemoved";
    public static string PowerProduced = "powerProduced";
    public static string PowerWasted = "powerWasted";
    public static string WorkTime = "workTime";
    public static string TravelTime = "travelTime";
    public static string AverageWorkTime = "averageWorkTime";
    public static string AverageTravelTime = "averageTravelTime";
    public static string LiveDuplicants = "liveDuplicants";
    public static string AverageStressCreated = "averageStressCreated";
    public static string AverageStressRemoved = "averageStressRemoved";
    public static string DomesticatedCritters = "domesticatedCritters";
    public static string WildCritters = "wildCritters";
    public static string AverageGerms = "averageGerms";
    public static string RocketsInFlight = "rocketsInFlight";
  }

  public class RetiredColonyStatistic
  {
    public string id;
    public Tuple<float, float>[] value;
    public string name;
    public string nameX;
    public string nameY;

    public RetiredColonyStatistic()
    {
    }

    public RetiredColonyStatistic(
      string id,
      Tuple<float, float>[] data,
      string name,
      string axisNameX,
      string axisNameY)
    {
      this.id = id;
      this.value = data;
      this.name = name;
      this.nameX = axisNameX;
      this.nameY = axisNameY;
    }

    public Tuple<float, float> GetByMaxValue()
    {
      if (this.value.Length == 0)
        return new Tuple<float, float>(0.0f, 0.0f);
      int index1 = -1;
      float num = -1f;
      for (int index2 = 0; index2 < this.value.Length; ++index2)
      {
        if ((double) this.value[index2].second > (double) num)
        {
          num = this.value[index2].second;
          index1 = index2;
        }
      }
      if (index1 == -1)
        index1 = 0;
      return this.value[index1];
    }

    public Tuple<float, float> GetByMaxKey()
    {
      if (this.value.Length == 0)
        return new Tuple<float, float>(0.0f, 0.0f);
      int index1 = -1;
      float num = -1f;
      for (int index2 = 0; index2 < this.value.Length; ++index2)
      {
        if ((double) this.value[index2].first > (double) num)
        {
          num = this.value[index2].first;
          index1 = index2;
        }
      }
      return this.value[index1];
    }
  }

  public class RetiredDuplicantData
  {
    public string name;
    public int age;
    public int skillPointsGained;
    public Dictionary<string, string> accessories;
  }
}
