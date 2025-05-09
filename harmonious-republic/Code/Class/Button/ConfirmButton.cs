using Godot;
using System;

public partial class ConfirmButton : ChangeSceneButton, IChangeScene
{
    public override void ChangeScene(PackedScene scene)
    {
        OutPutInformation();
        GetTree().ChangeSceneToPacked(scene);
    }

    public void OutPutInformation()
    {
        Data data = GetNode<Data>("/root/Data");
        
        // TODO: 这里需要输出信息图和环境图给data
        
    }
}
