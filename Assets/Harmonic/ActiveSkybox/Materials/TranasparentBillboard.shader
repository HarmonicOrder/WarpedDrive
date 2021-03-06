﻿Shader "Custom/TransparentBillboard" {
	Properties {
	[NoScaleOffset] 
      _MainTex ("Texture Image", 2D) = "white" {}
      
      Scale ("Scale", Float) = 1
   }
   Category
   {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        AlphaTest Greater .01
        
   SubShader {
      Pass {   
         CGPROGRAM
 
         #pragma vertex vert  
         #pragma fragment frag 
         
         #include "UnityCG.cginc"

         // User-specified uniforms            
         uniform sampler2D _MainTex;      
         uniform float Scale;  
 
         struct vertexInput {
            float4 vertex : POSITION;
            float4 tex : TEXCOORD0;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 tex : TEXCOORD0;
         };
 
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;

            output.pos = mul( 
            				UNITY_MATRIX_P, 
              			     mul(UNITY_MATRIX_MV, float4(0, 0, 0.0, 1.0))
              			     
              				+float4(input.vertex.x * Scale, input.vertex.y * Scale, 0.0, 0.0)
              				);
 
            output.tex = input.tex;

            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR
         {
         	return tex2D(_MainTex, float2(input.tex.xy));   
         }
 
         ENDCG
      }
   }
   }
}
