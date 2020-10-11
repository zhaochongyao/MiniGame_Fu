Shader "Unlit/Flash"
{
//闪光特效
//原理:模拟一条平行四边形亮条滑过贴图,根据夹角宽度进行计算
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [HDR]_Color("flashColor",Color) = (1,1,1,1)
        _Speed("Flash_Speed", Float) = 1.0
        _Angle("Flash_Angle",Range(-1.57,1.57)) = 0.78
        _Length("Flash_Length",Float) = 0.3
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "TransParent" }
        LOD 100

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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _Color;
            float4 _MainTex_ST;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            float _Speed; //闪光运动速度，正负值表示扫描方向不同，数值大小闪光运动速度快慢
            float _Angle; //闪光与X轴夹角的负值
            float _Length;//闪光的宽度与Y轴重合位置的长度，用于控制闪光宽度/
            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float size = 1.f + abs(tan(_Angle));//在Y轴投影最大值/
                if (_Length < 0.f)
                _Length = 0.f;//宽度最低为0/
                if (_Length > size)
                _Length = size;
                float bottom = fmod(_Time.y * _Speed, size);//取余，实现循环播放/
                if (_Angle < 0.f)
                bottom += tan(_Angle);//角度为负时需要将运动起始点向下移动tan(_Angle)的距离/
                if (_Speed < 0.f)
                bottom = size + bottom;
                float top = bottom + _Length * (_Speed / abs(_Speed));//top为运动方向上的上方/
                float alpha = min(1.f, abs(((bottom + top) / 2.f - i.uv.x * tan(_Angle)) - i.uv.y) / (_Length / 2.f));//根据该点距离闪光中心的距离设定alpha/
                col.rgb = lerp(fixed3(1, 1, 1), col.rgb, alpha);//alpha越高，说明距离闪光中心越远，取值越接近原像素颜色/
                return col;
            }
            ENDCG
        }
    }
}
