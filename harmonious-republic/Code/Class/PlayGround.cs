using Godot;
using HarmoniousRepublic;

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

		// 然后调用地图更新方法
		for (int z = 0; z < Constants.LayerCount; z++)
		{
			// TODO: 把shader的路径替换成到时候要用的路径
			TileMapLayer level = new TileMapLayer() { Name = $"{z}", TileSet = tileSet };
			ShaderMaterial shaderMaterial = new ShaderMaterial();
			shaderMaterial.Shader = GD.Load<Shader>("res://Code/Shader/GameMapShader.gdshader");
			level.Material = shaderMaterial;
			Map.AddChild(level);
		}

		GetNode<MapController>("MapController").UpdateMap(50);
	}

	public override void _Process(double delta)
	{

	}
}