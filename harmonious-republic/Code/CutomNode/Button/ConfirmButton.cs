using Godot;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

[GlobalClass]
public partial class ConfirmButton : ChangeSceneButton, IChangeScene
{
    public async override void ChangeScene(PackedScene scene)
    {
        await Task.Run(() => CallDeferred(nameof(ChangeSceneAnimAsync), scene));
    }

    public async void ChangeSceneAnimAsync(PackedScene scene)
    {
        Vector2 targetPosition = GetNode<Sprite2D>("../../SubViewportContainer/SubViewport/ScalePoint").GlobalPosition;
        Vector2 targetZoom = new Vector2(10f, 10f);
        Vector2 viewportPosition = GetNode<SubViewportContainer>("../../SubViewportContainer").Position;
        PlateGenerateMenuCamera camera = GetNode<PlateGenerateMenuCamera>("../../../PlateGenerateMenuCamera");

        float step = Mathf.Lerp(1f, 10f, 1f);

        camera.targetPosition = targetPosition + viewportPosition;
        camera.targetZoom = targetZoom;
        camera.zoomStep = new Vector2(step, step);
        camera.isReady = false;

        while (true)
        {
            if (camera.isReady)
            {
                GetTree().ChangeSceneToPacked(scene);
                break;
            }
            await Task.Delay(10);
        }
    }
}
