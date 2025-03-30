Shader "RenderObjects/Outline"
{
    Properties
    {
        _OutlineColor ("OutlineColor", Color) = (0.0, 0.0, 0.0, 1.0)
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque" "RenderingPipeline"="UniversalPipeline"
        }
        LOD 100
        ZWrite Off Cull Off

        Pass
        {
            Name "OutlinePass"

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionHCS : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert(Attributes input)
            {
                Varyings o;
                UNITY_SETUP_INSTANCE_ID(input);

                o.positionCS = float4(input.positionHCS.xy, 0.0, 1.0);
                o.uv = input.uv;

                // If we're on a Direct3D like platform
                #if UNITY_UV_STARTS_AT_TOP
                    // Flip UVs
                    o.uv = o.uv * float2(1.0, -1.0) + float2(0.0, 1.0);
                #endif
                
                return o;
            }

            float4 _OutlineColor;

            half4 frag(Varyings input) : SV_Target
            {
                return _OutlineColor;
            }
            ENDHLSL
        }
    }
}