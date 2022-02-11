Shader"GMShader/Ulit"
{
    Properties  
    {
        _ExampleTex("Example Texture", 2D) = "white"
        _BaseColor1("Base Color", Color) = (1, 1, 1, 1)
        _ExampleRange("Example Float Range", Range(0.0, 1.0)) = 0.5
    }
    
    SubShader
    {
            Pass
        {
            Tags{"RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline" "IgnoreProjector" = "True"}
        
            HLSLPROGRAM
            
            //定义顶点shader名字
            #pragma vertex vert
            //定义片元shader名字
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            //下面结构体包含了顶点着色器的输入数据
            struct Attributes
            {
                // object space
                float4 positionOS   : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                //这个结构体里必须包含SV_POSITION,Homogeneous Clipping Space
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
             
            };
               
            CBUFFER_START(UnityPerMaterial)
             float4 _ExampleTexture_ST;
            half4 _BaseColor1;
            float _ExampleRange;
            CBUFFER_END


             
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(IN.positionOS.xyz);
   

                OUT.positionHCS = vertexInput.positionCS;

                return OUT;
            }

            half4 frag(Varyings IN):SV_Target
            
            {
                return _BaseColor1;
            }
            ENDHLSL
            
                }
            Pass
            {
            }
        }
        Fallback "ExampleFallbackShader" 
    }