Shader "Unlit/HiddenShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Alpha ("Alpha", float) = 0.05
	}
	SubShader
	{
		Tags { "Queue" = "Transparent" }
		LOD 100

		Pass
		{
			Cull Back
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 uv : TEXCOORD0;
				float4 col : COLOR;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Alpha;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				float g = 0;
				float r = 0;
				float b = (_SinTime + 0.5) * 5;
				float4 color = float4(r, g, b, 0);
				o.uv = mul(UNITY_MATRIX_MV, v.vertex + float4(10 , 0 , 13 , 0)) / 10;
				o.col = o.uv + color;
				return o;
			}
			
			fixed4 frag (v2f i) : COLOR
			{
				return i.col;
			}
			ENDCG
		}
	}
}
