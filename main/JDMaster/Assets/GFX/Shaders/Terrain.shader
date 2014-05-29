Shader "Custom/Terrain"
{
Properties 
{
	_MainTex ("Texture A", 2D) = "white" {}
	_MainTexB ("Texture B", 2D) = "white" {}
	_MainTexC ("Texture C", 2D) = "white" {}
}

SubShader {
	Tags { "RenderType"="Opaque"}
	
CGPROGRAM
#pragma surface surf Lambert

sampler2D _MainTex;
sampler2D _MainTexB;
sampler2D _MainTexC;

struct Input {
    fixed2 uv_MainTex : TEXCOORD0;
    fixed2 uv_MainTexB : TEXCOORD1;
    fixed2 uv_MainTexC : TEXCOORD2;
    fixed4 color : COLOR;
};

 
void surf (Input IN, inout SurfaceOutput o) 
{
	fixed4 ta = tex2D(_MainTex, IN.uv_MainTex);
	fixed4 tb = tex2D(_MainTexB, IN.uv_MainTexB);
	fixed4 tc = tex2D(_MainTexC, IN.uv_MainTexC);
	
	fixed4 col = IN.color.r*ta + IN.color.g*tb + IN.color.b*tc;
	
	o.Albedo = col.rgb;
	o.Alpha = col.a;
}
ENDCG
}

Fallback "Mobile/VertexLit"
}