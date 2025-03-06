using Godot;
using System;
using System.Drawing;

public partial class TouchLocatingPad : Area2D
{
	[Export] public string lightPointPath;
	private bool isTouching = false;
	private SubViewport subViewport;
	private Vector2 mapPointAimPosition;
	private Vector2 pointAimPosition;

	public override void _Ready() {
		subViewport = GetNode<SubViewport>("../SubViewportContainer/SubViewport");
	}

	public override void _Process(double delta) {
		if (isTouching)
		{
			// 插值向目标点移动
			if (mapPointAimPosition != subViewport.GetNode<Sprite2D>("LightPoint").Position)
			{
				subViewport.GetNode<Sprite2D>("LightPoint").Position = subViewport.GetNode<Sprite2D>("LightPoint").Position.Lerp(mapPointAimPosition, 0.1f);
			}
			
			// 插值向目标点移动
			if (pointAimPosition != GetNode<Sprite2D>("LightPoint").Position)
			{
				GetNode<Sprite2D>("LightPoint").Position = GetNode<Sprite2D>("LightPoint").Position.Lerp(pointAimPosition, 0.1f);
			}
		}
	}
	
    public override void _MouseEnter()
    {
		AddChild(new Sprite2D() { Name = "LightPoint", Texture = GD.Load<Texture2D>(lightPointPath) });
		subViewport.AddChild(new Sprite2D() { Name = "LightPoint", Texture = GD.Load<Texture2D>(lightPointPath) });
		isTouching = true;
    }

	public override void _MouseExit()
	{
		RemoveChild(GetNode<Sprite2D>("LightPoint"));
		subViewport.RemoveChild(subViewport.GetNode<Sprite2D>("LightPoint"));
		isTouching = false;
	}

	public override void _Input(InputEvent @event) {
		if (@event is InputEventMouseMotion eventMouseMotion)
		{
			if (isTouching)
			{
				RectangleShape2D shape = (RectangleShape2D)GetNode<CollisionShape2D>("CollisionShape2D").Shape;

				pointAimPosition = GetLocalMousePosition();
				mapPointAimPosition = GetLocalMousePosition() * (subViewport.Size / shape.Size);
			}
		}
	}
}
