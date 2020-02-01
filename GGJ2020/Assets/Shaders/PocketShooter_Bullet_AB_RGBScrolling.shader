// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Bullet Alpha Blend Scrolling

Shader "FPS/Effects/Trails" {

Properties {
	_MainTex ("Bullet (R) Trail (G) Additive Trail (B) ", 2D) = "white" {}
	_Color1 ("Primary Color", Color) = (1,1,1,0)
	_Color2 ("Secondary Color", Color) = (1,1,1,0)
	_Multiplier("Additive Trail Multiplier", float) = 1
	_AMultiplier("Alpha Scale", float) = 2
	
	_HeatWeight("Heat Weight", Range(0,1)) = 1
	_MeshScaleFactor("Mesh Scale factor",float) = 2
	_Scrolltime1("Layer 1 Scrolltime", float) = 2.5
	_Scrolltime2("Layer 2 Scrolltime", float) = 1
	
	_Scale1("Layer 1 Scale", float) = 1
	_Scale2("Layer 2 Scale", float) = 1
	_Scale3("Layer 3 Scale", float) = 1
	
	_Debug("Debug", Range(0,1)) = 1
}

	
SubShader {
	
	
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	

	//Blend One OneMinusSrcColor
	Blend SrcAlpha OneMinusSrcAlpha
		BindChannels{
		Bind "Color", color
		Bind "Vertex", vertex
		Bind "TexCoord", texcoord
	}
	//Blend One OneMinusSrcColor
	
	Cull Off Lighting Off Fog { Mode Off }
	ZWrite Off
	
	LOD 100
	
	CGINCLUDE	
	#include "UnityCG.cginc"
	sampler2D _MainTex;
	
//	float _FadeOutDistNear;
//	float _FadeOutDistFar;
	float _Multiplier,_AMultiplier;
	float _HeatWeight;
	float _MeshScaleFactor;
	float _Scrolltime1;
	float _Scale1,_Scale2,_Scale3;
	fixed3 _Color1, _Color2;
	float _Debug;
	
	
	struct v2f {
		float4	pos	: SV_POSITION;
		float2	uv1		: TEXCOORD0;
		float2	uv2		: TEXCOORD1;
		float2	uv3		: TEXCOORD2;
		fixed4	color	: TEXCOORD3;
	};
	
	// vertex color assignments
	//R = Overallmask  		vertex mask used for overall masking in combination with fading mip maps vanishing of particle in distance
	//G = Scrollmask  		vertec mask for trail scrolling bits. prevent scrolling on the tip of the bullet with this, as well as fade out at tail, here we could add vanishing of trails
	//G = Heatweight  	could be open due to overall mask doing the job
	//B = speed   			passed value by script. Speed x from weaponDefintions is passed as x / 100 to normalize to one due to fixed vertexcolors, then mutliplied back in shader
	//A = particle scale 	passed value by script. Inital Particle size affects trail scrolling speed. here we could also add compressing/decompressiong trails.
	//
	
	
	
	v2f vert (appdata_full v)
	{
		v2f 		o;
		
		// passed values by particle system and script. script sets particleSystem.startColor according to speed and size of particle
		_Scrolltime1 *= normalize(v.color.b) * 100.0;

		_MeshScaleFactor *= (v.color.a) * 16.0;
		
		o.uv1	= v.texcoord.xy  ;
		o.uv1.x += v.color.g -1;
		//o.uv2	= v.texcoord.xy * float2(_Scale2 / _MeshScaleFactor, 1.0) - float2(_Time.y * (_Scrolltime1 / _MeshScaleFactor ) , 0.0 );
		
		float4 vertexpush = v.vertex;
		vertexpush.z -=  0.1  * (1-v.texcoord.x) * (1-v.color.r);
		//vertexpush.x -= 5 * ( v.texcoord.x) * (1 - v.color.r);
		vertexpush.z *=  saturate((1 - v.texcoord.x) * ( v.color.r * v.color.g) + 0.7);
		o.uv2	= v.texcoord.xy;
		o.uv2	-= float2(_Time.y * (_Scrolltime1 / _MeshScaleFactor) , 0);
		
		float pos = length(mul (UNITY_MATRIX_MV, v.vertex).xyz);
      	float diff = _MeshScaleFactor;// - unity_FogStart.x;
      	float invDiff = 1.0f / diff;
      	
      	//float finaldistance = (_MeshScaleFactor - pos) * invDiff;
      	float finaldistance = clamp ((_MeshScaleFactor - pos) * invDiff, 0.0, 16.0);
		
		//o.uv2	= v.texcoord.xy - float2(finaldistance , 1.0);
		
		//o.uv3	= v.texcoord.xy * float2(_Scale3 / _MeshScaleFactor,1.0) - float2(_Time.y * (_Scrolltime1 / _MeshScaleFactor ) , 0.0 );
		
		
		o.color	= v.color;// * _Multiplier;
		o.pos	= UnityObjectToClipPos(vertexpush);
//		
//		o.color.r = _MeshScaleFactor;
//		o.color.g = _MeshScaleFactor;
//		o.color.b = _MeshScaleFactor;
//		o.color.a = _MeshScaleFactor;
		
		
						
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
			
			fixed mainTex = tex2D(_MainTex, i.uv1).r ;

			mainTex *= tex2D(_MainTex, i.uv2).g * i.color.r * 2 * i.color.g; 

			fixed heat = tex2D(_MainTex, i.uv2).b * i.color.r* i.color.g* i.color.g;
			mainTex += heat ;
			
			_HeatWeight *= i.color.g * 2;
			
			fixed4 comp = fixed4(lerp(_Color2  ,_Color1 + heat * _Multiplier, saturate((mainTex - _HeatWeight)) ), saturate(mainTex * _AMultiplier *  i.color.r));
			return lerp(comp, i.color, _Debug) ;
		}
		ENDCG 
	}	
}


}

