using Godot;

namespace HarmoniousRepublic.Code.CustomNode;

public partial class PlateGenerateMenuCamera : Camera2D
{
    public bool isReady = true;
    public Vector2 positionStep;
    public Vector2 targetPosition;
    public Vector2 targetZoom;
    public Vector2 zoomStep;

    public override void _Ready()
    {
        targetPosition = Position;
        targetZoom = Zoom;
    }

    public override void _Process(double delta)
    {
        if (Position != targetPosition)
        {
            if (isReady)
            {
                isReady = false;
            }

            Position = Position.Lerp(targetPosition, 0.01f);
        }
        if (Zoom != targetZoom)
        {
            if (isReady)
            {
                isReady = false;
            }

            Zoom = Zoom.Lerp(targetZoom, 0.01f);
        }
        if (Position.DistanceTo(targetPosition) <= 10f && Zoom >= targetZoom - new Vector2(0.1f, 0.1f) && !isReady)
        {
            isReady = true;
        }
    }
}