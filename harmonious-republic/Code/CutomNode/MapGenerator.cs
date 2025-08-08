using Godot;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

public partial class MapGenerator : Node2D
{
    [Export] private TileSet tileSet;
    public Block[,] environmentMap;

    // 噪声值范围常量
    private const float MinNoiseValue = -1f;
    private const float MaxNoiseValue = 1f;
    private const float HeightScaleFactor = 100f;
    
    // 温度计算相关常量
    private const float BaseTemperature = -40f;
    private const float TemperatureRange = 80f;
    
    // 湿度计算相关常量
    private const float BaseHumidity = 110f;
    private const float HumidityHeightFactor = -1f;
    
    // 温度阈值常量 - 按温度从低到高排序
    private const float IceTempThresholdHigh = -35f;
    private const float ColdTempThreshold = -30f;
    private const float IceTempThresholdLow = -15f;
    private const float CoolTempThreshold = -10f;
    private const float FreezingTempThreshold = 0f;
    private const float WarmTempThreshold = 30f;
    
    // 湿度阈值常量
    private const float HighHumidityThreshold = 30f;
    private const float LowlandLowHumidityThreshold = 20f;
    private const float LowlandMediumHumidityThreshold = 65f;
    private const float HighlandLowHumidityThreshold = 40f;

    // 地形高度界限常量
    private const int SeaLevel = 45;
    private const int LowLandUpperBound = 80;
    private const int MidLandUpperBound = 90;
    private const int HighLandUpperBound = 100;
    
    // 地图生成限制参数
    private const int MaxZLevel = 100;
    private const int SurfaceLayerDepth = 20;
    private const int MinPlateSize = 10;
    private const int MaxPlateSize = 500;
    
    // 高度层级定义，用于颜色分级
    private const int HeightLevelMin = 20;
    private const int HeightLevelMax = 80;
    private const int HeightLevelCount = 5;
    
    // 各高度等级的分界值
    private static readonly int[] HeightLevelThresholds = {
        32, // 20-31: 等级0
        44, // 32-43: 等级1
        56, // 44-55: 等级2
        68, // 56-67: 等级3
        80  // 68-80: 等级4
    };
    
    // 土壤颜色数组
    private static readonly Color[] SoilColors = {
        new Color(0.114f, 0.071f, 0.047f),
        new Color(0.204f, 0.125f, 0.09f),
        new Color(0.318f, 0.196f, 0.137f),
        new Color(0.341f, 0.212f, 0.153f),
        new Color(0.522f, 0.314f, 0.216f)
    };

    // 水颜色数组
    private static readonly Color[] WaterColors = {
        new Color(0.11f, 0.349f, 0.4f),
        new Color(0.129f, 0.424f, 0.486f),
        new Color(0.165f, 0.541f, 0.612f),
        new Color(0.204f, 0.659f, 0.753f),
        new Color(0.231f, 0.753f, 0.859f)
    };

    // 草颜色数组
    private static readonly Color[] GrassColors = {
        new Color(0.227f, 0.4f, 0.106f),
        new Color(0.282f, 0.494f, 0.125f),
        new Color(0.322f, 0.569f, 0.145f),
        new Color(0.361f, 0.631f, 0.165f),
        new Color(0.388f, 0.694f, 0.176f)
    };

    // 石头颜色数组
    private static readonly Color[] StoneColors = {
        new Color(0.192f, 0.184f, 0.18f),
        new Color(0.263f, 0.251f, 0.239f),
        new Color(0.325f, 0.314f, 0.298f),
        new Color(0.384f, 0.388f, 0.388f),
        new Color(0.482f, 0.459f, 0.494f)
    };

    // 沙子颜色数组
    private static readonly Color[] SandColors = {
        new Color(0.42f, 0.392f, 0.302f),
        new Color(0.494f, 0.463f, 0.353f),
        new Color(0.69f, 0.647f, 0.494f),
        new Color(0.69f, 0.647f, 0.494f),
        new Color(0.753f, 0.706f, 0.545f)
    };

    // 冰颜色数组
    private static readonly Color[] IceColors = {
        new Color(0.02f, 0.29f, 0.431f),
        new Color(0.024f, 0.337f, 0.498f),
        new Color(0.024f, 0.427f, 0.624f),
        new Color(0.027f, 0.486f, 0.714f),
        new Color(0.031f, 0.557f, 0.82f)
    };

    // 雪颜色数组
    private static readonly Color[] SnowColors = {
        new Color(0.475f, 0.475f, 0.475f),
        new Color(0.725f, 0.725f, 0.725f),
        new Color(0.725f, 0.725f, 0.725f),
        new Color(0.8f, 0.8f, 0.8f),
        new Color(0.875f, 0.875f, 0.875f)
    };
    
    // 材质颜色映射字典
    private static readonly Dictionary<EnumMaterial, Color[]> MaterialColorMap = new Dictionary<EnumMaterial, Color[]>
    {
        { EnumMaterial.Soil, SoilColors },
        { EnumMaterial.Water, WaterColors },
        { EnumMaterial.Grass, GrassColors },
        { EnumMaterial.Stone, StoneColors },
        { EnumMaterial.Sand, SandColors },
        { EnumMaterial.Ice, IceColors },
        { EnumMaterial.Snow, SnowColors }
    };

