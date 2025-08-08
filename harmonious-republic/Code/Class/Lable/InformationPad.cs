using Godot;

public partial class InformationPad : RichTextLabel
{
	[Export] public string plateName {get; set;}
    [Export] public int plateSize {get; set;}
    [Export] public Vector2I cellPosition {get; set;}
    [Export] public int cellHeight {get; set;}
	[Export] public float cellHumidity {get; set;}
	[Export] public float cellTemperature {get; set;}
    [Export] public EnumMaterial cellMaterial {get; set;}
    public Block[,] environmentMap;
	
    // [Export] public string ;

	public override void _Ready()
	{
		VisibleRatio = 0;
		plateName = GetNode<Data>("/root/Data").plateName;
		plateSize = GetNode<Data>("/root/Data").plateSize;
		environmentMap = GetNode<Data>("/root/Data").environmentMap;
		ChangeText();
	}

    public override void _Process(double delta)
    {
		if (VisibleRatio < 1)
		{
			VisibleRatio += Mathf.Lerp(0f, 1f, 1f * (float)delta);
		}
    }

    public void ChangeText()
	{
		VisibleRatio = 0;
		Clear();
		AddText($"板块名称: {plateName}");
		AddText($"\n板块大小: {plateSize}");
		AddText($"\n缩放比: {GetNode<Node2D>("../SubViewportContainer/SubViewport/MapGenerator").Scale.X}");
		AddText($"\n\ncell坐标: {cellPosition}");
		
		// 检查environmentMap是否已初始化
		if (environmentMap != null)
		{
			// 确保访问的数组索引在有效范围内
			if (cellPosition.X >= 0 && cellPosition.X < environmentMap.GetLength(0) &&
				cellPosition.Y >= 0 && cellPosition.Y < environmentMap.GetLength(1))
			{
				AddText($"\ncell高度: {environmentMap[cellPosition.X, cellPosition.Y].position.Z}");
				AddText($"\ncell温度: {environmentMap[cellPosition.X, cellPosition.Y].temperature}");
				AddText($"\ncell湿度: {environmentMap[cellPosition.X, cellPosition.Y].humidity}");
				AddText($"\ncell环境: {environmentMap[cellPosition.X, cellPosition.Y].material}");
			}
			else
			{
				AddText($"\ncell高度: 无法获取 (索引越界)");
				AddText($"\ncell温度: 无法获取 (索引越界)");
				AddText($"\ncell湿度: 无法获取 (索引越界)");
				AddText($"\ncell环境: 无法获取 (索引越界)");
			}
		}
		else
		{
			AddText($"\ncell高度: 无法获取 (地图未生成)");
			AddText($"\ncell温度: 无法获取 (地图未生成)");
			AddText($"\ncell湿度: 无法获取 (地图未生成)");
			AddText($"\ncell环境: 无法获取 (地图未生成)");
		}
	}

	public void PlaySoundEffect()
	{
		GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
	}
}