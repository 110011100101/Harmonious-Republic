using Godot;
using System;

public partial class PlateNameLable : Label
{
	public void ChangeText(string text)
	{
		if (text == "")
		{
			Text = "板块名称";
		}
		else
		{
			Text = text;
			GetNode<Data>("/root/Data").plateName = text;
		}
	}
}
