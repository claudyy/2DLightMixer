// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/ModifierAdd"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Tex("Multiply", 2D) = "white" {}
		_Value("Multiply",float) = 1
		_Size("texture size",float) = 1

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
				float4 worldPos : TEXCOORD1;

			};
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.worldPos = mul (unity_ObjectToWorld, v.vertex);

				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _Tex;
			float _Value, _Size;
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 add = tex2D(_Tex, frac(i.worldPos.xy *_Size));
				
				return col + lerp(fixed4(0, 0, 0, 0),add,_Value);
			}
			ENDCG
		}
	}
}
