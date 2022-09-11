Shader "Custom/MyStandard"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Tags { "LightMode" = "DeferredGBuffer" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "../Include.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 worldNormal : TEXCOORD1;
            };

            float4x4 UNITY_MATRIX_VP,UNITY_MATRIX_M;
            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = mul(UNITY_MATRIX_VP,mul(UNITY_MATRIX_M,float4(v.vertex.xyz,1)));
                o.worldNormal = mul(UNITY_MATRIX_M,float4(v.normal.xyz,1));
                o.uv = v.uv;
                return o;
            }

            struct gbufferOutput
            {
                float4 albedo:SV_Target0;
                float4 specRough:SV_Target1;
                float4 normal:SV_Target2;
                float4 emission:SV_Target3;
            };

            gbufferOutput frag(v2f i)// : SV_Target
            {
                // half4 col = tex2D(_MainTex, i.uv);
                gbufferOutput output;
                output.albedo = tex2D(_MainTex, i.uv);
                output.specRough = 1;

                output.normal = i.worldNormal;
                output.emission = 1;
                return output;
            }
            ENDHLSL
        }
    }
}
