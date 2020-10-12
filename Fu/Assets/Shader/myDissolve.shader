Shader "Unlit/myDissolve"
{
///溶解特效
//原理：用rgb值减去溶解度,用clip剔除小于0的像素点,最终在1.01时溶解完毕

    Properties
    {
        _Color("color",Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _CutOutValue("CutOutValue",Range(0.0,1.01)) = 0.0  //分解比例，0代表不分解，1.01代表完全分解完
        _CutOutTex("Texture",2D) = "black"{}
    }
    SubShader
    {
        Tags
        {
            "Queue" = "TransParent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha
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
                float4 color    : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _CutOutTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _CutOutValue;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color*_Color;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv)*i.color;     //根据_Color调整图片颜色
                col.rgb *= col.a;                              //保证图片透明度
                fixed4 secondTex = tex2D(_CutOutTex,i.uv);
                clip(secondTex.rgb - _CutOutValue);         //剔除小于0的像素点
               
                return col;
            }
            ENDCG
        }
    }
}
