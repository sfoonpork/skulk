Shader "Custom/Emission"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Strength ("Strength", Range(0,10)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        Cull Off

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 viewDir;
        };

        fixed4 _Color;
        half _Strength;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float3 normal = normalize(o.Normal);
            float3 viewDir = normalize(IN.viewDir);
            o.Alpha = _Color.a;
            o.Emission = _Color.rgb * _Strength;
            if (dot(normal, viewDir) > 0)
            {
                discard;
            }
        }
        ENDCG
    }
    FallBack "Diffuse"
}
