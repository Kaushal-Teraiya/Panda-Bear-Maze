Shader "Custom/StrongIridescentHeartBubble"
{
    Properties
    {
        _Transparency ("Transparency", Range(0,1)) = 0.35
        _EdgePower ("Edge Power", Range(1,10)) = 5
        _IridescenceStrength ("Iridescence Strength", Range(0,10)) = 4
        _RainbowSpeed ("Rainbow Shift Speed", Range(0,5)) = 1
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent"
            "RenderPipeline"="UniversalPipeline"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
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
                float4 positionCS : SV_POSITION;
                float3 normalWS : TEXCOORD0;
                float3 viewDirWS : TEXCOORD1;
            };

            float _Transparency;
            float _EdgePower;
            float _IridescenceStrength;
            float _RainbowSpeed;

            Varyings vert (Attributes input)
            {
                Varyings output;

                VertexPositionInputs pos =
                GetVertexPositionInputs(input.positionOS.xyz);

                output.positionCS = pos.positionCS;

                output.normalWS =
                TransformObjectToWorldNormal(input.normalOS);

                output.viewDirWS =
                normalize(GetCameraPositionWS() - pos.positionWS);

                return output;
            }

            half4 frag (Varyings input) : SV_Target
            {
                float fresnel =
                pow(
                    1 - saturate(dot(input.normalWS, input.viewDirWS)),
                    _EdgePower
                );

                float rainbow =
                fresnel * 10 +
                _Time.y * _RainbowSpeed;

                float3 iridescent =
                float3(
                    sin(rainbow),
                    sin(rainbow + 2),
                    sin(rainbow + 4)
                ) * 0.5 + 0.5;

                iridescent *= fresnel * _IridescenceStrength;

                float alpha =
                fresnel * _Transparency;

                return float4(iridescent, alpha);
            }

            ENDHLSL
        }
    }
}