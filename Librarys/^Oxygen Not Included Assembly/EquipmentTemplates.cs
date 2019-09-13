﻿// Decompiled with JetBrains decompiler
// Type: EquipmentTemplates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentTemplates
{
  public static EquipmentDef CreateEquipmentDef(
    string Id,
    string Slot,
    SimHashes OutputElement,
    float Mass,
    string Anim,
    string SnapOn,
    string BuildOverride,
    int BuildOverridePriority,
    List<AttributeModifier> AttributeModifiers,
    string SnapOn1 = null,
    bool IsBody = false,
    EntityTemplates.CollisionShape CollisionShape = EntityTemplates.CollisionShape.CIRCLE,
    float width = 0.325f,
    float height = 0.325f,
    Tag[] additional_tags = null,
    string RecipeTechUnlock = null)
  {
    EquipmentDef instance = ScriptableObject.CreateInstance<EquipmentDef>();
    instance.Id = Id;
    instance.Slot = Slot;
    instance.RecipeTechUnlock = RecipeTechUnlock;
    instance.OutputElement = OutputElement;
    instance.Mass = Mass;
    instance.Anim = Assets.GetAnim((HashedString) Anim);
    instance.SnapOn = SnapOn;
    instance.SnapOn1 = SnapOn1;
    instance.BuildOverride = BuildOverride == null || BuildOverride.Length <= 0 ? (KAnimFile) null : Assets.GetAnim((HashedString) BuildOverride);
    instance.BuildOverridePriority = BuildOverridePriority;
    instance.IsBody = IsBody;
    instance.AttributeModifiers = AttributeModifiers;
    instance.CollisionShape = CollisionShape;
    instance.width = width;
    instance.height = height;
    instance.AdditionalTags = additional_tags;
    return instance;
  }
}
