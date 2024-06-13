
Shader "MobileFV/MFV_Foliage_Animated" {
    Properties {
    _Color ("Main Color", Color) = (1,1,1,1)
    _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}

    _Bump ("Normal", 2D) = "bump" {}
    _BumpPower ("Normal Power", Range (0.01, 2)) = 1 //added slider control for Normal strength

    _VertexWeight ("Vertex Weight", 2D) = "weight" {} //added new weight map slot to drive vertex animation

    _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    _ShakeDisplacement ("Scale", Range (0, 10.0)) = 0.53
    _ShakeTime ("Animation Time", Range (0, 1.0)) = .42
    _ShakeWindspeed ("Wind Speed", Range (0, 5.0)) = 2.7
    _ShakeBending ("Displacement", Range (0, 1.0)) = 0.186

}
 
SubShader {
    Tags {"Queue"="AlphaTest" "RenderType"="TransparentCutout"}
    LOD 2000
    Cull [_Cull]

   

CGPROGRAM
#pragma target 3.0
#pragma surface surf Lambert alphatest:_Cutoff vertex:vert addshadow
 
sampler2D _MainTex;
sampler2D _Bump;
sampler2D _VertexWeight;

fixed4 _Color;
fixed _BumpPower;
float _ShakeDisplacement;
float _ShakeTime;
float _ShakeWindspeed;
float _ShakeBending;


 
struct Input {
    float2 uv_MainTex;
    float2 uv_Normal;
    float2 uv_Weight;
};
 
void FastSinCos (float4 val, out float4 s, out float4 c) {
    val = val * 3.1415927;
    float4 r5 = val * val;
    float4 r6 = r5 * r5;
    float4 r7 = r6 * r5;
    float4 r8 = r6 * r5;
    float4 r1 = r5 * val;
    float4 r2 = r1 * r5;
    float4 r3 = r2 * r5;
    float4 sin7 = {1, -0.16161616, 0.0083333, -0.00019841} ;
    float4 cos8  = {-0.5, 0.041666666, -0.0013888889, 0.000024801587} ;
    s =  val + r1 * sin7.y + r2 * sin7.z + r3 * sin7.w;
    c = 1 + r5 * cos8.x + r6 * cos8.y + r7 * cos8.z + r8 * cos8.w;
}
 
void vert (inout appdata_full v) {

	float4 WeightMap = tex2Dlod (_VertexWeight, float4(v.texcoord.xy,0,0));

    float factor = (1 - v.color.r) * 0.5;
       
    const float _WindSpeed  = (_ShakeWindspeed  +  v.color.g );    
    const float _WaveScale = _ShakeDisplacement;
   
    const float4 _waveXSize = float4(0.048, _ShakeWindspeed, 0.24, 0.096);
    const float4 _waveZSize = float4 (0.024, .08, 0.08, 0.2);
    const float4 waveSpeed = float4 (1.2, 2, 1.6, 4.8);
 
    float4 _waveXmove = float4(0.024, 0.04, -0.12, 0.096);
    float4 _waveZmove = float4 (0.006, .02, -0.02, 0.1);
   
    float4 waves;
    waves = v.vertex.x * _waveXSize;
    waves += v.vertex.z * _waveZSize;
 
    waves += _Time.x * (1 - _ShakeTime * _WaveScale - v.color.b ) * waveSpeed *_WindSpeed;
 
    float4 s, c;
    waves = frac (waves);
    FastSinCos (waves, s,c);
 
    float waveAmount = WeightMap.r  * (v.color.a + _ShakeBending);
    s *= waveAmount;
 
    s *= normalize (waveSpeed);
 
    s = s * s;
    float fade = dot (s, 1.3);
    s = s * s;
    float3 waveMove = float3 (0,0,0);
    waveMove.x = dot (s, _waveXmove);
    waveMove.z = dot (s, _waveZmove);
    v.vertex.xz -= mul ((float3x3)unity_WorldToObject, waveMove).xz;
   
}
 
void surf (Input IN, inout SurfaceOutput o) {
    fixed4 baseColor = tex2D(_MainTex, IN.uv_MainTex) * _Color;
    o.Albedo = baseColor.rgb;
    o.Alpha = baseColor.a;
    fixed3 normal = UnpackNormal(tex2D(_Bump, IN.uv_Normal)); 
    o.Normal = normalize(normal);
    

}
ENDCG


} //end subshader

 
Fallback "Transparent/Cutout/VertexLit"
}
 
 

