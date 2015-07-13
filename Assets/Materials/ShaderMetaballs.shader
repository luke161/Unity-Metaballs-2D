Shader "Custom/ShaderMetaballs" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		
		Cull Off
		
        Pass {
			 CGPROGRAM

	            #pragma vertex vert
	            #pragma fragment frag
	            
	            uniform fixed4 _Color;

	            float4 vert(float4 v:POSITION) : SV_POSITION {
	                return mul (UNITY_MATRIX_MVP, v);
	            }

	            fixed4 frag() : SV_Target {
	                return _Color;
	            }

	          ENDCG
         }
	} 
}
