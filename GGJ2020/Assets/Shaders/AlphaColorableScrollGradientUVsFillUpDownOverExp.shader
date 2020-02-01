// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Tropicats/VFX/Alpha/colorable + overexposure + gradientMask + FillUpDown "
{
	Properties
	{
		_MainTex("Texture Mask (RGB)", 2D) = "white" {}
		_UV2Tiling("Extra Tiling", Float) = 1.0
		[PerRendererData]_Distance("Distance set by code", Float) = 1.0
		_AlphaState("Alpha State", Range(0,1)) = 0.5
		_AlphaDir("Alpha State Direction", Range(-1,1)) = 1
		_CutOffSharpness("Cutoff Sharpness", Range(0,1)) = 0.5
		_CutOffSize("Cutoff Size", Range(0.1,20)) = 0.5
		_ScrollSize("Size XY, Scrollamount XY", Vector) = (1,1,0,0)
		_FillInOffset("Fill up Offset", Range(-1,1)) = 0.5
		_AScale("Alpha Scale", Float) = 1.0
		_Multiply("Overexposure", Float) = 1.0
		_SSMin("Smoothstep Mask Min", Range(0,1)) = 0.1
		_SSMax("Smoothstep Mask Max", Range(0,1)) = 1.0
		_DebugTint("Debug Tint", Color) = (1,1,1,1)
		_StartEndGradientFactor("Start End Transparency Factor", float) = 1.0

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
		float2 texcoord2 : TEXCOORD1;
		float gradient : TEXCOORD2;
		fixed offset : TEXCOORD3;
	};

	struct v2f {
		float4 vertex : POSITION;
		fixed4 color : COLOR;
		float2 texcoord : TEXCOORD0;
		float2 texcoord2 : TEXCOORD1;
		float gradient : TEXCOORD2;
		fixed offset : TEXCOORD3;

	};

	float4 _ScrollSize;
	float _UV2Tiling, _AlphaState, _CutOffSize, _CutOffSharpness, _FillInOffset, _Distance, _StartEndGradientFactor, _AlphaDir;
	fixed4 _DebugTint;

	v2f vert(appdata_t v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.color = v.color * _DebugTint;
		o.texcoord = -v.texcoord;
		
		//_AlphaState = _AlphaState + fmod((_Time.z * 0.5 ) , 1);
		o.color.a *= _AlphaState;
		//o.color.a *= _AlphaDir;
		//o.color.a += saturate(-_AlphaDir);
		//
		o.gradient = 1 - pow(((v.texcoord.x + 1 - o.color.a * 2 ) * 2 - 1), 2); // expand alphaState 0 - 1 Range, then map it back to 1 - -1, then use pow to create parabolic curve
		o.gradient = pow(o.gradient , _CutOffSize);  // make the curve bigger or smaller
		o.gradient = smoothstep(_CutOffSharpness * 0.5, 1 - _CutOffSharpness * 0.5, o.gradient); // adjust the sharpness of the edges
		o.gradient *= pow((1-v.texcoord.x) * 2, _StartEndGradientFactor); // mask out the end so the scrolling texture doenst continue at the end point
		o.gradient *= pow((v.texcoord.x) *2, _StartEndGradientFactor);

		//o.offset = fmod((1 - o.color.a) + _FillInOffset, 1); // establish offset for the fill up, fill down animation of RG Texture.
		o.offset = saturate((1 - o.color.a * (1 - _FillInOffset)) - _FillInOffset ); // establish offset for the fill up, fill down animation of RG Texture. Rescale the animation based on offset
		o.offset *= _AlphaDir;
		o.offset += saturate(-_AlphaDir);
		o.texcoord2 =( v.texcoord * fixed2(_UV2Tiling * _Distance,1) - fixed2(_Time.z * _ScrollSize.z, _Time.z * _ScrollSize.w) )* fixed2(_ScrollSize.x, _ScrollSize.y);   // scrolling tex
		return o;
	}


	fixed _Multiply, _AScale, _SSMin, _SSMax;

	fixed4 frag(v2f i) : COLOR
	{
		fixed2 texFill = tex2D(_MainTex, i.texcoord).rg;

		float fade_in_ratio		= saturate(i.offset * 2.0); 		    // this will go from 0 to 1 from the start of the life of the particle to half of its duration (end of apparition)
		float fade_out_ratio	= saturate(i.offset - 0.5f) * 2.0; 	// this will go from 0 to 1 from the half of the life of the particle to the end of its life (end of disparition)

		fixed fadeInFactor	= saturate(texFill.r  * (1 - fade_out_ratio) * 2) ;  // blend in based in fade out ratio
		fixed fadeOutFactor = saturate(texFill.g  * (fade_in_ratio) * 2) ;		// blend out based in fade in ratio

		fixed final_result = (fadeInFactor * smoothstep(0.9, 1.0, fade_in_ratio) + fadeOutFactor * smoothstep(0.9, 1.0, 1 - fade_out_ratio)) ; // blend

		fixed tex = ( tex2D(_MainTex, i.texcoord2).b) * i.gradient;

		fixed4 composite = fixed4(i.color.rgb + (final_result + tex * 2) * 0.5 * _Multiply, (final_result + tex) * 0.5  * _AScale );

		return fixed4(composite.rgb, smoothstep(_SSMin, _SSMax, composite.a));
		//return final_result;
	}
		ENDCG
	}
	}
	}
}


