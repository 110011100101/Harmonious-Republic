using Godot;
using System;

public partial class KnboBar : ProgressBar
{
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}

	public void UpdateValue(float value)
	{
		Value = value;
	}
}
