using Godot;
using System;

public partial class SetUpSubdivisionFactor : SpinBox
{
    public void _UpdateSetUpSubdivisionFactor(float value)
    {
        GetNode<Data>("/root/Data").subdivisionFactor = (int)value;
    }

    public override void _Ready()
    {
        GetNode<Data>("/root/Data").subdivisionFactor = (int)Value;
    }
}
