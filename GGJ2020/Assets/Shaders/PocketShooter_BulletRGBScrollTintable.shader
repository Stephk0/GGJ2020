// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// straight from madfinger into our game, yeharw!

Shader "Pocket Shooter/Game/Bullet 3 Layer" {

Properties {
	_MainTex ("Layer 1 (R) Layer 2 (G) Layer 3 (B) ", 2D) = "white" {}
	_Color2 ("Layer 2 & 3 Color Tint", Color) = (1,1,1,0)
	_Multiplier("Multiplier", float) = 1
	_Scrolltime1("Layer 1 Scrolltime (no worky)", float) = 5
	_Scrolltime2("Layer 2 Scrolltime", float) = 2.5
	_Scrolltime3("Layer 2 Scrolltime", float) = 1
	
	_Scale1("Layer 1 Scale", float) = 1
	_Scale2("Layer 2 Scale", float) = 1
	_Scale3("Layer 3 Scale", float) = 1
}

	
SubShader {
	
	
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	
//	Blend One One
	Blend One OneMinusSrcColor
	Cull Back Lighting Off Fog { Mode Off }
	ZWrite Off
	
	LOD 100
	
	CGINCLUDE	
	#include "UnityCG.cginc"
	sampler2D _MainTex;
	
//	float _FadeOutDistNear;
//	float _FadeOutDistFar;
	float _Multiplier;
//	float _ContractionAmount;
	float _Scrolltime1,_Scrolltime2,_Scrolltime3;
	float _Scale1,_Scale2,_Scale3;
	fixed4 _Color2;
	
	
	struct v2f {
		float4	pos	: SV_POSITION;
		float2	uv1		: TEXCOORD0;
		float2	uv2		: TEXCOORD1;
		float2	uv3		: TEXCOORD2;
		fixed4	color	: TEXCOORD3;
	};
	
	v2f vert (appdata_full v)
	{
		v2f 		o;
		
		//o.uv1	= v.texcoord.xy  * float2(_Scale1,1.0) - float2(_Time.x * _Scrolltime1, 0.0 );
		o.uv1	= v.texcoord.xy ;
		o.uv2	= v.texcoord.xy  * float2(_Scale2,1.0) - float2(_Time.x * _Scrolltime2, 0.0 );
		o.uv3	= v.texcoord.xy  * float2(_Scale3,1.0) - float2(_Time.x * _Scrolltime3, 0.0 );
		o.color	= v.color * _Multiplier;
		o.pos	= UnityObjectToClipPos(v.vertex);
		
						
		return o;
	}
	ENDCG


	Pass {
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma fragmentoption ARB_precision_hint_fastest		
		fixed4 frag (v2f i) : COLOR
		{	
			
			fixed4 mainTex = tex2D(_MainTex, i.uv1).r * _Multiplier;
			mainTex.rgb *= i.color.rgb;
			mainTex.rgb += (tex2D(_MainTex, i.uv2).g + tex2D(_MainTex, i.uv3).b) * _Color2 * i.color.a; 
			return mainTex;// * _Color;
		}
		ENDCG 
	}	
}


}

