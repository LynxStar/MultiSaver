float4x4 View;
float4x4 Projection;

int Time;

Texture2D PhotoTexture;
sampler PhotoSampler = sampler_state 
{

	AddressU = Wrap;
	AddressV = Wrap;
	MinFilter = Anisotropic;
	MagFilter = Anisotropic;
	MipFilter = Point;

};

struct VertexShaderInput
{
	float4 Position : SV_POSITION;
	float3 Normal   : NORMAL0;
	float2 UV : TEXCOORD0;

};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float2 UV : TEXCOORD0;

};

VertexShaderOutput VertexShaderFunction(VertexShaderInput Input)
{

    VertexShaderOutput Output;

	Input.Position.x += Input.Normal.x;
	Input.Position.x = clamp(Input.Position.x - (Time * Input.Normal.z), Input.Normal.y, 5000);

	
	Output.Position = mul(Input.Position, mul(View, Projection));
	Output.UV = Input.UV;

    return Output;

}


float4 PixelShaderFunction(VertexShaderOutput input) : SV_Target
{

	return PhotoTexture.Sample(PhotoSampler, input.UV);

}

technique Technique1
{

    pass Pass1
    {

		VertexShader = compile vs_5_0 VertexShaderFunction();
		PixelShader = compile ps_5_0 PixelShaderFunction();

    }

}
