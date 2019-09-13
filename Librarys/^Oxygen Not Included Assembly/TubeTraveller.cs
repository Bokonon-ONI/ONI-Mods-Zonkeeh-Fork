﻿// Decompiled with JetBrains decompiler
// Type: TubeTraveller
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

public class TubeTraveller : GameStateMachine<TubeTraveller, TubeTraveller.Instance>, OxygenBreather.IGasProvider
{
  private List<Effect> immunities = new List<Effect>();
  private List<AttributeModifier> modifiers = new List<AttributeModifier>();

  public void InitModifiers()
  {
    this.modifiers.Add(new AttributeModifier(Db.Get().Attributes.Insulation.Id, (float) TUNING.EQUIPMENT.SUITS.ATMOSUIT_INSULATION, (string) STRINGS.BUILDINGS.PREFABS.TRAVELTUBE.NAME, false, false, true));
    this.modifiers.Add(new AttributeModifier(Db.Get().Attributes.ThermalConductivityBarrier.Id, TUNING.EQUIPMENT.SUITS.ATMOSUIT_THERMAL_CONDUCTIVITY_BARRIER, (string) STRINGS.BUILDINGS.PREFABS.TRAVELTUBE.NAME, false, false, true));
    this.modifiers.Add(new AttributeModifier(Db.Get().Amounts.Bladder.deltaAttribute.Id, TUNING.EQUIPMENT.SUITS.ATMOSUIT_BLADDER, (string) STRINGS.BUILDINGS.PREFABS.TRAVELTUBE.NAME, false, false, true));
    this.modifiers.Add(new AttributeModifier(Db.Get().Attributes.ScaldingThreshold.Id, (float) TUNING.EQUIPMENT.SUITS.ATMOSUIT_SCALDING, (string) STRINGS.BUILDINGS.PREFABS.TRAVELTUBE.NAME, false, false, true));
    this.immunities.Add(Db.Get().effects.Get("SoakingWet"));
    this.immunities.Add(Db.Get().effects.Get("WetFeet"));
    this.immunities.Add(Db.Get().effects.Get("PoppedEarDrums"));
  }

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.InitModifiers();
    default_state = (StateMachine.BaseState) this.root;
    this.root.DoNothing();
  }

  public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
  {
  }

  public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
  {
  }

  public bool ConsumeGas(OxygenBreather oxygen_breather, float amount)
  {
    return true;
  }

  public bool ShouldEmitCO2()
  {
    return false;
  }

  public bool ShouldStoreCO2()
  {
    return false;
  }

  public class Instance : GameStateMachine<TubeTraveller, TubeTraveller.Instance, IStateMachineTarget, object>.GameInstance
  {
    private List<TravelTubeEntrance> reservations = new List<TravelTubeEntrance>();
    private bool inTube;
    private bool hadSuitTank;

    public Instance(IStateMachineTarget master)
      : base(master)
    {
    }

    public int prefabInstanceID
    {
      get
      {
        return this.GetComponent<Navigator>().gameObject.GetComponent<KPrefabID>().InstanceID;
      }
    }

    public void OnPathAdvanced(object data)
    {
      this.UnreserveEntrances();
      this.ReserveEntrances();
    }

    public void ReserveEntrances()
    {
      PathFinder.Path path = this.GetComponent<Navigator>().path;
      if (path.nodes == null)
        return;
      for (int index = 0; index < path.nodes.Count - 1; ++index)
      {
        if (path.nodes[index].navType == NavType.Floor && path.nodes[index + 1].navType == NavType.Tube)
        {
          int cell = path.nodes[index].cell;
          if (Grid.HasUsableTubeEntrance(cell, this.prefabInstanceID))
          {
            GameObject gameObject = Grid.Objects[cell, 1];
            if ((bool) ((Object) gameObject))
            {
              TravelTubeEntrance component = gameObject.GetComponent<TravelTubeEntrance>();
              if ((bool) ((Object) component))
              {
                component.Reserve(this, this.prefabInstanceID);
                this.reservations.Add(component);
              }
            }
          }
        }
      }
    }

    public void UnreserveEntrances()
    {
      foreach (TravelTubeEntrance reservation in this.reservations)
      {
        if (!((Object) reservation == (Object) null))
          reservation.Unreserve(this, this.prefabInstanceID);
      }
      this.reservations.Clear();
    }

    public void OnTubeTransition(bool nowInTube)
    {
      if (nowInTube == this.inTube)
        return;
      this.inTube = nowInTube;
      Effects component1 = this.GetComponent<Effects>();
      Attributes attributes = this.gameObject.GetAttributes();
      if (nowInTube)
      {
        this.hadSuitTank = this.HasSuitTank();
        if (!this.hadSuitTank)
          this.GetComponent<OxygenBreather>().SetGasProvider((OxygenBreather.IGasProvider) this.sm);
        foreach (Effect immunity in this.sm.immunities)
          component1.AddImmunity(immunity);
        foreach (AttributeModifier modifier in this.sm.modifiers)
          attributes.Add(modifier);
      }
      else
      {
        if (!this.hadSuitTank)
          this.GetComponent<OxygenBreather>().SetGasProvider((OxygenBreather.IGasProvider) new GasBreatherFromWorldProvider());
        foreach (Effect immunity in this.sm.immunities)
          component1.RemoveImmunity(immunity);
        foreach (AttributeModifier modifier in this.sm.modifiers)
          attributes.Remove(modifier);
      }
      CreatureSimTemperatureTransfer component2 = this.gameObject.GetComponent<CreatureSimTemperatureTransfer>();
      if (!((Object) component2 != (Object) null))
        return;
      component2.RefreshRegistration();
    }

    private bool HasSuitTank()
    {
      AssignableSlotInstance slot = this.GetComponent<MinionIdentity>().GetEquipment().GetSlot(Db.Get().AssignableSlots.Suit);
      if (slot != null && (Object) slot.assignable != (Object) null)
        return (Object) slot.assignable.GetComponent<SuitTank>() != (Object) null;
      return false;
    }
  }
}
