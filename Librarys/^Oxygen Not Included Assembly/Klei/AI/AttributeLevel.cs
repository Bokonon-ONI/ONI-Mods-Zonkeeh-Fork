﻿// Decompiled with JetBrains decompiler
// Type: Klei.AI.AttributeLevel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace Klei.AI
{
  public class AttributeLevel
  {
    public float experience;
    public int level;
    public AttributeInstance attribute;
    public AttributeModifier modifier;
    public Notification notification;

    public AttributeLevel(AttributeInstance attribute)
    {
      string name = (string) MISC.NOTIFICATIONS.LEVELUP.NAME;
      HashedString invalid = HashedString.Invalid;
      // ISSUE: reference to a compiler-generated field
      if (AttributeLevel.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AttributeLevel.\u003C\u003Ef__mg\u0024cache0 = new Func<List<Notification>, object, string>(AttributeLevel.OnLevelUpTooltip);
      }
      // ISSUE: reference to a compiler-generated field
      Func<List<Notification>, object, string> fMgCache0 = AttributeLevel.\u003C\u003Ef__mg\u0024cache0;
      this.notification = new Notification(name, NotificationType.Good, invalid, fMgCache0, (object) null, true, 0.0f, (Notification.ClickCallback) null, (object) null, (Transform) null);
      this.attribute = attribute;
    }

    public int GetLevel()
    {
      return this.level;
    }

    public void Apply(AttributeLevels levels)
    {
      Attributes attributes = levels.GetAttributes();
      if (this.modifier != null)
      {
        attributes.Remove(this.modifier);
        this.modifier = (AttributeModifier) null;
      }
      this.modifier = new AttributeModifier(this.attribute.Id, (float) this.GetLevel(), (string) DUPLICANTS.MODIFIERS.SKILLLEVEL.NAME, false, false, true);
      attributes.Add(this.modifier);
    }

    public void SetExperience(float experience)
    {
      this.experience = experience;
    }

    public void SetLevel(int level)
    {
      this.level = level;
    }

    public float GetExperienceForNextLevel()
    {
      float num = (float) ((double) Mathf.Pow((float) this.level / (float) DUPLICANTSTATS.ATTRIBUTE_LEVELING.MAX_GAINED_ATTRIBUTE_LEVEL, DUPLICANTSTATS.ATTRIBUTE_LEVELING.EXPERIENCE_LEVEL_POWER) * (double) DUPLICANTSTATS.ATTRIBUTE_LEVELING.TARGET_MAX_LEVEL_CYCLE * 600.0);
      return (float) ((double) Mathf.Pow(((float) this.level + 1f) / (float) DUPLICANTSTATS.ATTRIBUTE_LEVELING.MAX_GAINED_ATTRIBUTE_LEVEL, DUPLICANTSTATS.ATTRIBUTE_LEVELING.EXPERIENCE_LEVEL_POWER) * (double) DUPLICANTSTATS.ATTRIBUTE_LEVELING.TARGET_MAX_LEVEL_CYCLE * 600.0) - num;
    }

    public float GetPercentComplete()
    {
      return this.experience / this.GetExperienceForNextLevel();
    }

    public void LevelUp(AttributeLevels levels)
    {
      ++this.level;
      this.experience = 0.0f;
      this.Apply(levels);
      this.experience = 0.0f;
      if ((UnityEngine.Object) PopFXManager.Instance != (UnityEngine.Object) null)
        PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, this.attribute.modifier.Name, levels.transform, new Vector3(0.0f, 0.5f, 0.0f), 1.5f, false, false);
      levels.GetComponent<Notifier>().Add(this.notification, string.Format((string) MISC.NOTIFICATIONS.LEVELUP.SUFFIX, (object) this.attribute.modifier.Name, (object) this.level));
      StateMachine.Instance instance = (StateMachine.Instance) new UpgradeFX.Instance((IStateMachineTarget) levels.GetComponent<KMonoBehaviour>(), new Vector3(0.0f, 0.0f, -0.1f));
      ReportManager.Instance.ReportValue(ReportManager.ReportType.LevelUp, 1f, levels.GetProperName(), (string) null);
      instance.StartSM();
      levels.Trigger(-110704193, (object) this.attribute.Id);
    }

    public bool AddExperience(AttributeLevels levels, float experience)
    {
      if (this.level >= DUPLICANTSTATS.ATTRIBUTE_LEVELING.MAX_GAINED_ATTRIBUTE_LEVEL)
        return false;
      this.experience += experience;
      this.experience = Mathf.Max(0.0f, this.experience);
      if ((double) this.experience < (double) this.GetExperienceForNextLevel())
        return false;
      this.LevelUp(levels);
      return true;
    }

    private static string OnLevelUpTooltip(List<Notification> notifications, object data)
    {
      return (string) MISC.NOTIFICATIONS.LEVELUP.TOOLTIP + notifications.ReduceMessages(false);
    }
  }
}
