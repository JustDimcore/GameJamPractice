Shader "GameJam/LaserShader"
{
    Properties
    {
        _MainTex ("MainTex", 2D) = "white" {}
        _Ramp ("Ramp", 2D) = "white" {}   
        _Color("Color", Color) = (1,0,0,1)
        _Intensity("VO Intensity", Float) = 1
        _Speed("Speed", Float) = 1        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
            uniform sampler2D _Ramp; uniform float4 _Ramp_ST;
            half4 _Color;
            half _Intensity;
            half _Speed;
            
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
                float4 tex = tex2Dlod (_MainTex, float4(float2(v.uv.x ,v.uv.y +(_Time.x *_Speed)),0,0));
                v.vertex.x += (tex  * _Intensity) * v.normal;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = _Color * tex2D(_MainTex, float2(i.uv.x , i.uv.y + _Time.x * _Speed));
                //fixed4 col = tex2D(_MainTex, i.uv);
                half4 _Ramp_var = tex2D(_Ramp, float2(TRANSFORM_TEX(col, _Ramp).x  + _Time.y * _Speed /2, TRANSFORM_TEX(col, _Ramp).y));
                UNITY_APPLY_FOG(i.fogCoord, col);
                return _Ramp_var;
            }
            ENDCG
        }
    }
}
