Shader "Unlit/HighlightObject"
{
   Properties
    {
        _MainTex ("Base Texture", 2D) = "white" { }
        _LightenFactor ("Lighten Factor", Range(0, 1)) = 0.2
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }

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

            sampler2D _MainTex;
            float _LightenFactor;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                // Sample the texture
                half4 col = tex2D(_MainTex, i.uv);

                // Lighten the texture by interpolating between the original color and white
                col.rgb += _LightenFactor * (1.0 - col.rgb);

                // Return the final color
                return col;
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}
