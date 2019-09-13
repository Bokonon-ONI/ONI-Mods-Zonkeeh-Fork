﻿// Decompiled with JetBrains decompiler
// Type: LogicPressureSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class LogicPressureSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
  private static readonly EventSystem.IntraObjectHandler<LogicPressureSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicPressureSensor>((System.Action<LogicPressureSensor, object>) ((component, data) => component.OnCopySettings(data)));
  [SerializeField]
  [Serialize]
  private bool activateAboveThreshold = true;
  public float rangeMax = 1f;
  public Element.State desiredState = Element.State.Gas;
  private float[] samples = new float[8];
  [SerializeField]
  [Serialize]
  private float threshold;
  private bool wasOn;
  public float rangeMin;
  private const int WINDOW_SIZE = 8;
  private int sampleIdx;
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<LogicPressureSensor>(-905833192, LogicPressureSensor.OnCopySettingsDelegate);
  }

  private void OnCopySettings(object data)
  {
    LogicPressureSensor component = ((GameObject) data).GetComponent<LogicPressureSensor>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.Threshold = component.Threshold;
    this.ActivateAboveThreshold = component.ActivateAboveThreshold;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.OnToggle += new System.Action<bool>(this.OnSwitchToggled);
    this.UpdateLogicCircuit();
    this.UpdateVisualState(true);
    this.wasOn = this.switchedOn;
  }

  public void Sim200ms(float dt)
  {
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    if (this.sampleIdx < 8)
    {
      this.samples[this.sampleIdx] = !Grid.Element[cell].IsState(this.desiredState) ? 0.0f : Grid.Mass[cell];
      ++this.sampleIdx;
    }
    else
    {
      this.sampleIdx = 0;
      float currentValue = this.CurrentValue;
      if (this.activateAboveThreshold)
      {
        if (((double) currentValue <= (double) this.threshold || this.IsSwitchedOn) && ((double) currentValue > (double) this.threshold || !this.IsSwitchedOn))
          return;
        this.Toggle();
      }
      else
      {
        if (((double) currentValue <= (double) this.threshold || !this.IsSwitchedOn) && ((double) currentValue > (double) this.threshold || this.IsSwitchedOn))
          return;
        this.Toggle();
      }
    }
  }

  private void OnSwitchToggled(bool toggled_on)
  {
    this.UpdateLogicCircuit();
    this.UpdateVisualState(false);
  }

  public float Threshold
  {
    get
    {
      return this.threshold;
    }
    set
    {
      this.threshold = value;
    }
  }

  public bool ActivateAboveThreshold
  {
    get
    {
      return this.activateAboveThreshold;
    }
    set
    {
      this.activateAboveThreshold = value;
    }
  }

  public float CurrentValue
  {
    get
    {
      float num = 0.0f;
      for (int index = 0; index < 8; ++index)
        num += this.samples[index];
      return num / 8f;
    }
  }

  public float RangeMin
  {
    get
    {
      return this.rangeMin;
    }
  }

  public float RangeMax
  {
    get
    {
      return this.rangeMax;
    }
  }

  public float GetRangeMinInputField()
  {
    if (this.desiredState == Element.State.Gas)
      return this.rangeMin * 1000f;
    return this.rangeMin;
  }

  public float GetRangeMaxInputField()
  {
    if (this.desiredState == Element.State.Gas)
      return this.rangeMax * 1000f;
    return this.rangeMax;
  }

  public LocString ThresholdValueName
  {
    get
    {
      return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE;
    }
  }

  public string AboveToolTip
  {
    get
    {
      return (string) UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_ABOVE;
    }
  }

  public string BelowToolTip
  {
    get
    {
      return (string) UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_BELOW;
    }
  }

  public string Format(float value, bool units)
  {
    GameUtil.MetricMassFormat massFormat = this.desiredState != Element.State.Gas ? GameUtil.MetricMassFormat.Kilogram : GameUtil.MetricMassFormat.Gram;
    float mass = value;
    bool includeSuffix = units;
    return GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, massFormat, includeSuffix, "{0:0.#}");
  }

  public float ProcessedSliderValue(float input)
  {
    input = this.desiredState != Element.State.Gas ? Mathf.Round(input) : Mathf.Round(input * 1000f) / 1000f;
    return input;
  }

  public float ProcessedInputValue(float input)
  {
    if (this.desiredState == Element.State.Gas)
      input /= 1000f;
    return input;
  }

  public LocString ThresholdValueUnits()
  {
    return GameUtil.GetCurrentMassUnit(this.desiredState == Element.State.Gas);
  }

  public LocString Title
  {
    get
    {
      return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TITLE;
    }
  }

  public ThresholdScreenLayoutType LayoutType
  {
    get
    {
      return ThresholdScreenLayoutType.SliderBar;
    }
  }

  public int IncrementScale
  {
    get
    {
      return 1;
    }
  }

  public NonLinearSlider.Range[] GetRanges
  {
    get
    {
      return NonLinearSlider.GetDefaultRange(this.RangeMax);
    }
  }

  private void UpdateLogicCircuit()
  {
    this.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, !this.switchedOn ? 0 : 1);
  }

  private void UpdateVisualState(bool force = false)
  {
    if (this.wasOn == this.switchedOn && !force)
      return;
    this.wasOn = this.switchedOn;
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    component.Play((HashedString) (!this.switchedOn ? "on_pst" : "on_pre"), KAnim.PlayMode.Once, 1f, 0.0f);
    component.Queue((HashedString) (!this.switchedOn ? "off" : "on"), KAnim.PlayMode.Once, 1f, 0.0f);
  }

  protected override void UpdateSwitchStatus()
  {
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, !this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusInactive : Db.Get().BuildingStatusItems.LogicSensorStatusActive, (object) null);
  }
}
