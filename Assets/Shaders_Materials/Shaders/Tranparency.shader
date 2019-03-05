Shader "Unlit/Tranparency"
{
	Properties
	{
		_Color("Color", Color) = (0,0,0,1)
		_Transparent("Tranparency", Range(0.0,1.0))=0.25
		
	}
	SubShader
	{
		
		Tags { "Queue"="Transparent" "RenderType"="Transparent"}
		LOD 100
		ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

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
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			float4 _Color;
			float _Transparent;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv =v.uv;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = _Color;
				col.a=_Transparent;
				return col;
			}
			ENDCG
		}
	}
}
