float random(in float2 value)
{
    value = frac(value * 0.3183099 + 0.1);
    value *= 17.0;
    return frac(value.x * value.y * (value.x + value.y));
}

float3 voronoi( in float2 x, in float u_time) {
    float2 n = floor(x);
    float2 f = frac(x);
    
    float2 mg, mr;
    float md = 8.0;
    for (int j= -1; j <= 1; j++) {
        for (int i= -1; i <= 1; i++) {
            float2 g = float2(float(i),float(j));
            float2 o = random( n + g );
            o = 0.5 + 0.5*sin( u_time + 6.2831*o );

            float2 r = g + o - f;
            float d = dot(r,r);

            if( d<md ) {
                md = d;
                mr = r;
                mg = g;
            }
        }
    }
    
    md = 8.0;
    for (int j= -2; j <= 2; j++) {
        for (int i= -2; i <= 2; i++) {
            float2 g = mg + float2(float(i),float(j));
            float2 o = random( n + g );
            o = 0.5 + 0.5 * sin( u_time + 6.2831*o );

            float2 r = g + o - f;

            if ( dot(mr-r,mr-r)>0.00001 ) {
                md = min(md, dot( 0.5*(mr+r), normalize(r-mr) ));
            }
        }
    }
    return float3(md, mr);
}

void VoronoiNoiseBorder_float(float2 Value, float Time, float width, out float Out)
{
    float d = voronoi(Value, Time).x;

    float color = smoothstep(0.00, width, d);

    Out = color;
}