using Godot;

namespace HarmoniousRepublic.Code.Library;

public partial class NocturneNoise : FastNoiseLite
{
    // 这个是我调好的噪声, 别乱动
    public NocturneNoise()
    {
        Seed = (int)GD.Randi();
        NoiseType = NoiseTypeEnum.Perlin;
        Frequency = 0.0735f;
        FractalOctaves = 10;
        FractalLacunarity = 2.7f;
        FractalGain = 0.27f;
    }
}