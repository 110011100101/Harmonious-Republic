using Godot;
using HarmoniousRepublic.Code.Class;

namespace HarmoniousRepublic.Code.CustomNode;

public partial class Data : Node
{
    public string plateName { get; set; }
    public int plateSize { get; set; } = 100;
    public Vector2I startLocation { get; set; }
    public Block[,,] gameMap { get; set; }
    public Block[,] environmentMap { get; set; }
    public bool isLoadingState { get; set; }
}