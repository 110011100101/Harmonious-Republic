using Godot;
using System;

public partial class ConfirmButton : ChangeSceneButton, IChangeScene
{
    public override void ChangeScene(PackedScene scene)
    {
        OutPutMap();
        GetTree().ChangeSceneToPacked(scene);
    }

    public void OutPutMap()
    {
        Data data = GetNode<Data>("/root/Data");
        data.heightMap = GetNode<InformationPad>("../../InformationPad").heightMap;
        data.humidityMap = GetNode<InformationPad>("../../InformationPad").humidityMap;
        data.temperatureMap = GetNode<InformationPad>("../../InformationPad").temperatureMap;
        data.environmentMap = GetNode<InformationPad>("../../InformationPad").environmentMap;
    }
}
