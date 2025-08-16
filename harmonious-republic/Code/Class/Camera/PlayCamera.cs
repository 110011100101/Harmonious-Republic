using System.Reflection.Metadata;
using Godot;
using HarmoniousRepublic;

// 注意: Camera有独立的设置Position的方法,用于监控位置的变更
public partial class PlayCamera : Camera2D
{
	public float cameraLevel;
	public float targetLevel;
	bool isTargetLevelChanged = false;

	public override void _Ready()
	{
		cameraLevel = 50f;
		targetLevel = 50f;
	}

	public override void _Process(double delta)
	{
		// 插值设置层数
		if (isTargetLevelChanged && Mathf.Abs(targetLevel - cameraLevel) <= 0.01f)
		{
			isTargetLevelChanged = false;
			cameraLevel = targetLevel;

			GetNode<MapController>("../MapController").UpdateMap(cameraLevel);
			GetNode<Label>("Label").Text = $"Level:{cameraLevel}";
		}
		else if (cameraLevel != targetLevel)
		{
			isTargetLevelChanged = true;
			cameraLevel = Mathf.Lerp(cameraLevel, targetLevel, Constants.cameraSwitchLevelLerp);

			GetNode<MapController>("../MapController").UpdateMap(cameraLevel);
			GetNode<Label>("Label").Text = $"Level:{cameraLevel}\nTargetLevel:{targetLevel}\nStep:{Mathf.Lerp(cameraLevel, targetLevel, 0.1f)}";
		}
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
						if (targetLevel > 0)
						{
							targetLevel -= 1;
						}
					}
					break;

				case MouseButton.WheelDown:
					if (mouseButtonEvent.Pressed)
					{
						if (targetLevel < 100)
						{
							targetLevel += 1;
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
				GetNode<MapController>("../MapController").UpdateMap(cameraLevel);
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
						GetNode<MapController>("../MapController").UpdateMap(cameraLevel);
					}
					break;

				case Key.Down:
					if (keyEvent.Pressed)
					{
						Zoom -= new Vector2(0.1f, 0.1f);
						GetNode<MapController>("../MapController").UpdateMap(cameraLevel);
					}
					break;
			}
		}
	}
}
