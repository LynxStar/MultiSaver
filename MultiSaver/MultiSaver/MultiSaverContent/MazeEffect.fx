float4x4 View;
float4x4 Projection;

texture MazeTexture;
sampler MazeSampler = sampler_state 
{

	texture = <MazeTexture>;
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

    return tex2D(MazeSampler, input.UV);

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