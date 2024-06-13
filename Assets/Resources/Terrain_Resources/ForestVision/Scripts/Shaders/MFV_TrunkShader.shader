Shader "MobileFV/MFV_TrunkShader" {
    Properties {
        
        [Header(Global Control)] _Intensity ("Overall Intensity", Range(5, 1)) = 1
        [Space(10)]
        
        [Header(Main Texture Controls)] _Color("Main Tint Color", Color) = (0.5,0.5,0.5,1)//grey
        _MainTex ("Base Texture", 2D) = "white" {}
        _Bump ("Normal", 2D) = "bump" {}
        _BumpPower ("Normal Power", Range (0.1, 2)) = 1 //added slider control for Normal strength
               
        [Space(10)]
        
        [Header(Side Texture Controls)] _SideDirection ("Side Direction", Vector) = (0,0,0)
        _SideLevel ("Side Level", Range(0,1) ) = 0.3
        _SideDepth ("Side Depth", Range(0,2)) = 0.7
        _SideColor ("Side Color", Color) = (0.0,0.392,0.0,1.0) //dark green
        _SideTex ("Side Texture", 2D) = "white" {}
       
    }
    SubShader {
       Tags { "RenderType"="Opaque" }
       LOD 200
 
       CGPROGRAM
       #pragma target 3.0
       #pragma surface surf Lambert vertex:vert addshadow
       
 		fixed4 _Color;
       sampler2D _MainTex;
       sampler2D _Bump;
       fixed _BumpPower;
              
       float _SideLevel;
       float4 _SideColor;
       sampler2D _SideTex;
       float4 _SideDirection;
       sampler2D _SideNorm;
       fixed _SideNMPower;
       
            
       
       float _SideDepth;
       float _TopDepth;
       float _BottomDepth;
       float _SnowDepth;
       float _Intensity;
 
       struct Input {
           float2 uv_MainTex;
           float2 uv_SideTex;
           float2 uv_Bump;
           float3 worldNormal;
           INTERNAL_DATA
       };
 
       void vert (inout appdata_full v) {
            //Convert the normal to world coortinates
            //float4 side = mul(UNITY_MATRIX_IT_MV, _SideDirection);            
       }
 
       void surf (Input IN, inout SurfaceOutput o) {
       
       		// get main textures
            fixed3 baseColor = tex2D (_MainTex, IN.uv_MainTex); 
            fixed3 sideColor = tex2D (_SideTex, IN.uv_SideTex); 
            
            //calculate normals
            fixed3 normalMain = UnpackNormal(tex2D(_Bump, IN.uv_Bump)); //get main bump
    		normalMain = lerp(o.Normal, normalMain, _BumpPower); //adjust intensity
    		  		
    		o.Normal = lerp(o.Normal, normalMain, _BumpPower);//so far just main normal map
    		
            //calculate world normals from inspector directions for color tints
            half difference = dot(WorldNormalVector(IN, o.Normal), _SideDirection.xyz) - lerp(1,-1,_SideLevel);
            
            // handle intensity levels
            difference = saturate(difference / _SideDepth) / _Intensity;
                        
            //perform final coloring
            sideColor = (difference * (sideColor *2)) * _SideColor.rgb  + (1-difference);
						
            //handle color tints
            o.Albedo =  (sideColor * baseColor) * _Color.rgb;
            
       }
       ENDCG
    } 
    FallBack "Diffuse"
}