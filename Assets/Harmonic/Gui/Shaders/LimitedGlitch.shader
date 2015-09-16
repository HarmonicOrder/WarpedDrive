Shader "Custom/LimitedGlitch" {
	Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _CellSize ("Cell Size", Vector) = (0.02, 0.02, 0, 0)
    }
    SubShader {
       Blend SrcAlpha OneMinusSrcAlpha
         Tags
         { 
             "Queue"="Overlay" 
             "RenderType"="Transparent" 
         }
        Pass {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"
            
            float4 _CellSize;
            uniform sampler2D _MainTex;

			 float rand(float2 co)
			 {
				 return frac(sin( dot(co.xy ,float3(12.9898,78.233,45.5432) )) * 43758.5453);
			 }
			 
            fixed4 frag(v2f_img i) : SV_Target {
			    float2 steppedUV = i.uv;
				steppedUV /= _CellSize.xy;
				steppedUV = round(steppedUV);
				steppedUV *= _CellSize.xy;
             	fixed4 color = tex2D(_MainTex, steppedUV);
				color.a = sin(
								fmod(
								_Time[1]*2
								-steppedUV.x/
								steppedUV.y
								+rand(steppedUV)
								,3.141592653589));
                 clip (color.a - 0.01);
                 return color;
            }
            ENDCG
        }
    }
}
