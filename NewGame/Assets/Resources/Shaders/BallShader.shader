Shader "Unlit/BallShader"
{
	Properties
	{
		_Red("Red", float) = 1.0
		_Green("Green", float) = 1.0
		_Blue("Blue", float) = 1.0
		_Alpha("Alpha", float) = 0.5
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" }
		LOD 100

		Pass
	{
		Cull Back
		//ZTest Always
		ZWrite Off
		Blend SrcAlpha One
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
		// make fog work
#pragma multi_compile_fog

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

	float _Alpha;
	float _Red;
	float _Green;
	float _Blue;

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);

		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		return float4(_Red,_Green,_Blue,_Alpha);
	}
			ENDCG
		}
	}
}
