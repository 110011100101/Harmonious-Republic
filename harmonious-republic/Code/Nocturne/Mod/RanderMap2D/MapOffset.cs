using Godot;
using Nocturne.Core.Class;

/// <summary>
/// 接受一个三维向量(摄像机坐标X, 摄像机坐标Y, 缩放值的其中一个值), 通常缩放值 xy 是绑定的
/// 返回一个新的偏移坐标
/// </summary>
public partial class MapOffset : Unit<Vector3, Vector2>
{
    int textureSize;

    MapOffset(int textureSize) : base()
    {
        this.textureSize = textureSize;
    }

    public override Vector2 Execute(Vector3 input)
    {
        return new Vector2(textureSize * input.Z * input.X, textureSize * input.Z * input.Y);
    }
}