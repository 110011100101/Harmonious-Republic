using Godot;
using Nocturne.Core.Class;
using Godot.Collections;
using System.Linq;
using System.Threading.Tasks;

public partial class MapGenerator : Node2D
{
    [Export] private TileSet tileSet;
    private Dictionary<Vector2I, Vector3> informationMaps;
    private Dictionary<Vector2I, EnumMaterial> environmentMap;
    private Block[,,] gameMap;

    public override void _Ready()
    {
        // 准备数据
        int plateSize = GetNode<Data>("/root/Data").plateSize;
        int subdivisionFactor = GetNode<Data>("/root/Data").subdivisionFactor;

        // 准备容器
        Container<int, Dictionary<Vector2I, Vector3>> mapGeneratorContainer = new();
        Framework<int, Dictionary<Vector2I, Vector3>> mapGenerator = new(mapGeneratorContainer);
        Container<Dictionary<Vector2I, Vector3>, Dictionary<Vector2I, EnumMaterial>> environmentGeneratorContainer = new();
        Framework<Dictionary<Vector2I, Vector3>, Dictionary<Vector2I, EnumMaterial>> environmentGenerator = new(environmentGeneratorContainer);

        // 添加单元
        mapGeneratorContainer.Add(new GenerateHeightMapUnit());
        mapGeneratorContainer.Add(new GenerateHumidityMapUnit(plateSize));
        mapGeneratorContainer.Add(new GenerateTemperatureMapUnit(plateSize));
        environmentGeneratorContainer.Add(new GenerateEnvironmentMapUnit(plateSize));

        // 执行
        informationMaps = mapGenerator.Main(plateSize);
        environmentMap = environmentGenerator.Main(informationMaps);
        gameMap = Generate3DMap(plateSize);

        // 输出结果
        OutPutMap(informationMaps, environmentMap, gameMap);

        // 渲染地图
        TileMapLayer map = AddMapIntoTree();
        InitizedMap(map, tileSet);
        RenderMap(environmentMap, informationMaps, map);
    }

    public TileMapLayer AddMapIntoTree()
    {
        TileMapLayer map = new TileMapLayer();

        AddChild(map);

        return map;
    }

    public void InitizedMap(TileMapLayer map, TileSet tileSet)
    {
        map.Name = "map";
        map.TileSet = tileSet;
        float scale = 15.625f / GetNode<Data>("/root/Data").plateSize;
        map.Scale = new Vector2(scale, scale);
    }

    public void OutPutMap(Dictionary<Vector2I, Vector3> informationMaps, Dictionary<Vector2I, EnumMaterial> environmentMap, Block[,,] gameMap)
    {
        GetNode<Data>("/root/Data").informationMaps = informationMaps;
        GetNode<Data>("/root/Data").environmentMap = environmentMap;
        GetNode<Data>("/root/Data").gameMap = gameMap;
    }

    public void RenderMap(Dictionary<Vector2I, EnumMaterial> environmentMap, Dictionary<Vector2I, Vector3> informationMaps, TileMapLayer map)
    {
        foreach (Vector2I item in environmentMap.Keys)
        {
            map.SetCell(item, (int)environmentMap[item], new Vector2I((int)(informationMaps[item].X / (int)Mathf.Sqrt(informationMaps.Count()) * 4), 0));
        }
    }

    // 没有办法, 这个
    public Block[,,] Generate3DMap(int plateSize)
    {
        Block[,,] map = new Block[plateSize, plateSize, plateSize];

        Parallel.For(0, plateSize, x =>
        {
            Parallel.For(0, plateSize, y =>
            {
                int heightLimit = (int)informationMaps[new Vector2I(x, y)].X;
                Parallel.For(0, plateSize, z =>
                {
                    if (z > heightLimit)
                    {
                        map[x, y, z] = new Block() { material = EnumMaterial.Air };
                    }
                    else if (z <= heightLimit && z > heightLimit - 20)
                    {
                        map[x, y, z] = new Block() { material = environmentMap[new Vector2I(x, y)] };
                    }
                    else
                    {
                        map[x, y, z] = new Block() { material = EnumMaterial.Stone };
                    }
                });
            });
        });

        return map;
    }
}