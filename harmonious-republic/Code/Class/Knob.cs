using Godot;

public partial class Knob : HSlider
{
	private float angleRange = 180; // 默认是从左边顺时针转到右边的角度
	private bool isDraging = false;
	
	public void EnterDragState()
	{
		GD.Print("EnterDragState");
		isDraging = true;
	}

	public void ExitDragState(bool isValueChanged)
	{
		GD.Print("ExitDragState");
		isDraging = false;
	}
	
	public override void _Input(InputEvent @event) {
		// 当按下了左键
		if (Input.IsMouseButtonPressed(MouseButton.Left))
		{
			// 并且鼠标移动时
			if (@event is InputEventMouseMotion eventMouseMotion)
			{
				// 处理拖拽事件
				if (isDraging)
				{
					// 计算转动的角度
					float angle = eventMouseMotion.Relative.X > 0 ? 3 : -3;
					// 旋转自身
					if (RotationDegrees + angle < angleRange && RotationDegrees + angle > 0)
					{
						RotationDegrees += angle;
					}
					else if (RotationDegrees + angle >= angleRange)
					{
						RotationDegrees = angleRange;
					}
					else if (RotationDegrees + angle <= 0)
					{
						RotationDegrees = 0;
					}
					
					Value = (RotationDegrees / angleRange) * 100;
				}
			}
		}
	}
}