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
                // 执行逻辑
                Vector2I block;
                float noiseValue;
                float height;
                float baseHumidity;
                float humidityValue;

                block = new Vector2I(x, y);
                noiseValue = noise.GetNoise2D(x, y);
                height = informationMaps[block].X;
                baseHumidity = (noiseValue + 1) * 50f; // noise[-1, 1] -> [0, 100]
                humidityValue = baseHumidity - height / 100; // [0, 100] * [0, 1] -> [0, 100]

                // 格式化数据
                Vector3 originalInformation;
                Vector3 information;
                float X;
                float Y;
                float Z;

                originalInformation = informationMaps[block];
                X = originalInformation.X;
                Y = humidityValue;
                Z = originalInformation.Z;
                information = new Vector3(X, Y, Z);
                informationMaps[block] = information;
            }
        }

        return informationMaps;
    }
}