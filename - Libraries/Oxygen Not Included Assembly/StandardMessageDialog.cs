﻿// Decompiled with JetBrains decompiler
// Type: StandardMessageDialog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class StandardMessageDialog : MessageDialog
{
  [SerializeField]
  private LocText description;
  private Message message;

  public override bool CanDisplay(Message message)
  {
    return typeof (Message).IsAssignableFrom(message.GetType());
  }

  public override void SetMessage(Message base_message)
  {
    this.message = base_message;
    this.description.text = this.message.GetMessageBody();
  }

  public override void OnClickAction()
  {
  }
}