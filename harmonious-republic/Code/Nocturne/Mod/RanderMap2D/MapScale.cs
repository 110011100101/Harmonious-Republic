using Godot;
using Nocturne.Core.Class;

/// <summary>
/// 接受一个二维向量(基准层z, 自身层z)
/// 返回新的缩放值
/// </summary>
public partial class MapScale : Unit<Vector2, Vector2>
{
    public override Vector2 Execute(Vector2 input)
    {
        // 拆分向量
        float newScale;
        float baseLayer = input.X;
        float selfLayer = input.Y;
        float scaleFactor = 1.25f;

        // 经过一系列计算得出缩放值
        // 算法: 1.25的层差次幂
        newScale = (float)Mathf.Pow(scaleFactor, baseLayer - selfLayer);

        return new Vector2(newScale, selfLayer);
    }
}
