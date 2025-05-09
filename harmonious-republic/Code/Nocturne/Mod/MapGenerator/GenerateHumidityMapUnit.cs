using System.Linq;
using Godot;
using Godot.Collections;
using Nocturne.Core.Class;

public class GenerateHumidityMapUnit : Unit<Dictionary<Vector2I, Vector3>, Dictionary<Vector2I, Vector3>>
{
    int plateSize;

    public GenerateHumidityMapUnit(int plateSize)
    {
        this.plateSize = plateSize;
    }
    public override Dictionary<Vector2I, Vector3> Execute(Dictionary<Vector2I, Vector3> informationMaps)
    {
        NocturneNoise noise = new NocturneNoise();

        for (int x = 0; x < plateSize; x++)
        {
            for (int y = 0; y < plateSize; y++)
            {
                Vector2I block = new Vector2I(x, y);

                float noiseValue = noise.GetNoise2D(x, y);
                float height = informationMaps[block].X;

                float baseHumidity = (noiseValue + 1) * 50f; // noise[-1, 1] -> [0, 100]

                float humidityValue = baseHumidity - height / 100; // [0, 100] * [0, 1] -> [0, 100]

                Vector3 originalInformation = informationMaps[block];
                float X = originalInformation.X;
                float Y = humidityValue;
                float Z = originalInformation.Z;
                Vector3 information = new Vector3(X, Y, Z);

                informationMaps[block] = information;
            }
        }

        return informationMaps;
    }
}