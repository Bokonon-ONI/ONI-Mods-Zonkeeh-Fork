﻿// Decompiled with JetBrains decompiler
// Type: ScheduleBlock
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;

[Serializable]
public class ScheduleBlock
{
  [Serialize]
  public string name;
  [Serialize]
  public List<ScheduleBlockType> allowed_types;
  [Serialize]
  private string _groupId;

  public ScheduleBlock(string name, List<ScheduleBlockType> allowed_types, string groupId)
  {
    this.name = name;
    this.allowed_types = allowed_types;
    this._groupId = groupId;
  }

  public string GroupId
  {
    set
    {
      this._groupId = value;
    }
    get
    {
      if (this._groupId == null)
        this._groupId = Db.Get().ScheduleGroups.FindGroupForScheduleTypes(this.allowed_types).Id;
      return this._groupId;
    }
  }

  public bool IsAllowed(ScheduleBlockType type)
  {
    if (this.allowed_types != null)
    {
      foreach (ScheduleBlockType allowedType in this.allowed_types)
      {
        if (type.IdHash == allowedType.IdHash)
          return true;
      }
    }
    return false;
  }
}
