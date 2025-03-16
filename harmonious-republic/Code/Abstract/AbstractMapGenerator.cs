using System.Threading.Tasks;
using Godot;
using Godot.Collections;
using RoseIsland.CustomClass;

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
            Matrix<int> heightMap = GenerateHeightMap(data.plateSize);
            Matrix<float> humidityMap = GenerateHumidityMap(heightMap);
            Matrix<float> temperatureMap = GenerateTemperatureMap(heightMap, humidityMap);
            Matrix<int> environmentMap = GenerateEnvironmentMap(heightMap, humidityMap, temperatureMap);
            RenderMap(environmentMap, map, data.plateSize);
        }

        // Method form interface
        public abstract TileMapLayer AddMapIntoTree();
        public abstract void InitizedMap(TileMapLayer map, TileSet tileSet);
        public abstract Matrix<int> GenerateHeightMap(int plateSize);
        public abstract Matrix<float> GenerateHumidityMap(Matrix<int> heightMap);
        public abstract Matrix<float> GenerateTemperatureMap(Matrix<int> heightMap, Matrix<float> humidityMap);
        public abstract Matrix<int> GenerateEnvironmentMap(Matrix<int> heightMap, Matrix<float> humidityMap, Matrix<float> temperatureMap);
        public abstract void RenderMap(Matrix<int> environmentMap, TileMapLayer map, int mapSize);

    }
}