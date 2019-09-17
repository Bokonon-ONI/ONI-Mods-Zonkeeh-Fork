Shader "Klei/PostFX/FogOfWar" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Pass {
			GpuProgramID 59581
			Program "vp" {
				SubProgram "d3d11 " {
					"!!vs_4_0
					//
					// Generated by Microsoft (R) D3D Shader Disassembler
					//
					//
					// Input signature:
					//
					// Name                 Index   Mask Register SysValue  Format   Used
					// -------------------- ----- ------ -------- -------- ------- ------
					// POSITION                 0   xyzw        0     NONE   float   xyz 
					// TEXCOORD                 0   xy          1     NONE   float   xy  
					//
					//
					// Output signature:
					//
					// Name                 Index   Mask Register SysValue  Format   Used
					// -------------------- ----- ------ -------- -------- ------- ------
					// SV_Position              0   xyzw        0      POS   float   xyzw
					// TEXCOORD                 0   xyzw        1     NONE   float   xyzw
					//
					vs_4_0
					dcl_constantbuffer CB0[11], immediateIndexed
					dcl_constantbuffer CB1[4], immediateIndexed
					dcl_constantbuffer CB2[21], immediateIndexed
					dcl_input v0.xyz
					dcl_input v1.xy
					dcl_output_siv o0.xyzw, position
					dcl_output o1.xyzw
					dcl_temps 2
					mul r0.xyzw, v0.yyyy, cb1[1].xyzw
					mad r0.xyzw, cb1[0].xyzw, v0.xxxx, r0.xyzw
					mad r0.xyzw, cb1[2].xyzw, v0.zzzz, r0.xyzw
					add r0.xyzw, r0.xyzw, cb1[3].xyzw
					mul r1.xyzw, r0.yyyy, cb2[18].xyzw
					mad r1.xyzw, cb2[17].xyzw, r0.xxxx, r1.xyzw
					mad r1.xyzw, cb2[19].xyzw, r0.zzzz, r1.xyzw
					mad o0.xyzw, cb2[20].xyzw, r0.wwww, r1.xyzw
					mad o1.zw, v1.xxxy, cb0[10].zzzw, cb0[10].xxxy
					mov o1.xy, v1.xyxx
					ret 
					// Approximately 0 instruction slots used"
				}
			}
			Program "fp" {
				SubProgram "d3d11 " {
					"!!ps_4_0
					//
					// Generated by Microsoft (R) D3D Shader Disassembler
					//
					//
					// Input signature:
					//
					// Name                 Index   Mask Register SysValue  Format   Used
					// -------------------- ----- ------ -------- -------- ------- ------
					// SV_Position              0   xyzw        0      POS   float       
					// TEXCOORD                 0   xyzw        1     NONE   float   xyzw
					//
					//
					// Output signature:
					//
					// Name                 Index   Mask Register SysValue  Format   Used
					// -------------------- ----- ------ -------- -------- ------- ------
					// SV_Target                0   xyzw        0   TARGET   float   xyzw
					//
					ps_4_0
					dcl_constantbuffer CB0[12], immediateIndexed
					dcl_sampler s0, mode_default
					dcl_sampler s1, mode_default
					dcl_resource_texture2d (float,float,float,float) t0
					dcl_resource_texture2d (float,float,float,float) t1
					dcl_input_ps linear v1.xyzw
					dcl_output o0.xyzw
					dcl_temps 2
					mad r0.x, -cb0[6].z, l(5.000000), l(1.000000)
					add r0.y, -r0.x, l(1.000000)
					add r0.x, -r0.x, v1.z
					div r0.y, l(1.000000, 1.000000, 1.000000, 1.000000), r0.y
					mul_sat r0.x, r0.y, r0.x
					mad r0.y, r0.x, l(-2.000000), l(3.000000)
					mul r0.x, r0.x, r0.x
					mad r0.x, -r0.y, r0.x, l(1.000000)
					sample r1.xyzw, v1.zwzz, t1.xyzw, s1
					add_sat r0.y, r1.w, cb0[11].x
					sample r1.xyzw, v1.xyxx, t0.xyzw, s0
					mul r0.yzw, r0.yyyy, r1.xxyz
					add r1.xy, cb0[6].zwzz, cb0[6].zwzz
					div r1.xy, l(1.000000, 1.000000, 1.000000, 1.000000), r1.xyxx
					mul_sat r1.xy, r1.xyxx, v1.zwzz
					mad r1.zw, r1.xxxy, l(0.000000, 0.000000, -2.000000, -2.000000), l(0.000000, 0.000000, 3.000000, 3.000000)
					mul r1.xy, r1.xyxx, r1.xyxx
					mul r1.xy, r1.xyxx, r1.zwzz
					mul r0.yzw, r0.yyzw, r1.xxxx
					mul r0.xyz, r0.xxxx, r0.yzwy
					mul o0.xyz, r1.yyyy, r0.xyzx
					mov o0.w, l(1.000000)
					ret 
					// Approximately 0 instruction slots used"
				}
			}
		}
	}
}