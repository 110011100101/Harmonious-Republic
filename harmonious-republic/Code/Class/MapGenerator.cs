using System;
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
            Seed = (int)GD.Randi(),                             // 随机种子
            NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin,
            Frequency = 0.0495f,                                // 这个值越小地图越大块
            FractalOctaves = 3,                                 // 增加分形层数
            FractalLacunarity = 2.7f,                           // 增强层间差异(值越大层间边缘越破碎)
            FractalGain = 0.27f,                                // 控制振幅衰减(值越大地图越稀碎)
        };
        
        for(int x = 0; x < plateSize; x++)
        for(int y = 0; y < plateSize; y++)
        {
            float noiseValue = noise.GetNoise2D(x, y);
            int height = (int)((noiseValue + 1.0f) * 2.0f);       // [-1,1] => [0.00, 4.00]

            heightMap.SetValue(new Vector2I(x, y), height);
        }

        return heightMap;
    }

    public override Matrix<float> GenerateHumidityMap(Matrix<int> heightMap)
    {
        Matrix<float> humidityMap = new Matrix<float>(heightMap.GetSize());
        FastNoiseLite noise = new FastNoiseLite()
        {
            Seed = (int)GD.Randi(),                             // 随机种子
            NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin,
            Frequency = 0.02f,                                  // 控制湿度分布的范围
            FractalOctaves = 3,                                 // 增加分形层数
            FractalLacunarity = 2.0f,                           // 增强层间差异
            FractalGain = 0.5f                                  // 控制振幅衰减
        };

        for (int x = 0; x < heightMap.GetSize(); x++)
        for (int y = 0; y < heightMap.GetSize(); y++)
        {
            float noiseValue = noise.GetNoise2D(x, y);
            int height = heightMap.GetValue(new Vector2I(x, y));

            float humidityValue = Mathf.Clamp(((noiseValue + 1) * 100) + height * noiseValue * 10, 0, 100) ;
            
            humidityMap.SetValue(new Vector2I(x, y), humidityValue);
        }

        return humidityMap;
    }

    public override Matrix<float> GenerateTemperatureMap(Matrix<int> heightMap, Matrix<float> humidityMap)
    {
        Matrix<float> temperatureMap = new Matrix<float>(heightMap.GetSize());
        FastNoiseLite noise = new FastNoiseLite()
        {
            Seed = (int)GD.Randi(),                             // 随机种子
            NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin,
            Frequency = 0.03f,                                  // 控制温度分布的范围
            FractalOctaves = 3,                                 // 增加分形层数
            FractalLacunarity = 2.0f,                           // 增强层间差异
            FractalGain = 0.5f                                  // 控制振幅衰减
        };

        for (int x = 0; x < heightMap.GetSize(); x++)
        for (int y = 0; y < heightMap.GetSize(); y++)
        {
            int height = heightMap.GetValue(new Vector2I(x, y));
            float humidity = humidityMap.GetValue(new Vector2I(x, y));
            float noiseValue = noise.GetNoise2D(x, y);

            // 调整温度计算逻辑
            float temperature = Mathf.Clamp((-20 + y - (height * 5) - humidity * noiseValue) * 0.5f, -40, 60);

            temperatureMap.SetValue(new Vector2I(x, y), temperature);
        }

        return temperatureMap;
    }

    public override Matrix<int> GenerateEnvironmentMap(Matrix<int> heightMap, Matrix<float> humidityMap, Matrix<float> temperatureMap)
    {
        Matrix<int> environmentMap = new Matrix<int>(heightMap.GetSize());

        for (int x = 0; x < heightMap.GetSize(); x++)
        for (int y = 0; y < heightMap.GetSize(); y++)
        {
            int height = heightMap.GetValue(new Vector2I(x, y));
            float humidity = humidityMap.GetValue(new Vector2I(x, y));
            float temperature = temperatureMap.GetValue(new Vector2I(x, y));

            EnumMaterial material = EnumMaterial.Air; // Default material

            if (humidity <= 10)
            {
                material = EnumMaterial.Stone;
            }
            else if (humidity <= 30)
            {
                material = temperature < -10 ? EnumMaterial.Stone : EnumMaterial.Soil;
            }
            else if (humidity <= 80)
            {
                material = temperature < -10 ? EnumMaterial.Snow : temperature < 20 ? EnumMaterial.Soil : EnumMaterial.Grass;
            }
            else if (humidity <= 100)
            {
                switch (height)
                {
                    case 0:
                        material = temperature < 0 ? EnumMaterial.Ice : EnumMaterial.Water;
                        break;
                    case 1:
                        material = temperature < 0 ? EnumMaterial.Ice : EnumMaterial.Water;
                        break;
                    case 2:
                        material = temperature < 0 ? EnumMaterial.Stone : EnumMaterial.Grass;
                        break;
                    case 3:
                        material = temperature < 0 ? EnumMaterial.Snow : EnumMaterial.Soil;
                        break;
                    case 4:
                        material = temperature < 0 ? EnumMaterial.Snow : EnumMaterial.Stone;
                        break;
                }
            }

            environmentMap.SetValue(new Vector2I(x, y), (int)material);
        }

        return environmentMap;
    }

    public override void RenderMap(Matrix<int> heightMap, Matrix<int> environmentMap, TileMapLayer map, int mapSize)
    {
        float scaleValue = 125f / (8f * mapSize);
        map.Scale = new Vector2(scaleValue, scaleValue);

        for (int x = 0; x < mapSize; x++)
        for (int y = 0; y < mapSize; y++)
        {
            int material = environmentMap.GetValue(new Vector2I(x, y));

            map.SetCell(new Vector2I(x, y), material, new Vector2I(heightMap.GetValue(new Vector2I(x, y)), 0));
        }
    }

    public void UpdateMapScale(float scale)
    {
        Scale = new Vector2(Mathf.Clamp((scale * 10 / 100) + 1, 1, 100), Mathf.Clamp((scale * 10 / 100) + 1, 1, 100)) ;
    }
}