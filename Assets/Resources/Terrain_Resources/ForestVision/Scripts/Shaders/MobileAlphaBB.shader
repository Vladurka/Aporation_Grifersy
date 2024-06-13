// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "ForestVision/MobileAlphaBB" {
Properties {
    _Color ("Main Color", Color) = (1,1,1,1)
    _MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
    

    // These are here only to provide default values
    _Cutoff ("Alpha cutoff", Range(0,1)) = 0.3
     [HideInInspector] _TreeInstanceColor ("TreeInstanceColor", Vector) = (1,1,1,1)
    [HideInInspector] _TreeInstanceScale ("TreeInstanceScale", Vector) = (1,1,1,1)
    [HideInInspector] _SquashAmount ("Squash", Float) = 1
}

SubShader {
    Tags { "IgnoreProjector"="True" "RenderType"="TreeTransparentCutout" }
    Cull Off
    LOD 200

CGPROGRAM
#pragma surface surf TreeLeaf alphatest:_Cutoff vertex:TreeVertLeaf addshadow nolightmap noforwardadd
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

Dependency "OptimizedShader" = "Hidden/Nature/Tree Creator Leaves Optimized"
FallBack "Diffuse"
}
