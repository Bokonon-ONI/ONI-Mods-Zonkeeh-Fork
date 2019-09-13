﻿// Decompiled with JetBrains decompiler
// Type: WireRefinedBridgeConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class WireRefinedBridgeConfig : WireBridgeConfig
{
  public new const string ID = "WireRefinedBridge";

  protected override string GetID()
  {
    return "WireRefinedBridge";
  }

  public override BuildingDef CreateBuildingDef()
  {
    BuildingDef buildingDef = base.CreateBuildingDef();
    buildingDef.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "utilityelectricbridgeconductive_kanim")
    };
    buildingDef.Mass = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
    buildingDef.MaterialCategory = MATERIALS.REFINED_METALS;
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.WireIDs, "WireRefinedBridge");
    return buildingDef;
  }

  protected override WireUtilityNetworkLink AddNetworkLink(GameObject go)
  {
    WireUtilityNetworkLink utilityNetworkLink = base.AddNetworkLink(go);
    utilityNetworkLink.maxWattageRating = Wire.WattageRating.Max2000;
    return utilityNetworkLink;
  }
}
