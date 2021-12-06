Shader "my_Shader/special/WaveSphere"
{
    Properties
    {
        [MainColor] _BaseColor1("Base Color", Color) = (1, 1, 1, 1)
        _BaseMap1 ("BaseMap", 2D) = "white"{}
        
        _Amplitude("_Amplitude", Range(0,10)) = 0
        _Frequency("_Frequency", Range(0,100)) = 0
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
                half _Amplitude;
                half _Frequency;
            CBUFFER_END

            TEXTURE2D(_BaseMap1);
            SAMPLER(sampler_BaseMap1);

            Varyings vert(Attributes input)
            {
                Varyings output;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz += input.normalOS * sin(input.positionOS.x * _Frequency + _Time.y) * _Amplitude);
                output.uv = input.uv;
                
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
