Shader "Hidden/MixColorShadow"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_LightLayer("LightLayer", 2D) = "white" {}
		_MixValue("MixValue",float) = 1
		_ShadowColor("ShadowColor",Color) = (0,0,0,0)
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
			
			sampler2D _MainTex, _LightLayer;
			float _MixValue;
			fixed4 _ShadowColor;
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 l = tex2D(_LightLayer, i.uv);
				float mix = lerp(l.r, 1, (1-_MixValue));
				return lerp(_ShadowColor, col, mix);
			}
			ENDCG
		}
	}
}
