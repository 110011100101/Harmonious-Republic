using RoseIsland.CustomClass;

namespace LoadGame
{
    public interface ILoadGame:
        IDetailedHeightMap,
        IDetailedEnvironmentMap;

    public interface IDetailedHeightMap
    {
        Matrix<int> DetailedHeightMap(Matrix<int> hightMap, int mapSize, int subdivisionFactor);
    }

    public interface IDetailedEnvironmentMap
    {
        Matrix<int> DetailedEnvironmentMap(Matrix<int> heightMap, Matrix<int> detailedHeightMap, Matrix<float> humidityMap, Matrix<float> temperatureMap, Matrix<int> environmentMap);
    } 
}
