using Godot;
using System;

public partial class SetUpBlockNumber : SpinBox
{
    public void _UpdateMapSize(float value)
    {
        GetNode<Data>("/root/Data").mapSize = (int)value;
    }

    public override void _Ready()
    {
        GetNode<Data>("/root/Data").mapSize = (int)Value;
    }
}
