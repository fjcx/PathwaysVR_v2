﻿Shader "Custom/GradualGrayScaleEffectShader" {
Properties{
	_MainTex("Base (RGB)", 2D) = "white" {}
}

SubShader{
	Pass{
		ZTest Always Cull Off ZWrite Off

CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#include "UnityCG.cginc"

	uniform sampler2D _MainTex;
	uniform half _RampOffset;
	uniform half _HueThreshold;
	uniform half _SatThreshold;
	half4 _MainTex_ST;

	float Epsilon = 1e-10;

	float _MaskValue;

	float3 RGBtoHCV(in float3 RGB)
	{
		// conversion based on http://www.chilliant.com/rgb2hsv.html
		float4 P = (RGB.g < RGB.b) ? float4(RGB.bg, -1.0, 2.0 / 3.0) : float4(RGB.gb, 0.0, -1.0 / 3.0);
		float4 Q = (RGB.r < P.x) ? float4(P.xyw, RGB.r) : float4(RGB.r, P.yzx);
		float C = Q.x - min(Q.w, Q.y);
		float H = abs((Q.w - Q.y) / (6 * C + Epsilon) + Q.z);
		return float3(H, C, Q.x);
	}

	float3 RGBtoHSV(in float3 RGB)
	{
		float3 HCV = RGBtoHCV(RGB);
		float S = HCV.y / (HCV.z + Epsilon);
		return float3(HCV.x, S, HCV.z);
	}

	float3 HUEtoRGB(in float H)
	{
		float R = abs(H * 6 - 3) - 1;
		float G = 2 - abs(H * 6 - 2);
		float B = 2 - abs(H * 6 - 4);
		return saturate(float3(R, G, B));
	}

	float3 HSVtoRGB(in float3 HSV)
	{
		float3 RGB = HUEtoRGB(HSV.x);
		return ((RGB - 1) * HSV.y + 1) * HSV.z;
	}

	fixed4 frag(v2f_img i) : SV_Target
	{
		fixed4 original = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv, _MainTex_ST));

		float3 hsvCol = RGBtoHSV(original.rgb);
		float3 outRgb;

		if (hsvCol.x < _HueThreshold && hsvCol.y > _SatThreshold) {
			outRgb = original.rgb;
		} else {
			hsvCol.y = hsvCol.y * _RampOffset;
			outRgb = HSVtoRGB(hsvCol);
		}
		original.rgb = outRgb;

		return original;
	}
		ENDCG

	}
	}

		Fallback off

}
