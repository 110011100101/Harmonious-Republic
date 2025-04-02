using Godot;

public partial class Test : Node
{
	public Data data;
	public override void _Ready()
	{
		data = GetNode<Data>("/root/Data");
		GD.Print(data.plateName);
		GD.Print(data.plateSize);
		GD.Print(data.startLocation);
		GD.Print(data.heightMap);
		GD.Print(data.humidityMap);
		GD.Print(data.temperatureMap);
		GD.Print(data.environmentMap);
	}

	public override void _Process(double delta)
	{
	}
}
