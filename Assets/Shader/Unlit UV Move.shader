Shader "my_Shader/Unlit_UV_Move"
{
    Properties
    {
        _BaseColor1("Base Color", Color) = (1, 1, 1, 1)
        _BaseMap1("Base Map", 2D) = "white" {}
        
        _XSpeed("X Speed", Range(0.0, 10.0)) = 0
        _YSpeed("Y Speed", Range(0.0, 10.0)) = 0
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

                half _XSpeed;
                half _YSpeed;
            CBUFFER_END

            TEXTURE2D(_BaseMap1);
            SAMPLER(sampler_BaseMap1);

            Varyings vert(Attributes input)
            {
                Varyings output;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                output.uv.x = input.uv.x + _Time * _XSpeed;
                output.uv.y = input.uv.y + _Time * _YSpeed;

                output.positionCS = vertexInput.positionCS;

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {


                float4 _baseTex = SAMPLE_TEXTURE2D(_BaseMap1,sampler_BaseMap1,input.uv);

                half4 return1 = _baseTex * _BaseColor1;

                return return1;
            }

            ENDHLSL

        }



        }

    //FallBack "Unlit/Color"

    }