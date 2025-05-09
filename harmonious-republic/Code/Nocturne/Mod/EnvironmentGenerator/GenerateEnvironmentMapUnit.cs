using System.Linq;
using Godot;
using Godot.Collections;
using Nocturne.Core.Class;

public class GenerateEnvironmentMapUnit : Unit<Dictionary<Vector2I, Vector3>, Dictionary<Vector2I, EnumMaterial>>
{
    int plateSize;

    public GenerateEnvironmentMapUnit(int plateSize)
    {
        this.plateSize = plateSize;
    }

    public override Dictionary<Vector2I, EnumMaterial> Execute(Dictionary<Vector2I, Vector3> informationMaps)
    {
        Dictionary<Vector2I, EnumMaterial> enviromentDic = new Dictionary<Vector2I, EnumMaterial>();

        for (int x = 0; x < plateSize; x++)
        {
            for (int y = 0; y < plateSize; y++)
            {
                Vector2I block = new Vector2I(x, y);
                EnumMaterial material = EnumMaterial.Air; // Default material
                float height = informationMaps[block].X;
                float humidity = informationMaps[block].Y;
                float temperature = informationMaps[block].Z;

                // 这个逻辑是基于EnumMaterial的, 除了C#约定你必须在新增材质的时候先引入枚举, 其他方面约等于完全独立
                // 你可以自由搭配前置的材质包mod(这个材质包是真真正正的材质包,不是纹理材质)
                if (height <= 45) // 海平面以下
                {
                    if (humidity >= 30)
                    {
                        if (temperature <= -15 && temperature >= -35)
                        {
                            material = EnumMaterial.Ice;
                        }
                        else
                        {
                            material = EnumMaterial.Water;
                        }
                    }
                    else
                    {
                        if (temperature <= 0)
                        {
                            material = EnumMaterial.Ice;
                        }
                        else
                        {
                            material = EnumMaterial.Stone;
                        }
                    }
                }
                else if (height <= 80) // 开始有陆地往上了
                {
                    if (humidity <= 20)
                    {
                        if (temperature <= 0)
                        {
                            material = EnumMaterial.Stone;
                        }
                        else
                        {
                            material = EnumMaterial.Sand;
                        }
                    }
                    else if (humidity <= 65)
                    {
                        if (temperature <= -30)
                        {
                            material = EnumMaterial.Stone;
                        }
                        else if (temperature <= -10)
                        {
                            material = EnumMaterial.Soil;
                        }
                        else if (temperature <= 30)
                        {
                            material = EnumMaterial.Grass;
                        }
                        else
                        {
                            material = EnumMaterial.Sand;
                        }
                    }
                    else
                    {
                        if (temperature <= 0)
                        {
                            material = EnumMaterial.Snow;
                        }
                        else if (temperature <= 30)
                        {
                            material = EnumMaterial.Grass;
                        }
                        else
                        {
                            material = EnumMaterial.Sand;
                        }
                    }

                }
                else if (height <= 90)
                {
                    if (humidity <= 40)
                    {
                        material = EnumMaterial.Stone;
                    }
                    else
                    {
                        material = EnumMaterial.Snow;
                    }
                }
                else if (height <= 100)
                {
                    material  = EnumMaterial.Snow;
                }

                enviromentDic.Add(block, material);
            }
        }

        return enviromentDic;
    }
}