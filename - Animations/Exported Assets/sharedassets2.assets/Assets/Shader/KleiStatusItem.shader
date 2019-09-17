Shader "Klei/StatusItem" {
	Properties {
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent+0" "RenderType" = "Transparent" }
		Pass {
			Tags { "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent+0" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 8725
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
					// TEXCOORD                 1   xy          2     NONE   float   xy  
					// COLOR                    0   xyzw        3     NONE   float   xyzw
					//
					//
					// Output signature:
					//
					// Name                 Index   Mask Register SysValue  Format   Used
					// -------------------- ----- ------ -------- -------- ------- ------
					// SV_POSITION              0   xyzw        0      POS   float   xyzw
					// TEXCOORD                 0   xy          1     NONE   float   xy  
					// TEXCOORD                 1     zw        1     NONE   float     zw
					// COLOR                    0   xyzw        2     NONE   float   xyzw
					//
					vs_4_0
					dcl_constantbuffer CB0[6], immediateIndexed
					dcl_constantbuffer CB1[4], immediateIndexed
					dcl_constantbuffer CB2[21], immediateIndexed
					dcl_input v0.xyz
					dcl_input v1.xy
					dcl_input v2.xy
					dcl_input v3.xyzw
					dcl_output_siv o0.xyzw, position
					dcl_output o1.xy
					dcl_output o1.zw
					dcl_output o2.xyzw
					dcl_temps 2
					mul r0.xy, v0.xyxx, cb0[5].xxxx
					mul r1.xyzw, r0.yyyy, cb1[1].xyzw
					mad r0.xyzw, cb1[0].xyzw, r0.xxxx, r1.xyzw
					mad r0.xyzw, cb1[2].xyzw, v0.zzzz, r0.xyzw
					add r0.xyzw, r0.xyzw, cb1[3].xyzw
					mul r1.xyzw, r0.yyyy, cb2[18].xyzw
					mad r1.xyzw, cb2[17].xyzw, r0.xxxx, r1.xyzw
					mad r1.xyzw, cb2[19].xyzw, r0.zzzz, r1.xyzw
					mad o0.xyzw, cb2[20].xyzw, r0.wwww, r1.xyzw
					mov o1.xy, v1.xyxx
					mov o1.zw, v2.xxxy
					mov o2.xyzw, v3.xyzw
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
					// SV_POSITION              0   xyzw        0      POS   float       
					// TEXCOORD                 0   xy          1     NONE   float   xy  
					// TEXCOORD                 1     zw        1     NONE   float     z 
					// COLOR                    0   xyzw        2     NONE   float   xyzw
					//
					//
					// Output signature:
					//
					// Name                 Index   Mask Register SysValue  Format   Used
					// -------------------- ----- ------ -------- -------- ------- ------
					// SV_Target                0   xyzw        0   TARGET   float   xyzw
					// SV_Target                1   xyzw        1   TARGET   float   xyzw
					// SV_Target                2   xyzw        2   TARGET   float   xyzw
					//
					ps_4_0
					dcl_sampler s0, mode_default
					dcl_sampler s1, mode_default
					dcl_sampler s2, mode_default
					dcl_sampler s3, mode_default
					dcl_sampler s4, mode_default
					dcl_sampler s5, mode_default
					dcl_sampler s6, mode_default
					dcl_sampler s7, mode_default
					dcl_sampler s8, mode_default
					dcl_sampler s9, mode_default
					dcl_sampler s10, mode_default
					dcl_resource_texture2d (float,float,float,float) t0
					dcl_resource_texture2d (float,float,float,float) t1
					dcl_resource_texture2d (float,float,float,float) t2
					dcl_resource_texture2d (float,float,float,float) t3
					dcl_resource_texture2d (float,float,float,float) t4
					dcl_resource_texture2d (float,float,float,float) t5
					dcl_resource_texture2d (float,float,float,float) t6
					dcl_resource_texture2d (float,float,float,float) t7
					dcl_resource_texture2d (float,float,float,float) t8
					dcl_resource_texture2d (float,float,float,float) t9
					dcl_resource_texture2d (float,float,float,float) t10
					dcl_input_ps linear v1.xy
					dcl_input_ps linear v1.z
					dcl_input_ps linear v2.xyzw
					dcl_output o0.xyzw
					dcl_output o1.xyzw
					dcl_output o2.xyzw
					dcl_temps 2
					lt r0.x, v1.z, l(1.000000)
					if_nz r0.x
					  sample r0.xyzw, v1.xyxx, t0.wxyz, s0
					  mov r1.x, l(-1)
					else 
					  lt r1.y, v1.z, l(2.000000)
					  if_nz r1.y
					    sample r0.xyzw, v1.xyxx, t1.wxyz, s1
					    mov r1.x, l(-1)
					  else 
					    lt r1.y, v1.z, l(3.000000)
					    if_nz r1.y
					      sample r0.xyzw, v1.xyxx, t2.wxyz, s2
					      mov r1.x, l(-1)
					    else 
					      lt r1.y, v1.z, l(4.000000)
					      if_nz r1.y
					        sample r0.xyzw, v1.xyxx, t3.wxyz, s3
					        mov r1.x, l(-1)
					      else 
					        lt r1.y, v1.z, l(5.000000)
					        if_nz r1.y
					          sample r0.xyzw, v1.xyxx, t4.wxyz, s4
					          mov r1.x, l(-1)
					        else 
					          lt r1.y, v1.z, l(6.000000)
					          if_nz r1.y
					            sample r0.xyzw, v1.xyxx, t5.wxyz, s5
					            mov r1.x, l(-1)
					          else 
					            lt r1.y, v1.z, l(7.000000)
					            if_nz r1.y
					              sample r0.xyzw, v1.xyxx, t6.wxyz, s6
					              mov r1.x, l(-1)
					            else 
					              lt r1.y, v1.z, l(8.000000)
					              if_nz r1.y
					                sample r0.xyzw, v1.xyxx, t7.wxyz, s7
					                mov r1.x, l(-1)
					              else 
					                lt r1.y, v1.z, l(9.000000)
					                if_nz r1.y
					                  sample r0.xyzw, v1.xyxx, t8.wxyz, s8
					                  mov r1.x, l(-1)
					                else 
					                  lt r1.y, v1.z, l(10.000000)
					                  if_nz r1.y
					                    sample r0.xyzw, v1.xyxx, t9.wxyz, s9
					                    mov r1.x, l(-1)
					                  else 
					                    lt r1.x, v1.z, l(11.000000)
					                    if_nz r1.x
					                      sample r0.xyzw, v1.xyxx, t10.wxyz, s10
					                    endif 
					                  endif 
					                endif 
					              endif 
					            endif 
					          endif 
					        endif 
					      endif 
					    endif 
					  endif 
					endif 
					if_z r1.x
					  sample r0.xyzw, v1.xyxx, t0.wxyz, s0
					endif 
					mul o0.w, r0.x, v2.w
					mov o0.xyz, v2.xyzx
					mov o1.xyzw, l(0,0,0,0)
					mov o2.xyzw, l(0,0,0,0)
					ret 
					// Approximately 0 instruction slots used"
				}
			}
		}
	}
}