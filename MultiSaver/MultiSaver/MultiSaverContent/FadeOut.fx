float4x4 View;
float4x4 Projection;

int Time;

texture PhotoTexture;
sampler PhotoSampler = sampler_state 
{

	texture = <PhotoTexture>;
	AddressU = Wrap;
	AddressV = Wrap;
	MinFilter = Anisotropic;
	MagFilter = Anisotropic;
	MipFilter = Point;

};

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float3 Normal   : NORMAL0;
	float2 UV : TEXCOORD0;

};

struct VertexShaderOutput
{
    float4 Position : POSITION0;	
	float3 Normal   : NORMAL0;
	float2 UV : TEXCOORD0;

};

VertexShaderOutput VertexShaderFunction(VertexShaderInput Input)
{

    VertexShaderOutput Output;

	Output.Position = mul(Input.Position, mul(View, Projection));
	Output.Normal = Input.Normal;
	Output.UV = Input.UV;

    return Output;

}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{

	float4 Color = tex2D(PhotoSampler, input.UV);
	
	float Alpha = clamp((input.Normal.x - Time) * input.Normal.y / 255, 0, 1);

    return float4(Color.x, Color.y, Color.z, Alpha);

}

technique Technique1
{

    pass Pass1
    {

        // TODO: set renderstates here.

        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();

    }

}