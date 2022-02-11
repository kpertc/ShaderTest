Shader "my_Shader/unlit/Blur_Unlit"
{
    Properties
    {
        _BaseColor1("Base Color", Color) = (1, 1, 1, 1)
        _BaseMap1 ("BaseMap", 2D) = "white"{}
        
        _blurIntensity ("Outline Width", Range(0, 20)) = 0.01
    }
    SubShader
    {
        Tags{"RenderType" = "Transparent" "RenderPipeline" = "UniversalRenderPipeline" "IgnoreProjector" = "True"}
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

            TEXTURE2D(_BaseMap1);
            SAMPLER(sampler_BaseMap1);

            TEXTURE2D (_CameraColorTexture);
            SAMPLER(sampler_CameraColorTexture);

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor1;
                half _blurIntensity;

                float4 _BaseMap1_TexelSize; //get texture size
                float4 _CameraColorTexture_TexelSize;
            
            CBUFFER_END

            

            Varyings vert(Attributes input)
            {
                Varyings output;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);

                
                
                output.positionCS = vertexInput.positionCS;

                output.uv = output.positionCS.xy * -1 * 0.9;

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                //float2 _uv = input.positionCS.xy/_ScreenParams.xy;
                float4 camera = SAMPLE_TEXTURE2D(_CameraColorTexture, sampler_CameraColorTexture, input.uv);
                
                return float4(camera);

 
                
            }

            ENDHLSL

        }



        }
    
    //https://blog.csdn.net/avi9111/article/details/120892104

    //FallBack "Unlit/Color"

    }


/*
                float dis = _blurIntensity * _BaseMap1_TexelSize;
            
                float4 c1 = SAMPLE_TEXTURE2D(_CameraColorTexture, sampler_CameraColorTexture, float2 (input.uv.x - dis, input.uv.y - dis ));
                float4 c2 = SAMPLE_TEXTURE2D(_CameraColorTexture, sampler_CameraColorTexture, float2 (input.uv.x + 0, input.uv.y - dis ));
                float4 c3 = SAMPLE_TEXTURE2D(_CameraColorTexture, sampler_CameraColorTexture, float2 (input.uv.x + dis, input.uv.y - dis ));

                float4 c4 = SAMPLE_TEXTURE2D(_CameraColorTexture, sampler_CameraColorTexture, float2 (input.uv.x - dis, input.uv.y + 0 ));
                float4 c5 = SAMPLE_TEXTURE2D(_CameraColorTexture, sampler_CameraColorTexture, float2 (input.uv.x + 0, input.uv.y + 0 ));
                float4 c6 = SAMPLE_TEXTURE2D(_CameraColorTexture, sampler_CameraColorTexture, float2 (input.uv.x + dis, input.uv.y + 0 ));

                float4 c7 = SAMPLE_TEXTURE2D(_CameraColorTexture, sampler_CameraColorTexture, float2 (input.uv.x - dis, input.uv.y + dis ));
                float4 c8 = SAMPLE_TEXTURE2D(_CameraColorTexture, sampler_CameraColorTexture, float2 (input.uv.x + 0, input.uv.y + dis ));
                float4 c9 = SAMPLE_TEXTURE2D(_CameraColorTexture, sampler_CameraColorTexture, float2 (input.uv.x + dis, input.uv.y + dis ));

                // Gaussian Distribution
                float4 outPutColor = (
                    c1 * 0.0947416 + c2 * 0.118318 + c3 * 0.094741 +
                    c4 * 0.118318 + c5 * 0.147761 + c6 * 0.118318 +
                    c7 * 0.094741 + c8 * 0.118318 + c9 * 0.094741
                );

                */

                
                //return outPutColor * _BaseColor1;