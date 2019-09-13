﻿// Decompiled with JetBrains decompiler
// Type: LiquidConduitRadiantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class LiquidConduitRadiantConfig : IBuildingConfig
{
  public const string ID = "LiquidConduitRadiant";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "LiquidConduitRadiant";
    int width = 1;
    int height = 1;
    string anim = "utilities_liquid_radiant_kanim";
    int hitpoints = 10;
    float construction_time = 10f;
    float[] tieR1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    float melting_point = 3200f;
    BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR1, refinedMetals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
    buildingDef.ThermalConductivity = 2f;
    buildingDef.Floodable = false;
    buildingDef.Overheatable = false;
    buildingDef.Entombable = false;
    buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
    buildingDef.ObjectLayer = ObjectLayer.LiquidConduit;
    buildingDef.TileLayer = ObjectLayer.LiquidConduitTile;
    buildingDef.ReplacementLayer = ObjectLayer.ReplacementLiquidConduit;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
    buildingDef.SceneLayer = Grid.SceneLayer.LiquidConduits;
    buildingDef.isKAnimTile = true;
    buildingDef.isUtility = true;
    buildingDef.DragBuild = true;
    buildingDef.ReplacementTags = new List<Tag>();
    buildingDef.ReplacementTags.Add(GameTags.Pipes);
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "LiquidConduitRadiant");
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    go.AddOrGet<Conduit>().type = ConduitType.Liquid;
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    KAnimGraphTileVisualizer graphTileVisualizer = go.AddComponent<KAnimGraphTileVisualizer>();
    graphTileVisualizer.connectionSource = KAnimGraphTileVisualizer.ConnectionSource.Liquid;
    graphTileVisualizer.isPhysicalBuilding = false;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.GetComponent<Building>().Def.BuildingUnderConstruction.GetComponent<Constructable>().isDiggingRequired = false;
    go.AddComponent<EmptyConduitWorkable>();
    KAnimGraphTileVisualizer graphTileVisualizer = go.AddComponent<KAnimGraphTileVisualizer>();
    graphTileVisualizer.connectionSource = KAnimGraphTileVisualizer.ConnectionSource.Liquid;
    graphTileVisualizer.isPhysicalBuilding = true;
    go.GetComponent<KPrefabID>().AddTag(GameTags.Pipes, false);
    LiquidConduitConfig.CommonConduitPostConfigureComplete(go);
  }
}
