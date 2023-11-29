Shader "Custom/waterSurfaceShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Speed ("Wave Speed", Range(0, 10)) = 1
        _Amplitude ("Wave Amplitude", Range(0, 1)) = 0.1
        _Frequency ("Wave Frequency", Range(0, 5)) = 1
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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
 
            sampler2D _MainTex;
            float _Speed;
            float _Amplitude;
            float _Frequency;
 
            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                // Apply a simple sine wave displacement to the Y coordinate of the vertex.
                float wave = _Amplitude * sin(_Time.y * _Frequency + v.uv.x * 10 + v.uv.y * 10);
                o.vertex.y += wave;

                return o;
            }
 
            half4 frag (v2f i) : SV_Target
            {
                // Sample the main texture and output the result.
                half4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}