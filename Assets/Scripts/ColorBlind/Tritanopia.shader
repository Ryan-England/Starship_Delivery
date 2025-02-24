Shader "Custom/Tritanopia"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
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

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // Color transformation matrix for Protanopia
                fixed3x3 tritanopiaMatrix = fixed3x3(
                    0.950, 0.050, 0.000,
                    0.000, 0.433, 0.567,
                    0.000, 0.475, 0.525
                );


                col.rgb = mul(tritanopiaMatrix, col.rgb);
                return col;
            }
            ENDCG
        }
    }
}
