Shader "Unlit/Radial_play"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_SecTex("Texture",2D)="white"{}
		_Radial ("Radial", Range(0.0,1.2))=0.0
		_Mask("Alpha",2D)= "white"{}
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }

		LOD 100

		Pass
		{
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
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _SecTex;
			sampler2D _Mask;
			float _Radial;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed mask= tex2D(_Mask,i.uv)*_Radial;
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 sec = tex2D(_SecTex, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return lerp(sec,col,mask.r);
			}
			ENDCG
		}
	}
}
