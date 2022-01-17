Shader "my_Shader/unlit/reverse_Color"
{
    Properties
    {
        _BaseColor1("Base Color", Color) = (1, 1, 1, 1)
        _BaseMap1 ("BaseMap", 2D) = "white"{}
    }
    SubShader
    {
        Tags{"RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline" "IgnoreProjector" = "True"}
        LOD 300

        Pass
        {
            Name "StandardLit"
            Tags{"LightMode" = "UniversalForward"}

            HLSLPROGRAM
            //#pragma prefer_hlslcc gles
            //#pragma exclude_renderers d3d11_9x
            //#pragma target 2.0

            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE


            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv           : TEXCOORD0;
                float3 normalOS   : NORMAL;
            };

            struct Varyings
            {
                float2 uv           : TEXCOORD0;
                float4 positionCS   : SV_POSITION;
            };

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor1;
            CBUFFER_END

            TEXTURE2D(_BaseMap1);
            SAMPLER(sampler_BaseMap1);

            Varyings vert(Attributes input)
            {
                Varyings output;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                output.uv = input.uv;

                output.positionCS = vertexInput.positionCS;

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {

                float4 _baseTex = SAMPLE_TEXTURE2D(_BaseMap1,sampler_BaseMap1,input.uv);
                float4 SC = _baseTex * _BaseColor1;

                //negative Color
                float4 return1 = half4(1 - SC.x, 1 - SC.y, 1 - SC.z, 1);

                //Black and White
                float4 returnGray = dot(return1.rgb, float3(0.299, 0.587, 0.114));

                //half4 return1 = _baseTex * _BaseColor1;
                
                return returnGray;
            }

            ENDHLSL

        }



        }

    //FallBack "Unlit/Color"

    }