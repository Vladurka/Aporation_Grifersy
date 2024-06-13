// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "MobileFV/MFV_Snow" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
        _Cutoff ("Alpha cutoff", Range(0,1)) = 0.3
        _Bump ("Bump", 2D) = "bump" {}
        _SnowLevel ("Snow Level", Range(0,1) ) = 0
        [Toggle(ENABLE_SNOWTEX)] _EnableSnow ("Snow Texture?", Float) = 0
        _SnowColor ("Snow Color", Color) = (1.0,1.0,1.0)
        _SnowTex ("Snow (RGB)", 2D) = "white" {}
        _SnowDirection ("Snow Direction", Vector) = (0,1,0)
        
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
        sampler2D _Bump;
        float _EnableSnow;
        float _SnowLevel;
        float3 _SnowColor;
        float4 _SnowDirection;
        
 
        struct Input {
            float2 uv_MainTex;
            float2 uv_SnowTex;
            float2 uv_Bump;
            float3 worldNormal;
            INTERNAL_DATA
        };
 
        void vert (inout appdata_full v) {
            
        }
 
        void surf (Input IN, inout SurfaceOutput o) {

       		// get main textures
            fixed4 baseColor = tex2D (_MainTex, IN.uv_MainTex); 
            fixed3 snowColor = tex2D (_SnowTex, IN.uv_SnowTex); 
            
            //calculate normals
            fixed3 normalMain = UnpackNormal(tex2D(_Bump, IN.uv_Bump)); //get main bump
    		
    		o.Normal = normalMain;
    		
           
            if(dot(WorldNormalVector(IN, o.Normal), _SnowDirection.xyz)>=lerp(1,-1,_SnowLevel))
            {
                //perform final coloring
                if(_EnableSnow == 1)
                {
                    o.Albedo = snowColor.rgb * _Color.rgb;
                }else{
                    o.Albedo = _SnowColor.rgb * _Color.rgb;
                }
                
            }
            else {
                o.Albedo = baseColor.rgb;
            }
            o.Alpha = baseColor.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}