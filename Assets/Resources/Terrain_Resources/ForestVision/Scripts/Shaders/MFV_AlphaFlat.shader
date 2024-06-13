Shader "MobileFV/MFV_Alpha_Flat" {
Properties {
    _Color ("Main Color", Color) = (1,1,1,1)
    _MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
    _Cutoff ("Alpha cutoff", Range(0,1)) = 0.3
}

SubShader {
    Tags { "RenderType"="TreeTransparentCutout" }
    Cull Off
    LOD 200

CGPROGRAM
#pragma surface surf TreeLeaf alphatest:_Cutoff  addshadow 
#include "UnityBuiltin3xTreeLibrary.cginc"

sampler2D _MainTex;


struct Input {
    float2 uv_MainTex;
    fixed4 color : COLOR; // color.a = AO
};

void surf (Input IN, inout LeafSurfaceOutput o) {
    fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
    o.Albedo = c.rgb * IN.color.rgb * IN.color.a;
    
    o.Alpha = c.a;
    
}
ENDCG
}

Fallback "Transparent/Cutout/VertexLit"
}
