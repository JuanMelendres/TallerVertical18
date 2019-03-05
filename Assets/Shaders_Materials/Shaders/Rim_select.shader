Shader "Custom/Rim_select" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", 2D) = "black" {}
		_Metallic ("Metallic", 2D) = "black" {}
		_RimPower("Rim power", Range(0.1, 20.0)) = 2.0
		_RimColor("Rim color", Color) = (1.0, 1.0, 1.0, 1.0)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float3 worldNormal;
			float3 viewDir;
			float2 uv_MainTex;
		};
		float _True;
		float _RimPower;
		float4 _RimColor;
		sampler2D _Glossiness;
		sampler2D _Metallic;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			float NdotV=1.0f-saturate(dot(IN.worldNormal,IN.viewDir));
			fixed rim= pow(NdotV,_RimPower)*_True;
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = lerp(c.rgb,_RimColor ,rim);
			// Metallic and smoothness come from slider variables
			o.Metallic = tex2D (_Metallic, IN.uv_MainTex).xyz;
			o.Smoothness = tex2D (_Glossiness, IN.uv_MainTex).xyz;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
