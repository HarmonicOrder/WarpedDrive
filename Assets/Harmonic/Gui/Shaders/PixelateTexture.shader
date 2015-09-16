Shader "Custom/PixelateTexture"
{
    Properties
    {
        _CellSize ("Cell Size", Vector) = (0.02, 0.02, 0, 0)
		_MainTex ("Sprite Texture", 2D) = "white" {}
    }
    SubShader {
        Tags { "RenderType"="Opaque" "Queue" = "Transparent" }
        LOD 200
     
        GrabPass { "_PixelationGrabTexture"}
     
        Pass {
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"
         
                struct v2f {
                    float4 pos : SV_POSITION;
                    float4 grabUV : TEXCOORD0;
					half2 texcoord  : TEXCOORD1;
                };
             
                float4 _CellSize;
				sampler2D _MainTex;
             
                v2f vert(appdata_base v) {
                    v2f o;
                    o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                    o.grabUV = ComputeGrabScreenPos(o.pos);
					o.texcoord = v.texcoord;
                    return o;
                }
         
                sampler2D _PixelationGrabTexture;
         
                float4 frag(v2f IN) : COLOR
                {
					float2 steppedUV = IN.grabUV.xy/IN.grabUV.w;
					if (tex2D(_MainTex, IN.texcoord).a == 1){
						steppedUV /= _CellSize.xy;
						steppedUV = round(steppedUV);
						steppedUV *= _CellSize.xy;
					}
					return tex2D(_PixelationGrabTexture, steppedUV);
                }
            ENDCG
        }
    }
}