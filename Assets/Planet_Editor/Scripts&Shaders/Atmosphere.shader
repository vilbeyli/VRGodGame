Shader "Atmosphere" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_NoiseTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_Thickness ("Cloud Thickness", Range(1,4)) = 1
}

SubShader {
	Tags { "Queue"="Transparent"  "RenderType"="Transparent"}
	LOD 200
Blend One One
Cull Back
Offset 0, -1 
CGPROGRAM
#pragma surface surf Lambert alpha
#pragma target 3.0


sampler2D _MainTex;
sampler2D _NoiseTex;
fixed4 _Color;
float _Thickness;

struct Input {
	float2 uv_MainTex;
	float2 uv_NoiseTex;
	float3 worldRefl;
			float3 worldNormal;
			float3 worldPos;
			float3 viewDir;
			INTERNAL_DATA
	
};
  half4 LightingLambert (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {
        half3 h = normalize (lightDir + viewDir);

        half diff = max (0, dot (s.Normal, lightDir));

        float nh = max (0, dot (s.Normal, h));
        float spec = pow (nh, 48.0);

        half4 c;
     

       c.rgb = (s.Albedo * _LightColor0.rgb*diff+diff* _LightColor0.rgb-.4);
        c.a =1;
        return c;
    }
void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color+.2;
	c.a-= tex2D(_NoiseTex, IN.uv_NoiseTex).r;
	IN.uv_MainTex += float4(.45, 0, 0, 0);
	c += tex2D(_MainTex, IN.uv_MainTex) * _Color+.2;
	
IN.uv_NoiseTex += float4(.45, 0, 0, 0);
c.a *= pow(tex2D(_NoiseTex, IN.uv_NoiseTex).r,3.0/_Thickness);

			o.Alpha = clamp(c.a,0.0,1.0);

    		o.Albedo = clamp(c.rgb,0.0,1.0);

}
ENDCG
}

Fallback "Transparent/VertexLit"
}
