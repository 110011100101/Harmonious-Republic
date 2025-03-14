using Godot;
using System;

public partial class InformationPad : RichTextLabel
{
	[Export] public string plateName {get; set;}
    [Export] public int plateSize {get; set;}
    // [Export] public string ;
    // [Export] public string ;
    // [Export] public string ;
    // [Export] public string ;
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
			VisibleRatio = Mathf.Lerp(VisibleRatio, 1.1f, 0.5f * (float)delta);
		}
    }

    public void ChangeText(float value)
	{
		VisibleRatio = 0;
		Clear();
		AddText($"板块名称:{plateName}");
		AddText($"\n板块大小:{plateSize}");
		AddText($"\n缩放比:{GetNode<Node2D>("../SubViewportContainer/SubViewport/MapGenerator").Scale.X}");
	}

	public void PlaySoundEffect()
	{
		GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
	}
}
