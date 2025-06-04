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
                // 执行逻辑
                Vector2I block = new Vector2I(x, y);
                float noiseValue;
                float height;
                float humidity;
                float baseTemperature;
                float fluctuationLatitude;
                float fluctuationHeight;
                float fluctuationHumidity;
                float temperature;

                noiseValue = noise.GetNoise2D(x, y);
                height = informationMaps[block].X;
                humidity = informationMaps[block].Y;

                baseTemperature = -20f;
                fluctuationLatitude = y; // I hope base temperature is frome -20 to 60.
                fluctuationHeight = height / 100; // [0, 4] -> [0, 20]
                fluctuationHumidity = humidity; // [0, 100] -> [0, 100]
                temperature = baseTemperature + fluctuationLatitude - (fluctuationHeight + fluctuationHumidity) * ((noiseValue + 1f) / 2f); // [-140, 160], noise[-1, 1] -> [0, 1]

                // 数据格式化
                Vector3 originalInformation;
                Vector3 information;
                float X;
                float Y;
                float Z;

                originalInformation = informationMaps[block];
                X = originalInformation.X;
                Y = originalInformation.Y;
                Z = temperature;
                information = new Vector3(X, Y, Z);
                informationMaps[block] = information;
            }
        }

        return informationMaps;
    }
}