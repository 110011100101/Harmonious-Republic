using System.Linq;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;
using Nocturne.Core.Class;

using Environment = System.Environment;

/// <summary>
/// 接收一个图，放大并返回
/// </summary>
public class RefineMapTo3DUnit : Unit<Dictionary<Vector2I, Vector3>, Dictionary<Vector3I, Vector3>>
{
    int subdivisionFactor;
    int plateSize;

    public RefineMapTo3DUnit(int plateSize, int subdivisionFactor)
    {
        this.plateSize = plateSize;
        this.subdivisionFactor = subdivisionFactor;
    }

    public override Dictionary<Vector3I, Vector3> Execute(Dictionary<Vector2I, Vector3> informationMaps)
    {
        // ?: 用什么格式输出
        // ?: 垂直分层的逻辑是什么
        
        
        return null;
    }
}