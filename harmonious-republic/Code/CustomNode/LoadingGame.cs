using Godot;

namespace HarmoniousRepublic.Code.CustomNode;

public partial class LoadingGame : Node2D
{
    public override void _Ready()
    {
        GD.Print(1);

        // 加载一些我也不知道是什么的东西
        Data data = GetNode<Data>("/root/Data");

        GD.Print(data.gameMap[0, 0, 0].material);
    }
}