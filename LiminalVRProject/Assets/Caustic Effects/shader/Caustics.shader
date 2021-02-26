Shader "Projector/Caustics" {
	Properties {
		_Color ("Color", Color) = (1,1,1,0)
		[NoScaleOffset]_MainTex ("Texture", 2D) = "black" { }
		_Size ("Grid Size", Float) = 10
		_Height ("Water Height", Float) = 2.0
		_DepthBlend ("Depth Blend", Float) = 10.0
		_EdgeBlend ("Edge Blend", Range (0, 100)) = 0.5
		_Multiply ("Multiply", Range (1, 2)) = 1.0
		_LOD ("LOD Bias", Range (1, 1000)) = 100
	 }
	 
	 Subshader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent+100" }
		Pass {
			ZWrite Off
			//Blend OneMinusDstColor One //- Soft Additive
			//Blend One One //- Linear Dodge
			Blend DstColor One

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#include "UnityCG.cginc"
		 
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 wPos : TEXCOORD1; // added for height comparisons.
				UNITY_FOG_COORDS(2)
			};
		 
			uniform sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
			float4x4 unity_Projector;
			float _Size;
			float _Height;
			float _DepthBlend;
			float _EdgeBlend;
			float _Multiply;
			float _LOD;
			float dist;

			v2f vert (appdata_tan v) {
				v2f o;
				o.pos = UnityObjectToClipPos (v.vertex);
				o.wPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.uv = TRANSFORM_TEX (mul (unity_Projector, v.vertex).xy, _MainTex);
				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}
		 
			fixed4 frag (v2f i) : COLOR {
				dist = distance(_WorldSpaceCameraPos, i.wPos);
				fixed4 c = tex2Dlod (_MainTex, float4(fmod (i.uv, 1 / _Size)*_Size,0,dist/_LOD)); // project tiled texture, set lod.

				if (i.wPos.y<_Height) 
					c = c-(i.wPos.y-_Height)/-_DepthBlend*2;
				else
					c = lerp(c,0,(i.wPos.y-_Height)/_EdgeBlend);

				c = saturate(c);

				UNITY_APPLY_FOG_COLOR(i.fogCoord, c, fixed4(0,0,0,0));				
				return c * _Color *_Multiply ; // apply final color
			}
			ENDCG
		}
	}
}