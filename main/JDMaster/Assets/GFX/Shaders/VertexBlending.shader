Shader "2-Part Vertex Blend" { 

 

Properties 

{ 

   _Color ("Main Color", Color) = (1,1,1) 

   _SpecColor ("Specular Color", Color) = (1,1,1) 

   _Shininess ("Shininess", Range (0,1) ) = 0.7 

   _Emission ("Emmisive Color", Color) = (0,0,0) 

   _Texture1 ("Texture 1 (white Alpha)", 2D) = "" 

   _Texture2 ("Texture 2 (black Alpha)", 2D) = "" 

} 

 

SubShader 

{ 

   BindChannels 

   { 

      Bind "vertex", vertex 

      Bind "texcoord", texcoord 

      Bind "color", color 

      Bind "normal", normal 

   } 

    

   Pass{SetTexture[_Texture1] {Combine texture * primary} } 

    

   Pass 

   { 

      Blend One One 

       

      SetTexture[_Texture2] {Combine previous Lerp(primary) texture} 

      SetTexture[_] {Combine previous * one - primary} 

   } 

    

   // Same as using Double, with SeparateSpecular Off. 

   Pass 

   { 

      Blend DstColor SrcColor 

       

      Material 

      { 

         Ambient [_Color] 

         Diffuse [_Color] 

         Specular [_SpecColor] 

         Shininess [_Shininess] 

         Emission [_Emission] 

      } 

      Lighting On 

   } 

} 

 

}