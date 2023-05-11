Shader "Custom/Hose_Surface"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _MetallicTex("Metallic Texture", 2D) = "white" {}
        _NormalMap("Normal Map", 2D) = "bump" {}
        _ExtrusionMaskTex("Extrusion Mask Texture", 2D) = "white" {}
        _ExtrusionMultiplier("Extrusion Multiplier", Float) = 1.0
        _ScrollSpeed("Scroll Speed", Float) = 1.0
        _TilingAmount("Tiling Amount", Float) = 1.0
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _MetallicTex;
        sampler2D _NormalMap;
        sampler2D _ExtrusionMaskTex;
        float _ExtrusionMultiplier;
        float _ScrollSpeed;
        float _TilingAmount;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_MetallicTex;
            float2 uv_NormalMap;
            float2 uv_ExtrusionMaskTex;
        };

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
        // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void vert(inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            float4 uv = v.texcoord;
            uv.xy = v.texcoord.xy * float2(2, _TilingAmount) + float2(2, _Time.y * _ScrollSpeed);
            fixed4 pixel = tex2Dlod(_ExtrusionMaskTex, uv);
            v.vertex = float4(v.normal * pixel * _ExtrusionMultiplier * 0.1, 0) + v.vertex;
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            //计算法线信息
            IN.uv_ExtrusionMaskTex *= _TilingAmount;
            IN.uv_ExtrusionMaskTex += _Time.y * _ScrollSpeed;
            float4 extrusionColor = tex2D(_ExtrusionMaskTex, IN.uv_ExtrusionMaskTex);
            extrusionColor *= _ExtrusionMultiplier;
            float3 normal = UnpackNormal(tex2D(_NormalMap, IN.uv_NormalMap));
            normal = lerp(normal, float3(0, 0, 1), extrusionColor);
            //类似光纤的效果
            // normal.y = lerp(normal.y, 1, extrusionColor.r);
            o.Normal = normal;


            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
            float4 metallic = tex2D(_MetallicTex, IN.uv_MetallicTex);
            o.Metallic = metallic.rgb;
            o.Smoothness = metallic.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}