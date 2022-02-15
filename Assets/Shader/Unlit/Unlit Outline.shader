//Tutorial
//https://www.videopoetics.com/tutorials/pixel-perfect-outline-shaders-unity/#grazing-angles

Shader "my_Shader/unlit/standard_Unlit_Outline"
{
    Properties
    {
        _BaseColor1("Base Color", Color) = (1, 1, 1, 1)
        _BaseMap1 ("BaseMap", 2D) = "white"{}
        
        _OutlineColor("Outline Color", Color) = (1, 1, 1, 1)
        _OutlineWidth ("Outline Width", Range(0, 1.5)) = 0
        
        _radius("Radius", Range(0.0, 5.0)) = 1.0
        _intensity("Intensity", Range(0.0, 3.0)) = 1.0
        _inputWS("Input World Position", Vector) = (0,0,0)
        
    }
    SubShader
    {
        Tags{"RenderType" = "Transparent" "RenderPipeline" = "UniversalRenderPipeline"}
        LOD 300

        
        Pass
        {
            Cull Front
            
            Name "Outline"
            Tags{"LightMode" = "SRPDefaultUnlit"}
            
            HLSLPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag

            //shadows?
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float3 clipNornal : TEXCOORD1;
            };

            CBUFFER_START(UnityPerMaterial)
                half _OutlineWidth;
                half4 _OutlineColor;
            CBUFFER_END
            
            Varyings vert(Attributes input)
            {
                Varyings output;
                
                // Get ClipSpace
                float4 clipPosition = GetVertexPositionInputs(input.positionOS.xyz).positionCS;

                // get ClipSpace Normal ???
                float3 clipNormal = mul((float3x3) UNITY_MATRIX_VP, mul((float3x3) UNITY_MATRIX_M, input.positionOS.xyz));
                
                clipPosition.xyz += clipNormal * _OutlineWidth;
                
                output.positionCS = clipPosition ;

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                return _OutlineColor;
            }
            
            ENDHLSL

        }
        
        Pass
        {
            Name "unLit + Halo"
            Tags{
                "LightMode" = "UniversalForward"
                "RenderType" = "Transparent"
                "Queue" = "Transparent"
            }

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
                float2 uv         : TEXCOORD0;
                float3 normalOS   : NORMAL;
            };

            struct Varyings
            {
                float2 uv           : TEXCOORD0;
                float4 positionCS   : SV_POSITION;
                float4 positionWS   : TEXCOORD1;
            };

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor1;
                float3 _inputWS;

                half _radius;
                half _intensity;
            CBUFFER_END

            TEXTURE2D(_BaseMap1);
            SAMPLER(sampler_BaseMap1);

            Varyings vert(Attributes input)
            {
                Varyings output;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);

                output.positionWS = mul(unity_ObjectToWorld, float4(input.positionOS.xyz,1)); // Get World Pos
                
                output.uv = input.uv;

                output.positionCS = vertexInput.positionCS;
                
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                float4 _baseTex = SAMPLE_TEXTURE2D(_BaseMap1,sampler_BaseMap1,input.uv);

                half highlight = (1 - saturate(_radius * distance(input.positionWS , _inputWS))) * _intensity;

                half4 return1 = ( _baseTex * _BaseColor1 + highlight);

                return return1;
            }

            ENDHLSL

        }
        
        UsePass "Universal Render Pipeline/Lit/ShadowCaster"
        
        
        
        



        }

    //FallBack "Unlit/Color"

    }