Shader "Custom/VR_DoubleCircle_UV"
{
    Properties
    {
        _InnerColor ("Inner Color", Color) = (0,1,1,0.4)
        _OuterColor ("Outer Color", Color) = (1,1,1,0.9)

        _Radius ("Current Radius", Float) = 0
        _MaxRadius ("Max Radius", Float) = 10

        _Softness ("Softness", Float) = 0.05
        _OuterThickness ("Outer Thickness", Float) = 0.05
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float _Radius;
            float _MaxRadius;
            float _Softness;
            float _OuterThickness;

            float4 _InnerColor;
            float4 _OuterColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 p = i.uv * 2 - 1;
                float dist = length(p);

                float r = _Radius / _MaxRadius;

                float inner = 1 - smoothstep(r - _Softness, r, dist);

                float outer = smoothstep(1 - _OuterThickness, 1, dist) * (1 - smoothstep(1, 1 + _OuterThickness, dist));

                float4 col = _InnerColor * inner + _OuterColor * outer;

                return col;
            }
            ENDCG
        }
    }
}