// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Tropicats/VFX/Alpha/colorable + overexposure"
{
	Properties
	{
		_MainTex("Texture Mask (RGB)", 2D) = "white" {}
		
		_AScale("Alpha Scale", Float) = 1.0
		_Multiply("Overexposure", Float) = 1.0
		_SSMin("Smoothstep Mask Min", Range(0,1)) = 0.1
		_SSMax("Smoothstep Mask Max", Range(0,1)) = 1.0

	}

		Category
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off Lighting Off ZWrite Off Fog{ Mode Off }

		SubShader{
		Pass{

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_particles

#include "UnityCG.cginc"

		sampler2D _MainTex;
	


	struct appdata_t {
		float4 vertex : POSITION;
		fixed4 color : COLOR;
		float2 texcoord : TEXCOORD0;
	};

	struct v2f {
		float4 vertex : POSITION;
		fixed4 color : COLOR;
		float2 texcoord : TEXCOORD0;

	};

	float4 _ScrollSize;

	v2f vert(appdata_t v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.color = v.color;
		o.texcoord = v.texcoord;
		return o;
	}


	float _Multiply, _AScale, _SSMin, _SSMax;

	fixed4 frag(v2f i) : COLOR
	{

		fixed tex = tex2D(_MainTex, i.texcoord).r;

		fixed4 composite = fixed4(i.color.rgb + tex *  i.color.a * _Multiply, tex * i.color.a * _AScale);
		return fixed4(composite.rgb, smoothstep(_SSMin, _SSMax, composite.a));
		
	}
		ENDCG
	}
	}
	}
}


