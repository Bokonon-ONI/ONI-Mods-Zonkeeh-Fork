﻿// Decompiled with JetBrains decompiler
// Type: EntityPrefabs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class EntityPrefabs : KMonoBehaviour
{
  public GameObject SelectMarker;
  public GameObject ForegroundLayer;

  public static EntityPrefabs Instance { get; private set; }

  public static void DestroyInstance()
  {
    EntityPrefabs.Instance = (EntityPrefabs) null;
  }

  protected override void OnPrefabInit()
  {
    EntityPrefabs.Instance = this;
  }
}
