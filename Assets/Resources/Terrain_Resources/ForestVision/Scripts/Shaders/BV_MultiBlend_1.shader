// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "ForestVision/MultiBlend" {
	
	Properties {
	    [Foldout] _MainShown ("", Float) = 1
		[Foldout] _TopShown ("", Float) = 1
		[Foldout] _SnowShown ("", Float) = 1
		[Foldout] _Mask1Shown ("", Float) = 1
		[Foldout] _SnowMaskShown ("", Float) = 1
        //========================================================================================================  Global        
        [Header(Global Control)] _Intensity ("Overall MultiBlend Effect Intensity", Range(1, 10)) = 1
        _Mask ("Source Mask: R, G, B, A", 2D) = "black" {}
        //========================================================================================================  
        //========================================================================================================   Main       
        [Space(10)]
        
        [Header(Main Texture Controls)] _Glossiness ("Smoothness", Range(0,5)) = 0
        _GlossMap ("Base (RGB) Trans (A)", 2D) = "white" {}
        _GlossColor("Gloss Tint Color", Color) = (0.5,0.5,0.5,1)//grey
        _Color("Main Tint Color", Color) = (0.5,0.5,0.5,1)//grey
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
        [NoScaleOffset] _Bump ("Normal", 2D) = "bump" {}
        _BumpPower ("Normal Power", Range (0.1, 2)) = 1 //added slider control for Normal strength
        
        [Header(Main Normal Detail Blender)]
        [KeywordEnum(Main Normal, Top Normal,Snow Normal 1, Snow Normal 2)] _MainNormalBlendChoice("Blend Normal Choice", Float) = 0
        _MainDetailMultiplier ("Detail Multiplier", Range(1,5)) = 1
        _MainBlendNormals ("Blend Normals", Range(0,1) ) = 0.5//used to custom blend 2 input normals for snow
        //========================================================================================================        
        [Space(30)]
        //========================================================================================================  TOP
        
        [Header(Top Texture Controls)] _TopGlossiness ("Top Smoothness", Range(0,5)) = 0
        _TopDirection ("Top Direction", Vector) = (0,1.5,0)
        _TopLevel ("Top Level", Range(-10,10) ) = 0.2 //0.2 def
        _TopDepth ("Top Depth", Range(0,50)) = 0.7 //0.7 def
        _TopColor ("Top Color", Color) = (1,0.894,0.710,1.0) //orange
        _TopTex ("Top Texture", 2D) = "white" {}
        [NoScaleOffset]_TopNorm ("Top Normal", 2D) = "bump" {}
        _TopNMPower ("Top Normal Power", Range (0, 2)) = 1 //added slider control for Normal strength
        
        [Header(Top Normal Detail Blender)]
        [KeywordEnum(Main Normal, Top Normal, Snow Normal 1, Snow Normal 2)] _TopNormalBlendChoice("Blend Normal Choice", Float) = 0
        _TopDetailMultiplier ("Detail Multiplier", Range(0.1,5)) = 1
        _TopBlendNormals ("Blend Normals", Range(0,1) ) = 0.5//used to custom blend 2 input normals for snow
        _TopAlbedoBlend("Blend Albedo", Range(0,2) ) = 0.5
        [Header(Top Texture Masking Options)]
        
        [Header(Main Mask)]
        [KeywordEnum(Mask R, Mask G, Mask B, Mask A, Main Source, Top Source, Snow Source)] _TopMaskChoice("Top Mask Channel Choice", Float) = 0
        [Toggle] _showTopMask("Show Top Mask?", Float) = 0
        _TopMaskDepth ("Top Mask Contrast Depth", Range(-10,10)) = 1 //0.7 def
        _TopMaskIntensityDepth ("Top Mask Intensity Depth", Range(-10,10)) = 1 //0.7 def
        _TopMaskTiling ("Top Mask Tiling Multiplier", Range(0.1,10)) = 1 //0.7 def
        //---------------------------------------------------------------------------------
        [Header(Secondary Mask)]
        [KeywordEnum(Mask R, Mask G, Mask B, Mask A, Main Source, Top Source, Snow Source)] _TopMaskChoice2("Second Mask Channel Choice", Float) = 0
        _TopMask2Blend ("Amount to blend 2nd Mask", Range(0,1)) = 0 //0.7 def
        [KeywordEnum(Additive, Composite, Edge Blend)] _TopMaskBlendStyle("Second Mask Blending Style", Float) = 0
        _TopMask2Tiling ("2nd Top Mask Tiling Multiplier", Range(0.1,10)) = 1 //0.7 def
        _TopMaskDepth2 ("2nd Top Mask Contrast Depth", Range(-10,10)) = 1 //0.7 def
        _TopMaskIntensityDepth2 ("2nd Top Mask Intensity Depth", Range(-10,10)) = 1 //0.7 def
        
        //========================================================================================================  
        
        [Space(30)]
        
    	//========================================================================================================  Snow
    	
        [Header(Snow Controls)] _SnowGlossiness ("Snow Smoothness", Range(0,5)) = 0.5
        _SnowDirection ("Snow Direction", Vector) = (10,10,-2)
       	[Toggle] _snowDisplace("Vertex Displacement?", Float) = 0
       	
        _SnowLevel ("Snow Level", Range(-10,10) ) = 0
        _SnowDepth ("Snow Depth", Range(0,50)) = 0
        _SnowColor ("Snow Color", Color) = (1.0,1.0,1.0,1.0)//white
        _SnowTex ("Snow Texture", 2D) = "white" {}
        _SnowSpec ("Snow Spec", 2D) = "white" {}
        _SnowSpecTiling("Snow Spec Tiling", Float) = 1
        
        [NoScaleOffset]_SnowNorm ("Base Snow Normal", 2D) = "bump" {}
        _SnowNorm2 ("Detail Snow Normal", 2D) = "bump" {}
        _SnowBlendNormals ("Blend Normals", Range(0,1) ) = 0.5//used to custom blend 2 input normals for snow
        
        _SnowNMPower ("Snow Normal Power", Range (0.1, 2)) = 1 //added slider control for Normal strength
        
        [Header(Snow Texture Masking Options)]
        [Header(Main Mask)]
        [KeywordEnum(Mask R, Mask G, Mask B, Mask A, Main Source, Top Source, Snow Source)] _SnowMaskChoice("Snow Mask Channel Choice", Float) = 0
        [Toggle] _showSnowMask("Show Snow Mask?", Float) = 0
        _SnowMaskDepth ("Snow Mask Contrast Depth", Range(-10,10)) = 1 //0.7 def
        _SnowMaskIntensityDepth ("Snow Mask Intensity Depth", Range(-1,1)) = 1 //0.7 def
        _SnowMaskTiling ("2nd Top Mask Tiling Multiplier", Range(1,10)) = 1 //0.7 def
        //---------------------------------------------------------------------------------
        [Header(Secondary Mask)]
        [KeywordEnum(Mask R, Mask G, Mask B, Mask A, Main Source, Top Source, Snow Source)] _SnowMaskChoice2("Second Mask Channel Choice", Float) = 0
        _SnowMask2Blend ("Amount to blend 2nd Mask", Range(0,1)) = 0 //0.7 def
        [KeywordEnum(Additive, Composite, Edge Blend)] _SnowMaskBlendStyle("Second Mask Blending Style", Float) = 0
        _SnowMask2Tiling ("2nd Top Mask Tiling Multiplier", Range(1,10)) = 1 //0.7 def
        _SnowMaskDepth2 ("2nd Top Mask Contrast Depth", Range(-10,10)) = 1 //0.7 def
        _SnowMaskIntensityDepth2 ("2nd Top Mask Intensity Depth", Range(-10,10)) = 1 //0.7 def
        
       
    }
    SubShader 
    {
    	
      	Tags {"RenderType"="Opaque"}
       
        
      	CGPROGRAM
      	#pragma target 4.0
      	#pragma surface surf Standard alphatest:_Cutoff vertex:vert addshadow
    	#pragma fragmentoption ARB_precision_hint_fastest
		#include "UnityCG.cginc"
 		///////////////////////////////////////// Global Parameters
 		float _Intensity;
 		sampler2D _Mask;//--------------------------------------------1
 		//////////////////////////////////////// Main texture params
 		half _Glossiness;
 		fixed4 _Color;
 		fixed _OcclusionStrength;
       	sampler2D _MainTex;//-----------------------------------------2
       	sampler2D _Bump;//--------------------------------------------3
       	fixed _BumpPower;
       	float _MainNormalBlendChoice;
       	float _MainDetailMultiplier;
       	float _MainBlendNormals;
       	
       	//////////////////////////////////////// Top texture params
       	half _TopGlossiness;
       	float4 _TopDirection;
       	float _TopLevel;
       	float _TopDepth;
       	float4 _TopColor;
       	sampler2D _TopTex;//------------------------------------------4
       	sampler2D _TopNorm;//-----------------------------------------5
       	fixed _TopNMPower;
       	float _TopNormalBlendChoice;
       	float _TopDetailMultiplier;
       	float _TopBlendNormals;
       	float _TopMaskChoice;
       	float _showTopMask;
       	fixed _TopMaskDepth; 
       	fixed _TopMaskIntensityDepth;
       	fixed _TopMaskTiling;
       	float _TopMaskChoice2;
       	fixed _TopMask2Blend;
       	float _TopMaskBlendStyle;
       	fixed _TopMask2Tiling;
       	fixed _TopMaskDepth2;
       	fixed _TopMaskIntensityDepth2;
       	fixed _TopAlbedoBlend;
       	
       	//////////////////////////////////////// Snow texture params
       	half _SnowGlossiness;
 		float4 _SnowDirection;
 		float _snowDisplace;		
      	float _SnowLevel;
       	float _SnowDepth;
       	float4 _SnowColor;
       	sampler2D _SnowTex;//----------------------------------------6
        sampler2D _SnowSpec;//---------------------------------------7
        sampler2D _SnowNorm;//---------------------------------------8
       	sampler2D _SnowNorm2;//--------------------------------------9
       	fixed _SnowSpecTiling;
       	float _SnowBlendNormals;
       	fixed _SnowNMPower;
       	fixed _SnowMaskTiling;
       	float _SnowMaskChoice;
       	float _showSnowMask;
       	fixed _SnowMaskDepth;
       	fixed _SnowMaskIntensityDepth;
       	float _SnowMaskChoice2;
       	fixed _SnowMask2Blend;
       	float _SnowMaskBlendStyle;
       	fixed _SnowMask2Tiling;
       	fixed _SnowMaskDepth2;
       	fixed _SnowMaskIntensityDepth2;
        //////////////////////////////////////// 
        
       	struct Input 
       	{
        	float2 uv_MainTex;
            float2 uv_TopTex;
            float2 uv_Mask;
           	float2 uv_SnowTex;
           	float2 uv_SnowSpec;
           	float2 uv_SnowNorm2;
           	float2 uv_SnowTexMask;
           	float3 worldNormal;
           	float2 uv_Weight;
           	
           	float3 viewDir;
    		INTERNAL_DATA
       };    
       
       void vert (inout appdata_full v) 
		{
			            
            float4 snow = mul(UNITY_MATRIX_IT_MV, _SnowDirection);
            float3 snormal = normalize(_SnowDirection.xyz);
            float3 sn = mul((float3x3)unity_WorldToObject, snormal).xyz;
 			//if user wants vertex deformation
 			if(_snowDisplace >0){
 			    if(dot(v.normal, sn) >= lerp(1,-1, (_SnowLevel*2)/3))
	            {
	               v.vertex.xyz += normalize(sn + v.normal) * _SnowDepth * _SnowLevel/20;
	            }
            }
        }
		
		void surf (Input IN, inout SurfaceOutputStandard o) 
       {
          	/////////////////////////         Color Channel         /////////////////////
          	
            float4 baseColor         = tex2D (_MainTex,  IN.uv_MainTex) * _Color; 
            float4 baseColor_topMask = tex2D (_MainTex,  IN.uv_MainTex * _TopMaskTiling) * _TopColor; 
            float4 baseColor_snowMask = tex2D (_MainTex, IN.uv_MainTex * _SnowMaskTiling) * _SnowColor;
            float4 baseColor_topMask2 = tex2D (_MainTex,  IN.uv_MainTex * _TopMask2Tiling) * _TopColor; 
            float4 baseColor_snowMask2 = tex2D (_MainTex, IN.uv_MainTex * _SnowMask2Tiling) * _SnowColor;
            //================================================================  TOP
            float4 topColor          = tex2D (_TopTex, IN.uv_TopTex) * _TopColor;
            float4 topColor_topMask  = tex2D (_TopTex, IN.uv_TopTex * _TopMaskTiling) * _TopColor;
            float4 topColor_snowMask = tex2D (_TopTex, IN.uv_TopTex * _SnowMaskTiling) * _SnowColor;
            float4 topColor_topMask2  = tex2D (_TopTex, IN.uv_TopTex * _TopMask2Tiling) * _TopColor;
            float4 topColor_snowMask2 = tex2D (_TopTex, IN.uv_TopTex * _SnowMask2Tiling) * _SnowColor;
            topColor = lerp(topColor_topMask, topColor * 2, _TopAlbedoBlend);
            //================================================================   SNOW
            float4 snowColor          = tex2D (_SnowTex, IN.uv_SnowTex) * _SnowColor; 
            float4 snowColor_topMask  = tex2D (_SnowTex, IN.uv_SnowTex * _TopMaskTiling) * _TopColor; 
            float4 snowColor_snowMask = tex2D (_SnowTex, IN.uv_SnowTex * _SnowMaskTiling) * _SnowColor; 
            float4 snowColor_topMask2  = tex2D (_SnowTex, IN.uv_SnowTex * _TopMask2Tiling) * _TopColor; 
            float4 snowColor_snowMask2 = tex2D (_SnowTex, IN.uv_SnowTex * _SnowMask2Tiling) * _SnowColor;
            //================================================================  MASKS
            float4 Mask1  = tex2D (_Mask, IN.uv_Mask * _TopMaskTiling);
            fixed3 mask1R = Mask1.r;
            fixed3 mask1G = Mask1.g;
            fixed3 mask1B = Mask1.b;
            fixed3 mask1A = Mask1.a;
            float4 Mask2  = tex2D (_Mask, IN.uv_Mask * _TopMask2Tiling);
            fixed3 Mask2R = Mask2.r;
            fixed3 Mask2G = Mask2.g;
            fixed3 Mask2B = Mask2.b;
            fixed3 Mask2A = Mask2.a;
            //=================================================================
            float4 SnowMask1  = tex2D (_Mask, IN.uv_Mask * _SnowMaskTiling); 
            fixed3 SnowMask1R = SnowMask1.r;
            fixed3 SnowMask1G = SnowMask1.g;
            fixed3 SnowMask1B = SnowMask1.b;
            fixed3 SnowMask1A = SnowMask1.a;
            float4 SnowMask2  = tex2D (_Mask, IN.uv_Mask * _SnowMask2Tiling);
            fixed3 SnowMask2R = SnowMask2.r;
            fixed3 SnowMask2G = SnowMask2.g;
            fixed3 SnowMask2B = SnowMask2.b;
            fixed3 SnowMask2A = SnowMask2.a;
            //================================================================
            float4 snowSpec = tex2D (_SnowSpec, IN.uv_SnowSpec * _SnowSpecTiling); 
            //================================================================
            
            ///////////////////////////////////////////////////////////  TOP MASK
            float4 topMask;
                        
            switch(_TopMaskChoice){
            	case 0:
            		topMask = saturate(dot((_TopMaskIntensityDepth-mask1R),_TopMaskDepth));
            		break;
            	case 1:
            		topMask = saturate(dot((_TopMaskIntensityDepth-mask1G),_TopMaskDepth));
            		break;
            	case 2:
            		topMask = saturate(dot((_TopMaskIntensityDepth-mask1B),_TopMaskDepth));
            		break;
            	case 3:
            		topMask = saturate(dot((_TopMaskIntensityDepth-mask1A),_TopMaskDepth));
            		break;
            	case 4:
            		topMask = saturate(dot((_TopMaskIntensityDepth-baseColor_topMask),_TopMaskDepth));
            		break;
            	case 5:
            		topMask = saturate(dot((_TopMaskIntensityDepth-topColor_topMask),_TopMaskDepth));
            		break;
            	case 6:
            		topMask = saturate(dot((_TopMaskIntensityDepth-snowColor_topMask),_TopMaskDepth));
            		break;
            	
            }
            
            
            float4 topMask2;
            switch(_TopMaskChoice2){
            	case 0:
            		topMask2 = saturate(dot((_TopMaskIntensityDepth2-Mask2R),_TopMaskDepth2));
            		break;
            	case 1:
            		topMask2 = saturate(dot((_TopMaskIntensityDepth2-Mask2G),_TopMaskDepth2));
            		break;
            	case 2:
            		topMask2 = saturate(dot((_TopMaskIntensityDepth2-Mask2B),_TopMaskDepth2));
            		break;
            	case 3:
            		topMask2 = saturate(dot((_TopMaskIntensityDepth2-Mask2A),_TopMaskDepth2));
            		break;
            	case 4:
            		topMask2 = saturate(dot((_TopMaskIntensityDepth2-baseColor_topMask2),_TopMaskDepth2));
            		break;
            	case 5:
            		topMask2 = saturate(dot((_TopMaskIntensityDepth2-topColor_topMask2),_TopMaskDepth2));
            		break;
            	case 6:
            		topMask2 = saturate(dot((_TopMaskIntensityDepth2-snowColor_topMask2),_TopMaskDepth2));
            		break;
            	
            }
            
            switch(_TopMaskBlendStyle){
            	case 0:
            		topMask = saturate(saturate(topMask - topMask2) * _TopMask2Blend); //additive
            		break;
            	case 1:
            		topMask = lerp(topMask, topMask2, _TopMask2Blend); //composite
            		break;
            	case 2:
            		topMask = saturate(dot(topMask,topMask2)) * _TopMask2Blend; //edgeBlend
            		break;
            }
           
            float4 visibleTopColor = lerp(topColor, baseColor ,topMask);
            
            
            //================================================================    SNOW MASK
            
            float4 snowMask;
            switch(_SnowMaskChoice){
            	case 0:
            		snowMask = saturate(dot((_SnowMaskIntensityDepth-SnowMask1R),_SnowMaskDepth));
            		break;
            	case 1:
            		snowMask = saturate(dot((_SnowMaskIntensityDepth-SnowMask1G),_SnowMaskDepth));
            		break;
            	case 2:
            		snowMask = saturate(dot((_SnowMaskIntensityDepth-SnowMask1B),_SnowMaskDepth));
            		break;
            	case 3:
            		snowMask = saturate(dot((_SnowMaskIntensityDepth-SnowMask1A),_SnowMaskDepth));
            		break;
            	case 4:
            		snowMask = saturate(dot((_SnowMaskIntensityDepth-baseColor_snowMask),_SnowMaskDepth));
            		break;
            	case 5:
            		snowMask = saturate(dot((_SnowMaskIntensityDepth-topColor_snowMask),_SnowMaskDepth));
            		break;
            	case 6:
            		snowMask = saturate(dot((_SnowMaskIntensityDepth-snowColor_snowMask),_SnowMaskDepth));
            		break;
            	
            }
            
            float snowMask2;
            switch(_SnowMaskChoice2){
            	case 0:
            		snowMask2 = saturate(dot((_SnowMaskIntensityDepth2-SnowMask2R),_SnowMaskDepth2));
            		break;
            	case 1:
            		snowMask2 = saturate(dot((_SnowMaskIntensityDepth2-SnowMask2G),_SnowMaskDepth2));
            		break;
            	case 2:
            		snowMask2 = saturate(dot((_SnowMaskIntensityDepth2-SnowMask2B),_SnowMaskDepth2));
            		break;
            	case 3:
            		snowMask2 = saturate(dot((_SnowMaskIntensityDepth2-SnowMask2A),_SnowMaskDepth2));
            		break;
            	case 4:
            		snowMask2 = saturate(dot((_SnowMaskIntensityDepth2-baseColor_snowMask2),_SnowMaskDepth2));
            		break;
            	case 5:
            		snowMask2 = saturate(dot((_SnowMaskIntensityDepth2-topColor_snowMask2),_SnowMaskDepth2));
            		break;
            	case 6:
            		snowMask2 = saturate(dot((_SnowMaskIntensityDepth2-snowColor_snowMask2),_SnowMaskDepth2));
            		break;
            	
            }
            
            switch(_SnowMaskBlendStyle){
            	case 0:
            		snowMask = saturate(saturate(snowMask - snowMask2) * _SnowMask2Blend); //additive
            		break;
            	case 1:
            		snowMask = lerp(snowMask, snowMask2, _SnowMask2Blend); //composite
            		break;
            	case 2:
            		snowMask = saturate(dot(snowMask,snowMask2)) * _SnowMask2Blend; //edgeBlend
            		break;
            		
            }
            
            float4 visibleSnowColor = lerp(snowColor, baseColor ,snowMask) ;
            //==================================================================        
            
            //////////////////////////////////////////////////////////////////////////////////////////// Normals
            
            fixed3 normalMain = UnpackNormal(tex2D(_Bump, IN.uv_MainTex)) *_BumpPower; 
            fixed3 normalMainBlend;
            switch(_MainNormalBlendChoice){
            	case 0:
            		normalMainBlend = UnpackNormal(tex2D(_Bump, IN.uv_MainTex *_MainDetailMultiplier)) *_BumpPower; 
            		break;
            	case 1:
            		normalMainBlend = UnpackNormal(tex2D(_TopNorm, IN.uv_TopTex * _MainDetailMultiplier)) *_BumpPower; 
            		break;
            	case 2:
            		normalMainBlend = UnpackNormal(tex2D(_SnowNorm, IN.uv_SnowTex * _MainDetailMultiplier)) *_BumpPower; 
            		break;
            	case 3:
            		normalMainBlend = UnpackNormal(tex2D(_SnowNorm2, IN.uv_SnowNorm2 * _MainDetailMultiplier)) *_BumpPower; 
            		break;
            }
            normalMain = lerp(normalMain, normalMainBlend - topMask, _MainBlendNormals);
    		//=========================================================================== 		
    		fixed3 normalTop = UnpackNormal(tex2D(_TopNorm, IN.uv_TopTex)); 
    		fixed3 normalTopBlend;
    		switch(_TopNormalBlendChoice){
            	case 0:
            		normalTopBlend = UnpackNormal(tex2D(_Bump, IN.uv_MainTex *_TopDetailMultiplier)) *_TopNMPower; 
            		break;
            	case 1:
            		normalTopBlend = UnpackNormal(tex2D(_TopNorm, IN.uv_TopTex * _TopDetailMultiplier)) *_TopNMPower; 
            		break;
            	case 2:
            		normalTopBlend = UnpackNormal(tex2D(_SnowNorm, IN.uv_SnowTex * _TopDetailMultiplier)) *_TopNMPower; 
            		break;
            	case 3:
            		normalTopBlend = UnpackNormal(tex2D(_SnowNorm2, IN.uv_SnowNorm2 * _TopDetailMultiplier)) *_TopNMPower; 
            		break;
            }
    		normalTop = lerp(normalTop, normalTopBlend, _TopBlendNormals);
    		
    		//=========================================================================== 		
    		   			
    		fixed3 snowNormal = UnpackNormal(tex2D(_SnowNorm, IN.uv_SnowTex)); 
    		fixed3 snowNormal2 = UnpackNormal(tex2D(_SnowNorm2, IN.uv_SnowNorm2)); 
    		
    		snowNormal = lerp(snowNormal, snowNormal2, _SnowBlendNormals);
    		fixed3 visibleSnowNormal = lerp(snowNormal, normalMain, (half3)snowMask) * _SnowNMPower;
    		
    		
           //////////////////////////////////////////////////////////////////////////////////////////// World Direction Levels
            half difference2 = dot(WorldNormalVector(IN, normalTop),    _TopDirection.xyz)    - lerp(1,-1,_TopLevel);
            half difference4 = dot(WorldNormalVector(IN, visibleSnowNormal),   _SnowDirection.xyz)   - lerp(1,-1,_SnowLevel);
            
            difference2 = saturate(difference2 / _TopDepth)   / _Intensity;
            difference4 = saturate(difference4 / _SnowDepth)  / _Intensity;
            
            //////////////////////////////////////////////////////////////////////////////////////////// Glossiness
            float4 baseGlossy   =  baseColor.g * _Glossiness;
            float4 topGlossy    =  lerp(saturate(dot(topMask,topMask2)), topColor, _TopGlossiness - difference2);
            float4 snowGlossy   =  lerp(snowMask, snowSpec, _SnowGlossiness );
    		
            //////////////////////////////////////////////////////////////////////////////////////////// Calculations
            
            ////////////////////////////////////////////////////////////////////////////////////////////  Final coloring
			
			//============================================================== Main color channels
			
			topColor    = lerp(baseColor, visibleTopColor,    saturate(difference2 - topMask));
			snowColor   = lerp(baseColor, visibleSnowColor,   saturate(difference4 - snowMask));
			//============================================================== Normals
			
			normalTop    = lerp(normalMain, normalTop, (half3)saturate(difference2 - topMask));
			snowNormal   = lerp(normalMain, visibleSnowNormal, (half3)saturate(difference4 - snowMask));
			//==============================================================  Glossiness
			
			topGlossy = lerp(baseGlossy, topGlossy, saturate(difference2 - topMask));
			snowGlossy = lerp(topGlossy, snowGlossy, saturate(difference4 - snowMask));
			
    		/////////////////////////////////////////////////////////////////////////////////////////// Final masking
    		    		
    		//================================================================== Color
    		fixed3 baseTop = lerp(baseColor,   visibleTopColor,    difference2);
    		fixed3 baseSnow   = lerp(baseTop, visibleSnowColor,  difference4 );
			//================================================================== Normals
            
            
            fixed3 baseNormalTop = lerp(normalMain, normalTop, (half3)saturate(difference2 - topMask));
            fixed3 newBaseNormal = lerp(baseNormalTop, snowNormal, (half3)saturate(difference4 - snowMask));
            //================================================================== Glossiness
            
            
            //=============================================================
            
            if(dot(WorldNormalVector(IN, snowNormal), _SnowDirection.xyz)>=lerp(1,-1,_SnowLevel ))
            {
            	// we want to visualize the mask being used
            	if(_showSnowMask > 0){
            		o.Albedo = 1-snowMask;
            	}else{
            		o.Albedo = baseSnow;
            		o.Normal = newBaseNormal ;
                	o.Smoothness = snowGlossy * 2;
            	}
                
            }
            else {
            	// we want to visualize the mask being used
            	if(_showTopMask > 0){
            		o.Albedo = 1 - topMask;
            	}else{
            		o.Albedo = baseTop;
            		o.Normal = baseNormalTop;
                	o.Smoothness = topGlossy;
            	}
            	
            	
            }
            
            
			o.Alpha = baseColor.a;
           
       }
       
      
      ENDCG
    }//end subshader
    FallBack "Diffuse"
	CustomEditor "BV_MultiBlendGUI"


}