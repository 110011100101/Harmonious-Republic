using Godot;
using System;

public partial class SetUpPlateSize : SpinBox
{
    public void _UpdateMapSize(float value)
    {
        GetNode<Data>("/root/Data").plateSize = (int)value;
    }

    public override void _Ready()
    {
        GetNode<Data>("/root/Data").plateSize = (int)Value;
    }
}
