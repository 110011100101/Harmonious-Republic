using Godot;
using Godot.Collections;

public partial class Data : Node
{
    public string plateName {get; set;}
    public int plateSize {get; set;}
    public int subdivisionFactor {get; set;}
    public Vector2I startLocation {get; set;}
    public Dictionary<Vector2I, Vector3> informationMaps {get; set;}
    public Dictionary<Vector2I, EnumMaterial> environmentMap {get; set;}
}
