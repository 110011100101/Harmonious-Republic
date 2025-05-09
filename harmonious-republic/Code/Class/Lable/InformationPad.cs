using Godot;
using RoseIsland.CustomClass;
using System;
using System.Threading;

public partial class InformationPad : RichTextLabel
{
	[Export] public string plateName {get; set;}
    [Export] public int plateSize {get; set;}
    [Export] public Vector2I cellPosition {get; set;}
    [Export] public int cellHeight {get; set;}
	[Export] public float cellHumidity {get; set;}
	[Export] public float cellTemperature {get; set;}
    [Export] public EnumMaterial cellMaterial {get; set;}
    // [Export] public string ;

	public override void _Ready()
	{
		VisibleRatio = 0;
		plateName = GetNode<Data>("/root/Data").plateName;
		plateSize = GetNode<Data>("/root/Data").plateSize;
		ChangeText(GetNode<Node2D>("../SubViewportContainer/SubViewport/MapGenerator").Scale.X);
	}

    public override void _Process(double delta)
    {
		if (VisibleRatio < 1)
		{
			VisibleRatio += Mathf.Lerp(0f, 1f, 1f * (float)delta);
		}
    }

    public void ChangeText(float value)
	{
		cellHeight = (int)GetNode<Data>("/root/Data").informationMaps[cellPosition].X;
		cellHumidity = (int)GetNode<Data>("/root/Data").informationMaps[cellPosition].Y;
		cellTemperature = (int)GetNode<Data>("/root/Data").informationMaps[cellPosition].Z;
		cellMaterial = GetNode<Data>("/root/Data").environmentMap[cellPosition];

		VisibleRatio = 0;
		Clear();
		AddText($"板块名称: {plateName}");
		AddText($"\n板块大小: {plateSize}");
		AddText($"\n缩放比: {GetNode<Node2D>("../SubViewportContainer/SubViewport/MapGenerator").Scale.X}");
		AddText($"\n\ncell坐标: {cellPosition}");
		AddText($"\ncell高度: {cellHeight}");
		AddText($"\ncell湿度: {cellHumidity}");
		AddText($"\ncell温度: {cellTemperature}");
		AddText($"\ncell环境: {cellMaterial}");
	}

	public void PlaySoundEffect()
	{
		GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
	}
}
