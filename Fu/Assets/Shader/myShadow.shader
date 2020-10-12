Shader "Unlit/myShadow"
{
    //人物影子的实现
    //原理：将人物垂直反转,将alpha值大于0的像素点置黑色
    //说明:在player下设置一个子物体,子物体的sprite随player变化,将子物体的material设为挂载了此shader的material
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TransRate("反转人物比例",Range(0,2.0)) = 1.0//1.0为完整影子向上向下可将影子调为部分显示
    }
    SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off ZWrite Off ZTest Always                            
        Tags { "RenderType"="Opaque"  "Queue" = "Transparent"}
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
           
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _TransRate;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.uv.y = _TransRate - o.uv.y;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                
                fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb = 1 - step(0, col.a);           //alpha大于0置黑色
                return col;
            }
            ENDCG
        }
    }
}
