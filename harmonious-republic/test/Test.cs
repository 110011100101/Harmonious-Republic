<<<<<<< HEAD
using Godot;

public partial class Test : Node2D
{

    public override void _Ready()
    {
        ShaderMaterial shaderMaterial = (ShaderMaterial)GetNode<TileMapLayer>("TileMapLayer0").Material;
        shaderMaterial.SetShaderParameter("level", 1.0f);
    }

    public override void _Process(double delta)
    {

    }
}
=======
>>>>>>> 8ca785b4bd2b34da06a7055c415504461de41885
