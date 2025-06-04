using Godot;
using System;

[GlobalClass, Icon("res://Code/CutomNode/Button/QuirGame.svg")]
public partial class QuirGameButton : Button
{
	public override void _Pressed()
	{
		GetTree().Quit();
	}
}
