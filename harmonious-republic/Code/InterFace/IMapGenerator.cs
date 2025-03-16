using Godot;
using RoseIsland.CustomClass;

namespace MapGeneration
{
    public interface IMapGenerator : 
        IHeightMapGenerator, 
        IMapInitialization, 
        IAddMapIntoTree, 
        IHumidityMapGenerator,
        ITemperatureMapGenerator,
        IEnvironmentMapGenerator
    {
        void RenderMap(Matrix<int> environmentMap, TileMapLayer map, int mapSize);
    }

    public interface IAddMapIntoTree
    {
        // return map
        TileMapLayer AddMapIntoTree();
    }

    public interface IMapInitialization
    {
        void InitizedMap(TileMapLayer map, TileSet tileSet);
    }

    public interface IHeightMapGenerator
    {
        /// <summary>
        /// return a matrix of cell's height.
        /// </summary>
        /// <param name="cells"></param>
        /// <returns></returns>
        Matrix<int> GenerateHeightMap(int plateSize);
    }

    public interface IHumidityMapGenerator
    {
        Matrix<float> GenerateHumidityMap(Matrix<int> heightMap);
    }

    public interface ITemperatureMapGenerator
    {
        Matrix<float> GenerateTemperatureMap(Matrix<int> heightMap, Matrix<float> humidityMap);
    }

    public interface IEnvironmentMapGenerator
    {
        Matrix<int> GenerateEnvironmentMap(Matrix<int> heightMap, Matrix<float> humidityMap, Matrix<float> temperatureMap);
    }
}