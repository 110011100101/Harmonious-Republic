using Godot;

namespace HarmoniousRepublic.Code.CustomNode;

public partial class TouchLocatingPad : Area2D
{
    private bool isTouching;
    private Vector2 mapPointTargetPosition;
    private Vector2 pointTargetPosition;
    private SubViewport subViewport;
    [Export] private string subViewportPath;

    [Export]
    public string pointTexturePath { get; set; }

    public override void _Ready()
    {
        subViewport = GetNode<SubViewport>(subViewportPath);
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
        Texture2D texture = GD.Load<Texture2D>(pointTexturePath);
        AddChild(new Sprite2D { Name = "LightPoint", Texture = texture });
        subViewport.AddChild(new Sprite2D { Name = "LightPoint", Texture = texture });
    }

    private void RemoveLightPointSprite()
    {
        RemoveChild(GetNode<Sprite2D>("LightPoint"));
        subViewport.RemoveChild(subViewport.GetNode<Sprite2D>("LightPoint"));
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion && isTouching)
        {
            UpdateTargetPositions();
        }

        if (@event is InputEventMouseButton eventMouseButton && isTouching)
        {
            HandleMouseButtonEvent(eventMouseButton);
        }
    }

    private void UpdateTargetPositions()
    {
        RectangleShape2D shape = (RectangleShape2D)GetNode<CollisionShape2D>("CollisionShape2D").Shape;
        pointTargetPosition = GetLocalMousePosition();
        mapPointTargetPosition = pointTargetPosition * (subViewport.Size / shape.Size);
    }

    private void HandleMouseButtonEvent(InputEventMouseButton eventMouseButton)
    {
        if (eventMouseButton.ButtonIndex == MouseButton.Left && eventMouseButton.Pressed)
        {
            AddOrUpdateScalePoint();
        }
        else if (eventMouseButton.ButtonIndex == MouseButton.Right && eventMouseButton.Pressed)
        {
            RemoveScalePoint();
        }
    }

    private void AddOrUpdateScalePoint()
    {
        RectangleShape2D shape = (RectangleShape2D)GetNode<CollisionShape2D>("CollisionShape2D").Shape;
        Vector2 offset = GetLocalMousePosition() * (subViewport.Size / shape.Size);

        // 触控板上的缩放点处理逻辑
        if (HasNode("ScalePoint"))
        {
            GetNode<Sprite2D>("ScalePoint").Position = GetLocalMousePosition();
        }
        else
        {
            AddChild(new Sprite2D
            {
                Name = "ScalePoint",
                Texture = GD.Load<Texture2D>(pointTexturePath),
                Position = GetLocalMousePosition()
            });
        }

        // 映射过去的缩放点处理逻辑
        if (subViewport.HasNode("ScalePoint"))
        {
            subViewport.GetNode<Sprite2D>("ScalePoint").Position = offset;
            subViewport.GetNode<Sprite2D>("MapGenerator/map/SelectRange").Position = (Vector2I)(offset / (1000f / GetNode<Data>("/root/Data").plateSize) * 64f);
        }
        else
        {
            subViewport.AddChild(new Sprite2D
            {
                Name = "ScalePoint",
                Texture = GD.Load<Texture2D>(pointTexturePath),
                Position = offset
            });

            subViewport.GetNode<Node2D>("MapGenerator/map").AddChild(new Sprite2D
            {
                Name = "SelectRange",
                Texture = GD.Load<Texture2D>("res://Assets/Texture/default/SelectRange.png"),
                Scale = new Vector2(3f, 3f),
                Position = (Vector2I)(offset / (1000f / GetNode<Data>("/root/Data").plateSize) * 64f)
            });
        }

        // 更新对应单元格信息
        Vector2I cell = (Vector2I)(offset / (1000f / GetNode<Data>("/root/Data").plateSize));

        GetNode<InformationPad>("../InformationPad").cellPosition = cell;
        GetNode<Data>("/root/Data").startLocation = cell;
        GetNode<Button>("../HBoxContainer/Confirm").Disabled = false;
        GetNode<InformationPad>("../InformationPad").ChangeText();
    }

    private void RemoveScalePoint()
    {
        if (HasNode("ScalePoint"))
        {
            RemoveChild(GetNode<Sprite2D>("ScalePoint"));

            // 重置单元格信息
            GetNode<InformationPad>("../InformationPad").cellPosition = new Vector2I(0, 0);
            GetNode<Data>("/root/Data").startLocation = new Vector2I(0, 0);
            GetNode<Button>("../HBoxContainer/Confirm").Disabled = true;
            GetNode<InformationPad>("../InformationPad").ChangeText();
        }

        if (subViewport.HasNode("ScalePoint"))
        {
            subViewport.RemoveChild(subViewport.GetNode<Sprite2D>("ScalePoint"));
            subViewport.GetNode<TileMapLayer>("MapGenerator/map").RemoveChild(subViewport.GetNode<Sprite2D>("MapGenerator/map/SelectRange"));
        }

        // 重置缩放基点
        subViewport.GetNode<Node2D>("MapGenerator").Position = new Vector2(0, 0);
        subViewport.GetNode<Node2D>("MapGenerator").GetNode<TileMapLayer>("map").Position = new Vector2(0, 0);
    }
}