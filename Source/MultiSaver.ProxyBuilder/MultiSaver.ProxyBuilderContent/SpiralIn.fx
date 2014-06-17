float4x4 View;
float4x4 Projection;

int Time;
float Rotation;

float2 Origin = float2(0, 0);

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
	float3 Normal   : NORMAL0;
	float2 UV : TEXCOORD0;

};

VertexShaderOutput VertexShaderFunction(VertexShaderInput Input)
{

    VertexShaderOutput Output;

	float2 Offset = Origin + float2(Input.Normal.y, Input.Normal.z) * clamp((Time - Input.Normal.x) / 180, 0, 1);

	float Rotation = clamp(Time - Input.Normal.x, 0, 1) * (Time - Input.Normal.x) * 3.14159265 / 90;

	Rotation = clamp(Rotation, 0, 3.14159265 * 2);

	float c = cos(Rotation);
 	float s = sin(Rotation);
	 	     
    float2x2 rotationMatrix = float2x2(c, -s, s, c);

	float2 Pos = mul(float2(Offset.x - Origin.x, Offset.y - Origin.y), rotationMatrix) + Origin;

	float4 PosF = float4(Pos.x, Pos.y, Input.Position.z, Input.Position.w);

	Output.Position = mul(PosF, mul(View, Projection));
	Output.Normal = Input.Normal;
	Output.UV = Input.UV;
		
    return Output;

}

float4 PixelShaderFunction(VertexShaderOutput input) : SV_Target
{

	return PhotoTexture.Sample(PhotoSampler, input.UV) * clamp(Time - input.Normal.x, 0, 1);

}

technique Technique1
{

    pass Pass1
    {

		VertexShader = compile vs_5_0 VertexShaderFunction();
		PixelShader = compile ps_5_0 PixelShaderFunction();

    }

}