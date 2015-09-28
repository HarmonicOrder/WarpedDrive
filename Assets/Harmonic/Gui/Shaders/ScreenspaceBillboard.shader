Shader "Custom/ScreenspaceBillboard" {
	Properties {
	[NoScaleOffset] 
      _MainTex ("Texture Image", 2D) = "white" {}
      _MinDist ("Minimum Distance", float) = 2.5
	  _Color ("Tint Color", Color) = (1,1,1,1)
   }
   Category
   {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        AlphaTest Greater .01
        ZTest Always
        
   SubShader {
      Pass {   
         CGPROGRAM
 
         #pragma vertex vert  
         #pragma fragment frag 
         
         #include "UnityCG.cginc"

         // User-specified uniforms            
         uniform sampler2D _MainTex;        
 		 uniform float _MinDist;
 		 uniform fixed4 _Color;
 		 
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

			float dist = length(ObjSpaceViewDir(input.vertex)) / 2;
			
			if (dist < _MinDist)
			  dist = _MinDist;

            output.pos = mul( 
            				UNITY_MATRIX_P, 
              			     mul(UNITY_MATRIX_MV, float4(0, 0, 0.0, 1.0))
              			     
              				+float4(input.vertex.x * dist, input.vertex.y * dist, 0.0, 0.0)
              				);
 
            output.tex = input.tex;

            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR
         {
            fixed4 color = tex2D(_MainTex, float2(input.tex.xy));
            color = color * _Color;
         	return color;
         }
 
         ENDCG
      }
   }
   }

}
