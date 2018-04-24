Shader "Hidden/ModifierAdd"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Tex ("add texture", 2D) = "white" {}
		_Value("AddValue",float) = 1
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
				fixed4 add = tex2D(_Tex, i.uv);
				
				return col + lerp(fixed4(0, 0, 0, 0),add,_Value);
			}
			ENDCG
		}
	}
}
