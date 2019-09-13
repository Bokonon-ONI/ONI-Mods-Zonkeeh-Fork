﻿// Decompiled with JetBrains decompiler
// Type: Rendering.World.Mask
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace Rendering.World
{
  public struct Mask
  {
    private TextureAtlas atlas;
    private int texture_idx;
    private bool transpose;
    private bool flip_x;
    private bool flip_y;
    private int atlas_offset;
    private const int TILES_PER_SET = 4;

    public unsafe Mask(
      TextureAtlas atlas,
      int texture_idx,
      bool transpose,
      bool flip_x,
      bool flip_y,
      bool is_opaque)
    {
      *(Mask*) ref this = new Mask();
      this.atlas = atlas;
      this.texture_idx = texture_idx;
      this.transpose = transpose;
      this.flip_x = flip_x;
      this.flip_y = flip_y;
      this.atlas_offset = 0;
      this.IsOpaque = is_opaque;
      this.Refresh();
    }

    public Vector2 UV0 { get; private set; }

    public Vector2 UV1 { get; private set; }

    public Vector2 UV2 { get; private set; }

    public Vector2 UV3 { get; private set; }

    public bool IsOpaque { get; private set; }

    public void SetOffset(int offset)
    {
      this.atlas_offset = offset;
      this.Refresh();
    }

    public void Refresh()
    {
      int num1 = this.atlas_offset * 4 + this.atlas_offset;
      if (num1 + this.texture_idx >= this.atlas.items.Length)
        num1 = 0;
      Vector4 uvBox = this.atlas.items[num1 + this.texture_idx].uvBox;
      Vector2 zero1 = Vector2.zero;
      Vector2 zero2 = Vector2.zero;
      Vector2 zero3 = Vector2.zero;
      Vector2 zero4 = Vector2.zero;
      if (this.transpose)
      {
        float num2 = uvBox.x;
        float num3 = uvBox.z;
        if (this.flip_x)
        {
          num2 = uvBox.z;
          num3 = uvBox.x;
        }
        zero1.x = num2;
        zero2.x = num2;
        zero3.x = num3;
        zero4.x = num3;
        float num4 = uvBox.y;
        float num5 = uvBox.w;
        if (this.flip_y)
        {
          num4 = uvBox.w;
          num5 = uvBox.y;
        }
        zero1.y = num4;
        zero2.y = num5;
        zero3.y = num4;
        zero4.y = num5;
      }
      else
      {
        float num2 = uvBox.x;
        float num3 = uvBox.z;
        if (this.flip_x)
        {
          num2 = uvBox.z;
          num3 = uvBox.x;
        }
        zero1.x = num2;
        zero2.x = num3;
        zero3.x = num2;
        zero4.x = num3;
        float num4 = uvBox.y;
        float num5 = uvBox.w;
        if (this.flip_y)
        {
          num4 = uvBox.w;
          num5 = uvBox.y;
        }
        zero1.y = num5;
        zero2.y = num5;
        zero3.y = num4;
        zero4.y = num4;
      }
      this.UV0 = zero1;
      this.UV1 = zero2;
      this.UV2 = zero3;
      this.UV3 = zero4;
    }
  }
}
