using System.Threading.Tasks;
using Godot;
using Godot.Collections;

namespace MapGeneration
{
    public interface IMapGenerator : IHeightMapGenerator, IBaseMapGenerator, IUpdateMapFromHeightMap, IMapInitialization, IAddMapIntoTree;

    public interface IAddMapIntoTree
    {
        // return map
        TileMapLayer AddMapIntoTree();
    }

    public interface IMapInitialization
    {
        void InitizedMap(TileMapLayer map, TileSet tileSet);
    }

    public interface IBaseMapGenerator
    {
        // return a array of cell's position.
        Array<Vector2I> GenerateBaseMap(int mapSize);
    }

    public interface IHeightMapGenerator
    {
        // return a dictionary of cell's height.
        Dictionary<Vector2I, int> GenerateHeightMap(Array<Vector2I> cells);
    }

    public interface IUpdateMapFromHeightMap
    {
        void UpdateMapFromHeightMap(Dictionary<Vector2I, int> heightMap, TileMapLayer map, int mapSize);
    }
}