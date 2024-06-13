Shader "ForestVision/MobileEmit" {
    Properties {
    _Color("Color", Color) = (1.000000,1.000000,1.000000,1.000000)
    _EmissionLevel ("Emission Level", Range(0,20) ) = 5.0
}
SubShader {
    Tags 
{ 
    "RenderType"="Opaque"
}
    LOD 150

    CGPROGRAM
    // Physically based Standard lighting model, and enable shadows on all light types
    #pragma surface surf Lambert noforwardadd

    float4 _Color;
    float _EmissionLevel;

    struct Input {
        half color : COLOR;
    };
    void surf (Input IN, inout SurfaceOutput o) {
        o.Albedo = _Color * _EmissionLevel;
    }
    ENDCG
}
FallBack "Diffuse"  }