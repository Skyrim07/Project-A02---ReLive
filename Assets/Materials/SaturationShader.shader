Shader "Hidden/SaturationShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise", 2D) = "white"{}
    }
    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float luminance(fixed4 col) {
                return col.r * 0.2 + col.g * 0.7 + col.b * 0.1;
            }

            sampler2D _MainTex;
            sampler2D _NoiseTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float l = luminance(col);
                fixed4 lcol = fixed4(l, l, l, 1);
                i.uv.x += sin(i.uv.y * 30+sin(i.uv.y*20 + _Time.y * 2) +_Time.y*4)*.007;

                col = lerp(col, lcol, step(i.uv.x, 0.5));

                return col;
            }
            ENDCG
        }
    }
}
