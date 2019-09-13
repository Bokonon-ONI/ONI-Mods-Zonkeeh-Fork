﻿// Decompiled with JetBrains decompiler
// Type: SelectableTextStyler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

public class SelectableTextStyler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IEventSystemHandler
{
  [SerializeField]
  private LocText target;
  [SerializeField]
  private SelectableTextStyler.State state;
  [SerializeField]
  private TextStyleSetting normalNormal;
  [SerializeField]
  private TextStyleSetting normalHovered;

  private void Start()
  {
    this.SetState(this.state, SelectableTextStyler.HoverState.Normal);
  }

  private void SetState(
    SelectableTextStyler.State state,
    SelectableTextStyler.HoverState hover_state)
  {
    if (state == SelectableTextStyler.State.Normal)
    {
      switch (hover_state)
      {
        case SelectableTextStyler.HoverState.Normal:
          this.target.textStyleSetting = this.normalNormal;
          break;
        case SelectableTextStyler.HoverState.Hovered:
          this.target.textStyleSetting = this.normalHovered;
          break;
      }
    }
    this.target.ApplySettings();
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    this.SetState(this.state, SelectableTextStyler.HoverState.Hovered);
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    this.SetState(this.state, SelectableTextStyler.HoverState.Normal);
  }

  public void OnPointerClick(PointerEventData eventData)
  {
    this.SetState(this.state, SelectableTextStyler.HoverState.Normal);
  }

  public enum State
  {
    Normal,
  }

  public enum HoverState
  {
    Normal,
    Hovered,
  }
}
