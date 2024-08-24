Shader "Custom/InvertedMaskShader"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _MaskTex("Mask Texture", 2D) = "white" {}
        _Color("Color Tint", Color) = (0,1,0,1) // Added property for color tint
    }
        SubShader
        {
            Tags { "Queue" = "Overlay" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
            LOD 200

            Pass
            {
                ZWrite Off
                Cull Off
                Fog { Mode Off }
                Blend SrcAlpha OneMinusSrcAlpha

                Stencil
                {
                    Ref 1
                    Comp NotEqual
                    Pass Replace
                }

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata_t
                {
                    float4 vertex : POSITION;
                    float2 texcoord : TEXCOORD0;
                };

                struct v2f
                {
                    float4 vertex : SV_POSITION;
                    float2 texcoord : TEXCOORD0;
                };

                sampler2D _MainTex;
                sampler2D _MaskTex;
                float4 _Color; // Color tint property

                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.texcoord = v.texcoord;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    float4 mask = tex2D(_MaskTex, i.texcoord);
                    if (mask.a <= 0.0)
                    {
                        discard; // Discard pixels where the mask alpha is zero (outside the mask)
                    }

                    fixed4 col = tex2D(_MainTex, i.texcoord) * _Color;
                    return col;
                }
                ENDCG
            }
        }
            FallBack "UI/Default"
}
