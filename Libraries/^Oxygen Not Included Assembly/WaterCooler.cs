﻿// Decompiled with JetBrains decompiler
// Type: WaterCooler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class WaterCooler : StateMachineComponent<WaterCooler.StatesInstance>, IApproachable, IEffectDescriptor
{
  private static readonly EventSystem.IntraObjectHandler<WaterCooler> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<WaterCooler>((System.Action<WaterCooler, object>) ((component, data) => component.OnStorageChange(data)));
  public CellOffset[] socializeOffsets = new CellOffset[4]
  {
    new CellOffset(-1, 0),
    new CellOffset(2, 0),
    new CellOffset(0, 0),
    new CellOffset(1, 0)
  };
  public int choreCount = 2;
  public float workTime = 5f;
  private CellOffset[] drinkOffsets = new CellOffset[2]
  {
    new CellOffset(0, 0),
    new CellOffset(1, 0)
  };
  public const float DRINK_MASS = 1f;
  public const string SPECIFIC_EFFECT = "Socialized";
  private Chore[] chores;
  private HandleVector<int>.Handle validNavCellChangedPartitionerEntry;
  private SocialGatheringPointWorkable[] workables;
  [MyCmpGet]
  private Storage storage;
  public bool choresDirty;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    GameScheduler.Instance.Schedule("Scheduling Tutorial", 2f, (System.Action<object>) (obj => Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, true)), (object) null, (SchedulerGroup) null);
    this.workables = new SocialGatheringPointWorkable[this.socializeOffsets.Length];
    for (int index = 0; index < this.workables.Length; ++index)
    {
      SocialGatheringPointWorkable gatheringPointWorkable = ChoreHelpers.CreateLocator("WaterCoolerWorkable", Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) this), this.socializeOffsets[index]), Grid.SceneLayer.Move)).AddOrGet<SocialGatheringPointWorkable>();
      gatheringPointWorkable.specificEffect = "Socialized";
      gatheringPointWorkable.SetWorkTime(this.workTime);
      this.workables[index] = gatheringPointWorkable;
    }
    this.chores = new Chore[this.socializeOffsets.Length];
    this.validNavCellChangedPartitionerEntry = GameScenePartitioner.Instance.Add(nameof (WaterCooler), (object) this, new Extents(Grid.PosToCell((KMonoBehaviour) this), this.socializeOffsets), GameScenePartitioner.Instance.validNavCellChangedLayer, new System.Action<object>(this.OnCellChanged));
    this.Subscribe<WaterCooler>(-1697596308, WaterCooler.OnStorageChangeDelegate);
    this.smi.StartSM();
  }

  protected override void OnCleanUp()
  {
    GameScenePartitioner.Instance.Free(ref this.validNavCellChangedPartitionerEntry);
    this.CancelDrinkChores();
    for (int index = 0; index < this.workables.Length; ++index)
    {
      if ((bool) ((UnityEngine.Object) this.workables[index]))
      {
        Util.KDestroyGameObject((Component) this.workables[index]);
        this.workables[index] = (SocialGatheringPointWorkable) null;
      }
    }
    base.OnCleanUp();
  }

  public void UpdateDrinkChores(bool force = true)
  {
    if (!force && !this.choresDirty)
      return;
    float massAvailable = this.storage.GetMassAvailable(GameTags.Water);
    int num = 0;
    for (int index = 0; index < this.socializeOffsets.Length; ++index)
    {
      CellOffset socializeOffset = this.socializeOffsets[index];
      Chore chore = this.chores[index];
      if (num < this.choreCount && this.IsOffsetValid(socializeOffset) && (double) massAvailable >= 1.0)
      {
        ++num;
        --massAvailable;
        if (chore == null || chore.isComplete)
          this.chores[index] = (Chore) new WaterCoolerChore((IStateMachineTarget) this, (Workable) this.workables[index], (System.Action<Chore>) null, (System.Action<Chore>) null, new System.Action<Chore>(this.OnChoreEnd));
      }
      else if (chore != null)
      {
        chore.Cancel("invalid");
        this.chores[index] = (Chore) null;
      }
    }
    this.choresDirty = false;
  }

  public void CancelDrinkChores()
  {
    for (int index = 0; index < this.socializeOffsets.Length; ++index)
    {
      Chore chore = this.chores[index];
      if (chore != null)
      {
        chore.Cancel("cancelled");
        this.chores[index] = (Chore) null;
      }
    }
  }

  private bool IsOffsetValid(CellOffset offset)
  {
    int cell = Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) this), offset);
    int anchor_cell = Grid.CellBelow(cell);
    return GameNavGrids.FloorValidator.IsWalkableCell(cell, anchor_cell, false);
  }

  private void OnChoreEnd(Chore chore)
  {
    this.choresDirty = true;
  }

  private void OnCellChanged(object data)
  {
    this.choresDirty = true;
  }

  private void OnStorageChange(object data)
  {
    this.choresDirty = true;
  }

  public CellOffset[] GetOffsets()
  {
    return this.drinkOffsets;
  }

  public int GetCell()
  {
    return Grid.PosToCell((KMonoBehaviour) this);
  }

  private void AddRequirementDesc(List<Descriptor> descs, Tag tag, float mass)
  {
    string str = tag.ProperName();
    Descriptor descriptor = new Descriptor();
    descriptor.SetupDescriptor(string.Format((string) UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, (object) str, (object) GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, (object) str, (object) GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement);
    descs.Add(descriptor);
  }

  List<Descriptor> IEffectDescriptor.GetDescriptors(BuildingDef def)
  {
    List<Descriptor> descs = new List<Descriptor>();
    Descriptor descriptor = new Descriptor();
    descriptor.SetupDescriptor((string) UI.BUILDINGEFFECTS.RECREATION, (string) UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect);
    descs.Add(descriptor);
    Effect.AddModifierDescriptions(this.gameObject, descs, "Socialized", true);
    this.AddRequirementDesc(descs, GameTags.Water, 1f);
    return descs;
  }

  Transform IApproachable.get_transform()
  {
    return this.transform;
  }

  public class States : GameStateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler>
  {
    public GameStateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.State unoperational;
    public GameStateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.State waitingfordelivery;
    public GameStateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.State dispensing;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.unoperational;
      this.unoperational.TagTransition(GameTags.Operational, this.waitingfordelivery, false).PlayAnim("off");
      this.waitingfordelivery.TagTransition(GameTags.Operational, this.unoperational, true).Transition(this.dispensing, (StateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.Transition.ConditionCallback) (smi => smi.HasMinimumMass()), UpdateRate.SIM_200ms).EventTransition(GameHashes.OnStorageChange, this.dispensing, (StateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.Transition.ConditionCallback) (smi => smi.HasMinimumMass())).PlayAnim("off");
      this.dispensing.Enter("StartMeter", (StateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.State.Callback) (smi => smi.StartMeter())).Enter("UpdateDrinkChores.force", (StateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.State.Callback) (smi => smi.master.UpdateDrinkChores(true))).Update("UpdateDrinkChores", (System.Action<WaterCooler.StatesInstance, float>) ((smi, dt) => smi.master.UpdateDrinkChores(true)), UpdateRate.SIM_200ms, false).Exit("CancelDrinkChores", (StateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.State.Callback) (smi => smi.master.CancelDrinkChores())).TagTransition(GameTags.Operational, this.unoperational, true).EventTransition(GameHashes.OnStorageChange, this.waitingfordelivery, (StateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.Transition.ConditionCallback) (smi => !smi.HasMinimumMass())).PlayAnim("working");
    }
  }

  public class StatesInstance : GameStateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.GameInstance
  {
    private Storage storage;
    private MeterController meter;

    public StatesInstance(WaterCooler smi)
      : base(smi)
    {
      this.meter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_bottle", nameof (meter), Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[1]
      {
        "meter_bottle"
      });
      this.storage = this.master.GetComponent<Storage>();
      this.Subscribe(-1697596308, new System.Action<object>(this.OnStorageChange));
    }

    private void OnStorageChange(object data)
    {
      this.meter.SetPositionPercent(Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg));
    }

    public void StartMeter()
    {
      PrimaryElement firstWithMass = this.storage.FindFirstWithMass(GameTags.Water);
      if ((UnityEngine.Object) firstWithMass == (UnityEngine.Object) null)
        return;
      this.meter.SetSymbolTint(new KAnimHashedString("meter_water"), firstWithMass.Element.substance.colour);
      this.OnStorageChange((object) null);
    }

    public bool HasMinimumMass()
    {
      return (double) this.storage.GetMassAvailable(GameTags.Water) >= 1.0;
    }
  }
}