using Godot;
using HarmoniousRepublic.Code.InterFace;

namespace HarmoniousRepublic.Code.CustomNode;

[GlobalClass]
public partial class ChangeSceneButton : Button, IChangeScene
{
    [Export] private string scenePath;
        
    public virtual void ChangeScene(PackedScene scene)
    {
        GetTree().ChangeSceneToPacked(scene);
    }

    public override void _Pressed()
    {
        ChangeScene(GD.Load<PackedScene>(scenePath));
    }
}