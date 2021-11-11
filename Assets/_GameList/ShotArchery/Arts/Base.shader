Shader "mShader/Base"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags{"Queue" = "Transparent" "IgnoreProjector" = "True" "ReaderType" = "Transparent"}
        Lighting Off ZWrite Off Fog{Mode off}

        BindChannels
        {
            //Bind "Color", color
            Bind "Vertex", vertex
            Bind "texcoord", texcoord
        }

        Pass
        {
            ColorMaterial AmbientAndDiffuse
            SetTexture[_MainTex] {Combine texture * primary}
        }
    }
}