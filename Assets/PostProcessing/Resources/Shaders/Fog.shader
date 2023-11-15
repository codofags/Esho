Shader "Custom/FogShader"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _FogColor("Fog Color", Color) = (0.5, 0.5, 0.5, 1.0)
        _Density("Fog Density", Range(0.001, 0.1)) = 0.01
        _Start("Fog Start", Range(0.0, 100.0)) = 0.0
        _End("Fog End", Range(1.0, 100.0)) = 10.0
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float fog : TEXCOORD1;
            };

            float4 _MainTex_ST;
            sampler2D _MainTex;

            float4 _FogColor;
            float _Density;
            float _Start;
            float _End;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                float depth = o.vertex.z / o.vertex.w;
                o.fog = saturate((depth - _Start) / (_End - _Start));
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                half4 col = tex2D(_MainTex, i.uv);
                half4 fogCol = lerp(col, _FogColor, i.fog);
                return fogCol;
            }
            ENDCG
        }
    }
}Shader "Custom/FogShader"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _FogColor("Fog Color", Color) = (0.5, 0.5, 0.5, 1.0)
        _Density("Fog Density", Range(0.001, 0.1)) = 0.01
        _Start("Fog Start", Range(0.0, 100.0)) = 0.0
        _End("Fog End", Range(1.0, 100.0)) = 10.0
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float fog : TEXCOORD1;
            };

            float4 _MainTex_ST;
            sampler2D _MainTex;

            float4 _FogColor;
            float _Density;
            float _Start;
            float _End;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                float depth = o.vertex.z / o.vertex.w;
                o.fog = saturate((depth - _Start) / (_End - _Start));
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                half4 col = tex2D(_MainTex, i.uv);
                half4 fogCol = lerp(col, _FogColor, i.fog);
                return fogCol;
            }
            ENDCG
        }
    }
}