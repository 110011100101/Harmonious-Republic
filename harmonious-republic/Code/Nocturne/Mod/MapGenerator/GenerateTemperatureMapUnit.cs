using System.Linq;
using Godot;
using Godot.Collections;
using Nocturne.Core.Class;

public class GenerateTemperatureMapUnit : Unit<Dictionary<Vector2I, Vector3>, Dictionary<Vector2I, Vector3>>
{
    private int plateSize;

    public GenerateTemperatureMapUnit(int plateSize)
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
                float humidity = informationMaps[block].Y;

                float baseTemperature = -20f;
                float fluctuationLatitude = 100f / 100 * y; // I hope base temperature is frome -20 to 60.
                float fluctuationHeight = height / 100; // [0, 4] -> [0, 20]
                float fluctuationHumidity = humidity; // [0, 100] -> [0, 100]

                float temperature = baseTemperature + fluctuationLatitude - (fluctuationHeight + fluctuationHumidity) * ((noiseValue + 1f) / 2f); // [-140, 160], noise[-1, 1] -> [0, 1]

                Vector3 originalInformation = informationMaps[block];
                float X = originalInformation.X;
                float Y = originalInformation.Y;
                float Z = temperature;
                Vector3 information = new Vector3(X, Y, Z);

                informationMaps[block] = information;
            }
        }

        return informationMaps;
    }
}