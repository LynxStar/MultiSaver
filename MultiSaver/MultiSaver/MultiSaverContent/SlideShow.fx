float4x4 View;
float4x4 Projection;

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
	float2 UV : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float2 UV : TEXCOORD0;

};

VertexShaderOutput VertexShaderFunction(VertexShaderInput Input)
{

    VertexShaderOutput Output;

	Output.Position = mul(Input.Position, mul(View, Projection));
	Output.UV = Input.UV;

    return Output;

}


float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{

    return tex2D(PhotoSampler, input.UV);

}

technique Technique1
{

    pass Pass1
    {

        // TODO: set renderstates here.

        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();

    }

}
