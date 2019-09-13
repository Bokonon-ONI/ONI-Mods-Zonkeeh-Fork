﻿// Decompiled with JetBrains decompiler
// Type: StressBehaviourMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using TUNING;

public class StressBehaviourMonitor : GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance>
{
  public StateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.FloatParameter timeInTierTwoStressResponse;
  public GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State satisfied;
  public StressBehaviourMonitor.StressedState stressed;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    this.root.EventTransition(GameHashes.Died, (GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State) null, (StateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) null);
    this.satisfied.EventTransition(GameHashes.Stressed, (GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State) this.stressed, (StateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi =>
    {
      if (smi.gameObject.GetSMI<StressMonitor.Instance>() != null)
        return smi.gameObject.GetSMI<StressMonitor.Instance>().IsStressed();
      return false;
    }));
    this.stressed.DefaultState((GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State) this.stressed.tierOne).ToggleExpression(Db.Get().Expressions.Unhappy, (Func<StressBehaviourMonitor.Instance, bool>) null).ToggleAnims((Func<StressBehaviourMonitor.Instance, HashedString>) (smi => (HashedString) smi.tierOneLocoAnim)).Transition(this.satisfied, (StateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi =>
    {
      if (smi.gameObject.GetSMI<StressMonitor.Instance>() != null)
        return !smi.gameObject.GetSMI<StressMonitor.Instance>().IsStressed();
      return false;
    }), UpdateRate.SIM_200ms);
    this.stressed.tierOne.DefaultState(this.stressed.tierOne.actingOut).EventTransition(GameHashes.StressedHadEnough, (GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State) this.stressed.tierTwo, (StateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) null);
    this.stressed.tierOne.actingOut.ToggleChore((Func<StressBehaviourMonitor.Instance, Chore>) (smi => smi.CreateTierOneStressChore()), this.stressed.tierOne.reprieve);
    this.stressed.tierOne.reprieve.ScheduleGoTo(30f, (StateMachine.BaseState) this.stressed.tierOne.actingOut);
    double num1;
    double num2;
    this.stressed.tierTwo.DefaultState(this.stressed.tierTwo.actingOut).Update((System.Action<StressBehaviourMonitor.Instance, float>) ((smi, dt) => num1 = (double) smi.sm.timeInTierTwoStressResponse.Set(smi.sm.timeInTierTwoStressResponse.Get(smi) + dt, smi)), UpdateRate.SIM_200ms, false).Exit("ResetStress", (StateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => num2 = (double) Db.Get().Amounts.Stress.Lookup(smi.gameObject).SetValue(STRESS.ACTING_OUT_RESET)));
    this.stressed.tierTwo.actingOut.ToggleChore((Func<StressBehaviourMonitor.Instance, Chore>) (smi => smi.CreateTierTwoStressChore()), this.stressed.tierTwo.reprieve);
    this.stressed.tierTwo.reprieve.ToggleChore((Func<StressBehaviourMonitor.Instance, Chore>) (smi => (Chore) new StressIdleChore(smi.master)), (GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State) null).Enter((StateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi =>
    {
      if ((double) smi.sm.timeInTierTwoStressResponse.Get(smi) < 150.0)
        return;
      double num3 = (double) smi.sm.timeInTierTwoStressResponse.Set(0.0f, smi);
      smi.GoTo((StateMachine.BaseState) this.stressed);
    })).ScheduleGoTo((Func<StressBehaviourMonitor.Instance, float>) (smi => smi.tierTwoReprieveDuration), (StateMachine.BaseState) this.stressed.tierTwo);
  }

  public class StressedState : GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State
  {
    public StressBehaviourMonitor.TierOneStates tierOne;
    public StressBehaviourMonitor.TierTwoStates tierTwo;
  }

  public class TierOneStates : GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State actingOut;
    public GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State reprieve;
  }

  public class TierTwoStates : GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State actingOut;
    public GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State reprieve;
  }

  public class Instance : GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public string tierOneLocoAnim = string.Empty;
    public Func<ChoreProvider, Chore> tierOneStressChoreCreator;
    public Func<ChoreProvider, Chore> tierTwoStressChoreCreator;
    public float tierTwoReprieveDuration;

    public Instance(
      IStateMachineTarget master,
      Func<ChoreProvider, Chore> tier_one_stress_chore_creator,
      Func<ChoreProvider, Chore> tier_two_stress_chore_creator,
      string tier_one_loco_anim,
      float tier_two_reprieve_duration = 3f)
      : base(master)
    {
      this.tierOneLocoAnim = tier_one_loco_anim;
      this.tierTwoReprieveDuration = tier_two_reprieve_duration;
      this.tierOneStressChoreCreator = tier_one_stress_chore_creator;
      this.tierTwoStressChoreCreator = tier_two_stress_chore_creator;
    }

    public Chore CreateTierOneStressChore()
    {
      return this.tierOneStressChoreCreator(this.GetComponent<ChoreProvider>());
    }

    public Chore CreateTierTwoStressChore()
    {
      return this.tierTwoStressChoreCreator(this.GetComponent<ChoreProvider>());
    }
  }
}
