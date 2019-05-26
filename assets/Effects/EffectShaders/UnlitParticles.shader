Shader "GameJam/UnlitParticles"
{
    Properties
    {
        [Enum(UnityEngine.Rendering.CullMode)] HARDWARE_CullMode("Cull faces", Float) = 2
        [Enum(UnityEngine.Rendering.CompareFunction)] HARDWARE_ZTest("ZTest", Float) = 2
        [Enum(On, 1, Off, 0)] HARDWARE_ZWrite("Depth write", Float) = 1
        _MainTex ("MainTex", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Blend One OneMinusSrcAlpha
            Cull[HARDWARE_CullMode]
            ZWrite[HARDWARE_ZWrite]
            ZTest[HARDWARE_ZTest]
            
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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
