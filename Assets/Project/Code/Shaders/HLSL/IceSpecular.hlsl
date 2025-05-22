void IceSpecular_float(float3 Specular, float Smoothness, float3 Color, float3 WorldNormal, float3 WorldView, out float3 Out)
{
#if SHADERGRAPH_PREVIEW
    Out = 1;
#else
    Light light = GetMainLight();
    Smoothness = exp2(10 * Smoothness + 1);
    WorldNormal = normalize(WorldNormal);
    WorldView = SafeNormalize(WorldView);
    Out = LightingSpecular(Color, normalize(light.direction), WorldNormal, WorldView, float4(Specular, 0), Smoothness);
#endif
}