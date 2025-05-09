using System.Linq;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;
using Nocturne.Core.Class;

using Environment = System.Environment;

/// <summary>
/// 接收一个图，放大并返回
/// </summary>
public class RefineMapUnit : Unit<Dictionary<Vector2I, Vector3>, Dictionary<Vector2I, Vector3>>
{
    int subdivisionFactor;
    int plateSize;

    public RefineMapUnit(int plateSize, int subdivisionFactor)
    {
        this.plateSize = plateSize;
        this.subdivisionFactor = subdivisionFactor;
    }

    public override Dictionary<Vector2I, Vector3> Execute(Dictionary<Vector2I, Vector3> informationMaps)
    {
        FastNoiseLite noise = new NocturneNoise() { Frequency = 0.00068f };

        int detailedPlateSize = plateSize * subdivisionFactor;

        Dictionary<Vector2I, Vector3> detailedInformationMaps = new();

        // 生成高颗粒度高度图
        // 针对每一个原来的像素执行
        // 优化生成算法,让高度更细致的变化
        for (int x = 0; x < detailedPlateSize; x++)
        {
            for (int y = 0; y < detailedPlateSize; y++)
            {
                Vector2I block = new Vector2I(x, y);

                Vector2I orginalblock = new Vector2I(x / subdivisionFactor, y / subdivisionFactor);
                Vector3 originalInformation = informationMaps[orginalblock];

                // FIXME: 这里的扩大算法不是很对
                // 基础格子高度 + 每层随机高度
                // float height = subdivisionFactor == 1 ? originalInformation.X : originalInformation.X * detailedLevelRange + (noiseValue + 1.0f) * detailedLevelRange;     // [-1,1] => [0.00, detailedLevelRange]
            
                float X = originalInformation.X;
                float Y = originalInformation.Y;
                float Z = originalInformation.Z;
                Vector3 information = new Vector3(X, Y, Z);

                detailedInformationMaps[block] = information;
            }
        }

        return detailedInformationMaps;
    }
}