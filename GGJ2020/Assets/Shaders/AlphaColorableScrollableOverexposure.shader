﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "GGJ2020/VFX/Alpha/colorable + scrollable + overexposure"
{
	Properties
	{
		_MainTex("Texture Mask (RGB)", 2D) = "white" {}
		_ScrollSize("Size XY, Scrollamount XY", Vector) = (1,1,0,0)
		_AScale("Alpha Scale", Float) = 1.0
		_Multiply("Overexposure", Float) = 1.0
		_SSMin("Smoothstep Mask Min", Range(0,1)) = 0.1
		_SSMax("Smoothstep Mask Max", Range(0,1)) = 1.0
		
		//_Color1 ("Primary Color", Color) = (1,1,1,0)
	    _Color2 ("Secondary Color", Color) = (1,1,1,0)
        _Color2Mix("Secondary Color mix", Float) = 1.0
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
	fixed4 _Color2;

	v2f vert(appdata_t v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.color = v.color;
		o.texcoord = (v.texcoord + fixed2(_Time.z * _ScrollSize.z , _Time.z * _ScrollSize.w)) * fixed2(_ScrollSize.x, _ScrollSize.y);
		return o;
	}


	float _Multiply, _AScale, _SSMin, _SSMax, _Color2Mix;

	fixed4 frag(v2f i) : COLOR
	{

        fixed3 fxTex = tex2D(_MainTex, i.texcoord) ;

		fixed4 composite = fixed4(i.color.rgb + fxTex.r *  i.color.a * _Multiply, fxTex.r * i.color.a * _AScale);
		//composite += fxTex.g * _Color2 * i.color.a * _Color2Mix;
		
		composite = lerp(composite, _Color2, fxTex.g * i.color.a * _Color2Mix);
		return fixed4(composite.rgb, smoothstep(_SSMin, _SSMax, composite.a));
		//return composite;
	}
		ENDCG
	}
	}
	}
}


