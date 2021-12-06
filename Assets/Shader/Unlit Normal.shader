Shader "my_Shader/Unlit_NormalColor"
{
    Properties
    {
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
                float3 normalOS   : NORMAL;
            };

            struct Varyings
            {
                float3 normal       : TEXCOORD0;
                float4 positionCS   : SV_POSITION;
            };

            CBUFFER_START(UnityPerMaterial)
                
            CBUFFER_END

            

            Varyings vert(Attributes input)
            {
                Varyings output;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                output.positionCS = vertexInput.positionCS;
                output.normal = input.normalOS;

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                return half4(input.normal, 1);
            }

            ENDHLSL

        }



        }

    //FallBack "Unlit/Color"

    }