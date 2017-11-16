// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Line"
{
	Properties
	{
		_Color ("Color", Color) = (0,1,0,1)
		_LineThickness ("Line Thickness", Range(0, 100)) = .01 
		_LineDensity ("Line Density", Range(1, 100)) = 1
		_Speed ("Speed", Range(-10,10)) = 1
		[Toggle(X)]
        _X ("X", Float) = 0
		[Toggle(X)]
        _Y ("Y", Float) = 0
		 [Toggle(X)]
        _Z ("Z", Float) = 0
		 [Toggle(ShowBackFaces)]
        _ShowBackFaces ("Show Back Faces", Float) = 0
	}
	SubShader
	{
		Tags {"Queue"="Transparent" "IgnoreProgector"="True" "RenderType"="Transparent" }
		LOD 200

		ZWrite Off
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD0;
			};
			
			v2f vert (appdata v)
			{
				v2f o;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 _Color;
			float _LineThickness;
			int _LineDensity;
			float _Speed;
			float _X;
			float _Y;
			float _Z;

			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = (0,0,0,0);
				float xwp = i.worldPos.x + (_Time * _Speed);
				float xwpr = round(xwp * _LineDensity)/_LineDensity;
				float ywp = i.worldPos.y + (_Time * _Speed);
				float ywpr = round(ywp * _LineDensity)/_LineDensity;
				float zwp = i.worldPos.z + (_Time * _Speed);
				float zwpr = round(zwp * _LineDensity)/_LineDensity;
				float tempLineThickness = ((_LineThickness / 100) / _LineDensity) / 2;
				if(abs(xwp - xwpr) < tempLineThickness && _X == 1 || abs(ywp - ywpr) < tempLineThickness && _Y == 1 || abs(zwp - zwpr) < tempLineThickness && _Z == 1)
					col = _Color;
				return col;
			}
			ENDCG
		}
	}
}
