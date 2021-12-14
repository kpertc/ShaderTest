Shader "GMShader/moban"
{


    Properties
    {
     
        _MainTex("MainTex", 2D) = "white" {}
        _BaseColor("BaseColour", Color) = (1,1,1,1)
    }
    SubShader 
    {
        Tags{"RenderType" = "Opaque" 
            "RenderPipeline" = "UniversalRenderPipeline"
             "IgnoreProjector" = "True"}

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

            CBUFFER_START(UnityPerMaterial)
        
        
            CBUFFER_END

            struct VertextInput
            {
                float4 vertex : POSITION;
            };
            struct VertextOutput
            {
                float4 pos : SV_POSITION;
            };
            
            VertextOutput vert(VertextInput v)
            {
                VertextOutput o = (VertextOutput)0;
                o.pos = TransformObjectToHClip(v.vertex);
                return o;
            }

            float4 frag(VertextInput i) : COLOR
            {
                return  float4(0.1,0.5,0.1,1.0);
            }
            ENDHLSL
        }
        UsePass "Universal Render Pipeline/Lit/ShadowCaster"
    }
}
        
        
    
       
        
        
        
  