using Godot;

public partial class Test : Node2D
{

    public override void _Ready()
    {

    }

    public override void _Process(double delta)
    {

    }

    public void CameraPositionChanged()
    {
        GD.Print("CameraPositionChanged");
    }

    public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotionEvent)
		{
			if (Input.IsMouseButtonPressed(MouseButton.Middle))
			{
				var mouseDelta = mouseMotionEvent.Relative / GetNode<Camera2D>("Camera2D").Zoom;
				GetNode<Camera2D>("Camera2D").Translate(new Vector2(-mouseDelta.X, -mouseDelta.Y));
			}
		}

		if (@event is InputEventKey keyEvent)
		{
			switch (keyEvent.Keycode)
			{
				case Key.Up:
					if (keyEvent.Pressed)
					{
						GetNode<Camera2D>("Camera2D").Zoom += new Vector2(0.1f, 0.1f);
					}
					break;

				case Key.Down:
					if (keyEvent.Pressed)
					{
						GetNode<Camera2D>("Camera2D").Zoom -= new Vector2(0.1f, 0.1f);
					}
					break;
			} 
		}
	}
}
