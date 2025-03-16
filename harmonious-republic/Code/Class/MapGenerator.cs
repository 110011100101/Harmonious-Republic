using System.Security.Cryptography.X509Certificates;
using Godot;
using MapGeneration;
using RoseIsland.CustomClass;

using EnvironmentPair = System.Collections.Generic.KeyValuePair<Godot.Vector2I, EnumMaterial>;
using HeightPair = System.Collections.Generic.KeyValuePair<Godot.Vector2I, int>;

public partial class MapGenerator : AbstractMapGenerator
{
    public override TileMapLayer AddMapIntoTree()
    {
        TileMapLayer map = new TileMapLayer();
        
        AddChild(map);
        
        return map;
    }

    public override void InitizedMap(TileMapLayer map, TileSet tileSet)
    {
        map.Name = "map";
        map.TileSet = tileSet;
    }

    public override Matrix<int> GenerateHeightMap(int plateSize)
    {
        Matrix<int> heightMap = new Matrix<int>(plateSize);

        FastNoiseLite noise = new FastNoiseLite()
        {
            Seed = (int)GD.Randi(),                             // 关键修改点
            NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin,
            Frequency = 0.0495f,                                // 这个值越小地图越大块(平滑)
            FractalOctaves = 5,                                 // 增加分形层数
            FractalLacunarity = 3,                              // 增强层间差异
            FractalGain = 0.27f,                                // 控制振幅衰减
        };
        
        for(int x = 0; x < plateSize; x++)
        for(int y = 0; y < plateSize; y++)
        {
            float noiseValue = noise.GetNoise2D(x, y);
            float stretched = (noiseValue + 1.1f) * 0.55f;          // [-1,1] => [0.05, 1.05]
            int height = Mathf.Clamp((int)(stretched * 4), 0, 4);
            heightMap.SetValue(new Vector2I(x, y), height);
        }

        return heightMap;
    }

    public override Matrix<float> GenerateHumidityMap(Matrix<int> heightMap)
    {
        // TODO: Implement GenerateHumidityMap method
        return new Matrix<float>(heightMap.GetSize());
    }

    public override Matrix<float> GenerateTemperatureMap(Matrix<int> heightMap, Matrix<float> humidityMap)
    {
        // TODO: Implement GenerateTemperatureMap method
        return new Matrix<float>(heightMap.GetSize());
    }

    public override Matrix<int> GenerateEnvironmentMap(Matrix<int> heightMap, Matrix<float> humidityMap, Matrix<float> temperatureMap)
    {
        // TODO: Implement GenerateEnvironmentMap method
        return heightMap;
    }

    public override void RenderMap(Matrix<int> environmentMap, TileMapLayer map, int mapSize)
    {
        float scaleValue = 125f / (8f * mapSize);
        map.Scale = new Vector2(scaleValue, scaleValue);

        for (int x = 0; x < mapSize; x++)
        for (int y = 0; y < mapSize; y++)
        {
            int height = environmentMap.GetValue(new Vector2I(x, y));
            EnumMaterial material = EnumMaterial.Air;

            switch (height)
            {
                case 0:
                    material = EnumMaterial.Ice;
                    break;
                case 1:
                    material = EnumMaterial.Water;
                    break;
                case 2:
                    material = EnumMaterial.Soil;
                    break;
                case 3:
                    material = EnumMaterial.Grass;
                    break;
                case 4:
                    material = EnumMaterial.Stone;
                    break;
            }

            map.SetCell(new Vector2I(x, y), (int)material, new Vector2I(0, 0));
        }

    }

    public void UpdateMapScale(float scale)
    {
        Scale = new Vector2(Mathf.Clamp((scale * 10 / 100) + 1, 1, 100), Mathf.Clamp((scale * 10 / 100) + 1, 1, 100)) ;
    }
}