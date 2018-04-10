Shader "Hidden/Blur"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_ScreenWidth("Width", Float) = 100
		_ScreenHeight("height", Float) = 100
		_Length("sample length", Float) = 1
		_Iteration("sample iteration", int) = 1
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float _ScreenHeight,_ScreenWidth,_Length;
			int _Iteration;
			float2 GetUV(float2 coord){
				return float2(coord.x /_ScreenWidth,coord.y / _ScreenHeight );
			}
			fixed4 SampleColor(float2 pos, float2 dir){
				fixed4 blurCol = tex2D(_MainTex, GetUV(pos + dir * 1));
				blurCol += tex2D(_MainTex, GetUV(pos + dir * 2));
				blurCol += tex2D(_MainTex, GetUV(pos + dir * 3));
				blurCol += tex2D(_MainTex, GetUV(pos + dir * 4));
				return blurCol;
			}
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				float2 coord = float2(i.uv.x *_ScreenWidth,i.uv.y *_ScreenHeight);
				fixed4 blurCol = col;
				float length = _Length;
				float2 t = float2(0,length);
				float2 tr = float2(length,length);
				float2 r = float2(length,0);
				float2 br = float2(length,-length);
				float2 b = float2(0,-length);
				float2 bl = float2(-length,-length);
				float2 l = float2(-length,0);
				float2 tl = float2(-length,length);
				for(int i = 1; i <= _Iteration;i++){
					blurCol += SampleColor(coord, t * i);
					blurCol += SampleColor(coord, tr * i);
					blurCol += SampleColor(coord, r * i);
					blurCol += SampleColor(coord, br * i);
					blurCol += SampleColor(coord, b * i);
					blurCol += SampleColor(coord, bl * i);
					blurCol += SampleColor(coord, l * i);
					blurCol += SampleColor(coord, tl * i);
					blurCol /=8;
				}	
				return blurCol;
			}
			ENDCG
		}
	}
}
