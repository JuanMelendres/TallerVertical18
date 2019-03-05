// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
Shader "Custom/Week1/CubeMapReflection"
{
	Properties
	{
		_Color("Color_Dirty", Color)= (0,0,0,0)		
		_Mask("Dirt",2D)= "white"{}
	}

	SubShader
	{
		Pass
		{

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			// _CubeMap generated from Unity and sent each frame to the shader
			samplerCUBE _CubeMap;
			sampler2D _Mask;
			float4 _Color;
	
			struct appdata
			{
				float2 uv:TEXCOORD0;
				// vertex & normal to "pick up".
				float4 vertex:POSITION;
				float3 normal:NORMAL;
			};

			struct v2f
			{
				float2 uv:TEXCOORD0;		
				// screenSpaceVertex
				float4 screenSpaceVertex:SV_POSITION;
				// worldSpaceNormal, worldSpaceVertex (float3)
				float3 worldSpaceNormal:TEXCOORD1;
				float3 worldSpaceVertex:TEXCOORD2;
			};
			
			v2f vert (appdata v)
			{		
				v2f o;
				o.screenSpaceVertex=UnityObjectToClipPos(v.vertex);
				o.worldSpaceVertex=mul(unity_ObjectToWorld,v.vertex).xyz;
				o.worldSpaceNormal=normalize(mul(unity_ObjectToWorld,float4(v.normal,0.0)).xyz);
				o.uv=v.uv;
				// unity_ObjectToWorld
				// don't forget to only normalize the vector(s)
				
				
				return o;
			}
			
			float4 frag (v2f i) : COLOR
			{
				// 1. Compute the vector viewDir: From the camera to the vertex
				//    _WorldSpaceCameraPos
				float3 viewDir= normalize(i.worldSpaceVertex-_WorldSpaceCameraPos);
				
				// 2. Compute the reflected ray with the reflect function.
				// 		float3 reflect(float3 i, float3 n);  
				//      with i: incidence vector (viewDir here) and n the normal vector.
				float3 ray= reflect(viewDir,normalize(i.worldSpaceNormal));
			
				// 3. To read the _CubeMap, you have to use the function texCUBE.
				//	  texCUBE will return the pixel read in the CubeMap. So a color, so a float4.
				//    Of course we don't use any uvs to read inside the CubeMap.
				return lerp(_Color,texCUBE(_CubeMap,ray),tex2D(_Mask,i.uv).r);
			}

			ENDCG
		}
	}
}
