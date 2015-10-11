Shader "Custom/TextureColorByMask" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Mask ("Mask (RGB)", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		sampler2D _Mask;
		half4 _Color;

		struct Input {
			float2 uv_MainTex;
			float2 uv_Mask;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			half4 m = tex2D (_Mask, IN.uv_Mask);

			o.Albedo = c.rgb * (1 - m.a + _Color * m.a);			
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
