﻿// Decompiled with JetBrains decompiler
// Type: BuildingConfigManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

public class BuildingConfigManager : KMonoBehaviour
{
  private Dictionary<IBuildingConfig, BuildingDef> configTable = new Dictionary<IBuildingConfig, BuildingDef>();
  private string[] NonBuildableBuildings = new string[2]
  {
    "Headquarters",
    "POIBunkerExteriorDoor"
  };
  private HashSet<System.Type> defaultKComponents = new HashSet<System.Type>();
  private HashSet<System.Type> defaultBuildingCompleteKComponents = new HashSet<System.Type>();
  private Dictionary<System.Type, HashSet<Tag>> ignoredDefaultKComponents = new Dictionary<System.Type, HashSet<Tag>>();
  private Dictionary<Tag, HashSet<System.Type>> buildingCompleteKComponents = new Dictionary<Tag, HashSet<System.Type>>();
  public static BuildingConfigManager Instance;
  private GameObject baseTemplate;

  protected override void OnPrefabInit()
  {
    BuildingConfigManager.Instance = this;
    this.baseTemplate = new GameObject("BuildingTemplate");
    this.baseTemplate.SetActive(false);
    this.baseTemplate.AddComponent<KPrefabID>();
    this.baseTemplate.AddComponent<KSelectable>();
    this.baseTemplate.AddComponent<Modifiers>();
    this.baseTemplate.AddComponent<PrimaryElement>();
    this.baseTemplate.AddComponent<BuildingComplete>();
    this.baseTemplate.AddComponent<StateMachineController>();
    this.baseTemplate.AddComponent<Deconstructable>();
    this.baseTemplate.AddComponent<SaveLoadRoot>();
    this.baseTemplate.AddComponent<OccupyArea>();
    this.baseTemplate.AddComponent<DecorProvider>();
    this.baseTemplate.AddComponent<Operational>();
    this.baseTemplate.AddComponent<BuildingEnabledButton>();
    this.baseTemplate.AddComponent<Prioritizable>();
    this.baseTemplate.AddComponent<BuildingHP>();
    this.baseTemplate.AddComponent<LoopingSounds>();
    this.baseTemplate.AddComponent<InvalidPortReporter>();
    this.defaultBuildingCompleteKComponents.Add(typeof (RequiresFoundation));
  }

  public static string GetUnderConstructionName(string name)
  {
    return name + "UnderConstruction";
  }

  public void RegisterBuilding(IBuildingConfig config)
  {
    BuildingDef buildingDef = config.CreateBuildingDef();
    this.configTable[config] = buildingDef;
    GameObject go = UnityEngine.Object.Instantiate<GameObject>(this.baseTemplate);
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) go);
    go.GetComponent<KPrefabID>().PrefabTag = buildingDef.Tag;
    go.name = buildingDef.PrefabID + "Template";
    go.GetComponent<Building>().Def = buildingDef;
    go.GetComponent<OccupyArea>().OccupiedCellsOffsets = buildingDef.PlacementOffsets;
    if (buildingDef.Deprecated)
      go.GetComponent<KPrefabID>().AddTag(GameTags.DeprecatedContent, false);
    config.ConfigureBuildingTemplate(go, buildingDef.Tag);
    buildingDef.BuildingComplete = BuildingLoader.Instance.CreateBuildingComplete(go, buildingDef);
    bool flag = true;
    for (int index = 0; index < this.NonBuildableBuildings.Length; ++index)
    {
      if (buildingDef.PrefabID == this.NonBuildableBuildings[index])
      {
        flag = false;
        break;
      }
    }
    if (flag)
    {
      buildingDef.BuildingUnderConstruction = BuildingLoader.Instance.CreateBuildingUnderConstruction(buildingDef);
      buildingDef.BuildingUnderConstruction.name = BuildingConfigManager.GetUnderConstructionName(buildingDef.BuildingUnderConstruction.name);
      buildingDef.BuildingPreview = BuildingLoader.Instance.CreateBuildingPreview(buildingDef);
      buildingDef.BuildingPreview.name += "Preview";
    }
    buildingDef.PostProcess();
    config.DoPostConfigureComplete(buildingDef.BuildingComplete);
    if (flag)
    {
      config.DoPostConfigurePreview(buildingDef, buildingDef.BuildingPreview);
      config.DoPostConfigureUnderConstruction(buildingDef.BuildingUnderConstruction);
    }
    Assets.AddBuildingDef(buildingDef);
  }

  public void ConfigurePost()
  {
    foreach (KeyValuePair<IBuildingConfig, BuildingDef> keyValuePair in this.configTable)
      keyValuePair.Key.ConfigurePost(keyValuePair.Value);
  }

  public void IgnoreDefaultKComponent(System.Type type_to_ignore, Tag building_tag)
  {
    HashSet<Tag> tagSet;
    if (!this.ignoredDefaultKComponents.TryGetValue(type_to_ignore, out tagSet))
    {
      tagSet = new HashSet<Tag>();
      this.ignoredDefaultKComponents[type_to_ignore] = tagSet;
    }
    tagSet.Add(building_tag);
  }

  private bool IsIgnoredDefaultKComponent(Tag building_tag, System.Type type)
  {
    bool flag = false;
    HashSet<Tag> tagSet;
    if (this.ignoredDefaultKComponents.TryGetValue(type, out tagSet) && tagSet.Contains(building_tag))
      flag = true;
    return flag;
  }

  public void AddBuildingCompleteKComponents(GameObject go, Tag prefab_tag)
  {
    foreach (System.Type completeKcomponent in this.defaultBuildingCompleteKComponents)
    {
      if (!this.IsIgnoredDefaultKComponent(prefab_tag, completeKcomponent))
        GameComps.GetKComponentManager(completeKcomponent).Add(go);
    }
    HashSet<System.Type> typeSet;
    if (!this.buildingCompleteKComponents.TryGetValue(prefab_tag, out typeSet))
      return;
    foreach (System.Type kcomponent_type in typeSet)
      GameComps.GetKComponentManager(kcomponent_type).Add(go);
  }

  public void DestroyBuildingCompleteKComponents(GameObject go, Tag prefab_tag)
  {
    foreach (System.Type completeKcomponent in this.defaultBuildingCompleteKComponents)
    {
      if (!this.IsIgnoredDefaultKComponent(prefab_tag, completeKcomponent))
        GameComps.GetKComponentManager(completeKcomponent).Remove(go);
    }
    HashSet<System.Type> typeSet;
    if (!this.buildingCompleteKComponents.TryGetValue(prefab_tag, out typeSet))
      return;
    foreach (System.Type kcomponent_type in typeSet)
      GameComps.GetKComponentManager(kcomponent_type).Remove(go);
  }

  public void AddDefaultBuildingCompleteKComponent(System.Type kcomponent_type)
  {
    this.defaultKComponents.Add(kcomponent_type);
  }

  public void AddBuildingCompleteKComponent(Tag prefab_tag, System.Type kcomponent_type)
  {
    HashSet<System.Type> typeSet;
    if (!this.buildingCompleteKComponents.TryGetValue(prefab_tag, out typeSet))
    {
      typeSet = new HashSet<System.Type>();
      this.buildingCompleteKComponents[prefab_tag] = typeSet;
    }
    typeSet.Add(kcomponent_type);
  }
}
