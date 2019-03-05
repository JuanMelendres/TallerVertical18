Shader "Unlit/History_pass"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_SecTex ("Texture", 2D) = "white" {}
		_fade ("Fade", Range(0.0,1.0))=0.0
		_lerp ("Lerp", Range(0.0,1.0))=0.0
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

			sampler2D _MainTex;
			sampler2D _SecTex;
			float _lerp;
			float _fade;

			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = lerp(tex2D(_MainTex, i.uv),tex2D(_SecTex, i.uv),_lerp);
				col.a= _fade;
				// apply fog
				return col;
			}
			ENDCG
		}
	}
}
