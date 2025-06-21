using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class LoadingGame : Node2D
{
    public override void _Ready()
    {
<<<<<<< HEAD
=======
        GD.Print(1);

        // 加载一些我也不知道是什么的东西
        Data data = GetNode<Data>("/root/Data");

        GD.Print(data.gameMap[0,0,0].material);
>>>>>>> 8ca785b4bd2b34da06a7055c415504461de41885
    }
}