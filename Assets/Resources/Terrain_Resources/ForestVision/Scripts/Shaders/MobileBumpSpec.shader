// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Simplified Bumped Specular shader. Differences from regular Bumped Specular one:
// - no Main Color nor Specular Color
// - specular lighting directions are approximated per vertex
// - writes zero to alpha channel
// - Normalmap uses Tiling/Offset of the Base texture
// - no Deferred Lighting support
// - no Lightmap support
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "ForestVision/Mobile Bumped Specular" {
Properties {
    [PowerSlider(5.0)] _Shininess ("Shininess", Range (0.03, 1)) = 0.078125
    _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
    
    
}
SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 250

CGPROGRAM
#pragma surface surf MobileBlinnPhong exclude_path:prepass nolightmap noforwardadd halfasview interpolateview

inline fixed4 LightingMobileBlinnPhong (SurfaceOutput s, fixed3 lightDir, fixed3 halfDir, fixed atten)
{
    fixed diff = max (0, dot (s.Normal, lightDir));
    fixed nh = max (0, dot (s.Normal, halfDir));
    fixed spec = pow (nh, s.Specular*128) * s.Gloss;

    fixed4 c;
    c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * spec) * atten;
    UNITY_OPAQUE_ALPHA(c.a);
    return c;
}

sampler2D _MainTex;
sampler2D _BumpMap;
half _Shininess;

struct Input {
    float2 uv_MainTex;
    float3 cameraRelativeWorldPos;
     INTERNAL_DATA
};

void surf (Input IN, inout SurfaceOutput o) {
    fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
    o.Albedo = tex.rgb;
    o.Gloss = tex.a;
    o.Alpha = tex.a;
    o.Specular = _Shininess;
    // flat world normal from position derivatives
            half3 flatWorldNormal = normalize(cross(ddy(IN.cameraRelativeWorldPos.xyz), ddx(IN.cameraRelativeWorldPos.xyz)));
 
            // construct world to tangent matrix
            half3 worldT =  WorldNormalVector(IN, half3(1,0,0));
            half3 worldB =  WorldNormalVector(IN, half3(0,1,0));
            half3 worldN =  WorldNormalVector(IN, half3(0,0,1));
            half3x3 tbn = half3x3(worldT, worldB, worldN);
 
            // apply world to tangent transform to flat world normal
            o.Normal = mul(tbn, flatWorldNormal);
}
ENDCG
}

FallBack "Mobile/VertexLit"
}
