Shader "my shader/PBR/S URP PBR"
{
    Properties
    {
            _BaseMap ("Base Texture", 2D) = "white" {}
            _BaseColor ("Example Colour", Color) = (0, 0.66, 0.73, 1)
            _Smoothness ("Smoothness", Float) = 0.5
         
            [Toggle(_ALPHATEST_ON)] _EnableAlphaTest("Enable Alpha Cutoff", Float) = 0.0
            _Cutoff ("Alpha Cutoff", Float) = 0.5
         
            [Toggle(_NORMALMAP)] _EnableBumpMap("Enable Normal/Bump Map", Float) = 0.0
            _BumpMap ("Normal/Bump Texture", 2D) = "bump" {}
            _BumpScale ("Bump Scale", Float) = 1
         
            [Toggle(_EMISSION)] _EnableEmission("Enable Emission", Float) = 0.0
            _EmissionMap ("Emission Texture", 2D) = "white" {}
            _EmissionColor ("Emission Colour", Color) = (0, 0, 0, 0)
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            // Material Keywords
            #pragma shader_feature _NORMALMAP
            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _ALPHAPREMULTIPLY_ON
            #pragma shader_feature _EMISSION
            
            // URP Keywords
            
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            
            
            #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE

            //shadows?
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            
            
            // Unity defined keywords
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile_fog
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                float4 tangentOS    : TANGENT;
                float4 color        : COLOR;
                float2 uv           : TEXCOORD0;
                float2 lightmapUV   : TEXCOORD1;
            };

            struct Varyings
            {
                float4 positionCS               : SV_POSITION;
                float4 color                    : COLOR;
                float2 uv                       : TEXCOORD0;
                DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 1);
                // Note this macro is using TEXCOORD1
            #ifdef REQUIRES_WORLD_SPACE_POS_INTERPOLATOR
                float3 positionWS               : TEXCOORD2;
            #endif
                float3 normalWS                 : TEXCOORD3;
            #ifdef _NORMALMAP
                float4 tangentWS                : TEXCOORD4;
            #endif
                float3 viewDirWS                : TEXCOORD5;
                half4 fogFactorAndVertexLight   : TEXCOORD6;
                // x: fogFactor, yzw: vertex light
            #ifdef REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
                float4 shadowCoord              : TEXCOORD7;
            #endif
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST; // Texture tiling & offset inspector values
                float4 _BaseColor;
                float _BumpScale;
                float4 _EmissionColor;
                float _Smoothness;
                float _Cutoff;
            CBUFFER_END

            sampler2D _MainTex;
            
            
            float4 _MainTex_ST;


            
            Varyings vert(Attributes IN){
                Varyings OUT;
 
                // Vertex Position
                VertexPositionInputs positionInputs = GetVertexPositionInputs(IN.positionOS.xyz);
                OUT.positionCS = positionInputs.positionCS;
            #ifdef REQUIRES_WORLD_SPACE_POS_INTERPOLATOR
                OUT.positionWS = positionInputs.positionWS;
            #endif
                // UVs & Vertex Colour
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                OUT.color = IN.color;
             
                // View Direction
                OUT.viewDirWS = GetWorldSpaceViewDir(positionInputs.positionWS);
             
                // Normals & Tangents
                VertexNormalInputs normalInputs = GetVertexNormalInputs(IN.normalOS, IN.tangentOS);
                OUT.normalWS =  normalInputs.normalWS;
            #ifdef _NORMALMAP
                real sign = IN.tangentOS.w * GetOddNegativeScale();
                OUT.tangentWS = half4(normalInputs.tangentWS.xyz, sign);
            #endif
             
                // Vertex Lighting & Fog
                half3 vertexLight = VertexLighting(positionInputs.positionWS, normalInputs.normalWS);
                half fogFactor = ComputeFogFactor(positionInputs.positionCS.z);
                OUT.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
             
                // Baked Lighting & SH (used for Ambient if there is no baked)
                OUTPUT_LIGHTMAP_UV(IN.lightmapUV, unity_LightmapST, OUT.lightmapUV);
                OUTPUT_SH(OUT.normalWS.xyz, OUT.vertexSH);
             
                // Shadow Coord
            #ifdef REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
                OUT.shadowCoord = GetShadowCoord(positionInputs);
            #endif
                return OUT;
            }

            InputData InitializeInputData(Varyings IN, half3 normalTS){
                InputData inputData = (InputData)0;
 
                #if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
                    inputData.positionWS = IN.positionWS;
                #endif
                                 
                    half3 viewDirWS = SafeNormalize(IN.viewDirWS);
                #ifdef _NORMALMAP
                    float sgn = IN.tangentWS.w; // should be either +1 or -1
                    float3 bitangent = sgn * cross(IN.normalWS.xyz, IN.tangentWS.xyz);
                    inputData.normalWS = TransformTangentToWorld(normalTS, half3x3(IN.tangentWS.xyz, bitangent.xyz, IN.normalWS.xyz));
                #else
                    inputData.normalWS = IN.normalWS;
                #endif
                 
                    inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
                    inputData.viewDirectionWS = viewDirWS;
                 
                #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                    inputData.shadowCoord = IN.shadowCoord;
                #elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
                    inputData.shadowCoord = TransformWorldToShadowCoord(inputData.positionWS);
                #else
                    inputData.shadowCoord = float4(0, 0, 0, 0);
                #endif
                 
                    //inputData.fogCoord = IN.fogFactorAndVertexLight.x;
                    //inputData.vertexLighting = IN.fogFactorAndVertexLight.yzw;
                    //inputData.bakedGI = SAMPLE_GI(IN.lightmapUV, IN.vertexSH, inputData.normalWS);
                    return inputData;
            }

            SurfaceData InitializeSurfaceData(Varyings IN){
                SurfaceData surfaceData = (SurfaceData)0;
                // Note, we can just use SurfaceData surfaceData; here and not set it.
                // However we then need to ensure all values in the struct are set before returning.
                // By casting 0 to SurfaceData, we automatically set all the contents to 0.
                     
                half4 albedoAlpha = SampleAlbedoAlpha(IN.uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));
                surfaceData.alpha = Alpha(albedoAlpha.a, _BaseColor, _Cutoff);
                surfaceData.albedo = albedoAlpha.rgb * _BaseColor.rgb * IN.color.rgb;
             
                // Not supporting the metallic/specular map or occlusion map
                // for an example of that see : https://github.com/Unity-Technologies/Graphics/blob/master/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl
             
                surfaceData.smoothness = _Smoothness;
                surfaceData.normalTS = SampleNormal(IN.uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
                surfaceData.emission = SampleEmission(IN.uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));
                surfaceData.occlusion = 1;
                return surfaceData;
            }

            half4 frag(Varyings IN) : SV_Target {
                SurfaceData surfaceData = InitializeSurfaceData(IN);
                InputData inputData = InitializeInputData(IN, surfaceData.normalTS);
                             
                // In URP v10+ versions we could use this :
                // half4 color = UniversalFragmentPBR(inputData, surfaceData);
             
                // But for other versions, we need to use this instead.
                // We could also avoid using the SurfaceData struct completely, but it helps to organise things.
                half4 color = UniversalFragmentPBR(inputData, surfaceData.albedo, surfaceData.metallic, 
                  surfaceData.specular, surfaceData.smoothness, surfaceData.occlusion, 
                  surfaceData.emission, surfaceData.alpha);
                             
                color.rgb = MixFog(color.rgb, inputData.fogCoord);
             
                // color.a = OutputAlpha(color.a);
                // Not sure if this is important really. It's implemented as :
                // saturate(outputAlpha + _DrawObjectPassData.a);
                // Where _DrawObjectPassData.a is 1 for opaque objects and 0 for alpha blended.
                // But it was added in URP v8, and versions before just didn't have it.
                // And I'm writing thing for v7.3.1 currently
                // We could still saturate the alpha to ensure it doesn't go outside the 0-1 range though :
                color.a = saturate(color.a);
             
                return color;
            }
            
            ENDHLSL
        }
    }
}
