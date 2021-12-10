Shader "my_Shader/unlit/Unlit_Texture_World"
{
    Properties
    {
        _BaseColor1("Base Color", Color) = (1,1,1,1)
        _CoverColor("Cover Colour", Color) = (1,1,1,1)
        
        _offset("Offset (Z-Pos)", Float) = 5.0
        
        _X("X", Float) = 5.0
        _Y("Y", Float) = 5.0
        
        _waveScale("Wave (Z-Pos)", Range(0.0, 1.0)) = 1
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
                float3 positionWS : TEXCOORD1;
                float2 uv           : TEXCOORD0;
                float4 positionCS   : SV_POSITION;
            };

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor1;
                half4 _CoverColor;
            
                half _X;
                half _Y;

                half _offset;

                half _waveScale;
            CBUFFER_END

            //TEXTURE2D(_BaseMap1);
            //SAMPLER(sampler_BaseMap1);

            Varyings vert(Attributes input)
            {

               
                Varyings output;
                
                float3 NormalStrength = clamp ( 1- abs(mul(unity_ObjectToWorld, float4(input.positionOS.xyz,1)).z - _offset), 0, 1);
                VertexPositionInputs vertexInput = GetVertexPositionInputs(NormalStrength * input.normalOS * _waveScale + input.positionOS.xyz);
                output.uv = input.uv;
                output.positionWS = mul(unity_ObjectToWorld, float4(input.positionOS.xyz,1));

                
                
                output.positionCS = vertexInput.positionCS;

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                half4 return1;
                
                half PosLerp = lerp(_X + _offset,_Y + _offset, -input.positionWS.z);
                
                return1 = _CoverColor * PosLerp + _BaseColor1 * clamp(1 - PosLerp, 0, 1);
                
                return return1;
            }

            ENDHLSL

        }



        }

    //FallBack "Unlit/Color"

    }