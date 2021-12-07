Shader "my_Shader/Unlit_Fresnel"
{
    Properties
    {
        _BaseColor1("Base Color", Color) = (1, 1, 1, 1)
        _BaseMap1 ("BaseMap", 2D) = "white"{}
        
        _FresnelScale("Fresnel Scale", Range(0.0, 100.0)) = 1
        _FresnelInDensity("Fresnel InDensity", Range(0.0, 100.0)) = 1
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
                float3 N  : TEXCOORD2;
                float3 V : TEXCOORD3;
                float3 WS : TEXCOORD4;
                float3 normal           : TEXCOORD1;
                float4 positionCS   : SV_POSITION;
            };

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor1;
                half _FresnelScale;
                half _FresnelInDensity;
            CBUFFER_END

            TEXTURE2D(_BaseMap1);
            SAMPLER(sampler_BaseMap1);

            Varyings vert(Attributes input)
            {
                Varyings output;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);

                output.WS = mul(unity_ObjectToWorld, input.positionOS.xyz);
                
                output.V = GetWorldSpaceViewDir(input.positionOS.xyz);
                //output.N = mul(input.normalOS, (float3x3)unity_WorldToObject);
                output.N = input.normalOS;
                output.uv = input.uv;

                output.positionCS = vertexInput.positionCS;

                //output.normal = input.normalOS;

                return output;
            }

            half4 frag(Varyings i) : SV_Target
            {
                float3 N = normalize(i.N);
                float3 V = normalize( _WorldSpaceCameraPos - i.WS);

                //float3 V = normalize( i.V);
                   
                float fresnel = _FresnelScale * pow(saturate(1- dot(N,V)), _FresnelInDensity);

                return fresnel;
                
                //float4 _baseTex = SAMPLE_TEXTURE2D(_BaseMap1,sampler_BaseMap1,input.uv);

                //half4 return1 = _baseTex * _BaseColor1 + fresnel;

                //return return1;
            }

            ENDHLSL

        }



        }

    //FallBack "Unlit/Color"

    }