    public override void _Ready()
    {
        try
        {
            // 获取地图尺寸参数
            int plateSize = GetNode<Data>("/root/Data").plateSize;
            
            // 验证地图尺寸参数
            if (plateSize < MinPlateSize || plateSize > MaxPlateSize)
            {
                return;
            }
            
            // 生成游戏地图数据
            Block[,,] gameMap = GenerateMap(plateSize);
            
            // 检查地图是否成功生成
            if (gameMap == null)
            {
                return;
            }
            
            // 创建用于渲染的图像
            Image environmentImage = Image.CreateEmpty(plateSize, plateSize, false, Image.Format.Rgba8);

            // 输出地图数据到Data节点
            OutPutMap(gameMap);

            // 渲染地图图像
            RenderMap(environmentMap, environmentImage, plateSize);
            
            // 打印地图高度统计信息
            PrintHeightDebugInfo(environmentMap);
        }
        catch (System.Exception)
        {
            // 异常处理
        }
    }

    // 将生成的地图数据输出到Data节点
    public void OutPutMap(Block[,,] gameMap)
    {
        // 检查地图数据是否有效
        if (gameMap == null)
        {
            return;
        }
        
        GetNode<Data>("/root/Data").gameMap = gameMap;
        GetNode<Data>("/root/Data").environmentMap = environmentMap;
    }

    // 渲染地图图像
    public void RenderMap(Block[,] environmentMap, Image environmentImage, int plateSize)
    {
        // 参数验证
        if (environmentMap == null || environmentImage == null || plateSize <= 0)
        {
            return;
        }

        // 遍历环境地图的每个位置，设置对应的颜色
        for (int i = 0; i < environmentMap.GetLength(0); i++)
        {
            for (int j = 0; j < environmentMap.GetLength(1); j++)
            {
                // 检查数组边界和元素有效性
                if (i >= environmentMap.GetLength(0) || j >= environmentMap.GetLength(1) || environmentMap[i, j] == null)
                    continue;

                // 调整Z值范围，将20-80分成5份，低于20的视为20，高于80的视为80
                int adjustedZ = environmentMap[i, j].position.Z;
                if (adjustedZ < HeightLevelMin) adjustedZ = HeightLevelMin;
                if (adjustedZ > HeightLevelMax) adjustedZ = HeightLevelMax;

                // 根据材质和高度确定颜色
                Color color = GetBlockColorByMaterialAndHeight(environmentMap[i, j].material, adjustedZ);
                environmentImage.SetPixel(i, j, color);
            }
        }

        // 调整图像大小并添加到场景中
        environmentImage.Resize(1000, 1000, Image.Interpolation.Nearest);
        float offset = 500f;
        AddChild(new Sprite2D()
        {
            Name = "map",
            Texture = ImageTexture.CreateFromImage(environmentImage),
            Position = new Vector2(offset, offset),
        });
    }

    // 根据材质和高度获取方块颜色
    private Color GetBlockColorByMaterialAndHeight(EnumMaterial material, int height)
    {
        int heightLevel = GetHeightLevel(height);
        
        // 使用字典映射获取颜色
        if (MaterialColorMap.TryGetValue(material, out Color[] colors))
        {
            // 确保高度等级在有效范围内
            if (heightLevel >= 0 && heightLevel < colors.Length)
            {
                return colors[heightLevel];
            }
        }
        
        // 默认返回透明色（对于Air等未在映射中的材质）
        return new Color(0, 0, 0, 0);
    }

    // 使用二分查找获取高度等级
    private int GetHeightLevel(int height)
    {
        // 调整高度范围
        if (height < HeightLevelMin) height = HeightLevelMin;
        if (height > HeightLevelMax) height = HeightLevelMax;
        
        // 使用二分查找确定高度等级，提高查找效率
        int left = 0;
        int right = HeightLevelThresholds.Length - 1;
        
        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            if (height < HeightLevelThresholds[mid])
            {
                right = mid - 1;
            }
            else
            {
                left = mid + 1;
            }
        }
        
