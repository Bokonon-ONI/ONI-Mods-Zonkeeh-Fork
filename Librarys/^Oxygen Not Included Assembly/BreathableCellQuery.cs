﻿// Decompiled with JetBrains decompiler
// Type: BreathableCellQuery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class BreathableCellQuery : PathFinderQuery
{
  private OxygenBreather breather;

  public BreathableCellQuery Reset(Brain brain)
  {
    this.breather = brain.GetComponent<OxygenBreather>();
    return this;
  }

  public override bool IsMatch(int cell, int parent_cell, int cost)
  {
    return this.breather.IsBreathableElementAtCell(cell, (CellOffset[]) null);
  }
}
