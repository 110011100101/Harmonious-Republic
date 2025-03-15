using System.Threading.Tasks;
using Godot;
using Godot.Collections;

namespace MapGeneration
{
    public abstract partial class AbstractMapGenerator : Node2D, IMapGenerator
    {
        [Export] private TileSet tileSet;
        private Data data;

        public override void _Ready()
        {
            data = GetNode<Data>("/root/Data");

            Main();
        }

        public void Main()
        {
            TileMapLayer map = AddMapIntoTree();
            InitizedMap(map, tileSet);
            Array<Vector2I> cells = GenerateBaseMap(data.plateSize);
            Dictionary<Vector2I, int> heightMap = GenerateHeightMap(cells);
            
            UpdateMapFromHeightMap(heightMap, map, data.plateSize);
        }

        public abstract TileMapLayer AddMapIntoTree();
        public abstract void InitizedMap(TileMapLayer map, TileSet tileSet);
        public abstract Array<Vector2I> GenerateBaseMap(int mapSize);          
        public abstract Dictionary<Vector2I, int> GenerateHeightMap(Array<Vector2I> cells);
        public abstract void UpdateMapFromHeightMap(Dictionary<Vector2I, int> heightMap, TileMapLayer map, int mapSize);
    }
}