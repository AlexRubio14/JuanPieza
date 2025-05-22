#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

void IceDepth_float(
    in Texture2D MainTex,
    in float2 UV,
    in SamplerState SS,
    in float Samples,
    in float Offset,
    in float3 WPos,
    in float Lerp,
    out float4 Out
    )
{
    float4 col = 0;
    float u_off = 0;
    float v_off = 0;
    float samples = Samples;

    for (int y = 0; y < samples; y++)
    {
        float2 uvs = float2(u_off, v_off);
        col += SAMPLE_TEXTURE2D(MainTex, SS, uvs + UV);
        u_off += Offset * (_WorldSpaceCameraPos.x - WPos.x);
        v_off += Offset * (_WorldSpaceCameraPos.z - WPos.z);
    }

    float4 render = (col /= samples);
    Out = lerp(SAMPLE_TEXTURE2D(MainTex, SS, UV), render, Lerp);
}
