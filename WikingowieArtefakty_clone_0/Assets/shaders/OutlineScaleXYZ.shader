Shader "Custom/Outlined_ScaleXYZ"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color("Main Color", Color) = (1,1,1,1)
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_OutlineWidthX ("Outline X", Range (0.0, 2.0)) = 0.07
		_OutlineWidthY ("Outline Y", Range (0.0, 2.0)) = 0.00
		_OutlineWidthZ ("Outline Z", Range (0.0, 2.0)) = 0.07
	}

	CGINCLUDE
	#include "UnityCG.cginc"

	struct appdata
	{
		float4 vertex : POSITION;
	};

	struct v2f
	{
		float4 pos : POSITION;
	};

	uniform float _OutlineWidthX;
	uniform float _OutlineWidthY;
	uniform float _OutlineWidthZ;
	uniform float4 _OutlineColor;
	uniform sampler2D _MainTex;
	uniform float4 _Color;

	ENDCG

	SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" }
		
		Tags{ "Queue" = "Geometry"}

		CGPROGRAM
		#pragma surface surf Lambert
		 
		struct Input {
			float2 uv_MainTex;
		};
		 
		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
		
		
		Pass //Outline
		{
			ZWrite On
			Cull Front
			//Offset 0,0
			//ColorMask RGB
			//Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag

			v2f vert(appdata v)
			{
				v.vertex.x *= ( 1 + _OutlineWidthX);
				
				v.vertex.y *= ( 1 + _OutlineWidthY);
				
				v.vertex.z *= ( 1 + _OutlineWidthZ);

				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;

			}

			half4 frag(v2f i) : COLOR
			{
				return _OutlineColor;
			}

			ENDCG
		}

		
	}
	Fallback "Diffuse"
}
