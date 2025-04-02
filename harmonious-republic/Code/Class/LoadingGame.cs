using System;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;
using RoseIsland.CustomClass;

public partial class LoadingGame : AbstracLoadGame
{
    public override Matrix<int> DetailedHeightMap(Matrix<int> hightMap, int plateSize, int subdivisionFactor)
    {
        int detailedPlateSize = plateSize * subdivisionFactor;
        int detailedLevelRange = detailedPlateSize / 5;
        Matrix<int> detailedHeightMap = new Matrix<int>(detailedPlateSize);
        Matrix<int> baseHeightMap = new Matrix<int>(detailedPlateSize);

        FastNoiseLite noise = new FastNoiseLite()
        {
            Seed = (int)GD.Randi(),                             // 随机种子
            NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin,
            Frequency = 0.0495f,                                // 这个值越小地图越大块
            FractalOctaves = 3,                                 // 增加分形层数
            FractalLacunarity = 2.7f,                           // 增强层间差异(值越大层间边缘越破碎)
            FractalGain = 0.27f,                                // 控制振幅衰减(值越大地图越稀碎)
        };

        // 生成高颗粒度高度图
        for (int x = 0; x < detailedPlateSize; x++)
        for (int y = 0; y < detailedPlateSize; y++)
        {
            float noiseValue = noise.GetNoise2D(x, y);
            int height = (int)((noiseValue + 1.0f) * detailedLevelRange);       // [-1,1] => [0.00, detailedLevelRange]

            detailedHeightMap.SetValue(new Vector2I(x, y), height);
        }

        // 构建基准高度图
        for (int x = 0; x < plateSize; x++)
        for (int y = 0; y < plateSize; y++)
        {
            for (int i = x * subdivisionFactor ; i < ((x + 1) * subdivisionFactor); i++)
            for (int j = y * subdivisionFactor ; j < ((y + 1) * subdivisionFactor); j++)
            {
                baseHeightMap.SetValue(new Vector2I(i, j), detailedHeightMap.GetValue(new Vector2I(i, j)) * detailedLevelRange);
            }
        }

        // 将基准高度图叠加到高颗粒度图上
        detailedHeightMap = detailedHeightMap.AddMatrix(baseHeightMap);
        // detailedHeightMap.Print();

        // GD.Print();
        // GD.Print(detailedLevelRange);

        return detailedHeightMap;
    }
    
    public override Matrix<int> DetailedEnvironmentMap(Matrix<int> heightMap, Matrix<int> detailedHeightMap, Matrix<float> humidityMap, Matrix<float> temperatureMap, Matrix<int> environmentMap)
    {
        GD.Print("任务完成");
        return environmentMap; /////////////
    }
}