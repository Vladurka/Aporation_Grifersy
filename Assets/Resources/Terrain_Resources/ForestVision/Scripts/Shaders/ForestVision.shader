// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "ForestVision/ForestVision" {
	Properties {
        
        [Header(Global Control)] _Intensity ("Overall Intensity", Range(10, 1)) = 5
        [Space(10)]
        
        [Header(Main Texture Controls)] _Color("Main Tint Color", Color) = (0.5,0.5,0.5,1)//grey
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
        _Bump ("Normal", 2D) = "bump" {}
        _BumpPower ("Normal Power", Range (0.1, 2)) = 1 //added slider control for Normal strength
        
        [Header(Detail Texture Controls)]_Detail ("Detail", 2D) = "grey" {}
        _DetailPower ("Detail Power", Range (0, 1)) = 0.2 //added slider control for Normal strength
        
        [Space(10)]
        
        [Header(Side Texture Controls)] _SideDirection ("Side Direction", Vector) = (-1,0,1)
        _SideLevel ("Side Level", Range(0,1) ) = 0.3 //0.3 default
        _SideDepth ("Side Depth", Range(0,1)) = 0.7 //0.7 def
        _SideColor ("Side Color", Color) = (0.0,0.392,0.0,1.0) //dark green
        _SideTex ("Side Texture", 2D) = "white" {}
        
        //_SideNorm ("Side Normal", 2D) = "white" {}
        //_SideNMPower ("Side Normal Power", Range (0, 2)) = 0.25 //added slider control for Normal strength
         
        [Space(10)]
        
        [Header(Top Texture Controls)] _TopDirection ("Top Direction", Vector) = (0,1.5,0)
        _TopLevel ("Top Level", Range(0,1) ) = 0.2 //0.2 def
        _TopDepth ("Top Depth", Range(0,1)) = 0.7 //0.7 def
        _TopColor ("Top Color", Color) = (1,0.894,0.710,1.0) //orange
        _TopTex ("Top Texture", 2D) = "white" {}
        
        //_TopNorm ("Top Normal", 2D) = "white" {}
        //_TopNMPower ("Top Normal Power", Range (0, 2)) = 0.25 //added slider control for Normal strength
        
        [Space(10)]
        
        [Header(Bottom Texture Controls)] _BottomDirection ("Bottom Direction", Vector) = (0,-1.5,0)
        _BottomLevel ("Bottom Level", Range(0,1) ) = 0.3 //0.3 def
        _BottomDepth ("Bottom Depth", Range(0,1)) = 0.7 //0.7 def
        _BottomColor ("Bottom Color", Color) = (0.502,0.502,0.0,1.0)//olive
        _BottomTex ("Bottom Texture", 2D) = "white" {}
        //_BottomNorm ("Bottom Normal", 2D) = "white" {}
        //_BottomNMPower ("Bottom Normal Power", Range (0, 2)) = 0.25 //added slider control for Normal strength
        
        [Header(Foliage Controls)] _VertexWeight ("Vertex Weight", 2D) = "black" {} //added new weight map slot to drive vertex animation
    	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    	_ShakeTime ("Animation Time", Range (0, 1.0)) = .25
    	_ShakeWindspeed ("Wind Speed", Range (0, 1.0)) = .3
    	_ShakeBending ("Displacement", Range (0, 1.0)) = 0.2
    	
    	[Space(10)]
        [Header(Snow Controls)] _SnowDirection ("Snow Direction", Vector) = (0,1,0)
        _SnowLevel ("Snow Level", Range(-0.1,1) ) = 0
        _SnowDepth ("Snow Depth", Range(0,1)) = 1
        
        //_SnowTex ("Snow Texture", 2D) = "white" {}
        //_SnowNorm ("Snow Normal", 2D) = "bump" {}
        //_SnowNMPower ("Snow Normal Power", Range (0.1, 2)) = 1 //added slider control for Normal strength
        
        
       [HideInInspector] _Depth ("Depth", Range(0,0.2)) = 0.1
       [HideInInspector]_SnowColor ("Snow Color", Color) = (1.0,1.0,1.0,1.0)//white
       
    }
    SubShader {
       Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
       LOD 800
       Cull [_Cull]
 
       CGPROGRAM
       	#pragma target 3.0
       	#pragma surface surf Lambert alphatest:_Cutoff vertex:vert addshadow
       	#pragma fragmentoption ARB_precision_hint_fastest
		#include "UnityCG.cginc"
       
 		fixed4 _Color;
       sampler2D _MainTex;
       sampler2D _Bump;
       fixed _BumpPower;
       sampler2D _Detail;
       fixed _DetailPower;
       
       float _SideLevel;
       float4 _SideColor;
       sampler2D _SideTex;
       float4 _SideDirection;
       sampler2D _SideNorm;
       fixed _SideNMPower;
       
       float _TopLevel;
       float4 _TopColor;
       sampler2D _TopTex;
       float4 _TopDirection;
       sampler2D _TopNorm;
       fixed _TopNMPower;
       
       float _BottomLevel;
       float4 _BottomColor;
       sampler2D _BottomTex;
       float4 _BottomDirection;
       sampler2D _BottomNorm;
       fixed _BottomNMPower;
       
       float _SnowLevel;
       float4 _SnowColor;
       sampler2D _SnowTex;
       float4 _SnowDirection;
       sampler2D _SnowNorm;
       fixed _SnowNMPower;
       
       float _Depth;
       float _SideDepth;
       float _TopDepth;
       float _BottomDepth;
       float _SnowDepth;
       float _Intensity;
      
        sampler2D _VertexWeight;
		  	
		float _ShakeDisplacement;
		float _ShakeTime = 0.1;
		float _ShakeWindspeed;
		float _ShakeBending;
 
       struct Input {
           float2 uv_MainTex;
           float2 uv_SideTex;
           float2 uv_TopTex;
           float2 uv_BottomTex;
           float2 uv_SnowTex;
           float2 uv_Bump;
           float2 uv_Detail;
           float3 worldNormal;
           float2 uv_Normal;
    		float2 uv_Weight;
    		INTERNAL_DATA
       };
 
 //----------------------------------------------------------------Foliage functions
 void FastSinCos (float4 val, out float4 s, out float4 c) {
    val = val * 6.408849 - 3.1415927;
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

			
    		
			//Convert the normal to world coortinates
            float4 side = mul(UNITY_MATRIX_IT_MV, _SideDirection);
            float4 top = mul(UNITY_MATRIX_IT_MV, _TopDirection);
            float4 bottom = mul(UNITY_MATRIX_IT_MV, _BottomDirection);
            float4 snow = mul(UNITY_MATRIX_IT_MV, _SnowDirection);
             
             //Convert the normal to world coortinates for snow
            float3 snormal = normalize(_SnowDirection.xyz);
            float3 sn = mul((float3x3)unity_WorldToObject, snormal).xyz;
 
            if(dot(v.normal, sn) >= lerp(1,-1, (_SnowLevel*2)/3))
            {
               v.vertex.xyz += normalize(sn + v.normal) * _SnowDepth * _SnowLevel/4;
            }
			//--------------------------------------------------------------------
			
	float4 WeightMap = tex2Dlod (_VertexWeight, float4(v.texcoord.xy,0,0));

    float factor = (1 - _ShakeDisplacement -  v.color.r) * 0.5;
       
    const float _WindSpeed  = (_ShakeWindspeed  +  v.color.g );    
    const float _WaveScale = _ShakeDisplacement;
   
    const float4 _waveXSize = float4(0.048, 0.06, 0.24, 0.096);
    const float4 _waveZSize = float4 (0.024, .08, 0.08, 0.2);
    const float4 waveSpeed = float4 (1.2, 2, 1.6, 4.8);
 
    float4 _waveXmove = float4(0.024, 0.04, -0.12, 0.096);
    float4 _waveZmove = float4 (0.006, .02, -0.02, 0.1);
   
    float4 waves;
    waves = v.vertex.x * _waveXSize;
    waves += v.vertex.z * _waveZSize;
 
    
    waves += _Time.x * (1 - _ShakeTime * 2 - v.color.b ) * waveSpeed *_WindSpeed;
 
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
//-----------------------------------------------------------------------------------------------
       
 
       void surf (Input IN, inout SurfaceOutput o) {
       
       		// get main textures
            fixed4 baseColor = tex2D (_MainTex, IN.uv_MainTex) * _Color; 
            fixed3 sideColor = tex2D (_SideTex, IN.uv_SideTex); 
            fixed3 topColor = tex2D (_TopTex, IN.uv_TopTex); 
            fixed3 bottomColor = tex2D (_BottomTex, IN.uv_BottomTex);  
            //fixed3 snowColor = tex2D (_SnowTex, IN.uv_SnowTex); 
            
            
    		
            //calculate world normals from inspector directions for color tints
            half difference = dot(WorldNormalVector(IN, o.Normal), _SideDirection.xyz) - lerp(1,-1,_SideLevel);
            half difference2 = dot(WorldNormalVector(IN, o.Normal), _TopDirection.xyz) - lerp(1,-1,_TopLevel);
            half difference3 = dot(WorldNormalVector(IN, o.Normal), _BottomDirection.xyz) - lerp(1,-1,_BottomLevel);
            //half difference4 = dot(WorldNormalVector(IN, o.Normal), _SnowDirection.xyz) - lerp(1,-1,_SnowLevel);
            
            // handle intensity levels
            difference = saturate(difference / _SideDepth) / _Intensity;
            difference2 = saturate(difference2 / _TopDepth)/ _Intensity;
            difference3 = saturate(difference3 / _BottomDepth)/ _Intensity;
            //difference4 = saturate(difference4 / _SnowDepth);
            
            //perform final coloring
            
            //if detailPower is greater than 0, then begin adding it into the formula, otherwise don't
            if(_DetailPower > 0){
				baseColor.rgb *= tex2D(_Detail,IN.uv_Detail).rgb * unity_ColorSpaceDouble.r + _DetailPower;
			}
			
			sideColor = (difference * (sideColor *2)) * _SideColor.rgb  + (1-difference);
			topColor = (difference2 * topColor) * _TopColor.rgb  + (1-difference2);
			bottomColor = (difference3 * (bottomColor*2)) * _BottomColor.rgb  + (1-difference3);
			//snowColor = difference4  * _SnowColor.rgb  + (1-difference4);
			
			//calculate normals
            fixed3 normalMain = UnpackNormal(tex2D(_Bump, IN.uv_Bump)); //get main bump
    		o.Normal = lerp(o.Normal, normalMain, _BumpPower);//so far just main normal map
    		
            
            if(dot(WorldNormalVector(IN, o.Normal), _SnowDirection.xyz)>=lerp(1,-1,_SnowLevel ))
            {
                o.Albedo = _SnowColor.rgb;
                //o.Normal = UnpackNormal (tex2D (_SnowNorm, IN.uv_Bump));
            }
            else {
                o.Albedo =  (sideColor * topColor * bottomColor) * baseColor * _Color.rgb;
                
            }
			
            
            o.Alpha = baseColor.a;
       }
       ENDCG
    } 
    FallBack "Diffuse"
}