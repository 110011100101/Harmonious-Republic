using Godot;

public partial class PlayGround : Node2D
{
	private Node2D Map;
	[Export] private TileSet tileSet;
	Data data;

	public override void _Ready()
	{
		// 生成一个空地图
		Map = GetNode<Node2D>("MapController");
		data = GetNode<Data>("/root/Data");

		for (int z = 0; z < 100; z++)
		{
			// 把shader的路径替换成到时候要用的路径
			TileMapLayer level = new TileMapLayer() { Name = $"{z}", TileSet = tileSet};
			ShaderMaterial shaderMaterial = new ShaderMaterial();
			shaderMaterial.Shader = GD.Load<Shader>("res://test/test_tilemaplayer.gdshader");
			level.Material = shaderMaterial;
			Map.AddChild(level);

			for (int x = 0; x < 100; x++)
			{
				for (int y = 0; y < 100; y++)
				{
					GetNode<TileMapLayer>($"MapController/{z}").SetCell(new Vector2I(x, y), (int)data.gameMap[x, y, z].material, new Vector2I(4, 0), 0);
				}
			}
		}
		GetNode<MapController>("MapController").UpdateMap(50);
	}

	public override void _Process(double delta)
	{

	}
}
