Shader "MobileFV/MFV_Snow_Animated" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
        _VertexWeight ("Vertex Weight", 2D) = "weight" {} //added new weight map slot to drive vertex animation

        _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    
    
        _Bump ("Bump", 2D) = "bump" {}
        _Snow ("Snow Level", Range(0,1) ) = 0
        _SnowColor ("Snow Color", Color) = (1.0,1.0,1.0,1.0)
        [Toggle(ENABLE_SNOWTEX)] _EnableSnow ("Snow Texture?", Float) = 0
        _SnowTex ("Snow (RGB)", 2D) = "white" {}
        _SnowDirection ("Snow Direction", Vector) = (0,1,0)
        
        _ShakeDisplacement ("Scale", Range (0, 10.0)) = 0.53
        _ShakeTime ("Animation Time", Range (0, 1.0)) = .42
        _ShakeWindspeed ("Wind Speed", Range (0, 5.0)) = 2.7
        _ShakeBending ("Displacement", Range (0, 1.0)) = 0.186
    }
    SubShader {
        Tags { "RenderType"="TreeTransparentCutout" }
        Cull Off
        LOD 200
 
        CGPROGRAM
        #pragma surface surf Lambert alphatest:_Cutoff vertex:vert addshadow
        #include "UnityBuiltin3xTreeLibrary.cginc"
 
        sampler2D _MainTex;
        sampler2D _SnowTex;
        sampler2D _VertexWeight;
        sampler2D _Bump;
        float _EnableSnow;
        float _Snow;
        float4 _SnowColor;
        float4 _SnowDirection;
        
        float _ShakeDisplacement;
        float _ShakeTime;
        float _ShakeWindspeed;
        float _ShakeBending;
 
        struct Input {
            fixed4 color : COLOR; // color.a = AO
            float2 uv_MainTex;
            float2 uv_SnowTex;
            float2 uv_Bump;
            float3 worldNormal;
            INTERNAL_DATA
        };
  
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
            fixed4 baseColor = tex2D (_MainTex, IN.uv_MainTex);
            half4 snowTexture = tex2D (_SnowTex, IN.uv_SnowTex);
            o.Normal = UnpackNormal (tex2D (_Bump, IN.uv_Bump));

            if(dot(WorldNormalVector(IN, o.Normal), _SnowDirection.xyz)>=lerp(1,-1,_Snow))
            {
                //perform final coloring
                if(_EnableSnow == 1)
                {
                    o.Albedo = snowTexture.rgb * _Color.rgb;
                }else{
                    o.Albedo = _SnowColor.rgb *_Color.rgb;
                }
            }
            else {
                o.Albedo = baseColor.rgb;
            }
            o.Alpha = baseColor.a;
        }
        ENDCG
    }
    Fallback "Transparent/Cutout/VertexLit"
}