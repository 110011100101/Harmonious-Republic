using Godot;
using System;

public partial class QuirGameButton : Button
{
	public override void _Pressed()
	{
		GetTree().Quit();
	}
}
