Shader "my_Shader/Unlit_MovingLight"
{
    Properties
    {
        _BaseColor1("Base Color", Color) = (1, 1, 1, 1)
        //_BaseMap1("Base Map", 2D) = "white" {}
        
        _colorStart("Start Color", Color) = (0, 0, 0, 1)
        _colorEnd("End Color", Color) = (1, 1, 1, 1)
        
        _Speed("X Speed", Range(-100.0, 100.0)) = 0
        _Scale("Scale", Float) = 0
        
        _X("X", Float) = 5.0
        _Y("Y", Float) = 5.0
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
                float3 positionWS   : TEXCOORD1;
                float4 positionCS   : SV_POSITION;
            };

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor1;

                half _colorStart;
                half _colorEnd;
            
                half _Speed;
                half _Scale;

                half _X;
                half _Y;

                half _offset;
            CBUFFER_END

            //TEXTURE2D(_BaseMap1);
            //SAMPLER(sampler_BaseMap1);

            Varyings vert(Attributes input)
            {
                Varyings output;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);

                output.positionWS = mul(unity_ObjectToWorld, float4(input.positionOS.xyz,1));

                output.uv = input.uv;
                
                output.positionCS = vertexInput.positionCS;
                
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                half PosLerp = cos(lerp(_X + _offset,_Y + _offset, input.positionWS.x) * _Scale + _Time * _Speed);

                half4 return1 = PosLerp * _BaseColor1;

                return return1;
            }

            ENDHLSL

        }



        }

    //FallBack "Unlit/Color"

    }