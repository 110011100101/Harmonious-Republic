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
					// 更新Value的值
					Value += eventMouseMotion.Relative.X > 0 ? 1 : -1;
				}
			}
		}
	}

    public override void _ValueChanged(double newValue)
    {
        // 根据Value的值调整旋转角度
        RotationDegrees = (float)(newValue / 100 * angleRange);
    }
}