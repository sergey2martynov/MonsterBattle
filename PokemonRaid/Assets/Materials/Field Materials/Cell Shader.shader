Shader "Unlit/Cell Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _UVTransparency ("UV Transparency", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="TransparencyCutout" "RenderPipeline" = "UniversalRenderPipeline"}
        LOD 100
        Cull off
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            float _UVTransparency;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                //col.a *= i.uv.x;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

                if (i.uv.x > _UVTransparency && 1 - i.uv.x > _UVTransparency
                    && i.uv.y > _UVTransparency && 1 - i.uv.y > _UVTransparency)
                {
                    discard;
                }

                if (col.a < 0.9)
                {
                    discard;
                }
                
                return col;
            }
            ENDCG
        }
    }
}
