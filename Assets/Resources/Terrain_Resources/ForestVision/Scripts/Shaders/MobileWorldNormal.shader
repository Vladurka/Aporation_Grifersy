// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "ForestVision/MobileWorldNormal" {
    Properties {
        //[Toggle] _UseMain("Blend Using Main Tex?", Float) = 0
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _TopDirection ("Top Direction", Vector) = (0,1,0)
        _TopLevel ("Top Level", Range(0,10) ) = 0.2 //0.2 def
        _TopDepth ("Top Depth", Range(0,1)) = 0.0 //0.7 def
        _TopIntensity ("Top Intensity", Range(0,1)) = 0.0 //0.7 def
        _TopColor ("Top Color", Color) = (1,0.894,0.710,1.0) //orange
        _TintColor("Tint Color", Color) = (1.000000,1.000000,1.000000,1.000000)
        _TintLevel ("Tint Level", Range(0,1)) = 0.0 //0.7 def
        //_ShadowColor ("Shadow Color", Color) = (1,0.894,0.710,1.0) //orange
        
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200
 
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:vert
        #pragma target 3.0
 
        sampler2D _MainTex;
        sampler2D _TopTex;
        //float _UseMain;
        float4 _TopDirection;
        float _TopLevel;
       	float _TopDepth;
        float _TopIntensity;
       	float4 _TopColor;
        float4 _TintColor;
        float _TintLevel;
        //float4 _ShadowColor;
       
 
        struct Input {
            float2 uv_MainTex;
            float3 cameraRelativeWorldPos;
            float3 worldNormal;
            INTERNAL_DATA
        };
 
        void vert (inout appdata_full v, out Input o) {
            //Convert the normal to world coortinates
            float4 snow = mul(UNITY_MATRIX_IT_MV, _TopDirection);
            float3 snormal = normalize(_TopDirection.xyz);
            float3 sn = mul((float3x3)unity_WorldToObject, snormal).xyz;

             UNITY_INITIALIZE_OUTPUT(Input,o);
            o.cameraRelativeWorldPos = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0)) - _WorldSpaceCameraPos.xyz;
 
        }

        
//     half4 LightingCSLambert (SurfaceOutput s, half3 lightDir, half atten) 
//    {
//         fixed diff = max (0, dot (s.Normal, lightDir));

//         fixed4 c;
//         c.rgb = s.Albedo * _LightColor0.rgb * (diff * atten * 2);
        
//         //shadow colorization
//         c.rgb += _ShadowColor.xyz * max(0.0,(1.0-(diff*atten*2)));
//         c.a = s.Alpha;
//         return c;
//     }

 
        void surf (Input IN, inout SurfaceOutputStandard o) {
            half4 color = tex2D (_MainTex, IN.uv_MainTex);
            
            half difference2 = dot(WorldNormalVector(IN, o.Normal), _TopDirection.xyz) - lerp(1,-1,_TopLevel);

           difference2 = saturate(difference2 / _TopDepth);
            _TopColor = lerp(color, _TopColor , saturate(difference2 - _TopIntensity));

            if(dot(WorldNormalVector(IN, o.Normal), _TopDirection.xyz)>=lerp(1,-1,_TopLevel))
            {
                o.Albedo = _TopColor   * saturate(_TintColor / _TintLevel);
            }
            else {
                o.Albedo = color.rgb  * saturate(_TintColor / _TintLevel);
            }
            
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
    FallBack "Diffuse"
}