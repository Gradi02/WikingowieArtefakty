Shader "Custom/WaterShaderV2"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Speed ("Wave Speed", Range(0, 10)) = 1
        _Amplitude ("Wave Amplitude", Range(0, 1)) = 0.1
        _Frequency ("Wave Frequency", Range(0, 5)) = 1
        _WaveHeight ("Wave Height", Range(-1, 1)) = 0.5
		_WaveColor ("Wave Color", Color) = (1.0, 1.0, 1.0, 1.0)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float _Speed;
            float _Amplitude;
            float _Frequency;
            float _WaveHeight;
			float4 _WaveColor;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                float wave = _Amplitude * sin(_Time.y * _Frequency + v.uv.x * 10 + v.uv.y * 10);
                o.vertex.y += wave;
				
				
                if (wave <= _WaveHeight)
                {
					o.color = _WaveColor;
                }
                else
                {
					o.color = float4(0, 0, 0, 1);
                }
                
				
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
			
				half4 col = tex2D(_MainTex, i.uv);
				
				/* to jak chcemy miec solidne dlugie biale paski :>
				
				float wave = _Amplitude * sin(_Time.y * _Frequency + i.uv.x * 10 + i.uv.y * 10);
                if (wave <= _WaveHeight)
                {
					i.color = _WaveColor;
                }
                else
                {
					i.color = float4(0, 0, 0, 1);
                }
				*/
				
				
                return col + i.color;
                
            }
            ENDCG
        }
    }
}
