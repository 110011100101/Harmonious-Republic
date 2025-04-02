using Godot;
using RoseIsland.CustomClass;
using System;

public partial class Data : Node
{
    public string plateName {get; set;}
    public int plateSize {get; set;}
    public int subdivisionFactor {get; set;}
    public Vector2I startLocation {get; set;}
    public Matrix<int> heightMap {get; set;}
    public Matrix<int> detailedHeightMap {get; set;}
    public Matrix<float> humidityMap {get; set;}
    public Matrix<float> temperatureMap {get; set;}
    public Matrix<int> environmentMap {get; set;}
    public Matrix<int> detailedEnvironmentMap {get; set;}
}
