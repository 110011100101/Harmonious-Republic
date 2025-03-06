using System.Security.Cryptography.X509Certificates;
using Godot;
using Godot.Collections;
using MapGeneration;

public partial class MapGenerator : AbstractMapGenerator
{
    private float scaleValue = 1;
    public override TileMapLayer AddMapIntoTree()
    {
        TileMapLayer map  = new TileMapLayer();
        
        AddChild(map);

        return map;
    }

    public override void InitizedMap(TileMapLayer map, TileSet tileSet)
    {
        map.Name = "map";
        map.TileSet = tileSet;
    }

    public override Array<Vector2I> GenerateBaseMap(int mapSize)
    {
        Array<Vector2I> cells = new Array<Vector2I>();

        for (int i = 0; i < mapSize; i++)
        for (int j = 0; j < mapSize; j++)
        {
            cells.Add(new Vector2I(i, j));
        }

        return cells;
    }

    public override Dictionary<Vector2I, int> GenerateHeightMap(Array<Vector2I> cells)
    {
        Dictionary<Vector2I, int> heightMap = new Dictionary<Vector2I, int>();
        FastNoiseLite noise = new FastNoiseLite()
        {
            Seed = (int)GD.Randi(),                             // 关键修改点
            NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin,
            Frequency = 0.05f,                                  // 这个值越小地图越大块(平滑)
            FractalOctaves = 5,                                 // 增加分形层数
            FractalLacunarity = 3,                              // 增强层间差异
            FractalGain = 0.27f,                                // 控制振幅衰减
        };
        
        foreach (var cell in cells)
        {
            float noiseValue = noise.GetNoise2D(cell.X, cell.Y);
            float stretched = (noiseValue + 1.1f) * 0.55f;          // [-1,1] => [0.05, 1.05]
            int height = Mathf.Clamp((int)(stretched * 4), 0, 4);
            heightMap[cell] = height;
        }
        return heightMap;
    }

    public override void UpdateMapFromHeightMap(Dictionary<Vector2I, int> heightMap, TileMapLayer map, int mapSize)
    {
        scaleValue = 125f / (8f * mapSize);
        map.Scale = new Vector2(scaleValue, scaleValue);

        foreach (Vector2I cell in heightMap.Keys)
        {
            int height = heightMap[cell];
            
            map.SetCell(cell, (int)EnumMaterial.Soil , new Vector2I(height, 0));
        }
    }

    public void UpdateMapScale(float scale)
    {
        GetChild<TileMapLayer>(0).Scale = new Vector2((scale / 100) + scaleValue, (scale / 100) + scaleValue);
    }
}