using Godot;
using System;
using System.Drawing;

public partial class TouchLocatingPad : Area2D
{
	[Export] public string PointTexturePath { get; set; }
	private bool isTouching = false;
	private SubViewport subViewport;
	private Vector2 mapPointTargetPosition;
	private Vector2 pointTargetPosition;

	public override void _Ready()
	{
		subViewport = GetNode<SubViewport>("../SubViewportContainer/SubViewport");
	}

	public override void _Process(double delta)
	{
		if (!isTouching) return;

		UpdateSpritePosition(subViewport.GetNode<Sprite2D>("LightPoint"), mapPointTargetPosition);
		UpdateSpritePosition(GetNode<Sprite2D>("LightPoint"), pointTargetPosition);
	}

	private void UpdateSpritePosition(Sprite2D sprite, Vector2 targetPosition)
	{
		if (sprite.Position != targetPosition)
		{
			sprite.Position = sprite.Position.Lerp(targetPosition, 0.1f);
		}
	}

	public override void _MouseEnter()
	{
		AddLightPointSprite();
		isTouching = true;
	}

	public override void _MouseExit()
	{
		RemoveLightPointSprite();
		isTouching = false;
	}

	private void AddLightPointSprite()
	{
		var texture = GD.Load<Texture2D>(PointTexturePath);
		AddChild(new Sprite2D() { Name = "LightPoint", Texture = texture });
		subViewport.AddChild(new Sprite2D() { Name = "LightPoint", Texture = texture });
	}

	private void RemoveLightPointSprite()
	{
		RemoveChild(GetNode<Sprite2D>("LightPoint"));
		subViewport.RemoveChild(subViewport.GetNode<Sprite2D>("LightPoint"));
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion eventMouseMotion && isTouching)
		{
			UpdateTargetPositions(eventMouseMotion);
		}

		if (@event is InputEventMouseButton eventMouseButton && isTouching)
		{
			HandleMouseButtonEvent(eventMouseButton);
		}
	}

	private void UpdateTargetPositions(InputEventMouseMotion eventMouseMotion)
	{
		RectangleShape2D shape = (RectangleShape2D)GetNode<CollisionShape2D>("CollisionShape2D").Shape;
		pointTargetPosition = GetLocalMousePosition();
		mapPointTargetPosition = pointTargetPosition * (subViewport.Size / shape.Size);
	}

	private void HandleMouseButtonEvent(InputEventMouseButton eventMouseButton)
	{
		if (eventMouseButton.ButtonIndex == MouseButton.Left)
		{
			AddOrUpdateScalePoint();
		}
		else if (eventMouseButton.ButtonIndex == MouseButton.Right)
		{
			RemoveScalePoint();
		}
	}

	private void AddOrUpdateScalePoint()
	{
		RectangleShape2D shape = (RectangleShape2D)GetNode<CollisionShape2D>("CollisionShape2D").Shape;
		Vector2 offset = GetLocalMousePosition() * (subViewport.Size / shape.Size);

		// 触控板缩放点处理逻辑
		if (HasNode("ScalePoint"))
		{
			GetNode<Sprite2D>("ScalePoint").Position = GetLocalMousePosition();
		}
		else
		{
			AddChild(new Sprite2D()
			{
				Name = "ScalePoint",
				Texture = GD.Load<Texture2D>(PointTexturePath),
				Position = GetLocalMousePosition()
			});
		}

		// 映射缩放点处理逻辑
		if (subViewport.HasNode("ScalePoint"))
		{
			subViewport.GetNode<Sprite2D>("ScalePoint").Position = offset;
		}
		else
		{
			subViewport.AddChild(new Sprite2D()
			{
				Name = "ScalePoint",
				Texture = GD.Load<Texture2D>(PointTexturePath),
				Position = offset
			});
		}

		// 更新缩放基点
		subViewport.GetNode<Node2D>("MapGenerator").Position = offset;
		subViewport.GetNode<Node2D>("MapGenerator").GetChild<TileMapLayer>(0).Position = -offset;
	}

	private void RemoveScalePoint()
	{
		if (HasNode("ScalePoint"))
		{
			RemoveChild(GetNode<Sprite2D>("ScalePoint"));
		}

		if (subViewport.HasNode("ScalePoint"))
		{
			subViewport.RemoveChild(subViewport.GetNode<Sprite2D>("ScalePoint"));
		}

		subViewport.GetNode<Node2D>("MapGenerator").Position = new Vector2(0, 0);
		subViewport.GetNode<Node2D>("MapGenerator").GetChild<TileMapLayer>(0).Position = new Vector2(0, 0);
	}
}