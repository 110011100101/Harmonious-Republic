using Godot;
using System;

public partial class SetUpBlockNumber : SpinBox
{
    public void _UpdateMapSize(float value)
    {
        GetNode<Data>("/root/Data").mapSize = (int)value;
		GD.Print(value);
    }
}
