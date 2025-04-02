using System.Threading.Tasks;
using Godot;
using LoadGame;
using RoseIsland.CustomClass;

public abstract partial class AbstracLoadGame : Node2D, ILoadGame
{
    Data data;

	public override void _Ready()
	{
        data = GetNode<Data>("/root/Data");
        LoadGame();
	}

    public void LoadGame()
    {
        Matrix<int> detailedHeightMap = DetailedHeightMap(data.heightMap, data.plateSize, data.subdivisionFactor);
        Matrix<int> detailedEnvironmentMap = DetailedEnvironmentMap(data.heightMap, detailedHeightMap, data.humidityMap, data.temperatureMap, data.environmentMap);
        UpdateData(detailedHeightMap, detailedEnvironmentMap);
    }

    public abstract Matrix<int> DetailedHeightMap(Matrix<int> hightMap, int plateSize, int subdivisionFactor);
    public abstract Matrix<int> DetailedEnvironmentMap(Matrix<int> heightMap, Matrix<int> detailedHeightMap, Matrix<float> humidityMap, Matrix<float> temperatureMap, Matrix<int> environmentMap);

    public void UpdateData(Matrix<int> detailedHeightMap, Matrix<int> detailedEnvironmentMap)
    {
        data.detailedHeightMap = detailedHeightMap;
        data.detailedEnvironmentMap = detailedEnvironmentMap;
    }
}   
