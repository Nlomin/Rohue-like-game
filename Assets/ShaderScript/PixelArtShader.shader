Shader "Custom/PixelArtToonShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color Tint", Color) = (1,1,1,1)
        _PixelSize ("Pixel Size", Float) = 1
        _LightColor ("Light Color", Color) = (1, 1, 1, 1)
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
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _Color;
            float4 _MainTex_ST;
            float _PixelSize;
            float4 _LightColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // Get pixelated UVs
                float2 screenUV = i.vertex.xy / i.vertex.w;
                screenUV = 0.5 * screenUV + 0.5;
                screenUV *= _ScreenParams.xy;

                screenUV = floor(screenUV / _PixelSize) * _PixelSize;
                screenUV /= _ScreenParams.xy;

                float4 texColor = tex2D(_MainTex, i.uv);

                // Simple diffuse shading based on light direction
                float3 lightDir = normalize(float3(0.5, 1, 0.3));
                float diff = max(0, dot(normalize(i.normal), lightDir));

                // Posterize light levels for pixel-art effect
                float levels = 3.0;
                diff = floor(diff * levels) / levels;

                float4 color = texColor * _Color * (diff * _LightColor + 0.1);
                return color;
            }
            ENDCG
        }
    }
}

