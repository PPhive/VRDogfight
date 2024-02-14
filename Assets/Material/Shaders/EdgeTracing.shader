Shader "Custom/EdgeDetection" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _EdgeThreshold ("Edge Threshold", Range(0, 1)) = 0.5
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        float _EdgeThreshold;

        struct Input {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutput o) {
            float2 texelSize = 1.0 / _ScreenParams.xy;

            float left = tex2D(_MainTex, IN.uv_MainTex - float2(texelSize.x, 0)).r;
            float right = tex2D(_MainTex, IN.uv_MainTex + float2(texelSize.x, 0)).r;
            float bottom = tex2D(_MainTex, IN.uv_MainTex - float2(0, texelSize.y)).r;
            float top = tex2D(_MainTex, IN.uv_MainTex + float2(0, texelSize.y)).r;

            float2 sobelHorizontal = float2(left - right, bottom - top);
            float2 sobelVertical = float2(bottom - top, left - right);
            float sobelHorizontalMag = length(sobelHorizontal);
            float sobelVerticalMag = length(sobelVertical);
            float edgeMagnitude = max(sobelHorizontalMag, sobelVerticalMag);

            float edge = step(_EdgeThreshold, edgeMagnitude);

            o.Albedo = edge * float3(0, 0, 0); // Black outline
            o.Alpha = 1.0 - edge; // Transparent inside the object
        }
        ENDCG
    }
    FallBack "Diffuse"
}
