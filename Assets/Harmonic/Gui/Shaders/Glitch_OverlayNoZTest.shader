Shader "UI/Glitch_OverlayNoZTest"
 {
     Properties
     {
         [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
         _Color ("Tint", Color) = (1,1,1,1)
         
         _StencilComp ("Stencil Comparison", Float) = 8
         _Stencil ("Stencil ID", Float) = 0
         _StencilOp ("Stencil Operation", Float) = 0
         _StencilWriteMask ("Stencil Write Mask", Float) = 255
         _StencilReadMask ("Stencil Read Mask", Float) = 255
 
         _ColorMask ("Color Mask", Float) = 15
     }
 
     SubShader
     {
         Tags
         { 
             "Queue"="Overlay" 
             "IgnoreProjector"="True" 
             "RenderType"="Transparent" 
             "PreviewType"="Plane"
             "CanUseSpriteAtlas"="True"
         }
         
         Stencil
         {
             Ref [_Stencil]
             Comp [_StencilComp]
             Pass [_StencilOp] 
             ReadMask [_StencilReadMask]
             WriteMask [_StencilWriteMask]
         }
 
         Cull Off
         Lighting Off
         ZWrite Off
         ZTest Off
         Blend SrcAlpha OneMinusSrcAlpha
         ColorMask [_ColorMask]
 
         Pass
         {
         CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag
             #include "UnityCG.cginc"
             
             struct appdata_t
             {
                 float4 vertex   : POSITION;
                 float4 color    : COLOR;
                 float2 texcoord : TEXCOORD0;
             };
 
             struct v2f
             {
                 float4 vertex   : SV_POSITION;
                 fixed4 color    : COLOR;
                 half2 texcoord  : TEXCOORD0;
             };
             
             fixed4 _Color;
			 
			 float rand(float3 co)
			 {
				 return frac(sin( dot(co.xyz ,float3(12.9898,78.233,45.5432) )) * 43758.5453);
			 }
 
			 float rand(float2 co)
			 {
				 return frac(sin( dot(co.xy ,float3(12.9898,78.233,45.5432) )) * 43758.5453);
			 }
 
			 float rand(float co)
			 {
				 return frac(sin( dot(co ,float3(12.9898,78.233,45.5432) )) * 43758.5453);
			 }
			 
             v2f vert(appdata_t IN)
             {
                 v2f OUT;
                 OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
                 OUT.texcoord = IN.texcoord;
 #ifdef UNITY_HALF_TEXEL_OFFSET
                 OUT.vertex.xy += (_ScreenParams.zw-1.0)*float2(-1,1);
 #endif
				//_Color.a = sin(fmod(_Time[1]*2, 3.141592653589));
				//_Color.a = sin(fmod(rand(OUT.vertex.xyz)+_Time[1]*2, 3.141592653589));
				//_Color.a = sin(
				//				fmod(
				//				(_Time[1]*2)
								//+
								 //_ScreenParams.x
								//OUT.vertex.x
								//fmod(OUT.vertex.x, _ScreenParams.x)
								//+
								//fmod(OUT.vertex.y, _ScreenParams.y)
				//				,3.141592653589));
                 OUT.color = IN.color * _Color;
                 return OUT;
             }
 
             sampler2D _MainTex;
 
             fixed4 frag(v2f IN) : SV_Target
             {
             	float2 griddedF;
             	//griddedF.x = floor(IN.texcoord.x / 160);
             	//griddedF.y = floor(IN.texcoord.y / 160);
             	griddedF.x = IN.texcoord.x / 900;
             	griddedF.y = IN.texcoord.y / 900;
             	IN.color.a = sin(
								fmod(
								_Time[1]*2
								-griddedF.x
								+griddedF.y
								//+rand(griddedF)// / 16
								//+rand(IN.texcoord.y) / 16
								//- fmod(IN.texcoord.x, 2)
								//+ fmod(IN.texcoord.y, 2)
								//+ rand(IN.vertex.xyz)
								//+ rand(fmod(IN.vertex.xyz, 16))
								//+ rand(IN.vertex.xyz / 16)
								,3.141592653589))
								+rand(griddedF);
                 half4 color = tex2D(_MainTex, IN.texcoord) * IN.color;
                 clip (color.a - 0.01);
                 return color;
             }
         ENDCG
         }
     }
 }