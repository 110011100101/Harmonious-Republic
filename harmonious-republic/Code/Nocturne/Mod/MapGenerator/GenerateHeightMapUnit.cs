using Godot;
using Godot.Collections;
using Nocturne.Core.Class;

public class GenerateHeightMapUnit : Unit<int, Dictionary<Vector2I, Vector3>>
{
    public override Dictionary<Vector2I, Vector3> Execute(int plateSize)
    {
        NocturneNoise noise = new NocturneNoise();
        Dictionary<Vector2I, Vector3> informationMaps = new Dictionary<Vector2I, Vector3>();

        for (int x = 0; x < plateSize; x++)
        {
            for (int y = 0; y < plateSize; y++)
            {
                float height;
                float noiseValue;
                Vector2I block;
                Vector3 information;

                block = new Vector2I(x, y);
                noiseValue = noise.GetNoise2D(x, y);
                height = (int)((noiseValue + 1f) * 50f); // noise[-1,1] -> [0, 2] -> [0.00, 100]
                information = new Vector3(height, 0, 0);
                informationMaps[block] = information;
            }
        }

        return informationMaps;
    }
}