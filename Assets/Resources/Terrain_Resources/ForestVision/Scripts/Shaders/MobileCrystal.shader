// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "ForestVision/MobileCrystal"
{
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        [PowerSlider(5.0)] _Shininess ("Shininess", Range (0.03, 1)) = 0.078125
        _RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
      _RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
 
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:vert lightModel 
        #pragma target 3.0
 
        sampler2D _MainTex;
        float4 _RimColor;
      float _RimPower;
 
        struct Input {
            float2 uv_MainTex;
            float3 cameraRelativeWorldPos;
            float3 worldNormal;
            float3 viewDir;
            INTERNAL_DATA
        };
 
        half _Shininess;
        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
 
        // pass camera relative world position from vertex to fragment
        void vert(inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input,o);
            o.cameraRelativeWorldPos = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0)) - _WorldSpaceCameraPos.xyz;
        }
 
        void surf (Input IN, inout SurfaceOutputStandard o) {
 
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb * _Color.rgb;
 
            // flat world normal from position derivatives
            half3 flatWorldNormal = normalize(cross(ddy(IN.cameraRelativeWorldPos.xyz), ddx(IN.cameraRelativeWorldPos.xyz)));
 
            // construct world to tangent matrix
            half3 worldT =  WorldNormalVector(IN, half3(1,0,0));
            half3 worldB =  WorldNormalVector(IN, half3(0,1,0));
            half3 worldN =  WorldNormalVector(IN, half3(0,0,1));
            half3x3 tbn = half3x3(worldT, worldB, worldN);
 
            // apply world to tangent transform to flat world normal
            o.Normal = mul(tbn, flatWorldNormal);
             o.Smoothness = _Shininess;
             half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
              o.Emission = _RimColor.rgb * pow (rim, _RimPower);
        }
        ENDCG
    }
    FallBack "Diffuse"
}