        return left < HeightLevelCount ? left : HeightLevelCount - 1;
    }

    // 生成地图数据
    public Block[,,] GenerateMap(int plateSize)
    {
        // 参数验证
        if (plateSize < MinPlateSize || plateSize > MaxPlateSize)
        {
            return null;
        }
        
        try
        {
            // 初始化环境地图和游戏地图
            environmentMap = new Block[plateSize, plateSize];
            Block[,,] gameMap = new Block[plateSize, plateSize, plateSize];
            
            // 创建噪声生成器
            FastNoiseLite noise = new FastNoiseLite()
            {
                NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin,
                Seed = (int)GD.Randi(),
                Frequency = 7.35f / plateSize,
            };

            // 遍历每个地图位置生成地形数据
            for (int x = 0; x < plateSize; x++)
            {
                for (int y = 0; y < plateSize; y++)
                {
                    // 生成高度、温度和湿度数据
                    float noiseValue = noise.GetNoise2D(x, y);
                    int height = (int)((noiseValue - MinNoiseValue) / (MaxNoiseValue - MinNoiseValue) * HeightScaleFactor);
                    float temperature = BaseTemperature + TemperatureRange / plateSize * y + noiseValue * 15f;
                    float humidity = BaseHumidity + HumidityHeightFactor * height + 20f / plateSize * y + noiseValue * 10f;

                    // 根据条件确定材质类型
                    EnumMaterial material = DetermineMaterialByConditions(height, temperature, humidity);

                    // 填充Z轴方向的数据
                    FillZAxisData(gameMap, x, y, height, material);

                    // 设置环境地图数据
                    environmentMap[x, y] = new Block()
                    {
                        position = new Vector3I(x, y, height),
                        material = material,
                        temperature = temperature,
                        humidity = humidity
                    };
                }
            }

            return gameMap;
        }
        catch (System.Exception)
        {
            // 异常处理
            return null;
        }
    }
    
    // 填充Z轴方向的数据
    private void FillZAxisData(Block[,,] gameMap, int x, int y, int height, EnumMaterial material)
    {
        // 参数验证
        if (gameMap == null) return;
        
        // 对Z轴使用并行处理，因为各层之间没有依赖关系
        Parallel.For(0, MaxZLevel, z =>
        {
            // 防止数组越界
            if (x >= gameMap.GetLength(0) || y >= gameMap.GetLength(1) || z >= gameMap.GetLength(2))
                return;
                
            // 根据高度确定不同层级的材质
            if (z > height)
            {
                // 空中区域
                gameMap[x, y, z] = new Block() { material = EnumMaterial.Air };
            }
            else if (z > height - SurfaceLayerDepth)
            {
                // 地表层
                gameMap[x, y, z] = new Block() { material = material };
            }
            else
            {
                // 深层石头
                gameMap[x, y, z] = new Block() { material = EnumMaterial.Stone };
            }

            gameMap[x, y, z].position = new Vector3I(x, y, z);
        });
    }
    
    // 根据高度、温度和湿度条件确定材质类型
    private EnumMaterial DetermineMaterialByConditions(int height, float temperature, float humidity)
    {
        // 海平面以下区域
        if (height <= SeaLevel)
        {
            if (humidity >= HighHumidityThreshold)
            {
                if (temperature <= IceTempThresholdLow && temperature >= IceTempThresholdHigh)
                {
                    return EnumMaterial.Ice;
                }
                else
                {
                    return EnumMaterial.Water;
                }
            }
            else
            {
                if (temperature <= FreezingTempThreshold)
                {
                    return EnumMaterial.Ice;
                }
                else
                {
                    return EnumMaterial.Stone;
                }
            }
        }
        // 低地 (开始有陆地往上了)
        else if (height <= LowLandUpperBound)
        {
            if (humidity <= LowlandLowHumidityThreshold)
            {
                if (temperature <= FreezingTempThreshold)
                {
                    return EnumMaterial.Stone;
                }
                else
                {
                    return EnumMaterial.Sand;
                }
            }
            else if (humidity <= LowlandMediumHumidityThreshold)
            {
                if (temperature <= ColdTempThreshold)
                {
                    return EnumMaterial.Stone;
                }
                else if (temperature <= CoolTempThreshold)
                {
                    return EnumMaterial.Soil;
                }
                else if (temperature <= WarmTempThreshold)
                {
                    return EnumMaterial.Grass;
                }
                else
                {
                    return EnumMaterial.Sand;
                }
            }
            else
            {
                if (temperature <= FreezingTempThreshold)
                {
                    return EnumMaterial.Snow;
                }
                else if (temperature <= WarmTempThreshold)
                {
                    return EnumMaterial.Grass;
                }
                else
                {
                    return EnumMaterial.Sand;
                }
            }
        }
        // 中地
        else if (height <= MidLandUpperBound)
        {
            if (humidity <= HighlandLowHumidityThreshold)
            {
                return EnumMaterial.Stone;
            }
            else
            {
                return EnumMaterial.Snow;
            }
        }
        // 高地
        else if (height <= HighLandUpperBound)
        {
            return EnumMaterial.Snow;
        }

        return EnumMaterial.Air;
    }
    
    // 打印地图高度统计信息
    private void PrintHeightDebugInfo(Block[,] environmentMap)
    {
        // 参数验证
        if (environmentMap == null)
        {
            return;
        }
        
        // 计算地图中的最小和最大高度值
        int minZ = int.MaxValue;
        int maxZ = int.MinValue;
        
        for (int i = 0; i < environmentMap.GetLength(0); i++)
        {
            for (int j = 0; j < environmentMap.GetLength(1); j++)
            {
                // 检查数组边界和元素有效性
                if (i >= environmentMap.GetLength(0) || j >= environmentMap.GetLength(1) || environmentMap[i, j] == null)
                    continue;
                    
                int z = environmentMap[i, j].position.Z;
                if (z < minZ) minZ = z;
                if (z > maxZ) maxZ = z;
            }
        }
    }
}