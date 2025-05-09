using Godot;

public partial class NocturneNoise : FastNoiseLite
{
	// 这个是我调好的噪声, 别乱动
	public NocturneNoise() : base()
	{
			Seed = (int)GD.Randi();
			NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin;
			Frequency = 0.0735f;
			FractalOctaves = 10;
			FractalLacunarity = 2.7f;
			FractalGain = 0.27f;
	}
}