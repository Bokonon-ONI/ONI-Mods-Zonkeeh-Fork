﻿// Decompiled with JetBrains decompiler
// Type: GasConduitConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class GasConduitConfig : IBuildingConfig
{
  public const string ID = "GasConduit";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "GasConduit";
    int width = 1;
    int height = 1;
    string anim = "utilities_gas_kanim";
    int hitpoints = 10;
    float construction_time = 3f;
    float[] tieR0 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
    string[] rawMinerals = MATERIALS.RAW_MINERALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR0, rawMinerals, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
    buildingDef.Floodable = false;
    buildingDef.Overheatable = false;
    buildingDef.Entombable = false;
    buildingDef.ViewMode = OverlayModes.GasConduits.ID;
    buildingDef.ObjectLayer = ObjectLayer.GasConduit;
    buildingDef.TileLayer = ObjectLayer.GasConduitTile;
    buildingDef.ReplacementLayer = ObjectLayer.ReplacementGasConduit;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = 0.0f;
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
    buildingDef.SceneLayer = Grid.SceneLayer.GasConduits;
    buildingDef.isKAnimTile = true;
    buildingDef.isUtility = true;
    buildingDef.DragBuild = true;
    buildingDef.ReplacementTags = new List<Tag>();
    buildingDef.ReplacementTags.Add(GameTags.Vents);
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, buildingDef.PrefabID);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    go.AddOrGet<Conduit>().type = ConduitType.Gas;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.GetComponent<Building>().Def.BuildingUnderConstruction.GetComponent<Constructable>().isDiggingRequired = false;
    go.AddComponent<EmptyConduitWorkable>();
    KAnimGraphTileVisualizer graphTileVisualizer = go.AddComponent<KAnimGraphTileVisualizer>();
    graphTileVisualizer.connectionSource = KAnimGraphTileVisualizer.ConnectionSource.Gas;
    graphTileVisualizer.isPhysicalBuilding = true;
    go.GetComponent<KPrefabID>().AddTag(GameTags.Vents, false);
    LiquidConduitConfig.CommonConduitPostConfigureComplete(go);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    KAnimGraphTileVisualizer graphTileVisualizer = go.AddComponent<KAnimGraphTileVisualizer>();
    graphTileVisualizer.connectionSource = KAnimGraphTileVisualizer.ConnectionSource.Gas;
    graphTileVisualizer.isPhysicalBuilding = false;
  }
}