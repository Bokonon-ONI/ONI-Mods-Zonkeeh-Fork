﻿// Decompiled with JetBrains decompiler
// Type: Harmony.UpgradeToLatestVersion
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

using System;

namespace Harmony
{
  [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method)]
  internal class UpgradeToLatestVersion : Attribute
  {
    public int version;

    public UpgradeToLatestVersion(int version)
    {
      this.version = version;
    }
  }
}
