Shader "GMShader/Lambert"
{
    Properties
    {
        _MainTex("MainTex", 2D) = "white" {}
        _BaseColor("BaseColour", Color) = (1,1,1,1)
    }
    SubShader 
    {
        Tags
        {
            "RenderType" = "Opaque" 
            "RenderPipeline" = "UniversalRenderPipeline"
        }

        Pass
        {   
            Name "FORWARD"
            Tags
            {
                "LightMode"="UniversalForward"
            }
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            
            CBUFFER_START(UnityPerMaterial)

            float4 _MainTex_ST;
            half4  _BaseColor;
        
            CBUFFER_END

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            struct VertextInput
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD;
                float3 normal : NORMAL;
            };
            struct VertextOutput
            {
                float4 pos : SV_POSITION;
                float2 uv : NORMAL;
                float3 nDirWS : TEXCOORD1;
            };
            
            VertextOutput vert(VertextInput v)
            {
                VertextOutput o = (VertextOutput)0;
                o.pos = TransformObjectToHClip(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.nDirWS = TransformObjectToWorldNormal(v.normal,true);
                return o;
            }

            float4 frag(VertextOutput i) : COLOR
            {
                float3 nDir = i.nDirWS;

                Light mylight = GetMainLight();

                float4 LightColor = float4(mylight.color,1);

                float3 lDir =normalize(mylight.direction);

                float nDotl = dot(nDir,lDir);

                float lambert = max(0.0, nDotl);

                half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv)*_BaseColor;
                      
                return tex*lambert*LightColor;
            }
            ENDHLSL
        }
        UsePass "Universal Render Pipeline/Lit/ShadowCaster"
    }
}