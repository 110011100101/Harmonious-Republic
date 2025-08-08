using Godot;

// 注意: Camera有独立的设置Position的方法,用于监控位置的变更
public partial class PlayCamera : Camera2D
{
	public float level = 50;
	public float targetLevel = 50;
	bool isTargetLevelChanged = false;

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		// 插值设置层数
		if (isTargetLevelChanged && Mathf.Abs(targetLevel - level) <= 0.01f)
		{
			level = targetLevel;
			GetNode<MapController>("../MapController").UpdateMap(targetLevel);
			isTargetLevelChanged = false;
		}
		else if (level != targetLevel)
		{
			isTargetLevelChanged = true;
			level = Mathf.Lerp(level, targetLevel, 0.1f);
			// 调用那个函数
			GetNode<MapController>("../MapController").UpdateMap(level);
		}
		GetNode<Label>("Label").Text = "Level:" + level;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButtonEvent)
		{
			// 处理鼠标按键事件
			switch (mouseButtonEvent.ButtonIndex)
			{
				case MouseButton.Left:
					if (mouseButtonEvent.Pressed)
					{
						// 左键按下逻辑
					}
					break;

				case MouseButton.Right:
					if (mouseButtonEvent.Pressed)
					{
						// 右键按下逻辑
					}
					break;

				case MouseButton.Middle:
					if (mouseButtonEvent.Pressed)
					{
						// 中键按下逻辑
					}
					else
					{
						// 中键释放逻辑
					}
					break;

				case MouseButton.WheelUp:
					if (mouseButtonEvent.Pressed)
					{
						if (targetLevel < 100)
						{
							targetLevel += 1;
						}
					}
					break;

				case MouseButton.WheelDown:
					if (mouseButtonEvent.Pressed)
					{
						if (targetLevel > 0)
						{
							targetLevel -= 1;
						}
					}
					break;
			}
		}
		// 鼠标移动事件
		else if (@event is InputEventMouseMotion mouseMotionEvent)
		{
			if (Input.IsMouseButtonPressed(MouseButton.Middle))
			{
				var mouseDelta = mouseMotionEvent.Relative / Zoom;
				Translate(new Vector2(-mouseDelta.X, -mouseDelta.Y));
				GetNode<MapController>("../MapController").UpdateMap(level);
			}
		}

		if (@event is InputEventKey keyEvent)
		{
			switch (keyEvent.Keycode)
			{
				case Key.Up:
					if (keyEvent.Pressed)
					{
						Zoom += new Vector2(0.1f, 0.1f);
						GetNode<MapController>("../MapController").UpdateMap(level);
					}
					break;

				case Key.Down:
					if (keyEvent.Pressed)
					{
						Zoom -= new Vector2(0.1f, 0.1f);
						GetNode<MapController>("../MapController").UpdateMap(level);
					}
					break;
			}
		}
	}
}
