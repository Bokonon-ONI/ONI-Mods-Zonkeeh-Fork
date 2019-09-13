﻿// Decompiled with JetBrains decompiler
// Type: TemperatureMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;

public class TemperatureMonitor : GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance>
{
  public GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.State homeostatic;
  public GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.State hyperthermic;
  public GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.State hypothermic;
  public GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.State hyperthermic_pre;
  public GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.State hypothermic_pre;
  public GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.State deathcold;
  public GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.State deathhot;
  private const float TEMPERATURE_AVERAGING_RANGE = 4f;
  public StateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.IntParameter warmUpCell;
  public StateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.IntParameter coolDownCell;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.homeostatic;
    this.root.Enter((StateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi =>
    {
      smi.averageTemperature = smi.primaryElement.Temperature;
      SicknessTrigger component = smi.master.GetComponent<SicknessTrigger>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.AddTrigger(GameHashes.TooHotSickness, new string[1]
      {
        "HeatSickness"
      }, (SicknessTrigger.SourceCallback) ((s, t) => (string) DUPLICANTS.DISEASES.INFECTIONSOURCES.INTERNAL_TEMPERATURE));
      component.AddTrigger(GameHashes.TooColdSickness, new string[1]
      {
        "ColdSickness"
      }, (SicknessTrigger.SourceCallback) ((s, t) => (string) DUPLICANTS.DISEASES.INFECTIONSOURCES.INTERNAL_TEMPERATURE));
    })).Update("UpdateTemperature", (System.Action<TemperatureMonitor.Instance, float>) ((smi, dt) => smi.UpdateTemperature(dt)), UpdateRate.SIM_200ms, false);
    this.homeostatic.Transition(this.hyperthermic_pre, (StateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.IsHyperthermic()), UpdateRate.SIM_200ms).Transition(this.hypothermic_pre, (StateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.IsHypothermic()), UpdateRate.SIM_200ms).TriggerOnEnter(GameHashes.OptimalTemperatureAchieved, (Func<TemperatureMonitor.Instance, object>) null);
    this.hyperthermic_pre.Enter((StateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi =>
    {
      smi.master.Trigger(-1174019026, (object) smi.master.gameObject);
      smi.GoTo((StateMachine.BaseState) this.hyperthermic);
    }));
    this.hypothermic_pre.Enter((StateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi =>
    {
      smi.master.Trigger(54654253, (object) smi.master.gameObject);
      smi.GoTo((StateMachine.BaseState) this.hypothermic);
    }));
    this.hyperthermic.Transition(this.homeostatic, (StateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.IsHyperthermic()), UpdateRate.SIM_200ms).ToggleUrge(Db.Get().Urges.CoolDown);
    this.hypothermic.Transition(this.homeostatic, (StateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.IsHypothermic()), UpdateRate.SIM_200ms).ToggleUrge(Db.Get().Urges.WarmUp);
    this.deathcold.Enter("KillCold", (StateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.KillCold())).TriggerOnEnter(GameHashes.TooColdFatal, (Func<TemperatureMonitor.Instance, object>) null);
    this.deathhot.Enter("KillHot", (StateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.KillHot())).TriggerOnEnter(GameHashes.TooHotFatal, (Func<TemperatureMonitor.Instance, object>) null);
  }

  public class Instance : GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public float HypothermiaThreshold = 307.15f;
    public float HyperthermiaThreshold = 313.15f;
    public float FatalHypothermia = 305.15f;
    public float FatalHyperthermia = 315.15f;
    public AmountInstance temperature;
    public PrimaryElement primaryElement;
    private Navigator navigator;
    private SafetyQuery warmUpQuery;
    private SafetyQuery coolDownQuery;
    public float averageTemperature;

    public Instance(IStateMachineTarget master)
      : base(master)
    {
      this.primaryElement = this.GetComponent<PrimaryElement>();
      this.temperature = Db.Get().Amounts.Temperature.Lookup(this.gameObject);
      this.warmUpQuery = new SafetyQuery(Game.Instance.safetyConditions.WarmUpChecker, this.GetComponent<KMonoBehaviour>(), int.MaxValue);
      this.coolDownQuery = new SafetyQuery(Game.Instance.safetyConditions.CoolDownChecker, this.GetComponent<KMonoBehaviour>(), int.MaxValue);
      this.navigator = this.GetComponent<Navigator>();
    }

    public void UpdateTemperature(float dt)
    {
      this.smi.averageTemperature *= (float) (1.0 - (double) dt / 4.0);
      this.smi.averageTemperature += this.smi.primaryElement.Temperature * (dt / 4f);
      double num = (double) this.smi.temperature.SetValue(this.smi.averageTemperature);
    }

    public bool IsHyperthermic()
    {
      return (double) this.temperature.value > (double) this.HyperthermiaThreshold;
    }

    public bool IsHypothermic()
    {
      return (double) this.temperature.value < (double) this.HypothermiaThreshold;
    }

    public bool IsFatalHypothermic()
    {
      return (double) this.temperature.value < (double) this.FatalHypothermia;
    }

    public bool IsFatalHyperthermic()
    {
      return (double) this.temperature.value > (double) this.FatalHyperthermia;
    }

    public void KillHot()
    {
      this.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Overheating);
    }

    public void KillCold()
    {
      this.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Frozen);
    }

    public float ExtremeTemperatureDelta()
    {
      if ((double) this.temperature.value > (double) this.HyperthermiaThreshold)
        return this.temperature.value - this.HyperthermiaThreshold;
      if ((double) this.temperature.value < (double) this.HypothermiaThreshold)
        return this.temperature.value - this.HypothermiaThreshold;
      return 0.0f;
    }

    public float IdealTemperatureDelta()
    {
      return this.temperature.value - 310.15f;
    }

    public int GetWarmUpCell()
    {
      return this.sm.warmUpCell.Get(this.smi);
    }

    public int GetCoolDownCell()
    {
      return this.sm.coolDownCell.Get(this.smi);
    }

    public void UpdateWarmUpCell()
    {
      this.warmUpQuery.Reset();
      this.navigator.RunQuery((PathFinderQuery) this.warmUpQuery);
      this.sm.warmUpCell.Set(this.warmUpQuery.GetResultCell(), this.smi);
    }

    public void UpdateCoolDownCell()
    {
      this.coolDownQuery.Reset();
      this.navigator.RunQuery((PathFinderQuery) this.coolDownQuery);
      this.sm.coolDownCell.Set(this.coolDownQuery.GetResultCell(), this.smi);
    }
  }
}
