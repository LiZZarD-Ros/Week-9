Shader "Custom/URP_Gradient"
{
    Properties
    {
        _TopColor ("Top Color", Color) = (1,1,1,1)
        _BottomColor ("Bottom Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 200

        Pass
        {
            Name "GradientPass"
            Tags { "LightMode"="UniversalForward" }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct Attributes
            {
                float4 position : POSITION;
            };
            
            struct Varyings
            {
                float4 position : SV_POSITION;
                float2 screenPos : TEXCOORD0;
            };
            
            fixed4 _TopColor;
            fixed4 _BottomColor;
            
            Varyings vert (Attributes v)
            {
                Varyings o;
                o.position = UnityObjectToClipPos(v.position);
                o.screenPos = o.position.xy;
                return o;
            }
            
            fixed4 frag (Varyings i) : SV_Target
            {
                float screenY = i.screenPos.y / i.position.w;
                screenY = 1.0 - screenY;
                return lerp(_BottomColor, _TopColor, screenY);
            }
            ENDHLSL
        }
    }
    FallBack "Unlit/Color"
}
