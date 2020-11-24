// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Slash"
{
	Properties
	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		_Main("Main", 2D) = "white" {}
		_Opacity("Opacity", Float) = 0
		_Dissolve("Dissolve", 2D) = "white" {}
		_SpeedMainTexUVNoise("Speed MainTex U/V + Noise", Vector) = (0,0,0,0)
		_Emissiontex("Emission tex", 2D) = "white" {}
		_Emission("Emission", Float) = 4.5
		_Color0("Color 0", Color) = (0,0,0,0)
		_Desaturation("Desaturation", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}


	Category 
	{
		SubShader
		{
		LOD 0

			Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask RGB
			Cull Off
			Lighting Off 
			ZWrite Off
			ZTest LEqual
			
			Pass {
			
				CGPROGRAM
				
				#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
				#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
				#endif
				
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0
				#pragma multi_compile_instancing
				#pragma multi_compile_particles
				#pragma multi_compile_fog
				#include "UnityShaderVariables.cginc"
				#define ASE_NEEDS_FRAG_COLOR


				#include "UnityCG.cginc"

				struct appdata_t 
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
					float4 ase_texcoord1 : TEXCOORD1;
				};

				struct v2f 
				{
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					UNITY_FOG_COORDS(1)
					#ifdef SOFTPARTICLES_ON
					float4 projPos : TEXCOORD2;
					#endif
					UNITY_VERTEX_INPUT_INSTANCE_ID
					UNITY_VERTEX_OUTPUT_STEREO
					float4 ase_texcoord3 : TEXCOORD3;
				};
				
				
				#if UNITY_VERSION >= 560
				UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
				#else
				uniform sampler2D_float _CameraDepthTexture;
				#endif

				//Don't delete this comment
				// uniform sampler2D_float _CameraDepthTexture;

				uniform sampler2D _MainTex;
				uniform fixed4 _TintColor;
				uniform float4 _MainTex_ST;
				uniform float _InvFade;
				uniform float4 _Color0;
				uniform sampler2D _Emissiontex;
				uniform float4 _Emissiontex_ST;
				uniform float _Desaturation;
				uniform float _Emission;
				uniform sampler2D _Main;
				uniform float4 _Main_ST;
				uniform float _Opacity;
				uniform sampler2D _Dissolve;
				uniform float4 _SpeedMainTexUVNoise;
				uniform float4 _Dissolve_ST;
				float3 HSVToRGB( float3 c )
				{
					float4 K = float4( 1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0 );
					float3 p = abs( frac( c.xxx + K.xyz ) * 6.0 - K.www );
					return c.z * lerp( K.xxx, saturate( p - K.xxx ), c.y );
				}
				
				float3 RGBToHSV(float3 c)
				{
					float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
					float4 p = lerp( float4( c.bg, K.wz ), float4( c.gb, K.xy ), step( c.b, c.g ) );
					float4 q = lerp( float4( p.xyw, c.r ), float4( c.r, p.yzx ), step( p.x, c.r ) );
					float d = q.x - min( q.w, q.y );
					float e = 1.0e-10;
					return float3( abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
				}


				v2f vert ( appdata_t v  )
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					UNITY_TRANSFER_INSTANCE_ID(v, o);
					o.ase_texcoord3 = v.ase_texcoord1;

					v.vertex.xyz +=  float3( 0, 0, 0 ) ;
					o.vertex = UnityObjectToClipPos(v.vertex);
					#ifdef SOFTPARTICLES_ON
						o.projPos = ComputeScreenPos (o.vertex);
						COMPUTE_EYEDEPTH(o.projPos.z);
					#endif
					o.color = v.color;
					o.texcoord = v.texcoord;
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed4 frag ( v2f i  ) : SV_Target
				{
					UNITY_SETUP_INSTANCE_ID( i );
					UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( i );

					#ifdef SOFTPARTICLES_ON
						float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
						float partZ = i.projPos.z;
						float fade = saturate (_InvFade * (sceneZ-partZ));
						i.color.a *= fade;
					#endif

					float2 uv_Emissiontex = i.texcoord.xy * _Emissiontex_ST.xy + _Emissiontex_ST.zw;
					float3 hsvTorgb40 = RGBToHSV( tex2D( _Emissiontex, uv_Emissiontex ).rgb );
					float4 texCoord29 = i.ase_texcoord3;
					texCoord29.xy = i.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
					float3 hsvTorgb41 = HSVToRGB( float3(( hsvTorgb40.x + texCoord29.z ),hsvTorgb40.y,hsvTorgb40.z) );
					float3 desaturateInitialColor43 = hsvTorgb41;
					float desaturateDot43 = dot( desaturateInitialColor43, float3( 0.299, 0.587, 0.114 ));
					float3 desaturateVar43 = lerp( desaturateInitialColor43, desaturateDot43.xxx, _Desaturation );
					float3 smoothstepResult39 = smoothstep( float3( 0,0,0 ) , float3( 0.5377358,0.5377358,0.5377358 ) , desaturateVar43);
					float4 _Vector1 = float4(-0.07,1,-2,1);
					float3 temp_cast_1 = (_Vector1.x).xxx;
					float3 temp_cast_2 = (_Vector1.y).xxx;
					float3 temp_cast_3 = (_Vector1.z).xxx;
					float3 temp_cast_4 = (_Vector1.w).xxx;
					float3 clampResult33 = clamp( (temp_cast_3 + (smoothstepResult39 - temp_cast_1) * (temp_cast_4 - temp_cast_3) / (temp_cast_2 - temp_cast_1)) , float3( 0,0,0 ) , float3( 1,1,1 ) );
					float2 uv_Main = i.texcoord.xy * _Main_ST.xy + _Main_ST.zw;
					float smoothstepResult45 = smoothstep( 0.0 , 0.41 , tex2D( _Main, uv_Main ).a);
					float clampResult5 = clamp( ( smoothstepResult45 * _Opacity ) , 0.0 , 1.0 );
					float2 appendResult21 = (float2(_SpeedMainTexUVNoise.z , _SpeedMainTexUVNoise.w));
					float4 uvs4_Dissolve = i.texcoord;
					uvs4_Dissolve.xy = i.texcoord.xy * _Dissolve_ST.xy + _Dissolve_ST.zw;
					float2 panner22 = ( 1.0 * _Time.y * appendResult21 + uvs4_Dissolve.xy);
					float2 break25 = panner22;
					float2 appendResult26 = (float2(break25.x , ( texCoord29.w + break25.y )));
					float t16 = uvs4_Dissolve.w;
					float w15 = uvs4_Dissolve.z;
					float3 _Vector0 = float3(0.3,0,1);
					float ifLocalVar11 = 0;
					if( ( tex2D( _Dissolve, appendResult26 ).r * t16 ) >= w15 )
					ifLocalVar11 = _Vector0.y;
					else
					ifLocalVar11 = _Vector0.z;
					float4 appendResult6 = (float4(( ( _Color0 * i.color ) + ( float4( clampResult33 , 0.0 ) * _Emission * i.color ) ).rgb , ( i.color.a * clampResult5 * ifLocalVar11 )));
					

					fixed4 col = appendResult6;
					UNITY_APPLY_FOG(i.fogCoord, col);
					return col;
				}
				ENDCG 
			}
		}	
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18707
329;243;1312;1287;2549.975;291.8442;1.283158;True;True
Node;AmplifyShaderEditor.Vector4Node;19;-2270.948,-36.78333;Float;False;Property;_SpeedMainTexUVNoise;Speed MainTex U/V + Noise;3;0;Create;True;0;0;False;0;False;0,0,0,0;0,0,0.8,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;30;-1114.367,-834.8382;Inherit;True;Property;_Emissiontex;Emission tex;4;0;Create;True;0;0;False;0;False;-1;None;08c79037855638f4c9cf2475be797657;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;29;-1635.247,675.4503;Inherit;False;1;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-1758.949,-13.38143;Inherit;False;0;9;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RGBToHSVNode;40;-775.3197,-820.8156;Inherit;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;21;-1932.751,143.0614;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;22;-1544.253,166.7573;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;42;-625.429,-615.5823;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.HSVToRGBNode;41;-485.4622,-811.2429;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;44;-424.9442,-585.1811;Inherit;False;Property;_Desaturation;Desaturation;7;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;25;-1479.863,400.2471;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleAddOpNode;27;-1241.438,576.8777;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DesaturateOpNode;43;-219.7553,-749.4682;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;16;-1313.988,30.75597;Inherit;False;t;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;32;-40.09276,-603.9628;Inherit;False;Constant;_Vector1;Vector 1;5;0;Create;True;0;0;False;0;False;-0.07,1,-2,1;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;39;23.57318,-920.5736;Inherit;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0.5377358,0.5377358,0.5377358;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;26;-1066.38,411.633;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;2;-845.7886,-368.999;Inherit;True;Property;_Main;Main;0;0;Create;True;0;0;False;0;False;-1;None;a9a65b7406db7cc43887c3799e34af16;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;45;-529.7009,-406.4095;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.41;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;31;269.2991,-818.9044;Inherit;True;5;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;1,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;1,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;15;-1315.646,-59.18088;Inherit;False;w;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;9;-678.6069,325.6898;Inherit;True;Property;_Dissolve;Dissolve;2;0;Create;True;0;0;False;0;False;-1;None;b0912ba7ad600c74a81b25c67b729038;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;18;-707.9033,651.3391;Inherit;False;16;t;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-467.8026,-34.68522;Inherit;False;Property;_Opacity;Opacity;1;0;Create;True;0;0;False;0;False;0;1.9;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;13;-431.8134,583.0294;Inherit;False;Constant;_Vector0;Vector 0;3;0;Create;True;0;0;False;0;False;0.3,0,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ClampOpNode;33;525.0548,-723.9158;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;1,1,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.VertexColorNode;8;188.8624,-349.601;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-191.2689,-164.3188;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;36;223.8379,-1080.224;Inherit;False;Property;_Color0;Color 0;6;0;Create;True;0;0;False;0;False;0,0,0,0;0,0.1411762,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-310.9453,386.059;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;456.9294,-535.3431;Inherit;False;Property;_Emission;Emission;5;0;Create;True;0;0;False;0;False;4.5;2.46;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;17;-381.5034,499.3391;Inherit;False;15;w;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;577.1645,-880.1845;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ConditionalIfNode;11;-76.66998,350.2462;Inherit;True;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;5;-2.363652,-149.6047;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;719.8779,-569.7421;Inherit;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;37;938.8506,-447.4213;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;358.7469,-125.5252;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;6;635.6611,-138.8301;Inherit;False;COLOR;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PannerNode;23;-1714.742,-202.5021;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;20;-1915.883,-57.67533;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;820.3143,-108.0418;Float;False;True;-1;2;ASEMaterialInspector;0;9;Slash;0b6a9f8b4f707c74ca64c0be8e590de0;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;True;2;False;-1;True;True;True;True;False;0;False;-1;False;False;False;False;True;2;False;-1;True;3;False;-1;False;True;4;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;40;0;30;0
WireConnection;21;0;19;3
WireConnection;21;1;19;4
WireConnection;22;0;14;0
WireConnection;22;2;21;0
WireConnection;42;0;40;1
WireConnection;42;1;29;3
WireConnection;41;0;42;0
WireConnection;41;1;40;2
WireConnection;41;2;40;3
WireConnection;25;0;22;0
WireConnection;27;0;29;4
WireConnection;27;1;25;1
WireConnection;43;0;41;0
WireConnection;43;1;44;0
WireConnection;16;0;14;4
WireConnection;39;0;43;0
WireConnection;26;0;25;0
WireConnection;26;1;27;0
WireConnection;45;0;2;4
WireConnection;31;0;39;0
WireConnection;31;1;32;1
WireConnection;31;2;32;2
WireConnection;31;3;32;3
WireConnection;31;4;32;4
WireConnection;15;0;14;3
WireConnection;9;1;26;0
WireConnection;33;0;31;0
WireConnection;3;0;45;0
WireConnection;3;1;4;0
WireConnection;10;0;9;1
WireConnection;10;1;18;0
WireConnection;38;0;36;0
WireConnection;38;1;8;0
WireConnection;11;0;10;0
WireConnection;11;1;17;0
WireConnection;11;2;13;2
WireConnection;11;3;13;2
WireConnection;11;4;13;3
WireConnection;5;0;3;0
WireConnection;34;0;33;0
WireConnection;34;1;35;0
WireConnection;34;2;8;0
WireConnection;37;0;38;0
WireConnection;37;1;34;0
WireConnection;7;0;8;4
WireConnection;7;1;5;0
WireConnection;7;2;11;0
WireConnection;6;0;37;0
WireConnection;6;3;7;0
WireConnection;23;0;20;0
WireConnection;20;0;19;1
WireConnection;20;1;19;2
WireConnection;1;0;6;0
ASEEND*/
//CHKSM=A288C9EA4C5E99B7A914618AB04495284CB9D8E3