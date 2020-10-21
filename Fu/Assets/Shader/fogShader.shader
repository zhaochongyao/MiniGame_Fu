Shader "MyShader/fogShader"
{
    ///迷雾着色器 
    /// 只实现了基础的迷雾追踪人物
    ///原理:采用透明度混合在顶层渲染迷雾贴图,根据传入的object坐标转化为裁剪坐标实现跟随移动
    //挂载在原本的迷雾材质上
    Properties
    {
            _Texture0("光源", 2D) = "black"{}
            _Texture1("迷雾背景", 2D) = "black"{}
            _HideRate("迷雾遮盖浓度",Range(0,1)) = 1.0
            _MaxBlack("最大阴暗度",Range(0,1)) = 0.85
            [HideInInspector]_XScrollLength("XScrollLength", float) = 0      //X轴移动长度
            [HideInInspector]_YScrollLength("YScrollLength", float) = 0      //y轴移动长度
            //防止缩放迷雾的时候跟随主角移动出现误差,需要在迷雾的gameObject上挂载脚本实时update rate
            _XScaleRate("X轴缩放程度",float) = 1                            //x轴scale
            _YScaleRate("Y轴缩放程度",float) = 1                            //y轴scale
            [HDR] _Color("Color", Color) = (1, 1, 1, 1)                      //颜色
    }
        SubShader
    {
        Tags {  "Queue" = "Overlay"}
        Blend DstColor OneMinusSrcAlpha                             //透明度混合
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            sampler2D _Texture0;
            sampler2D _Texture1;
            float _XScrollLength;
            float _YScrollLength;
            float _XScaleRate;
            float _YScaleRate;
            float _HideRate;
            float _MaxBlack;
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
                o.pos = UnityObjectToClipPos(v.vertex-float3(_XScrollLength/_XScaleRate,_YScrollLength/_YScaleRate,0)); //根据player的偏移量计算渲染偏移量  
                o.uv = v.uv ;
                return o;
            }
            half4 frag(v2f i) :SV_Target{
                   half3 color1 = tex2D(_Texture0,i.uv).rgb;                                               
                   half3 color2 = tex2D(_Texture1, i.uv).rgb;               
                   if (color1.r < 1-_HideRate && color1.g < 1-_HideRate && color1.b < 1-_HideRate) {         //根据遮盖比例调整遮盖部分的rgb值
                       color1.rgb = float3(1-_HideRate, 1-_HideRate, 1-_HideRate);
                   }
                   half3 result = color1;                                              
                  
                   return half4(result, _MaxBlack);
            }
            ENDCG
        }

    }
}
