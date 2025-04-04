#ifndef RGB_TO_CMYK_INCLUDED
#define RGB_TO_CMYK_INCLUDED

void RGBtoCMYK_float(float3 rgb, out float4 cmyk)
{
    float k = 1.0 - max(rgb.r, max(rgb.g, rgb.b));
    float c = (1.0 - rgb.r - k) / max(1.0 - k, 0.0001);
    float m = (1.0 - rgb.g - k) / max(1.0 - k, 0.0001);
    float y = (1.0 - rgb.b - k) / max(1.0 - k, 0.0001);
    cmyk = float4(saturate(c), saturate(m), saturate(y), saturate(k));
}

void RGBtoCMYK_half(half3 rgb, out half4 cmyk)
{
    half k = 1.0 - max(rgb.r, max(rgb.g, rgb.b));
    half c = (1.0 - rgb.r - k) / max(1.0 - k, 0.0001);
    half m = (1.0 - rgb.g - k) / max(1.0 - k, 0.0001);
    half y = (1.0 - rgb.b - k) / max(1.0 - k, 0.0001);
    cmyk = half4(saturate(c), saturate(m), saturate(y), saturate(k));
}

#endif