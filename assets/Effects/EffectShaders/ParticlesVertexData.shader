Shader "Custom/ParticlesVertexData"
{
		Properties
		{
				_MainTex("MainTex", 2D) = "white" {}
				_Color("Color", Color) = (1, 1, 1, 1)
		}
		SubShader
		{
				Tags { "RenderType" = "Opaque" }
				LOD 100

				Pass
				{
						CGPROGRAM
						#pragma vertex vert
						#pragma fragment frag
						// make fog work
						#pragma multi_compile_fog

						#include "UnityCG.cginc"

						uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
						half4 _Color;

						struct appdata
						{
								float4 vertex : POSITION;
								float4 normal : NORMAL;
								float4 color : COLOR;
								float3 uv : TEXCOORD0;
						};

						struct v2f
						{
								float2 uv : TEXCOORD0;
								float custom_data : TEXCOORD1;
								float4 color : TEXCOORD2;
								UNITY_FOG_COORDS(1)
								float4 vertex : SV_POSITION;
						};

						v2f vert(appdata v)
						{
								v2f o;
								o.vertex = UnityObjectToClipPos(v.vertex);
								o.uv = TRANSFORM_TEX(v.uv.xy, _MainTex);
								o.custom_data = v.uv.z;
								UNITY_TRANSFER_FOG(o,o.vertex);
								return o;
						}

						fixed4 frag(v2f i) : SV_Target
						{
								fixed4 col = tex2D(_MainTex, i.uv);
								//col *= (1-i.custom_data * 10) * _Color;
								half4 final = lerp(col, _Color, 1 - i.custom_data+0.1)
								UNITY_APPLY_FOG(i.fogCoord, col);
								return final;
						}
				ENDCG
				}
		}
}
