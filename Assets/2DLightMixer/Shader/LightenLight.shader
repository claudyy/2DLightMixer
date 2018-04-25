﻿Shader "Hidden/LightenLight"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_LightLayer ("LightLayer", 2D) = "white" {}
		_AddValue("AddValue",float) = 1
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
			sampler2D _LightLayer;
			float _AddValue;
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 l = tex2D(_LightLayer, i.uv);
				fixed4 c = max(col, l);
				return lerp(col,c,_AddValue);
			}
			ENDCG
		}
	}
}