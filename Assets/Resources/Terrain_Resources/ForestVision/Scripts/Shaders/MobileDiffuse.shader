 Shader "ForestVision/MobileDiffuse" {
    Properties {
        _Color("Color", Color) = (1.000000,1.000000,1.000000,1.000000)
      _MainTex ("Texture", 2D) = "white" {}
      
    }
    SubShader {
      Tags { "RenderType" = "Opaque" }
      CGPROGRAM
      #pragma surface surf Lambert
      struct Input {
          float2 uv_MainTex;
      };
      sampler2D _MainTex;
      float4 _Color;
      
      void surf (Input IN, inout SurfaceOutput o) {
          o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb * _Color;
          
      }
      ENDCG
    } 
    Fallback "Diffuse"
  }