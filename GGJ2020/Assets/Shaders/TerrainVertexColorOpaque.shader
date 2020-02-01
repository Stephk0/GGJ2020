// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "GGJ19/VertexColorUnlit"
{
	Properties
	{
		_MainTex ("Cloud Texture", 2D) = "white" {}
		_CloudScaleScroll ("CloudScale X,CloudScale Y,CloudScroll X, CloudScroll Y",Vector) = (16,16,1,0.2)
		_CloudIntensity("Cloud Intensity",Float) = 1.0
		_BaseGradientShift("_BaseGradientShift",Float) = 1.0
		_BaseGradientSize("_BaseGradientSize",Float) = 1.0
		_BaseGradientColor("BasgrdientColor", Color) = (1,1,1,1)
		_Overexposure("Overexposure",Float) = 1.0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
			//Cull Off Lighting Off
			//ZWrite On

		Pass
		{
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
				float2 uv2 : TEXCOORD1;
				fixed4 color : COLOR;
				float3 worldPos : TEXCOORD2;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float3 worldPos : TEXCOORD2;
				float3 normal : NORMAL;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _CloudScaleScroll;
			float _BaseGradientShift, _BaseGradientSize, _Overexposure;
			fixed3 _BaseGradientColor;
			
			v2f vert (appdata v)
			{
				v2f o;
				//o.vertex.xz += _SinTime.y;
				//o.vertex = UnityObjectToClipPos(v.vertex);
				o.vertex = UnityObjectToClipPos(v.vertex);
				//o.vertex.xz += _SinTime.y;

				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				o.worldPos = mul(unity_ObjectToWorld, v.vertex);

				o.uv = o.worldPos.xz / float2(_CloudScaleScroll.x, _CloudScaleScroll.y);
				o.uv += float2 (_Time.x * _CloudScaleScroll.z, _Time.x * _CloudScaleScroll.w);

				o.uv2 = o.worldPos.xz / float2(_CloudScaleScroll.x * 0.5, _CloudScaleScroll.y) * 0.5;
				o.uv2 += float2 (_Time.x * _CloudScaleScroll.z * 0.5, _Time.x * _CloudScaleScroll.w * 0.5);
				o.normal = v.normal;

				//mul(_Object2World, v.vertex).xyz;
				o.color.rgb = v.color.rgb;
				o.color.rgb *= saturate((saturate(o.worldPos.y * _BaseGradientSize + _BaseGradientShift) + _BaseGradientColor) )  *_Overexposure;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			float _CloudIntensity;

			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = i.color;
				fixed overlay	= tex2D(_MainTex, i.uv).r;

				//col.a = 2;
				col.rgb *= lerp(1, saturate(overlay + abs(1-i.normal.z)) , _CloudIntensity);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
