﻿Shader "Unlit/Screen" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
		_Color ("COlor", Color) = (1,1,1,1)
    }
    SubShader {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }

		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
		Cull Off

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float4 _MainTex_TexelSize;

			float4 _Color;

            v2f vert (appdata v) {
				float2 scale = float2(_MainTex_TexelSize.z * _MainTex_TexelSize.y, 1);
				v.vertex.xy *= scale;

                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

			float4 frag (v2f i) : SV_Target {
				float4 col = tex2D(_MainTex, i.uv);
                return float4(col.xyz, 1) * _Color;
            }
            ENDCG
        }
    }
}
