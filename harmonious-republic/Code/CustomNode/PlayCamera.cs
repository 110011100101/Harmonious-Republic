using System;
using Godot;
using HarmoniousRepublic.Code.StaticClass;

// 注意: Camera有独立的设置Position的方法,用于监控位置的变更
namespace HarmoniousRepublic.Code.CustomNode;

public partial class PlayCamera : Camera2D
{
    private float cameraLevel;
    private bool isTargetLevelChanged;
    private float targetLevel;

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
        // TODO: 这里死循环了, 下面这个条件和上面的有错误
        else if (Math.Abs(cameraLevel - targetLevel) > 0.01f)
        {
            isTargetLevelChanged = true;
            cameraLevel = Mathf.Lerp(cameraLevel, targetLevel, Constants.CameraSwitchLevelLerp);

            GetNode<MapController>("../MapController").UpdateMap(cameraLevel);
            GetNode<Label>("Label").Text = $"Level:{cameraLevel}\nTargetLevel:{targetLevel}\nStep:{Mathf.Lerp(cameraLevel, targetLevel, 0.1f)}";
        }
    }

    public override void _Input(InputEvent @event)
    {
        switch (@event)
        {
            case InputEventMouseButton mouseButtonEvent:
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
                        // 中键释放逻辑
                        break;

                    case MouseButton.WheelUp:
                        if (mouseButtonEvent.Pressed)
                        {
                            if (targetLevel > 0f)
                            {
                                targetLevel -= 1f;
                            }
                        }
                        break;

                    case MouseButton.WheelDown:
                        if (mouseButtonEvent.Pressed)
                        {
                            if (targetLevel < 100f)
                            {
                                targetLevel += 1f;
                            }
                        }
                        break;
                }
                break;

            // 鼠标移动事件
            case InputEventMouseMotion mouseMotionEvent:
            {
                if (Input.IsMouseButtonPressed(MouseButton.Middle))
                {
                    Vector2 mouseDelta = mouseMotionEvent.Relative / Zoom;
                    Translate(new Vector2(-mouseDelta.X, -mouseDelta.Y));
                    GetNode<MapController>("../MapController").UpdateMap(cameraLevel);
                }
                break;
            }

            case InputEventKey keyEvent:
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
                break;
        }
    }
}