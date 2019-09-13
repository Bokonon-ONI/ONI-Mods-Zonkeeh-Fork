﻿// Decompiled with JetBrains decompiler
// Type: PowerStationToolsConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class PowerStationToolsConfig : IEntityConfig
{
  public static readonly Tag tag = TagManager.Create("PowerStationTools");
  public const string ID = "PowerStationTools";
  public const float MASS = 5f;

  public GameObject CreatePrefab()
  {
    return EntityTemplates.CreateLooseEntity("PowerStationTools", (string) ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME, (string) ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.DESC, 5f, true, Assets.GetAnim((HashedString) "kit_electrician_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, 0, SimHashes.Creature, new List<Tag>()
    {
      GameTags.MiscPickupable
    });
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}