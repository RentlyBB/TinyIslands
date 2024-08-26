Shader "Custom/URPTextureTiling"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Scale("Texture Scale", Float) = 1.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 200

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _Color;
            float _Scale;

            Varyings vert(Attributes v)
            {
                Varyings o;

                // Transform the object space position to world space
                float4 positionWS = mul(GetObjectToWorldMatrix(), v.positionOS);
                o.positionHCS = TransformWorldToHClip(positionWS);
                o.worldPos = positionWS.xyz;
                o.worldNormal = normalize(mul((float3x3) GetObjectToWorldMatrix(), v.normalOS));

                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                float3 normal = normalize(i.worldNormal);
                float2 uv;
                half4 c;

                if (abs(normal.x) > 0.5)
                {
                    uv = i.worldPos.yz; // side
                }
                else if (abs(normal.z) > 0.5)
                {
                    uv = i.worldPos.xy; // front
                }
                else
                {
                    uv = i.worldPos.xz; // top
                }

                // Sample the texture with scaling and apply the color
                c = tex2D(_MainTex, uv * _Scale);
                c.rgb *= _Color.rgb;

                // No lighting, just return the color and texture
                return half4(c.rgb, c.a);
            }
            ENDHLSL
        }
    }

    FallBack "Universal Render Pipeline/Lit"
}