﻿// Decompiled with JetBrains decompiler
// Type: Hud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Hud : KScreen
{
  public override void OnKeyDown(KButtonEvent e)
  {
    if (!e.TryConsume(Action.Help))
      return;
    GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ControlsScreen.gameObject, (GameObject) null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay);
  }
}
