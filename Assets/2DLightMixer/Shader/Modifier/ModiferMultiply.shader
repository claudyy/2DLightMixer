Shader "Hidden/ModifierMultiply"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Tex("Multiply", 2D) = "white" {}
		_Value("Multiply",float) = 1
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
			sampler2D _Tex;
			float _Value;
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 mul = tex2D(_Tex, i.uv);
				
				return col * lerp(fixed4(1, 1, 1, 1),mul,_Value);
			}
			ENDCG
		}
	}
}
