Shader "MyShader/fogShader"
{
    ///迷雾着色器 
    /// 只实现了基础的迷雾和简单的追踪人物,shader用的不太熟练,大家可以一起改一下迷雾追踪主角的方式
    /// 暂时采用根据主角移动距离来调节贴图的uv坐标,但世界坐标与uv坐标的转换没查到API,因此用常数系数来
    /// 缩小uv的变化比例,模拟迷雾与主角的等距移动
    Properties
    {
            _Texture0("光源", 2D) = "black"{}
            _Texture1("迷雾背景", 2D) = "black"{}
            //测试发现横纵向坐标变化比例不一样,因此分开调试
            _PositionRateX("横向坐标比例",float) = 15                            //横向坐标变化比例
            _PositionRateY("纵向坐标比例",float) = 5                            //纵向坐标变化比例
            [HideInInspector]_XScrollLength("XScrollLength", float) = 0      //X轴移动长度
            [HideInInspector]_YScrollLength("YScrollLength", float) = 0      //y轴移动长度
            [HDR] _Color("Color", Color) = (1, 1, 1, 1)                      //颜色
    }
        SubShader
    {

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            sampler2D _Texture0;
            sampler2D _Texture1;
            float _XScrollLength;
            float _YScrollLength;
            float _PositionRateX;
            float _PositionRateY;
            //应用程序阶段结构体
            struct appdata {    
                float4 vertex:POSITION;
                float2 uv:TEXCOORD;
            };
            //顶点着色器传给片元着色器的结构体
           
            struct v2f {
                float4 pos:SV_POSITION;
                float2 uv:TEXCOORD;    
            };
            
            v2f vert(appdata v){
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv ;
                return o;
            }
            half4 frag(v2f i) :SV_Target{
                   half3 color1 = tex2D(_Texture0,i.uv + float2(_XScrollLength/_PositionRateX, _YScrollLength/_PositionRateY)).rgb;      //只改变光源贴图的uv坐标,背景不动以实现迷雾的移动
                   half3 color2 = tex2D(_Texture1, i.uv).rgb;                                    
                   half3 result = color1 * color2;                                              //颜色相乘实现正片叠底效果
                   return half4(result, 1);
            }
            ENDCG
        }

    }
}
