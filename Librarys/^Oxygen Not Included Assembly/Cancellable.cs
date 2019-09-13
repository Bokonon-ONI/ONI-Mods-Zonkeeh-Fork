﻿// Decompiled with JetBrains decompiler
// Type: Cancellable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

[SkipSaveFileSerialization]
public class Cancellable : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<Cancellable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Cancellable>((System.Action<Cancellable, object>) ((component, data) => component.OnCancel(data)));

  protected override void OnPrefabInit()
  {
    this.Subscribe<Cancellable>(2127324410, Cancellable.OnCancelDelegate);
  }

  protected virtual void OnCancel(object data)
  {
    this.DeleteObject();
  }
}
