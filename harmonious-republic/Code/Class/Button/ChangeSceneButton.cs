using Godot;
using System;

public partial class ChangeSceneButton : Button, IChangeScene
{
    [Export] private string scenePath;
	
    public override void _Pressed()
    {
        ChangeScene(GD.Load<PackedScene>(scenePath));
    }

    public virtual void ChangeScene(PackedScene scene)
	{
		GetTree().ChangeSceneToPacked(scene);
	}
}
