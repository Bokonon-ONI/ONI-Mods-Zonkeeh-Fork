﻿// Decompiled with JetBrains decompiler
// Type: ThermalBlockConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class ThermalBlockConfig : IBuildingConfig
{
  private static readonly CellOffset[] overrideOffsets = new CellOffset[4]
  {
    new CellOffset(-1, -1),
    new CellOffset(1, -1),
    new CellOffset(-1, 1),
    new CellOffset(1, 1)
  };
  public const string ID = "ThermalBlock";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "ThermalBlock";
    int width = 1;
    int height = 1;
    string anim = "thermalblock_kanim";
    int hitpoints = 30;
    float construction_time = 120f;
    float[] tieR5 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
    string[] anyBuildable = MATERIALS.ANY_BUILDABLE;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.NotInTiles;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR5, anyBuildable, melting_point, build_location_rule, DECOR.NONE, none, 0.2f);
    buildingDef.Floodable = false;
    buildingDef.Overheatable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.ViewMode = OverlayModes.Temperature.ID;
    buildingDef.DefaultAnimState = "off";
    buildingDef.ObjectLayer = ObjectLayer.Backwall;
    buildingDef.SceneLayer = Grid.SceneLayer.Backwall;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<AnimTileable>().objectLayer = ObjectLayer.Backwall;
    go.AddComponent<ZoneTile>();
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.GetComponent<KPrefabID>().prefabSpawnFn += (KPrefabID.PrefabFn) (game_object =>
    {
      HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(game_object);
      StructureTemperaturePayload payload = GameComps.StructureTemperatures.GetPayload(handle);
      int cell = Grid.PosToCell(game_object);
      payload.OverrideExtents(new Extents(cell, ThermalBlockConfig.overrideOffsets));
      GameComps.StructureTemperatures.SetPayload(handle, ref payload);
    });
  }
}
